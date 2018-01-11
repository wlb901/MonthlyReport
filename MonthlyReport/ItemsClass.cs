using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyReport
{
    class ItemsClass
    {
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public ItemsClass(string itemName, decimal itemPrice)
        {
            ItemName = itemName;
            ItemPrice = itemPrice;
        }
    }
}
