namespace db_test
{
    partial class 出荷可能一覧表Form
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
            this.出荷可能一覧表Template1 = new db_test.出荷可能一覧表Template();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.出荷可能一覧表_得意先順CrystalReport1 = new db_test.出荷可能一覧表_得意先順CrystalReport();
            this.出荷可能一覧表CrystalReport1 = new db_test.出荷可能一覧表CrystalReport();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(642, 357);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.出荷可能一覧表Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(496, 12);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 10;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // 出荷可能一覧表Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 357);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow1);
            this.KeyPreview = true;
            this.Name = "出荷可能一覧表Form";
            this.Text = "出荷可能一覧表Form";
            this.Load += new System.EventHandler(this.出荷可能一覧表Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 出荷可能一覧表Template 出荷可能一覧表Template1;
        private System.Windows.Forms.Button ButtonF3;
        private 出荷可能一覧表_得意先順CrystalReport 出荷可能一覧表_得意先順CrystalReport1;
        private 出荷可能一覧表CrystalReport 出荷可能一覧表CrystalReport1;
    }
}