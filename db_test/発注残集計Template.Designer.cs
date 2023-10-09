namespace db_test
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class 発注残集計Template
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
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.ThreeDBorder threeDBorder1 = new GrapeCity.Win.MultiRow.ThreeDBorder();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border5 = new GrapeCity.Win.MultiRow.Border();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.終了buttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.labelCell12 = new GrapeCity.Win.MultiRow.LabelCell();
            this.labelCell3 = new GrapeCity.Win.MultiRow.LabelCell();
            this.担当者checkBoxCell = new GrapeCity.Win.MultiRow.CheckBoxCell();
            this.仕入先checkBoxCell = new GrapeCity.Win.MultiRow.CheckBoxCell();
            this.商品checkBoxCell = new GrapeCity.Win.MultiRow.CheckBoxCell();
            // 
            // Row
            // 
            this.Row.Height = 1;
            this.Row.Width = 432;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.終了buttonCell);
            this.columnHeaderSection1.Cells.Add(this.labelCell12);
            this.columnHeaderSection1.Cells.Add(this.labelCell3);
            this.columnHeaderSection1.Cells.Add(this.担当者checkBoxCell);
            this.columnHeaderSection1.Cells.Add(this.仕入先checkBoxCell);
            this.columnHeaderSection1.Cells.Add(this.商品checkBoxCell);
            this.columnHeaderSection1.Height = 174;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.ReadOnly = false;
            this.columnHeaderSection1.Selectable = true;
            this.columnHeaderSection1.Width = 432;
            // 
            // 終了buttonCell
            // 
            this.終了buttonCell.Location = new System.Drawing.Point(287, 119);
            this.終了buttonCell.Name = "終了buttonCell";
            this.終了buttonCell.Size = new System.Drawing.Size(80, 24);
            cellStyle1.BackColor = System.Drawing.SystemColors.Control;
            cellStyle1.Border = border1;
            cellStyle1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle1.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle1.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.終了buttonCell.Style = cellStyle1;
            this.終了buttonCell.TabIndex = 0;
            this.終了buttonCell.Value = "終了";
            // 
            // labelCell12
            // 
            this.labelCell12.Location = new System.Drawing.Point(0, 2);
            this.labelCell12.Name = "labelCell12";
            this.labelCell12.Size = new System.Drawing.Size(432, 29);
            cellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            cellStyle2.Border = border2;
            cellStyle2.Font = new System.Drawing.Font("ＭＳ ゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            cellStyle2.UseCompatibleTextRendering = GrapeCity.Win.MultiRow.MultiRowTriState.True;
            this.labelCell12.Style = cellStyle2;
            this.labelCell12.TabIndex = 1;
            this.labelCell12.TabStop = false;
            this.labelCell12.Value = "発注残集計";
            // 
            // labelCell3
            // 
            this.labelCell3.Location = new System.Drawing.Point(15, 70);
            this.labelCell3.Name = "labelCell3";
            this.labelCell3.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.Horizontal;
            this.labelCell3.Selectable = false;
            this.labelCell3.Size = new System.Drawing.Size(85, 22);
            cellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            cellStyle3.Border = threeDBorder1;
            cellStyle3.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelCell3.Style = cellStyle3;
            this.labelCell3.TabIndex = 2;
            this.labelCell3.TabStop = false;
            this.labelCell3.Value = "集計単位";
            // 
            // 担当者checkBoxCell
            // 
            this.担当者checkBoxCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.担当者checkBoxCell.Location = new System.Drawing.Point(120, 72);
            this.担当者checkBoxCell.Name = "担当者checkBoxCell";
            cellStyle4.Border = border3;
            cellStyle4.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle4.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.担当者checkBoxCell.Style = cellStyle4;
            this.担当者checkBoxCell.TabIndex = 3;
            this.担当者checkBoxCell.Text = "担当者";
            // 
            // 仕入先checkBoxCell
            // 
            this.仕入先checkBoxCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.仕入先checkBoxCell.Location = new System.Drawing.Point(210, 72);
            this.仕入先checkBoxCell.Name = "仕入先checkBoxCell";
            cellStyle5.Border = border4;
            cellStyle5.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle5.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.仕入先checkBoxCell.Style = cellStyle5;
            this.仕入先checkBoxCell.TabIndex = 4;
            this.仕入先checkBoxCell.Text = "仕入先";
            // 
            // 商品checkBoxCell
            // 
            this.商品checkBoxCell.FalseValue = "0";
            this.商品checkBoxCell.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.商品checkBoxCell.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.商品checkBoxCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.商品checkBoxCell.Location = new System.Drawing.Point(305, 72);
            this.商品checkBoxCell.Name = "商品checkBoxCell";
            this.商品checkBoxCell.Size = new System.Drawing.Size(80, 25);
            cellStyle6.Border = border5;
            cellStyle6.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle6.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.商品checkBoxCell.Style = cellStyle6;
            this.商品checkBoxCell.TabIndex = 5;
            this.商品checkBoxCell.Text = "商品";
            this.商品checkBoxCell.TrueValue = "1";
            this.商品checkBoxCell.Value = true;
            // 
            // 発注残集計Template
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 175;
            this.Width = 432;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.ButtonCell 終了buttonCell;
        private GrapeCity.Win.MultiRow.LabelCell labelCell12;
        private GrapeCity.Win.MultiRow.LabelCell labelCell3;
        private GrapeCity.Win.MultiRow.CheckBoxCell 担当者checkBoxCell;
        private GrapeCity.Win.MultiRow.CheckBoxCell 仕入先checkBoxCell;
        private GrapeCity.Win.MultiRow.CheckBoxCell 商品checkBoxCell;
    }
}
