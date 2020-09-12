namespace MonthlyReport
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InvoicesButton = new System.Windows.Forms.Button();
            this.TaxButton = new System.Windows.Forms.Button();
            this.HelpButton = new System.Windows.Forms.Button();
            this.CreateFileButton = new System.Windows.Forms.Button();
            this.TaxTextBox = new System.Windows.Forms.TextBox();
            this.InvoicesTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InvoicesButton
            // 
            this.InvoicesButton.AutoSize = true;
            this.InvoicesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InvoicesButton.Location = new System.Drawing.Point(12, 59);
            this.InvoicesButton.Name = "InvoicesButton";
            this.InvoicesButton.Size = new System.Drawing.Size(96, 30);
            this.InvoicesButton.TabIndex = 0;
            this.InvoicesButton.Text = "Invoices";
            this.InvoicesButton.UseVisualStyleBackColor = true;
            this.InvoicesButton.Click += new System.EventHandler(this.InvoicesButton_Click);
            // 
            // TaxButton
            // 
            this.TaxButton.AutoSize = true;
            this.TaxButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaxButton.Location = new System.Drawing.Point(12, 95);
            this.TaxButton.Name = "TaxButton";
            this.TaxButton.Size = new System.Drawing.Size(96, 30);
            this.TaxButton.TabIndex = 1;
            this.TaxButton.Text = "Tax";
            this.TaxButton.UseVisualStyleBackColor = true;
            this.TaxButton.Click += new System.EventHandler(this.TaxButton_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.AutoSize = true;
            this.HelpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton.Location = new System.Drawing.Point(375, 12);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(75, 30);
            this.HelpButton.TabIndex = 2;
            this.HelpButton.Text = "Help";
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // CreateFileButton
            // 
            this.CreateFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateFileButton.AutoSize = true;
            this.CreateFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateFileButton.Location = new System.Drawing.Point(183, 130);
            this.CreateFileButton.Name = "CreateFileButton";
            this.CreateFileButton.Size = new System.Drawing.Size(101, 30);
            this.CreateFileButton.TabIndex = 3;
            this.CreateFileButton.Text = "Create File";
            this.CreateFileButton.UseVisualStyleBackColor = true;
            this.CreateFileButton.Click += new System.EventHandler(this.CreateFileButton_Click);
            // 
            // TaxTextBox
            // 
            this.TaxTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaxTextBox.Location = new System.Drawing.Point(114, 97);
            this.TaxTextBox.Name = "TaxTextBox";
            this.TaxTextBox.Size = new System.Drawing.Size(336, 27);
            this.TaxTextBox.TabIndex = 4;
            // 

            // InvoicesTextBox
            // 
            this.InvoicesTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InvoicesTextBox.Location = new System.Drawing.Point(114, 61);
            this.InvoicesTextBox.Name = "InvoicesTextBox";
            this.InvoicesTextBox.Size = new System.Drawing.Size(336, 27);
            this.InvoicesTextBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Select files for Invoices and Tax";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 171);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InvoicesTextBox);
            this.Controls.Add(this.TaxTextBox);
            this.Controls.Add(this.CreateFileButton);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.TaxButton);
            this.Controls.Add(this.InvoicesButton);
            this.Name = "Form1";
            this.Text = "Monthly Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InvoicesButton;
        private System.Windows.Forms.Button TaxButton;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.Button CreateFileButton;
        private System.Windows.Forms.TextBox TaxTextBox;
        private System.Windows.Forms.TextBox InvoicesTextBox;
        private System.Windows.Forms.Label label1;
    }
}

