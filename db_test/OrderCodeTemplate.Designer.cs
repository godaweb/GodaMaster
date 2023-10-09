namespace Demo.Layout.Order
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class OrderCodeTemplate
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
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.columnHeaderCell1 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell2 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell4 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell5 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.cornerHeaderCell1 = new GrapeCity.Win.MultiRow.CornerHeaderCell();
            this.numericUpDownCell1 = new GrapeCity.Win.MultiRow.NumericUpDownCell();
            this.textBoxCell1 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.textBoxCell3 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.textBoxCell4 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.rowHeaderCell1 = new GrapeCity.Win.MultiRow.RowHeaderCell();
            // 
            // Row
            // 
            this.Row.Cells.Add(this.numericUpDownCell1);
            this.Row.Cells.Add(this.textBoxCell1);
            this.Row.Cells.Add(this.textBoxCell3);
            this.Row.Cells.Add(this.textBoxCell4);
            this.Row.Cells.Add(this.rowHeaderCell1);
            this.Row.Height = 21;
            this.Row.Width = 540;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell1);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell2);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell4);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell5);
            this.columnHeaderSection1.Cells.Add(this.cornerHeaderCell1);
            this.columnHeaderSection1.Height = 21;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 540;
            // 
            // columnHeaderCell1
            // 
            this.columnHeaderCell1.Location = new System.Drawing.Point(40, 0);
            this.columnHeaderCell1.Name = "columnHeaderCell1";
            this.columnHeaderCell1.Size = new System.Drawing.Size(70, 21);
            this.columnHeaderCell1.TabIndex = 0;
            this.columnHeaderCell1.Value = "受注コード";
            // 
            // columnHeaderCell2
            // 
            this.columnHeaderCell2.Location = new System.Drawing.Point(110, 0);
            this.columnHeaderCell2.Name = "columnHeaderCell2";
            this.columnHeaderCell2.Size = new System.Drawing.Size(140, 21);
            this.columnHeaderCell2.TabIndex = 1;
            this.columnHeaderCell2.Value = "出荷先名";
            // 
            // columnHeaderCell4
            // 
            this.columnHeaderCell4.Location = new System.Drawing.Point(250, 0);
            this.columnHeaderCell4.Name = "columnHeaderCell4";
            this.columnHeaderCell4.Size = new System.Drawing.Size(110, 21);
            this.columnHeaderCell4.TabIndex = 2;
            this.columnHeaderCell4.Value = "出荷先都道府県";
            // 
            // columnHeaderCell5
            // 
            this.columnHeaderCell5.Location = new System.Drawing.Point(360, 0);
            this.columnHeaderCell5.Name = "columnHeaderCell5";
            this.columnHeaderCell5.Size = new System.Drawing.Size(180, 21);
            this.columnHeaderCell5.TabIndex = 3;
            this.columnHeaderCell5.Value = "出荷先住所1";
            // 
            // cornerHeaderCell1
            // 
            this.cornerHeaderCell1.Location = new System.Drawing.Point(0, 0);
            this.cornerHeaderCell1.Name = "cornerHeaderCell1";
            this.cornerHeaderCell1.Size = new System.Drawing.Size(40, 21);
            this.cornerHeaderCell1.TabIndex = 4;
            // 
            // numericUpDownCell1
            // 
            this.numericUpDownCell1.DataField = "受注コード";
            this.numericUpDownCell1.Location = new System.Drawing.Point(40, 0);
            this.numericUpDownCell1.Name = "numericUpDownCell1";
            this.numericUpDownCell1.ShowSpinButton = GrapeCity.Win.MultiRow.CellButtonVisibility.NotShown;
            this.numericUpDownCell1.Size = new System.Drawing.Size(70, 21);
            this.numericUpDownCell1.TabIndex = 0;
            // 
            // textBoxCell1
            // 
            this.textBoxCell1.DataField = "出荷先名";
            this.textBoxCell1.Location = new System.Drawing.Point(110, 0);
            this.textBoxCell1.Name = "textBoxCell1";
            this.textBoxCell1.Size = new System.Drawing.Size(140, 21);
            this.textBoxCell1.TabIndex = 1;
            // 
            // textBoxCell3
            // 
            this.textBoxCell3.DataField = "出荷先都道府県";
            this.textBoxCell3.Location = new System.Drawing.Point(250, 0);
            this.textBoxCell3.Name = "textBoxCell3";
            this.textBoxCell3.Size = new System.Drawing.Size(110, 21);
            this.textBoxCell3.TabIndex = 2;
            // 
            // textBoxCell4
            // 
            this.textBoxCell4.DataField = "出荷先住所1";
            this.textBoxCell4.Location = new System.Drawing.Point(360, 0);
            this.textBoxCell4.Name = "textBoxCell4";
            this.textBoxCell4.Size = new System.Drawing.Size(180, 21);
            this.textBoxCell4.TabIndex = 3;
            // 
            // rowHeaderCell1
            // 
            this.rowHeaderCell1.Location = new System.Drawing.Point(0, 0);
            this.rowHeaderCell1.Name = "rowHeaderCell1";
            this.rowHeaderCell1.Size = new System.Drawing.Size(40, 21);
            this.rowHeaderCell1.TabIndex = 4;
            // 
            // OrderCodeTemplate
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 42;
            this.Width = 540;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.NumericUpDownCell numericUpDownCell1;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell1;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell3;
        private GrapeCity.Win.MultiRow.TextBoxCell textBoxCell4;
        private GrapeCity.Win.MultiRow.RowHeaderCell rowHeaderCell1;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell1;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell2;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell4;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell5;
        private GrapeCity.Win.MultiRow.CornerHeaderCell cornerHeaderCell1;
    }
}
