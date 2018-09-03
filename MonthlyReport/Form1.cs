using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MonthlyReport
{
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
            List<InvoiceClass> invoiceList = ReadInvoiceFile(invoicesFile);
            List<InvoiceTotal> totalsList = ReadTotalsFile(totalsFile);
            



            //2. Maniputlate data
            List<OutputClass> outputList = new List<OutputClass>();

            try
            {
                for (int i = 0; i < invoiceList.Count(); i++)

                {
                    invoiceList[i].InvoiceTotal = totalsList[i].Total;
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


        static List<InvoiceClass> ReadInvoiceFile(string invoiceFile)
        {
            string prevInvoiceNumber = "0";

            List<InvoiceClass> invoiceList = new List<InvoiceClass>();
            List<ItemsClass> itemsList = new List<ItemsClass>();

            
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

                        InvoiceClass invoice = new InvoiceClass(0, "", itemsList, 0);


                        // Only continue if the first item in the words list does not contain any letters.
                        // This is to skip anything that doesn't start with an invoice number.
                        // Usually the first two lines and the last line of the file should be skipped.
                        if (!words[0].Any(Char.IsLetter))
                        {

                            if (words[0] != prevInvoiceNumber)
                            {

                                List<ItemsClass> items = new List<ItemsClass>();

                                // words[2] is the item name words[3] is the item price.
                                items.Add(new ItemsClass(words[2], decimal.Parse(words[3], format)));

     
                                // words[0] is the item number. words[1] is the invoice name
                                invoiceList.Add(new InvoiceClass(int.Parse(words[0]), words[1], items, 0));

  
                                i++;

      
                                prevInvoiceNumber = words[0];
                            }
                            else
                            {
                           
                                // words[2] is the item name. words[3] is the item price.
                                invoiceList[i].InvoiceItemArray.Add(new ItemsClass(words[2], decimal.Parse(words[3], format)));

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



        static List<OutputClass> OutputList (List<InvoiceClass> invoiceList)
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
                decimal total = invoiceList[i].InvoiceTotal;


                if (IsTaxSale(invoiceList[i]))
                {
                    for(int j = 0; j < invoiceList[i].InvoiceItemArray.Count(); j++)
                    {
                        if(invoiceList[i].InvoiceItemArray[j].ItemName.Contains("F.E.T"))
                        {
                            fet += invoiceList[i].InvoiceItemArray[j].ItemPrice;
                        }
                        if(invoiceList[i].InvoiceItemArray[j].ItemName.Contains("DISPOSAL"))
                        {
                            disposal += invoiceList[i].InvoiceItemArray[j].ItemPrice;
                        }
                        if(invoiceList[i].InvoiceItemArray[j].ItemName.Contains("DISMOUNT") ||
                            invoiceList[i].InvoiceItemArray[j].ItemName.Contains("REPAIR"))
                        {
                            labor += invoiceList[i].InvoiceItemArray[j].ItemPrice;
                        }
                        if(invoiceList[i].InvoiceItemArray[j].ItemName == "SCRAP TIRE ENVIRONMENTAL FEE")
                        {
                            scrap += invoiceList[i].InvoiceItemArray[j].ItemPrice;
                        }
                        if(invoiceList[i].InvoiceItemArray[j].ItemName.Contains("CASING") ||
                           invoiceList[i].InvoiceItemArray[j].ItemName.Contains("ADJ"))
                        { 
                            casing += invoiceList[i].InvoiceItemArray[j].ItemPrice;
                        }
                        
                    }
                    tax = total - fet - disposal - labor - scrap - casing;
                    output.Add(new OutputClass(invoiceList[i].InvoiceNumber, invoiceList[i].InvoiceName, tax, 0, 
                        fet, disposal, labor, scrap, casing, total, total));
                }
                else
                {
                    output.Add(new OutputClass(invoiceList[i].InvoiceNumber, invoiceList[i].InvoiceName, 0, total,
                        0, 0, 0, 0, 0, total, total));
                }
            }
            return output;
        }


   
        static bool IsTaxSale (InvoiceClass invoice)
        {
            decimal itemsTotal = 0;
            for(int i = 0; i < invoice.InvoiceItemArray.Count(); i++)
            {
                itemsTotal += invoice.InvoiceItemArray[i].ItemPrice;
            }

            // If the items total = the invoice total, then it was not a tax sale.
            // The difference comes from the taxed amount.
            if(itemsTotal == invoice.InvoiceTotal)
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
