namespace Dehasoft.WinForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Button btnBuy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnSync = new Button();
            lblStatus = new Label();
            dgvProducts = new DataGridView();
            numQuantity = new NumericUpDown();
            btnBuy = new Button();
            SuspendLayout();
            // 
            // btnSync
            // 
            btnSync.Location = new Point(30, 30);
            btnSync.Name = "btnSync";
            btnSync.Size = new Size(250, 40);
            btnSync.TabIndex = 0;
            btnSync.Text = "Siparişleri Senkronize Et";
            btnSync.UseVisualStyleBackColor = true;
            btnSync.Click += btnSync_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(30, 80);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(139, 20);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Durum: Bekleniyor...";
            // 
            // dgvProducts
            // 
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(30, 120);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.RowTemplate.Height = 29;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(600, 250);
            dgvProducts.TabIndex = 2;
            // 
            // numQuantity
            // 
            numQuantity.Location = new Point(30, 390);
            numQuantity.Name = "numQuantity";
            numQuantity.Size = new Size(120, 27);
            numQuantity.Minimum = 1;
            numQuantity.TabIndex = 3;
            // 
            // btnBuy
            // 
            btnBuy.Location = new Point(170, 387);
            btnBuy.Name = "btnBuy";
            btnBuy.Size = new Size(110, 30);
            btnBuy.TabIndex = 4;
            btnBuy.Text = "Satın Al";
            btnBuy.UseVisualStyleBackColor = true;
            btnBuy.Click += btnBuy_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(680, 450);
            Controls.Add(btnBuy);
            Controls.Add(numQuantity);
            Controls.Add(dgvProducts);
            Controls.Add(lblStatus);
            Controls.Add(btnSync);
            Name = "Form1";
            Text = "Dehasoft Entegrasyon";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}