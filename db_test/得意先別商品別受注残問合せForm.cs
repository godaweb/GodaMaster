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
    public partial class 得意先別商品別受注残問合せForm : Form
    {

        private string receiveDataSyohin = "";
        private string receiveDataToksaki = "";
        //商品検索Form fsSyohin;
        //得意先検索Form fsToksaki;

        int rowcnt = 0;
        object toksakicd = "";
        object EOSKBN = "";
        object kyakuTyuKBN = "";
        object sirnm = "";
        object hatyuno = "";
        object tantocd = "";
        object jyutyuno = "";
        object jyutyugyono = "";
        int hatyuzan = 0;
        int jyutyuzan = 0;
        int jyutyuzansu = 0;
        int genzaiko = 0;
        int kake = 0;
        int zaiko = 0;
        object hatyuhakoFLG = "";
        object urikbncd = "";
        object tel1 = "";
        object tel2 = "";
        object tel3 = "";

        public enum DeleteAuthorResult
        {
             Success,
             FailureByNotFoundAuId,
             FailureByExistingTitles
        }

        public 得意先別商品別受注残問合せForm()
        {
            InitializeComponent();
            //fsSyohin = new 商品検索Form();
            //fsSyohin.Owner = this;
            //fsToksaki = new 得意先検索Form();
            //fsToksaki.Owner = this;
        }

        private void 得意先別商品別受注残Form_Load(object sender, EventArgs e)
        {

            this.gcMultiRow1.Template = new 得意先別商品別受注残問合せTemplate();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            gcMultiRow1.ScrollBars = ScrollBars.Vertical;

            //20190414 Start
            //this.WindowState = FormWindowState.Maximized;
            //20190414 End

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
            gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            //gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            //gcMultiRow1.DataError +=new EventHandler<DataErrorEventArgs>(gcMultiRow1_DataError);
            //gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            //gcMultiRow1.PreviewKeyDown +=new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            //gcMultiRow1.CellClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellClick);
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
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);    // 得意先、商品検索画面表示
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F4);    // 回答日　入力
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);    // 検索実行
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F6);    // 画面クリア
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);    // 更新
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);   // 終了

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先別商品別受注残問合せFunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先別商品別受注残問合せFunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先別商品別受注残問合せFunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先別商品別受注残問合せFunctionKeyAction(Keys.F6), Keys.F6);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先別商品別受注残問合せFunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先別商品別受注残問合せFunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value = 0;

            gcMultiRow1.ColumnHeaders[0].Cells["txt注意事項"].Style.Multiline = MultiRowTriState.True;
            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt担当者S");

        }


        // 項目の最後で検索
        void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "実行buttonCell":
                    if (createData() != 0)
                    {
                        gcMultiRow1.ColumnFooters[0].Cells["回答日buttonCell"].Selectable = false;
                        gcMultiRow1.ColumnFooters[0].Cells["更新buttonCell"].Selectable = false;
                        //SendKeys.Send("{UP}");
                        MessageBox.Show("データがありません。", "得意先別商品別受注残問合せ");
                        this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt担当者S");
                    }
                    else
                    {
                        gcMultiRow1.ColumnFooters[0].Cells["回答日buttonCell"].Selectable = true;
                        gcMultiRow1.ColumnFooters[0].Cells["更新buttonCell"].Selectable = true;
                    }
                    break;
                //case "クリアbuttonCell":
                //    gcMultiRow1.Rows.Clear();
                //    this.Refresh();

                //    gcMultiRow1.ColumnHeaders[0].Cells["検索得意先名textBoxCell"].Value = null;
                //    gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬtextBoxCell"].Value = null;
                //    gcMultiRow1.ColumnHeaders[0].Cells["担当textBoxCell"].Value = null;
                //    gcMultiRow1.ColumnHeaders[0].Cells["郵便番号textBoxCell"].Value = null;
                //    gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = null;
                //    gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = null;
                //    gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = null;
                //    gcMultiRow1.ColumnHeaders[0].Cells["txt注意事項"].Value = null;
                //    gcMultiRow1.EndEdit();
 
                //    this.gcMultiRow1.Select();
                //    this.gcMultiRow1.EditMode = EditMode.EditOnEnter;
                //    this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt検索得意先S");
                //    break;
                case "終了buttonCell":
                    this.Hide();
                    break;
            }

        }

        private void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "クリアbuttonCell":
                    if (mClear() == 0)
                    {
                    }
                    break;

                case "回答日buttonCell":
                    if (mkaitou() == 0)
                    {
                    }
                    break;

                case "更新buttonCell": // 保留
                    break;

            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "txt担当者S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt担当者S"].Value != null)
                    {
                            gcMultiRow1.ColumnHeaders[0].Cells["txt担当者E"].Value = gcMultiRow1.ColumnHeaders[0].Cells["txt担当者S"].Value;
                    }
                    break;

                case "txt検索得意先S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先S"].Value != null)
                    {
                        DataTable table = new DataTable();
                        String sql = "SELECT TOK.得意先名, TOK.電話１, TOK.電話２, TOK.電話３, ";
                        sql = sql + "TOK.ＦＡＸ電話１, TOK.ＦＡＸ電話２, TOK.ＦＡＸ電話３, ";
                        sql = sql + "TOK.担当者コード, TAN.担当者名, TOK.郵便番号, ";
                        sql = sql + "TOK.都道府県名, TOK.住所１, TOK.住所２, TOK.注意事項 ";
                        sql = sql + "FROM T得意先マスタ TOK ";
                        sql = sql + "LEFT JOIN T担当者マスタ TAN ";
                        sql = sql + "ON TOK.担当者コード = TAN.担当者コード ";
                        sql = sql + "WHERE TOK.得意先コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先S"].Value + "'";

                        table = Utility.GetComboBoxData(sql);

                        if (table.Rows.Count > 0)
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["検索得意先名textBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["得意先名"]) ? null : table.Rows[0]["得意先名"]);
                            if ((string)(DBNull.Value.Equals(table.Rows[0]["電話３"]) ? null : table.Rows[0]["電話３"]) != null)
                            {
                                gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬtextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["電話１"]) ? null : table.Rows[0]["電話１"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["電話２"]) ? null : table.Rows[0]["電話２"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["電話３"]) ? null : table.Rows[0]["電話３"]);
                            }
                            if ((string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話３"]) ? null : table.Rows[0]["ＦＡＸ電話３"]) != null)
                            {
                                gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬtextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話１"]) ? null : table.Rows[0]["ＦＡＸ電話１"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話２"]) ? null : table.Rows[0]["ＦＡＸ電話２"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話３"]) ? null : table.Rows[0]["ＦＡＸ電話３"]);
                            }
                            gcMultiRow1.ColumnHeaders[0].Cells["担当textBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["担当者名"]) ? null : table.Rows[0]["担当者名"]);
                            gcMultiRow1.ColumnHeaders[0].Cells["郵便番号textBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["郵便番号"]) ? null : table.Rows[0]["郵便番号"]);
                            gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["住所１"]) ? null : table.Rows[0]["住所１"]);
                            gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["住所２"]) ? null : table.Rows[0]["住所２"]);
                            gcMultiRow1.ColumnHeaders[0].Cells["txt注意事項"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["注意事項"]) ? null : table.Rows[0]["注意事項"]);

                            gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先E"].Value = gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先S"].Value;

                            break;
                        }
                        else
                        {
                            MessageBox.Show("得意先マスタにありません。", "得意先別商品別受注残問合せ");
                            this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt検索得意先S");
                        }
                    }else
                    {
                    }

                    gcMultiRow1.ColumnHeaders[0].Cells["検索得意先名textBoxCell"].Value = null;
                    gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬtextBoxCell"].Value = null;
                    gcMultiRow1.ColumnHeaders[0].Cells["担当textBoxCell"].Value = null;
                    gcMultiRow1.ColumnHeaders[0].Cells["郵便番号textBoxCell"].Value = null;
                    gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = null;
                    gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = null;
                    gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = null;
                    gcMultiRow1.ColumnHeaders[0].Cells["txt注意事項"].Value = null;
                    gcMultiRow1.EndEdit();

                    break;
                case "txt検索商品S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品S"].Value != null)
                    {
                        DataTable table = new DataTable();
                        String sql = "SELECT 商品名 FROM T商品マスタ ";
                        sql = sql + "WHERE 商品コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品S"].Value + "'";

                        table = Utility.GetComboBoxData(sql);

                        if (table.Rows.Count > 0)
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["検索商品名textBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["商品名"]) ? null : table.Rows[0]["商品名"]);
                        }
                        else
                        {
                            MessageBox.Show("商品マスタにありません。", "得意先別商品別受注残問合せ");
                            gcMultiRow1.ColumnHeaders[0].Cells["検索商品名textBoxCell"].Value = null;
                        }
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品S"].Value = null;
                    }

                    gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品E"].Value = gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品S"].Value;

                    break;
                case "txt受注日S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value != null)
                    {
                        string buf = "";
                        buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value;
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
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt受注日S");
                            return;
                        }
                        buf = buf.Substring(0, 4) + "/" + buf.Substring(4, 2) + "/" + buf.Substring(6, 2);
                        DateTime dt;
                        // DateTimeに変換できるかチェック
                        if (DateTime.TryParse(buf, out dt))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value = buf;
                        }
                        else
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt受注日S");
                        } 
                    }
                    break;
                case "txt受注日E":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value != null)
                    {
                        string buf = "";
                        buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value;
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
                            this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value = buf;
                        }
                        else
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                        }
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.CellName == "表示順radioGroupCell")
            {
                if (this.gcMultiRow1.RowCount > 0)
                {
                    if (gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value.Equals(0))
                    {
                        // 最初のセルを基準に昇順で並び替える
                        gcMultiRow1.Sort(0, System.Windows.Forms.SortOrder.Ascending);
                    }
                    else
                    {
                        // 最初のセルを基準に昇順で並び替える
                        gcMultiRow1.Sort(1, System.Windows.Forms.SortOrder.Ascending);
                    }
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
                    EditingActions.CommitRow.Execute(gcMultiRow1);
                    String cname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (cname == "txt検索得意先S")
                    {
                        得意先検索Form fsToksaki;
                        fsToksaki = new 得意先検索Form();
                        fsToksaki.Owner = this;
                        fsToksaki.Show();
                    }
                    else if (cname == "txt検索商品S")
                    {
                        商品検索Form fsSyohin;
                        fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                    }
                    break;
                case Keys.F4:    // 回答日
                    target = this.ButtonF4;
                    if (mkaitou() == 0)
                    {
                    }                                   
                    break;
                case Keys.F5:
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    target = this.ButtonF5;
                    if (createData() != 0)
                    {
                        gcMultiRow1.ColumnFooters[0].Cells["回答日buttonCell"].Selectable = false;
                        gcMultiRow1.ColumnFooters[0].Cells["更新buttonCell"].Selectable = false;
                        //SendKeys.Send("{UP}");
                        MessageBox.Show("データがありません。", "得意先別商品別受注残問合せ");
                        this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt担当者S");
                    }
                    else
                    {
                        gcMultiRow1.ColumnFooters[0].Cells["回答日buttonCell"].Selectable = true;
                        gcMultiRow1.ColumnFooters[0].Cells["更新buttonCell"].Selectable = true;
                    }
                    break;
                case Keys.F6:
                    target = this.ButtonF6;
                    if (mClear() == 0)
                    {
                    }
                    break;
                case Keys.F9:    // 回答日
                    target = this.ButtonF9;
                    break;
                case Keys.F10:
                    target = this.ButtonF10;
                    this.Hide();
                    break;
                default:
                    break;
            }

            //target.BackColor = SystemColors.ActiveCaption;
            //target.ForeColor = SystemColors.ActiveCaptionText;
            //target.Refresh();

            //0.2秒間待機
            //System.Threading.Thread.Sleep(200);

            //target.BackColor = SystemColors.Control;
            //target.ForeColor = SystemColors.ControlText;
        }


        // 検索の戻り値
        public string ReceiveDataSyohin
        {
            set
            {
                receiveDataSyohin = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品S"].Value = receiveDataSyohin.ToString();
                //SendKeys.Send("{ENTER}");
                this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt受注日S");
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
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先S"].Value = receiveDataToksaki.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataToksaki;
            }
        }


        private void 終了button_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private int mClear()
        {
            gcMultiRow1.Rows.Clear();
            this.Refresh();

            gcMultiRow1.ColumnHeaders[0].Cells["txt担当者S"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["txt担当者E"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先S"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先E"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["検索得意先名textBoxCell"].Value = null;

            gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品S"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品E"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["検索商品名textBoxCell"].Value = null;

            gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value = null;

            gcMultiRow1.ColumnHeaders[0].Cells["txt回答名"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["txt明細摘要"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["txt伝票摘要"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["txt発注摘要"].Value = null;

            gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬtextBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["ＦＡＸtextBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["担当textBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["郵便番号textBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["txt注意事項"].Value = null;

            gcMultiRow1.EndEdit();

            this.gcMultiRow1.Select();
            this.gcMultiRow1.EditMode = EditMode.EditOnEnter;
            this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt担当者S");

            return 1;
        }

        private int mkaitou()
        {
            for (int i = 0; i < this.gcMultiRow1.RowCount - 1; i++)
            {
                gcMultiRow1.Rows[i].Cells["回答コードtextBoxCell"].ReadOnly = false;
                gcMultiRow1.Rows[i].Cells["回答納期textBoxCell"].ReadOnly = false;
                gcMultiRow1.Rows[i].Cells["回答名textBoxCell"].ReadOnly = false;
            }

            return 1;
        }

        private int createData()
        {
            int ret = 0;
            
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
            command.CommandText = "得意先別商品別受注残問合せ";

            // 担当者
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt担当者S"].Value != null)
            {
                command.Parameters.AddWithValue("@担当者コードS", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt担当者S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@担当者コードS", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt担当者E"].Value != null)
            {
                command.Parameters.AddWithValue("@担当者コードE", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt担当者E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@担当者コードE", DBNull.Value);
            }
            // 得意先
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先S"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先コード", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先コード", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先E"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先コードE", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先コードE", DBNull.Value);
            }
            // 商品
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品S"].Value != null)
            {
                command.Parameters.AddWithValue("@商品コード", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品コード", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品E"].Value != null)
            {
                command.Parameters.AddWithValue("@商品コードE", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品コードE", DBNull.Value);
            }
            // 受注日
            if (gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value != null)
            {
                DateTime jyuDate = DateTime.Parse(gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value.ToString());
                command.Parameters.AddWithValue("@受注日", jyuDate.ToString("yyyy/MM/dd"));
            }
            else
            {
                command.Parameters.AddWithValue("@受注日", DBNull.Value);
            }
            if (gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value != null)
            {
                DateTime jyuDate = DateTime.Parse(gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value.ToString());
                command.Parameters.AddWithValue("@受注日E", jyuDate.ToString("yyyy/MM/dd"));
            }
            else
            {
                command.Parameters.AddWithValue("@受注日E", DBNull.Value);
            }
            // 回答
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt回答名"].Value != null)
            {
                command.Parameters.AddWithValue("@回答名", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt回答名"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@回答名", DBNull.Value);
            }
            // 明細摘要
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt明細摘要"].Value != null)
            {
                command.Parameters.AddWithValue("@明細摘要", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt明細摘要"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@明細摘要", DBNull.Value);
            }
            // 伝票摘要
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt伝票摘要"].Value != null)
            {
                command.Parameters.AddWithValue("@伝票摘要", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt伝票摘要"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@伝票摘要", DBNull.Value);
            }
            // 発注摘要
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt発注摘要"].Value != null)
            {
                command.Parameters.AddWithValue("@発注摘要", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt発注摘要"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注摘要", DBNull.Value);
            }

            // 表示順番
            if ((int)gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value == 0)
            {
                command.Parameters.AddWithValue("@表示順", "1");
            }
            else
            {
                command.Parameters.AddWithValue("@表示順", "2");
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(dataSet);

            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];
            dataTable.AcceptChanges();

            //dataTable.AcceptChanges();
            if (dataTable.Rows.Count <= 0)
            {
                ret = 1;
            }

            //this.gcMultiRow1.DataSource = dataSet;

            //dataSet.Tables[0].Rows[0]["得意先コード"].ToString();

            /*
            if (dataTable.Rows == null)
            {
                return;
            }else{
                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("データがありません","得意先別商品受注残問合せ");
                    gcMultiRow1.Select();
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "表示順radioGroupCell");
                    return;
                }
            }
             */

            if (dataTable.Rows.Count == 0)
            {
                return 1;
            }
            else
            {
                gcMultiRow1.EditMode = GrapeCity.Win.MultiRow.EditMode.EditOnKeystrokeOrShortcutKey;

                rowcnt = 0;

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

                    //                    rowcnt = 0;
                    toksakicd = "";
                    EOSKBN = "";
                    kyakuTyuKBN = "";
                    sirnm = "";
                    hatyuno = "";
                    tantocd = "";
                    jyutyuno = "";
                    jyutyugyono = "";
                    hatyuzan = 0;
                    jyutyuzan = 0;
                    jyutyuzansu = 0;
                    genzaiko = 0;
                    zaiko = 0;
                    hatyuhakoFLG = "";
                    urikbncd = "";
                    tel1 = "";
                    tel2 = "";
                    tel3 = "";
                    kake = 0;

                    foreach (DataColumn dc in dataTable.Columns)
                    {
                        Console.Write(dr[dc] + "\t");
                        switch (dc.ColumnName)
                        {
                            case "得意先コード":
                                toksakicd = dr[dc];
                                //                                this.gcMultiRow1.SetValue(0, gcMultiRow1.Template.Row.Cells["得意先コードtextBoxCell"].CellIndex, dr[dc]);
                                //this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["得意先コードtextBoxCell"].CellIndex, dr[dc]);
                                //                                gcMultiRow1.Rows[0].Cells[11].Value = dr[dc];
                                //Console.WriteLine("得意先。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "受注番号":
                                jyutyuno = dr[dc];
                                //this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注番号textBoxCell"].CellIndex, dr[dc]);
                                //Console.WriteLine("受注番号。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "受注行番号":
                                jyutyugyono = dr[dc];
                                //this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注行番号textBoxCell"].CellIndex, dr[dc]);
                                //Console.WriteLine("受注行番号。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "EOS区分":
                                EOSKBN = dr[dc];
                                //Console.WriteLine("EOS。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "客注区分":
                                kyakuTyuKBN = dr[dc];
                                //Console.WriteLine("客注区分。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "メーカーコード":
                                //Console.WriteLine("メーカーコード。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "メーカー名":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["メーカー名textBoxCell"].CellIndex, dr[dc]);
                                //Console.WriteLine("メーカー名。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            //case "商品コード":
                            //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["商品コードtextBoxCell"].CellIndex, dr[dc]);
                            //    //Console.WriteLine("商品。");　//コンソール画面に「赤色です」と出力する
                            //    break;
                            case "商名":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["商名textBoxCell"].CellIndex, dr[dc]);
                                //Console.WriteLine("商品。");　//コンソール画面に「赤色です」と出力する。
                                //this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["商名textBoxCell"].CellIndex, dr[dc]);
                                break;
                            case "明細摘要":
                                //Console.WriteLine("明細摘要。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "略名":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["略名textBoxCell"].CellIndex, dr[dc]);
                                //Console.WriteLine("略名。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "受注日":
                                if (dr[dc].ToString() != "")
                                {
                                    DateTime jyuDate = DateTime.Parse(dr[dc].ToString());
                                    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注日textBoxCell"].CellIndex, jyuDate.ToString("yyyy/MM/dd"));
                                    //Console.WriteLine("受注日。");　//コンソール画面に「赤色です」と出力する。
                                }
                                break;
                            case "回答コード":
                                //Console.WriteLine("回答コード。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "回答名":
                                //Console.WriteLine("回答名。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "回答納期":
                                //Console.WriteLine("回答納期。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "受注残":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注残numericUpDownCell"].CellIndex, dr[dc]);
                                if (dr[dc] != DBNull.Value)
                                {
                                    jyutyuzan = Convert.ToInt32(dr[dc]);
                                }
                                //Console.WriteLine("受注残。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "発注残":
                                if (dr[dc] != DBNull.Value)
                                {
                                    hatyuzan = Convert.ToInt32(dr[dc]);
                                }
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["発注残numericUpDownCell"].CellIndex, dr[dc]);
                                //Console.WriteLine("発注残。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "納品掛率":
                                if (dr[dc] != DBNull.Value)
                                {
                                    kake = (int)(Convert.ToDouble(dr[dc]) * 100);
                                }
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["納品掛率numericUpDownCell"].CellIndex, kake);
                                //Console.WriteLine("納品掛率。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "受注単価":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注単価numericUpDownCell"].CellIndex, dr[dc]);
                                //Console.WriteLine("受注単価。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "発注番号":
                                hatyuno = dr[dc];
                                //Console.WriteLine("発注番号。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "仕入先コード":
                                //Console.WriteLine("仕入先。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "仕名":
                                sirnm = dr[dc];
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["仕名textBoxCell"].CellIndex, sirnm);
                                //Console.WriteLine("仕名。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "メーカー品番":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["メーカー品番textBoxCell"].CellIndex, dr[dc]);
                                //Console.WriteLine("メーカー品番。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "電話１":
                                tel1 = dr[dc];
                                //Console.WriteLine("電話1。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "電話２":
                                tel2 = dr[dc];
                                //Console.WriteLine("電話2。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "電話３":
                                tel3 = dr[dc];
                                //Console.WriteLine("電話3。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "売上区分コード":
                                urikbncd = dr[dc];
                                //Console.WriteLine("売上区分コード。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "現在在庫数":
                                if (dr[dc] != DBNull.Value)
                                {
                                    genzaiko = Convert.ToInt32(dr[dc]);
                                }
                                //Console.WriteLine("現在在庫数。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "T受注残数":
                                if (dr[dc] != DBNull.Value)
                                {
                                    jyutyuzansu = Convert.ToInt32(dr[dc]);
                                    Console.WriteLine(jyutyuzansu);
                                }
                                break;
                            case "発注書発行フラグ":
                                hatyuhakoFLG = dr[dc];
                                //Console.WriteLine("発注書発行フラグ");　//コンソール画面に「黄色です」と出力する。
                                break;
                            case "担当者コード":
                                tantocd = dr[dc];
                                //Console.WriteLine("担当者コード");　//コンソール画面に「黄色です」と出力する。
                                break;
                            case "定価":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txt定価"].CellIndex, dr[dc]);
                                break;
                            case "伝票摘要":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txt伝票摘要明細"].CellIndex, dr[dc]);
                                break;
                            case "発注摘要":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txt発注摘要明細"].CellIndex, dr[dc]);                   
                                break;
                            case "在庫数":
                                if (dr[dc] != DBNull.Value)
                                {
                                    zaiko = Convert.ToInt32(dr[dc]);
                                }
                                break;
                        }
                    }

                    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["得意先コードtextBoxCell"].CellIndex, toksakicd);
                    if (jyutyuno != null)
                    {
                        jyutyuno.ToString().PadLeft(6, '0');
                        jyutyuno = jyutyuno + "-" + jyutyugyono;
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注NOtextBoxCell"].CellIndex, jyutyuno);
                    }
                    if (EOSKBN.Equals("21"))
                    {
                        if (kyakuTyuKBN.Equals("1"))
                        {
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["EOStextBoxCell"].CellIndex, "EOS外");
                        }
                        else if (kyakuTyuKBN.Equals("2"))
                        {
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["EOStextBoxCell"].CellIndex, "EOS客");
                        }
                    }
                    if (sirnm != null)
                    {
                        if (hatyuno.ToString() != "0")
                            tantocd.ToString().PadLeft(3, '0');
                        tantocd = tantocd + "E";
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["EMtextBoxCell"].CellIndex, tantocd);
                    }
                    if (sirnm != null)
                    {
                        if (hatyuno.ToString() != "0")
                            hatyuno.ToString().PadLeft(6, '0');
                        hatyuno = hatyuno + "-" + jyutyugyono;
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["発注NOtextBoxCell"].CellIndex, hatyuno);
                    }
                    if (hatyuzan.ToString() == null && genzaiko < 0)
                    {
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["在庫状況textBoxCell"].CellIndex, "●");
                    }
                    else if (hatyuzan == jyutyuzan)
                    {
                        if (hatyuhakoFLG.ToString() == "1")
                        {
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["在庫状況textBoxCell"].CellIndex, "★");
                        }
                        else
                        {
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["在庫状況textBoxCell"].CellIndex, "☆");
                        }
                    }
                    else if (hatyuzan > 0 && jyutyuzan > hatyuzan)
                    {
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["在庫状況textBoxCell"].CellIndex, "▲");
                    }
                    else if (hatyuzan == 0)
                    {
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["在庫状況textBoxCell"].CellIndex, "○");
                    }
                    if (urikbncd.ToString() == "4")
                    {
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["直送textBoxCell"].CellIndex, "直");
                    }
                    if (tel3 != null)
                    {
                        tel1 = tel1.ToString() + tel2.ToString() + tel3.ToString();
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["仕入先電話textBoxCell"].CellIndex, tel1);
                    }

                    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txt可能数"].CellIndex, zaiko - jyutyuzansu);
                    //=IIf([EOS区分]=21 And [客注区分]=1,"EOS外",IIf([EOS区分]=21 And [客注区分]=2,"EOS客",""))
                    //=IIf(IsNull([仕名]),Null,IIf([発注番号]=0,Null,Format([担当者コード],"000") & "E"))
                    //=IIf(IsNull([受注番号]),Null,Format([受注番号],"000000") & "-" & [受注行番号])
                    //=IIf(IsNull([仕名]),Null,IIf([発注番号]=0,Null,Format([発注番号],"000000") & "-" & [受注行番号]))
                    //=IIf([発注残] Is Null And [出荷可能数]>0,Null,IIf([発注残] Is Null And [出荷可能数]<0,"●",IIf([受注残]=[発注残],IIf([発注書発行フラグ]=1,"★","☆"),IIf([発注残]>0 And [受注残]>[発注残],"▲",IIf([発注残]=0,"○",Null)))))
                    //=IIf([売上区分コード]='4',"直","")
                }


                //if (dataTable.Rows.Count.Equals(rowcnt-1)) break;

                //Console.WriteLine("Before UP" + rowcnt.ToString());
                rowcnt++;
                //Console.WriteLine("After UP" + rowcnt.ToString());
                gcMultiRow1.EndEdit();
                //gcMultiRow1.BeginEdit(true);
                //gcMultiRow1.RowCount += 1;
                connection.Close();

                return 0;
            }
        }


        // エラー処理
        void adapter_FillError(object sender, FillErrorEventArgs e)
        {
            if (e.Errors.GetType() == typeof(System.OverflowException))
            {
                // Code to handle precision loss.  
                //Add a row to table using the values from the first two columns.  
                DataRow myRow = e.DataTable.Rows.Add(new object[] { e.Values[0], e.Values[1], DBNull.Value });
                //Set the RowError containing the value for the third column.  
                //e.RowError = "OverflowException Encountered. Value from data source: " + e.Values[2];
                MessageBox.Show("OverflowException Encountered. Value from data source: " + e.Values[2]);
                e.Continue = true;
            }
        }

        void gcMultiRow1_DataError(object sender, DataErrorEventArgs e)
        {
            /*
            if (e.Exception != null)
            {
                MessageBox.Show(this,
                    string.Format(" {0} でエラーが発生しました。\n\n説明: {1}",
                    e.CellName, e.Exception.Message),
                    "エラーが発生しました",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = false;
                System.ArgumentException argEx = new System.ArgumentException("Index is out of range", "index", e.Exception);
                throw argEx;
            }
            */
            // The first id cell only can input number, if user input some invalid value, DataError event will be fired.
            // You should handle this event to handle some error cases.
            if ((e.Context & DataErrorContexts.ValueValidation) != 0)
            {
                // When committing value occurs error, show a massage box to notify user, and roll back value.
                EditingActions.CancelEdit.Execute(this.gcMultiRow1);
                MessageBox.Show(e.Exception.Message);
            }
            //else
            //{
                // Other handle.
            //}
        }

        private void gcMultiRow1_CellContentClick_1(object sender, CellEventArgs e)
        {

        }

        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "表示順radioGroupCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索得意先S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索得意先S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索得意先S");
        //            }
        //            break;
        //        case "txt検索得意先S":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索商品S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索商品S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索商品S");
        //            }
        //            break;
        //        case "txt検索商品S":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索得意先S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索得意先S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            break;
        //        case "txt受注日S":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索備考textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索商品S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索備考textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索商品S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索備考textBoxCell");
        //            }
        //            break;
        //        case "検索備考textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索事業所textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索事業所textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索事業所textBoxCell");
        //            }
        //            break;
        //        case "検索事業所textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索備考textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索備考textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            break;
        //        case "実行buttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索事業所textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索事業所textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            break;
        //        case "クリアbuttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            break;
        //        case "終了buttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索得意先S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索事業所textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索得意先S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索事業所textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt検索得意先S");
        //            }
        //            break;
        //    }
        //}

    }
}
