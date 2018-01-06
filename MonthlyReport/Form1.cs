using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonthlyReport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            //Bring up new form
        }

        private void InvoicesButton_Click(object sender, EventArgs e)
        {
            //select Invoice File
        }

        private void TotalsButton_Click(object sender, EventArgs e)
        {
            //select Totals File
        }

        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            //select save location
            //create file
        }
    }
}
