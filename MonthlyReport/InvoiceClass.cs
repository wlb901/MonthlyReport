using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyReport
{
    class InvoiceClass
    {
        public int InvoiceNumber { get; set; }
        public string InvoiceName { get; set; }
        public List <ItemsClass> InvoiceItemArray { get; set; }
        public decimal InvoiceTotal { get; set; }
        public InvoiceClass(int invoiceNumber, string invoiceName, List <ItemsClass> invoiceItemArray, decimal invoiceTotal)
        {
            InvoiceNumber = invoiceNumber;
            InvoiceName = invoiceName;
            InvoiceItemArray = invoiceItemArray;
            InvoiceTotal = invoiceTotal;
        }
    }
}
