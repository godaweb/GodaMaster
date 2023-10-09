namespace db_test
{
    partial class 受注明細参照Form
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
            this.ButtonF9 = new System.Windows.Forms.Button();
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.ButtonF5 = new System.Windows.Forms.Button();
            this.ButtonF4 = new System.Windows.Forms.Button();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.受注明細参照Template1 = new db_test.受注明細参照Template();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(756, 4);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 23);
            this.ButtonF9.TabIndex = 6;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(840, 4);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 5;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // ButtonF5
            // 
            this.ButtonF5.Location = new System.Drawing.Point(680, 4);
            this.ButtonF5.Name = "ButtonF5";
            this.ButtonF5.Size = new System.Drawing.Size(75, 23);
            this.ButtonF5.TabIndex = 7;
            this.ButtonF5.Text = "button1";
            this.ButtonF5.UseVisualStyleBackColor = true;
            this.ButtonF5.Visible = false;
            // 
            // ButtonF4
            // 
            this.ButtonF4.Location = new System.Drawing.Point(600, 4);
            this.ButtonF4.Name = "ButtonF4";
            this.ButtonF4.Size = new System.Drawing.Size(75, 23);
            this.ButtonF4.TabIndex = 8;
            this.ButtonF4.Text = "button1";
            this.ButtonF4.UseVisualStyleBackColor = true;
            this.ButtonF4.Visible = false;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(519, 4);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 9;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(1175, 562);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.受注明細参照Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            this.gcMultiRow1.CellContentClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow1_CellContentClick);
            // 
            // 受注明細参照Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 562);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.ButtonF4);
            this.Controls.Add(this.ButtonF5);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.gcMultiRow1);
            this.KeyPreview = true;
            this.Name = "受注明細参照Form";
            this.Text = "受注明細参照Form";
            this.Load += new System.EventHandler(this.受注明細参照Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 受注明細参照Template 受注明細参照Template1;
        private System.Windows.Forms.Button ButtonF9;
        private System.Windows.Forms.Button ButtonF10;
        private System.Windows.Forms.Button ButtonF5;
        private System.Windows.Forms.Button ButtonF4;
        private System.Windows.Forms.Button ButtonF3;
    }
}