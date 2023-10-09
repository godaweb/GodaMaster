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
using System.Threading;

namespace db_test
{
    public partial class 受注明細参照Form : Form
    {
        private string receiveDataSyohin = "";
        private string receiveDataToksaki = "";

        int rowcnt = 0;
        int cancelFlag = 0;
        int idoFlag = 0;
        int henkanFlag = 0;
        string beforeCellName = null;
        string nowCellName = null;
        string afterCellName = null;

        int 参照区分index = 0;
        int 日付始index = 0;
        int 日付終index = 0;
        int クライアントindex = 0;
        int 受注番号始index = 0;
        int 受注番号終index = 0;
        int オペレーターindex = 0;
        int 終了index = 0;
        int リストindex = 0;

        public 受注明細参照Form()
        {

            InitializeComponent();

        }
        private void 受注明細参照Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 受注明細参照Template();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            //this.gcMultiRow1.ScrollBars = ScrollBars.None;
            gcMultiRow1.ScrollBars = ScrollBars.Vertical;

            受注明細参照Form.CheckForIllegalCrossThreadCalls = false; // スレッド間エラー 一時的対応
            //this.WindowState = FormWindowState.Maximized;

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
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //gcMultiRow1.CellEditedFormattedValueChanged += new EventHandler<CellEditedFormattedValueChangedEventArgs>(gcMultiRow1_CellEditedFormattedValueChanged);
            //gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            //gcMultiRow1.CellBeginEdit += new EventHandler<CellBeginEditEventArgs>(gcMultiRow1_CellBeginEdit);
            gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            //gcMultiRow1.CellLeave += new EventHandler<CellEventArgs>(gcMultiRow1_CellLeave);
            //gcMultiRow1.DataError += new EventHandler<DataErrorEventArgs>(gcMultiRow1_DataError);
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            //gcMultiRow1.SelectionChanged += new EventHandler(gcMultiRow1_SelectionChanged);
            //gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);


            
            // GcMultiRowコントロールがフォーカスを失ったとき
            // セルの選択状態を非表示にする
            gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;
            //gcMultiRow1.EditMode = EditMode.EditOnShortcutKey;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellThenControl, Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellThenControl, Keys.Left);


            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注明細参照FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注明細参照FunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注明細参照FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注明細参照FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注明細参照FunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Default;

            // インデックスの保存
            //参照区分index = gcMultiRow1.ColumnHeaders[0].Cells["参照区分radioGroupCell"].CellIndex;
            //参照区分index = Utility.getHeadIndex(gcMultiRow1, "参照区分radioGroupCell");
            //日付始index = gcMultiRow1.ColumnHeaders[0].Cells["日付始textBoxCell"].CellIndex;
            //日付終index = gcMultiRow1.ColumnHeaders[0].Cells["日付終textBoxCell"].CellIndex;
            //クライアントindex = gcMultiRow1.ColumnHeaders[0].Cells["クライアントgcTextBoxCell"].CellIndex;
            //受注番号始index = gcMultiRow1.ColumnHeaders[0].Cells["受注番号始gcTextBoxCell"].CellIndex;
            //受注番号終index = gcMultiRow1.ColumnHeaders[0].Cells["受注番号終gcTextBoxCell"].CellIndex;
            //オペレーターindex = gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcTextBoxCell"].CellIndex;
            //終了index = gcMultiRow1.ColumnHeaders[0].Cells["終了buttonCell"].CellIndex;
            //リストindex = gcMultiRow1.ColumnHeaders[0].Cells["終了buttonCell"].CellIndex;

            //リストindex = gcMultiRow1.Template.Row.Cells["終了buttonCell"].CellIndex;

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["参照区分radioGroupCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["日付始textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");
            gcMultiRow1.ColumnHeaders[0].Cells["日付終textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");
            //gcMultiRow1.ColumnHeaders[0].Cells["クライアントgcTextBoxCell"].Value = "103";
            //gcMultiRow1.ColumnHeaders[0].Cells["クライアントgcTextBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcTextBoxCell"].Value = "*";

            // 初期フォーカス
            //gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt得意先S");

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "実行buttonCell")
            {
                int ret = execProcedure();
                if (ret == 0)
                {
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注番号始gcTextBoxCell");
                }
                else if (ret == 1)
                {
                    MessageBox.Show("データがありません。", "受注明細参照");
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
                    cancelFlag = 0;
                    //afterCellName = "日付始textBoxCell";
                }
                else
                {
                    MessageBox.Show("データ取得エラー。", "受注明細参照");
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付始textBoxCell");
                }
            }
            else if (e.CellName == "終了buttonCell")
            {
                this.Close();
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
                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt得意先S");
                }
            }
        }


        //class EditThenMoveNextAction : GrapeCity.Win.MultiRow.Action
        //{
        //    public override bool CanExecute(GcMultiRow target)
        //    {
        //        return true;
        //    }

        //    protected override void OnExecute(GcMultiRow target)
        //    {
        //        if (target.IsCurrentCellInEditMode == false && EditingActions.BeginEdit.CanExecute(target))
        //        {
        //            EditingActions.BeginEdit.Execute(target);
        //        }
        //        else
        //        {
        //            SelectionActions.MoveToNextCell.Execute(target);
        //        }
        //    }
        //}

 
        void gcMultiRow1_DataError(object sender, DataErrorEventArgs e)
        {
            string cellName = e.CellName;
        }

        //void gcMultiRow1_CellLeave(object sender, CellEventArgs e)
        //{
        //    string cellName = e.CellName;
        //}

        // 項目の最後で検索
        void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            //string cellName = e.CellName;
            //if (gcMultiRow1.CurrentCell is TextBoxCell)
            //    gcMultiRow1.BeginEdit(false);

            switch (e.CellName)
            {
                case "実行buttonCell":

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

                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt得意先S");
                    // blnSeach = false;

                    break;
            }

        }

        //void gcMultiRow1_CellBeginEdit(object sender, CellBeginEditEventArgs e)
        //{
        //    string cellName = e.CellName;
        //}

        
        void gcMultiRow1_SelectionChanged(object sender, EventArgs e)
        {
            this.Text = "Selected cell count: " + this.gcMultiRow1.SelectedCells.Count;
            this.Text += "  Selected row count: " + this.gcMultiRow1.SelectedRows.Count;
        }

        
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
                case "受注番号始gcTextBoxCell":
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

                case "受注番号終gcTextBoxCell":
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
                case "txt得意先S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value != null)
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value = (string)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value = null;
                    }
                    break;
                case "受注番号始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号始gcTextBoxCell"].Value != null)
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["受注番号終gcTextBoxCell"].Value = (string)gcMultiRow1.ColumnHeaders[0].Cells["受注番号始gcTextBoxCell"].Value;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["受注番号終gcTextBoxCell"].Value = null;
                    }
                    break;
                case "txt商品S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value != null)
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value = (string)gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value = null;
                    }
                    break;
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
                        gcMultiRow1.ColumnHeaders[0].Cells["日付終textBoxCell"].Value = null;
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
                        afterCellName = "受注番号終gcTextBoxCell";
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
                        MessageBox.Show("データ取得エラー。", "売上計上明細参照");
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
                        //SendKeys.Send("{ENTER}");
                        //SendKeys.Send("{UP}");
                    }
                    else if (ret == 1)
                    {
                        MessageBox.Show("データがありません。", "売上計上明細参照");
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
        
        // 検索実行
        private int execProcedure()
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
            command.CommandText = "受注明細参照";

            command.Parameters.AddWithValue("@参照区分", gcMultiRow1.ColumnHeaders[0].Cells["参照区分radioGroupCell"].EditedFormattedValue);
            command.Parameters.AddWithValue("@日付始", (String)gcMultiRow1.ColumnHeaders[0].Cells["日付始textBoxCell"].Value);
            command.Parameters.AddWithValue("@日付終", (String)gcMultiRow1.ColumnHeaders[0].Cells["日付終textBoxCell"].Value);
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["受注番号始gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@受注番号始", (String)gcMultiRow1.ColumnHeaders[0].Cells["受注番号始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注番号始", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["受注番号終gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@受注番号終", (String)gcMultiRow1.ColumnHeaders[0].Cells["受注番号終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注番号終", DBNull.Value);
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

            // txt得意先S
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先S", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先S", DBNull.Value);
            }

            // txt得意先E
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先E", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt得意先E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先E", DBNull.Value);
            }

            // txt商品S
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value != null)
            {
                command.Parameters.AddWithValue("@商品S", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品S", DBNull.Value);
            }

            // txt商品E
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value != null)
            {
                command.Parameters.AddWithValue("@商品E", (String)gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品E", DBNull.Value);
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

                        case "受注番号":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注番号textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "受注日":
                            if (dr[dc].ToString() != null)
                            {
                                DateTime jyuDate = DateTime.Parse(dr[dc].ToString());
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注日textBoxCell"].CellIndex, jyuDate.ToString("yyyy/MM/dd"));
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注日textBoxCell"].CellIndex, null);
                            }
                            break;
                        case "納期":
                            DateTime noDate = DateTime.Parse(dr[dc].ToString());
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["納期textBoxCell"].CellIndex, noDate.ToString("yyyy/MM/dd"));
                            break;
                        case "相手先注文番号":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["相手先注文番号textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "得意先コード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["得意先コードtextBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "得名":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["得名textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "商品コード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["商品コードtextBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "商名":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["商名textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "倉庫コード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["倉庫コードtextBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "受注数":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注数numericUpDownCell"].CellIndex, dr[dc]);
                            break;
                        case "受注単価":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["受注単価numericUpDownCell"].CellIndex, dr[dc]);
                            break;
                        case "発注番号":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["発注番号textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "仕入先コード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["仕入先コードtextBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "オペレーターコード":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["オペレーターコードtextBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "規格":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["規格textBoxCell"].CellIndex, dr[dc]);
                            break;
                        case "チェック":
                            this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["チェックcheckBoxCell"].CellIndex, dr[dc]);
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

        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F3:
                    target = this.ButtonF3;
                    String cname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (cname == "txt得意先S" || cname == "txt得意先E")
                    {
                        得意先検索Form fsToksaki = new 得意先検索Form();
                        fsToksaki = new 得意先検索Form();
                        fsToksaki.Owner = this;
                        fsToksaki.mTextBox = cname;
                        fsToksaki.Show();
                    }
                    else if (cname == "txt商品S" || cname == "txt商品E")
                    {
                        商品検索Form fsSyohin = new 商品検索Form();
                        fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.mTextBox = cname;
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
                        MessageBox.Show("データがありません。", "受注明細参照");
                    }
                    else
                    {
                        MessageBox.Show("データ取得エラー。", "受注明細参照");
                    }

                    gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt得意先S");

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
                    this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt得意先S");
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


        // 検索の戻り値
        public string ReceiveDataSyohin
        {
            set
            {
                receiveDataSyohin = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value = receiveDataSyohin.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataSyohin;
            }
        }

        public string ReceiveDataSyohin_E
        {
            set
            {
                receiveDataSyohin = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value = receiveDataSyohin.ToString();
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

        /*
                private void 受注明細参照Form_KeyDown(object sender, KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.F2)
                    {
                        if (this.ActiveControl is GcMultiRow)
                        {
                            //Console.WriteLine("F2 key down");

                            // F2キーによる編集の開始が必要な場合
                            EditingActions.BeginEdit.Execute(this.ActiveControl as GcMultiRow);
                        }
                        e.Handled = true;
                    }
                }
        */

        /*
                 void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
                 {
                     TextBoxEditingControl textBox = e.Control as TextBoxEditingControl;
                     if (textBox != null)
                     {
                         textBox.KeyDown -= new KeyEventHandler(textBox_KeyDown);
                         textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                     }
                 }

                 void textBox_KeyDown(object sender, KeyEventArgs e)
                 {
                     TextBoxEditingControl textBox1 = sender as TextBoxEditingControl;
                     Console.WriteLine(textBox1.GcMultiRow.CurrentCellPosition.ToString());

                     TextBoxEditingControl textBox = sender as TextBoxEditingControl;

                     // 呼び出し元のGcMultiRowコントロール

                     Console.WriteLine(textBox1.GcMultiRow.Name);

                     switch (gcMultiRow1.CurrentCellPosition.CellName)
                     {
                         case "クライアントgcTextBoxCell":
                             if (40 == e.KeyValue)
                             {
                                 Console.Write("Doen");
                             }
                             switch (e.KeyValue)
                             {
                                 case 37:    //Left
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
                                     break;
                                 case 38:    //Up
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
                                     break;
                                 case 39:    //Right
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
                                     break;
                                 case 40:    //Down
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                                     break;
                                 case 13:    //Enter
                                     execProcedure();
                                     break;
                                 case 9:    //Tab
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                                     break;
                             }
                             break;
                         case "オペレーターgcTextBoxCell":
                             switch (e.KeyCode)
                             {
                                 case Keys.Down:
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");
                                     break;
                                 case Keys.Up:
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                                     break;
                                 case Keys.Enter:
                                     execProcedure();
                                     break;
                             }
                             break;
                         case "リストbuttonCell":
                             switch (e.KeyCode)
                             {
                                 case Keys.Down:
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                                     break;
                                 case Keys.Up:
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
                                     break;
                                 case Keys.Enter:
                                     //this.Hide();
                                     break;
                             }
                             break;
                         case "終了buttonCell":
                             switch (e.KeyCode)
                             {
                                 case Keys.Down:
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                                     break;
                                 case Keys.Up:
                                     //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
                                     break;
                                 case Keys.Enter:
                                     this.Hide();
                                     break;
                             }
                             break;
                     }
                 }

                 private void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
                 {
                     // 編集用コントロールのKeyDownイベントの検出
                     e.Control.KeyDown -= editor_KeyDown;
                     e.Control.KeyDown += editor_KeyDown;

                 }

                 private void editor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
                 {
                     // キー操作による動作の実装 
                     int senderIndex = ((GrapeCity.Win.MultiRow.TextBoxEditingControl)(sender)).GcMultiRow.CurrentCell.CellIndex;
                     if (日付始index == senderIndex)
                     {
                         switch (e.KeyValue)
                         {
                             case 116:    //F5
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveDown.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 40:    //Down
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveDown.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 38:    //Up
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveUp.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             //case 37:    //Left
                             //    e.SuppressKeyPress = true;
                             //    SelectionActions.MoveLeft.Execute(((IEditingControl)sender).GcMultiRow);
                             //    break;
                             //case 39:    //Right
                             //    e.SuppressKeyPress = true;
                             //    SelectionActions.MoveRight.Execute(((IEditingControl)sender).GcMultiRow);
                             //    break;
                             case 13:    //Enter
                                 e.SuppressKeyPress = true;
                                 //gcMultiRow1.BeginEdit(true);
                                 //SelectionActions.MoveToNextCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
                                 EditingActions.BeginEdit.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                         }
                     }
                     else if (日付終index == senderIndex)
                     {
                         switch (e.KeyValue)
                         {
                             case 40:    //Down
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveDown.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 38:    //Up
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveUp.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 37:    //Left
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveLeft.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 39:    //Right
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveRight.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 13:    //Enter
                                 e.SuppressKeyPress = true;
                                 //SelectionActions.MoveToNextCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                         }
                     }
                     else if (クライアントindex == senderIndex)
                     {
                         switch (e.KeyValue)
                         {
                             case 40:    //Down
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveDown.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 38:    //Up
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveUp.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 37:    //Left
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveLeft.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 39:    //Right
                                 e.SuppressKeyPress = true;
                                 //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                                 SelectionActions.MoveToNextCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 116:    //F5
                                 int ret = execProcedure();
                                 if (ret == 0)
                                 {
                                     //SendKeys.Send("{ENTER}");
                                     //SendKeys.Send("{UP}");
                                     //Console.WriteLine("ffff");
                                     //afterCellName = "受注番号終gcTextBoxCell";
                                     gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号始gcTextBoxCell");
                                 }
                                 else if (ret == 1)
                                 {
                                     MessageBox.Show("データがありません。", "売上計上明細参照");
                                     //Cell currentCell = null;
                                     //gcMultiRow1.CancelEdit();
                                     gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                                     //gcMultiRow1.CancelEdit();
                                     cancelFlag = 0;
                                     //Cell currentCell = gcMultiRow1.ColumnHeaders[0].Cells[gcMultiRow1.Template.ColumnHeaders[0].Cells["日付始textBoxCell"].CellIndex];
                                     //gcMultiRow1.CancelEdit();
                                     afterCellName = "日付始textBoxCell";
                                 }
                                 else
                                 {
                                     MessageBox.Show("データ取得エラー。", "売上計上明細参照");
                                     gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                                     //Cell currentCell = gcMultiRow1.Rows[e.RowIndex].Cells[e.CellIndex];
                                 }
                                 e.SuppressKeyPress = true;
                                 //SelectionActions.MoveToNextCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                         }
                     }
                     else if (受注番号始index == senderIndex)
                     {
                         switch (e.KeyValue)
                         {
                             case 40:    //Down
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveDown.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 38:    //Up
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveUp.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 37:    //Left
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveLeft.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 39:    //Right
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveRight.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 13:    //Enter
                                 e.SuppressKeyPress = true;
                                 //SelectionActions.MoveToNextCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                         }
                     }
                     else if (受注番号終index == senderIndex)
                     {
                         switch (e.KeyValue)
                         {
                             case 40:    //Down
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveDown.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 38:    //Up
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveUp.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 37:    //Left
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveLeft.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 39:    //Right
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveRight.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 13:    //Enter
                                 e.SuppressKeyPress = true;
                                 //SelectionActions.MoveToNextCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                         }
                     }
                     else if (オペレーターindex == senderIndex)
                     {
                         switch (e.KeyValue)
                         {
                             case 40:    //Down
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveDown.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 38:    //Up
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveUp.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 37:    //Left
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveLeft.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 39:    //Right
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveRight.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             //case 13:    //Enter
                             case 116:    //F5
                                 int ret = execProcedure();
                                 if (ret == 0)
                                 {
                                     //SendKeys.Send("{ENTER}");
                                     //SendKeys.Send("{UP}");
                                     //Console.WriteLine("ffff");
                                     //afterCellName = "受注番号終gcTextBoxCell";
                                     gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号始gcTextBoxCell");
                                 }
                                 else if (ret == 1)
                                 {
                                     MessageBox.Show("データがありません。", "売上計上明細参照");
                                     //Cell currentCell = null;
                                     //gcMultiRow1.CancelEdit();
                                     gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                                     //gcMultiRow1.CancelEdit();
                                     cancelFlag = 0;
                                     //Cell currentCell = gcMultiRow1.ColumnHeaders[0].Cells[gcMultiRow1.Template.ColumnHeaders[0].Cells["日付始textBoxCell"].CellIndex];
                                     //gcMultiRow1.CancelEdit();
                                     afterCellName = "日付始textBoxCell";
                                 }
                                 else
                                 {
                                     MessageBox.Show("データ取得エラー。", "売上計上明細参照");
                                     gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "日付始textBoxCell");
                                     //Cell currentCell = gcMultiRow1.Rows[e.RowIndex].Cells[e.CellIndex];
                                 }
                                 e.SuppressKeyPress = true;
                                 //SelectionActions.MoveToNextCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                         }
                     }
                     else if (終了index == senderIndex)
                     {
                         switch (e.KeyValue)
                         {
                             case 40:    //Down
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveDown.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 38:    //Up
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveUp.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 37:    //Left
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveLeft.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 39:    //Right
                                 e.SuppressKeyPress = true;
                                 SelectionActions.MoveRight.Execute(((IEditingControl)sender).GcMultiRow);
                                 break;
                             case 13:    //Enter
                                 e.SuppressKeyPress = true;
                                 this.Close();
                                 break;
                         }
                     }
                 }
         */


    }
}
