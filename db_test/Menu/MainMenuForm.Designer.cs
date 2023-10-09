namespace db_test.Menu
{
    partial class MainMenuForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabelOperatorName = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.mainMenuCategory1 = new db_test.Menu.MainMenuCategory();
            this.elementHost2 = new System.Windows.Forms.Integration.ElementHost();
            this.mainMenuControl1 = new db_test.Menu.MainMenuControl();
            this.toolStripLabelCurrentTerm = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripLabelOperatorName,
            this.toolStripLabel2,
            this.toolStripLabelCurrentTerm});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolStrip1.Size = new System.Drawing.Size(560, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripLabel1.Image = global::db_test.Properties.Resources.user_green;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(88, 22);
            this.toolStripLabel1.Text = "オペレーター：";
            // 
            // toolStripLabelOperatorName
            // 
            this.toolStripLabelOperatorName.Name = "toolStripLabelOperatorName";
            this.toolStripLabelOperatorName.Size = new System.Drawing.Size(0, 22);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripLabel2.Image = global::db_test.Properties.Resources.calendar;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(59, 22);
            this.toolStripLabel2.Text = "当期：";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.elementHost1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.elementHost2);
            this.splitContainer.Size = new System.Drawing.Size(560, 367);
            this.splitContainer.SplitterDistance = 186;
            this.splitContainer.TabIndex = 3;
            this.splitContainer.TabStop = false;
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(186, 367);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.mainMenuCategory1;
            // 
            // elementHost2
            // 
            this.elementHost2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost2.Location = new System.Drawing.Point(0, 0);
            this.elementHost2.Name = "elementHost2";
            this.elementHost2.Size = new System.Drawing.Size(370, 367);
            this.elementHost2.TabIndex = 0;
            this.elementHost2.Text = "elementHost2";
            this.elementHost2.Child = this.mainMenuControl1;
            // 
            // toolStripLabelCurrentTerm
            // 
            this.toolStripLabelCurrentTerm.Name = "toolStripLabelCurrentTerm";
            this.toolStripLabelCurrentTerm.Size = new System.Drawing.Size(0, 22);
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 392);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenuForm";
            this.Text = "メインメニュー";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelOperatorName;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private MainMenuCategory mainMenuCategory1;
        private System.Windows.Forms.Integration.ElementHost elementHost2;
        private MainMenuControl mainMenuControl1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabelCurrentTerm;

    }
}