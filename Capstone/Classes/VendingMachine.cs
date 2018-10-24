using System;
using System.Collections.Generic;
using System.IO;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        VendingMachineItem vendingMachineItem = new VendingMachineItem();

        //keeps track of how much money is currently in the vending machine
        public decimal CurrentMoney { get; set; }

        private List<VendingMachineItem> items = new List<VendingMachineItem>();

        private List<LogFile> logFiles = new List<LogFile>();

        private string filePath = @"C:\VendingMachine";
        private string fileName = "vendingmachine.csv";
        private string outputFileName = "Log.txt";
        private string salesReportFileName = "Sales_Report_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt";

        public bool ReadFile()
        {
            bool result = true;

            try
            {
                string path = Path.Combine(filePath, fileName);
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string nextString = sr.ReadLine();
                        string[] splitString = nextString.Split('|');

                        VendingMachineItem vendingMachineItem = new VendingMachineItem();
                        vendingMachineItem.SlotLoc = splitString[0];
                        vendingMachineItem.Item = splitString[1];
                        vendingMachineItem.Price = decimal.Parse(splitString[2]);
                        vendingMachineItem.Qty = 5;
                        items.Add(vendingMachineItem);
                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }


        public VendingMachineItem[] List()
        {
            return items.ToArray();
        }

        //if a valid dollar amount was fed to the machine, the entry from the user updates the current amount in the machine
        public bool FeedMoney(decimal entry)
        {
            List<decimal> validDollars = new List<decimal> { 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M };
            AddMoneyLog(entry);

            if (validDollars.Contains(entry))
            {
                CurrentMoney += entry;
                return true;
            }
            else
            {
                return false;
            }
        }

        //checks if the user's selection is valid/available and if they have enough money
        //the resulting string determines what messages are output to the user 
        public int ProductSelection(string userInput)
        {
            int returnInt = 1;

            for (int i = 0; i < items.Count; i++)
            {
                if (userInput == items[i].SlotLoc)
                {
                    if (CurrentMoney - items[i].Price >= 0)
                    {
                        if (items[i].Qty > 0)
                        {
                            decimal price = items[i].Price;
                            items[i].Qty--;
                            CurrentMoney -= price;

                            string purchaseMessage = $"{userInput} - {items[i].Item}";
                            returnInt = 4;

                            ConsumedMessage(userInput);
                            PurchaseItemLog(purchaseMessage, price);
                        }
                        else
                        {
                            returnInt = 2;
                        }
                    }
                    else
                    {
                        returnInt = 3;
                    }
                }
            }

            return returnInt;
        }

        public int ConsumedMessage(string userInput)
        {
            int returnInt = 0;
            string selection = userInput.Substring(0, 1);

            switch (selection)
            {
                case "A":
                    returnInt = 1;
                    break;

                case "B":
                    returnInt = 2;
                    break;

                case "C":
                    returnInt = 3;
                    break;

                case "D":
                    returnInt = 4;
                    break;
            }

            return returnInt;
        }

        //determines the type of change that is returned to the user 
        public string ReturnChange()
        {
            ChangeReturnLog();
            int quarterCount = 0;
            int dimeCount = 0;
            int nickelCount = 0;

            while (CurrentMoney != 0.00M)
            {
                if (CurrentMoney >= 0.25M)
                {
                    CurrentMoney -= 0.25M;
                    quarterCount++;
                }
                else if (CurrentMoney >= 0.10M)
                {
                    CurrentMoney -= 0.10M;
                    dimeCount++;
                }
                else if (CurrentMoney >= 0.05M)
                {
                    CurrentMoney -= 0.05M;
                    nickelCount++;
                }
            }
            string message = $"Change returned: {quarterCount} quarter(s), {dimeCount} dime(s), and {nickelCount} nickel(s)";
            return message;
        }
        //creates a new line in the log with the info from returning change
        public void ChangeReturnLog()
        {
            LogFile newItem = new LogFile();
            newItem.TransactionDateTime = DateTime.Now;
            newItem.TransactionType = "Return Change";
            newItem.dollarAmount = Math.Round(CurrentMoney * 1.00m, 2, MidpointRounding.AwayFromZero);
            newItem.endingDollarAmount = 0.00M;
            logFiles.Add(newItem);
        }
        //creates a new line in the log with the info from adding money
        public void AddMoneyLog(decimal entry)
        {
            LogFile newItem = new LogFile();
            newItem.TransactionDateTime = DateTime.Now;
            newItem.TransactionType = "Feed Money";
            newItem.dollarAmount = Math.Round(entry * 1.00m, 2, MidpointRounding.AwayFromZero);
            newItem.endingDollarAmount = Math.Round((CurrentMoney + entry) * 1.00m, 2, MidpointRounding.AwayFromZero);
            logFiles.Add(newItem);
        }
        //creates a line in the action log each time a user selects a product
        public void PurchaseItemLog(string purchaseMessage, decimal price)
        {
            LogFile newItem = new LogFile();

            newItem.TransactionDateTime = DateTime.Now;
            newItem.TransactionType = purchaseMessage;
            newItem.dollarAmount = Math.Round(price * 1.00m, 2, MidpointRounding.AwayFromZero);
            newItem.endingDollarAmount = Math.Round(CurrentMoney * 1.00m, 2, MidpointRounding.AwayFromZero);
            logFiles.Add(newItem);
        }

        public bool WriteFile()
        {
            bool result = true;

            try
            {
                string outputPath = Path.Combine(filePath, outputFileName);
                using (StreamWriter sw = new StreamWriter(outputPath, false))
                {
                    sw.WriteLine("Date/Time".PadRight(25) + "Transaction".PadRight(25) + "$ +/-".PadRight(10) + "Ending Amount");
                    sw.WriteLine("--------------------------------------------------------------------------");
                    foreach (LogFile files in logFiles)
                    {
                        string outputString = files.TransactionDateTime.ToString().PadRight(25) + files.TransactionType.ToString().PadRight(25) + "$" + files.dollarAmount.ToString().PadRight(10) + "$" + files.endingDollarAmount;
                        sw.WriteLine(outputString);
                    }
                }

            }
            catch
            {
                result = false;
            }

            return result;
        }

        public bool WriteSalesReport()
        {
            bool result = true;
            decimal totalSales = 0.00M;
            Dictionary<string, int> salesReport = new Dictionary<string, int>();

            foreach (LogFile item in logFiles)
            {
                if (item.TransactionType != "Feed Money" && item.TransactionType != "Return Change")
                {
                    if (!salesReport.ContainsKey(item.TransactionType))
                    {
                        salesReport.Add(item.TransactionType, 1);
                        totalSales += item.dollarAmount;
                    }
                    else
                    {
                        salesReport[item.TransactionType]++;
                    }
                }
            }

            try
            {
                string outputPath = Path.Combine(filePath, salesReportFileName);
                using (StreamWriter sw = new StreamWriter(outputPath, false))
                {
                    sw.WriteLine("Product|Purchased Count");
                    foreach (KeyValuePair<string, int> kvp in salesReport)
                    {
                        string outputString = kvp.Key.Substring(5) + "|" + kvp.Value;
                        sw.WriteLine(outputString);

                    }
                    sw.WriteLine();
                    sw.WriteLine("** TOTAL SALES **  " + "$" + Math.Round(totalSales * 1.00m, 2, MidpointRounding.AwayFromZero));
                }

            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}

