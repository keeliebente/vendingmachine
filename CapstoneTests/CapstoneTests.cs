using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineTest
    {
        UserInterface userInterface = new UserInterface();
        VendingMachine vendingMachine = new VendingMachine();
        VendingMachineItem item = new VendingMachineItem();

        [TestMethod()]
        public void FeedMoney1Test()
        {
            vendingMachine.FeedMoney(5.00M);
            decimal result = vendingMachine.CurrentMoney;
            Assert.AreEqual(5.00M, result);

            vendingMachine.FeedMoney(10.00M);
            result = vendingMachine.CurrentMoney;
            Assert.AreEqual(15.00M, result);

            vendingMachine.FeedMoney(12.00M);
            result = vendingMachine.CurrentMoney;
            Assert.AreEqual(15.00M, result);
        }

        [TestMethod()]
        public void PurchaseSuccessfulTest()
        {
            vendingMachine.ReadFile();
            vendingMachine.FeedMoney(50.00M);
            int result = vendingMachine.ProductSelection("A1");
            Assert.AreEqual(4, result);
        }

        [TestMethod()]
        public void SoldOutTest()
        {
            vendingMachine.ReadFile();
            vendingMachine.FeedMoney(50.00M);
            int result = vendingMachine.ProductSelection("A1");
            result = vendingMachine.ProductSelection("A1");
            result = vendingMachine.ProductSelection("A1");
            result = vendingMachine.ProductSelection("A1");
            result = vendingMachine.ProductSelection("A1");
            result = vendingMachine.ProductSelection("A1");
            Assert.AreEqual(2, result);
        }

        [TestMethod()]
        public void NotEnoughMoneyTest()
        {
            vendingMachine.ReadFile();
            vendingMachine.FeedMoney(2.00M);
            int result = vendingMachine.ProductSelection("A1");
            Assert.AreEqual(3, result);
        }

        [TestMethod()]
        public void InvalidItemTest()
        {
            vendingMachine.ReadFile();
            vendingMachine.FeedMoney(2.00M);
            int result = vendingMachine.ProductSelection("A6");
            Assert.AreEqual(1, result);
        }

        [TestMethod()]
        public void ConsumeMessage()
        {
            vendingMachine.ReadFile();
            vendingMachine.FeedMoney(50.00M);
            int result = vendingMachine.ConsumedMessage("A3");
            Assert.AreEqual(1, result);

            result = vendingMachine.ConsumedMessage("C2");
            Assert.AreEqual(3, result);

            result = vendingMachine.ConsumedMessage("B2");
            Assert.AreEqual(2, result);

            result = vendingMachine.ConsumedMessage("D3");
            Assert.AreEqual(4, result);
        }

        [TestMethod()]
        public void ChangeTest()
        {
            vendingMachine.FeedMoney(5.00M);
            string result = vendingMachine.ReturnChange();
            Assert.AreEqual("Change returned: 20 quarter(s), 0 dime(s), and 0 nickel(s)", result);
        }

        [TestMethod()]
        public void ChangeTest2()
        {
            vendingMachine.ReadFile();
            vendingMachine.FeedMoney(5.00M);
            vendingMachine.ProductSelection("A1");
            string result = vendingMachine.ReturnChange();
            Assert.AreEqual("Change returned: 7 quarter(s), 2 dime(s), and 0 nickel(s)", result);
        }
    }
}
