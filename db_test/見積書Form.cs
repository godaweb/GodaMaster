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
    public partial class 見積書Form : Form
    {

        private DataSet dataSet;
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        private DataTable dataTableFAX;

        private string receiveDataToksaki = "";
        得意先検索Form fsToksaki;
        GcComboBoxCell comboBoxCell02;

        int rowcnt = 0;
        int fireKbn = 1;

        public 見積書Form()
        {
            InitializeComponent();
            fsToksaki = new 得意先検索Form();
            fsToksaki.Owner = this;
        }

        private void 見積書Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 見積書Template();
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
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            // GcMultiRowがフォーカスを失ったとき
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
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 出荷指示一覧表FunctionKeyAction(Keys.F3), Keys.F3);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // コンボボックス
            //comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["事業所gcComboBoxCell"] as GcComboBoxCell;
            //comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 事業所コード, 事業所名 FROM T事業所マスタ ORDER BY 事業所コード");
            //comboBoxCell02.ListHeaderPane.Visible = false;
            //comboBoxCell02.ListHeaderPane.Visible = false;
            //comboBoxCell02.ListHeaderPane.Visible = false;
            //comboBoxCell02.TextSubItemIndex = 0;
            //comboBoxCell02.TextSubItemIndex = 1;
            //comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            //comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            //comboBoxCell02.TextFormat = "[1]";

            // 初期表示
            DateTime dt = DateTime.Now;
            //dt = dt.AddDays(-1);
            gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value = dt.ToString("yyyy/MM/dd");
            gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value = dt.ToString("yyyy/MM/dd");
            gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value = "*";
            
            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt受注日S");

        }


        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "txt受注日S":
                    string buf = gcMultiRow1.CurrentCell.EditedFormattedValue.ToString();
                    if (buf != "*")
                    {
                        buf = buf.Replace("/", "");
                        buf = Utility.getDate(buf);
                    }
                    gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value = buf;
                    break;
            }
            switch (e.CellName)
            {
                case "txt受注日E":
                    string buf = gcMultiRow1.CurrentCell.EditedFormattedValue.ToString();
                    if (buf != "*")
                    {
                        buf = buf.Replace("/", "");
                        buf = Utility.getDate(buf);
                    }
                    gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value = buf;
                    break;
            }

        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "txt受注日S":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].EditedFormattedValue == null)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        e.Cancel = false;
                    }
                    break;
                case "txt受注日E":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].EditedFormattedValue == null)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        e.Cancel = false;
                    }
                    break;
                case "事業所コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell02, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            MessageBox.Show("マスタに存在しません。", "受注入力");
                            e.Cancel = true;
                        }
                    }
                    break;
            }
        }

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "プレビューbuttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            if (createReportData("0") == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();
                                プレビューform.dataSet = dataSet;
                                プレビューform.rptName = "見積書CrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "見積書");
                            }
                            break;
                    }
                    break;
                case "印刷buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            if (createReportData("0") == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();
                                プレビューform.dataTable = dataTable;
                                プレビューform.rptName = "見積書CrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "見積書");
                            }
                            break;
                    }
                    break;
                case "終了buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            this.Hide();
                            break;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "終了buttonCell")
            {
                this.Hide();
            }
            if (e.CellName == "プレビューbuttonCell")
            {
                if (createReportData("0") == 0)
                {
                    プレビューForm プレビューform = new プレビューForm();
                    プレビューform.dataTable = dataSet.Tables[0];
                    プレビューform.rptName = "見積書CrystalReport";
                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "見積書");
                }
            }
            else if (e.CellName == "印刷buttonCell")
            {
                if (createReportData("1") == 0)
                {
                    見積書CrystalReport cr = new 見積書CrystalReport();
                    cr.SetDataSource(dataSet.Tables[0]);
                    cr.PrintToPrinter(0, false, 0, 0);
                }
                else
                {
                    MessageBox.Show("データがありません", "見積書");
                }
            }
        }


        private int createReportData(string PrintMode)
        {

            int ret = 0;

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;

            // ストアド プロシージャ名を指定
            command.CommandText = "受注確認書";

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value != null)
            {
                command.Parameters.AddWithValue("@受注日", this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注日", "1960/01/01");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value != null)
            {
                command.Parameters.AddWithValue("@受注日E", this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注日E", "2100/12/31");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@受注番号始", this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注番号始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@受注番号終", this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注番号終", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先コード始", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先コード始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先コード終", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先コード終", "*");
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            dataSet = new DataSet();
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);


            //DataTableオブジェクトを用意
            dataTable = new DataTable();
            dataTable = dataSet.Tables[0];

            //dataTable.AcceptChanges();
            if (dataTable.Rows.Count <= 0)
            {
                ret = 1;
            }


            DataTable dt = new DataTable();

            // 3列定義します。
            dt.Columns.Add("案内文章", Type.GetType("System.String"));
            dataSet.Tables.Add(dt);

            // 4行追加します。
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataSet.Tables[0].Rows[i]["案内文章"] = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt案内文章"].Value;
            }

            //// DataSetにdtを追加します。
            //dataSet.Tables.Add(dt);


            return ret;
        }

        private void gcMultiRow1_CellContentClick_1(object sender, CellEventArgs e)
        {

        }


        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "txt受注日S":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
        //            }
        //            break;
        //        case "事業所コードtextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
        //            }
        //            break;
        //        case "事業所gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
        //            }
        //            break;
        //        case "受注NO始textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
        //            }
        //            break;
        //        case "受注NO終textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
        //            }
        //            break;
        //        case "得意先コード始textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
        //            }
        //            break;
        //        case "得意先コード終textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            break;
        //        case "プレビューbuttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            break;
        //        case "印刷buttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            break;
        //        case "終了buttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt受注日S");
        //            }
        //            break;
        //    }
        //}

    }
}
