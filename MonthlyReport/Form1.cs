using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MonthlyReport
{

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
    public partial class Form1 : Form
    {
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
            /*This will be the execute button. It will do three things: 
             * 1. Read in the files for Invoices and Totals
             * 2. Manipulate data to fit the final report
             *       Combine totals from the totals list and the list of invoices. Then put all data in the proper columns.
             * 3. Generate a .CSV file with the final report */

            

            //1. Read files
            List<Invoice> invoiceList = ReadInvoiceFile(invoicesFile);
            List<InvoiceTotal> totalsList = ReadTotalsFile(totalsFile);
            



            //2. Maniputlate data
            List<OutputClass> outputList = new List<OutputClass>();

            try
            {
                for (int i = 0; i < invoiceList.Count(); i++)

                {
                    invoiceList[i].setTotal(totalsList[i].Total);
                }

                outputList = OutputList(invoiceList);



                //3. Generate output file
                GenerateOutput(outputList);
            }
            catch
            {
                MessageBox.Show("Both files must contain the same number of invoices. \nPlease check files and try again.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        static List<Invoice> ReadInvoiceFile(string invoiceFile)
        {
            string prevInvoiceNumber = "0";

            List<Invoice> invoiceList = new List<Invoice>();
            List<Items> itemsList = new List<Items>();

            
            var format = new NumberFormatInfo();
            format.NegativeSign = "-";


            /*
             * i is used to iterate through invoiceList. If an invoice number is the same
             * as the previous invoice number (prevInvoiceNumber), it does not increment. This creates in object
             * that includes all items with the same invoice number.
             * - invoice #12345 might include multiple items
             */
            int i = -1;



            try
            {

                /* 
                 * using TextFieldParser from Visual Basic to parse the CSV file
                 * I originally had a problem parsing company names that contained commas.
                 * This is the simplest method I found to ignore anything between double quotes.
                */            
                using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(invoiceFile))
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] words = parser.ReadFields();

                        Invoice invoice = new Invoice(0, "", itemsList, 0);


                        // Only continue if the first item in the words list does not contain any letters.
                        // This is to skip anything that doesn't start with an invoice number.
                        // Usually the first two lines and the last line of the file should be skipped.
                        if (!words[0].Any(Char.IsLetter))
                        {

                            if (words[0] != prevInvoiceNumber)
                            {

                                List<Items> items = new List<Items>();

                                // words[2] is the item name words[3] is the item price.
                                items.Add(new Items(words[2], decimal.Parse(words[3], format)));

     
                                // words[0] is the item number. words[1] is the invoice name
                                invoiceList.Add(new Invoice(int.Parse(words[0]), words[1], items, 0));

  
                                i++;

      
                                prevInvoiceNumber = words[0];
                            }
                            else
                            {
                           
                                // words[2] is the item name. words[3] is the item price.
                                invoiceList[i].getItemList().Add(new Items(words[2], decimal.Parse(words[3], format)));

                                prevInvoiceNumber = words[0];
                            }

                        }

                    }
                    
                    parser.Close();
                }
                    
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return invoiceList;
        } 



        static List<InvoiceTotal> ReadTotalsFile(string totalsFile)
        {
            List<InvoiceTotal> totalsList = new List<InvoiceTotal>();

            try
            {

                using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(totalsFile))
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] words = parser.ReadFields();
                        if (!words[0].Any(Char.IsLetter))
                        {
                            totalsList.Add(new InvoiceTotal(int.Parse(words[0]), decimal.Parse(words[1])));
                        }
                    }
                    parser.Close();
                   
                }
                
            }
             catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return totalsList;
        }

  

        static void GenerateOutput (List<OutputClass> outputList)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    using (StreamWriter sw = new StreamWriter(myStream))
                    {
                        sw.WriteLine("Invoice,Name,TaxSale,WholeSale,FET,Disp,Labor,Scrap,Casing,TIS");

                        for (int i = 0; i < outputList.Count(); i++)
                        {
                            sw.WriteLine(outputList[i].OutputNumber + ",\"" + outputList[i].OutputName + "\"," + outputList[i].TaxSale + ","
                                + outputList[i].Wholesale + "," + outputList[i].FET + "," + outputList[i].Disposal + "," + outputList[i].Labor +
                                "," + outputList[i].Scrap + "," + outputList[i].Casing + "," + outputList[i].TIS);
                        }
                    }

                    myStream.Close();
                }
            }
        }


        static List<OutputClass> OutputList (List<Invoice> invoiceList)
        {
            List<OutputClass> output = new List<OutputClass>();



            for (int i = 0; i < invoiceList.Count(); i++)
            {
                decimal tax = 0;
                decimal fet = 0;
                decimal disposal = 0;
                decimal labor = 0;
                decimal scrap = 0;
                decimal casing = 0;
                decimal total = 0;

                //total = sum of item prices -> to not include tax
                for (int j = 0; j < invoiceList[i].getItemList().Count(); j++)
                {
                    total += invoiceList[i].getItemList()[j].getPrice();
                }
                


                if (IsTaxSale(invoiceList[i]))
                {
                    for(int j = 0; j < invoiceList[i].getItemList().Count(); j++)
                    {
                        if(invoiceList[i].getItemList()[j].getName().Contains("F.E.T", StringComparison.OrdinalIgnoreCase))
                        {
                            fet += invoiceList[i].getItemList()[j].getPrice();
                        }
                        if(invoiceList[i].getItemList()[j].getName().Contains("DISPOSAL", StringComparison.OrdinalIgnoreCase))
                        {
                            disposal += invoiceList[i].getItemList()[j].getPrice();
                        }
                        if(invoiceList[i].getItemList()[j].getName().Contains("DISMOUNT", StringComparison.OrdinalIgnoreCase) ||
                            invoiceList[i].getItemList()[j].getName().Contains("REPAIR", StringComparison.OrdinalIgnoreCase))
                        {
                            labor += invoiceList[i].getItemList()[j].getPrice();
                        }
                        if(invoiceList[i].getItemList()[j].getName().Contains("SCRAP TIRE ENVIRONMENTAL FEE", StringComparison.OrdinalIgnoreCase))
                        {
                            scrap += invoiceList[i].getItemList()[j].getPrice();
                        }
                        if(invoiceList[i].getItemList()[j].getName().Contains("CASING", StringComparison.OrdinalIgnoreCase) ||
                           invoiceList[i].getItemList()[j].getName().Contains("ADJ", StringComparison.OrdinalIgnoreCase))
                        { 
                            casing += invoiceList[i].getItemList()[j].getPrice();
                        }
                        
                    }
                    tax = total - fet - disposal - labor - scrap - casing;
                    output.Add(new OutputClass(invoiceList[i].getNumber(), invoiceList[i].getName(), tax, 0, 
                        fet, disposal, labor, scrap, casing, total, total));
                }
                else
                {
                    output.Add(new OutputClass(invoiceList[i].getNumber(), invoiceList[i].getName(), 0, total,
                        0, 0, 0, 0, 0, total, total));
                }
            }
            return output;
        }


   
        static bool IsTaxSale (Invoice invoice)
        {
            decimal itemsTotal = 0;
            for(int i = 0; i < invoice.getItemList().Count(); i++)
            {
                itemsTotal += invoice.getItemList()[i].getPrice();
            }

            // If the items total = the invoice total, then it was not a tax sale.
            // The difference comes from the taxed amount.
            if(itemsTotal == invoice.getTotal())
            {
                // Not taxed - IsTaxSale = False
                return false;
            }
            else
            {
                // Taxed - IsTaxeSale = True
                return true;
            }
        }


    }
}
