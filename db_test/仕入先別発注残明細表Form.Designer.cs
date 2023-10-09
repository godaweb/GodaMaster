namespace db_test
{
    partial class 仕入先別発注残明細表Form
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
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border5 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border6 = new GrapeCity.Win.MultiRow.Border();
            this.sPEEDDB = new db_test.SPEEDDB();
            this.t部課マスタBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.t部課マスタTableAdapter = new db_test.SPEEDDBTableAdapters.T部課マスタTableAdapter();
            this.tableAdapterManager = new db_test.SPEEDDBTableAdapters.TableAdapterManager();
            this.仕入先別発注残明細表CrystalReport1 = new db_test.仕入先別発注残明細表CrystalReport();
            this.t商品マスタTableAdapter1 = new db_test.SPEEDDBDataSetTableAdapters.T商品マスタTableAdapter();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.仕入先別発注残明細表Template1 = new db_test.仕入先別発注残明細表Template();
            this.ButtonF3 = new System.Windows.Forms.Button();
            this.textBoxCell22 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.textBoxCell23 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.textBoxCell24 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.textBoxCell1 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.textBoxCell2 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.textBoxCell3 = new GrapeCity.Win.MultiRow.TextBoxCell();
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.t部課マスタBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // sPEEDDB
            // 
            this.sPEEDDB.DataSetName = "SPEEDDB";
            this.sPEEDDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // t部課マスタBindingSource
            // 
            this.t部課マスタBindingSource.DataMember = "T部課マスタ";
            this.t部課マスタBindingSource.DataSource = this.sPEEDDB;
            // 
            // t部課マスタTableAdapter
            // 
            this.t部課マスタTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.T部課マスタTableAdapter = this.t部課マスタTableAdapter;
            this.tableAdapterManager.UpdateOrder = db_test.SPEEDDBTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.商品変換取込商品未登録TableAdapter = null;
            this.tableAdapterManager.商品変換取込変換商品未登録TableAdapter = null;
            this.tableAdapterManager.商品変換取込変換商品登録済TableAdapter = null;
            // 
            // t商品マスタTableAdapter1
            // 
            this.t商品マスタTableAdapter1.ClearBeforeFill = true;
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(635, 351);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.仕入先別発注残明細表Template1;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // ButtonF3
            // 
            this.ButtonF3.Location = new System.Drawing.Point(512, 8);
            this.ButtonF3.Name = "ButtonF3";
            this.ButtonF3.Size = new System.Drawing.Size(75, 23);
            this.ButtonF3.TabIndex = 10;
            this.ButtonF3.Text = "button1";
            this.ButtonF3.UseVisualStyleBackColor = true;
            this.ButtonF3.Visible = false;
            // 
            // textBoxCell22
            // 
            this.textBoxCell22.HighlightText = true;
            this.textBoxCell22.Location = new System.Drawing.Point(25, 294);
            this.textBoxCell22.Name = "textBoxCell22";
            this.textBoxCell22.Size = new System.Drawing.Size(40, 12);
            cellStyle1.BackColor = System.Drawing.SystemColors.Control;
            cellStyle1.Border = border1;
            cellStyle1.DisabledBackColor = System.Drawing.SystemColors.Control;
            cellStyle1.EditingBackColor = System.Drawing.SystemColors.Control;
            cellStyle1.PatternColor = System.Drawing.SystemColors.Control;
            cellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            this.textBoxCell22.Style = cellStyle1;
            this.textBoxCell22.TabIndex = 55;
            // 
            // textBoxCell23
            // 
            this.textBoxCell23.HighlightText = true;
            this.textBoxCell23.Location = new System.Drawing.Point(118, 294);
            this.textBoxCell23.Name = "textBoxCell23";
            this.textBoxCell23.Size = new System.Drawing.Size(40, 12);
            cellStyle2.BackColor = System.Drawing.SystemColors.Control;
            cellStyle2.Border = border2;
            cellStyle2.DisabledBackColor = System.Drawing.SystemColors.Control;
            cellStyle2.EditingBackColor = System.Drawing.SystemColors.Control;
            cellStyle2.PatternColor = System.Drawing.SystemColors.Control;
            cellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
            this.textBoxCell23.Style = cellStyle2;
            this.textBoxCell23.TabIndex = 56;
            // 
            // textBoxCell24
            // 
            this.textBoxCell24.HighlightText = true;
            this.textBoxCell24.Location = new System.Drawing.Point(283, 294);
            this.textBoxCell24.Name = "textBoxCell24";
            this.textBoxCell24.Size = new System.Drawing.Size(40, 12);
            cellStyle3.BackColor = System.Drawing.SystemColors.Control;
            cellStyle3.Border = border3;
            cellStyle3.DisabledBackColor = System.Drawing.SystemColors.Control;
            cellStyle3.EditingBackColor = System.Drawing.SystemColors.Control;
            cellStyle3.PatternColor = System.Drawing.SystemColors.Control;
            cellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
            this.textBoxCell24.Style = cellStyle3;
            this.textBoxCell24.TabIndex = 57;
            // 
            // textBoxCell1
            // 
            this.textBoxCell1.HighlightText = true;
            this.textBoxCell1.Location = new System.Drawing.Point(25, 294);
            this.textBoxCell1.Name = "textBoxCell1";
            this.textBoxCell1.Size = new System.Drawing.Size(40, 12);
            cellStyle4.BackColor = System.Drawing.SystemColors.Control;
            cellStyle4.Border = border4;
            cellStyle4.DisabledBackColor = System.Drawing.SystemColors.Control;
            cellStyle4.EditingBackColor = System.Drawing.SystemColors.Control;
            cellStyle4.PatternColor = System.Drawing.SystemColors.Control;
            cellStyle4.SelectionBackColor = System.Drawing.SystemColors.Control;
            this.textBoxCell1.Style = cellStyle4;
            this.textBoxCell1.TabIndex = 55;
            // 
            // textBoxCell2
            // 
            this.textBoxCell2.HighlightText = true;
            this.textBoxCell2.Location = new System.Drawing.Point(118, 294);
            this.textBoxCell2.Name = "textBoxCell2";
            this.textBoxCell2.Size = new System.Drawing.Size(40, 12);
            cellStyle5.BackColor = System.Drawing.SystemColors.Control;
            cellStyle5.Border = border5;
            cellStyle5.DisabledBackColor = System.Drawing.SystemColors.Control;
            cellStyle5.EditingBackColor = System.Drawing.SystemColors.Control;
            cellStyle5.PatternColor = System.Drawing.SystemColors.Control;
            cellStyle5.SelectionBackColor = System.Drawing.SystemColors.Control;
            this.textBoxCell2.Style = cellStyle5;
            this.textBoxCell2.TabIndex = 56;
            // 
            // textBoxCell3
            // 
            this.textBoxCell3.HighlightText = true;
            this.textBoxCell3.Location = new System.Drawing.Point(283, 294);
            this.textBoxCell3.Name = "textBoxCell3";
            this.textBoxCell3.Size = new System.Drawing.Size(40, 12);
            cellStyle6.BackColor = System.Drawing.SystemColors.Control;
            cellStyle6.Border = border6;
            cellStyle6.DisabledBackColor = System.Drawing.SystemColors.Control;
            cellStyle6.EditingBackColor = System.Drawing.SystemColors.Control;
            cellStyle6.PatternColor = System.Drawing.SystemColors.Control;
            cellStyle6.SelectionBackColor = System.Drawing.SystemColors.Control;
            this.textBoxCell3.Style = cellStyle6;
            this.textBoxCell3.TabIndex = 57;
            // 
            // 仕入先別発注残明細表Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(635, 351);
            this.Controls.Add(this.ButtonF3);
            this.Controls.Add(this.gcMultiRow1);
            this.KeyPreview = true;
            this.Name = "仕入先別発注残明細表Form";
            this.Text = "仕入先別発注残明細表Form";
            this.Load += new System.EventHandler(this.仕入先別発注残明細表Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sPEEDDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.t部課マスタBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SPEEDDB sPEEDDB;
        private System.Windows.Forms.BindingSource t部課マスタBindingSource;
        private SPEEDDBTableAdapters.T部課マスタTableAdapter t部課マスタTableAdapter;
        private SPEEDDBTableAdapters.TableAdapterManager tableAdapterManager;
        private 仕入先別発注残明細表CrystalReport 仕入先別発注残明細表CrystalReport1;
        private SPEEDDBDataSetTableAdapters.T商品マスタTableAdapter t商品マスタTableAdapter1;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private 仕入先別発注残明細表Template 仕入先別発注残明細表Template1;
        private System.Windows.Forms.Button ButtonF3;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell22;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell23;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell24;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell1;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell2;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell3;

    }
}