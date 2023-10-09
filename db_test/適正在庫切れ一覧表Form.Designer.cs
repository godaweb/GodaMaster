namespace db_test
{
    partial class 適正在庫切れ一覧表Form
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
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.適正在庫切れ一覧表Template1 = new db_test.適正在庫切れ一覧表Template();
            this.textBoxCell1 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.textBoxCell2 = new GrapeCity.Win.MultiRow.TextBoxCell();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(636, 266);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.適正在庫切れ一覧表Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // textBoxCell1
            // 
            this.textBoxCell1.HighlightText = true;
            this.textBoxCell1.Location = new System.Drawing.Point(6, 52);
            this.textBoxCell1.Name = "textBoxCell1";
            this.textBoxCell1.Size = new System.Drawing.Size(10, 21);
            cellStyle1.BackColor = System.Drawing.SystemColors.Control;
            cellStyle1.Border = border1;
            cellStyle1.DisabledBackColor = System.Drawing.SystemColors.Control;
            cellStyle1.EditingBackColor = System.Drawing.SystemColors.Control;
            this.textBoxCell1.Style = cellStyle1;
            this.textBoxCell1.TabIndex = 45;
            // 
            // textBoxCell2
            // 
            this.textBoxCell2.HighlightText = true;
            this.textBoxCell2.Location = new System.Drawing.Point(6, 86);
            this.textBoxCell2.Name = "textBoxCell2";
            this.textBoxCell2.Size = new System.Drawing.Size(10, 21);
            cellStyle2.BackColor = System.Drawing.SystemColors.Control;
            cellStyle2.Border = border2;
            cellStyle2.DisabledBackColor = System.Drawing.SystemColors.Control;
            cellStyle2.EditingBackColor = System.Drawing.SystemColors.Control;
            this.textBoxCell2.Style = cellStyle2;
            this.textBoxCell2.TabIndex = 46;
            // 
            // 適正在庫切れ一覧表Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 266);
            this.Controls.Add(this.gcMultiRow1);
            this.KeyPreview = true;
            this.Name = "適正在庫切れ一覧表Form";
            this.Text = "適正在庫切れ一覧表Form";
            this.Load += new System.EventHandler(this.適正在庫切れ一覧表Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 適正在庫切れ一覧表Template 適正在庫切れ一覧表Template1;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell1;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell2;
    }
}