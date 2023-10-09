namespace db_test
{
    partial class 受注残集計_商品別内訳Form
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
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.受注残集計_商品別内訳Template1 = new db_test.受注残集計_商品別内訳Template();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.AllowUserToDeleteRows = false;
            this.gcMultiRow1.AllowUserToResize = false;
            this.gcMultiRow1.AllowUserToZoom = false;
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(972, 428);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.受注残集計_商品別内訳Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 受注残集計_商品別内訳Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 428);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "受注残集計_商品別内訳Form";
            this.Text = "受注残集計_商品別内訳Form";
            this.Load += new System.EventHandler(this.受注残集計_商品別内訳Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 受注残集計_商品別内訳Template 受注残集計_商品別内訳Template1;
    }
}