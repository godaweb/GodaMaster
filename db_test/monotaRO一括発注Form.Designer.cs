namespace db_test
{
    partial class monotaRO一括発注Form
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
            this.ButtonF6 = new System.Windows.Forms.Button();
            this.ButtonF5 = new System.Windows.Forms.Button();
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.monotaRO一括発注DataSet1 = new db_test.monotaRO一括発注DataSet();
            this.wT発注戻しファイルTableAdapter1 = new db_test.monotaRO一括発注DataSetTableAdapters.WT発注戻しファイルTableAdapter();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.monotaRO一括発注Template1 = new db_test.monotaRO一括発注Template();
            this.ButtonF4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.monotaRO一括発注DataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonF6
            // 
            this.ButtonF6.Location = new System.Drawing.Point(832, 8);
            this.ButtonF6.Name = "ButtonF6";
            this.ButtonF6.Size = new System.Drawing.Size(75, 23);
            this.ButtonF6.TabIndex = 17;
            this.ButtonF6.Text = "button1";
            this.ButtonF6.UseVisualStyleBackColor = true;
            this.ButtonF6.Visible = false;
            // 
            // ButtonF5
            // 
            this.ButtonF5.Location = new System.Drawing.Point(752, 8);
            this.ButtonF5.Name = "ButtonF5";
            this.ButtonF5.Size = new System.Drawing.Size(75, 23);
            this.ButtonF5.TabIndex = 16;
            this.ButtonF5.Text = "button1";
            this.ButtonF5.UseVisualStyleBackColor = true;
            this.ButtonF5.Visible = false;
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(1064, 8);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 15;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // monotaRO一括発注DataSet1
            // 
            this.monotaRO一括発注DataSet1.DataSetName = "monotaRO一括発注DataSet";
            this.monotaRO一括発注DataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // wT発注戻しファイルTableAdapter1
            // 
            this.wT発注戻しファイルTableAdapter1.ClearBeforeFill = true;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(1178, 689);
            this.gcMultiRow1.TabIndex = 1;
            this.gcMultiRow1.Template = this.monotaRO一括発注Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // ButtonF4
            // 
            this.ButtonF4.Location = new System.Drawing.Point(672, 8);
            this.ButtonF4.Name = "ButtonF4";
            this.ButtonF4.Size = new System.Drawing.Size(75, 23);
            this.ButtonF4.TabIndex = 18;
            this.ButtonF4.Text = "button1";
            this.ButtonF4.UseVisualStyleBackColor = true;
            this.ButtonF4.Visible = false;
            // 
            // monotaRO一括発注Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 689);
            this.Controls.Add(this.ButtonF4);
            this.Controls.Add(this.ButtonF6);
            this.Controls.Add(this.ButtonF5);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.gcMultiRow1);
            this.KeyPreview = true;
            this.Name = "monotaRO一括発注Form";
            this.Text = "monotaRO一括発注Form";
            this.Load += new System.EventHandler(this.monotaRO一括発注Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.monotaRO一括発注DataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private monotaRO一括発注Template monotaRO一括発注Template1;
        private System.Windows.Forms.Button ButtonF6;
        private System.Windows.Forms.Button ButtonF5;
        private System.Windows.Forms.Button ButtonF10;
        private monotaRO一括発注DataSet monotaRO一括発注DataSet1;
        private monotaRO一括発注DataSetTableAdapters.WT発注戻しファイルTableAdapter wT発注戻しファイルTableAdapter1;
        private System.Windows.Forms.Button ButtonF4;
    }
}