namespace db_test
{
    partial class 発注残集計_仕入先Form
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
            this.発注残集計仕入先BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPEEDDB = new db_test.SPEEDDB();
            this.発注残集計_仕入先Template1 = new db_test.発注残集計_仕入先Template();
            this.発注残集計_仕入先TableAdapter = new db_test.SPEEDDBTableAdapters.発注残集計_仕入先TableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.発注残集計仕入先BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.AllowUserToDeleteRows = false;
            this.gcMultiRow1.DataSource = this.発注残集計仕入先BindingSource;
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(615, 262);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.発注残集計_仕入先Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 発注残集計仕入先BindingSource
            // 
            this.発注残集計仕入先BindingSource.DataMember = "発注残集計_仕入先";
            this.発注残集計仕入先BindingSource.DataSource = this.sPEEDDB;
            // 
            // sPEEDDB
            // 
            this.sPEEDDB.DataSetName = "SPEEDDB";
            this.sPEEDDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 発注残集計_仕入先TableAdapter
            // 
            this.発注残集計_仕入先TableAdapter.ClearBeforeFill = true;
            // 
            // 発注残集計_仕入先Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 262);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "発注残集計_仕入先Form";
            this.Text = "発注残集計_仕入先Form";
            this.Load += new System.EventHandler(this.発注残集計_仕入先Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.発注残集計仕入先BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private System.Windows.Forms.BindingSource 発注残集計仕入先BindingSource;
        private SPEEDDB sPEEDDB;
        private 発注残集計_仕入先Template 発注残集計_仕入先Template1;
        private SPEEDDBTableAdapters.発注残集計_仕入先TableAdapter 発注残集計_仕入先TableAdapter;
    }
}