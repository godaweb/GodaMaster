namespace db_test
{
    partial class 新受注入力Form
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
            this.components = new System.ComponentModel.Container();
            this.受注ファイルDataSet = new db_test.受注ファイルDataSet();
            this.受注ファイルDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gcMultiRow3 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.受注入力明細Template1 = new db_test.受注入力明細Template();
            this.gcMultiRow2 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.受注入力入力Template1 = new db_test.受注入力入力Template();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.受注入力ヘッダTemplate1 = new db_test.受注入力ヘッダTemplate();
            ((System.ComponentModel.ISupportInitialize)(this.受注ファイルDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注ファイルDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // 受注ファイルDataSet
            // 
            this.受注ファイルDataSet.DataSetName = "受注ファイルDataSet";
            this.受注ファイルDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 受注ファイルDataSetBindingSource
            // 
            this.受注ファイルDataSetBindingSource.DataSource = this.受注ファイルDataSet;
            this.受注ファイルDataSetBindingSource.Position = 0;
            // 
            // gcMultiRow3
            // 
            this.gcMultiRow3.Location = new System.Drawing.Point(8, 316);
            this.gcMultiRow3.Name = "gcMultiRow3";
            this.gcMultiRow3.Size = new System.Drawing.Size(1036, 332);
            this.gcMultiRow3.TabIndex = 2;
            this.gcMultiRow3.Template = this.受注入力明細Template1;
            this.gcMultiRow3.Text = "gcMultiRow3";
            // 
            // gcMultiRow2
            // 
            this.gcMultiRow2.AllowUserToAddRows = false;
            this.gcMultiRow2.AllowUserToResize = false;
            this.gcMultiRow2.AllowUserToZoom = false;
            this.gcMultiRow2.DataSource = this.受注ファイルDataSetBindingSource;
            this.gcMultiRow2.Location = new System.Drawing.Point(12, 268);
            this.gcMultiRow2.Name = "gcMultiRow2";
            this.gcMultiRow2.RowCount = 1;
            this.gcMultiRow2.Size = new System.Drawing.Size(1032, 60);
            this.gcMultiRow2.TabIndex = 1;
            this.gcMultiRow2.Template = this.受注入力入力Template1;
            this.gcMultiRow2.Text = "gcMultiRow2";
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(1044, 284);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.受注入力ヘッダTemplate1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 新受注入力Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 674);
            this.Controls.Add(this.gcMultiRow3);
            this.Controls.Add(this.gcMultiRow2);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "新受注入力Form";
            this.Text = "新受注入力Form";
            this.Load += new System.EventHandler(this.新受注入力Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.受注ファイルDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注ファイルDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 受注入力ヘッダTemplate 受注入力ヘッダTemplate1;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow2;
        private 受注入力入力Template 受注入力入力Template1;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow3;
        private 受注入力明細Template 受注入力明細Template1;
        private System.Windows.Forms.BindingSource 受注ファイルDataSetBindingSource;
        private 受注ファイルDataSet 受注ファイルDataSet;
    }
}