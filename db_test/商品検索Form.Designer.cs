namespace db_test
{
    partial class 商品検索Form
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
            GrapeCity.Win.MultiRow.ShortcutKeyManager shortcutKeyManager1 = new GrapeCity.Win.MultiRow.ShortcutKeyManager();
            this.ButtonF5 = new System.Windows.Forms.Button();
            this.商品マスタ検索BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPEEDDBBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPEEDDB = new db_test.SPEEDDB();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.商品マスタ検索TableAdapter1 = new db_test.SPEEDDBTableAdapters.商品マスタ検索TableAdapter();
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.ButtonF9 = new System.Windows.Forms.Button();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.商品検索Template1 = new db_test.商品検索Template();
            ((System.ComponentModel.ISupportInitialize)(this.商品マスタ検索BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDBBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonF5
            // 
            this.ButtonF5.Location = new System.Drawing.Point(492, 0);
            this.ButtonF5.Name = "ButtonF5";
            this.ButtonF5.Size = new System.Drawing.Size(75, 23);
            this.ButtonF5.TabIndex = 6;
            this.ButtonF5.Text = "button1";
            this.ButtonF5.UseVisualStyleBackColor = true;
            this.ButtonF5.Visible = false;
            // 
            // 商品マスタ検索BindingSource
            // 
            this.商品マスタ検索BindingSource.DataMember = "商品マスタ検索";
            this.商品マスタ検索BindingSource.DataSource = this.sPEEDDBBindingSource;
            // 
            // sPEEDDBBindingSource
            // 
            this.sPEEDDBBindingSource.DataSource = this.sPEEDDB;
            this.sPEEDDBBindingSource.Position = 0;
            // 
            // sPEEDDB
            // 
            this.sPEEDDB.DataSetName = "SPEEDDB";
            this.sPEEDDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(336, 0);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 7;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // 商品マスタ検索TableAdapter1
            // 
            this.商品マスタ検索TableAdapter1.ClearBeforeFill = true;
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(776, 0);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 16;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(652, 0);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 23);
            this.ButtonF9.TabIndex = 15;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.AllowUserToDeleteRows = false;
            this.gcMultiRow1.AllowUserToResize = false;
            this.gcMultiRow1.AllowUserToZoom = false;
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.ShortcutKeyManager = shortcutKeyManager1;
            this.gcMultiRow1.Size = new System.Drawing.Size(984, 562);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.商品検索Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 商品検索Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.ButtonF5);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "商品検索Form";
            this.Text = "商品検索Form";
            this.Load += new System.EventHandler(this.商品検索Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.商品マスタ検索BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDBBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 商品検索Template 商品検索Template1;
        private System.Windows.Forms.Button ButtonF5;
        private SPEEDDBTableAdapters.商品マスタ検索TableAdapter 商品マスタ検索TableAdapter1;
        private System.Windows.Forms.BindingSource sPEEDDBBindingSource;
        private SPEEDDB sPEEDDB;
        private System.Windows.Forms.BindingSource 商品マスタ検索BindingSource;
        private System.Windows.Forms.Button ButtonF3;
        private System.Windows.Forms.Button ButtonF10;
        private System.Windows.Forms.Button ButtonF9;
    }
}