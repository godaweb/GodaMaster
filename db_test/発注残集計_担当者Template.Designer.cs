namespace db_test
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class 発注残集計_担当者Template
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
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle7 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.InputMan.DropDownButton dropDownButton1 = new GrapeCity.Win.MultiRow.InputMan.DropDownButton();
            GrapeCity.Win.MultiRow.CellStyle cellStyle8 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle9 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle10 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border5 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle11 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border6 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle12 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border7 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle13 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border8 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle14 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border9 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle15 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border10 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.MathStatistics mathStatistics1 = new GrapeCity.Win.MultiRow.MathStatistics();
            GrapeCity.Win.MultiRow.CellStyle cellStyle16 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Expression expression1 = new GrapeCity.Win.MultiRow.Expression();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.labelCell10 = new GrapeCity.Win.MultiRow.LabelCell();
            this.検索事業所textBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.検索事業所gcComboBoxCell = new GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell(false);
            this.labelCell12 = new GrapeCity.Win.MultiRow.LabelCell();
            this.終了buttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.columnHeaderSection2 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.columnHeaderCell2 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell5 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell8 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell1 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnFooterSection1 = new GrapeCity.Win.MultiRow.ColumnFooterSection();
            this.labelCell1 = new GrapeCity.Win.MultiRow.LabelCell();
            this.発注残高総合計 = new GrapeCity.Win.MultiRow.SummaryCell();
            this.仕入担当者コード = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.担当者名 = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.発注残高 = new GrapeCity.Win.MultiRow.NumericUpDownCell();
            this.構成比 = new GrapeCity.Win.MultiRow.SummaryCell();
            this.rowHeaderCell1 = new GrapeCity.Win.MultiRow.RowHeaderCell();
            this.発注残高合計 = new GrapeCity.Win.MultiRow.NumericUpDownCell();
            ((System.ComponentModel.ISupportInitialize)(this.検索事業所gcComboBoxCell)).BeginInit();
            // 
            // Row
            // 
            this.Row.Cells.Add(this.仕入担当者コード);
            this.Row.Cells.Add(this.担当者名);
            this.Row.Cells.Add(this.発注残高);
            this.Row.Cells.Add(this.構成比);
            this.Row.Cells.Add(this.rowHeaderCell1);
            this.Row.Cells.Add(this.発注残高合計);
            this.Row.Height = 25;
            this.Row.Width = 598;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.labelCell10);
            this.columnHeaderSection1.Cells.Add(this.検索事業所textBoxCell);
            this.columnHeaderSection1.Cells.Add(this.検索事業所gcComboBoxCell);
            this.columnHeaderSection1.Cells.Add(this.labelCell12);
            this.columnHeaderSection1.Cells.Add(this.終了buttonCell);
            this.columnHeaderSection1.Height = 85;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 598;
            // 
            // labelCell10
            // 
            this.labelCell10.Location = new System.Drawing.Point(11, 48);
            this.labelCell10.Name = "labelCell10";
            this.labelCell10.Selectable = false;
            this.labelCell10.Size = new System.Drawing.Size(85, 21);
            cellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.SystemColors.Highlight);
            cellStyle6.Border = border1;
            cellStyle6.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle6.ForeColor = System.Drawing.Color.Black;
            cellStyle6.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle6.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.labelCell10.Style = cellStyle6;
            this.labelCell10.TabIndex = 0;
            this.labelCell10.Value = "事業所";
            // 
            // 検索事業所textBoxCell
            // 
            this.検索事業所textBoxCell.HighlightText = true;
            this.検索事業所textBoxCell.Location = new System.Drawing.Point(104, 48);
            this.検索事業所textBoxCell.Name = "検索事業所textBoxCell";
            this.検索事業所textBoxCell.Size = new System.Drawing.Size(65, 21);
            cellStyle7.BackColor = System.Drawing.Color.White;
            border2.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle7.Border = border2;
            cellStyle7.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle7.ForeColor = System.Drawing.Color.Black;
            cellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            this.検索事業所textBoxCell.Style = cellStyle7;
            this.検索事業所textBoxCell.TabIndex = 1;
            // 
            // 検索事業所gcComboBoxCell
            // 
            this.検索事業所gcComboBoxCell.AutoSelect = true;
            this.検索事業所gcComboBoxCell.ExitOnArrowKey = true;
            this.検索事業所gcComboBoxCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.検索事業所gcComboBoxCell.Location = new System.Drawing.Point(179, 48);
            this.検索事業所gcComboBoxCell.Name = "検索事業所gcComboBoxCell";
            this.検索事業所gcComboBoxCell.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Return))), "ApplyRecommendedValue")});
            this.検索事業所gcComboBoxCell.SideButtons.Add(dropDownButton1);
            this.検索事業所gcComboBoxCell.Size = new System.Drawing.Size(205, 21);
            cellStyle8.BackColor = System.Drawing.Color.White;
            border3.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle8.Border = border3;
            cellStyle8.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle8.ForeColor = System.Drawing.Color.Black;
            this.検索事業所gcComboBoxCell.Style = cellStyle8;
            this.検索事業所gcComboBoxCell.TabIndex = 2;
            this.検索事業所gcComboBoxCell.TabStop = false;
            // 
            // labelCell12
            // 
            this.labelCell12.Location = new System.Drawing.Point(2, 2);
            this.labelCell12.Name = "labelCell12";
            this.labelCell12.Selectable = false;
            this.labelCell12.Size = new System.Drawing.Size(596, 29);
            cellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(122)))), ((int)(((byte)(122)))));
            cellStyle9.Border = border4;
            cellStyle9.Font = new System.Drawing.Font("ＭＳ ゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            cellStyle9.UseCompatibleTextRendering = GrapeCity.Win.MultiRow.MultiRowTriState.True;
            this.labelCell12.Style = cellStyle9;
            this.labelCell12.TabIndex = 3;
            this.labelCell12.TabStop = false;
            this.labelCell12.Value = "発注残集計　担当者";
            // 
            // 終了buttonCell
            // 
            this.終了buttonCell.Location = new System.Drawing.Point(503, 48);
            this.終了buttonCell.Name = "終了buttonCell";
            this.終了buttonCell.Size = new System.Drawing.Size(80, 24);
            cellStyle10.BackColor = System.Drawing.SystemColors.Control;
            cellStyle10.Border = border5;
            cellStyle10.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle10.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle10.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.終了buttonCell.Style = cellStyle10;
            this.終了buttonCell.TabIndex = 4;
            this.終了buttonCell.Value = "終了";
            // 
            // columnHeaderSection2
            // 
            this.columnHeaderSection2.Cells.Add(this.columnHeaderCell2);
            this.columnHeaderSection2.Cells.Add(this.columnHeaderCell5);
            this.columnHeaderSection2.Cells.Add(this.columnHeaderCell8);
            this.columnHeaderSection2.Cells.Add(this.columnHeaderCell1);
            this.columnHeaderSection2.Height = 24;
            this.columnHeaderSection2.Name = "columnHeaderSection2";
            this.columnHeaderSection2.Width = 598;
            // 
            // columnHeaderCell2
            // 
            this.columnHeaderCell2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell2.Location = new System.Drawing.Point(11, 3);
            this.columnHeaderCell2.Name = "columnHeaderCell2";
            this.columnHeaderCell2.Size = new System.Drawing.Size(107, 21);
            cellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            border6.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle11.Border = border6;
            cellStyle11.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle11.ForeColor = System.Drawing.Color.Black;
            cellStyle11.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell2.Style = cellStyle11;
            this.columnHeaderCell2.TabIndex = 0;
            this.columnHeaderCell2.Value = "担当者コード";
            // 
            // columnHeaderCell5
            // 
            this.columnHeaderCell5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell5.Location = new System.Drawing.Point(118, 3);
            this.columnHeaderCell5.Name = "columnHeaderCell5";
            this.columnHeaderCell5.Size = new System.Drawing.Size(305, 21);
            cellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            border7.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle12.Border = border7;
            cellStyle12.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle12.ForeColor = System.Drawing.Color.Black;
            cellStyle12.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell5.Style = cellStyle12;
            this.columnHeaderCell5.TabIndex = 1;
            this.columnHeaderCell5.Value = "担　当　者　名";
            // 
            // columnHeaderCell8
            // 
            this.columnHeaderCell8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell8.Location = new System.Drawing.Point(423, 3);
            this.columnHeaderCell8.Name = "columnHeaderCell8";
            this.columnHeaderCell8.Size = new System.Drawing.Size(110, 21);
            cellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            border8.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle13.Border = border8;
            cellStyle13.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle13.ForeColor = System.Drawing.Color.Black;
            cellStyle13.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.columnHeaderCell8.Style = cellStyle13;
            this.columnHeaderCell8.TabIndex = 2;
            this.columnHeaderCell8.Value = "発注残高";
            // 
            // columnHeaderCell1
            // 
            this.columnHeaderCell1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell1.Location = new System.Drawing.Point(533, 3);
            this.columnHeaderCell1.Name = "columnHeaderCell1";
            this.columnHeaderCell1.Size = new System.Drawing.Size(50, 21);
            cellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            border9.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle14.Border = border9;
            cellStyle14.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle14.ForeColor = System.Drawing.Color.Black;
            cellStyle14.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.columnHeaderCell1.Style = cellStyle14;
            this.columnHeaderCell1.TabIndex = 3;
            this.columnHeaderCell1.Value = "構成比";
            // 
            // columnFooterSection1
            // 
            this.columnFooterSection1.Cells.Add(this.labelCell1);
            this.columnFooterSection1.Cells.Add(this.発注残高総合計);
            this.columnFooterSection1.Height = 40;
            this.columnFooterSection1.Name = "columnFooterSection1";
            this.columnFooterSection1.Width = 598;
            // 
            // labelCell1
            // 
            this.labelCell1.Location = new System.Drawing.Point(314, 9);
            this.labelCell1.Name = "labelCell1";
            this.labelCell1.Selectable = false;
            this.labelCell1.Size = new System.Drawing.Size(85, 21);
            cellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            border10.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.SystemColors.Highlight);
            cellStyle15.Border = border10;
            cellStyle15.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle15.ForeColor = System.Drawing.Color.Black;
            cellStyle15.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle15.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.labelCell1.Style = cellStyle15;
            this.labelCell1.TabIndex = 0;
            this.labelCell1.Value = "合計";
            // 
            // 発注残高総合計
            // 
            mathStatistics1.CellName = "発注残高";
            this.発注残高総合計.Calculation = mathStatistics1;
            this.発注残高総合計.Location = new System.Drawing.Point(422, 9);
            this.発注残高総合計.Name = "発注残高総合計";
            this.発注残高総合計.Selectable = false;
            this.発注残高総合計.Size = new System.Drawing.Size(110, 21);
            cellStyle16.Font = new System.Drawing.Font("Meiryo UI", 9.75F);
            cellStyle16.Format = "##,#";
            cellStyle16.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.発注残高総合計.Style = cellStyle16;
            this.発注残高総合計.TabIndex = 1;
            // 
            // 仕入担当者コード
            // 
            this.仕入担当者コード.DataField = "仕入担当者コード";
            this.仕入担当者コード.Location = new System.Drawing.Point(11, 4);
            this.仕入担当者コード.Name = "仕入担当者コード";
            this.仕入担当者コード.Size = new System.Drawing.Size(107, 21);
            cellStyle1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.仕入担当者コード.Style = cellStyle1;
            this.仕入担当者コード.TabIndex = 0;
            // 
            // 担当者名
            // 
            this.担当者名.DataField = "担当者名";
            this.担当者名.Location = new System.Drawing.Point(118, 4);
            this.担当者名.Name = "担当者名";
            this.担当者名.Selectable = false;
            this.担当者名.Size = new System.Drawing.Size(305, 21);
            cellStyle2.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.担当者名.Style = cellStyle2;
            this.担当者名.TabIndex = 1;
            // 
            // 発注残高
            // 
            this.発注残高.DataField = "発注残高";
            this.発注残高.Location = new System.Drawing.Point(423, 4);
            this.発注残高.Name = "発注残高";
            this.発注残高.Selectable = false;
            this.発注残高.ShowSpinButton = GrapeCity.Win.MultiRow.CellButtonVisibility.NotShown;
            this.発注残高.ShowSpinButtonInEditState = false;
            this.発注残高.Size = new System.Drawing.Size(110, 21);
            cellStyle3.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle3.Format = "##,#";
            cellStyle3.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.発注残高.Style = cellStyle3;
            this.発注残高.TabIndex = 2;
            this.発注残高.ThousandsSeparator = true;
            // 
            // 構成比
            // 
            expression1.ExpressionString = "発注残高 / 発注残高合計 * 100";
            this.構成比.Calculation = expression1;
            this.構成比.Location = new System.Drawing.Point(533, 4);
            this.構成比.Name = "構成比";
            this.構成比.ReadOnly = false;
            this.構成比.Selectable = false;
            this.構成比.Size = new System.Drawing.Size(50, 21);
            cellStyle4.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle4.Format = "##,0.0";
            cellStyle4.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.構成比.Style = cellStyle4;
            this.構成比.TabIndex = 3;
            // 
            // rowHeaderCell1
            // 
            this.rowHeaderCell1.DataField = "商品コード";
            this.rowHeaderCell1.Location = new System.Drawing.Point(2, 2);
            this.rowHeaderCell1.Name = "rowHeaderCell1";
            this.rowHeaderCell1.Size = new System.Drawing.Size(10, 21);
            cellStyle5.ForeColor = System.Drawing.Color.White;
            this.rowHeaderCell1.Style = cellStyle5;
            this.rowHeaderCell1.TabIndex = 4;
            // 
            // 発注残高合計
            // 
            this.発注残高合計.DataField = "発注残高合計";
            this.発注残高合計.Location = new System.Drawing.Point(335, 4);
            this.発注残高合計.Name = "発注残高合計";
            this.発注残高合計.Selectable = false;
            this.発注残高合計.Size = new System.Drawing.Size(38, 21);
            this.発注残高合計.TabIndex = 5;
            this.発注残高合計.Visible = false;
            // 
            // 発注残集計_担当者Template
            // 
            this.ColumnFooters.AddRange(new GrapeCity.Win.MultiRow.ColumnFooterSection[] {
            this.columnFooterSection1});
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1,
            this.columnHeaderSection2});
            this.Height = 174;
            this.Width = 598;
            ((System.ComponentModel.ISupportInitialize)(this.検索事業所gcComboBoxCell)).EndInit();

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection2;
        private GrapeCity.Win.MultiRow.ColumnFooterSection columnFooterSection1;
        private GrapeCity.Win.MultiRow.LabelCell labelCell10;
        private GrapeCity.Win.MultiRow.TextBoxCell 検索事業所textBoxCell;
        private GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell 検索事業所gcComboBoxCell;
        private GrapeCity.Win.MultiRow.LabelCell labelCell12;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell2;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell5;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell8;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell1;
        private GrapeCity.Win.MultiRow.TextBoxCell 仕入担当者コード;
        private GrapeCity.Win.MultiRow.TextBoxCell 担当者名;
        private GrapeCity.Win.MultiRow.NumericUpDownCell 発注残高;
        private GrapeCity.Win.MultiRow.SummaryCell 構成比;
        private GrapeCity.Win.MultiRow.RowHeaderCell rowHeaderCell1;
        private GrapeCity.Win.MultiRow.NumericUpDownCell 発注残高合計;
        private GrapeCity.Win.MultiRow.LabelCell labelCell1;
        private GrapeCity.Win.MultiRow.SummaryCell 発注残高総合計;
        private GrapeCity.Win.MultiRow.ButtonCell 終了buttonCell;
    }
}
