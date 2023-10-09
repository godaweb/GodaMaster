namespace db_test
{
    partial class monotaRO受注取込Form
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
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.wTドラスタ受注ファイルDataSet1 = new db_test.WTドラスタ受注ファイルDataSet();
            this.wTドラスタ受注ファイルTableAdapter1 = new db_test.WTドラスタ受注ファイルDataSetTableAdapters.WTドラスタ受注ファイルTableAdapter();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.monotaRO受注取込Template1 = new db_test.monotaRO受注取込Template();
            this.wTドラスタ受注ファイル1TableAdapter1 = new db_test.WTドラスタ受注ファイルDataSetTableAdapters.WTドラスタ受注ファイル1TableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.wTドラスタ受注ファイルDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(504, 8);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 14;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(320, 8);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 23);
            this.ButtonF9.TabIndex = 13;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(412, 8);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 12;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // wTドラスタ受注ファイルDataSet1
            // 
            this.wTドラスタ受注ファイルDataSet1.DataSetName = "WTドラスタ受注ファイルDataSet";
            this.wTドラスタ受注ファイルDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // wTドラスタ受注ファイルTableAdapter1
            // 
            this.wTドラスタ受注ファイルTableAdapter1.ClearBeforeFill = true;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.AllowUserToDeleteRows = false;
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(608, 320);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.monotaRO受注取込Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            this.gcMultiRow1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.gcMultiRow1_PreviewKeyDown);
            // 
            // wTドラスタ受注ファイル1TableAdapter1
            // 
            this.wTドラスタ受注ファイル1TableAdapter1.ClearBeforeFill = true;
            // 
            // monotaRO受注取込Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 320);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow1);
            this.KeyPreview = true;
            this.Name = "monotaRO受注取込Form";
            this.Text = "monotaRO受注取込Form";
            this.Load += new System.EventHandler(this.monotaRO受注取込Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.wTドラスタ受注ファイルDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private monotaRO受注取込Template monotaRO受注取込Template1;
        private System.Windows.Forms.Button ButtonF10;
        private System.Windows.Forms.Button ButtonF9;
        private System.Windows.Forms.Button ButtonF3;
        private WTドラスタ受注ファイルDataSet wTドラスタ受注ファイルDataSet1;
        private WTドラスタ受注ファイルDataSetTableAdapters.WTドラスタ受注ファイルTableAdapter wTドラスタ受注ファイルTableAdapter1;
        private WTドラスタ受注ファイルDataSetTableAdapters.WTドラスタ受注ファイル1TableAdapter wTドラスタ受注ファイル1TableAdapter1;
    }
}