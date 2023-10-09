namespace db_test
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class monotaRO回答データ作成Template
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
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.RectangleShapeRenderer rectangleShapeRenderer1 = new GrapeCity.Win.MultiRow.RectangleShapeRenderer();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.ThreeDBorder threeDBorder1 = new GrapeCity.Win.MultiRow.ThreeDBorder();
            GrapeCity.Win.MultiRow.CellStyle cellStyle7 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle8 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle9 = new GrapeCity.Win.MultiRow.CellStyle();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.labelCell12 = new GrapeCity.Win.MultiRow.LabelCell();
            this.CSV出力buttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.終了buttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.shapeCell1 = new GrapeCity.Win.MultiRow.ShapeCell();
            this.出力ファイル名textBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.labelCell6 = new GrapeCity.Win.MultiRow.LabelCell();
            this.ファイルオープンbuttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.プレビューbuttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.印刷buttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            // 
            // Row
            // 
            this.Row.Height = 1;
            this.Row.Width = 584;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.labelCell12);
            this.columnHeaderSection1.Cells.Add(this.CSV出力buttonCell);
            this.columnHeaderSection1.Cells.Add(this.終了buttonCell);
            this.columnHeaderSection1.Cells.Add(this.shapeCell1);
            this.columnHeaderSection1.Cells.Add(this.出力ファイル名textBoxCell);
            this.columnHeaderSection1.Cells.Add(this.labelCell6);
            this.columnHeaderSection1.Cells.Add(this.ファイルオープンbuttonCell);
            this.columnHeaderSection1.Cells.Add(this.プレビューbuttonCell);
            this.columnHeaderSection1.Cells.Add(this.印刷buttonCell);
            this.columnHeaderSection1.Height = 238;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 584;
            // 
            // labelCell12
            // 
            this.labelCell12.Location = new System.Drawing.Point(0, 0);
            this.labelCell12.Name = "labelCell12";
            this.labelCell12.Selectable = false;
            this.labelCell12.Size = new System.Drawing.Size(584, 29);
            cellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            cellStyle1.Border = border1;
            cellStyle1.Font = new System.Drawing.Font("ＭＳ ゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            cellStyle1.UseCompatibleTextRendering = GrapeCity.Win.MultiRow.MultiRowTriState.True;
            this.labelCell12.Style = cellStyle1;
            this.labelCell12.TabIndex = 0;
            this.labelCell12.TabStop = false;
            this.labelCell12.Value = "monotaRO回答データ作成";
            // 
            // CSV出力buttonCell
            // 
            this.CSV出力buttonCell.Location = new System.Drawing.Point(303, 99);
            this.CSV出力buttonCell.Name = "CSV出力buttonCell";
            cellStyle2.BackColor = System.Drawing.SystemColors.Control;
            cellStyle2.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle2.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle2.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.CSV出力buttonCell.Style = cellStyle2;
            this.CSV出力buttonCell.TabIndex = 1;
            this.CSV出力buttonCell.Value = "CSV出力";
            // 
            // 終了buttonCell
            // 
            this.終了buttonCell.Location = new System.Drawing.Point(462, 99);
            this.終了buttonCell.Name = "終了buttonCell";
            cellStyle3.BackColor = System.Drawing.SystemColors.Control;
            cellStyle3.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle3.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle3.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.終了buttonCell.Style = cellStyle3;
            this.終了buttonCell.TabIndex = 2;
            this.終了buttonCell.Value = "終了";
            // 
            // shapeCell1
            // 
            this.shapeCell1.Location = new System.Drawing.Point(168, 155);
            this.shapeCell1.Name = "shapeCell1";
            this.shapeCell1.Renderer = rectangleShapeRenderer1;
            this.shapeCell1.Size = new System.Drawing.Size(243, 61);
            cellStyle4.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle4.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle4.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.TopCenter;
            cellStyle4.TextEffect = GrapeCity.Win.MultiRow.TextEffect.Flat;
            this.shapeCell1.Style = cellStyle4;
            this.shapeCell1.TabIndex = 3;
            this.shapeCell1.Value = "***チェックリスト***";
            // 
            // 出力ファイル名textBoxCell
            // 
            this.出力ファイル名textBoxCell.HighlightText = true;
            this.出力ファイル名textBoxCell.Location = new System.Drawing.Point(128, 56);
            this.出力ファイル名textBoxCell.Name = "出力ファイル名textBoxCell";
            this.出力ファイル名textBoxCell.Size = new System.Drawing.Size(382, 21);
            cellStyle5.BackColor = System.Drawing.Color.White;
            border2.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle5.Border = border2;
            cellStyle5.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.出力ファイル名textBoxCell.Style = cellStyle5;
            this.出力ファイル名textBoxCell.TabIndex = 4;
            // 
            // labelCell6
            // 
            this.labelCell6.Location = new System.Drawing.Point(18, 56);
            this.labelCell6.Name = "labelCell6";
            this.labelCell6.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.Horizontal;
            this.labelCell6.Selectable = false;
            this.labelCell6.Size = new System.Drawing.Size(98, 21);
            cellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            cellStyle6.Border = threeDBorder1;
            cellStyle6.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelCell6.Style = cellStyle6;
            this.labelCell6.TabIndex = 5;
            this.labelCell6.TabStop = false;
            this.labelCell6.Value = "出力ﾌｧｲﾙ名";
            // 
            // ファイルオープンbuttonCell
            // 
            this.ファイルオープンbuttonCell.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ファイルオープンbuttonCell.Location = new System.Drawing.Point(513, 56);
            this.ファイルオープンbuttonCell.Name = "ファイルオープンbuttonCell";
            this.ファイルオープンbuttonCell.Size = new System.Drawing.Size(29, 21);
            cellStyle7.BackColor = System.Drawing.SystemColors.Control;
            cellStyle7.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle7.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle7.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.ファイルオープンbuttonCell.Style = cellStyle7;
            this.ファイルオープンbuttonCell.TabIndex = 6;
            this.ファイルオープンbuttonCell.Value = "...";
            // 
            // プレビューbuttonCell
            // 
            this.プレビューbuttonCell.Location = new System.Drawing.Point(204, 181);
            this.プレビューbuttonCell.Name = "プレビューbuttonCell";
            cellStyle8.BackColor = System.Drawing.SystemColors.Control;
            cellStyle8.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle8.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle8.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.プレビューbuttonCell.Style = cellStyle8;
            this.プレビューbuttonCell.TabIndex = 7;
            this.プレビューbuttonCell.Value = "ﾌﾟﾚﾋﾞｭｰ";
            // 
            // 印刷buttonCell
            // 
            this.印刷buttonCell.Location = new System.Drawing.Point(306, 181);
            this.印刷buttonCell.Name = "印刷buttonCell";
            cellStyle9.BackColor = System.Drawing.SystemColors.Control;
            cellStyle9.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle9.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle9.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.印刷buttonCell.Style = cellStyle9;
            this.印刷buttonCell.TabIndex = 8;
            this.印刷buttonCell.Value = "印刷";
            // 
            // monotaRO回答データ作成Template
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 239;
            this.Width = 584;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.LabelCell labelCell12;
        private GrapeCity.Win.MultiRow.ButtonCell CSV出力buttonCell;
        private GrapeCity.Win.MultiRow.ButtonCell 終了buttonCell;
        private GrapeCity.Win.MultiRow.ShapeCell shapeCell1;
        private GrapeCity.Win.MultiRow.TextBoxCell 出力ファイル名textBoxCell;
        private GrapeCity.Win.MultiRow.LabelCell labelCell6;
        private GrapeCity.Win.MultiRow.ButtonCell ファイルオープンbuttonCell;
        private GrapeCity.Win.MultiRow.ButtonCell プレビューbuttonCell;
        private GrapeCity.Win.MultiRow.ButtonCell 印刷buttonCell;
    }
}
