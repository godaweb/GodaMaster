namespace db_test
{
    partial class 受注残集計_得意先内訳Form
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
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.受注残集計得意先別内訳BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPEEDDB = new db_test.SPEEDDB();
            this.受注残集計_得意先内訳Template1 = new db_test.受注残集計_得意先内訳Template();
            this.受注残集計_得意先別内訳TableAdapter = new db_test.SPEEDDBTableAdapters.受注残集計_得意先別内訳TableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注残集計得意先別内訳BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.AllowUserToDeleteRows = false;
            this.gcMultiRow1.DataSource = this.受注残集計得意先別内訳BindingSource;
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(975, 262);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.受注残集計_得意先内訳Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 受注残集計得意先別内訳BindingSource
            // 
            this.受注残集計得意先別内訳BindingSource.DataMember = "受注残集計_得意先別内訳";
            this.受注残集計得意先別内訳BindingSource.DataSource = this.sPEEDDB;
            // 
            // sPEEDDB
            // 
            this.sPEEDDB.DataSetName = "SPEEDDB";
            this.sPEEDDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 受注残集計_得意先別内訳TableAdapter
            // 
            this.受注残集計_得意先別内訳TableAdapter.ClearBeforeFill = true;
            // 
            // 受注残集計_得意先内訳Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 262);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "受注残集計_得意先内訳Form";
            this.Text = "受注残集計_得意先内訳Form";
            this.Load += new System.EventHandler(this.受注残集計_得意先内訳Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注残集計得意先別内訳BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private System.Windows.Forms.BindingSource 受注残集計得意先別内訳BindingSource;
        private SPEEDDB sPEEDDB;
        private 受注残集計_得意先内訳Template 受注残集計_得意先内訳Template1;
        private SPEEDDBTableAdapters.受注残集計_得意先別内訳TableAdapter 受注残集計_得意先別内訳TableAdapter;
    }
}