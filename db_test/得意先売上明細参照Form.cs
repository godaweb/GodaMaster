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

namespace db_test
{
    public partial class 得意先売上明細参照Form : Form
    {
        private string receiveDataSyohin = "";
        private string receiveDataSyohinE = "";
        private string receiveDataToksaki = "";

        //商品検索Form fsSyohin;
        //得意先検索Form fsToksaki;

        int rowcnt = 0;

        public 得意先売上明細参照Form()
        {
            InitializeComponent();
            //fsSyohin = new 商品検索Form();
            //fsSyohin.Owner = this;
            //fsToksaki = new 得意先検索Form();
            //fsToksaki.Owner = this;
        }

        private void 得意先売上明細参照Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 得意先売上明細参照Template();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            //this.gcMultiRow1.ScrollBars = ScrollBars.None;
            gcMultiRow1.ScrollBars = ScrollBars.Vertical;

            得意先売上明細参照Form.CheckForIllegalCrossThreadCalls = false; // スレッド間エラー 一時的対応
            //this.WindowState = FormWindowState.Maximized;

            //セル色の設定
            //非選択状態の色
            //gcMultiRow1.Rows[0].Cells[0].Style.BackColor = Color.Blue;
            //gcMultiRow1.Rows[0].Cells[0].Style.ForeColor = Color.White;


            // イベント
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            gcMultiRow1.CellContentClick +=new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);


            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellInRow, Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellInRow, Keys.Left);

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先売上明細参照FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先売上明細参照FunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先売上明細参照FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先売上明細参照FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先売上明細参照FunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // 初期表示
            DateTime dtNow = DateTime.Now;
            // 年 (Year) を取得する
            string dat = dtNow.Year.ToString() + "/" + dtNow.Month.ToString() + "/" + dtNow.Day.ToString();

            gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value = DateTime.Today.AddMonths(-1).ToString("yyyy/MM/dd");
            //gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value = DateTime.Today.AddMonths(-1).ToString("yyyy/MM/dd");
            //gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");
            gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");


            //選択状態のときの色
            gcMultiRow1.DefaultCellStyle.SelectionBackColor = Color.White;
            gcMultiRow1.DefaultCellStyle.SelectionForeColor = Color.Black;

            //編集状態のときの色
            gcMultiRow1.DefaultCellStyle.EditingBackColor = Color.Yellow;
            gcMultiRow1.DefaultCellStyle.EditingForeColor = Color.Black;

            //無効のときの色
            gcMultiRow1.DefaultCellStyle.DisabledBackColor = Color.White;
            gcMultiRow1.DefaultCellStyle.DisabledForeColor = Color.Black;

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日始textBoxCell");

            // ReadOnly
            //gcMultiRow1.ColumnHeaders[0].Cells["検索商品名textBoxCell"].ReadOnly = true;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードS"].Enabled = true;
        }

        
        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            string buf = "";

            switch (e.CellName)
            {
                case "売上日始textBoxCell":
                    if (e.FormattedValue != null)
                    {
                        buf = "";
                        buf = e.FormattedValue.ToString();
                        buf = buf.Replace("/", "");
                        if (buf.Length == 4)
                        {
                            // 必要な変数を宣言する
                            DateTime dtNow = DateTime.Now;
                            // 年 (Year) を取得する
                            string iYear = dtNow.Year.ToString();
                            buf = iYear + buf;
                        }
                        else if (buf.Length == 6)
                        {
                            buf = "20" + buf;
                        }
                        else if (buf.Length == 8)
                        {
                            buf = buf;
                        }
                        else
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            e.Cancel = true;
                            return;
                        }
                        buf = buf.Substring(0, 4) + "/" + buf.Substring(4, 2) + "/" + buf.Substring(6, 2);
                        DateTime dt;
                        // DateTimeに変換できるかチェック
                        if (!DateTime.TryParse(buf, out dt))
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                        e.Cancel = true;
                    }
                    break;
                case "売上日終textBoxCell":
                    if (e.FormattedValue != null)
                    {
                        buf = "";
                        buf = e.FormattedValue.ToString();
                        buf = buf.Replace("/", "");
                        if (buf.Length == 4)
                        {
                            // 必要な変数を宣言する
                            DateTime dtNow = DateTime.Now;
                            // 年 (Year) を取得する
                            string iYear = dtNow.Year.ToString();
                            buf = iYear + buf;
                        }
                        else if (buf.Length == 6)
                        {
                            buf = "20" + buf;
                        }
                        else if (buf.Length == 8)
                        {
                            buf = buf;
                        }
                        else
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            e.Cancel = true;
                            return;
                        }
                        buf = buf.Substring(0, 4) + "/" + buf.Substring(4, 2) + "/" + buf.Substring(6, 2);
                        DateTime dt;
                        // DateTimeに変換できるかチェック
                        if (!DateTime.TryParse(buf, out dt))
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                        e.Cancel = true;
                    }
                    break;

            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {

            string buf = "";

            switch (e.CellName)
            {
                case "売上日始textBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value != null)
                    {
                        buf = "";
                        buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value;
                        buf = buf.Replace("/", "");
                        if (buf.Length == 4)
                        {
                            // 必要な変数を宣言する
                            DateTime dtNow = DateTime.Now;
                            // 年 (Year) を取得する
                            string iYear = dtNow.Year.ToString();
                            buf = iYear + buf;
                        }
                        else if (buf.Length == 6)
                        {
                            buf = "20" + buf;
                        }
                        else if (buf.Length == 8)
                        {
                            buf = buf;
                        }
                        else
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            return;
                        }
                        buf = buf.Substring(0, 4) + "/" + buf.Substring(4, 2) + "/" + buf.Substring(6, 2);
                        DateTime dt;
                        // DateTimeに変換できるかチェック
                        if (DateTime.TryParse(buf, out dt))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value = buf;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value = buf;
                        }
                        else
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                        }
                    }
                    else
                    {
                        MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                        return;
                    }
                    break;
                case "売上日終textBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value != null)
                    {
                        buf = "";
                        buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value;
                        buf = buf.Replace("/", "");
                        if (buf.Length == 4)
                        {
                            // 必要な変数を宣言する
                            DateTime dtNow = DateTime.Now;
                            // 年 (Year) を取得する
                            string iYear = dtNow.Year.ToString();
                            buf = iYear + buf;
                        }
                        else if (buf.Length == 6)
                        {
                            buf = "20" + buf;
                        }
                        else if (buf.Length == 8)
                        {
                            buf = buf;
                        }
                        else
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            return;
                        }
                        buf = buf.Substring(0, 4) + "/" + buf.Substring(4, 2) + "/" + buf.Substring(6, 2);
                        DateTime dt;
                        // DateTimeに変換できるかチェック
                        if (DateTime.TryParse(buf, out dt))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value = buf;
                        }
                        else
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                        }
                    }
                    else
                    {
                        MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                        return;
                    }
                    break;
                case "txt商品コードS":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードS"].Value != null)
                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードE"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードS"].Value;
                    else
                    // 空白
                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードE"].Value = null;

                    break;
            }
        }

        
        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {

            if (e.CellName == "検索得意先textBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["検索得意先textBoxCell"].Value != null)
                {
                    DataTable table = new DataTable();
                    String sql = "SELECT TOK.得意先名, TOK.電話１, TOK.電話２, TOK.電話３, ";
                    sql = sql + "TOK.ＦＡＸ電話１, TOK.ＦＡＸ電話２, TOK.ＦＡＸ電話３, ";
                    sql = sql + "TOK.担当者コード, TAN.担当者名, TOK.郵便番号, ";
                    sql = sql + "TOK.都道府県名, TOK.住所１, TOK.住所２, TOK.現在売掛金残高, TOK.検収区分 ";
                    sql = sql + "FROM T得意先マスタ TOK ";
                    sql = sql + "LEFT JOIN T担当者マスタ TAN ";
                    sql = sql + "ON TOK.担当者コード = TAN.担当者コード ";
                    sql = sql + "WHERE TOK.得意先コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["検索得意先textBoxCell"].Value + "'";

                    table = Utility.GetComboBoxData(sql);

                    if (table.Rows.Count > 0)
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["検索得意先名textBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["得意先名"]) ? null : table.Rows[0]["得意先名"]);
                        gcMultiRow1.ColumnHeaders[0].Cells["売掛金残高textBoxCell"].Value = (DBNull.Value.Equals(table.Rows[0]["現在売掛金残高"]) ? null : table.Rows[0]["現在売掛金残高"]);
                    }
                }
                else
                {
                    gcMultiRow1.ColumnHeaders[0].Cells["検索得意先名textBoxCell"].Value = DBNull.Value;
                }

                /*
                int ret = execProcedure();

                if (ret == 0)
                {
                    //SendKeys.Send("{ENTER}");
                    //SendKeys.Send("{UP}");
                }
                else if (ret == 1)
                {
                    MessageBox.Show("データがありません。", "得意先売上明細参照");
                }
                else
                {
                    MessageBox.Show("データ取得エラー。", "得意先売上明細参照");
                }
                */
            }

            if (e.CellName == "txt商品コードS")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードS"].Value != null)
                {
                    DataTable table = new DataTable();
                    String sql = "SELECT 商品名 FROM T商品マスタ ";
                    sql = sql + "WHERE 商品コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードS"].Value + "'";

                    table = Utility.GetComboBoxData(sql);

                    if (table.Rows.Count > 0)
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt商品名S"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["商品名"]) ? null : table.Rows[0]["商品名"]);
                    }
                }
                else
                {
                    gcMultiRow1.ColumnHeaders[0].Cells["txt商品名S"].Value = DBNull.Value;
                }
                /*
                if (execProcedure() == 0)
                {
                    //SendKeys.Send("{ENTER}");
                    SendKeys.Send("{UP}");
                }
                else if (execProcedure() == 1)
                {
                    MessageBox.Show("データがありません。", "得意先売上明細参照");
                }
                else
                {
                    MessageBox.Show("データ取得エラー。", "得意先売上明細参照");
                }
                */

            }

            if (e.CellName == "txt商品コードE")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードE"].Value != null)
                {
                    DataTable table = new DataTable();
                    String sql = "SELECT 商品名 FROM T商品マスタ ";
                    sql = sql + "WHERE 商品コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードE"].Value + "'";

                    table = Utility.GetComboBoxData(sql);

                    if (table.Rows.Count > 0)
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt商品名E"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["商品名"]) ? null : table.Rows[0]["商品名"]);
                    }
                } 
                else
                {
                    gcMultiRow1.ColumnHeaders[0].Cells["txt商品名E"].Value = DBNull.Value;
                }

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
                        得意先検索Form fsToksaki = new 得意先検索Form();
                        fsToksaki = new 得意先検索Form();
                        fsToksaki.Owner = this;
                        fsToksaki.Show();
                    }
                    else if (cname == "txt商品コードS" || cname == "txt商品コードE")
                    {
                        商品検索Form fsSyohin = new 商品検索Form();
                        fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                    }
                    break;
                case Keys.F4:
                    target = this.ButtonF4;
                    // リスト
                    /*
                    if (createReportData() == 0)
                    {
                        プレビューForm プレビューform = new プレビューForm();

                        プレビューform.dataSet = dataSet;
                        プレビューform.rptName = "受注明細参照リストCrystalReport";
                        プレビューform.Show();
                    }
                    else
                    {
                        MessageBox.Show("データがありません", "仕入先別発注残明細表");
                    }
                    */
                    break;
                case Keys.F5:
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    target = this.ButtonF5;

                    int ret = execProcedure();
                    if (ret == 0)
                    {
                    }
                    else if (ret == 1)
                    {
                        MessageBox.Show("データがありません。", "得意先売上明細参照");
                    }
                    else
                    {
                        MessageBox.Show("データ取得エラー。", "得意先売上明細参照");
                    }

                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                    
                    break;
                case Keys.F9:
                    target = this.ButtonF9;
                    //gcMultiRow1.Rows.Clear();
                    try
                    {
                        //DataTableオブジェクトを用意
                        DataTable dataTable = (DataTable)gcMultiRow1.DataSource;
                        dataTable.Clear();
                    }
                    catch (DataException e)
                    {
                        // Process exception and return.
                        Console.WriteLine("Exception of type {0} occurred.",
                            e.GetType());
                    }
                    this.Refresh();
                    this.gcMultiRow1.Select();
                    this.gcMultiRow1.EditMode = EditMode.EditOnEnter;
                    this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                    break;
                case Keys.F10:
                    target = this.ButtonF10;
                    this.Close();
                    break;
                default:
                    break;
            }

            target.BackColor = SystemColors.ActiveCaption;
            target.ForeColor = SystemColors.ActiveCaptionText;
            target.Refresh();

            //0.2秒間待機
            System.Threading.Thread.Sleep(200);

            target.BackColor = SystemColors.Control;
            target.ForeColor = SystemColors.ControlText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //fsSyohin.SendData = "Penguin";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //fsToksaki.SendData = "Penguin";
        }

        public string ReceiveDataSyohin
        {
            set
            {
                receiveDataSyohin = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードS"].Value = receiveDataSyohin.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataSyohin;
            }
        }

        public string ReceiveDataToksaki
        {
            set
            {
                receiveDataToksaki = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["検索得意先textBoxCell"].Value = receiveDataToksaki.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataToksaki;
            }
        }

        private void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "検索buttonCell")
            {
                int ret = execProcedure();
                if (ret == 0)
                {
                    //gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt商品コードS");
                }
                else if (ret == 1)
                {
                    MessageBox.Show("データがありません。", "得意先売上明細参照");
                    //gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                    //cancelFlag = 0;
                    //afterCellName = "日付始textBoxCell";
                }
                else
                {
                    MessageBox.Show("データ取得エラー。", "得意先売上明細参照");
                    //gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                }
            }
            else if (e.CellName == "終了buttonCell")
            {
                this.Close();
            }
            else if (e.CellName == "消込buttonCell")
            {
                /*
                if (createReportData() == 0)
                {
                    プレビューForm プレビューform = new プレビューForm();

                    プレビューform.dataSet = dataSet;
                    プレビューform.rptName = "受注明細参照リストCrystalReport";
                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "仕入先別発注残明細表");
                }
                */
            }
            else if (e.CellName == "クリアbuttonCell")
            {
                if (gcMultiRow1.Rows != null)
                {
                    gcMultiRow1.DataSource = null;
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                }
            }
        }


        void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "検索buttonCell":
                    int ret = execProcedure();
                    if (ret == 0)
                    {
                    }
                    else if (ret == 1)
                    {
                        MessageBox.Show("データがありません。", "受注明細参照");
                    }
                    else
                    {
                        MessageBox.Show("データ取得エラー。", "受注明細参照");
                    }

                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");

                    break;
            }

        }
        private int execProcedure()
        {
            //Template template1 = new 商品問合せTemplate();
            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;
            // ストアド プロシージャ名を指定
            command.CommandText = "得意先売上明細参照";
            if (gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@売上日始", (String)gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@売上日始", DBNull.Value);
            }
            if (gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@売上日終", (String)gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@売上日終", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["検索得意先textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先コード", (String)gcMultiRow1.ColumnHeaders[0].Cells["検索得意先textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先コード", DBNull.Value);
            }

            // txt商品コードS
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードS"].Value != null)
            {
                command.Parameters.AddWithValue("@商品コードS", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードS"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品コードS", DBNull.Value);
            }

            // txt商品コードE
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードE"].Value != null)
            {
                command.Parameters.AddWithValue("@商品コードE", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt商品コードE"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品コードE", DBNull.Value);
            }

            // txt伝票摘要
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt伝票摘要"].Value != null)
            {
                command.Parameters.AddWithValue("@伝票摘要", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt伝票摘要"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@伝票摘要", DBNull.Value);
            }

            // txt明細摘要
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt明細摘要"].Value != null)
            {
                command.Parameters.AddWithValue("@明細摘要", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt明細摘要"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@明細摘要", DBNull.Value);
            }


            command.Parameters.AddWithValue("@検収チェック", 0);
            command.Parameters.AddWithValue("@入金チェック", 0);

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            try
            {
                adapter.Fill(dataSet);
            }
            catch (SqlException e)
            {
                if (e.Number == 547)
                {
                    return 9;
                    throw;
                }
                if (e.Number == 241)
                {
                    return 9;
                    throw;
                }
                if (e.Number == 242)
                {
                    return 9;
                    throw;
                }
            }

            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            dataTable.AcceptChanges();

            this.gcMultiRow1.DataSource = dataTable;

            //dataSet.Tables[0].Rows[0]["得意先コード"].ToString();

            //gcMultiRow1.EditMode = GrapeCity.Win.MultiRow.EditMode.EditOnKeystrokeOrShortcutKey;

            /*
            foreach (DataRow dr in dataTable.Rows)
            {

                if (dataTable.Rows.Count == rowcnt - 1)
                {
                    Console.WriteLine("最終データ");
                    gcMultiRow1.EndEdit();
                    connection.Close();
                    break;
                }

                this.gcMultiRow1.Rows.Insert(rowcnt);

                foreach (DataColumn dc in dataTable.Columns)
                {
                    Console.Write(dr[dc] + "\t");
                    switch (dc.ColumnName)
                    {
                        case "納入日":
                            DateTime jyuDate = DateTime.Parse(dr[dc].ToString());
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["納入日textBoxCell"].CellIndex, jyuDate.ToString("yyyy/MM/dd"));
                            break;
                        case "原価単価":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["原価単価numericUpDownCell"].CellIndex, dr[dc]);
                            break;
                        case "伝票摘要":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["伝票摘要textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "明細摘要":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["明細摘要textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "納名":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["納名textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "売上伝票番号":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["売上伝票番号textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "売上日":
                            DateTime uriDate = DateTime.Parse(dr[dc].ToString());
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["売上日textBoxCell"].CellIndex, uriDate.ToString("yyyy/MM/dd"));
                            break;
                        case "売上区分コード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["売上区分textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "請求月区分コード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["請求月区分textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "担当者コード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["担当者コードtextBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "商品コード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["商品コードtextBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "商名":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["商名textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "数量":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["数量numericUpDownCell"].CellIndex, dr[dc]);
                            break;
                        case "売上単価":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["売上単価numericUpDownCell"].CellIndex, dr[dc]);
                            break;
                        case "税抜売上金額":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["税抜売上金額numericUpDownCell"].CellIndex, dr[dc]);
                            break;
                        case "消費税":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["消費税numericUpDownCell"].CellIndex, dr[dc]);
                            break;
                        case "規格":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["規格textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "相手先注文番号":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["相手先注文番号textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "仕入先コード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["仕入先コードtextBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "入金チェック":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["入金チェックcheckBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "検収チェック":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["検収チェックcheckBoxCell"].CellIndex, dr[dc]);
                            break;

                    }
                }

                rowcnt++;
                gcMultiRow1.EndEdit();
                connection.Close();
            }
            */

            if (gcMultiRow1.Rows.Count == 0)
            {
                return 1;
            }

            return 0;

        }
    }
}
