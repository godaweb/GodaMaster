﻿namespace db_test
{
    partial class 商品変換マスタメンテForm
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
            this.ButtonF5 = new System.Windows.Forms.Button();
            this.ButtonF10 = new System.Windows.Forms.Button();
            this.ButtonF9 = new System.Windows.Forms.Button();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.商品変換マスタメンテDataSet1 = new db_test.商品変換マスタメンテDataSet4();
            this.ButtonDEL = new System.Windows.Forms.Button();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.商品変換マスタメンテTemplate1 = new db_test.商品変換マスタメンテTemplate();
            ((System.ComponentModel.ISupportInitialize)(this.商品変換マスタメンテDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonF5
            // 
            this.ButtonF5.Location = new System.Drawing.Point(480, 8);
            this.ButtonF5.Name = "ButtonF5";
            this.ButtonF5.Size = new System.Drawing.Size(75, 23);
            this.ButtonF5.TabIndex = 17;
            this.ButtonF5.Text = "button1";
            this.ButtonF5.UseVisualStyleBackColor = true;
            this.ButtonF5.Visible = false;
            // 
            // ButtonF10
            // 
            this.ButtonF10.Location = new System.Drawing.Point(792, 8);
            this.ButtonF10.Name = "ButtonF10";
            this.ButtonF10.Size = new System.Drawing.Size(75, 23);
            this.ButtonF10.TabIndex = 16;
            this.ButtonF10.Text = "button1";
            this.ButtonF10.UseVisualStyleBackColor = true;
            this.ButtonF10.Visible = false;
            // 
            // ButtonF9
            // 
            this.ButtonF9.Location = new System.Drawing.Point(668, 8);
            this.ButtonF9.Name = "ButtonF9";
            this.ButtonF9.Size = new System.Drawing.Size(75, 23);
            this.ButtonF9.TabIndex = 15;
            this.ButtonF9.Text = "button1";
            this.ButtonF9.UseVisualStyleBackColor = true;
            this.ButtonF9.Visible = false;
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(320, 8);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 14;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // 商品変換マスタメンテDataSet1
            // 
            this.商品変換マスタメンテDataSet1.DataSetName = "商品変換マスタメンテDataSet4";
            this.商品変換マスタメンテDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ButtonDEL
            // 
            this.ButtonDEL.Location = new System.Drawing.Point(912, 8);
            this.ButtonDEL.Name = "ButtonDEL";
            this.ButtonDEL.Size = new System.Drawing.Size(75, 23);
            this.ButtonDEL.TabIndex = 18;
            this.ButtonDEL.Text = "button1";
            this.ButtonDEL.UseVisualStyleBackColor = true;
            this.ButtonDEL.Visible = false;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(1068, 473);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.商品変換マスタメンテTemplate1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // 商品変換マスタメンテForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 473);
            this.Controls.Add(this.ButtonDEL);
            this.Controls.Add(this.ButtonF5);
            this.Controls.Add(this.ButtonF10);
            this.Controls.Add(this.ButtonF9);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow1);
            this.KeyPreview = true;
            this.Name = "商品変換マスタメンテForm";
            this.Text = "商品変換マスタメンテForm";
            this.Load += new System.EventHandler(this.商品変換マスタメンテForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.商品変換マスタメンテDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 商品変換マスタメンテTemplate 商品変換マスタメンテTemplate1;
        private System.Windows.Forms.Button ButtonF5;
        private System.Windows.Forms.Button ButtonF10;
        private System.Windows.Forms.Button ButtonF9;
        private System.Windows.Forms.Button ButtonF3;
        private db_test.商品変換マスタメンテDataSet4 商品変換マスタメンテDataSet1;
        private System.Windows.Forms.Button ButtonDEL;
    }
}