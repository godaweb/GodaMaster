namespace db_test
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class 伝票検索Template
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
            this.listBoxCell1 = new GrapeCity.Win.MultiRow.ListBoxCell();
            this.listBoxCell2 = new GrapeCity.Win.MultiRow.ListBoxCell();
            // 
            // Row
            // 
            this.Row.Cells.Add(this.listBoxCell1);
            this.Row.Cells.Add(this.listBoxCell2);
            this.Row.Height = 279;
            this.Row.Width = 728;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Height = 114;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 718;
            // 
            // listBoxCell1
            // 
            this.listBoxCell1.Location = new System.Drawing.Point(8, 9);
            this.listBoxCell1.Name = "listBoxCell1";
            this.listBoxCell1.Size = new System.Drawing.Size(375, 260);
            this.listBoxCell1.TabIndex = 0;
            // 
            // listBoxCell2
            // 
            this.listBoxCell2.Location = new System.Drawing.Point(392, 9);
            this.listBoxCell2.Name = "listBoxCell2";
            this.listBoxCell2.Size = new System.Drawing.Size(328, 260);
            this.listBoxCell2.TabIndex = 1;
            // 
            // 伝票検索Template
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 393;
            this.Width = 728;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.ListBoxCell listBoxCell1;
        private GrapeCity.Win.MultiRow.ListBoxCell listBoxCell2;
    }
}
