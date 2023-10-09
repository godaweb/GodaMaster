namespace db_test
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class 伝票検索右Template
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
            GrapeCity.Win.MultiRow.CellStyle cellStyle8 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle9 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle10 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border5 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.columnHeaderCell28 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell29 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell30 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell36 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell37 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.コードtextBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.品名textBoxCell = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.受注残textBoxCell = new GrapeCity.Win.MultiRow.NumericUpDownCell();
            this.単価textBoxCell = new GrapeCity.Win.MultiRow.NumericUpDownCell();
            this.受注残金額textBoxCell = new GrapeCity.Win.MultiRow.NumericUpDownCell();
            // 
            // Row
            // 
            this.Row.Cells.Add(this.コードtextBoxCell);
            this.Row.Cells.Add(this.品名textBoxCell);
            this.Row.Cells.Add(this.受注残textBoxCell);
            this.Row.Cells.Add(this.単価textBoxCell);
            this.Row.Cells.Add(this.受注残金額textBoxCell);
            this.Row.Height = 21;
            this.Row.Width = 404;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell28);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell29);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell30);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell36);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell37);
            this.columnHeaderSection1.Height = 21;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 404;
            // 
            // columnHeaderCell28
            // 
            this.columnHeaderCell28.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell28.Location = new System.Drawing.Point(7, 1);
            this.columnHeaderCell28.Name = "columnHeaderCell28";
            this.columnHeaderCell28.Size = new System.Drawing.Size(59, 21);
            cellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle6.Border = border1;
            cellStyle6.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle6.ForeColor = System.Drawing.Color.Black;
            this.columnHeaderCell28.Style = cellStyle6;
            this.columnHeaderCell28.TabIndex = 0;
            this.columnHeaderCell28.TabStop = false;
            this.columnHeaderCell28.Value = "コード";
            // 
            // columnHeaderCell29
            // 
            this.columnHeaderCell29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell29.Location = new System.Drawing.Point(66, 1);
            this.columnHeaderCell29.Name = "columnHeaderCell29";
            this.columnHeaderCell29.Size = new System.Drawing.Size(144, 21);
            cellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            border2.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle7.Border = border2;
            cellStyle7.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle7.ForeColor = System.Drawing.Color.Black;
            this.columnHeaderCell29.Style = cellStyle7;
            this.columnHeaderCell29.TabIndex = 1;
            this.columnHeaderCell29.TabStop = false;
            this.columnHeaderCell29.Value = "品名";
            // 
            // columnHeaderCell30
            // 
            this.columnHeaderCell30.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell30.Location = new System.Drawing.Point(210, 1);
            this.columnHeaderCell30.Name = "columnHeaderCell30";
            this.columnHeaderCell30.Size = new System.Drawing.Size(55, 21);
            cellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            border3.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle8.Border = border3;
            cellStyle8.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle8.ForeColor = System.Drawing.Color.Black;
            this.columnHeaderCell30.Style = cellStyle8;
            this.columnHeaderCell30.TabIndex = 2;
            this.columnHeaderCell30.TabStop = false;
            this.columnHeaderCell30.Value = "受注残";
            // 
            // columnHeaderCell36
            // 
            this.columnHeaderCell36.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell36.Location = new System.Drawing.Point(324, 1);
            this.columnHeaderCell36.Name = "columnHeaderCell36";
            this.columnHeaderCell36.Size = new System.Drawing.Size(80, 21);
            cellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            border4.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle9.Border = border4;
            cellStyle9.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle9.ForeColor = System.Drawing.Color.Black;
            this.columnHeaderCell36.Style = cellStyle9;
            this.columnHeaderCell36.TabIndex = 3;
            this.columnHeaderCell36.TabStop = false;
            this.columnHeaderCell36.Value = "受注残金額";
            // 
            // columnHeaderCell37
            // 
            this.columnHeaderCell37.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell37.Location = new System.Drawing.Point(265, 1);
            this.columnHeaderCell37.Name = "columnHeaderCell37";
            this.columnHeaderCell37.Size = new System.Drawing.Size(59, 21);
            cellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            border5.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.Black);
            cellStyle10.Border = border5;
            cellStyle10.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle10.ForeColor = System.Drawing.Color.Black;
            this.columnHeaderCell37.Style = cellStyle10;
            this.columnHeaderCell37.TabIndex = 4;
            this.columnHeaderCell37.TabStop = false;
            this.columnHeaderCell37.Value = "単価";
            // 
            // コードtextBoxCell
            // 
            this.コードtextBoxCell.DataField = "コード";
            this.コードtextBoxCell.Location = new System.Drawing.Point(7, 0);
            this.コードtextBoxCell.Name = "コードtextBoxCell";
            this.コードtextBoxCell.Size = new System.Drawing.Size(59, 21);
            cellStyle1.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.コードtextBoxCell.Style = cellStyle1;
            this.コードtextBoxCell.TabIndex = 0;
            // 
            // 品名textBoxCell
            // 
            this.品名textBoxCell.DataField = "品名";
            this.品名textBoxCell.Location = new System.Drawing.Point(66, 0);
            this.品名textBoxCell.Name = "品名textBoxCell";
            this.品名textBoxCell.Size = new System.Drawing.Size(144, 21);
            cellStyle2.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.品名textBoxCell.Style = cellStyle2;
            this.品名textBoxCell.TabIndex = 1;
            // 
            // 受注残textBoxCell
            // 
            this.受注残textBoxCell.DataField = "受注残";
            this.受注残textBoxCell.Location = new System.Drawing.Point(210, 0);
            this.受注残textBoxCell.Name = "受注残textBoxCell";
            this.受注残textBoxCell.ShowSpinButton = GrapeCity.Win.MultiRow.CellButtonVisibility.NotShown;
            this.受注残textBoxCell.ShowSpinButtonInEditState = false;
            this.受注残textBoxCell.Size = new System.Drawing.Size(55, 21);
            cellStyle3.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle3.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.受注残textBoxCell.Style = cellStyle3;
            this.受注残textBoxCell.TabIndex = 2;
            this.受注残textBoxCell.ThousandsSeparator = true;
            // 
            // 単価textBoxCell
            // 
            this.単価textBoxCell.DataField = "単価";
            this.単価textBoxCell.Location = new System.Drawing.Point(265, 0);
            this.単価textBoxCell.Name = "単価textBoxCell";
            this.単価textBoxCell.ShowSpinButton = GrapeCity.Win.MultiRow.CellButtonVisibility.NotShown;
            this.単価textBoxCell.ShowSpinButtonInEditState = false;
            this.単価textBoxCell.Size = new System.Drawing.Size(59, 21);
            cellStyle4.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle4.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.単価textBoxCell.Style = cellStyle4;
            this.単価textBoxCell.TabIndex = 3;
            this.単価textBoxCell.ThousandsSeparator = true;
            // 
            // 受注残金額textBoxCell
            // 
            this.受注残金額textBoxCell.DataField = "受注残金額";
            this.受注残金額textBoxCell.Location = new System.Drawing.Point(324, 0);
            this.受注残金額textBoxCell.Name = "受注残金額textBoxCell";
            this.受注残金額textBoxCell.ShowSpinButton = GrapeCity.Win.MultiRow.CellButtonVisibility.NotShown;
            this.受注残金額textBoxCell.ShowSpinButtonInEditState = false;
            cellStyle5.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle5.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.受注残金額textBoxCell.Style = cellStyle5;
            this.受注残金額textBoxCell.TabIndex = 4;
            this.受注残金額textBoxCell.ThousandsSeparator = true;
            // 
            // 伝票検索右Template
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 42;
            this.Width = 404;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.TextBoxCell コードtextBoxCell;
        private GrapeCity.Win.MultiRow.TextBoxCell 品名textBoxCell;
        private GrapeCity.Win.MultiRow.NumericUpDownCell 受注残textBoxCell;
        private GrapeCity.Win.MultiRow.NumericUpDownCell 単価textBoxCell;
        private GrapeCity.Win.MultiRow.NumericUpDownCell 受注残金額textBoxCell;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell28;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell29;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell30;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell36;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell37;
    }
}
