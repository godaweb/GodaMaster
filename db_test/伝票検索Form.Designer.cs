namespace db_test
{
    partial class 伝票検索Form
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
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.ButtonF9 = new System.Windows.Forms.Button();
            this.ButtonF5 = new System.Windows.Forms.Button();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.gcMultiRow4 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.伝票検索明細BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPEEDDB = new db_test.SPEEDDB();
            this.伝票検索ヘッダBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.伝票検索明細TableAdapter = new db_test.SPEEDDBTableAdapters.伝票検索明細TableAdapter();
            this.伝票検索ヘッダTableAdapter = new db_test.SPEEDDBTableAdapters.伝票検索ヘッダTableAdapter();
            this.伝票検索ヘッダBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.gcMultiRow3 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.伝票検索右Template1 = new db_test.伝票検索右Template();
            this.gcMultiRow2 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.伝票検索左Template1 = new db_test.伝票検索左Template();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.伝票検索_ヘッダTemplate1 = new db_test.伝票検索_ヘッダTemplate();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.伝票検索明細BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.伝票検索ヘッダBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.伝票検索ヘッダBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(520, 0);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 16);
            this.ButtonF10.TabIndex = 24;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(780, 0);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 16);
            this.ButtonF9.TabIndex = 23;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // ButtonF5
            // 
            this.ButtonF5.Location = new System.Drawing.Point(696, 0);
            this.ButtonF5.Name = "ButtonF5";
            this.ButtonF5.Size = new System.Drawing.Size(75, 16);
            this.ButtonF5.TabIndex = 22;
            this.ButtonF5.Text = "button1";
            this.ButtonF5.UseVisualStyleBackColor = true;
            this.ButtonF5.Visible = false;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(612, 0);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 16);
            this.ButtonF3.TabIndex = 21;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // gcMultiRow4
            // 
            this.gcMultiRow4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow4.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow4.Name = "gcMultiRow4";
            this.gcMultiRow4.Size = new System.Drawing.Size(869, 473);
            this.gcMultiRow4.TabIndex = 20;
            this.gcMultiRow4.Text = "gcMultiRow4";
            // 
            // 伝票検索明細BindingSource
            // 
            this.伝票検索明細BindingSource.DataMember = "伝票検索明細";
            this.伝票検索明細BindingSource.DataSource = this.sPEEDDB;
            // 
            // sPEEDDB
            // 
            this.sPEEDDB.DataSetName = "SPEEDDB";
            this.sPEEDDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 伝票検索ヘッダBindingSource
            // 
            this.伝票検索ヘッダBindingSource.DataMember = "伝票検索ヘッダ";
            this.伝票検索ヘッダBindingSource.DataSource = this.sPEEDDB;
            // 
            // 伝票検索明細TableAdapter
            // 
            this.伝票検索明細TableAdapter.ClearBeforeFill = true;
            // 
            // 伝票検索ヘッダTableAdapter
            // 
            this.伝票検索ヘッダTableAdapter.ClearBeforeFill = true;
            // 
            // 伝票検索ヘッダBindingSource1
            // 
            this.伝票検索ヘッダBindingSource1.DataMember = "伝票検索ヘッダ";
            this.伝票検索ヘッダBindingSource1.DataSource = this.sPEEDDB;
            // 
            // gcMultiRow3
            // 
            this.gcMultiRow3.AllowUserToAddRows = false;
            this.gcMultiRow3.AllowUserToDeleteRows = false;
            this.gcMultiRow3.DataSource = this.伝票検索明細BindingSource;
            this.gcMultiRow3.Location = new System.Drawing.Point(443, 132);
            this.gcMultiRow3.Name = "gcMultiRow3";
            this.gcMultiRow3.Size = new System.Drawing.Size(425, 344);
            this.gcMultiRow3.TabIndex = 2;
            this.gcMultiRow3.Template = this.伝票検索右Template1;
            this.gcMultiRow3.Text = "gcMultiRow3";
            // 
            // gcMultiRow2
            // 
            this.gcMultiRow2.AllowClipboard = false;
            this.gcMultiRow2.AllowUserToAddRows = false;
            this.gcMultiRow2.AllowUserToDeleteRows = false;
            this.gcMultiRow2.ClipboardCopyMode = GrapeCity.Win.MultiRow.ClipboardCopyMode.Disable;
            this.gcMultiRow2.DataSource = this.伝票検索ヘッダBindingSource;
            this.gcMultiRow2.EditMode = GrapeCity.Win.MultiRow.EditMode.EditOnEnter;
            this.gcMultiRow2.Location = new System.Drawing.Point(0, 132);
            this.gcMultiRow2.Name = "gcMultiRow2";
            this.gcMultiRow2.Size = new System.Drawing.Size(444, 344);
            this.gcMultiRow2.TabIndex = 1;
            this.gcMultiRow2.Template = this.伝票検索左Template1;
            this.gcMultiRow2.Text = "gcMultiRow2";
            this.gcMultiRow2.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Location = new System.Drawing.Point(-8, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(880, 132);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.伝票検索_ヘッダTemplate1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 伝票検索Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 473);
            this.Controls.Add(this.gcMultiRow3);
            this.Controls.Add(this.gcMultiRow2);
            this.Controls.Add(this.gcMultiRow1);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF5);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow4);
            this.Name = "伝票検索Form";
            this.Text = "伝票検索Form";
            this.Load += new System.EventHandler(this.伝票検索Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.伝票検索明細BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.伝票検索ヘッダBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.伝票検索ヘッダBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 伝票検索_ヘッダTemplate 伝票検索_ヘッダTemplate1;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow2;
        private System.Windows.Forms.BindingSource 伝票検索ヘッダBindingSource;
        private SPEEDDB sPEEDDB;
        private 伝票検索左Template 伝票検索左Template1;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow3;
        private System.Windows.Forms.BindingSource 伝票検索明細BindingSource;
        private 伝票検索右Template 伝票検索右Template1;
        private SPEEDDBTableAdapters.伝票検索明細TableAdapter 伝票検索明細TableAdapter;
        private SPEEDDBTableAdapters.伝票検索ヘッダTableAdapter 伝票検索ヘッダTableAdapter;
        private System.Windows.Forms.BindingSource 伝票検索ヘッダBindingSource1;
        private System.Windows.Forms.Button ButtonF10;
        private System.Windows.Forms.Button ButtonF9;
        private System.Windows.Forms.Button ButtonF5;
        private System.Windows.Forms.Button ButtonF3;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow4;
    }
}