namespace db_test
{
    partial class SelectOperatorDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectOperatorDialog));
            this.textBoxOperatorName = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxOperatorCode = new System.Windows.Forms.ComboBox();
            this.labelOperator = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.sPEEDDBDataSet1 = new db_test.SPEEDDBDataSet1();
            this.オペレーターマスタ選択BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.オペレーターマスタ選択TableAdapter = new db_test.SPEEDDBDataSet1TableAdapters.オペレーターマスタ選択TableAdapter();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDBDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.オペレーターマスタ選択BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxOperatorName
            // 
            this.textBoxOperatorName.Location = new System.Drawing.Point(188, 99);
            this.textBoxOperatorName.Name = "textBoxOperatorName";
            this.textBoxOperatorName.ReadOnly = true;
            this.textBoxOperatorName.Size = new System.Drawing.Size(198, 19);
            this.textBoxOperatorName.TabIndex = 0;
            this.textBoxOperatorName.TabStop = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(289, 139);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(97, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(186, 139);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(97, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnOkClicked);
            // 
            // comboBoxOperatorCode
            // 
            this.comboBoxOperatorCode.DataSource = this.オペレーターマスタ選択BindingSource;
            this.comboBoxOperatorCode.DisplayMember = "オペレーターコード";
            this.comboBoxOperatorCode.FormattingEnabled = true;
            this.comboBoxOperatorCode.Location = new System.Drawing.Point(113, 98);
            this.comboBoxOperatorCode.Name = "comboBoxOperatorCode";
            this.comboBoxOperatorCode.Size = new System.Drawing.Size(69, 20);
            this.comboBoxOperatorCode.TabIndex = 0;
            this.comboBoxOperatorCode.ValueMember = "オペレーター名";
            this.comboBoxOperatorCode.TextChanged += new System.EventHandler(this.OnOperatorCodeChanged);
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Location = new System.Drawing.Point(12, 102);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(91, 12);
            this.labelOperator.TabIndex = 5;
            this.labelOperator.Text = "オペレーター番号：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(398, 76);
            this.panel1.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::db_test.Properties.Resources.splash_logo;
            this.pictureBox1.Location = new System.Drawing.Point(14, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(159, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // sPEEDDBDataSet1
            // 
            this.sPEEDDBDataSet1.DataSetName = "SPEEDDBDataSet1";
            this.sPEEDDBDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // オペレーターマスタ選択BindingSource
            // 
            this.オペレーターマスタ選択BindingSource.DataMember = "オペレーターマスタ選択";
            this.オペレーターマスタ選択BindingSource.DataSource = this.sPEEDDBDataSet1;
            // 
            // オペレーターマスタ選択TableAdapter
            // 
            this.オペレーターマスタ選択TableAdapter.ClearBeforeFill = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(184, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 21);
            this.label1.TabIndex = 8;
            this.label1.Text = "販売情報システム";
            // 
            // SelectOperatorDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(398, 174);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelOperator);
            this.Controls.Add(this.comboBoxOperatorCode);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxOperatorName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectOperatorDialog";
            this.ShowIcon = false;
            this.Text = "ゴーダ - オペレータ番号入力";
            this.Load += new System.EventHandler(this.SelectOperatorDialog_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDBDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.オペレーターマスタ選択BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOperatorName;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox comboBoxOperatorCode;
        private System.Windows.Forms.Label labelOperator;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private SPEEDDBDataSet1 sPEEDDBDataSet1;
        private System.Windows.Forms.BindingSource オペレーターマスタ選択BindingSource;
        private SPEEDDBDataSet1TableAdapters.オペレーターマスタ選択TableAdapter オペレーターマスタ選択TableAdapter;
        private System.Windows.Forms.Label label1;
    }
}