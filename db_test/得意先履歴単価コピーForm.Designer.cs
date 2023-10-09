namespace db_test
{
    partial class 得意先履歴単価コピーForm
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
            this.得意先履歴単価コピーTemplate1 = new db_test.得意先履歴単価コピーTemplate();
            this.得意先履歴単価コピーDataSet1 = new db_test.得意先履歴単価コピーDataSet();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.得意先履歴単価コピーDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(613, 358);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.得意先履歴単価コピーTemplate1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 得意先履歴単価コピーDataSet1
            // 
            this.得意先履歴単価コピーDataSet1.DataSetName = "得意先履歴単価コピーDataSet";
            this.得意先履歴単価コピーDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 得意先履歴単価コピーForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 358);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "得意先履歴単価コピーForm";
            this.Text = "得意先履歴単価コピーForm";
            this.Load += new System.EventHandler(this.得意先履歴単価コピーForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.得意先履歴単価コピーDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 得意先履歴単価コピーTemplate 得意先履歴単価コピーTemplate1;
        private 得意先履歴単価コピーDataSet 得意先履歴単価コピーDataSet1;
    }
}