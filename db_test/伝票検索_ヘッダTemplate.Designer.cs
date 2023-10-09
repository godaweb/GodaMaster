namespace db_test
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class 伝票検索_ヘッダTemplate
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
            GrapeCity.Win.MultiRow.InputMan.DropDownButton dropDownButton1 = new GrapeCity.Win.MultiRow.InputMan.DropDownButton();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.ThreeDBorder threeDBorder2 = new GrapeCity.Win.MultiRow.ThreeDBorder();
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.InputMan.DropDownButton dropDownButton2 = new GrapeCity.Win.MultiRow.InputMan.DropDownButton();
            GrapeCity.Win.MultiRow.CellStyle cellStyle7 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border5 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle8 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.ThreeDBorder threeDBorder3 = new GrapeCity.Win.MultiRow.ThreeDBorder();
            GrapeCity.Win.MultiRow.CellStyle cellStyle9 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.ThreeDBorder threeDBorder4 = new GrapeCity.Win.MultiRow.ThreeDBorder();
            GrapeCity.Win.MultiRow.CellStyle cellStyle10 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border6 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle11 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.ThreeDBorder threeDBorder5 = new GrapeCity.Win.MultiRow.ThreeDBorder();
            GrapeCity.Win.MultiRow.CellStyle cellStyle12 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border7 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle13 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle14 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle15 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.InputMan.DropDownButton dropDownButton3 = new GrapeCity.Win.MultiRow.InputMan.DropDownButton();
            GrapeCity.Win.MultiRow.CellStyle cellStyle16 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border8 = new GrapeCity.Win.MultiRow.Border();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.取引先コードtextBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.担当者コードtextBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.labelCell3 = new GrapeCity.Win.MultiRow.LabelCell();
            this.担当者gcComboBoxCell = new GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell(false);
            this.取引先labelCell = new GrapeCity.Win.MultiRow.LabelCell();
            this.事業所コードtextBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.事業所gcComboBoxCell = new GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell(false);
            this.labelCell7 = new GrapeCity.Win.MultiRow.LabelCell();
            this.番号labelCell = new GrapeCity.Win.MultiRow.LabelCell();
            this.伝票番号textBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.日付labelCell = new GrapeCity.Win.MultiRow.LabelCell();
            this.日付textBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.選択buttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.キャンセルbuttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.検索buttonCell = new GrapeCity.Win.MultiRow.ButtonCell();
            this.取引先gcComboBoxCell = new GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell(false);
            ((System.ComponentModel.ISupportInitialize)(this.担当者gcComboBoxCell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.事業所gcComboBoxCell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.取引先gcComboBoxCell)).BeginInit();
            // 
            // Row
            // 
            this.Row.Height = 1;
            this.Row.Width = 877;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.取引先コードtextBoxCell);
            this.columnHeaderSection1.Cells.Add(this.担当者コードtextBoxCell);
            this.columnHeaderSection1.Cells.Add(this.labelCell3);
            this.columnHeaderSection1.Cells.Add(this.担当者gcComboBoxCell);
            this.columnHeaderSection1.Cells.Add(this.取引先labelCell);
            this.columnHeaderSection1.Cells.Add(this.事業所コードtextBoxCell);
            this.columnHeaderSection1.Cells.Add(this.事業所gcComboBoxCell);
            this.columnHeaderSection1.Cells.Add(this.labelCell7);
            this.columnHeaderSection1.Cells.Add(this.番号labelCell);
            this.columnHeaderSection1.Cells.Add(this.伝票番号textBoxCell);
            this.columnHeaderSection1.Cells.Add(this.日付labelCell);
            this.columnHeaderSection1.Cells.Add(this.日付textBoxCell);
            this.columnHeaderSection1.Cells.Add(this.選択buttonCell);
            this.columnHeaderSection1.Cells.Add(this.キャンセルbuttonCell);
            this.columnHeaderSection1.Cells.Add(this.検索buttonCell);
            this.columnHeaderSection1.Cells.Add(this.取引先gcComboBoxCell);
            this.columnHeaderSection1.Height = 128;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.ReadOnly = false;
            this.columnHeaderSection1.Selectable = true;
            this.columnHeaderSection1.Width = 877;
            // 
            // 取引先コードtextBoxCell
            // 
            this.取引先コードtextBoxCell.HighlightText = true;
            this.取引先コードtextBoxCell.Location = new System.Drawing.Point(111, 83);
            this.取引先コードtextBoxCell.Name = "取引先コードtextBoxCell";
            this.取引先コードtextBoxCell.Size = new System.Drawing.Size(65, 21);
            cellStyle1.BackColor = System.Drawing.Color.White;
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle1.Border = border1;
            cellStyle1.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.取引先コードtextBoxCell.Style = cellStyle1;
            this.取引先コードtextBoxCell.TabIndex = 2;
            // 
            // 担当者コードtextBoxCell
            // 
            this.担当者コードtextBoxCell.HighlightText = true;
            this.担当者コードtextBoxCell.Location = new System.Drawing.Point(111, 52);
            this.担当者コードtextBoxCell.Name = "担当者コードtextBoxCell";
            this.担当者コードtextBoxCell.Size = new System.Drawing.Size(40, 21);
            cellStyle2.BackColor = System.Drawing.Color.White;
            border2.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle2.Border = border2;
            cellStyle2.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.担当者コードtextBoxCell.Style = cellStyle2;
            this.担当者コードtextBoxCell.TabIndex = 1;
            this.担当者コードtextBoxCell.TabStop = false;
            // 
            // labelCell3
            // 
            this.labelCell3.Location = new System.Drawing.Point(21, 52);
            this.labelCell3.Name = "labelCell3";
            this.labelCell3.Selectable = false;
            this.labelCell3.Size = new System.Drawing.Size(85, 21);
            cellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            cellStyle3.Border = threeDBorder1;
            cellStyle3.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.labelCell3.Style = cellStyle3;
            this.labelCell3.TabIndex = 12;
            this.labelCell3.TabStop = false;
            this.labelCell3.Value = "担当者";
            // 
            // 担当者gcComboBoxCell
            // 
            this.担当者gcComboBoxCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.担当者gcComboBoxCell.Location = new System.Drawing.Point(157, 52);
            this.担当者gcComboBoxCell.Name = "担当者gcComboBoxCell";
            this.担当者gcComboBoxCell.Selectable = false;
            this.担当者gcComboBoxCell.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Return))), "ApplyRecommendedValue")});
            this.担当者gcComboBoxCell.SideButtons.Add(dropDownButton1);
            this.担当者gcComboBoxCell.Size = new System.Drawing.Size(119, 21);
            cellStyle4.BackColor = System.Drawing.Color.White;
            border3.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle4.Border = border3;
            cellStyle4.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.担当者gcComboBoxCell.Style = cellStyle4;
            this.担当者gcComboBoxCell.TabIndex = 6;
            this.担当者gcComboBoxCell.TabStop = false;
            // 
            // 取引先labelCell
            // 
            this.取引先labelCell.Location = new System.Drawing.Point(21, 83);
            this.取引先labelCell.Name = "取引先labelCell";
            this.取引先labelCell.Selectable = false;
            this.取引先labelCell.Size = new System.Drawing.Size(85, 21);
            cellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            cellStyle5.Border = threeDBorder2;
            cellStyle5.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle5.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.取引先labelCell.Style = cellStyle5;
            this.取引先labelCell.TabIndex = 13;
            this.取引先labelCell.TabStop = false;
            this.取引先labelCell.Value = "得意先";
            // 
            // 事業所コードtextBoxCell
            // 
            this.事業所コードtextBoxCell.HighlightText = true;
            this.事業所コードtextBoxCell.Location = new System.Drawing.Point(111, 21);
            this.事業所コードtextBoxCell.Name = "事業所コードtextBoxCell";
            this.事業所コードtextBoxCell.Size = new System.Drawing.Size(40, 21);
            cellStyle6.BackColor = System.Drawing.Color.White;
            border4.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle6.Border = border4;
            cellStyle6.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.事業所コードtextBoxCell.Style = cellStyle6;
            this.事業所コードtextBoxCell.TabIndex = 0;
            // 
            // 事業所gcComboBoxCell
            // 
            this.事業所gcComboBoxCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.事業所gcComboBoxCell.Location = new System.Drawing.Point(157, 21);
            this.事業所gcComboBoxCell.Name = "事業所gcComboBoxCell";
            this.事業所gcComboBoxCell.Selectable = false;
            this.事業所gcComboBoxCell.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Return))), "ApplyRecommendedValue")});
            this.事業所gcComboBoxCell.SideButtons.Add(dropDownButton2);
            this.事業所gcComboBoxCell.Size = new System.Drawing.Size(155, 21);
            cellStyle7.BackColor = System.Drawing.Color.White;
            border5.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle7.Border = border5;
            cellStyle7.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.事業所gcComboBoxCell.Style = cellStyle7;
            this.事業所gcComboBoxCell.TabIndex = 5;
            this.事業所gcComboBoxCell.TabStop = false;
            // 
            // labelCell7
            // 
            this.labelCell7.Location = new System.Drawing.Point(21, 21);
            this.labelCell7.Name = "labelCell7";
            this.labelCell7.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.Horizontal;
            this.labelCell7.Selectable = false;
            this.labelCell7.Size = new System.Drawing.Size(85, 21);
            cellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            cellStyle8.Border = threeDBorder3;
            cellStyle8.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelCell7.Style = cellStyle8;
            this.labelCell7.TabIndex = 11;
            this.labelCell7.TabStop = false;
            this.labelCell7.Value = "事業所";
            // 
            // 番号labelCell
            // 
            this.番号labelCell.Location = new System.Drawing.Point(341, 52);
            this.番号labelCell.Name = "番号labelCell";
            this.番号labelCell.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.Horizontal;
            this.番号labelCell.Selectable = false;
            this.番号labelCell.Size = new System.Drawing.Size(85, 21);
            cellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            cellStyle9.Border = threeDBorder4;
            cellStyle9.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.番号labelCell.Style = cellStyle9;
            this.番号labelCell.TabIndex = 15;
            this.番号labelCell.TabStop = false;
            this.番号labelCell.Value = "伝票番号";
            // 
            // 伝票番号textBoxCell
            // 
            this.伝票番号textBoxCell.HighlightText = true;
            this.伝票番号textBoxCell.Location = new System.Drawing.Point(431, 52);
            this.伝票番号textBoxCell.Name = "伝票番号textBoxCell";
            this.伝票番号textBoxCell.Size = new System.Drawing.Size(84, 21);
            cellStyle10.BackColor = System.Drawing.Color.White;
            border6.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle10.Border = border6;
            cellStyle10.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.伝票番号textBoxCell.Style = cellStyle10;
            this.伝票番号textBoxCell.TabIndex = 4;
            // 
            // 日付labelCell
            // 
            this.日付labelCell.Location = new System.Drawing.Point(341, 21);
            this.日付labelCell.Name = "日付labelCell";
            this.日付labelCell.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.Horizontal;
            this.日付labelCell.Selectable = false;
            this.日付labelCell.Size = new System.Drawing.Size(85, 21);
            cellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(199)))));
            cellStyle11.Border = threeDBorder5;
            cellStyle11.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.日付labelCell.Style = cellStyle11;
            this.日付labelCell.TabIndex = 14;
            this.日付labelCell.TabStop = false;
            this.日付labelCell.Value = "日付指定";
            // 
            // 日付textBoxCell
            // 
            this.日付textBoxCell.HighlightText = true;
            this.日付textBoxCell.Location = new System.Drawing.Point(431, 21);
            this.日付textBoxCell.Name = "日付textBoxCell";
            this.日付textBoxCell.Size = new System.Drawing.Size(84, 21);
            cellStyle12.BackColor = System.Drawing.Color.White;
            border7.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle12.Border = border7;
            cellStyle12.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.日付textBoxCell.Style = cellStyle12;
            this.日付textBoxCell.TabIndex = 3;
            this.日付textBoxCell.TabStop = false;
            // 
            // 選択buttonCell
            // 
            this.選択buttonCell.Location = new System.Drawing.Point(565, 52);
            this.選択buttonCell.Name = "選択buttonCell";
            this.選択buttonCell.Selectable = false;
            cellStyle13.BackColor = System.Drawing.SystemColors.Control;
            cellStyle13.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle13.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle13.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle13.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.選択buttonCell.Style = cellStyle13;
            this.選択buttonCell.TabIndex = 9;
            this.選択buttonCell.Value = "選　択";
            // 
            // キャンセルbuttonCell
            // 
            this.キャンセルbuttonCell.Location = new System.Drawing.Point(565, 83);
            this.キャンセルbuttonCell.Name = "キャンセルbuttonCell";
            this.キャンセルbuttonCell.Selectable = false;
            cellStyle14.BackColor = System.Drawing.SystemColors.Control;
            cellStyle14.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle14.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle14.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle14.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.キャンセルbuttonCell.Style = cellStyle14;
            this.キャンセルbuttonCell.TabIndex = 10;
            this.キャンセルbuttonCell.Value = "ｷｬﾝｾﾙ";
            // 
            // 検索buttonCell
            // 
            this.検索buttonCell.Location = new System.Drawing.Point(565, 21);
            this.検索buttonCell.Name = "検索buttonCell";
            this.検索buttonCell.Selectable = false;
            cellStyle15.BackColor = System.Drawing.SystemColors.Control;
            cellStyle15.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle15.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle15.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle15.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.検索buttonCell.Style = cellStyle15;
            this.検索buttonCell.TabIndex = 8;
            this.検索buttonCell.Value = "検　索";
            // 
            // 取引先gcComboBoxCell
            // 
            this.取引先gcComboBoxCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.取引先gcComboBoxCell.Location = new System.Drawing.Point(185, 83);
            this.取引先gcComboBoxCell.Name = "取引先gcComboBoxCell";
            this.取引先gcComboBoxCell.Selectable = false;
            this.取引先gcComboBoxCell.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Return))), "ApplyRecommendedValue")});
            this.取引先gcComboBoxCell.SideButtons.Add(dropDownButton3);
            this.取引先gcComboBoxCell.Size = new System.Drawing.Size(356, 21);
            cellStyle16.BackColor = System.Drawing.Color.White;
            border8.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle16.Border = border8;
            cellStyle16.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.取引先gcComboBoxCell.Style = cellStyle16;
            this.取引先gcComboBoxCell.TabIndex = 7;
            this.取引先gcComboBoxCell.TabStop = false;
            // 
            // 伝票検索_ヘッダTemplate
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 129;
            this.Width = 877;
            ((System.ComponentModel.ISupportInitialize)(this.担当者gcComboBoxCell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.事業所gcComboBoxCell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.取引先gcComboBoxCell)).EndInit();

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.TextBoxCell 取引先コードtextBoxCell;
        private GrapeCity.Win.MultiRow.TextBoxCell 担当者コードtextBoxCell;
        private GrapeCity.Win.MultiRow.LabelCell labelCell3;
        private GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell 担当者gcComboBoxCell;
        private GrapeCity.Win.MultiRow.LabelCell 取引先labelCell;
        private GrapeCity.Win.MultiRow.TextBoxCell 事業所コードtextBoxCell;
        private GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell 事業所gcComboBoxCell;
        private GrapeCity.Win.MultiRow.LabelCell labelCell7;
        private GrapeCity.Win.MultiRow.LabelCell 番号labelCell;
        private GrapeCity.Win.MultiRow.TextBoxCell 伝票番号textBoxCell;
        private GrapeCity.Win.MultiRow.LabelCell 日付labelCell;
        private GrapeCity.Win.MultiRow.TextBoxCell 日付textBoxCell;
        private GrapeCity.Win.MultiRow.ButtonCell 選択buttonCell;
        private GrapeCity.Win.MultiRow.ButtonCell キャンセルbuttonCell;
        private GrapeCity.Win.MultiRow.ButtonCell 検索buttonCell;
        private GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell 取引先gcComboBoxCell;
    }
}
