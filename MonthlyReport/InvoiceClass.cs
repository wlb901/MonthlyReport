using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyReport
{
    class Invoice
    {
        private int number;
        private string name;
        private List<Items> itemList;
        private decimal total;
        public Invoice(int number, string name, List <Items> itemList, decimal total)
        {
            this.number = number;
            this.name = name;
            this.itemList = itemList;
            this.total = total;
        }

        public int getNumber()
        {
            return number;
        }

        public void setNumber(int newNumber)
        {
            this.number = newNumber;
        }

        public string getName()
        {
            return name;
        }

        public void setName(string newName)
        {
            this.name = newName;
        }

        public List<Items> getItemList()
        {
            return itemList;
        }
        
        public void setItemList(List<Items> newItemList)
        {
            this.itemList = newItemList;
        }

        public decimal getTotal()
        {
            return total;
        }

        public void setTotal(decimal newTotal)
        {
            this.total = newTotal;
        }
    }

}
