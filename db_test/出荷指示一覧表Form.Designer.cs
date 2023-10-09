namespace db_test
{
    partial class 出荷指示一覧表Form
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
            this.出荷指示一覧表TableAdapter1 = new db_test.SPEEDDBTableAdapters.出荷指示一覧表TableAdapter();
            this.出荷指示一覧表CrystalReport1 = new db_test.出荷指示一覧表CrystalReport();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.出荷指示一覧表Template1 = new db_test.出荷指示一覧表Template();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // 出荷指示一覧表TableAdapter1
            // 
            this.出荷指示一覧表TableAdapter1.ClearBeforeFill = true;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(540, 8);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 9;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(636, 520);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.出荷指示一覧表Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 出荷指示一覧表Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 500);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow1);
            this.KeyPreview = true;
            this.Name = "出荷指示一覧表Form";
            this.Text = "出荷指示一覧表Form";
            this.Load += new System.EventHandler(this.出荷指示一覧表Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 出荷指示一覧表Template 出荷指示一覧表Template1;
        private SPEEDDBTableAdapters.出荷指示一覧表TableAdapter 出荷指示一覧表TableAdapter1;
        private 出荷指示一覧表CrystalReport 出荷指示一覧表CrystalReport1;
        private System.Windows.Forms.Button ButtonF3;
    }
}