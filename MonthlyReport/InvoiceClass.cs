using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyReport
{
    class Invoice
    {
        private DateTime date;
        private int number;
        private string name;
        private List<Items> itemList;
        private decimal total;
        private decimal tax;
        public Invoice(DateTime date, int number, string name, List <Items> itemList, decimal total, decimal tax)
        {
            this.date = date;
            this.number = number;
            this.name = name;
            this.itemList = itemList;
            this.total = total;
            this.tax = tax;
        }

        public DateTime getDate()
        {
            return date;
        }

        public void setDate(DateTime newDate)
        {
            this.date = newDate;
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

        public decimal getTax()
        {
            return tax;
        }

        public void setTax(decimal newTax)
        {
            this.tax = newTax;
        }
    }

}
