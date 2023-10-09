namespace db_test
{
    partial class 商品別受注出荷履歴照会Form
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
            this.商品別受注出荷履歴照会BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPEEDDB = new db_test.SPEEDDB();
            this.商品別受注出荷履歴照会TableAdapter = new db_test.SPEEDDBTableAdapters.商品別受注出荷履歴照会TableAdapter();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.商品別受注出荷履歴照会Template1 = new db_test.商品別受注出荷履歴照会Template();
            this.ButtonF10 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.商品別受注出荷履歴照会BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // 商品別受注出荷履歴照会BindingSource
            // 
            this.商品別受注出荷履歴照会BindingSource.DataMember = "商品別受注出荷履歴照会";
            this.商品別受注出荷履歴照会BindingSource.DataSource = this.sPEEDDB;
            // 
            // sPEEDDB
            // 
            this.sPEEDDB.DataSetName = "SPEEDDB";
            this.sPEEDDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 商品別受注出荷履歴照会TableAdapter
            // 
            this.商品別受注出荷履歴照会TableAdapter.ClearBeforeFill = true;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.AllowUserToDeleteRows = false;
            this.gcMultiRow1.DataSource = this.商品別受注出荷履歴照会BindingSource;
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(670, 343);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.商品別受注出荷履歴照会Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(524, 4);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 15;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // 商品別受注出荷履歴照会Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 343);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "商品別受注出荷履歴照会Form";
            this.Text = "商品別受注出荷履歴照会Form";
            this.Load += new System.EventHandler(this.商品別受注出荷履歴照会Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.商品別受注出荷履歴照会BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private System.Windows.Forms.BindingSource 商品別受注出荷履歴照会BindingSource;
        private SPEEDDB sPEEDDB;
        private 商品別受注出荷履歴照会Template 商品別受注出荷履歴照会Template1;
        private SPEEDDBTableAdapters.商品別受注出荷履歴照会TableAdapter 商品別受注出荷履歴照会TableAdapter;
        private System.Windows.Forms.Button ButtonF10;
    }
}