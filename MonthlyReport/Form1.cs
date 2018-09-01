using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
             * 3. Generate a .CSV file with the final report */

            
            //1. Read files
            List<InvoiceClass> invoiceList0 = ReadInvoiceFile(invoicesFile);
            List<InvoiceTotal> totalsList = ReadTotalsFile(totalsFile);
            
            //2. Maniputlate data
            List<OutputClass> outputList = new List<OutputClass>();
            List<InvoiceClass> sortedInvoice = invoiceList0.OrderBy(o => o.InvoiceNumber).ToList();
            
            for (int i = 0; i < sortedInvoice.Count(); i++)
                
            {
                sortedInvoice[i].InvoiceTotal = totalsList[i].Total;
            }

            outputList = OutputList(sortedInvoice);


            //3. Generate output file
            GenerateOutput(outputList);

        }


        //Function to read invoiceFile to a list of objects
        static List<InvoiceClass> ReadInvoiceFile(string invoiceFile)
        {
            string prevInvoiceNumber = "0";
            List<InvoiceClass> invoiceList = new List<InvoiceClass>();
            List<ItemsClass> itemsList = new List<ItemsClass>();
            var format = new NumberFormatInfo();
            format.NegativeSign = "-";
            int i = -1;

            try
            {

                using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(invoiceFile))
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] words = parser.ReadFields();//line.Split(',');

                        InvoiceClass invoice = new InvoiceClass(0, "", itemsList, 0);
                        //if (words[0].Any(x => !char.IsLetter(x)))
                        if (!words[0].Any(Char.IsLetter))
                        {
                            //Console.WriteLine(words[0]);
                            if (words[0] != prevInvoiceNumber)
                            {
                                List<ItemsClass> items = new List<ItemsClass>();
                                items.Add(new ItemsClass(words[2], decimal.Parse(words[3])));
                                invoiceList.Add(new InvoiceClass(int.Parse(words[0]), words[1], items, 0));
                                i++;
                                prevInvoiceNumber = words[0];
                            }
                            else
                            {
                                invoiceList[i].InvoiceItemArray.Add(new ItemsClass(words[2], decimal.Parse(words[3], format)));
                                prevInvoiceNumber = words[0];
                            }
                        }//if                      

                    }//while
                    parser.Close();
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("invoice error");
                MessageBox.Show(ex.Message);
            }

            return invoiceList;
        }

        //Function to read totalsFile to a list of objects
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
                            Console.WriteLine(words[0]);
                            totalsList.Add(new InvoiceTotal(int.Parse(words[0]), decimal.Parse(words[1])));

                        }
                    }
                    parser.Close();
                   
                }
                Console.WriteLine("closing");
                
                //file.Close();
            }
             catch (Exception ex)
            {
                Console.WriteLine("totals error");
                MessageBox.Show(ex.Message);

            }
            return totalsList;
        }

        //Function to generate output as a csv file
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

        //Function to create a list for output
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

        //Function to check if an invoice is a tax sale (or whole sale)
        static bool IsTaxSale (InvoiceClass invoice)
        {
            decimal itemsTotal = 0;
            for(int i = 0; i < invoice.InvoiceItemArray.Count(); i++)
            {
                itemsTotal += invoice.InvoiceItemArray[i].ItemPrice;
            }

            if(itemsTotal == invoice.InvoiceTotal)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
