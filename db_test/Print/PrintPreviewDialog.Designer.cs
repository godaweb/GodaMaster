namespace db_test.Print
{
    partial class PrintPreviewDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPreviewDialog));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonZoomOriginal = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBoxZoom = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
            this.printPreview = new db_test.Print.PrintPreview();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPrint,
            this.toolStripSeparator1,
            this.toolStripButtonZoomOriginal,
            this.toolStripComboBoxZoom,
            this.toolStripSeparator2,
            this.toolStripButtonClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(896, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonPrint
            // 
            this.toolStripButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrint.Image = global::db_test.Properties.Resources.printer;
            this.toolStripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrint.Name = "toolStripButtonPrint";
            this.toolStripButtonPrint.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPrint.Text = "toolStripButton1";
            this.toolStripButtonPrint.Click += new System.EventHandler(this.OnPrintButtonClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonZoomOriginal
            // 
            this.toolStripButtonZoomOriginal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZoomOriginal.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomOriginal.Image")));
            this.toolStripButtonZoomOriginal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomOriginal.Name = "toolStripButtonZoomOriginal";
            this.toolStripButtonZoomOriginal.Size = new System.Drawing.Size(39, 22);
            this.toolStripButtonZoomOriginal.Text = "100%";
            this.toolStripButtonZoomOriginal.Click += new System.EventHandler(this.OnZoomOriginalButtonClicked);
            // 
            // toolStripComboBoxZoom
            // 
            this.toolStripComboBoxZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxZoom.DropDownWidth = 100;
            this.toolStripComboBoxZoom.Items.AddRange(new object[] {
            "25%",
            "50%",
            "100%",
            "200%",
            "300%"});
            this.toolStripComboBoxZoom.Name = "toolStripComboBoxZoom";
            this.toolStripComboBoxZoom.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxZoom.SelectedIndexChanged += new System.EventHandler(this.OnZoomSelectedChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonClose
            // 
            this.toolStripButtonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClose.Image")));
            this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClose.Name = "toolStripButtonClose";
            this.toolStripButtonClose.Size = new System.Drawing.Size(41, 22);
            this.toolStripButtonClose.Text = "閉じる";
            this.toolStripButtonClose.Click += new System.EventHandler(this.OnCloseButtonClicked);
            // 
            // printPreview
            // 
            this.printPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreview.Location = new System.Drawing.Point(0, 25);
            this.printPreview.Name = "printPreview";
            this.printPreview.Size = new System.Drawing.Size(896, 656);
            this.printPreview.TabIndex = 2;
            // 
            // PrintPreviewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 681);
            this.Controls.Add(this.printPreview);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PrintPreviewDialog";
            this.Text = "印刷プレビュー";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonClose;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOriginal;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxZoom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private PrintPreview printPreview;
    }
}