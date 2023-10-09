namespace db_test
{
    partial class EOSドラスタ送信データ作成Form
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
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.eoSドラスタ送信データ作成Template1 = new db_test.EOSドラスタ送信データ作成Template();
            this.wTドラスタ受注ファイルDataSet1 = new db_test.WTドラスタ受注ファイルDataSet();
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.ButtonF9 = new System.Windows.Forms.Button();
            this.ButtonF3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wTドラスタ受注ファイルDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(626, 492);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.eoSドラスタ送信データ作成Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // wTドラスタ受注ファイルDataSet1
            // 
            this.wTドラスタ受注ファイルDataSet1.DataSetName = "WTドラスタ受注ファイルDataSet";
            this.wTドラスタ受注ファイルDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(520, 8);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 14;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(336, 8);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 23);
            this.ButtonF9.TabIndex = 13;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(428, 8);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 12;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // EOSドラスタ送信データ作成Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 492);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "EOSドラスタ送信データ作成Form";
            this.Text = "EOSドラスタ送信データ作成Form";
            this.Load += new System.EventHandler(this.EOSドラスタ送信データ作成Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wTドラスタ受注ファイルDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private EOSドラスタ送信データ作成Template eoSドラスタ送信データ作成Template1;
        private WTドラスタ受注ファイルDataSet wTドラスタ受注ファイルDataSet1;
        private System.Windows.Forms.Button ButtonF10;
        private System.Windows.Forms.Button ButtonF9;
        private System.Windows.Forms.Button ButtonF3;
    }
}