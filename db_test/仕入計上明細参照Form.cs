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
    public partial class 仕入計上明細参照Form : Form
    {
        int rowcnt = 0;
        int cancelFlag = 0;
        int idoFlag = 0;
        int henkanFlag = 0;
        string beforeCellName = null;
        string nowCellName = null;
        string afterCellName = null;

        public 仕入計上明細参照Form()
        {
            InitializeComponent();
        }

        private void 仕入計上明細参照Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 仕入計上明細参照Template();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            gcMultiRow1.ScrollBars = ScrollBars.Vertical;

            仕入計上明細参照Form.CheckForIllegalCrossThreadCalls = false; // スレッド間エラー 一時的対応
            //this.WindowState = FormWindowState.Maximized;

            //セル色の設定
            //非選択状態の色
            //gcMultiRow1.Rows[0].Cells[0].Style.BackColor = Color.White;
            //gcMultiRow1.Rows[0].Cells[0].Style.ForeColor = Color.Gray;

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
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.CellEditedFormattedValueChanged += new EventHandler<CellEditedFormattedValueChangedEventArgs>(gcMultiRow1_CellEditedFormattedValueChanged);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);

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
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入計上明細参照FunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入計上明細参照FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入計上明細参照FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入計上明細参照FunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["参照区分radioGroupCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["日付始textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");
            gcMultiRow1.ColumnHeaders[0].Cells["日付終textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");
            //gcMultiRow1.ColumnHeaders[0].Cells["クライアントgcTextBoxCell"].Value = "103";
            gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcTextBoxCell"].Value = "*";

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");

        }


        void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            if (e.CellName == "実行buttonCell")
            {
                int ret = execProcedure();
                if (ret == 0)
                {
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
                }
                else if (ret == 1)
                {
                    MessageBox.Show("データがありません。", "仕入計上明細参照");
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
                    cancelFlag = 0;
                    //afterCellName = "日付始textBoxCell";
                }
                else
                {
                    MessageBox.Show("データ取得エラー。", "仕入計上明細参照");
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
                }
            }
        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "終了buttonCell")
            {
                this.Close();
                return;
            }
            else if (e.CellName == "リストbuttonCell")
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
                }
            }
        }

        //void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        //{
        //    TextBoxEditingControl textBox = e.Control as TextBoxEditingControl;
        //    if (textBox != null)
        //    {
        //        textBox.KeyDown -= new KeyEventHandler(textBox_KeyDown);
        //        textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
        //    }
        //}

        //void textBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (gcMultiRow1.CurrentCellPosition.CellName)
        //    {
        //        case "クライアントgcTextBoxCell":
        //            switch (e.KeyCode)
        //            {
        //                case Keys.Down:
        //                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
        //                    break;
        //                case Keys.Up:
        //                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "発注日始textBoxCell");
        //                    break;
        //                case Keys.Enter:
        //                    execProcedure();
        //                    break;
        //            }
        //            break;
        //        case "オペレーターgcTextBoxCell":
        //            switch (e.KeyCode)
        //            {
        //                case Keys.Down:
        //                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");
        //                    break;
        //                case Keys.Up:
        //                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //                    break;
        //                case Keys.Enter:
        //                    execProcedure();
        //                    break;
        //            }
        //            break;
        //        case "リストbuttonCell":
        //            switch (e.KeyCode)
        //            {
        //                case Keys.Down:
        //                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "発注日始textBoxCell");
        //                    break;
        //                case Keys.Up:
        //                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
        //                    break;
        //                case Keys.Enter:
        //                    //this.Hide();
        //                    break;
        //            }
        //            break;
        //        case "終了buttonCell":
        //            switch (e.KeyCode)
        //            {
        //                case Keys.Down:
        //                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "発注日始textBoxCell");
        //                    break;
        //                case Keys.Up:
        //                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
        //                    break;
        //                case Keys.Enter:
        //                    this.Hide();
        //                    break;
        //            }
        //            break;
        //    }
        //}

        void gcMultiRow1_DataError(object sender, DataErrorEventArgs e)
        {
            string cellName = e.CellName;
        }

        void gcMultiRow1_CellLeave(object sender, CellEventArgs e)
        {
            string cellName = e.CellName;
        }

        void gcMultiRow1_CellBeginEdit(object sender, CellBeginEditEventArgs e)
        {
            string cellName = e.CellName;

        }


        void gcMultiRow1_SelectionChanged(object sender, EventArgs e)
        {
            this.Text = "Selected cell count: " + this.gcMultiRow1.SelectedCells.Count;
            this.Text += "  Selected row count: " + this.gcMultiRow1.SelectedRows.Count;
        }

        void gcMultiRow1_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            switch (e.CellName)
            {
                case "参照区分radioGroupCell":
                    int ret = execProcedure();
                    if (ret == 0)
                    {
                        gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
                    }
                    else if (ret == 1)
                    {
                        MessageBox.Show("データがありません。", "仕入計上明細参照");
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                    }
                    else
                    {
                        MessageBox.Show("データ取得エラー。", "仕入計上明細参照");
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                    }
                    break;
            }
        }

        //void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        //{
        //    switch (gcMultiRow1.CurrentCellPosition.CellName)
        //    {
        //        case "実行buttonCell":
        //            switch (e.KeyValue)
        //            {
        //                case 13:
        //                    int ret = execProcedure();
        //                    if (ret == 0)
        //                    {
        //                        gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
        //                    }
        //                    else if (ret == 1)
        //                    {
        //                        MessageBox.Show("データがありません。", "仕入計上明細参照");
        //                        gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //                        cancelFlag = 0;
        //                        //afterCellName = "日付始textBoxCell";
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("データ取得エラー。", "仕入計上明細参照");
        //                        gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //                    }
        //                    break;
        //            }
        //            break;
        //        case "リストbuttonCell":
        //            switch (e.KeyValue)
        //            {
        //                case 13:
        //                    // リスト
        //                    /*
        //                    if (createReportData() == 0)
        //                    {
        //                        プレビューForm プレビューform = new プレビューForm();

        //                        プレビューform.dataSet = dataSet;
        //                        プレビューform.rptName = "受注明細参照リストCrystalReport";
        //                        プレビューform.Show();
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("データがありません", "仕入先別発注残明細表");
        //                    }
        //                    */
        //                    break;
        //            }
        //            break;
        //        case "クリアbuttonCell":
        //            switch (e.KeyValue)
        //            {
        //                case 13:
        //                    //クリア
        //                    if (gcMultiRow1.Rows != null)
        //                    {
        //                        gcMultiRow1.DataSource = null;
        //                    }
        //                    break;
        //            }
        //            break;
        //        case "終了buttonCell":
        //            switch (e.KeyValue)
        //            {
        //                case 13:
        //                    this.Close();
        //                    break;
        //            }
        //            break;
        //    }
        //}

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            string buf = "";

            switch (e.CellName)
            {
                /*
                case "クライアントgcTextBoxCell":
                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                    //gcMultiRow1.EndEdit();
                    break;
                case "伝票番号始gcTextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        buf = chkDate(e.FormattedValue.ToString());
                        if (0 <= buf.IndexOf("/"))
                        {
                            henkanFlag = 1;
                            e.Cancel = false;
                        }
                    }
                    break;

                case "伝票番号終gcTextBoxCell":
                    buf = "";
                    break;
                */
                case "日付始textBoxCell":
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
                case "日付終textBoxCell":
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
                /*
                case "伝票番号始gcTextBoxCell":
                    if (henkanFlag == 1)
                    {
                        henkanFlag = 0;
                        if (gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始gcTextBoxCell"].Value != null)
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["日付始textBoxCell"].Value = gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始gcTextBoxCell"].Value;
                        }
                        gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始gcTextBoxCell"].Value = null;
                        return;
                    }
                    break;
                case "伝票番号始終gcTextBoxCell":
                    if (cancelFlag == 1)
                    {
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                        cancelFlag = 0;
                    }
                    break;
                */
                case "日付始textBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["日付始textBoxCell"].Value != null)
                    {
                        buf = "";
                        buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["日付始textBoxCell"].Value;
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
                            this.gcMultiRow1.ColumnHeaders[0].Cells["日付始textBoxCell"].Value = buf;
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
                case "日付終textBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["日付終textBoxCell"].Value != null)
                    {
                        buf = "";
                        buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["日付終textBoxCell"].Value;
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
                            this.gcMultiRow1.ColumnHeaders[0].Cells["日付終textBoxCell"].Value = buf;
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
            }
        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            int ret;

            switch (e.CellName)
            {
                case "クライアントgcTextBoxCell":
                    /*
                    gcMultiRow1.CurrentCell = null;
                    ret = execProcedure();
                    if (ret == 0)
                    {
                        //SendKeys.Send("{ENTER}");
                        //SendKeys.Send("{UP}");
                        Console.WriteLine("ffff");
                        afterCellName = "伝票番号終gcTextBoxCell";
                    }
                    else if (ret == 1)
                    {
                        //MessageBox.Show("データがありません。", "売上計上明細参照");
                        //Cell currentCell = null;
                        //gcMultiRow1.CancelEdit();
                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                        //gcMultiRow1.CancelEdit();
                        cancelFlag = 0;
                        //Cell currentCell = gcMultiRow1.ColumnHeaders[0].Cells[gcMultiRow1.Template.ColumnHeaders[0].Cells["日付始textBoxCell"].CellIndex];
                        //gcMultiRow1.CancelEdit();
                        afterCellName = "日付始textBoxCell";
                    }
                    else
                    {
                        MessageBox.Show("データ取得エラー。", "仕入計上明細参照");
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                        Cell currentCell = gcMultiRow1.Rows[e.RowIndex].Cells[e.CellIndex];
                    }
                    */
                    break;
                case "オペレーターgcTextBoxCell":
                    /*
                    ret = execProcedure();
                    if (ret == 0)
                    {
                        SendKeys.Send("{ENTER}");
                        SendKeys.Send("{UP}");
                    }
                    else if (ret == 1)
                    {
                        MessageBox.Show("データがありません。", "仕入計上明細参照");
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                        gcMultiRow1.EndEdit();
                    }
                    else
                    {
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                        MessageBox.Show("データ取得エラー。", "売上計上明細参照");
                    }
                     */
                    break;
            }

        }

        private int execProcedure()
        {

            //gcMultiRow1.Rows.Clear();

            rowcnt = 0;

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;
            // ストアド プロシージャ名を指定
            command.CommandText = "仕入計上明細参照";

            command.Parameters.AddWithValue("@参照区分", gcMultiRow1.ColumnHeaders[0].Cells["参照区分radioGroupCell"].Value);
            command.Parameters.AddWithValue("@日付始", gcMultiRow1.ColumnHeaders[0].Cells["日付始textBoxCell"].Value);
            command.Parameters.AddWithValue("@日付終", gcMultiRow1.ColumnHeaders[0].Cells["日付終textBoxCell"].Value);
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@伝票番号始", (String)gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@伝票番号始", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@伝票番号終", (String)gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@伝票番号終", DBNull.Value);
            }
            //if ((String)gcMultiRow1.ColumnHeaders[0].Cells["クライアントgcTextBoxCell"].Value != null)
            //{
            //    command.Parameters.AddWithValue("@WSID", (String)gcMultiRow1.ColumnHeaders[0].Cells["クライアントgcTextBoxCell"].Value);
            //}
            //else
            //{
            //    command.Parameters.AddWithValue("@WSID", DBNull.Value);
            //}
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcTextBoxCell"].Value != "*" && (String)gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@オペレーターコード", (String)gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@オペレーターコード", DBNull.Value);
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
            finally
            {
                connection.Close();
            }

            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            dataTable.AcceptChanges();

            this.gcMultiRow1.DataSource = dataTable;


            if (gcMultiRow1.Rows.Count == 0)
            {
                return 1;
            }

            return 0;

        }

        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F4:
                    target = this.ButtonF4;
                    // リスト
                    /*
                    if (createReportData() == 0)
                    {
                        プレビューForm プレビューform = new プレビューForm();

                        プレビューform.dataSet = dataSet;
                        プレビューform.rptName = "仕入計上明細参照リストCrystalReport";
                        プレビューform.Show();
                    }
                    else
                    {
                        MessageBox.Show("データがありません", "仕入計上明細参照");
                    }
                    */
                    break;
                case Keys.F5:
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    target = this.ButtonF5;

                    int ret = execProcedure();
                    if (ret == 0)
                    {
                        gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
                    }
                    else if (ret == 1)
                    {
                        MessageBox.Show("データがありません。", "仕入計上明細参照");
                        gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
                    }
                    else
                    {
                        MessageBox.Show("データ取得エラー。", "仕入計上明細参照");
                        gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
                    }
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
                    this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                    break;
                case Keys.F10:
                    target = this.ButtonF10;
                    this.Hide();
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
        private string chkDate(string buf)
        {
            string ret = null;
            string dt = null;
            string buf2 = null;
            buf2 = buf.Replace("/", "");
            if (buf2.Length == 4)
            {
                dt = DateTime.Today.ToString("yyyy") + buf2;
            }
            else if (buf2.Length == 6)
            {
                dt = DateTime.Today.ToString("yy") + buf2;
            }
            else if (buf2.Length == 8)
            {
                dt = buf2;
            }
            dt = dt.Substring(0, 4) + "/" + dt.Substring(4, 2) + "/" + dt.Substring(6, 2);
            DateTime DT;
            // DateTimeに変換できるかチェック
            if (DateTime.TryParse(dt, out DT))
            {
                ret = dt;
            }
            else
            {
                ret = buf;
            }

            return ret;

        }

        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "参照区分radioGroupCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //            }
        //            break;
        //        case "日付始textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付終textBoxCell");
        //            }
        //            break;
        //        case "日付終textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
        //            }
        //            break;
        //        case "伝票番号始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終gcTextBoxCell");
        //            }
        //            break;
        //        case "伝票番号終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クライアントgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クライアントgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クライアントgcTextBoxCell");
        //            }
        //            break;
        //        case "クライアントgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターgcTextBoxCell");
        //            }
        //            break;
        //        case "オペレーターgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クライアントgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クライアントgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            break;
        //        case "実行buttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
        //            }
        //            break;
        //        case "リストbuttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "実行buttonCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            break;
        //        case "終了buttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
        //            }
        //            break;
        //    }
        //}

    }
}
