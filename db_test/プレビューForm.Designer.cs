namespace db_test
{
    partial class プレビューForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(プレビューForm));
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.仕入先別発注残明細表CrystalReport1 = new db_test.仕入先別発注残明細表CrystalReport();
            this.仕入先別発注残明細表TableAdapter1 = new db_test.SPEEDDBTableAdapters.仕入先別発注残明細表TableAdapter();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = 0;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.ReportSource = this.仕入先別発注残明細表CrystalReport1;
            this.crystalReportViewer1.Size = new System.Drawing.Size(636, 505);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.Load += new System.EventHandler(this.crystalReportViewer1_Load);

            // 
            // 仕入先別発注残明細表TableAdapter1
            // 
            this.仕入先別発注残明細表TableAdapter1.ClearBeforeFill = true;

            // 
            // プレビューForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 505);
            this.Controls.Add(this.crystalReportViewer1);
            this.Name = "プレビューForm";
            this.Text = "プレビューForm";
            this.Load += new System.EventHandler(this.プレビューForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private 仕入先別発注残明細表CrystalReport 仕入先別発注残明細表CrystalReport1;
        private SPEEDDBTableAdapters.仕入先別発注残明細表TableAdapter 仕入先別発注残明細表TableAdapter1;
    }
}