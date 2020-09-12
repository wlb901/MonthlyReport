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
        string taxFile;

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

        
        private void TaxButton_Click(object sender, EventArgs e)
        {
            //select Totals File
            ofd.Filter = "CSV|*csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                taxFile = ofd.FileName;
                TaxTextBox.Text = ofd.SafeFileName;
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
            List<Invoice> invoiceList = ReadInvoiceFile(invoicesFile, taxFile);
            //List<InvoiceTotal> totalsList = ReadTotalsFile(totalsFile);
            



            //2. Maniputlate data
            List<OutputClass> outputList = new List<OutputClass>();

            try
            {
                for (int i = 0; i < invoiceList.Count(); i++)
                {
                    decimal total = 0;
                    //invoiceList[i].setTotal(totalsList[i].Total);
                    for (int j = 0; j < invoiceList[i].getItemList().Count(); j++)
                    {
                        total += invoiceList[i].getItemList()[j].getPrice();
                    }
                    invoiceList[i].setTotal(total);
                }

                Console.WriteLine(invoiceList[0].getNumber());
                outputList = OutputList(invoiceList);



                //3. Generate output file
                GenerateOutput(outputList);
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Both files must contain the same number of invoices. \nPlease check files and try again.", "Error",
               MessageBox.Show(ex.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        struct InvoiceTax
        {
            public int number;
            public decimal tax;
        }

        static List<InvoiceTax> ReadTaxFile(string taxFile)
        {
            List<InvoiceTax> taxList = new List<InvoiceTax>();

            try
            {
                using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(taxFile))
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] words = parser.ReadFields();
                        if (!words[0].Any(Char.IsLetter))
                        {
                            InvoiceTax temp;
                            temp.number = int.Parse(words[0]);
                            temp.tax = decimal.Parse(words[1]);
                            Console.WriteLine("number: " + temp.number);
                            //Console.WriteLine("tax: " + temp.tax);
                            taxList.Add(temp);
                        }
                    }
                    parser.Close();

                }
                
            }
             catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return taxList;
        }


        static List<Invoice> ReadInvoiceFile(string invoiceFile, string taxFile)
        {
            string prevInvoiceNumber = "0";

            List<Invoice> invoiceList = new List<Invoice>();
            List<Items> itemsList = new List<Items>();

            List<InvoiceTax> taxList = ReadTaxFile(taxFile);
            
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

                    int taxIndex = 0;

                    while (!parser.EndOfData)
                    {
                        string[] words = parser.ReadFields();

                        DateTime date = new DateTime();

                        // Date, number, name, itemsList, total, tax
                        Invoice invoice = new Invoice(date, 0, "", itemsList, 0, 0);



                        // Only continue if the first item in the words list does not contain any letters.
                        // This is to skip anything that doesn't start with an invoice number.
                        // Usually the first two lines and the last line of the file should be skipped.
                        if (!words[1].Any(Char.IsLetter))
                        {
                            Console.WriteLine("IsLetter");

                            if (words[1] != prevInvoiceNumber)
                            {
                                Console.WriteLine("not prevInvoiceNumber");
                                Console.WriteLine(words[3]);
                                //Console.WriteLine(words[4]);
                                List<Items> items = new List<Items>
                                {

                                    // words[2] is the item name words[3] is the item price.
                                    
                                    new Items(words[3], words[4], decimal.Parse(words[5], format))
                                };


                                // words[0] is the item number. words[1] is the invoice name
                                Console.WriteLine(words[0]);
                                date = DateTime.ParseExact(words[0], "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                Console.WriteLine(date);

                                if (int.Parse(words[1]) != taxList[taxIndex].number)
                                {
                                    throw new Exception("Invoice numbers don't match. Invoice number: " + int.Parse(words[1]) + " Tax invoice number: " + taxList[taxIndex].number);
                                }

                                invoiceList.Add(new Invoice(date, int.Parse(words[1]), words[2], items, 0, taxList[taxIndex].tax));

                                taxIndex++;
                                i++;
      
                                prevInvoiceNumber = words[1];
                            }
                            else
                            {
                                Console.WriteLine("is prevInvoiceNumber");
                           
                                // words[2] is the item name. words[3] is the tax code. words[4] is the price.
                                invoiceList[i].getItemList().Add(new Items(words[3], words[4], decimal.Parse(words[5], format)));

                                prevInvoiceNumber = words[1];
                            }

                        }
                        Console.WriteLine("not is letter");

                    }
                    
                    parser.Close();
                }
                    
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("0 number: " + invoiceList[0].getNumber());
            Console.WriteLine("invoiceList count: " + invoiceList.Count());
            return invoiceList;
        } 



      /*  static List<InvoiceTotal> ReadTotalsFile(string totalsFile)
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
*/
  

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
                        sw.WriteLine("Date,Invoice,Name,TaxSale,WholeSale,FET,Disp,Labor,Scrap,Casing,TIS");

                        Console.WriteLine("Count: " + outputList.Count());
                        Console.WriteLine("outputNumber[0]: " + outputList[0].OutputNumber);

                        for (int i = 0; i < outputList.Count(); i++)
                        {
                            sw.WriteLine(outputList[i].Date.Month + "/" + outputList[i].Date.Day + "/" + outputList[i].Date.Year + "," + outputList[i].OutputNumber + ",\"" + outputList[i].OutputName + "\"," + outputList[i].TaxSale + ","
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
                decimal taxSale = 0;
                decimal fet = 0;
                decimal disposal = 0;
                decimal labor = 0;
                decimal scrap = 0;
                decimal casing = 0;
                decimal total = 0;

                //total = sum of item prices -> to not include tax
                /*
                for (int j = 0; j < invoiceList[i].getItemList().Count(); j++)
                {
                    total += invoiceList[i].getItemList()[j].getPrice();
                }
               */

                total = invoiceList[i].getTotal();

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
                    //TODO should just be invoice.total?????
                    Console.WriteLine("outputTax: " + invoiceList[i].getTax());
                    taxSale = total - fet - disposal - labor - scrap - casing + invoiceList[i].getTax();
                    output.Add(new OutputClass(invoiceList[i].getDate(), invoiceList[i].getNumber(), invoiceList[i].getName(), taxSale, 0, 
                        fet, disposal, labor, scrap, casing, invoiceList[i].getTotal() + invoiceList[i].getTax(), invoiceList[i].getTotal() + invoiceList[i].getTax()));
                    Console.WriteLine("taxSale");
                }
                else
                {
                    output.Add(new OutputClass(invoiceList[i].getDate(), invoiceList[i].getNumber(), invoiceList[i].getName(), 0, total,
                        0, 0, 0, 0, 0, total, total));
                    Console.WriteLine("WholeSale");
                }
            }
            return output;
        }


   
        // if all items in invoice are "Non-Taxable Sales", then it is wholesale (IsTaxSale returns false)
        static bool IsTaxSale (Invoice invoice)
        {

            if (invoice.getTax() != 0)
            {
                return true;
            }
            /*for(int i = 0; i < invoice.getItemList().Count(); i++)
            {
                if (invoice.getItemList()[i].getTaxCode() == "Taxable Sales")
                {
                    return true;
                }
            }
*/
            return false;


          /*  decimal itemsTotal = 0;
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
            */
        }


    }
}
