namespace db_test
{
    partial class 伝票検索計上Form
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
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.ButtonF9 = new System.Windows.Forms.Button();
            this.ButtonF5 = new System.Windows.Forms.Button();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.gcMultiRow3 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.伝票検索計上右Template1 = new db_test.伝票検索計上右Template();
            this.gcMultiRow2 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.伝票検索計上左Template1 = new db_test.伝票検索計上左Template();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.伝票検索計上_ヘッダTemplate1 = new db_test.伝票検索計上_ヘッダTemplate();
            this.speeddb1 = new db_test.SPEEDDB();
            this.伝票検索計上明細TableAdapter1 = new db_test.SPEEDDBTableAdapters.伝票検索計上明細TableAdapter();
            this.伝票検索計上ヘッダTableAdapter1 = new db_test.SPEEDDBTableAdapters.伝票検索計上ヘッダTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speeddb1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(544, 0);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 16);
            this.ButtonF10.TabIndex = 28;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(804, 0);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 16);
            this.ButtonF9.TabIndex = 27;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // ButtonF5
            // 
            this.ButtonF5.Location = new System.Drawing.Point(720, 0);
            this.ButtonF5.Name = "ButtonF5";
            this.ButtonF5.Size = new System.Drawing.Size(75, 16);
            this.ButtonF5.TabIndex = 26;
            this.ButtonF5.Text = "button1";
            this.ButtonF5.UseVisualStyleBackColor = true;
            this.ButtonF5.Visible = false;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(636, 0);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 16);
            this.ButtonF3.TabIndex = 25;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // gcMultiRow3
            // 
            this.gcMultiRow3.Location = new System.Drawing.Point(448, 128);
            this.gcMultiRow3.Name = "gcMultiRow3";
            this.gcMultiRow3.Size = new System.Drawing.Size(432, 348);
            this.gcMultiRow3.TabIndex = 2;
            this.gcMultiRow3.Template = this.伝票検索計上右Template1;
            this.gcMultiRow3.Text = "gcMultiRow3";
            // 
            // gcMultiRow2
            // 
            this.gcMultiRow2.Location = new System.Drawing.Point(0, 128);
            this.gcMultiRow2.Name = "gcMultiRow2";
            this.gcMultiRow2.Size = new System.Drawing.Size(448, 348);
            this.gcMultiRow2.TabIndex = 1;
            this.gcMultiRow2.Template = this.伝票検索計上左Template1;
            this.gcMultiRow2.Text = "gcMultiRow2";
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(880, 132);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.伝票検索計上_ヘッダTemplate1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // speeddb1
            // 
            this.speeddb1.DataSetName = "SPEEDDB";
            this.speeddb1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 伝票検索計上明細TableAdapter1
            // 
            this.伝票検索計上明細TableAdapter1.ClearBeforeFill = true;
            // 
            // 伝票検索計上ヘッダTableAdapter1
            // 
            this.伝票検索計上ヘッダTableAdapter1.ClearBeforeFill = true;
            // 
            // 伝票検索計上Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 476);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF5);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow3);
            this.Controls.Add(this.gcMultiRow2);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "伝票検索計上Form";
            this.Text = "伝票検索計上Form";
            this.Load += new System.EventHandler(this.伝票検索計上Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speeddb1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 伝票検索計上_ヘッダTemplate 伝票検索計上_ヘッダTemplate1;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow2;
        private 伝票検索計上左Template 伝票検索計上左Template1;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow3;
        private 伝票検索計上右Template 伝票検索計上右Template1;
        private System.Windows.Forms.Button ButtonF10;
        private System.Windows.Forms.Button ButtonF9;
        private System.Windows.Forms.Button ButtonF5;
        private System.Windows.Forms.Button ButtonF3;
        private SPEEDDB speeddb1;
        private SPEEDDBTableAdapters.伝票検索計上明細TableAdapter 伝票検索計上明細TableAdapter1;
        private SPEEDDBTableAdapters.伝票検索計上ヘッダTableAdapter 伝票検索計上ヘッダTableAdapter1;
    }
}