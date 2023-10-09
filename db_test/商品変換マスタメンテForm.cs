using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using System.Data.SqlClient;
using InputManCell = GrapeCity.Win.MultiRow.InputMan;
using db_test.商品変換マスタメンテDataSet4TableAdapters;

namespace db_test
{
    public partial class 商品変換マスタメンテForm : Form
    {
        private string receiveDataSyohin = "";
        private string receiveDataToksaki = "";
        //商品検索Form fsSyohin;
        //得意先検索Form fsToksaki;

        private DataTable dataTable;
        private 商品変換マスタメンテDataSet4 商品変換マスタメンテdataSet;
        private WT商品変換マスタTableAdapter WT商品変換マスタtableAdapter;
        private T商品変換マスタTableAdapter T商品変換マスタtableAdapter;

        int rowcnt = 0;
        object toksakicd = "";
        object EOSKBN = "";
        object kyakuTyuKBN = "";

        public 商品変換マスタメンテForm()
        {
            InitializeComponent();
        }

        private void 商品変換マスタメンテForm_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 商品変換マスタメンテTemplate();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = true;
            this.gcMultiRow1.ScrollBars = ScrollBars.None;

            //セル色の設定
            //非選択状態の色
            //gcMultiRow1.Rows[0].Cells[0].Style.BackColor = Color.Blue;
            //gcMultiRow1.Rows[0].Cells[0].Style.ForeColor = Color.White;

            //選択状態のときの色
            gcMultiRow1.DefaultCellStyle.SelectionBackColor = Color.White;
            gcMultiRow1.DefaultCellStyle.SelectionForeColor = Color.Black;

            //編集状態のときの色
            gcMultiRow1.DefaultCellStyle.EditingBackColor = Color.Yellow;
            gcMultiRow1.DefaultCellStyle.EditingForeColor = Color.Black;

            //無効のときの色
            gcMultiRow1.DefaultCellStyle.DisabledBackColor = Color.White;
            gcMultiRow1.DefaultCellStyle.DisabledForeColor = Color.Black;

            // イベント
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);

            // GcMultiRowがフォーカスを失ったときに
            // セルのCellStyle.SelectionBackColorとCellStyle.SelectionForeColorを表示しない場合はtrue。
            // それ以外の場合はfalse。
            gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            //gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveRight, Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);    // 得意先、商品検索画面表示
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);    // 検索実行
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);    // 更新
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);   // 終了
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Delete);   // 終了

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品変換マスタメンテFunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品変換マスタメンテFunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品変換マスタメンテFunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品変換マスタメンテFunctionKeyAction(Keys.F10), Keys.F10);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品変換マスタメンテFunctionKeyAction(Keys.Delete), Keys.Delete);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品変換マスタメンテFunctionKeyAction(Keys.Insert), Keys.Insert );

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            /*
            GcComboBoxCell comboBoxCell1 = this.gcMultiRow1.ColumnHeaders[0].Cells["検索事業所gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell1.DataSource = Utility.GetComboBoxData("SELECT 事業所コード, 事業所名 FROM T事業所マスタ");
            comboBoxCell1.ListHeaderPane.Visible = false;
            comboBoxCell1.TextSubItemIndex = 0;
            comboBoxCell1.TextSubItemIndex = 1;
            comboBoxCell1.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell1.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell1.TextFormat = "[1]";
            */
             
            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value = 1;

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            string toksakicd;
            string syohincd_from;
            string syohincd_to;
            string hyojijyun;

            switch (e.CellName) 
            {
                case "検索buttonCell":
                    toksakicd = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString();
                    syohincd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Value.ToString();
                    syohincd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Value.ToString();
                    hyojijyun = this.gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value.ToString();
                
                    // コントロールの描画を停止する
                    gcMultiRow1.SuspendLayout();

                    //dataTable = 発注入力dataSet.T発注ファイル;

                    商品変換マスタメンテdataSet = ((商品変換マスタメンテDataSet4)(this.商品変換マスタメンテDataSet1));
                    //dataTable = 商品変換マスタメンテdataSet.WT商品変換マスタ;

                    WT商品変換マスタtableAdapter = new WT商品変換マスタTableAdapter();

                    try
                    {
                        //myTA.DeleteWK受注Bファイル();
                        //T受注ファイルtableAdapter.FillBy受注番号(受注ファイルdataSet.T受注ファイル, serchcode);
                        dataTable = WT商品変換マスタtableAdapter.GetDataBy(toksakicd, syohincd_from, syohincd_to, "1", hyojijyun);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                        return;
                    }

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("データがありません。");
                        return;
                    }

                    gcMultiRow1.DataSource = dataTable;

                    break;
                case "確認buttonCell":
                    if (dataTable.Rows.Count > 0)
                    {

                        SqlDb db = new SqlDb();
                        db.Connect();
                        db.BeginTransaction();
                        db.ExecuteSql("delete from WT商品変換マスタ", -1);
                        db.CommitTransaction();
                        db.Disconnect();

                        string connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString))
                        {
                            foreach (var column in dataTable.Columns)
                            {
                                if (column.ToString() != "新消費税適用" & column.ToString() != "新消費税率" & column.ToString() != "消費税率")
                                {
                                    bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());
                                }
                            }
                            bulkCopy.BulkCopyTimeout = 600; // in seconds
                            bulkCopy.DestinationTableName = "WT商品変換マスタ";
                            bulkCopy.WriteToServer(dataTable);
                        }

                        toksakicd = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString();
                        syohincd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Value.ToString();
                        syohincd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Value.ToString();
                        hyojijyun = this.gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value.ToString();

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();

                        //dataTable = 発注入力dataSet.T発注ファイル;

                        商品変換マスタメンテdataSet = ((商品変換マスタメンテDataSet4)(this.商品変換マスタメンテDataSet1));
                        //dataTable = 商品変換マスタメンテdataSet.WT商品変換マスタ;

                        T商品変換マスタtableAdapter = new T商品変換マスタTableAdapter();

                        try
                        {
                            //myTA.DeleteWK受注Bファイル();
                            //T受注ファイルtableAdapter.FillBy受注番号(受注ファイルdataSet.T受注ファイル, serchcode);
                            T商品変換マスタtableAdapter.商品変換マスタメンテ更新(toksakicd, syohincd_from, syohincd_to, "1");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                            return;
                        }

                    }
                    break;

            }

        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "得意先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    break;
                case "商品コード始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終textBoxCell");
                    }
                    break;
                case "商品コード終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    break;
                case "表示順radioGroupCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "消費税計算checkBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "消費税計算checkBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "消費税計算checkBoxCell");
                    }
                    break;
                case "消費税計算checkBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    break;
                case "検索buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "確認buttonCell");
                    }
                    break;
                case "クリアbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    break;
                case "終了buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                    }
                    break;
            }
        }

        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F3:
                    target = this.ButtonF3;
                    String cname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (cname == "検索得意先textBoxCell")
                    {
                        得意先検索Form fsToksaki;
                        fsToksaki = new 得意先検索Form();
                        fsToksaki.Owner = this;
                        fsToksaki.Show();
                    }
                    else if (cname == "検索商品textBoxCell")
                    {
                        商品検索Form fsSyohin;
                        fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                    }
                    break;
                case Keys.F5:
                    target = this.ButtonF5;
                    GcMultiRow gcMultiRow = this.gcMultiRow1;
                    foreach (Row selectedRow in gcMultiRow.SelectedRows.OrderByDescending(row => row.Index))
                    {
                    if (!selectedRow.IsNewRow)
                    gcMultiRow.Rows.RemoveAt(selectedRow.Index);
                    }
                    break;
                    /*
                    if (createData() != 0)
                    {
                        gcMultiRow1.ColumnFooters[0].Cells["回答日buttonCell"].Selectable = false;
                        gcMultiRow1.ColumnFooters[0].Cells["更新buttonCell"].Selectable = false;
                        //SendKeys.Send("{UP}");
                        MessageBox.Show("データがありません。", "得意先別商品別受注残問合せ");
                    }
                    else
                    {
                        gcMultiRow1.ColumnFooters[0].Cells["回答日buttonCell"].Selectable = true;
                        gcMultiRow1.ColumnFooters[0].Cells["更新buttonCell"].Selectable = true;
                    }
                    */
                case Keys.F9:    // 回答日
                    target = this.ButtonF9;
                    break;
                case Keys.F10:
                    target = this.ButtonF10;
                    this.Hide();
                    break;
                case Keys.Delete:
                    /*
                    target = this.ButtonDEL;
                    GcMultiRow gcMultiRow = this.gcMultiRow1;
                    foreach (Row selectedRow in gcMultiRow.SelectedRows.OrderByDescending(row => row.Index))
                    {
                    if (!selectedRow.IsNewRow)
                    gcMultiRow.Rows.RemoveAt(selectedRow.Index);
                    }
                    */
                    break;
                default:
                    throw new NotSupportedException();
            }

            //target.BackColor = SystemColors.ActiveCaption;
            //target.ForeColor = SystemColors.ActiveCaptionText;
            //target.Refresh();

            //0.2秒間待機
            //System.Threading.Thread.Sleep(200);

            //target.BackColor = SystemColors.Control;
            //target.ForeColor = SystemColors.ControlText;
        }

    }
}
