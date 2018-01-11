using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyReport
{
    class InvoiceTotal
    {
        public int Number { get; set; }
        public decimal Total { get; set; }
        public InvoiceTotal(int number, decimal total)
        {
            Number = number;
            Total = total;
        }
    }
}
