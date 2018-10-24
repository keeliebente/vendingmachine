using System;

namespace Capstone.Classes
{
    public class UserInterface
    {
        private VendingMachine vendingMachine = new VendingMachine();

        public void RunInterface()
        {
            Console.WriteLine("Welcome to the Vendo-Matic 6000!");
            vendingMachine.ReadFile();
            bool done = false;
            while (!done)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Display Vending Machine Items - Press 1");
                Console.WriteLine("Purchase- Press 2");
                Console.WriteLine("End - Press 3");
                Console.WriteLine();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine();
                        Console.WriteLine("Slot".PadRight(7) +  "Item Name".PadRight(20) +  "Price".PadRight(10) +  "Qty");
                        Console.WriteLine("-----------------------------------------");
                        DisplayItemInformation();
                        break;

                    case "2":
                        SecondMenu();
                        break;

                    case "3":
                        vendingMachine.WriteFile();
                        vendingMachine.WriteSalesReport();
                        done = true;
                        break;

                    default:
                        Console.WriteLine("Invalid Selection");
                        break;
                }
            }
        }

        private void DisplayItemInformation()
        {
            VendingMachineItem[] result = vendingMachine.List();

            foreach (VendingMachineItem item in result)
            {
                Console.WriteLine(item.ToString());


            }
        }

        private void SecondMenu()
        {
            bool done = false;
            while (!done)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Feed Money - Press 1");
                Console.WriteLine("Select Product- Press 2");
                Console.WriteLine("Finish Transaction - Press 3");
                Console.WriteLine($"Current Money Provided: ${vendingMachine.CurrentMoney}");
                Console.WriteLine();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddMoney();
                        break;

                    case "2":
                        SelectProduct();
                        break;

                    case "3":
                        ReturnChange();
                        done = true;
                        break;

                    default:
                        Console.WriteLine("Invalid Selection");
                        break;
                }
            }
        }

        public void AddMoney()
        {
            Console.WriteLine("Please enter a dollar amount to feed");
            decimal moneyFed = decimal.Parse(Console.ReadLine());
            bool successful = vendingMachine.FeedMoney(moneyFed);

            if (successful)
            {
                Console.WriteLine();
                Console.WriteLine("Money Accepted");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Dollar amount invalid");
            }

        }

        public void SelectProduct()
        {
            Console.WriteLine("Enter item slot code");
            string userSelection = Console.ReadLine().ToUpper();
            int result = vendingMachine.ProductSelection(userSelection);

            switch (result)
            {
                case 1:
                    Console.WriteLine("INVALID SELECTION");
                    break;

                case 2:
                    Console.WriteLine("SOLD OUT");
                    break;

                case 3:
                    Console.WriteLine("NOT ENOUGH MONEY");
                    break;

                case 4:
                    Console.WriteLine("PURCHASE SUCCESSFUL");
                    break;
            }

            //if the item is not sold out, if the vending machine has enough money, and the selection is valid
            //go to the method in vending machine that determines what should be written to the console
            //based on the item purchased 

            if (result > 3)
            {
                ConsumeMessage(userSelection);
            }
        }

        public void ConsumeMessage(string userSelection)
        {
            int result = vendingMachine.ConsumedMessage(userSelection);

            switch (result)
            {
                case 1:
                    Console.WriteLine("Crunch Crunch, Yum!");
                    break;

                case 2:
                    Console.WriteLine("Munch Munch, Yum!");
                    break;

                case 3:
                    Console.WriteLine("Glug Glug, Yum!");
                    break;

                case 4:
                    Console.WriteLine("Chew Chew, Yum!");
                    break;
            }
        }

        public void ReturnChange()
        {
            Console.WriteLine(vendingMachine.ReturnChange());
        }
    }
}
