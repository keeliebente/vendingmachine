using System;
using System.Collections.Generic;
using System.IO;

namespace Capstone.Classes
{
    public class VendingMachineItem
    {
        public string SlotLoc { get; set; }
        public string Item { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }

        public override string ToString()
        {
            if (Qty == 0)
            {
                return SlotLoc.PadRight(7) + Item.PadRight(20) + "$" + Price.ToString().PadRight(10) + "SOLD OUT";
            }
            return SlotLoc.PadRight(7) + Item.PadRight(20) + "$" + Price.ToString().PadRight(10) + Qty;
        }
        //contains an option that writes "Sold out" in place of quantity if the quantity is 0
    }
}
