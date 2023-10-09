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
using System.Configuration;
using System.IO;
using db_test.WTEXCEL受注ファイルDataSetTableAdapters;

namespace db_test
{
    public partial class EXCEL受注取込Form : Form
    {
        private string iniFile = null;
        private Encoding SJIS = Encoding.GetEncoding("Shift-JIS");
        private ExcelCsvClass excelCsvClass;
        //DataTable table = new DataTable("BULK");
        private WTEXCEL受注ファイルTableAdapter wtEXCEL受注ファイルtableAdapter = new WTEXCEL受注ファイルTableAdapter();
        DialogClass dialogClass = new DialogClass();
        string fileFilter = "EXCELファイル(*.xls;*.xlsx)|*.xls;*.xlsx|すべてのファイル(*.*)|*.*";
        string importFile = null;

        public EXCEL受注取込Form()
        {
            InitializeComponent();
        }

        private void EXCEL受注取込Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new EXCEL受注取込Template();
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

            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //gcMultiRow1.Validating += new CancelEventHandler(gcMultiRow1_Validating);
            //gcMultiRow1.Validated += new EventHandler(gcMultiRow1_Validated);
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);

            // GcMultiRowがフォーカスを失ったときに
            // セルのCellStyle.SelectionBackColorとCellStyle.SelectionForeColorを表示しない場合はtrue。
            gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);
            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            //gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Tab);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveRight, Keys.Right);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);

            // 各ファンクションキーに対応するアクションを登録する
            //this.gcMultiRow1.ShortcutKeyManager.Register(new 出荷指示一覧表FunctionKeyAction(Keys.F3), Keys.F3);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            iniFile = Properties.Settings.Default.EXCEL受注取込ファイル;
            gcMultiRow1.ColumnHeaders[0].Cells["受注ファイルtextBoxCell"].Value = iniFile;

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            int ret;

            GcMultiRow gcMultiRow = (GcMultiRow)sender;
            if (gcMultiRow.CurrentCell is ButtonCell)
            {
                switch (e.CellName)
                {
                    case "取込buttonCell":
                        if (gcMultiRow1.ColumnHeaders[0].Cells["受注ファイルtextBoxCell"].Value == null)
                        {
                            MessageBox.Show("ファイルを入力してください", "EOSドラスタ受注取込");
                            return;
                        }
                        else if (gcMultiRow1.ColumnHeaders[0].Cells["受注ファイルtextBoxCell"].Value.ToString() == "")
                        {
                            MessageBox.Show("ファイルを入力してください", "EOSドラスタ受注取込");
                            return;
                        }

                        string excelFile = gcMultiRow1.ColumnHeaders[0].Cells["受注ファイルtextBoxCell"].Value.ToString();

                        string csvFile =System.IO.Path.GetDirectoryName(@excelFile)+"\\data.csv";

                        if (System.IO.File.Exists(csvFile))
                        {
                            System.IO.File.Delete(@csvFile);
                        }

                        excelCsvClass = new ExcelCsvClass();
                        excelCsvClass.excel_RunMacro(excelFile);
                            
                            
                        if (!System.IO.File.Exists(csvFile))
                        {
                            MessageBox.Show("'" + csvFile + "'は存在しません。");
                            return;
                        }

                        ret = readExcel(csvFile);
                        if (ret == 0)
                        {
                            /*
                            ret = Jyuchu_Kosin_EXCEL();
                            if (ret == 0)
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
                                command.CommandText = "pr_BG";
                                command.ExecuteNonQuery();

                                command.Dispose();
                                connection.Close();
                            }
                            else
                            {
                                MessageBox.Show("受注更新に失敗しました", "EOSドラスタ受注取込");
                            }
                            */
                            MessageBox.Show("CSVの取り込みに成功しました", "EOSドラスタ受注取込");
                        }
                        else
                        {
                            MessageBox.Show("CSVの取り込みに失敗しました", "EOSドラスタ受注取込");
                        }
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "更新buttonCell");
                        break;
                    case "更新buttonCell":
                        ret = Jyuchu_Kosin_EXCEL();
                        if (ret == 0)
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
                            command.CommandText = "pr_BG";
                            command.ExecuteNonQuery();

                            command.Dispose();
                            connection.Close();
                            MessageBox.Show("受注更新に成功しました", "EOSドラスタ受注取込");
                        }
                        else
                        {
                            MessageBox.Show("受注更新に失敗しました", "EOSドラスタ受注取込");
                        }
                        break;
                    case "ファイルオープンbuttonCell":
                        importFile = dialogClass.OpenFileByDialog(iniFile, fileFilter);
                        gcMultiRow1.ColumnHeaders[0].Cells["受注ファイルtextBoxCell"].Value = importFile;
                        break;
                    case "プレビューbuttonCell":
                        DataTable dt = this.wtEXCEL受注ファイルtableAdapter.GetData();
                        if (dt.Rows.Count>0)
                        {
                            プレビューForm プレビューform = new プレビューForm();

                            プレビューform.dataTable = dt;
                            プレビューform.rptName = "EXCEL受注取込チェックリストCrystalReport";
                            プレビューform.Show();
                        }
                        else
                        {
                            MessageBox.Show("データがありません", "EXCEL受注取込チェックリストCrystalReport");
                        }
                        break;
                    case "終了buttonCell":
                        this.Hide();
                        break;
                }
            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "受注ファイルtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    break;
                case "ファイルオープンbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    break;
                case "取込buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    break;
                case "更新buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    break;
                case "終了buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    break;
                case "プレビューbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                    }
                    break;
                case "印刷buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                    }
                    break;
            }
        }

        void gcMultiRow1_Validated(object sender, EventArgs e)
        {
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "受注ファイルtextBoxCell":
                    
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "ファイルオープンbuttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルgcTextBoxCell");
                    break;
                case "取込buttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "更新buttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルgcTextBoxCell");
                    break;
                case "終了buttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "プレビューbuttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "印刷buttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
            }

        }

        void gcMultiRow1_Validating(object sender, CancelEventArgs e)
        {
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "受注ファイルgcTextBoxCell":
                    
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "ファイルオープンbuttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルgcTextBoxCell");
                    break;
                case "取込buttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "更新buttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルgcTextBoxCell");
                    break;
                case "終了buttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "プレビューbuttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "印刷buttonCell":
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
            }
        }

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int ret;
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "取込buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.LButton:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込buttonCell");
                            break;
                        case Keys.F5:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込buttonCell");
                            break;
                        case Keys.Down:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込buttonCell");
                            break;
                        case Keys.Up:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                            break;
                        case Keys.Enter:
                            if (gcMultiRow1.ColumnHeaders[0].Cells["受注ファイルtextBoxCell"].Value == null)
                            {
                                MessageBox.Show("ファイルを入力してください", "EOSドラスタ受注取込");
                                return;
                            }

                            string excelFile = gcMultiRow1.ColumnHeaders[0].Cells["受注ファイルtextBoxCell"].Value.ToString();

                            string csvFile =System.IO.Path.GetDirectoryName(@excelFile)+"\\data.csv";

                            if (System.IO.File.Exists(csvFile))
                            {
                                System.IO.File.Delete(@csvFile);
                            }

                            excelCsvClass = new ExcelCsvClass();
                            excelCsvClass.excel_RunMacro(excelFile);
                            
                            
                            if (!System.IO.File.Exists(csvFile))
                            {
                                MessageBox.Show("'" + csvFile + "'は存在しません。");
                                return;
                            }

                            ret = readExcel(csvFile);
                            if (ret == 0)
                            {
                                ret = Jyuchu_Kosin_EXCEL();
                                if (ret == 0)
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
                                    command.CommandText = "pr_BG";
                                    command.BeginExecuteNonQuery();

                                    command.Dispose();
                                    connection.Close();
                                }
                                else
                                {
                                    MessageBox.Show("受注更新に失敗しました", "EOSドラスタ受注取込");
                                }
                            }
                            else
                            {
                                MessageBox.Show("CSVの取り込みに失敗しました", "EOSドラスタ受注取込");
                            }

                            MessageBox.Show("受注更新に成功しました", "EOSドラスタ受注取込");
    
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "更新buttonCell");

                            break;
                    }
                    break;

                case "更新buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Down:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "更新buttonCell");
                            break;
                        case Keys.Up:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                            break;
                        case Keys.Enter:

                            if (Jyuchu_Kosin_EXCEL()==0)
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
                                command.CommandText = "pr_BG";
                                command.BeginExecuteNonQuery();

                            }

                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");

                            break;
                    }
                    break;

                case "ファイルオープンbuttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Down:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込buttonCell");
                            break;
                        case Keys.Up:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                            break;
                        case Keys.Enter:
                            //OpenFileDialogクラスのインスタンスを作成
                            OpenFileDialog openFileDialog1 = new OpenFileDialog();
                            iniFile = System.Configuration.ConfigurationSettings.AppSettings["EOSDORASTAJYUTYUDIR"];

                            //ファイル名を指定する
                            openFileDialog1.FileName = iniFile;
                            //フォルダを指定する
                            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
                            openFileDialog1.InitialDirectory = @iniFile;
                            //[ファイルの種類]に表示される選択肢を指定する
                            openFileDialog1.Filter = "CSVファイル(*.csv;*.txt)|*.csv;*.txt|すべてのファイル(*.*)|*.*";
                            //[ファイルの種類]ではじめに選択されるものを指定する
                            //2番目の「すべてのファイル」が選択されているようにする
                            openFileDialog1.FilterIndex = 1;
                            openFileDialog1.Title = "開くファイルを選択してください";
                            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
                            openFileDialog1.RestoreDirectory = true;
                            //存在しないファイルの名前が指定されたとき警告を表示する
                            openFileDialog1.CheckFileExists = true;
                            //存在しないパスが指定されたとき警告を表示する
                            openFileDialog1.CheckPathExists = true;

                            //ダイアログを表示する
                            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                //fireFLG = false;

                                gcMultiRow1.ColumnHeaders[0].Cells["受注ファイルtextBoxCell"].Value = openFileDialog1.FileName;
                                Properties.Settings.Default.EOSDORASTAJYUTYUFILE = openFileDialog1.FileName;
                                Properties.Settings.Default.Save();
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                                //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込buttonCell");

                                //OKボタンがクリックされたとき、選択されたファイルを読み取り専用で開く
                                System.IO.Stream stream;
                                stream = openFileDialog1.OpenFile();
                                if (stream != null)
                                {
                                    //内容を読み込み、表示する
                                    System.IO.StreamReader sr =
                                        new System.IO.StreamReader(stream);
                                    Console.WriteLine(sr.ReadToEnd());
                                    //閉じる
                                    sr.Close();
                                    stream.Close();
                                }
                            }
                            break;
                        case Keys.LButton:
                            //OpenFileDialogクラスのインスタンスを作成
                            openFileDialog1 = new OpenFileDialog();
                            iniFile = System.Configuration.ConfigurationSettings.AppSettings["EOSDORASTAJYUTYUDIR"];

                            //ファイル名を指定する
                            openFileDialog1.FileName = iniFile;
                            //フォルダを指定する
                            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
                            openFileDialog1.InitialDirectory = @iniFile;
                            //[ファイルの種類]に表示される選択肢を指定する
                            openFileDialog1.Filter = "CSVファイル(*.csv;*.txt)|*.csv;*.txt|すべてのファイル(*.*)|*.*";
                            //[ファイルの種類]ではじめに選択されるものを指定する
                            //2番目の「すべてのファイル」が選択されているようにする
                            openFileDialog1.FilterIndex = 1;
                            openFileDialog1.Title = "開くファイルを選択してください";
                            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
                            openFileDialog1.RestoreDirectory = true;
                            //存在しないファイルの名前が指定されたとき警告を表示する
                            openFileDialog1.CheckFileExists = true;
                            //存在しないパスが指定されたとき警告を表示する
                            openFileDialog1.CheckPathExists = true;

                            //ダイアログを表示する
                            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                //fireFLG = false;

                                gcMultiRow1.ColumnHeaders[0].Cells["受注ファイルtextBoxCell"].Value = openFileDialog1.FileName;
                                Properties.Settings.Default.EOSDORASTAJYUTYUFILE = openFileDialog1.FileName;
                                Properties.Settings.Default.Save();
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                                //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込buttonCell");

                                //OKボタンがクリックされたとき、選択されたファイルを読み取り専用で開く
                                System.IO.Stream stream;
                                stream = openFileDialog1.OpenFile();
                                if (stream != null)
                                {
                                    //内容を読み込み、表示する
                                    System.IO.StreamReader sr =
                                        new System.IO.StreamReader(stream);
                                    Console.WriteLine(sr.ReadToEnd());
                                    //閉じる
                                    sr.Close();
                                    stream.Close();
                                }
                            }
                            break;
                    }
                    break;
                case "受注ファイルtextBoxCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Down:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
                            break;
                        case Keys.Up:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
                            break;
                        case Keys.Enter:
                            /*
                            if (createReportData() == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();

                                プレビューform.dataSet = dataSet;
                                プレビューform.rptName = "得意先別商品別掛率リストCrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "得意先別商品別掛率リスト");
                            }
                            */
                            break;
                    }
                    break;
                case "プレビューbuttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Down:
                            //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
                            break;
                        case Keys.Up:
                            //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
                            break;
                        case Keys.Enter:
                            /*
                            if (createReportData() == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();

                                プレビューform.dataSet = dataSet;
                                プレビューform.rptName = "得意先別商品別掛率リストCrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "得意先別商品別掛率リスト");
                            }
                            */
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

        void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "受注ファイルgcTextBoxCell":
                //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "ファイルオープンbuttonCell":
                //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルgcTextBoxCell");
                    break;
                case "取込buttonCell":
                //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "更新buttonCell":
                //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルgcTextBoxCell");
                    break;
                case "終了buttonCell":
                //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "プレビューbuttonCell":
                //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
                case "印刷buttonCell":
                //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "ファイルオープンbuttonCell");
                    break;
            }
        }

        private int readExcel(string fileName)
        {
            // ■ＣＳＶファイルよりＲＥＡＤ■
            int i;  string TextLine;    string Fl_name; int FLG;

            string kbn;       // レコード区分
            // ヘッダー
            object H2;        // 帳票区分
            object H3;        // 伝票番号
            object H5;        // 店コード
            object H6;        // 部門
            object H56;       // 店コード+部門
            object H7;        // 発注日
            object H8;        // 店納品日
            object H9;        // 取引先コード
            object H11;       // 社名
            object H12;       // 店名
            object H13;       // 部名
            object H14;       // 発注区分
            object H15;       // 請求月
            object H17;       // ＥＯＳ区分
            object H18;       // 社コード
            object H19;       // 店コードＢ
            object H20;       // 社名Ｂ
            object H21;       // 直送区分
            object H22;       // 客注区分
            object H23;       // 
            object H24;       // 経費区分
            object H25;       // 単価履歴区分
            object H26;       // 回答名
            object H27;       // 担当者コード
            object H28;       // 伝票摘要
            object H29;       // GODA商品コード
            object H30;       // 請求先コード
            object H31;       // 原価単価

            // ボディー
            object B3;       // 伝票行
            object B4;       // 商品コード
            object B5=null;       // 入数
            object B6;       // C/S数
            object B7;       // 発注単位
            object B8;       // 数量
            object B9;       // 原価単価
            object B10;      // 原価金額
            object B11;      // 商品名
            object B12;      // 規格
            object B13;      // 表示価格
            object B14;      // 棚番
            object B17;      // ＪＡＮコード
            object B18;      // 商品番号
            object B19;      // 本部原価単価
            object B20;      // 本部原価金額
            object B21;      // 納入単価
            object B22;      // 店舗売価

            // 変数
            string DMY;
            string strDMY;
            string TOK_NM;        // 得意先名
            string TAN_CD;        // 担当者コード
            int URI_KBN;      // 売上切捨区分
            int UZEI_KBN;     // 売上税区分
            string BUK_CD;        // 部課コード
            string SEIKYUSAKI_CD; // 請求先ｺｰﾄﾞ
            string SYO_CD;        // 商品コード
            string SYO_NM;        // 商品名
            string KIKAKU;        // 規格
            int ZAI_KBN;      // 在庫管理区分
            string HIN_CD;        // 品種コード
            string MEK_CD;        // メーカーコード
            decimal GENKA;       // 原価単価
            int TNI_CD;       // 単位コード
            decimal GK_RITU;       // 代表原価掛率
            decimal RITU;          // 掛率
            decimal SU_KBN;      // 外内税区分
            decimal SYO_RITU;    // 消費税率
            decimal NSYO_RITU;   // 新消費税率
            object NSYO_TEKI;    // 新消費税適用
            decimal TEIKA;       // 定価
            decimal ZAIKO;       // 在庫数 04/01/29
            decimal JUCHUZAN;       // 受注残数 04/01/29
            decimal HACHUZAN;       // 発注残数 04/01/29

            object MAKER_HIN;    // メーカー品番
            object TANA_BAN;     // 棚番
            object MAKER_NM;     // メーカー名

            decimal NONYU_TANKA;       // 納入単価
            decimal TENPO_BAIKA;       // 店舗売価＝表示価格

            string Err_FlgA;
            string Err_FlgB;
            string Err_FlgC;

            string fPath;
            int idif;
            string ERR_SYO_CD;        // エラー商品コード
            string[] X;
            string T2;
            bool dorastaKBN;
            string buf;

//            SqlBulkCopy bulkCopy;
            bool firstLine = false;
            string connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            DataSet WTEXCEL受注ファイルdataSet = new WTEXCEL受注ファイルDataSet();
            DataTable WTEXCEL受注ファイルdataTable = new DataTable();
            WTEXCEL受注ファイルdataTable = WTEXCEL受注ファイルdataSet.Tables["WTEXCEL受注ファイル"];
            DataRow newRow;

            SqlDb db = new SqlDb();
            db.Connect();
            db.BeginTransaction();
            db.ExecuteSql("delete from WTEXCEL受注ファイル", -1);
            db.CommitTransaction();
            db.Disconnect();

            string line = "";
            int icnt = 0;
            using (StreamReader sr = new StreamReader(fileName, SJIS))
            {
                while ((line = sr.ReadLine()) != null)
                {

                    DataTable table = null;
                    string sql = "";

                    // 最初の行を見出しとして無視する
                    if (firstLine == true)
                    {
                        firstLine = false;
                        continue;
                    }

                    X = line.Split(',');

                    if (X[30] == "450099")
                        dorastaKBN = true;
                    else
                        dorastaKBN = false;

                    Err_FlgA = null;
                    //< ヘッダー >=============================================================================
                    H2 = Utility.Z_Set(null);             //Z_Set((Mid$(TextLine, 2, 2)))  //帳票区分
                    H3 = Utility.S_Set(X[4]);             //S_Set(Mid$(TextLine, 7, 6))    //伝票番号
                    //店コード
                    H5 = Utility.Right("0000" + X[12], 4);   //Right$("000" & CStr(Mid$(TextLine, 18, 3)), 3)   //2012/1 h.yamamoto chg
                    //部門
                    H6 = null;                    //CStr(Mid$(TextLine, 21, 1))     //2012/1 h.yamamoto chg
                    // 得意先コード(店コード+部門)
                    if (dorastaKBN)
                    {
                        if (X[5] == "11")
                        {
                            //H56 = Right$("000000" & CStr(X(12)), 6)
                            H56 = Utility.Right("000" + X[11].ToString(), 3) + Utility.Right("0000" + X[12].ToString(), 4);
                        }
                        else if (X[5] == "21" && X[6] == "1")
                        {
                            H56 = Utility.Right("000" + X[11].ToString(), 3) + Utility.Right("0000" + X[12].ToString(), 4);
                        }
                        else
                        {
                            //H56 = Right$("000000" & CStr(X(12)), 6)
                            H56 = Utility.Right("000" + X[11].ToString(), 3) + Utility.Right("0000" + X[12].ToString(), 4);
                            // 得意先マスタチェック
                            table = new DataTable();
                            sql = "SELECT A.業態コード";
                            sql = sql + " FROM T得意先マスタ AS A ";
                            sql = sql + " WHERE A.得意先コード = '" + H56 + "' ";
                            table = Utility.GetComboBoxData(sql);
                            if (table.Rows.Count > 0)
                            {
                                if (table.Rows[0]["業態コード"] == null)
                                {
                                    H56 = H56;
                                }
                                else
                                {
                                    H56 = table.Rows[0]["業態コード"];
                                }
                            }
                            else
                            {
                                Err_FlgA = "A";
                                H56 = null;
                            }
                        }
                    }
                    else
                    {
                        H56 = System.Convert.ToString(X[12]);
                        // 得意先マスタチェック
                        sql = "SELECT A.業態コード";
                        sql = sql + " FROM T得意先マスタ AS A ";
                        sql = sql + " WHERE A.得意先コード = '" + H56 + "' ";
                        table = Utility.GetComboBoxData(sql);
                        if (table.Rows.Count == 0)
                        {
                            Err_FlgA = "A";
                            H56 = null;
                        }
                    }

                    //発注日
                    DMY = X[3].ToString();
                    H7 = null;
                    if (Utility.S_Set(DMY) != " ")
                    {
                        H7 = DateTime.Parse(Utility.Left(DMY.ToString(), 4) + "/" + Utility.Mid(DMY.ToString(), 6, 2) + "/" + Utility.Mid(DMY.ToString(), 9, 2)).AddDays(0);
                    }
                    //店納品日
                    DMY = X[3].ToString();
                    H8 = null;
                    if (Utility.S_Set(DMY) != " ")
                    {
                        H8 = DateTime.Parse(Utility.Left(DMY.ToString(), 4) + "/" + Utility.Mid(DMY.ToString(), 6, 2) + "/" + Utility.Mid(DMY.ToString(), 9, 2)).AddDays(1);
                    }
                    H9 = Utility.Z_Set(X[10]);    //Z_Set(Mid$(TextLine, 39, 6))   //取引先コード    //2012/1 h.yamamoto chg
                    H11 = Utility.S_Set(null);    //S_Set(Mid$(TextLine, 48, 20)) //社名     //2012/1 h.yamamoto chg
                    H12 = Utility.S_Set(null);    //S_Set(Mid$(TextLine, 68, 10)) //店名    //2012/1 h.yamamoto chg
                    H13 = Utility.S_Set(null);    //S_Set(Mid$(TextLine, 78, 10)) //部名   //2012/1 h.yamamoto chg
                    H14 = Utility.Z_Set(X[7]);    //Z_Set(Mid$(TextLine, 88, 1))  //発注区分    //2012/1 h.yamamoto chg
                    H15 = null;   //Z_Set(Null)    //Z_Set(Mid$(TextLine, 89, 2))  //請求月
                    H17 = Utility.Z_Set(X[5]);    //Z_Set(Mid$(TextLine, 99, 2))  //ＥＯＳ区分  //2012/1 h.yamamoto chg
                    H18 = Utility.Z_Set(X[11]);   //Z_Set(Mid$(TextLine, 13, 4))  //社コード   //2012/1 h.yamamoto chg
                    H19 = H5;                            //店コードＢ
                    H20 = H11;                           //社名Ｂ
                    H21 = Utility.Z_Set(X[8]);    //Z_Set(Mid$(TextLine, 101, 1)) //直送区分    //2012/1 h.yamamoto chg
                    H22 = Utility.Z_Set(X[6]);    //Z_Set(Mid$(TextLine, 102, 1)) //客注区分   //2012/1 h.yamamoto chg
                    H23 = Utility.Z_Set(1);
                    H24 = Utility.Z_Set(X[24]);         //経費区分
                    H25 = Utility.Z_Set(X[25]);       //単価履歴区分(単価更新フラグ)
                    H26 = Utility.S_Set(X[26]);              //回答名
                    H27 = Utility.S_Set(X[27]);       //担当者コード
                    H28 = Utility.S_Set(X[28]);       //伝票摘要
                    H29 = Utility.S_Set(X[29]);       //GODA商品コード
                    
                    Err_FlgB = null;
                    Err_FlgC = null;
                    //ボディー -------------------------------------------------------
                    B3 = Utility.Z_Set(X[2]);     //Z_Set(Mid$(TextLine, 4, 2)) //伝票行  //2012/1 h.yamamoto chg
                    B4 = Utility.S_Set(X[14]);    //S_Set(Mid$(TextLine, 6, 13)) //商品コード  //2012/1 h.yamamoto chg
                    B6 = Utility.Z_Set(null);     //Z_Set(Mid$(TextLine, 22, 4))  //C/S数   //2012/1 h.yamamoto chg
                    B8 = Utility.Z_Set(X[20]);    //Z_Set(Mid$(TextLine, 28, 5))//数量   //2012/1 h.yamamoto chg
                    B9 = Utility.Z_Set(X[17]);   //Z_Set(Mid$(TextLine, 33, 7))//原価単価  //2012/1 h.yamamoto chg
                    B10 = Math.Floor(Convert.ToDecimal(B8) * Convert.ToDecimal(B9));   //Z_Set(Mid$(TextLine, 40, 9))//原価金額   //2012/1 h.yamamoto chg
                    //===============================
                    B7 = Utility.S_Set(null);     //S_Set(Mid$(TextLine, 26, 2)) //発注単位  //2012/1 h.yamamoto chg
                    //B11 = Utility.Left(Utility.S_Set(X[15]), 20);   //S_Set(Mid$(TextLine, 49, 20))//ＥＯＳ商品名  //2012/1 h.yamamoto chg
                    //B12 = Utility.Left(Utility.S_Set(X[16]), 20);   //S_Set(Mid$(TextLine, 69, 9)) //ＥＯＳ規格  //2012/1 h.yamamoto chg
                    buf = Utility.S_Set(X[15]);
                    buf = CSharp.Japanese.Kanaxs.Kana.ToHankaku(buf);
                    buf = CSharp.Japanese.Kanaxs.Kana.ToHankakuKana(buf);
                    B11 = Utility.Left(buf, 20);   //S_Set(Mid$(TextLine, 49, 20))//ＥＯＳ商品名  //2012/1 h.yamamoto chg
                    buf = Utility.S_Set(X[16]);
                    buf = CSharp.Japanese.Kanaxs.Kana.ToHankaku(buf);
                    buf = CSharp.Japanese.Kanaxs.Kana.ToHankakuKana(buf);
                    B12 = Utility.Left(buf, 9);   //S_Set(Mid$(TextLine, 69, 9)) //ＥＯＳ規格  //2012/1 h.yamamoto chg
                    B13 = Utility.Z_Set(X[19]);   //Z_Set(Mid$(TextLine, 78, 7))//表示価格  //2012/1 h.yamamoto chg
                    B14 = Utility.Z_Set(null);    //Z_Set(Mid$(TextLine, 85, 6))//棚番  //2012/1 h.yamamoto chg
                    B17 = Utility.Z_Set(X[14]);   //Z_Set(Mid$(TextLine, 101, 13))//ＪＡＮコード  //2012/1 h.yamamoto chg
                    B18 = Utility.S_Set(null);    //S_Set(Mid$(TextLine, 114, 8))//商品番号  //2012/1 h.yamamoto chg
                    B19 = B9;                             //本部原価単価
                    B20 = B10;                            //本部原価金額
                    B21 = Utility.Z_Set(X[18]);              //B13 //納入単価  //2012/1 h.yamamoto chg
                    B22 = Utility.Z_Set(X[19]);  //Z_Set(Mid$(TextLine, 122, 7))//店舗売価  //2012/1 h.yamamoto chg

                    TOK_NM = "";
                    TAN_CD = "";
                    URI_KBN = 0;
                    UZEI_KBN = 0;
                    BUK_CD = "";
                    SEIKYUSAKI_CD = ""; 

                    sql = "SELECT A.得意先名,A.担当者コード,A.売上切捨区分,A.売上税区分,B.部課コード,A.請求先コード";
                    sql = sql + " FROM T得意先マスタ AS A ";
                    sql = sql + " LEFT JOIN T担当者マスタ AS B ";
                    sql = sql + " ON A.担当者コード = B.担当者コード";
                    sql = sql + " WHERE A.得意先コード = '" + H56.ToString() + "'";

                    table = Utility.GetComboBoxData(sql);

                    if (table.Rows.Count > 0)
                    {
                        TOK_NM = table.Rows[0]["得意先名"].ToString();
                        TAN_CD = table.Rows[0]["担当者コード"].ToString();
                        URI_KBN = Convert.ToInt32(table.Rows[0]["売上切捨区分"]);
                        UZEI_KBN = Convert.ToInt32(table.Rows[0]["売上税区分"]);
                        BUK_CD = table.Rows[0]["部課コード"].ToString();
                        SEIKYUSAKI_CD = table.Rows[0]["請求先コード"].ToString();
                    }
                    else
                    {
                        Err_FlgA = "A";
                    }

                    SYO_CD = "";
                    SYO_NM = "";
                    KIKAKU = "";
                    ZAI_KBN = 0;
                    HIN_CD = "";
                    MEK_CD = "";
                    GENKA = 0;
                    TNI_CD = 0;
                    GK_RITU = 0;
                    RITU = 0;
                    SU_KBN = 0;
                    SYO_RITU = 0;
                    NSYO_RITU = 0;
                    NSYO_TEKI = null;
                    MAKER_HIN = "";
                    TANA_BAN = "";
                    MAKER_NM = "";
                    ZAIKO = 0;
                    JUCHUZAN = 0;
                    HACHUZAN = 0;
                    TEIKA = 0;

                    if (dorastaKBN)
                    {

                        //商品変換マスタチェック
                        sql = "SELECT A.商品コード FROM T商品変換マスタ AS A ";
                        sql = sql + " WHERE A.得意先コード = '450099'";
                        sql = sql + " AND A.先方商品コード = '" + B4 + "'";

                        table = Utility.GetComboBoxData(sql);

                        if (table.Rows.Count > 0)
                        {
                            SYO_CD = table.Rows[0]["商品コード"].ToString();
                        }
                        else
                        {
                            Err_FlgB = "B";
                            SYO_CD = B4.ToString();
                        }
                    }
                    else
                    {
                        SYO_CD = H29.ToString();
                        SYO_NM = B11.ToString();
                    }

                    //商品マスタチェック
                    sql = "SELECT A.商品コード,A.商品名,A.規格,A.在庫管理区分,A.品種コード,A.メーカーコード,";
                    sql = sql + " A.原価単価,A.単位コード,A.代表原価掛率,1 AS 掛率,";
                    sql = sql + " A.外内税区分,A.消費税率,A.新消費税率,A.新消費税適用,";
                    sql = sql + " A.メーカー品番,A.棚番,C.メーカー名,A.定価";
                    sql = sql + " ,A.現在在庫数,A.受注残数,A.発注残数";
                    sql = sql + " ,A.入数";                                          //2012/1 h.yamamoto add
                    sql = sql + " FROM T商品マスタ AS A ";
                    sql = sql + " LEFT JOIN Tメーカーマスタ AS C ON A.メーカーコード = C.メーカーコード";
                    sql = sql + " WHERE A.商品コード = '" + SYO_CD + "'";

                    table = Utility.GetComboBoxData(sql);

                    if (table.Rows.Count > 0)
                    {
                        SYO_CD = table.Rows[0]["商品コード"].ToString();
                        SYO_NM = table.Rows[0]["商品名"].ToString();
                        KIKAKU = table.Rows[0]["規格"].ToString();
                        ZAI_KBN = Convert.ToInt32(Utility.Z_Set(table.Rows[0]["在庫管理区分"]));
                        HIN_CD = Utility.S_Set(table.Rows[0]["品種コード"].ToString());
                        MEK_CD = Utility.S_Set(table.Rows[0]["メーカーコード"].ToString());
                        GENKA = Utility.Z_Set(table.Rows[0]["原価単価"]);
                        TNI_CD = Convert.ToInt32(Utility.Z_Set(table.Rows[0]["単位コード"]));
                        GK_RITU = Utility.Z_Set(table.Rows[0]["代表原価掛率"]);
                        if (Utility.Z_Set(table.Rows[0]["定価"]).Equals(0))
                        {
                            RITU = 0;
                        }
                        else
                        {
                            RITU = Convert.ToDecimal((Convert.ToInt32(B9) * 100000 / Utility.Z_Set(table.Rows[0]["定価"]))) / 100000;
                        }
                        SU_KBN = Utility.Z_Set(table.Rows[0]["外内税区分"]);
                        SYO_RITU = Utility.Z_Set(table.Rows[0]["消費税率"]);
                        NSYO_RITU = Utility.Z_Set(table.Rows[0]["新消費税率"]);
                        NSYO_TEKI = Utility.S_Set(table.Rows[0]["新消費税適用"].ToString());
                        MAKER_HIN = Utility.S_Set(table.Rows[0]["メーカー品番"].ToString());
                        TANA_BAN = Utility.S_Set(table.Rows[0]["棚番"].ToString());
                        MAKER_NM = Utility.S_Set(table.Rows[0]["メーカー名"].ToString());
                        TEIKA = Utility.Z_Set(table.Rows[0]["定価"]);
                        ZAIKO = Utility.Z_Set(table.Rows[0]["現在在庫数"]);
                        JUCHUZAN = Utility.Z_Set(table.Rows[0]["受注残数"]);
                        HACHUZAN = Utility.Z_Set(table.Rows[0]["発注残数"]);
                        B5 = Utility.Z_Set(table.Rows[0]["入数"]);

                        //受注残数カウント　04/01/29
                        /*
                        strSQL = "SELECT 受注残カウント"
                        strSQL = strSQL & " FROM WT_ドラスタ受注残カウント AS A"
                        strSQL = strSQL & " WHERE 商品コード = //" & S_Set(RDS_SYO!商品コード) & "//;"
                        Set RDS_SYOCNT = db.OpenRecordset(strSQL)

                        If RDS_SYOCNT.BOF = True Or RDS_SYOCNT.EOF = True Then
                            strSQL = "INSERT INTO WT_ドラスタ受注残カウント(商品コード,受注残カウント)"
                            strSQL = strSQL & "VALUES(//" & S_Set(RDS_SYO!商品コード) & "//," & Z_Set(JUCHUZAN) + Z_Set(B8) & ");"
                            dbMain.Execute (strSQL)
                        Else
                            strSQL = "UPDATE WT_ドラスタ受注残カウント "
                            strSQL = strSQL & "SET 受注残カウント = 受注残カウント + " & Z_Set(B8)
                            strSQL = strSQL & " WHERE 商品コード = //" & S_Set(RDS_SYO!商品コード) & "//;"
                            dbMain.Execute (strSQL)
                            JUCHUZAN = Z_Set(RDS_SYOCNT!受注残カウント) - Z_Set(B8) //04/01/29
                        End If
                        */
                    }
                    else
                    {
                        Err_FlgC = "C";
                        ERR_SYO_CD = SYO_CD;
                        //商品マスタチェック
                        SYO_CD = "99999999999";
                        sql = "SELECT A.商品コード,A.商品名,A.規格,A.在庫管理区分,A.品種コード,A.メーカーコード,";
                        sql = sql + " A.原価単価,A.単位コード,A.代表原価掛率,1 AS 掛率,";
                        sql = sql + " A.外内税区分,A.消費税率,A.新消費税率,A.新消費税適用,";
                        sql = sql + " A.メーカー品番,A.棚番,C.メーカー名,A.定価";
                        sql = sql + " ,A.現在在庫数,A.受注残数,A.発注残数";
                        sql = sql + " ,A.入数";
                        sql = sql + " FROM T商品マスタ AS A ";
                        sql = sql + " LEFT JOIN Tメーカーマスタ AS C ON A.メーカーコード = C.メーカーコード";
                        sql = sql + " WHERE A.商品コード = '" + SYO_CD + "'";

                        table = Utility.GetComboBoxData(sql);

                        if (table.Rows.Count > 0)
                        {
                            //========================
                            //"99999999999"のデータ
                            //========================
                            SYO_CD = Utility.S_Set(table.Rows[0]["商品コード"].ToString());
                            SYO_NM = Utility.S_Set(table.Rows[0]["商品名"].ToString());
                            KIKAKU = Utility.S_Set(table.Rows[0]["規格"].ToString());
                            ZAI_KBN = Convert.ToInt32(Utility.Z_Set(table.Rows[0]["在庫管理区分"]));
                            HIN_CD = Utility.S_Set(table.Rows[0]["品種コード"].ToString());
                            MEK_CD = Utility.S_Set(table.Rows[0]["メーカーコード"].ToString());
                            GENKA = Utility.Z_Set(table.Rows[0]["原価単価"]);
                            TNI_CD = Convert.ToInt32(Utility.Z_Set(table.Rows[0]["単位コード"]));
                            GK_RITU = Utility.Z_Set(table.Rows[0]["代表原価掛率"]);
                            if (Utility.Z_Set(table.Rows[0]["定価"]).Equals(0))
                            {
                                RITU = 0;
                            }
                            else
                            {
                                RITU = Convert.ToDecimal((Convert.ToInt32(B9) / Utility.Z_Set(table.Rows[0]["定価"]) * 100000)) / 100000;
                            }
                            SU_KBN = Utility.Z_Set(table.Rows[0]["外内税区分"]);
                            SYO_RITU = Utility.Z_Set(table.Rows[0]["消費税率"]);
                            NSYO_RITU = Utility.Z_Set(table.Rows[0]["新消費税率"]);
                            NSYO_TEKI = Utility.Z_Set(table.Rows[0]["新消費税適用"]);
                            MAKER_HIN = Utility.S_Set(table.Rows[0]["メーカー品番"].ToString());
                            TANA_BAN = Utility.S_Set(table.Rows[0]["棚番"].ToString());
                            MAKER_NM = Utility.S_Set(table.Rows[0]["メーカー名"].ToString());
                            TEIKA = Utility.Z_Set(table.Rows[0]["定価"]);

                            ZAIKO = Utility.Z_Set(table.Rows[0]["現在在庫数"]);
                            JUCHUZAN = Utility.Z_Set(table.Rows[0]["受注残数"]);
                            HACHUZAN = Utility.Z_Set(table.Rows[0]["発注残数"]);
                            B5 = Utility.Z_Set(table.Rows[0]["入数"]);

                            //受注残数カウント　04/01/29
                            /*
                            strSQL = "SELECT 受注残カウント"
                            strSQL = strSQL & " FROM WT_ドラスタ受注残カウント AS A"
                            strSQL = strSQL & " WHERE 商品コード = //99999999999//;"
                            Set RDS_SYOCNT = db.OpenRecordset(strSQL)

                            If RDS_SYOCNT.BOF = True Or RDS_SYOCNT.EOF = True Then
                                strSQL = "INSERT INTO WT_ドラスタ受注残カウント(商品コード,受注残カウント)"
                                strSQL = strSQL & "VALUES(//99999999999//," & Z_Set(JUCHUZAN) + Z_Set(B8) & ");"
                                dbMain.Execute (strSQL)
                             Else
                                strSQL = "UPDATE WT_ドラスタ受注残カウント "
                                strSQL = strSQL & "SET 受注残カウント = 受注残カウント + " & Z_Set(B8)
                                strSQL = strSQL & " WHERE 商品コード = //99999999999//;"
                                dbMain.Execute (strSQL)
                                JUCHUZAN = Z_Set(RDS_SYOCNT!受注残カウント) - Z_Set(B8) //04/01/29
                            End If
                            */
                        }
                    }

                    // 小数点第3位を切り捨て
                    GENKA = Utility.Z_Set(X[31]);                              // 原価単価
                    if (TEIKA > 0)
                    {
                        GK_RITU = Math.Truncate(GENKA / TEIKA * 1000) / 1000;  // 原価掛率
                    }
                    else
                    {
                        GK_RITU = 0;
                    }
                    NONYU_TANKA = Utility.Z_Set(X[18]);                              // 納入単価
                    TENPO_BAIKA = Utility.Z_Set(X[19]);                              // 納入単価
                    if (NONYU_TANKA > 0)
                    {
                        RITU = Math.Truncate((TENPO_BAIKA / NONYU_TANKA * 1000)) / 1000;  // 納品掛率
                    }
                    else
                    {
                        RITU = 0;
                    }

                    newRow = WTEXCEL受注ファイルdataTable.NewRow();
                    
                    newRow["本支店区分"] = 1;  //Z_Set(Forms.F_会社基本.本支店区分);
                    newRow["処理コード"] = 3;
                    newRow["入力区分"] = 1;
                    newRow["処理番号"] = 0;    // 管理番号より
                    newRow["エラーフラグ"] = 0;
                    newRow["理由コード"] = DBNull.Value;
                    newRow["受発注行数"] = 0;
                    newRow["受注番号"] = 0;
                    newRow["売上伝票番号"] = DBNull.Value;
                    newRow["相手先注文番号"] = H3;
                    newRow["自社受付番号"] = null;
                    newRow["処理日"] = DateTime.Now;
                    //newRow["受注日"] = DBNull.Value;
                    newRow["受注日"] = H7;
                    newRow["売上日"] = DBNull.Value;
                    newRow["納入日"] = DBNull.Value;
                    //newRow["納期"] = DBNull.Value;
                    newRow["納期"] = H8;
                    newRow["処理区"] = DBNull.Value;
                    newRow["請求月区分コード"] = DBNull.Value;
                    newRow["得意先コード"] = H56;
                    newRow["得名"] = TOK_NM;
                    newRow["事業所コード"] = "1";
                    newRow["ランク"] = 0;
                    newRow["部課コード"] = BUK_CD;
                    newRow["担当者コード"] = TAN_CD;
                    newRow["代理店コード"] = null;
                    newRow["代名"] = null;
                    newRow["納入先コード"] = null;
                    newRow["納名"] = null;
                    newRow["請求先コード"] = SEIKYUSAKI_CD;
                    newRow["売上切捨区分"] = URI_KBN;
                    newRow["売上税区分"] = UZEI_KBN;
                    newRow["伝票摘要"] = H28;
                    newRow["配送区分"] = null;
                    newRow["商品コード"] = SYO_CD;
                    newRow["商名"] = SYO_NM;
                    newRow["規格"] = KIKAKU;
                    newRow["形式寸法"] = null;
                    newRow["材質"] = null;
                    newRow["分類"] = 0;
                    //newRow["在庫管理区分"] = ZAI_KBN;
                    newRow["在庫管理区分"] = -1;
                    newRow["在庫管理INDEX"] = 1;
                    newRow["品種コード"] = HIN_CD;
                    newRow["メーカーコード"] = MEK_CD;
                    newRow["入数"] = B5;
                    newRow["単位コード"] = TNI_CD;
                    newRow["倉庫コード"] = "1";
                    newRow["ケース数"] = B6;
                    newRow["受注数"] = B8;
                    newRow["指示累計数"] = 0;
                    newRow["売上累計数"] = 0;
                    newRow["受注単価"] = B13;           // 売単価
                    newRow["受注金額"] = B10;
                    newRow["税抜売上金額"] = 0;
                    newRow["原価単価"] = GENKA;
                    newRow["原価金額"] = Convert.ToInt32(B8) * GENKA;
                    newRow["粗利"] = Convert.ToDecimal(B10) - Convert.ToDecimal((Convert.ToInt32(B8) * GENKA));
                    newRow["消費税"] = 0;
                    newRow["税抜仕入金額"] = 0;
                    newRow["外内税区分"] = SU_KBN;
                    newRow["消費税率"] = SYO_RITU;
                    newRow["新消費税率"] = NSYO_RITU;
                    newRow["新消費税適用"] = NSYO_TEKI;
                    newRow["明細摘要"] = null;
                    newRow["発注番号"] = 0;
                    newRow["発注連番"] = 0;
                    newRow["発注納期"] = DBNull.Value;
                    newRow["仕入伝票番号"] = DBNull.Value;
                    newRow["仕入区分コード"] = DBNull.Value;
                    newRow["仕入先コード"] = DBNull.Value;
                    newRow["仕入システム区分"] = DBNull.Value;
                    newRow["支払月区分コード"] = DBNull.Value;
                    newRow["仕入先コード"] = DBNull.Value;
                    newRow["仕名"] = DBNull.Value;
                    newRow["仕入分類"] = 0;
                    newRow["仕入事業所コード"] = DBNull.Value;
                    newRow["仕入担当者コード"] = DBNull.Value;
                    newRow["仕入切捨区分"] = 0;
                    newRow["仕入税区分"] = 0;
                    newRow["チェック"] = 0;
                    newRow["完了フラグ"] = 0;
                    newRow["完了INDEX"] = 0;
                    newRow["WS_ID"] = "04";
                    newRow["受注更新フラグ"] = 0;
                    newRow["得意先更新フラグ"] = 0;
                    newRow["商品更新フラグ"] = 0;
                    newRow["商品倉庫更新フラグ"] = 0;
                    newRow["仕入先更新フラグ"] = 0;
                    newRow["オペレーターコード"] = "92";
                    newRow["修正オペレーターコード"] = "1";
                    newRow["受注行"] = B3;
                    newRow["処理月日"] = DBNull.Value;
                    newRow["管理年月"] = 0;
                    newRow["受注行番号"] = B3;
                    newRow["定価"] = B9;
                    newRow["納品掛率"] = RITU;
                    newRow["原価掛率"] = GK_RITU;
                    newRow["発注有無区分"] = 0;
                    newRow["回答納期"] = H8;
                    //newRow["回答納期"] = DBNull.Value;
                    newRow["在庫数"] = ZAIKO; // 04/01/29
                    newRow["受注残数"] = JUCHUZAN; // 04/01/29
                    newRow["発注残数"] = HACHUZAN; // 04/01/29
                    newRow["出荷指示発行フラグ"] = 0;
                    newRow["商品注意事項"] = null;
                    newRow["発注摘要"] = null;
                    newRow["売上区分コード"] = 1;
                    newRow["システム区分"] = 101;
                    newRow["処理区分"] = DBNull.Value;
                    newRow["得意先"] = null;
                    newRow["仕入先"] = null;
                    newRow["商品"] = null;
                    newRow["回答コード"] = DBNull.Value;
                    newRow["回答名"] = H26;
                    newRow["前受注数"] = DBNull.Value;
                    newRow["発注書発行フラグ"] = DBNull.Value;
                    newRow["注文書発行フラグ"] = DBNull.Value;
                    newRow["取引先コード"] = H9;
                    newRow["社名"] = H11;
                    newRow["店名"] = H12;
                    newRow["部名"] = H13;
                    newRow["発注区分"] = 1;
                    //newRow["請求月"] = H15;
                    newRow["請求月"] = DBNull.Value;
                    newRow["EOS区分"] = 21;
                    newRow["帳票区分"] = DBNull.Value;
                    newRow["発注単位"] = B7;
                    newRow["EOS商品コード"] = B4;
                    newRow["EOS商品名"] = B11;
                    buf = KIKAKU;
                    buf = CSharp.Japanese.Kanaxs.Kana.ToHankaku(buf);
                    buf = CSharp.Japanese.Kanaxs.Kana.ToHankakuKana(buf);
                    buf = Utility.Left(buf, 9);   //S_Set(Mid$(TextLine, 69, 9)) //ＥＯＳ規格  //2012/1 h.yamamoto chg
                    newRow["EOS規格"] = buf;
                    //newRow["EOS規格"] = B12;
                    newRow["表示価格"] = B13;
                    newRow["EOS棚番"] = B14;
                    newRow["JANコード"] = B17;
                    newRow["メーカー品番"] = MAKER_HIN;
                    newRow["棚番"] = TANA_BAN;
                    newRow["メーカー名"] = MAKER_NM;
                    newRow["店コード"] = H5;
                    //newRow["部コード"] = H6;
                    newRow["部コード"] = DBNull.Value;
                    newRow["更新ビット"] = DBNull.Value;
                    newRow["商品番号"] = B18;
                    newRow["社コード"] = H18;
                    newRow["店コードB"] = H19;
                    newRow["直送区分"] = H21;
                    newRow["客注区分"] = H22;
                    newRow["経費区分"] = H24;
                    newRow["返品区分"] = 0;
                    newRow["本部原価単価"] = Utility.Z_Set(X[19]);
                    if (dorastaKBN)
                    {
                        newRow["本部原価金額"] = Utility.Z_Set(X[20]) * Utility.Z_Set(X[19]);
                        newRow["納入単価"] = Utility.Z_Set(X[19]);
                    }
                    else
                    {
                        newRow["本部原価金額"] = B20;
                        newRow["納入単価"] = B21;
                    }
                    newRow["店舗売価"] = Utility.Z_Set(X[17]);

                    DMY = null;
                    if (Err_FlgA != null)
                    {
                        DMY = Err_FlgA;
                    }
                    if (Err_FlgB != null)
                    {
                        DMY = DMY + Err_FlgB;
                        DMY = Err_FlgB;
                    }
                    if (Err_FlgC != null)
                    {
                        //DMY = DMY + Err_FlgC;
                        DMY = Err_FlgC;
                    }

                    newRow["エラー区分"] = DMY;

                    newRow["大分類コード"] = Utility.S_Set(X[13]);
                    newRow["店舗備考"] = Utility.S_Set(X[23]);
                    newRow["仕入先備考"] = DBNull.Value;
                    if ((decimal)H25 == 0)
                    {
                        newRow["単価更新フラグ"] = 1;
                    }
                    else
                    {
                        newRow["単価更新フラグ"] = 0;
                    }
                    newRow["登録番号"] = Utility.Z_Set(X[0]);        // 2012/1 h.yamamoto add
                    newRow["ドラスタ発注番号"] = Utility.Z_Set(X[1]);   // 2012/1 h.yamamoto add
                    newRow["客注番号"] = null;        // 2012/1 h.yamamoto add
                    newRow["発注数"] = 0;
                    newRow["ドラスタ区分"] = dorastaKBN;
                    newRow["東西区分"] = 0;

                    WTEXCEL受注ファイルdataTable.Rows.Add(newRow);

                    /*
                    DMY = null;
                    if (Err_FlgA != null)
                    {
                        DMY = Err_FlgA.ToString();
                    }
                    if (Err_FlgB != null)
                    {
                        //DMY = DMY + Err_FlgB;
                        DMY = DMY.ToString() + Err_FlgB.ToString();
                    }
                    if (Err_FlgC != null)
                    {
                        //DMY = DMY + Err_FlgC;
                        DMY = DMY.ToString() + Err_FlgC.ToString();
                    }
                    if (DMY == null)
                    {
                        strDMY = null;
                    }
                    else
                    {
                        strDMY = DMY.ToString();
                    }

                    WTEXCEL受注ファイルtableAdapter = new WTEXCEL受注ファイルTableAdapter();
                    WTEXCEL受注ファイルtableAdapter.Insert(
                         1,
                         3,
                         1,
                         0,
                         Convert.ToBoolean(0),
                         null,
                         0,
                         0,
                         null,
                         H3.ToString(),
                         null,
                         DateTime.Today,
                         Convert.ToDateTime(H7),
                         null,
                         null,
                         Convert.ToDateTime(H8),
                         null,
                         null,
                         H56.ToString(),
                         TOK_NM,
                         "1",
                         0,
                         BUK_CD,
                         TAN_CD,
                         null,
                         null,
                         null,
                         null,
                         SEIKYUSAKI_CD,
                         Convert.ToInt16(URI_KBN),
                         Convert.ToInt16(UZEI_KBN),
                         H28.ToString(),
                         null,
                         SYO_CD,
                         SYO_NM,
                         KIKAKU,
                         null,
                         null,
                         0,
                         Convert.ToBoolean(ZAI_KBN),
                         0,
                         HIN_CD,
                         MEK_CD,
                         0,
                         Convert.ToInt16(TNI_CD),
                         "1",
                         Convert.ToDecimal(B6),
                         Convert.ToDecimal(B8),
                         0,
                         0,
                         Convert.ToDecimal(B13),
                         Convert.ToDecimal(B10),
                         0,
                         GENKA,
                         Convert.ToInt32(B8) * GENKA,
                         Convert.ToDecimal(B10) - Convert.ToDecimal((Convert.ToInt32(B8) * GENKA)),
                         0,
                         0,
                         0,
                         Convert.ToInt16(SU_KBN),
                         SYO_RITU,
                         NSYO_RITU,
                         null,   //NSYO_TEKI,
                         null,
                         0,
                         0,
                         null,
                         null,
                         null,
                         null,
                         null,
                         null,
                         null,
                         0,
                         null,
                         null,
                         0,
                         0,
                         Convert.ToBoolean(0),
                         Convert.ToBoolean(0),
                         0,
                         04,
                         Convert.ToBoolean(0),
                         Convert.ToBoolean(0),
                         Convert.ToBoolean(0),
                         Convert.ToBoolean(0),
                         Convert.ToBoolean(0),
                         "92",
                         "1",
                         Convert.ToInt32(B3),
                         null,
                         0,
                         Convert.ToInt32(B3),
                         Convert.ToDecimal(B9),
                         RITU,
                         GK_RITU,
                         0,
                         null,    //H8,
                         ZAIKO,
                         JUCHUZAN,
                         HACHUZAN,
                         0,
                         null,
                         null,
                         1,
                         101,
                         null,
                         null,
                         null,
                         null,
                         null,
                         null,
                         null,
                         null,
                         null,
                         Convert.ToInt32(H9),
                         H11.ToString(),
                         H12.ToString(),
                         H13.ToString(),
                         Convert.ToInt32(H14),
                         Convert.ToInt32(H15),
                         H17.ToString(),
                         null,
                         B7.ToString(),
                         B4.ToString(),
                         Utility.Right(B11.ToString(), 5), //10
                         null, //Utility.Right(B12.ToString(), 2),  //4
                         Convert.ToDecimal(B13),
                         Convert.ToDecimal(B14),
                         Convert.ToDecimal(B17),
                         MAKER_HIN.ToString(),
                         TANA_BAN.ToString(),
                         MAKER_NM.ToString(),
                         Convert.ToInt32(H5),
                         Convert.ToInt32(H6),
                         strDMY,
                         null,
                         B18.ToString(),
                         Convert.ToInt32(H18),
                         Convert.ToInt32(H19),
                         Convert.ToInt32(H21),
                         Convert.ToInt32(H22),
                         Convert.ToInt32(H24),
                         0,
                         Utility.Z_Set(X[19]),
                         Utility.Z_Set(X[20]) * Utility.Z_Set(X[19]),
                         Utility.Z_Set(X[19]),
                         Utility.Z_Set(X[17]),
                         Utility.S_Set(X[13]),
                         Utility.S_Set(X[23]),
                         null,
                         Convert.ToBoolean(0),
                         Convert.ToInt32(X[0]),
                         Convert.ToInt32(X[1]),
                         null,
                         Convert.ToInt32(X[9]),
                         dorastaKBN,
                         0);
                         */
                }
                sr.Close();
            }

            // 明細件数なしチェック
            /*
            connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            using (bulkCopy = new SqlBulkCopy(connectionString)) ;
            {
                bulkCopy.BulkCopyTimeout = 600; // in seconds
                bulkCopy.DestinationTableName = "WTドラスタ受注ファイル";
                bulkCopy.WriteToServer(this.table);
            }
            */

            string cst = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cst))
            {
                // フィールド名のマッピング
                foreach (var column in WTEXCEL受注ファイルdataTable.Columns)
                {
                    if (column.ToString() != "エラーA" & column.ToString() != "エラーB" & column.ToString() != "エラーC")
                    {
                        bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());
                    }
                }
                bulkCopy.BulkCopyTimeout = 600; // in seconds
                bulkCopy.DestinationTableName = WTEXCEL受注ファイルdataTable.TableName; // テーブル名をSqlBulkCopyに教える
                bulkCopy.WriteToServer(WTEXCEL受注ファイルdataTable);

                //bulkCopy.BulkCopyTimeout = 600; // in seconds
                //bulkCopy.DestinationTableName = "T受注戻しファイル";
                //bulkCopy.WriteToServer(dataTable);
            }

            //明細件数なしチェック
            string sql2 = "SELECT A.受注連番";
            sql2 = sql2 + " FROM WTEXCEL受注ファイル AS A";

            DataTable table2 = Utility.GetComboBoxData(sql2);

            if (table2.Rows.Count <= 0)
            {
                MessageBox.Show("明細がありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -9;
            }

            //エラー件数カウント
            /*
            sql2 = "SELECT Count(A.エラー区分) AS CNT";
            sql2 = sql2 + " FROM WTEXCEL受注ファイル AS A";
            sql2 = sql2 + " HAVING Count(A.エラー区分)is not null;";

            table2 = Utility.GetComboBoxData(sql2);

            if (table2.Rows.Count > 0)
            {
                icnt = 0;
                for (i = 0; i <= table2.Rows.Count - 1; i++)
                {
                    icnt = (int)table2.Rows[i]["CNT"];
                }
                if (icnt > 0)
                {
                    MessageBox.Show("取り込み時エラーが " + table2.Rows.Count + "件 あります.", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }
            }
            */

            DataTable dt = this.wtEXCEL受注ファイルtableAdapter.GetDataBy受注取込エラーリスト();
            if (dt.Rows.Count > 0)
            {
                プレビューForm プレビューform = new プレビューForm();

                プレビューform.dataTable = dt;
                プレビューform.rptName = "EXCEL受注取込エラーリストCrystalReport";
                プレビューform.Show();
                return 1;
            }

            /*
            If Rds.BOF = False And Rds.EOF = False Then
                If Rds!CNT <> 0 Then
                    MsgBox "取り込み時エラーが " & Rds!CNT & "件 あります.", vbCritical, "ドラスタ受注取込"
                    //エラーリスト表示
                    DoCmd.OpenReport "R_ドラスタ受注エラーリスト", acViewPreview, strSQL
    
                    strSQL = "SELECT Count(A.エラー区分) AS CNT"
                    strSQL = strSQL & " FROM WT_ドラスタ受注ファイル AS A"
                    strSQL = strSQL & " HAVING Left(S_SET(A.エラー区分),1) = //A//;"
                    Set Rds = db.OpenRecordset(strSQL)
    
                    If Rds!CNT = 0 Then
                        Read_CSV = 0
                    End If
                    Exit Function
                End If
            End If
            */

            //正常件数カウント
            sql2 = "SELECT Count(A.受注連番) AS CNT";
            sql2 = sql2 + " FROM WTEXCEL受注ファイル AS A";

            table2 = Utility.GetComboBoxData(sql2);

            if (table2.Rows.Count > 0)
            {
                icnt = 0;
                for (i = 0; i <= table2.Rows.Count - 1; i++)
                {
                    icnt = (int)table2.Rows[i]["CNT"];
                }
                //if (icnt > 0)
                //{
                //    MessageBox.Show(table2.Rows.Count + "件の明細を正常に取り込みました.", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }

            //using (bulkCopy = new SqlBulkCopy(connectionString)) ;
            //{
            //    bulkCopy.BulkCopyTimeout = 600; // in seconds
            //    bulkCopy.DestinationTableName = "WTドラスタ受注ファイル";
            //    bulkCopy.WriteToServer(WTドラスタ受注ファイルdataTable);
            //}

            return 0;

        }

        public int Jyuchu_Kosin_EXCEL()
        {
            // ***************************受入処理******************************
            // １：処理番号カウント更新
            // ２：処理番号を会社基本にセット
            // ３：入力区分に"1"セット
            // ４：排他チェック
            // ５：T_受注戻ファイルに追加
            // ６：T_処理履歴ファイルに追加
            // ７：排他解除
            // 戻り値：False（失敗） True（成功）
            // *****************************************************************
            int ret = 0;
            SqlDb sqlDb = new SqlDb();
            DataTable dataTable;
            sqlDb.Connect();

            Form F_Kihon;         // F_会社基本
            DateTime S_date;          // ｼｽﾃﾑ日付
            string strSQL;
            bool tankaKosinFLG;
            bool dorastaKBN;

            // ----------------------
            // T_ＷＳ番号
            // ----------------------
            long W_Syori_no;      // 処理番号
            // *********
            // 変数
            // *********
            // -------------------------------------
            // 相手先注文番号,受注行番号 退避
            // -------------------------------------
            long BEF_CHU_NO;
            long CUR_CHU_NO;
            int CHU_GYO;

            // 処理番号リード
            strSQL = "";

            dataTable = sqlDb.ExecuteSql("SELECT 処理番号,WS番号 FROM TＷＳ番号 WHERE ＫＥＹ=1", -1);

            if (dataTable.Rows.Count > 0)
            {
                if (dataTable.Rows[0]["処理番号"] == null)
                {
                    W_Syori_no = 1;
                }
                else
                {
                    W_Syori_no = Convert.ToInt64(dataTable.Rows[0]["処理番号"]) + 1;
                }
            }
            else
            {
                if (dataTable.Rows[0]["WS番号"] == null)
                {
                    W_Syori_no = 1;
                }
                else
                {
                    W_Syori_no = Convert.ToInt64(dataTable.Rows[0]["WS番号"]) * 10000000 + 1;
                }
            }

            //dataTable.Dispose();

            // ｼｽﾃﾑ日付取得
            S_date = DateTime.Today;
            //S_date = DateTime.Today.ToString("yyyy/MM/dd");

            // T_ドラスタ受注ファイルより相手先注文番号⇒ドラスタ発注番号,受注行番号リード　'2012/3　Moriguchi
            CUR_CHU_NO = 0; CHU_GYO = 0;

            strSQL = "SELECT ドラスタ発注番号,受注行番号,単価更新フラグ,ドラスタ区分 FROM WTEXCEL受注ファイル ORDER BY ドラスタ発注番号,受注行番号";

            dataTable = sqlDb.ExecuteSql(strSQL, -1);

            if (dataTable.Rows.Count != 0)
            {
                BEF_CHU_NO = Convert.ToInt64(dataTable.Rows[0]["ドラスタ発注番号"]);
                CHU_GYO = Convert.ToInt32(dataTable.Rows[0]["受注行番号"]);
            }
            else
            {
                return 1;
            }

            tankaKosinFLG = Convert.ToBoolean(dataTable.Rows[0]["単価更新フラグ"]);
            dorastaKBN = Convert.ToBoolean(dataTable.Rows[0]["ドラスタ区分"]);

            // ****************************
            // メインＬＯＯＰ ＳＴＡＲＴ
            // ****************************
            for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
            {
                CHU_GYO = Convert.ToInt32(dataTable.Rows[i]["受注行番号"]);

                // -------------------------------
                // T_受注戻ファイルに挿入
                // -------------------------------
                strSQL = "INSERT INTO T受注戻しファイル ( ";
                strSQL = strSQL + "本支店区分, 処理コード, 入力区分, 処理番号, エラーフラグ, 受発注行数,";
                strSQL = strSQL + "受注番号, 相手先注文番号, 自社受付番号, 処理日, 受注日, 納期, 処理区, 得意先コード,";
                strSQL = strSQL + "得名, 事業所コード, ランク, 部課コード, 担当者コード, 代理店コード, 代名, 納入先コード,";
                strSQL = strSQL + "納名, 請求先コード, 売上切捨区分, 売上税区分, 伝票摘要, 配送区分, 商品コード, 商名,";
                strSQL = strSQL + "規格, 形式寸法, 材質, 分類, 在庫管理区分, 品種コード, メーカーコード, 入数, 単位コード,";
                strSQL = strSQL + "倉庫コード, ケース数, 受注数, 指示累計数, 売上累計数, 受注単価, 受注金額, 原価単価,";
                strSQL = strSQL + "原価金額, 粗利, 外内税区分, 消費税率, 新消費税率, 新消費税適用, 明細摘要, 発注番号,";
                strSQL = strSQL + "発注連番, 発注納期, 仕入先コード, 仕名, 仕入分類, 仕入事業所コード, 仕入担当者コード,";
                strSQL = strSQL + "仕入切捨区分, 仕入税区分, チェック, 完了フラグ, WS_ID, オペレーターコード, ";
                strSQL = strSQL + "修正オペレーターコード, 受注行, 処理月日, 管理年月, 受注行番号, 定価, 納品掛率,";
                strSQL = strSQL + "原価掛率, 発注有無区分, 回答納期, 在庫数, 受注残数, 発注残数, 出荷指示発行フラグ,";
                strSQL = strSQL + "商品注意事項, 発注摘要, 売上区分コード, システム区分, 回答コード, 回答名 ";
                // 2012/1 h.yamamoto add str *------*
                strSQL = strSQL + ", 取引先コード ";
                strSQL = strSQL + ", 社名 ";
                strSQL = strSQL + ", 店名 ";
                strSQL = strSQL + ", 部名 ";
                strSQL = strSQL + ", 発注区分 ";
                strSQL = strSQL + ", 請求月 ";
                strSQL = strSQL + ", ＥＯＳ区分 ";
                strSQL = strSQL + ", 帳票区分 ";
                strSQL = strSQL + ", 発注単位 ";
                strSQL = strSQL + ", ＥＯＳ商品コード ";
                strSQL = strSQL + ", ＥＯＳ商品名 ";
                strSQL = strSQL + ", ＥＯＳ規格 ";
                strSQL = strSQL + ", 表示価格 ";
                strSQL = strSQL + ", ＥＯＳ棚番 ";
                strSQL = strSQL + ", ＪＡＮコード ";
                strSQL = strSQL + ", メーカー品番 ";
                strSQL = strSQL + ", 棚番 ";
                strSQL = strSQL + ", メーカー名 ";
                strSQL = strSQL + ", 店コード ";
                strSQL = strSQL + ", 部コード ";
                strSQL = strSQL + ", エラー区分 ";
                strSQL = strSQL + ", 更新ビット ";
                strSQL = strSQL + ", 商品番号 ";
                strSQL = strSQL + ", 社コード ";
                strSQL = strSQL + ", 店コードＢ ";
                strSQL = strSQL + ", 直送区分 ";
                strSQL = strSQL + ", 客注区分 ";
                strSQL = strSQL + ", 経費区分 ";
                strSQL = strSQL + ", 返品区分 ";
                strSQL = strSQL + ", 本部原価単価 ";
                strSQL = strSQL + ", 本部原価金額 ";
                strSQL = strSQL + ", 納入単価 ";
                strSQL = strSQL + ", 店舗売価 ";
                // strSQL = strSQL & ", 取込日付 "
                strSQL = strSQL + ", 大分類コード ";
                strSQL = strSQL + ", 店舗備考 ";
                strSQL = strSQL + ", 仕入先備考 ";
                strSQL = strSQL + ", 単価更新フラグ ";
                strSQL = strSQL + ", 登録番号 ";
                strSQL = strSQL + ", ドラスタ発注番号 ";
                strSQL = strSQL + ", 客注番号 ";
                // *------* 2012/1 h.yamamoto add end
                strSQL = strSQL + " )";
                strSQL = strSQL + " SELECT A.本支店区分, A.処理コード, A.入力区分, " + W_Syori_no + ", A.エラーフラグ,";
                strSQL = strSQL + "A.受発注行数, A.受注番号, A.相手先注文番号, A.自社受付番号, A.処理日, A.受注日, A.納期,";
                strSQL = strSQL + "A.処理区, A.得意先コード, A.得名, A.事業所コード, A.ランク, A.部課コード, A.担当者コード,";
                strSQL = strSQL + "A.代理店コード, A.代名, A.納入先コード, A.納名, A.請求先コード, A.売上切捨区分, A.売上税区分,";
                strSQL = strSQL + "A.伝票摘要, A.配送区分, A.商品コード , A.商名, A.規格, A.形式寸法, A.材質, A.分類,";
                strSQL = strSQL + "A.在庫管理区分, A.品種コード, A.メーカーコード, A.入数, A.単位コード, A.倉庫コード,";
                strSQL = strSQL + "A.ケース数, A.受注数, A.指示累計数, A.売上累計数, A.受注単価, A.受注金額, A.原価単価,";
                strSQL = strSQL + "A.原価金額, A.粗利, A.外内税区分, A.消費税率, A.新消費税率, A.新消費税適用, A.明細摘要,";
                strSQL = strSQL + "A.発注番号, A.発注連番, A.発注納期, A.仕入先コード, A.仕名, A.仕入分類 , A.仕入事業所コード,";
                strSQL = strSQL + "A.仕入担当者コード, A.仕入切捨区分, A.仕入税区分, A.チェック, A.完了フラグ, A.WS_ID,";
                strSQL = strSQL + "A.オペレーターコード, A.修正オペレーターコード, A.受注行, A.処理月日, A.管理年月, A.受注行番号,";
                strSQL = strSQL + "A.定価, A.納品掛率, A.原価掛率, A.発注有無区分, A.回答納期, A.在庫数, A.受注残数, A.発注残数,";
                strSQL = strSQL + "A.出荷指示発行フラグ, A.商品注意事項, A.発注摘要, A.売上区分コード, A.システム区分,";
                strSQL = strSQL + "A.回答コード, A.回答名 ";
                // 2012/1 h.yamamoto add str *------*
                strSQL = strSQL + ", A.取引先コード ";
                strSQL = strSQL + ", A.社名 ";
                strSQL = strSQL + ", A.店名 ";
                strSQL = strSQL + ", A.部名 ";
                strSQL = strSQL + ", A.発注区分 ";
                strSQL = strSQL + ", A.請求月 ";
                strSQL = strSQL + ", A.ＥＯＳ区分 ";
                strSQL = strSQL + ", A.帳票区分 ";
                strSQL = strSQL + ", A.発注単位 ";
                strSQL = strSQL + ", A.ＥＯＳ商品コード ";
                strSQL = strSQL + ", A.ＥＯＳ商品名 ";
                strSQL = strSQL + ", A.ＥＯＳ規格 ";
                strSQL = strSQL + ", A.表示価格 ";
                strSQL = strSQL + ", A.ＥＯＳ棚番 ";
                strSQL = strSQL + ", A.ＪＡＮコード ";
                strSQL = strSQL + ", A.メーカー品番 ";
                strSQL = strSQL + ", A.棚番 ";
                strSQL = strSQL + ", A.メーカー名 ";
                strSQL = strSQL + ", A.店コード ";
                strSQL = strSQL + ", A.部コード ";
                strSQL = strSQL + ", A.エラー区分 ";
                strSQL = strSQL + ", A.更新ビット ";
                strSQL = strSQL + ", A.商品番号 ";
                strSQL = strSQL + ", A.社コード ";
                strSQL = strSQL + ", A.店コードＢ ";
                strSQL = strSQL + ", A.直送区分 ";
                strSQL = strSQL + ", A.客注区分 ";
                strSQL = strSQL + ", A.経費区分 ";
                strSQL = strSQL + ", A.返品区分 ";
                strSQL = strSQL + ", A.本部原価単価 ";
                strSQL = strSQL + ", A.本部原価金額 ";
                strSQL = strSQL + ", A.納入単価 ";
                strSQL = strSQL + ", A.店舗売価 ";
                // strSQL = strSQL & ", A.取込日付 "
                strSQL = strSQL + ", A.大分類コード ";
                strSQL = strSQL + ", A.店舗備考 ";
                strSQL = strSQL + ", A.仕入先備考 ";
                strSQL = strSQL + ", A.単価更新フラグ";
                // strSQL = strSQL & ", A.登録番号 "
                // strSQL = strSQL & ", A.ドラスタ発注番号 "
                strSQL = strSQL + ", null ";
                strSQL = strSQL + ", null ";
                strSQL = strSQL + ", A.客注番号 ";
                // *------* 2012/1 h.yamamoto add end
                strSQL = strSQL + " FROM WTEXCEL受注ファイル AS A";
                strSQL = strSQL + " WHERE A.ドラスタ発注番号 = " + BEF_CHU_NO;
                strSQL = strSQL + " AND A.受注行番号 =" + CHU_GYO;
                sqlDb.BeginTransaction();
                sqlDb.ExecuteSql(strSQL, -1);
                sqlDb.CommitTransaction();

                if (dataTable.Rows.Count.Equals(i + 1))
                {
                    CUR_CHU_NO = 0;
                }
                else
                {
                    CUR_CHU_NO = Convert.ToInt64(dataTable.Rows[i + 1]["ドラスタ発注番号"]); ;
                }

                // ドラスタ発注番号ブレークでT_処理履歴に投げる
                if (BEF_CHU_NO != CUR_CHU_NO)
                {
                    // ---------------------
                    // F_会社基本セット
                    // ---------------------
                    /*
                    F_Kihon.入力区分 = 1;
                    F_Kihon.事業所コード = "1";
                    F_Kihon.処理番号 = W_Syori_no;
                    F_Kihon.番号 = 0;
                    F_Kihon.仕入番号 = 0;
                    F_Kihon.入金番号 = 0;
                    F_Kihon.シス区分 = 101;
                    F_Kihon.OPE = "92";
                    */

                    // ----------------------------
                    // T_処理履歴テーブルセット
                    // ----------------------------
                    strSQL = "INSERT INTO T処理履歴テーブル ( ";
                    strSQL = strSQL + " 本支店区分, 処理コード, 処理名, 入力区分, 事業所コード, 処理番号,";
                    strSQL = strSQL + " 売上伝票番号, 仕入伝票番号, 入金番号, システム区分, 処理日, 更新フラグ,";
                    strSQL = strSQL + " オペレーターコード )";
                    strSQL = strSQL + " SELECT A.本支店区分, 3 AS 処理ＣＤ,";
                    strSQL = strSQL + "'受注入力' AS 処理名称,";
                    strSQL = strSQL + "1 AS 区分,";
                    strSQL = strSQL + "'1' AS 事業所, ";
                    strSQL = strSQL + W_Syori_no + " AS 処理番,";
                    strSQL = strSQL + "0 AS 番号,";
                    strSQL = strSQL + "0 AS 仕入番,";
                    strSQL = strSQL + "0 AS 入金番, ";
                    strSQL = strSQL + "101 AS システム,";
                    strSQL = strSQL + " GETDATE() AS 日付, ";
                    strSQL = strSQL + "1 AS 更新,";
                    strSQL = strSQL + "'92' AS ＯＰ";
                    strSQL = strSQL + " FROM T会社基本 AS A;";
                    sqlDb.BeginTransaction();
                    sqlDb.ExecuteSql(strSQL, -1);
                    sqlDb.CommitTransaction();

                    BEF_CHU_NO = CUR_CHU_NO;
                    W_Syori_no = W_Syori_no + 1;
                }

            }

            // ------------------------------
            // T_ＷＳ番号　処理番号　更新
            // ------------------------------
            strSQL = " UPDATE TＷＳ番号 SET 処理番号 = " + (W_Syori_no - 1).ToString();
            strSQL = strSQL + " WHERE ＫＥＹ = 1;";
            sqlDb.BeginTransaction();
            sqlDb.ExecuteSql(strSQL, -1);
            sqlDb.CommitTransaction();

            /*
            strSQL = "INSERT INTO Tドラスタ受注ファイル ( 受注連番, 本支店区分, 処理コード, 入力区分, 処理番号 ";
            strSQL = strSQL + ",エラーフラグ, 受発注行数, 受注番号, 相手先注文番号, 自社受付番号, 処理日, 受注日, 納期, 処理区";
            strSQL = strSQL + ",得意先コード, 得名, 事業所コード, ランク, 部課コード, 担当者コード, 代理店コード, 代名, 納入先コード";
            strSQL = strSQL + ",納名, 請求先コード, 売上切捨区分, 売上税区分, 伝票摘要, 配送区分, 商品コード, 商名, 規格, 形式寸法";
            strSQL = strSQL + ",材質, 分類, 在庫管理区分, 品種コード, メーカーコード, 入数, 単位コード, 倉庫コード, ケース数, 受注数";
            strSQL = strSQL + ",指示累計数, 売上累計数, 受注単価, 受注金額, 原価単価, 原価金額, 粗利, 外内税区分, 消費税率, 新消費税率";
            strSQL = strSQL + ",新消費税適用, 明細摘要, 発注番号, 発注連番, 発注納期, 仕入先コード, 仕名, 仕入分類, 仕入事業所コード";
            strSQL = strSQL + ",仕入担当者コード, 仕入切捨区分, 仕入税区分, チェック, 完了フラグ, WS_ID, オペレーターコード";
            strSQL = strSQL + ",修正オペレーターコード, 受注行, 処理月日, 管理年月, 受注行番号, 定価, 納品掛率, 原価掛率, 発注有無区分";
            strSQL = strSQL + ",回答納期, 在庫数, 受注残数, 発注残数, 出荷指示発行フラグ, 商品注意事項, 発注摘要, 売上区分コード";
            strSQL = strSQL + ",システム区分, 回答コード, 回答名, 取引先コード, 社名, 店名, 部名, 発注区分, 請求月, ＥＯＳ区分";
            strSQL = strSQL + ",帳票区分, 発注単位, ＥＯＳ商品コード, ＥＯＳ商品名, ＥＯＳ規格, 表示価格, ＥＯＳ棚番, ＪＡＮコード";
            strSQL = strSQL + ",メーカー品番, 棚番, メーカー名, 店コード, 部コード, エラー区分, 更新ビット, 商品番号, 社コード";
            strSQL = strSQL + ",店コードＢ, 直送区分, 客注区分, 経費区分, 返品区分, 本部原価単価, 本部原価金額, 納入単価, 店舗売価, 取込日付 ";
            strSQL = strSQL + ",大分類コード, 店舗備考, 仕入先備考 ";
            strSQL = strSQL + " )";
            strSQL = strSQL + " SELECT A.受注連番, A.本支店区分, A.処理コード, A.入力区分, A.処理番号";
            strSQL = strSQL + ",A.エラーフラグ, A.受発注行数, A.受注番号, A.相手先注文番号, A.自社受付番号, A.処理日, A.受注日, A.納期, A.処理区";
            strSQL = strSQL + ",A.得意先コード, A.得名, A.事業所コード, A.ランク, A.部課コード, A.担当者コード, A.代理店コード, A.代名, A.納入先コード";
            strSQL = strSQL + ",A.納名, A.請求先コード, A.売上切捨区分, A.売上税区分, A.伝票摘要, A.配送区分, A.商品コード, A.商名, A.規格, A.形式寸法";
            strSQL = strSQL + ",A.材質, A.分類, A.在庫管理区分, A.品種コード, A.メーカーコード, A.入数, A.単位コード, A.倉庫コード, A.ケース数, A.受注数";
            strSQL = strSQL + ",A.指示累計数, A.売上累計数, A.受注単価, A.受注金額, A.原価単価, A.原価金額, A.粗利, A.外内税区分, A.消費税率, A.新消費税率";
            strSQL = strSQL + ",A.新消費税適用, A.明細摘要, A.発注番号, A.発注連番, A.発注納期, A.仕入先コード , A.仕名, A.仕入分類, A.仕入事業所コード";
            strSQL = strSQL + ",A.仕入担当者コード, A.仕入切捨区分, A.仕入税区分, A.チェック, A.完了フラグ, A.WS_ID, A.オペレーターコード";
            strSQL = strSQL + ",A.修正オペレーターコード, A.受注行, A.処理月日, A.管理年月, A.受注行番号, A.定価, A.納品掛率, A.原価掛率, A.発注有無区分";
            strSQL = strSQL + ",A.回答納期, A.在庫数, A.受注残数, A.発注残数, A.出荷指示発行フラグ, A.商品注意事項, A.発注摘要, A.売上区分コード";
            strSQL = strSQL + ",A.システム区分, A.回答コード , A.回答名, A.取引先コード, A.社名, A.店名, A.部名, A.発注区分, A.請求月, A.EOS区分";
            strSQL = strSQL + ",A.帳票区分, A.発注単位, A.EOS商品コード, A.EOS商品名, A.EOS規格, A.表示価格, A.EOS棚番, A.JANコード";
            strSQL = strSQL + ",A.メーカー品番, A.棚番, A.メーカー名, A.店コード, A.部コード, A.エラー区分, A.更新ビット, A.商品番号, A.社コード";
            strSQL = strSQL + ",A.店コードB, A.直送区分, A.客注区分, A.経費区分, A.返品区分, A.本部原価単価, A.本部原価金額, A.納入単価, A.店舗売価,GETDATE() AS 取込日付 ";
            strSQL = strSQL + ",A.大分類コード, A.店舗備考, A.仕入先備考 ";
            strSQL = strSQL + " FROM WTドラスタ受注ファイル AS A;";
            sqlDb.BeginTransaction();
            sqlDb.ExecuteSql(strSQL, -1);
            sqlDb.CommitTransaction();
            */

            // -------------------------------------
            // W_オンライン単価商品毎集計 Delete
            // -------------------------------------
            //strSQL = " DELETE W_オンライン単価商品毎集計.*";
            //strSQL = strSQL + " FROM W_オンライン単価商品毎集計;";
            //dbMain.Execute(strSQL);

            // -------------------------------
            // W_ドラスタ更新単価　Delete
            // -------------------------------
            //strSQL = " DELETE W_ドラスタ更新単価.*";
            //strSQL = strSQL + " FROM W_ドラスタ更新単価;";
            //dbMain.Execute(strSQL);

            // -------------------------------------
            // W_オンライン単価商品毎集計 Insert
            // -------------------------------------
            //strSQL = "INSERT INTO W_オンライン単価商品毎集計 ( ＥＯＳ商品コード, 商品コード, 本部原価単価, 納入単価, 店舗売価, ＪＡＮコード )";
            //strSQL = strSQL + " SELECT A.ＥＯＳ商品コード, Min(A.商品コード) AS 商品コードの最小 ";
            //strSQL = strSQL + ",Min(A.本部原価単価) AS 本部原価単価の最小, Min(A.納入単価) AS 納入単価の最小 ";
            //strSQL = strSQL + ",Min(A.店舗売価) AS 店舗売価の最小, Min(A.ＪＡＮコード) AS ＪＡＮコードの最小 ";
            //strSQL = strSQL + " FROM WT_ドラスタ受注ファイル AS A";
            //strSQL = strSQL + " GROUP BY A.ＥＯＳ商品コード";
            //strSQL = strSQL + " HAVING Min(A.商品コード) <> '999999999999'";
            //strSQL = strSQL + " ORDER BY A.ＥＯＳ商品コード;";
            //dbMain.Execute(strSQL);

            // -------------------------------
            // W_ドラスタ更新単価　Insert
            // -------------------------------
            //strSQL = "INSERT INTO W_ドラスタ更新単価 (得意先コード,商品コード,単価,掛率)";
            //strSQL = strSQL + " SELECT B.得意先コード,A.商品コード,A.単価,A.掛率";
            //strSQL = strSQL + " FROM ( SELECT C.商品コード,C.本部原価";
            //strSQL = strSQL + ",IIf(C.本部原価 Is Null,0,C.本部原価) AS 単価";
            //strSQL = strSQL + ",IIf(D.定価 Is Null OR D.定価 = 0,0,IIf(C.本部原価 Is Null,0,FIX(C.本部原価/D.定価*100000))/100000) AS 掛率";
            //strSQL = strSQL + " FROM T_商品変換マスタ AS C LEFT JOIN T_商品マスタ AS D";
            //strSQL = strSQL + " ON C.商品コード = D.商品コード";
            //strSQL = strSQL + " WHERE C.得意先コード = '450099' AND C.予備区分1 = 0) AS A,T_得意先マスタ AS B";
            //strSQL = strSQL + " WHERE B.得分類Ｂコード = '1'";
            //dbMain.Execute(strSQL);

            // -------------------------------
            // T_商品変換マスタの更新 Update
            // -------------------------------
            //strSQL = "UPDATE T_商品変換マスタ AS A";
            //strSQL = strSQL + " SET A.予備区分1 = 1  WHERE A.得意先コード = '450099' AND A.予備区分1 = 0";
            //dbMain.Execute(strSQL);

            sqlDb.Disconnect();

            return 0;

        }
    
    }
}
