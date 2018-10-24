using System;
using System.Collections.Generic;
using System.IO;

namespace Capstone.Classes
{
    public class LogFile
    {
        public DateTime TransactionDateTime {get; set;}
        public string TransactionType { get; set; }
        public decimal dollarAmount { get; set; }
        public decimal endingDollarAmount { get; set; }
    }
}
