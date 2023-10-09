namespace db_test
{
    partial class 得意先検索Form
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
            this.t得意先マスタBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.t得意先マスタTableAdapter = new db_test.SPEEDDBDataSetTableAdapters.T得意先マスタTableAdapter();
            this.speeddbDataSet = new db_test.SPEEDDBDataSet();
            this.speeddb1 = new db_test.SPEEDDB();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.ButtonF9 = new System.Windows.Forms.Button();
            this.ButtonF5 = new System.Windows.Forms.Button();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.得意先検索Template1 = new db_test.得意先検索Template();
            this.得意先検索Template2 = new db_test.得意先検索Template();
            ((System.ComponentModel.ISupportInitialize)(this.t得意先マスタBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speeddbDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speeddb1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // t得意先マスタTableAdapter
            // 
            this.t得意先マスタTableAdapter.ClearBeforeFill = true;
            // 
            // speeddbDataSet
            // 
            this.speeddbDataSet.DataSetName = "SPEEDDBDataSet";
            this.speeddbDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // speeddb1
            // 
            this.speeddb1.DataSetName = "SPEEDDB";
            this.speeddb1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(324, 0);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 8;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(232, 0);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 19;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(492, 0);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 23);
            this.ButtonF9.TabIndex = 18;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // ButtonF5
            // 
            this.ButtonF5.Location = new System.Drawing.Point(408, 0);
            this.ButtonF5.Name = "ButtonF5";
            this.ButtonF5.Size = new System.Drawing.Size(75, 23);
            this.ButtonF5.TabIndex = 17;
            this.ButtonF5.Text = "button1";
            this.ButtonF5.UseVisualStyleBackColor = true;
            this.ButtonF5.Visible = false;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.DataSource = this.speeddbDataSet;
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(861, 532);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.得意先検索Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 得意先検索Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 532);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF5);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "得意先検索Form";
            this.Text = "得意先検索";
            this.Load += new System.EventHandler(this.得意先Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.t得意先マスタBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speeddbDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speeddb1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource t得意先マスタBindingSource;
        private SPEEDDBDataSetTableAdapters.T得意先マスタTableAdapter t得意先マスタTableAdapter;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private SPEEDDBDataSet speeddbDataSet;
        private 得意先検索Template 得意先検索Template1;
        private 得意先検索Template 得意先検索Template2;
        private SPEEDDB speeddb1;
        private System.Windows.Forms.Button ButtonF3;
        private System.Windows.Forms.Button ButtonF10;
        private System.Windows.Forms.Button ButtonF9;
        private System.Windows.Forms.Button ButtonF5;
    }
}