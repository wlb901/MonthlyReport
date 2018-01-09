using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonthlyReport
{
    public partial class Form1 : Form
    {
        //static List<InvoiceClass> invoiceList = new List<InvoiceClass>();
        OpenFileDialog ofd = new OpenFileDialog();
        string invoicesFile;
        string totalsFile;

        public Form1()
        {
            InitializeComponent();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            //Open Help.txt
            try
            {
                System.Diagnostics.Process.Start("Help.txt");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void InvoicesButton_Click(object sender, EventArgs e)
        {
            //select Invoice File
            ofd.Filter = "CSV|*csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                invoicesFile = ofd.FileName;
                InvoicesTextBox.Text = ofd.SafeFileName;
            }
        }

        private void TotalsButton_Click(object sender, EventArgs e)
        {
            //select Totals File
            ofd.Filter = "CSV|*csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                totalsFile = ofd.FileName;
                TotalsTextBox.Text = ofd.SafeFileName;
            }
        }

        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            /*This will be the execute button. It will do four things: 
             * 1. Select a location to save the final report (.CSV)
             * 2. Read in the files for Invoices and Totals
             * 3. Manipulate data to fit the final report
             * 4. Generate a .CSV file with the final report */


            //1. Save location

            //2. Read files
            List <InvoiceClass> invoiceList0 = ReadInvoiceFile(invoicesFile);
            List<InvoiceTotal> totalsList = ReadTotalsFile(totalsFile);

            for(int i = 0; i < invoiceList0.Count(); i++)
            {
                Console.Write(invoiceList0[i].InvoiceNumber + " " + invoiceList0[i].InvoiceName + " ");
                for(int j = 0; j < invoiceList0[i].InvoiceItemArray.Count(); j++)
                {
                    Console.Write(invoiceList0[i].InvoiceItemArray[j].ItemName + " " + invoiceList0[i].InvoiceItemArray[j].ItemPrice + " ");
                }
                Console.WriteLine(invoiceList0[i].InvoiceTotal);
            }

            //3. Maniputlate data

            //4. Generate output file
        }

        //Function to read invoiceFile to a list of objects
        static List<InvoiceClass> ReadInvoiceFile(string invoiceFile)
        {
            string line;
            string prevInvoiceNumber = "0";
            List<InvoiceClass> invoiceList = new List<InvoiceClass>();
            List<ItemsClass> itemsList = new List<ItemsClass>();
            var format = new NumberFormatInfo();
            format.NegativeSign = "-";
            int i = -1;

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(invoiceFile);

                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(';');

                    if (words[1] != "")
                    {
                        InvoiceClass invoice = new InvoiceClass(0, "", itemsList, 0);
                        if (words[0] != prevInvoiceNumber)
                        {
                            List<ItemsClass> items = new List<ItemsClass>();
                            items.Add(new ItemsClass(words[2], double.Parse(words[3])));
                            invoiceList.Add(new InvoiceClass(int.Parse(words[0]), words[1], items, 0));
                            i++;
                            prevInvoiceNumber = words[0];
                        }
                        else
                        {
                            invoiceList[i].InvoiceItemArray.Add(new ItemsClass(words[2], double.Parse(words[3], format)));
                            prevInvoiceNumber = words[0];
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return invoiceList;
        }

        //Function to read totalsFile to a list of objects
        static List<InvoiceTotal> ReadTotalsFile(string totalsFile)
        {
            string line;
            List<InvoiceTotal> totalsList = new List<InvoiceTotal>();

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(totalsFile);

                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    totalsList.Add(new InvoiceTotal(words[0], words[1]));
                }
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return totalsList;

        }
    }
}
