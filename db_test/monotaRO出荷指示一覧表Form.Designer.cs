namespace db_test
{
    partial class monotaRO出荷指示一覧表Form
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
            this.monotaRO出荷指示一覧表Template1 = new db_test.monotaRO出荷指示一覧表Template();
            this.ButtonF3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(641, 479);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.monotaRO出荷指示一覧表Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(516, 8);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 10;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // monotaRO出荷指示一覧表Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 479);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "monotaRO出荷指示一覧表Form";
            this.Text = "monotaRO出荷指示一覧表Form";
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private monotaRO出荷指示一覧表Template monotaRO出荷指示一覧表Template1;
        private System.Windows.Forms.Button ButtonF3;
    }
}