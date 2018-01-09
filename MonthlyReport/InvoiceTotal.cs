using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyReport
{
    class InvoiceTotal
    {
        public string Number { get; set; }
        public string Total { get; set; }
        public InvoiceTotal(string number, string total)
        {
            Number = number;
            Total = total;
        }
    }
}
