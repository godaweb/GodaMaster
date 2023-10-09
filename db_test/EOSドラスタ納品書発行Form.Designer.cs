namespace db_test
{
    partial class EOSドラスタ納品書発行Form
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
            this.eoSドラスタ納品書発行Template1 = new db_test.EOSドラスタ納品書発行Template();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(625, 272);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.eoSドラスタ納品書発行Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // EOSドラスタ納品書発行Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 272);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "EOSドラスタ納品書発行Form";
            this.Text = "EOSドラスタ納品書発行Form";
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private EOSドラスタ納品書発行Template eoSドラスタ納品書発行Template1;
    }
}