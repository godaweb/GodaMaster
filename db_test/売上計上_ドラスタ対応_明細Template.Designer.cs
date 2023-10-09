namespace db_test
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class 売上計上_ドラスタ対応_明細Template
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region MultiRow Template Designer generated code

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.ThreeDBorder threeDBorder1 = new GrapeCity.Win.MultiRow.ThreeDBorder();
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.大分類コードtextBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.labelCell3 = new GrapeCity.Win.MultiRow.LabelCell();
            this.戻るbuttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.textBoxCell30 = new GrapeCity.Win.MultiRow.TextBoxCell();
            // 
            // Row
            // 
            this.Row.Height = 1;
            this.Row.Width = 307;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.大分類コードtextBoxCell);
            this.columnHeaderSection1.Cells.Add(this.labelCell3);
            this.columnHeaderSection1.Cells.Add(this.戻るbuttonCell);
            this.columnHeaderSection1.Cells.Add(this.textBoxCell30);
            this.columnHeaderSection1.Height = 140;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.ReadOnly = false;
            this.columnHeaderSection1.Selectable = true;
            this.columnHeaderSection1.Width = 307;
            // 
            // 大分類コードtextBoxCell
            // 
            this.大分類コードtextBoxCell.Location = new System.Drawing.Point(151, 30);
            this.大分類コードtextBoxCell.Name = "大分類コードtextBoxCell";
            this.大分類コードtextBoxCell.Size = new System.Drawing.Size(40, 21);
            cellStyle1.BackColor = System.Drawing.Color.White;
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle1.Border = border1;
            cellStyle1.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.大分類コードtextBoxCell.Style = cellStyle1;
            this.大分類コードtextBoxCell.TabIndex = 0;
            // 
            // labelCell3
            // 
            this.labelCell3.Location = new System.Drawing.Point(36, 30);
            this.labelCell3.Name = "labelCell3";
            this.labelCell3.Selectable = false;
            this.labelCell3.Size = new System.Drawing.Size(104, 21);
            cellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            cellStyle2.Border = threeDBorder1;
            cellStyle2.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.labelCell3.Style = cellStyle2;
            this.labelCell3.TabIndex = 2;
            this.labelCell3.TabStop = false;
            this.labelCell3.Value = "大分類コード";
            // 
            // 戻るbuttonCell
            // 
            this.戻るbuttonCell.Location = new System.Drawing.Point(191, 90);
            this.戻るbuttonCell.Name = "戻るbuttonCell";
            this.戻るbuttonCell.ReadOnly = false;
            this.戻るbuttonCell.TabIndex = 1;
            this.戻るbuttonCell.Value = "戻る";
            // 
            // textBoxCell30
            // 
            this.textBoxCell30.HighlightText = true;
            this.textBoxCell30.Location = new System.Drawing.Point(151, 90);
            this.textBoxCell30.Name = "textBoxCell30";
            this.textBoxCell30.Size = new System.Drawing.Size(10, 10);
            cellStyle3.BackColor = System.Drawing.SystemColors.Control;
            cellStyle3.Border = border2;
            cellStyle3.DisabledBackColor = System.Drawing.SystemColors.Control;
            cellStyle3.EditingBackColor = System.Drawing.SystemColors.Control;
            cellStyle3.PatternColor = System.Drawing.SystemColors.Control;
            cellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
            this.textBoxCell30.Style = cellStyle3;
            this.textBoxCell30.TabIndex = 3;
            // 
            // 売上計上_ドラスタ対応_明細Template
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 141;
            this.Width = 307;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.TextBoxCell 大分類コードtextBoxCell;
        private GrapeCity.Win.MultiRow.LabelCell labelCell3;
        private GrapeCity.Win.MultiRow.ButtonCell 戻るbuttonCell;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell30;
    }
}
