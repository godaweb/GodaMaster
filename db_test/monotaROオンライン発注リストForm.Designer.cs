namespace db_test
{
    partial class monotaROオンライン発注リストForm
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
            this.monotaROオンライン発注リストTemplate1 = new db_test.monotaROオンライン発注リストTemplate();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(640, 276);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.monotaROオンライン発注リストTemplate1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // monotaROオンライン発注リストForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 276);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "monotaROオンライン発注リストForm";
            this.Text = "monotaROオンライン発注リストForm";
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private monotaROオンライン発注リストTemplate monotaROオンライン発注リストTemplate1;
    }
}