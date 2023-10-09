namespace db_test
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class 受注入力_ドラスタ対応_単価更新Template
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
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.戻るbuttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.単価更新フラグcheckBoxCell = new GrapeCity.Win.MultiRow.CheckBoxCell();
            this.labelCell1 = new GrapeCity.Win.MultiRow.LabelCell();
            // 
            // Row
            // 
            this.Row.Height = 1;
            this.Row.Width = 358;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.戻るbuttonCell);
            this.columnHeaderSection1.Cells.Add(this.単価更新フラグcheckBoxCell);
            this.columnHeaderSection1.Cells.Add(this.labelCell1);
            this.columnHeaderSection1.Height = 145;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.ReadOnly = false;
            this.columnHeaderSection1.Selectable = true;
            this.columnHeaderSection1.Width = 358;
            // 
            // 戻るbuttonCell
            // 
            this.戻るbuttonCell.Location = new System.Drawing.Point(237, 101);
            this.戻るbuttonCell.Name = "戻るbuttonCell";
            this.戻るbuttonCell.ReadOnly = false;
            this.戻るbuttonCell.TabIndex = 1;
            this.戻るbuttonCell.Value = "戻る";
            // 
            // 単価更新フラグcheckBoxCell
            // 
            this.単価更新フラグcheckBoxCell.FalseValue = "0";
            this.単価更新フラグcheckBoxCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.単価更新フラグcheckBoxCell.Location = new System.Drawing.Point(58, 56);
            this.単価更新フラグcheckBoxCell.Name = "単価更新フラグcheckBoxCell";
            cellStyle1.Border = border1;
            cellStyle1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.単価更新フラグcheckBoxCell.Style = cellStyle1;
            this.単価更新フラグcheckBoxCell.TabIndex = 0;
            this.単価更新フラグcheckBoxCell.Text = "する";
            this.単価更新フラグcheckBoxCell.TrueValue = "-1";
            // 
            // labelCell1
            // 
            this.labelCell1.Location = new System.Drawing.Point(58, 24);
            this.labelCell1.Name = "labelCell1";
            this.labelCell1.Selectable = false;
            this.labelCell1.Size = new System.Drawing.Size(129, 21);
            cellStyle2.Border = border2;
            cellStyle2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.labelCell1.Style = cellStyle2;
            this.labelCell1.TabIndex = 2;
            this.labelCell1.Value = "単価更新しますか？";
            // 
            // 受注入力_ドラスタ対応_単価更新Template
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 146;
            this.Width = 358;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.ButtonCell 戻るbuttonCell;
        private GrapeCity.Win.MultiRow.CheckBoxCell 単価更新フラグcheckBoxCell;
        private GrapeCity.Win.MultiRow.LabelCell labelCell1;
    }
}
