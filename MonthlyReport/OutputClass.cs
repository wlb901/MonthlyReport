using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyReport
{
    class OutputClass
    {
        public DateTime Date { get; set; }
        public int OutputNumber { get; set; }
        public string OutputName { get; set; }
        public decimal TaxSale { get; set; }
        public decimal Wholesale { get; set; }
        public decimal FET { get; set; }
        public decimal Disposal { get; set; }
        public decimal Labor { get; set; }
        public decimal Scrap { get; set; }
        public decimal Casing { get; set; }
        public decimal TIS { get; set; }
        public decimal Charged { get; set; }
        public OutputClass(DateTime date, int outputNumber, string outputName, decimal taxSale, decimal wholesale, 
            decimal fet, decimal disposal, decimal labor, decimal scrap, decimal casing, decimal tis, decimal charged)
        {
            Date = date;
            OutputNumber = outputNumber;
            OutputName = outputName;
            TaxSale = taxSale;
            Wholesale = wholesale;
            FET = fet;
            Disposal = disposal;
            Labor = labor;
            Scrap = scrap;
            Casing = casing;
            TIS = tis;
            Charged = charged;
        }
    }
}
