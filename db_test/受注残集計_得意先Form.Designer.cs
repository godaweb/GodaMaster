namespace db_test
{
    partial class 受注残集計_得意先Form
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
            this.受注残集計得意先BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPEEDDB = new db_test.SPEEDDB();
            this.受注残集計_得意先Template1 = new db_test.受注残集計_得意先Template();
            this.受注残集計_得意先TableAdapter = new db_test.SPEEDDBTableAdapters.受注残集計_得意先TableAdapter();
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.ButtonF9 = new System.Windows.Forms.Button();
            this.ButtonF3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注残集計得意先BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.AllowUserToDeleteRows = false;
            this.gcMultiRow1.DataSource = this.受注残集計得意先BindingSource;
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(778, 262);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.受注残集計_得意先Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 受注残集計得意先BindingSource
            // 
            this.受注残集計得意先BindingSource.DataMember = "受注残集計_得意先";
            this.受注残集計得意先BindingSource.DataSource = this.sPEEDDB;
            // 
            // sPEEDDB
            // 
            this.sPEEDDB.DataSetName = "SPEEDDB";
            this.sPEEDDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 受注残集計_得意先TableAdapter
            // 
            this.受注残集計_得意先TableAdapter.ClearBeforeFill = true;
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(680, 4);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 17;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(496, 4);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 23);
            this.ButtonF9.TabIndex = 16;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(588, 4);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 15;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // 受注残集計_得意先Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 262);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "受注残集計_得意先Form";
            this.Text = "受注残集計_得意先Form";
            this.Load += new System.EventHandler(this.受注残集計_得意先Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注残集計得意先BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private System.Windows.Forms.BindingSource 受注残集計得意先BindingSource;
        private SPEEDDB sPEEDDB;
        private 受注残集計_得意先Template 受注残集計_得意先Template1;
        private SPEEDDBTableAdapters.受注残集計_得意先TableAdapter 受注残集計_得意先TableAdapter;
        private System.Windows.Forms.Button ButtonF10;
        private System.Windows.Forms.Button ButtonF9;
        private System.Windows.Forms.Button ButtonF3;
    }
}