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

namespace db_test
{
    public partial class 仕入先別商品別発注残問合せForm : Form
    {
        private string receiveDataSyohin = "";
        private string receiveDataSirsaki = "";
        private string receiveDataToksaki = "";

        商品検索Form fsSyohin;
        仕入先検索Form fsSirsaki;

        int rowcnt = 0;
        object sirsakicd = "";
        object EOSKBN = "";
        object kyakuTyuKBN = "";
        object sirsakinm = "";
        object hatyuno = "";
        object tantocd = "";
        object jyutyuno = "";
        object hatyugyono = "";
        int hatyuzan = 0;
        int jyutyuzan = 0;
        int genzaiko = 0;
        int kake = 0;
        object hatyuhakoFLG = "";
        object urikbncd = "";
        object tel1 = "";
        object tel2 = "";
        object tel3 = "";
        object toksakicd = "";
        object toksakinm = "";
        object sirsakitantocd = "";
        object syohincd = "";
        object syohinnm = "";

        GcComboBoxCell comboBoxCell01 = null;

        public 仕入先別商品別発注残問合せForm()
        {
            InitializeComponent();
            fsSyohin = new 商品検索Form();
            fsSyohin.Owner = this;
            fsSirsaki = new 仕入先検索Form();
            fsSirsaki.Owner = this;
        }

        private void 仕入先別商品別発注残問合せForm_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 仕入先別商品別発注残問合せTemplate();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
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
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            //gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);

            // GcMultiRowコントロールがフォーカスを失ったとき
            // セルの選択状態を非表示にする
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
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F6);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先別商品別発注残問合せFunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先別商品別発注残問合せFunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先別商品別発注残問合せFunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先別商品別発注残問合せFunctionKeyAction(Keys.F6), Keys.F6);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先別商品別発注残問合せFunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先別商品別発注残問合せFunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            //gcMultiRow1.ColumnHeaders[0].Cells["事業所コードgcTextBoxCell"].Value = "";

            comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");

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
                        MessageBox.Show("データがありません。", "仕入先別商品別発注残問合せ");
                        this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
                    }
                    else
                    {
                        gcMultiRow1.ColumnFooters[0].Cells["回答日buttonCell"].Selectable = true;
                        gcMultiRow1.ColumnFooters[0].Cells["更新buttonCell"].Selectable = true;
                    }
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
                    if (mKaitou() == 0)
                    {
                    }
                    break;

                case "更新buttonCell": // 保留
                    break;

                case "終了buttonCell":
                    this.Close();
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "仕入先コードgcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードgcTextBoxCell"].Value != null)
                    {
                        DataTable table = new DataTable();
                        String sql = "SELECT SIR.仕入先名, SIR.電話１, SIR.電話２, SIR.電話３, ";
                        sql = sql + "SIR.ＦＡＸ電話１, SIR.ＦＡＸ電話２, SIR.ＦＡＸ電話３, ";
                        sql = sql + "SIR.担当者コード, TAN.担当者名, SIR.郵便番号, ";
                        sql = sql + "SIR.都道府県名, SIR.住所１, SIR.住所２ ";
                        sql = sql + "FROM T仕入先マスタ SIR ";
                        sql = sql + "LEFT JOIN T担当者マスタ TAN ";
                        sql = sql + "ON SIR.担当者コード = TAN.担当者コード ";
                        sql = sql + "WHERE SIR.仕入先コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードgcTextBoxCell"].Value + "'";

                        table = Utility.GetComboBoxData(sql);

                        if (table.Rows.Count > 0)
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["仕入先名gcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["仕入先名"]) ? null : table.Rows[0]["仕入先名"]);
                            if ((string)(DBNull.Value.Equals(table.Rows[0]["電話３"]) ? null : table.Rows[0]["電話３"]) != null)
                            {
                                gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬgcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["電話１"]) ? null : table.Rows[0]["電話１"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["電話２"]) ? null : table.Rows[0]["電話２"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["電話３"]) ? null : table.Rows[0]["電話３"]);
                            }
                            if ((string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話３"]) ? null : table.Rows[0]["ＦＡＸ電話３"]) != null)
                            {
                                gcMultiRow1.ColumnHeaders[0].Cells["ＦＡＸgcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話１"]) ? null : table.Rows[0]["ＦＡＸ電話１"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話２"]) ? null : table.Rows[0]["ＦＡＸ電話２"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話３"]) ? null : table.Rows[0]["ＦＡＸ電話３"]);
                            }
                            gcMultiRow1.ColumnHeaders[0].Cells["郵便番号gcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["郵便番号"]) ? null : table.Rows[0]["郵便番号"]);
                            gcMultiRow1.ColumnHeaders[0].Cells["住所１gcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["住所１"]) ? null : table.Rows[0]["住所１"]);
                            gcMultiRow1.ColumnHeaders[0].Cells["住所２gcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["住所２"]) ? null : table.Rows[0]["住所２"]);
                        }
                        else
                        {
                            MessageBox.Show("仕入先マスタにありません。", "仕入先別商品別発注残問合せ");
                            gcMultiRow1.ColumnHeaders[0].Cells["仕入先名gcTextBoxCell"].Value = null;
                            gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬgcTextBoxCell"].Value = null;
                            gcMultiRow1.ColumnHeaders[0].Cells["ＦＡＸgcTextBoxCell"].Value = null;
                            gcMultiRow1.ColumnHeaders[0].Cells["郵便番号gcTextBoxCell"].Value = null;
                            gcMultiRow1.ColumnHeaders[0].Cells["住所１gcTextBoxCell"].Value = null;
                            gcMultiRow1.ColumnHeaders[0].Cells["住所２gcTextBoxCell"].Value = null;
                            gcMultiRow1.EndEdit();
                        }
                    } 
                    break;
                case "商品コードgcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["商品コードgcTextBoxCell"].Value != null)
                    {
                        DataTable table = new DataTable();
                        String sql = "SELECT 商品名 FROM T商品マスタ ";
                        sql = sql + "WHERE 商品コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["商品コードgcTextBoxCell"].Value + "'";

                        table = Utility.GetComboBoxData(sql);

                        if (table.Rows.Count > 0)
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["商品名gcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["商品名"]) ? null : table.Rows[0]["商品名"]);
                        }
                        else
                        {
                            MessageBox.Show("商品マスタにありません。", "仕入先別商品別発注残問合せ");
                            gcMultiRow1.ColumnHeaders[0].Cells["商品名gcTextBoxCell"].Value = null;
                        }
                    }
                    break;
                case "発注日gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注日gcTextBoxCell"].Value != null)
                    {
                        string buf = "";
                        buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注日gcTextBoxCell"].Value;
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
                            this.gcMultiRow1.ColumnHeaders[0].Cells["発注日gcTextBoxCell"].Value = null;
                            return;
                        }
                        buf = buf.Substring(0, 4) + "/" + buf.Substring(4, 2) + "/" + buf.Substring(6, 2);
                        DateTime dt;
                        // DateTimeに変換できるかチェック
                        if (DateTime.TryParse(buf, out dt))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["発注日gcTextBoxCell"].Value = buf;
                        }
                        else
                        {
                            MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            this.gcMultiRow1.ColumnHeaders[0].Cells["発注日gcTextBoxCell"].Value = null;
                        } 
                    }
                    break;
                case "担当者コードgcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value;
                        }
                    }
                    break;

                case "txt得意先S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value != null)
                    {
                        DataTable table = new DataTable();
                        String sql = "SELECT TOK.得意先名, TOK.電話１, TOK.電話２, TOK.電話３, ";
                        sql = sql + "TOK.ＦＡＸ電話１, TOK.ＦＡＸ電話２, TOK.ＦＡＸ電話３, ";
                        sql = sql + "TOK.担当者コード, TAN.担当者名, TOK.郵便番号, ";
                        sql = sql + "TOK.都道府県名, TOK.住所１, TOK.住所２, TOK.現在売掛金残高, TOK.検収区分 ";
                        sql = sql + "FROM T得意先マスタ TOK ";
                        sql = sql + "LEFT JOIN T担当者マスタ TAN ";
                        sql = sql + "ON TOK.担当者コード = TAN.担当者コード ";
                        sql = sql + "WHERE TOK.得意先コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value + "'";

                        table = Utility.GetComboBoxData(sql);

                        if (table.Rows.Count > 0)
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["txt得意先名S"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["得意先名"]) ? null : table.Rows[0]["得意先名"]);
                            gcMultiRow1.ColumnHeaders[0].Cells["txt得意先名E"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["得意先名"]) ? null : table.Rows[0]["得意先名"]);

                            if ((string)(DBNull.Value.Equals(table.Rows[0]["電話３"]) ? null : table.Rows[0]["電話３"]) != null)
                            {
                                gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬgcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["電話１"]) ? null : table.Rows[0]["電話１"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["電話２"]) ? null : table.Rows[0]["電話２"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["電話３"]) ? null : table.Rows[0]["電話３"]);
                            }
                            if ((string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話３"]) ? null : table.Rows[0]["ＦＡＸ電話３"]) != null)
                            {
                                gcMultiRow1.ColumnHeaders[0].Cells["ＦＡＸgcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話１"]) ? null : table.Rows[0]["ＦＡＸ電話１"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話２"]) ? null : table.Rows[0]["ＦＡＸ電話２"]) + "-" + (string)(DBNull.Value.Equals(table.Rows[0]["ＦＡＸ電話３"]) ? null : table.Rows[0]["ＦＡＸ電話３"]);
                            }

                            gcMultiRow1.ColumnHeaders[0].Cells["郵便番号gcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["郵便番号"]) ? null : table.Rows[0]["郵便番号"]);
                            gcMultiRow1.ColumnHeaders[0].Cells["住所１gcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["住所１"]) ? null : table.Rows[0]["住所１"]);
                            gcMultiRow1.ColumnHeaders[0].Cells["住所２gcTextBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["住所２"]) ? null : table.Rows[0]["住所２"]);
                        }

                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先名S"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先名E"].Value = null;

                        this.gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬgcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["ＦＡＸgcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["郵便番号gcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["住所１gcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["住所２gcTextBoxCell"].Value = null;
                    }
                    break;
                case "txt得意先E":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value != null)
                    {
                        DataTable table = new DataTable();
                        String sql = "SELECT TOK.得意先名, TOK.電話１, TOK.電話２, TOK.電話３, ";
                        sql = sql + "TOK.ＦＡＸ電話１, TOK.ＦＡＸ電話２, TOK.ＦＡＸ電話３, ";
                        sql = sql + "TOK.担当者コード, TAN.担当者名, TOK.郵便番号, ";
                        sql = sql + "TOK.都道府県名, TOK.住所１, TOK.住所２, TOK.現在売掛金残高, TOK.検収区分 ";
                        sql = sql + "FROM T得意先マスタ TOK ";
                        sql = sql + "LEFT JOIN T担当者マスタ TAN ";
                        sql = sql + "ON TOK.担当者コード = TAN.担当者コード ";
                        sql = sql + "WHERE TOK.得意先コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value + "'";

                        table = Utility.GetComboBoxData(sql);

                        if (table.Rows.Count > 0)
                            gcMultiRow1.ColumnHeaders[0].Cells["txt得意先名E"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["得意先名"]) ? null : table.Rows[0]["得意先名"]);
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先名E"].Value = null;
                    }
                    break;

            }
        }


        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F3: // 検索
                    target = this.ButtonF3;
                    String cname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (cname == "仕入先コードgcTextBoxCell")
                    {
                        仕入先検索Form fsSirsaki = new 仕入先検索Form();
                        //fsSirsaki = new 仕入先検索Form();
                        fsSirsaki.Owner = this;
                        fsSirsaki.Show();

                    }
                    else if (cname == "商品コードgcTextBoxCell")
                    {
                        商品検索Form fsSyohin = new 商品検索Form();
                        //fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                    }
                    else if (cname == "txt得意先S" || cname == "txt得意先E")
                    {
                        得意先検索Form fsToksaki = new 得意先検索Form();
                        //fsToksaki = new 得意先検索Form();
                        fsToksaki.Owner = this;
                        fsToksaki.mTextBox = cname;
                        fsToksaki.Show();
                    }

                    break;
                case Keys.F4: // 回答日
                    //target = this.ButtonF3;
                    if (mKaitou() == 0)
                    {
                    }                                   
                    break;
                case Keys.F5: // 検索
                    target = this.ButtonF3;
                    if (createData() != 0)
                    {
                        gcMultiRow1.ColumnFooters[0].Cells["回答日buttonCell"].Selectable = false;
                        gcMultiRow1.ColumnFooters[0].Cells["更新buttonCell"].Selectable = false;
                        MessageBox.Show("データがありません。", "仕入先別商品別発注残問合せ");
                        this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
                    }
                    else
                    {
                        gcMultiRow1.ColumnFooters[0].Cells["回答日buttonCell"].Selectable = true;
                        gcMultiRow1.ColumnFooters[0].Cells["更新buttonCell"].Selectable = true;
                    }

                    if (mKaitou() == 0)
                    {
                    }
                    break;

                case Keys.F6: // クリア
                    target = this.ButtonF3;
                    if (mClear() == 0)
                    {
                    }
                    break;
                case Keys.F9:
                    target = this.ButtonF9;
                    gcMultiRow1.Rows.Clear();
                    this.Refresh();
                    this.gcMultiRow1.Select();
                    this.gcMultiRow1.EditMode = EditMode.EditOnEnter;
                    this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
                    break;
                case Keys.F10:
                    target = this.ButtonF4;
                    this.Close();
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



        private int createData()
        {

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;
            // ストアド プロシージャ名を指定
            command.CommandText = "仕入先別商品別発注残問合せ";
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードgcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード", (String)gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードgcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["商品コードgcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@商品コード", (String)gcMultiRow1.ColumnHeaders[0].Cells["商品コードgcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品コード", DBNull.Value);
            }
            if (gcMultiRow1.ColumnHeaders[0].Cells["発注日gcTextBoxCell"].Value != null)
            {
                DateTime jyuDate = DateTime.Parse(gcMultiRow1.ColumnHeaders[0].Cells["発注日gcTextBoxCell"].Value.ToString());
                command.Parameters.AddWithValue("@発注日", jyuDate.ToString("yyyy/MM/dd"));
            }
            else
            {
                command.Parameters.AddWithValue("@発注日", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["備考gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@明細摘要", (String)gcMultiRow1.ColumnHeaders[0].Cells["備考gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@明細摘要", DBNull.Value);
            }
            //if ((String)gcMultiRow1.ColumnHeaders[0].Cells["事業所コードgcTextBoxCell"].Value != null && (String)gcMultiRow1.ColumnHeaders[0].Cells["事業所コードgcTextBoxCell"].Value != "*")
            //{
            //    command.Parameters.AddWithValue("@事業所コード", (String)gcMultiRow1.ColumnHeaders[0].Cells["事業所コードgcTextBoxCell"].Value);
            //}
            //else
            //{
            //    command.Parameters.AddWithValue("@事業所コード", DBNull.Value);
            //}
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value != null && (String)gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value != "*")
            {
                command.Parameters.AddWithValue("@担当者コード", (String)gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@担当者コード", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value != null && (String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value != "*")
            {
                command.Parameters.AddWithValue("@得意先S", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先S", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value != null && (String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value != "*")
            {
                command.Parameters.AddWithValue("@得意先E", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先E", DBNull.Value);
            }

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
                    throw;
                }
                if (e.Number == 241)
                {
                    return 1;
                    throw;
                }
            }

            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            //DataView dv = new DataView(dataTable);
            // 昇順
            //dv.Sort = "受注番号";
            // 降順
            //dv.Sort = "発注日 DESC";

            // 並び替え後のデータをDataTableに戻す
            //dataTable = dv.ToTable();
            
            //dataTable.AcceptChanges();

            //this.gcMultiRow1.DataSource = dataSet;

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
                    sirsakicd = "";
                    EOSKBN = "";
                    kyakuTyuKBN = "";
                    sirsakinm = "";
                    hatyuno = "";
                    tantocd = "";
                    jyutyuno = "";
                    hatyugyono = "";
                    hatyuzan = 0;
                    jyutyuzan = 0;
                    genzaiko = 0;
                    hatyuhakoFLG = "";
                    urikbncd = "";
                    tel1 = "";
                    tel2 = "";
                    tel3 = "";
                    kake = 0;
                    toksakicd = "";
                    toksakinm = "";
                    sirsakitantocd = "";
                    syohincd = "";
                    syohinnm = "";

                    foreach (DataColumn dc in dataTable.Columns)
                    {
                        Console.Write(dr[dc] + "\t");
                        switch (dc.ColumnName)
                        {
                            case "仕入先コード":
                                sirsakicd = dr[dc];
                                break;
                            case "仕入先名":    //略名
                                sirsakinm = dr[dc];
                                break;
                            case "メーカーコード":
                                //Console.WriteLine("メーカーコード。");　//コンソール画面に「赤色です」と出力する。
                                break;
                            case "メーカー名":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["メーカー名textBoxCell"].CellIndex, dr[dc]);
                                break;
                            case "発注番号":
                                hatyuno = dr[dc];
                                break;
                            case "発注行番号":
                                hatyugyono = dr[dc];
                                break;
                            case "商品コード":
                                syohincd = dr[dc];
                                break;
                            case "商名":
                                syohinnm = dr[dc];
                                break;
                            case "発注日":
                                DateTime jyuDate = DateTime.Parse(dr[dc].ToString());
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["発注日textBoxCell"].CellIndex, jyuDate.ToString("yyyy/MM/dd"));
                                break;
                            case "明細摘要":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["明細摘要textBoxCell"].CellIndex, dr[dc]);
                                break;
                            case "得意先コード":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["得意先コードtextBoxCell"].CellIndex, dr[dc]);
                                toksakicd = dr[dc];
                                break;
                            case "得意先名":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["得意先略名textBoxCell"].CellIndex, dr[dc]);
                                break;
                            case "回答コード":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["回答コードtextBoxCell"].CellIndex, dr[dc]);
                                break;
                            case "回答名":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["回答名textBoxCell"].CellIndex, dr[dc]);
                                break;
                            case "回答納期":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["回答納期textBoxCell"].CellIndex, dr[dc]);
                                break;
                            case "発注残":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["発注残numericUpDownCell"].CellIndex, dr[dc]);
                                break;
                            case "原価掛率":
                                if (dr[dc] != DBNull.Value)
                                {
                                    kake = (int)(Convert.ToDouble(dr[dc]) * 100);
                                }
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["原価掛率numericUpDownCell"].CellIndex, kake);
                                break;
                            case "発注単価":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["発注単価numericUpDownCell"].CellIndex, dr[dc]);
                                break;
                            case "発注書発行フラグ":
                                hatyuhakoFLG = dr[dc];
                                break;
                            case "仕名":
                                sirsakinm = dr[dc];
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["仕名textBoxCell"].CellIndex, sirsakinm);
                                break;
                            case "メーカー品番":
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["メーカー品番textBoxCell"].CellIndex, dr[dc]);
                                break;
                            case "電話１":
                                tel1 = dr[dc];
                                break;
                            case "電話２":
                                tel2 = dr[dc];
                                break;
                            case "電話３":
                                tel3 = dr[dc];
                                break;
                            case "受注番号":
                                jyutyuno = dr[dc];
                                break;
                            case "担当者コード":
                                tantocd = dr[dc];
                                break;
                            case "仕入担当者コード":
                                sirsakitantocd = dr[dc];
                                break;

                        }
                    }

                    // 記号textBoxCell
                    if (syohincd != null)
                    {
                        if (toksakicd == null)
                        {
                            if (sirsakitantocd.ToString() != "0")
                            {
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["記号textBoxCell"].CellIndex, "   M");
                            }
                            else
                            {
                                sirsakitantocd.ToString().PadLeft(3, '0');
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["記号textBoxCell"].CellIndex, sirsakitantocd + "M");
                            }
                        }
                        else
                        {
                            tantocd.ToString().PadLeft(3, '0');
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["記号textBoxCell"].CellIndex, tantocd + "E");
                        }
                    }

                    // 発注NOtextBoxCell
                    if (hatyuno != null)
                    {
                        hatyuno.ToString().PadLeft(6, '0');
                        hatyuno = hatyuno + "-" + hatyugyono;
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["発注NOtextBoxCell"].CellIndex, hatyuno);
                    }

                    if (hatyuhakoFLG != null && hatyuhakoFLG.ToString() != "0")
                    {
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["発注書発行フラグtextBoxCell"].CellIndex, "★");
                    }

                    if (jyutyuno != null)
                    {
                        string jyuno = jyutyuno.ToString().PadLeft(6, '0') + "-" + hatyugyono;
                        //jyutyuno = jyutyuno + "-" + hatyugyono;
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注NOtextBoxCell"].CellIndex, jyuno);
                    }

                    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["商名textBoxCell"].CellIndex, syohinnm + " " + sirsakinm);

                    if (tel3 != null)
                    {
                        tel1 = tel1.ToString() + "-" + tel2.ToString() + "-" + tel3.ToString();
                        this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["仕入先電話textBoxCell"].CellIndex, tel1);
                    }

                    rowcnt++;
                }

//                rowcnt++;
                gcMultiRow1.EndEdit();
                //gcMultiRow1.BeginEdit(true);
                //gcMultiRow1.RowCount += 1;
                connection.Close();

                return 0;
            }
        }

        private int mClear()
        {
            gcMultiRow1.Rows.Clear();
            this.Refresh();

            //gcMultiRow1.ColumnHeaders[0].Cells["txt担当者S"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt担当者E"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先S"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt検索得意先E"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["検索得意先名textBoxCell"].Value = null;

            //gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品S"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt検索商品E"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["検索商品名textBoxCell"].Value = null;

            //gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value = null;

            //gcMultiRow1.ColumnHeaders[0].Cells["txt回答名"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt明細摘要"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt伝票摘要"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt発注摘要"].Value = null;

            //gcMultiRow1.ColumnHeaders[0].Cells["ＴＥＬtextBoxCell"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["ＦＡＸtextBoxCell"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["担当textBoxCell"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["郵便番号textBoxCell"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["住所１textBoxCell"].Value = null;
            //gcMultiRow1.ColumnHeaders[0].Cells["txt注意事項"].Value = null;

            gcMultiRow1.EndEdit();

            this.gcMultiRow1.Select();
            this.gcMultiRow1.EditMode = EditMode.EditOnEnter;
            this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");

            return 1;
        }

        private int mKaitou()
        {
            for (int i = 0; i < this.gcMultiRow1.RowCount - 1; i++)
            {
                gcMultiRow1.Rows[i].Cells["回答コードtextBoxCell"].ReadOnly = false;
                gcMultiRow1.Rows[i].Cells["回答納期textBoxCell"].ReadOnly = false;
                gcMultiRow1.Rows[i].Cells["回答名textBoxCell"].ReadOnly = false;
            }

            return 1;
        }


        public string ReceiveDataSyohin
        {
            set
            {
                receiveDataSyohin = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["商品コードgcTextBoxCell"].Value = receiveDataSyohin.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataSyohin;
            }
        }

        public string ReceiveDataSirsaki
        {
            set
            {
                receiveDataSirsaki = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードgcTextBoxCell"].Value = receiveDataSirsaki.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataSirsaki;
            }
        }

        public string ReceiveDataToksaki
        {
            set
            {
                receiveDataToksaki = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value = receiveDataToksaki.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataToksaki;
            }
        }

        public string ReceiveDataToksaki_E
        {
            set
            {
                receiveDataToksaki = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value = receiveDataToksaki.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataToksaki;
            }
        }


        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "仕入先コードgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コードgcTextBoxCell");
        //            }
        //            break;
        //        case "商品コードgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日gcTextBoxCell");
        //            }
        //            break;
        //        case "発注日gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "備考gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "備考gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "備考gcTextBoxCell");
        //            }
        //            break;
        //        case "備考gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードgcTextBoxCell");
        //            }
        //            break;
        //        case "事業所コードgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "備考gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "備考gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            break;
        //        case "事業所名gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードgcTextBoxCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードgcTextBoxCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            break;
        //    }
        //}

    }
}
