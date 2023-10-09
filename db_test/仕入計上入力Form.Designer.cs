namespace db_test
{
    partial class 仕入計上入力Form
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
            this.ButtonF9 = new System.Windows.Forms.Button();
            this.ButtonF5 = new System.Windows.Forms.Button();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.仕入計上DataSet1 = new db_test.仕入計上DataSet();
            this.speeddbDataSet1 = new db_test.SPEEDDBDataSet();
            this.t仕入修正戻ファイルTableAdapter1 = new db_test.仕入計上DataSetTableAdapters.T仕入修正戻ファイルTableAdapter();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.仕入計上入力Template1 = new db_test.仕入計上入力Template();
            this.仕入先マスタ検索TableAdapter1 = new db_test.SPEEDDBTableAdapters.仕入先マスタ検索TableAdapter();
            this.exceL受注取込Template1 = new db_test.EXCEL受注取込Template();
            this.ButtonF4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.仕入計上DataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speeddbDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(700, 4);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 23);
            this.ButtonF9.TabIndex = 13;
            this.ButtonF9.TabStop = false;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // ButtonF5
            // 
            this.ButtonF5.Location = new System.Drawing.Point(620, 4);
            this.ButtonF5.Name = "ButtonF5";
            this.ButtonF5.Size = new System.Drawing.Size(75, 23);
            this.ButtonF5.TabIndex = 12;
            this.ButtonF5.TabStop = false;
            this.ButtonF5.Text = "button1";
            this.ButtonF5.UseVisualStyleBackColor = true;
            this.ButtonF5.Visible = false;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(540, 4);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 11;
            this.ButtonF3.TabStop = false;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(780, 4);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 10;
            this.ButtonF10.TabStop = false;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // 仕入計上DataSet1
            // 
            this.仕入計上DataSet1.DataSetName = "仕入計上DataSet";
            this.仕入計上DataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // speeddbDataSet1
            // 
            this.speeddbDataSet1.DataSetName = "SPEEDDBDataSet";
            this.speeddbDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // t仕入修正戻ファイルTableAdapter1
            // 
            this.t仕入修正戻ファイルTableAdapter1.ClearBeforeFill = true;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(1057, 549);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.仕入計上入力Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 仕入先マスタ検索TableAdapter1
            // 
            this.仕入先マスタ検索TableAdapter1.ClearBeforeFill = true;
            // 
            // ButtonF4
            // 
            this.ButtonF4.Location = new System.Drawing.Point(860, 4);
            this.ButtonF4.Name = "ButtonF4";
            this.ButtonF4.Size = new System.Drawing.Size(75, 23);
            this.ButtonF4.TabIndex = 14;
            this.ButtonF4.TabStop = false;
            this.ButtonF4.Text = "button1";
            this.ButtonF4.UseVisualStyleBackColor = true;
            this.ButtonF4.Visible = false;
            // 
            // 仕入計上入力Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 549);
            this.Controls.Add(this.ButtonF4);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF5);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "仕入計上入力Form";
            this.Text = "仕入計上入力Form";
            this.Load += new System.EventHandler(this.仕入計上入力Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.仕入計上DataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speeddbDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 仕入計上入力Template 仕入計上入力Template1;
        //private 仕入計上入力明細Template 仕入計上入力明細Template1;
        //private 仕入計上入力DataSetTableAdapters.WK発注BファイルTableAdapter wK発注BファイルTableAdapter1;
        //private 仕入計上入力DataSet 仕入計上入力DataSet1;
        private System.Windows.Forms.Button ButtonF9;
        private System.Windows.Forms.Button ButtonF5;
        private System.Windows.Forms.Button ButtonF3;
        private System.Windows.Forms.Button ButtonF10;
        private 仕入計上DataSet 仕入計上DataSet1;
        private SPEEDDBDataSet speeddbDataSet1;
        private 仕入計上DataSetTableAdapters.T仕入修正戻ファイルTableAdapter t仕入修正戻ファイルTableAdapter1;
        private SPEEDDBTableAdapters.仕入先マスタ検索TableAdapter 仕入先マスタ検索TableAdapter1;
        private EXCEL受注取込Template exceL受注取込Template1;
        private System.Windows.Forms.Button ButtonF4;
    }
}