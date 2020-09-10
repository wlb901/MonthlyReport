using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyReport
{
    class Items
    {
        private string name;
        private decimal price;
        public Items(string itemName, decimal itemPrice)
        {
            this.name = itemName;
            this.price = itemPrice;
        }

        public string getName()
        {
            return name;
        }

        public decimal getPrice()
        {
            return price;
        }

        public void setPrice(decimal newPrice)
        {
            this.price = newPrice;
        }

        public void setName(string newName)
        {
            this.name = newName;
        }
    }
}
