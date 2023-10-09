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
using db_test.monotaRO受注取込DataSetTableAdapters;
using db_test.SPEEDDB管理DataSetTableAdapters;
using db_test.SPEEDDBVIEWDataSetTableAdapters;

namespace db_test
{
    public partial class monotaRO受注取込Form : Form
    {
        private vw_ShohinTableAdapter vw_ShohintableAdapter;
        private string iniFolder = null;
        private Encoding SJIS = Encoding.GetEncoding("Shift-JIS");
        DataTable table = new DataTable("BULK");
        DialogClass dialogClass = new DialogClass();
        string fileFilter = "CSVファイル(*.csv;*.txt)|*.csv;*.txt|すべてのファイル(*.*)|*.*";
        string importFolder = null;
        string importFile = null;
        string[] importFileArray = null;
        bool fireFLG;
        int fileCount;
        private WT_monotaRO_ドラスタ発注番号TableAdapter WT_monotaRO_ドラスタ発注番号tableAdapter = new WT_monotaRO_ドラスタ発注番号TableAdapter();
        private WT_MonotaRO受注ファイルTableAdapter WT_MonotaRO受注ファイルtableAdapter = new WT_MonotaRO受注ファイルTableAdapter();
        private WTドラスタ受注ファイルTableAdapter WTドラスタ受注ファイルtableAdapter = new WTドラスタ受注ファイルTableAdapter();

        public monotaRO受注取込Form()
        {
            InitializeComponent();
        }

        private void monotaRO受注取込Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new monotaRO受注取込Template();
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

            gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);

            // GcMultiRowがフォーカスを失ったときに
            // セルのCellStyle.SelectionBackColorとCellStyle.SelectionForeColorを表示しない場合はtrue。
            // それ以外の場合はfalse。
            gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down | Keys.Control);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up | Keys.Control);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left | Keys.Control);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right | Keys.Control);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveRight, Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);

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

            iniFolder = Properties.Settings.Default.monotaRO受注取込フォルダ;
            gcMultiRow1.ColumnHeaders[0].Cells["受注フォルダtextBoxCell"].Value = iniFolder;
            gcMultiRow1.ColumnHeaders[0].Cells["更新buttonCell"].Enabled = false;
            
            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注フォルダtextBoxCell");

        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "受注フォルダtextBoxCell":
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
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    break;
                case "フォルダオープンbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注フォルダtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注フォルダtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取込buttonCell");
                    }
                    break;
                case "取込buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "更新buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注フォルダtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "更新buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注フォルダtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "更新buttonCell");
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
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    break;
                case "終了buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注フォルダtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注フォルダtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    break;
                case "プレビューbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "更新buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "更新buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                    }
                    break;
                case "印刷buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    break;
            }
        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            int ret = 0;
            int errflg = 0;
            switch (e.CellName)
            {
                case "取込buttonCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["受注フォルダtextBoxCell"].Value == null)
                    {
                        MessageBox.Show("フォルダを入力してください", "monotaRO受注取込");
                        break;
                    }
                    importFolder = gcMultiRow1.ColumnHeaders[0].Cells["受注フォルダtextBoxCell"].Value.ToString();
                    if (Utility.Right(importFolder,1).Equals("/"))
                    {
                        importFolder += "/"; 
                    }
                    if (!Directory.Exists(importFolder))
                    {
                        MessageBox.Show("'" + importFolder + "'は存在しません。", "monotaRO受注取込");
                        break;
                    }
                    if (!Directory.Exists(importFolder+"backup"))
                    {
                        Directory.CreateDirectory(@importFolder + "backup");
                    }

                    fileCount = Directory.GetFiles(importFolder, "*.csv", SearchOption.TopDirectoryOnly).Length;
                    importFileArray = new string[fileCount];

                    // カレントディレクトリを変更
                    System.Environment.CurrentDirectory = @importFolder;

                    int i=0;
                    foreach (string file in Directory.GetFiles(".", "*.csv"))
                    {
                        importFileArray[i]=importFolder+file;
                        i++;
                    }

                    if (importFileArray==null)
                    {
                        MessageBox.Show("ファイルは存在しません。", "monotaRO受注取込");
                        break;
                    }

                    SqlDb db = new SqlDb();
                    db.Connect();
                    db.BeginTransaction();
                    db.ExecuteSql("delete from WTドラスタ受注ファイル", -1);
                    db.ExecuteSql("delete from WT_MonotaRO受注ファイル", -1);
                    db.ExecuteSql("delete from WT_monotaRO_ドラスタ発注番号", -1);
                    db.ExecuteSql("delete from WTドラスタ受注残カウント", -1);
                    db.CommitTransaction();
                    db.Disconnect();

                    foreach (string file in importFileArray)
                    {
                        ret = readCsv(file);
                        if (!ret.Equals(0))
                        {
                            errflg = 1;
                            File.Move(@file, @file.Replace(".csv",".err"));
                            //break;
                        }
                        else
                        {
                            File.Move(@file, Path.GetDirectoryName(@file) + "/backup/" + Path.GetFileName(@file));
                        }
                        //File.Delete(@Path.ChangeExtension(file, "txt"));
                    }
                    if (errflg == 0)
                    {
                        MessageBox.Show("CSVの取り込みに成功しました", "monotaRO受注取込");
                        gcMultiRow1.ColumnHeaders[0].Cells["更新buttonCell"].Enabled = true;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "更新buttonCell");
                    }
                    else
                    {
                        MessageBox.Show("CSVの取り込みに失敗しました", "monotaRO受注取込");
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込buttonCell");
                        DataTable dt = WT_MonotaRO受注ファイルtableAdapter.GetDataByMonotaRO受注取込エラーリスト();
                        プレビューForm プレビューform = new プレビューForm();
                        プレビューform.dataTable = dt;
                        プレビューform.rptName = "monotaRO受注取込エラーリストCrystalReport";
                        プレビューform.Show();
                    }
                    break;

                case "更新buttonCell":
                    ret = Jyuchu_Kosin();
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
                        MessageBox.Show("受注更新に成功しました", "monotaRO受注取込");
                    }
                    else
                    {
                        MessageBox.Show("受注更新に失敗しました", "monotaRO受注取込");
                    }
                    gcMultiRow1.ColumnHeaders[0].Cells["更新buttonCell"].Enabled = false;
                    break;

                case "フォルダオープンbuttonCell":

                    importFolder = dialogClass.OpenFolderByDialog(iniFolder, "");
                    //importFile = dialogClass.OpenFileByDialog(iniFolder, fileFilter);
                    gcMultiRow1.ColumnHeaders[0].Cells["受注フォルダtextBoxCell"].Value = importFolder;
                    break;

                case "プレビューbuttonCell":

                    DataTable dt2 = WT_MonotaRO受注ファイルtableAdapter.GetDataByMonotaRO受注取込チェックリスト();

                    if (dt2.Rows.Count > 0)
                    {
                        プレビューForm プレビューform = new プレビューForm();

                        プレビューform.dataTable = dt2;
                        プレビューform.rptName = "monotaRO受注取込チェックリストCrystalReport";
                        プレビューform.Show();
                    }
                    else
                    {
                        MessageBox.Show("データがありません", "monotaRO受注取込チェックリストCrystalReport");
                    }
                    break;

                case "終了buttonCell":
                    this.Hide();
                    break;
            }
        }

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int ret = 0;
            int errflg = 0;

            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "取込buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            if (gcMultiRow1.ColumnHeaders[0].Cells["受注フォルダtextBoxCell"].Value == null)
                            {
                                MessageBox.Show("フォルダを入力してください", "monotaRO受注取込");
                                break;
                            }
                            importFolder = gcMultiRow1.ColumnHeaders[0].Cells["受注フォルダtextBoxCell"].Value.ToString();
                            if (Utility.Right(importFolder,1).Equals("/"))
                            {
                                importFolder += "/"; 
                            }
                            if (!Directory.Exists(importFolder))
                            {
                                MessageBox.Show("'" + importFolder + "'は存在しません。", "monotaRO受注取込");
                                break;
                            }

                            if (!Directory.Exists(importFolder + "backup"))
                            {
                                Directory.CreateDirectory(@importFolder + "backup");
                            }

                            fileCount = Directory.GetFiles(importFolder, "*.csv", SearchOption.TopDirectoryOnly).Length;
                            importFileArray = new string[fileCount];

                            // カレントディレクトリを変更
                            System.Environment.CurrentDirectory = @importFolder;

                            int i=0;
                            foreach (string file in Directory.GetFiles(".", "*.csv"))
                            {
                                importFileArray[i]=importFolder+file;
                                i++;
                            }

                            if (importFileArray==null)
                            {
                                MessageBox.Show("ファイルは存在しません。", "monotaRO受注取込");
                                break;
                            }

                            SqlDb db = new SqlDb();
                            db.Connect();
                            db.BeginTransaction();
                            db.ExecuteSql("delete from WTドラスタ受注ファイル", -1);
                            db.ExecuteSql("delete from WT_MonotaRO受注ファイル", -1);
                            db.ExecuteSql("delete from WT_monotaRO_ドラスタ発注番号", -1);
                            db.ExecuteSql("delete from WTドラスタ受注残カウント", -1);
                            db.CommitTransaction();
                            db.Disconnect();

                            foreach (string file in importFileArray)
                            {
                                ret = readCsv(file);
                                if (!ret.Equals(0))
                                {
                                    errflg = 1;
                                    File.Move(@file, @file.Replace(".csv", ".err"));
                                    //break;
                                }
                                else
                                {
                                    File.Move(@file, Path.GetDirectoryName(@file) + "/backup/" + Path.GetFileName(@file));
                                }
                                File.Delete(@Path.ChangeExtension(file, "txt"));
                            }
                            if (errflg == 0)
                            {
                                MessageBox.Show("CSVの取り込みに成功しました", "monotaRO受注取込");
                                gcMultiRow1.ColumnHeaders[0].Cells["更新buttonCell"].Enabled = true;
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "更新buttonCell");
                            }
                            else
                            {
                                MessageBox.Show("CSVの取り込みに失敗しました", "monotaRO受注取込");
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込buttonCell");
                                DataTable dt = WT_MonotaRO受注ファイルtableAdapter.GetDataByMonotaRO受注取込エラーリスト();
                                プレビューForm プレビューform = new プレビューForm();
                                プレビューform.dataTable = dt;
                                プレビューform.rptName = "monotaRO受注取込エラーリストCrystalReport";
                                プレビューform.Show();
                            }
                            break;
                    }
                    break;
                case "更新buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            ret = Jyuchu_Kosin();
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
                                MessageBox.Show("受注更新に成功しました", "monotaRO受注取込");
                            }
                            else
                            {
                                MessageBox.Show("受注更新に失敗しました", "monotaRO受注取込");
                            }
                            gcMultiRow1.ColumnHeaders[0].Cells["更新buttonCell"].Enabled = false;
                            break;
                    }
                    break;
                case "フォルダオープンbuttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            importFolder = dialogClass.OpenFolderByDialog("", "");
                            //importFile = dialogClass.OpenFileByDialog(iniFolder, fileFilter);
                            gcMultiRow1.ColumnHeaders[0].Cells["受注フォルダtextBoxCell"].Value = importFolder;
                            break;
                    }
                    break;
                case "受注フォルダtextBoxCell":
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

        private int readCsv(string fileName)
        {
            int i;
            int icnt;
            string TextLine;
            string Fl_name;
            string sql;
            int FLG;
            int ret = 0;
            DataTable table = null;
    
            string kbn;       //レコード区分
            //ヘッダー
            object H2;        //帳票区分
            object H3;        //伝票番号
            object H5;        //店コード
            object H6;        //部門
            object H56;       //店コード+部門
            object H7;        //発注日
            object H8;        //店納品日
            object H9;        //取引先コード
            object H11;       //社名
            object H12;       //店名
            object H13;       //部名
            object H14;       //発注区分
            object H15;       //請求月
            object H17;       //ＥＯＳ区分
            object H18;       //社コード
            object H19;       //店コードＢ
            object H20;       //社名Ｂ
            object H21;       //直送区分
            object H22;       //客注区分
            object H23;       //経費区分
            object H24;       //返品区分
    
            //ボディー
            object B3;       //伝票行
            object B4;       //商品コード
            object B5=null;       //入数
            object B6;       //C/S数
            object B7;       //発注単位
            object B8;       //数量
            object B9;       //原価単価
            object B10;      //原価金額
            object B11;      //商品名
            object B12;      //規格
            object B13;      //表示価格
            object B14;      //棚番
            object B17;      //ＪＡＮコード
            object B18;      //商品番号
            object B19;      //本部原価単価
            object B20;      //本部原価金額
            object B21;      //納入単価
            object B22;      //店舗売価
   
            //変数
            object DMY;
            string TOK_NM;        //得意先名
            string TAN_CD;        //担当者コード
            int URI_KBN;      //売上切捨区分
            int UZEI_KBN;     //売上税区分
            string BUK_CD;        //部課コード
            string SYO_CD;        //商品コード
            string SYO_NM;        //商品名
            string KIKAKU;        //規格
            string ZAI_KBN;      //在庫管理区分
            string HIN_CD;        //品種コード
            string MEK_CD;        //メーカーコード
            decimal GENKA;       //原価単価
            int TNI_CD;       //単位コード
            decimal GK_RITU;       //代表原価掛率
            decimal RITU;          //掛率
            decimal SU_KBN;      //外内税区分
            decimal SYO_RITU;    //消費税率
            decimal NSYO_RITU;   //新消費税率
            string NSYO_TEKI;    //新消費税適用
            decimal TEIKA=0;       //定価
            string SEIKYU_CD;    //請求先コード
    
            decimal ZAIKO=0;       //在庫数 04/01/29
            decimal JUCHUZAN=0;       //受注残数 04/01/29
            decimal HACHUZAN=0;       //発注残数 04/01/29
    
            object MAKER_HIN;    //メーカー品番
            object TANA_BAN;     //棚番
            object MAKER_NM;     //メーカー名

            string Err_FlgA;
            string Err_FlgB;
            string Err_FlgC;
            string Err_FlgZ;
            string Err_FlgZ_JNO;
            string Err_FlgZ_JGYONO;
    
            int idif;
            string ERR_SYO_CD;        //エラー商品コード
            string[] X;
            string T2;
            string buf;
            int cnt;
            long jrenban=0;
            string fname;
            object drano;

            int gyocnt;

//            SqlBulkCopy bulkCopy;
            bool firstLine = false;
            string connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            //DataSet WTドラスタ受注ファイルdataSet = new WTドラスタ受注ファイルDataSet();
            //DataTable WTドラスタ受注ファイルdataTable = new DataTable();
            //WTドラスタ受注ファイルdataTable = WTドラスタ受注ファイルdataSet.Tables["WTドラスタ受注ファイル"];
            DataRow newRow;

            DataSet monotaRO受注取込dataSet = new monotaRO受注取込DataSet();
            DataTable WTドラスタ受注ファイルdataTable = monotaRO受注取込dataSet.Tables["WTドラスタ受注ファイル"];
            DataTable WT_MonotaRO受注ファイルdataTable = monotaRO受注取込dataSet.Tables["WT_MonotaRO受注ファイル"];
            DataTable WT_monotaRO_ドラスタ発注番号dataTable = monotaRO受注取込dataSet.Tables["WT_monotaRO_ドラスタ発注番号"];
            DataTable WTドラスタ受注残カウントdataTable = monotaRO受注取込dataSet.Tables["WTドラスタ受注残カウント"];
            
            DataRow WTドラスタ受注ファイルdataRow;
            DataRow WT_MonotaRO受注ファイルdataRow;
            DataRow WT_monotaRO_ドラスタ発注番号dataRow;
            DataRow WTドラスタ受注残カウントdataRow;

            fname = LF2CRLF(fileName);
            string fn = System.IO.Path.GetFileName(fileName);
            string[] ary = fn.Replace(".csv","").Split('-');

            WT_monotaRO_ドラスタ発注番号dataRow = WT_monotaRO_ドラスタ発注番号dataTable.NewRow();
            WT_monotaRO_ドラスタ発注番号dataRow["monotaRO発注番号"] = ary[0];
            WT_monotaRO_ドラスタ発注番号dataRow["連番"] = ary[1];
            WT_monotaRO_ドラスタ発注番号dataRow["時間"] = ary[2];
            WT_monotaRO_ドラスタ発注番号dataRow["ファイル名"] = fn;

            WT_monotaRO_ドラスタ発注番号dataTable.Rows.Add(WT_monotaRO_ドラスタ発注番号dataRow);

            WT_monotaRO_ドラスタ発注番号tableAdapter.Update(WT_monotaRO_ドラスタ発注番号dataRow);

            string line = "";
            using (StreamReader sr = new StreamReader(fname, SJIS))
            {

                drano = Utility.GetIdent("WT_monotaRO_ドラスタ発注番号");
                gyocnt = 1;
                firstLine = true;

                while ((line = sr.ReadLine()) != null)
                {
                    // 最初の行を見出しとして無視する
                    if (firstLine == true)
                    {
                        firstLine = false;
                        continue;
                    }

                    if ( gyocnt==7)
                    {
                        gyocnt = 1;

                        fn = System.IO.Path.GetFileName(@fileName);
                        ary = fn.Replace(".csv", "").Split('-');

                        WT_monotaRO_ドラスタ発注番号dataRow = WT_monotaRO_ドラスタ発注番号dataTable.NewRow();
                        WT_monotaRO_ドラスタ発注番号dataRow["monotaRO発注番号"] = ary[0];
                        WT_monotaRO_ドラスタ発注番号dataRow["連番"] = ary[1];
                        WT_monotaRO_ドラスタ発注番号dataRow["時間"] = ary[2];
                        WT_monotaRO_ドラスタ発注番号dataRow["ファイル名"] = fn;

                        WT_monotaRO_ドラスタ発注番号dataTable.Rows.Add(WT_monotaRO_ドラスタ発注番号dataRow);

                        WT_monotaRO_ドラスタ発注番号tableAdapter.Update(WT_monotaRO_ドラスタ発注番号dataRow);
                    
                        drano = Utility.GetIdent("WT_monotaRO_ドラスタ発注番号");
                    }

                    X = line.Split(',');

                    Err_FlgA = null;
                    //< ヘッダー >=============================================================================
                    H2 = Utility.Z_Set(null);             //Z_Set((Mid$(TextLine, 2, 2)))  //帳票区分
                    H3 = Utility.S_Set(null);
                    //店コード
                    H5 = DBNull.Value;
                    //部門
                    H6 = DBNull.Value;                   //CStr(Mid$(TextLine, 21, 1))     //2012/1 h.yamamoto chg
                    // 得意先コード("20"+納入先倉庫コード)
                    if (Utility.Z_Set(X[4]) == 1)
                    {
                        H56 = "209" + "0" + Utility.Right("00" + System.Convert.ToString(X[8]), 2);
                    }
                    else if (Utility.Z_Set(X[4]) == 3)
                    {
                        H56 = "20" + "00" + Utility.Right("00" + System.Convert.ToString(X[8]), 2);
                    }
                    else if (Utility.Z_Set(X[4]) == 2)
                    {
                        H56 = "209999";
                    }
                    else
                    {
                        Err_FlgA = "A";  // 品種種別が未設定です
                        H56 = "20" + Utility.Right("00000" + System.Convert.ToString(X[8]), 5);
                    }
                    /*
                    table = new DataTable();
                    sql = "SELECT A.業態コード";
                    sql = sql + " FROM T得意先マスタ AS A ";
                    sql = sql + " WHERE A.得意先コード = '" + H56 + "' ";

                    table = Utility.GetComboBoxData(sql);

                    if (table.Rows.Count > 0)
                    {
                        H56 = table.Rows[0]["業態コード"];
                    }
                    else
                    {
                        Err_FlgA = "A";
                        H56 = null;
                    }
                    */
                    
                    //発注日
                    // 店納品日
                    var now = System.DateTime.Now;
                    // yyyymmdd形式の文字列に変換
                    DMY = now.ToString("yyyy/MM/dd"); 
                    H7 = Utility.S_Set(X[1]);
                    H8 = null;
                    if (DMY != "")
                    {
                        DateTime DT = DateTime.Parse(Utility.Left(DMY.ToString(), 4) + "/" + Utility.Mid(DMY.ToString(), 6, 2) + "/" + Utility.Mid(DMY.ToString(), 9, 2));
                        // 仙台倉庫の場合、着日を翌々日とする
                        if (Utility.Right("00" + System.Convert.ToString(X[8]), 2) == "71" | Utility.Right("00" + System.Convert.ToString(X[8]), 2) == "72" | Utility.Right("00" + System.Convert.ToString(X[8]), 2) == "41")
                        {
                            H8 = DT.AddDays(2).ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            H8 = DT.AddDays(1).ToString("yyyy/MM/dd");
                        }
                    }

                    H9 = Utility.Z_Set(X[2]);     // 【ドラ】取引先コード => 【モノ】サプライやコード
                    H11 = Utility.S_Set(null);    // 【ドラ】社名 => 【モノ】該当なし
                    H12 = Utility.S_Set(null);    // 【ドラ】店名 => 【モノ】該当なし
                    H13 = Utility.S_Set(null);    // 【ドラ】部名 => 【モノ】該当なし
                    H14 = Utility.Z_Set(null);    // 【ドラ】発注区分 => 【モノ】該当なし
                    H15 = DBNull.Value;           // 【ドラ】請求月 => 【モノ】該当なし
                                          // H17 = Z_Set(X(5))   '【ドラ】ＥＯＳ区分 => 【モノ】該当なし
                    H17 = Utility.Z_Set(null);
                    // H18 = Z_Set(X(11))  '【ドラ】社コード => 【モノ】該当なし
                    H18 = Utility.Z_Set(null);
                    H19 = H5;             // 店コードＢ
                    H20 = H11;            // 社名Ｂ
                    H21 = Utility.Z_Set(X[4]);    // 【ドラ】直送区分 => 【モノ】品種種別
                                          // H22 = Z_Set(X(6))   '【ドラ】客注区分 => 【モノ】該当なし
                    H22 = Utility.Z_Set(null);
                    H23 = Utility.Z_Set(0);       // 【ドラ】経費区分 => 【モノ】該当なし
                    H24 = Utility.Z_Set(null);    // 【ドラ】返品区分 => 【モノ】該当なし
                    Err_FlgB = null;
                    Err_FlgC = null;

                    // ボディー -------------------------------------------------------
                    // B3 = Utility.Z_Set(X(2))    '【ドラ】伝票行 => 【モノ】発注明細ID
                    B3 = Utility.Z_Set(X[16]);
                    // B4 = Utility.S_Set(X(14))   '【ドラ】商品コード => 【モノ】monotaRO品番
                    // B4 = Utility.S_Set(X(19))   '【ドラ】商品コード => 【モノ】monotaRO品番
                    B4 = Utility.S_Set(X[20]);    // 【ドラ】商品コード => 【モノ】貴社管理番号
                    B5 = Utility.Z_Set(X[26]);    // 【ドラ】該当なし => 【モノ】入数
                    B6 = Utility.Z_Set(null);     // 【ドラ】該当なし => 【モノ】該当なし C/S数
                    B7 = Utility.S_Set(null);     // 【ドラ】該当なし => 【モノ】該当なし 発注単位
                                          // B8 = Z_Set(X(20))   '【ドラ】数量 => 【モノ】数量
                    B8 = Utility.Z_Set(X[29]);
                    // B9 = Utility.Z_Set(X(17))   '【ドラ】原価単価 => 【モノ】単価
                    B9 = Utility.Z_Set(X[27]);
                    B10 = Convert.ToInt32(B8) * Convert.ToInt32(B9);   // 【ドラ】原価金額 => 【モノ】金額
                                          // B11 = Left(S_Set(X(15)), 20)   '【ドラ】商品名 =>【モノ】品名
                    B11 = Utility.Left(Utility.S_Set(X[22]), 2);
                    // B12 = Left(S_Set(X(16)), 20)   '【ドラ】規格 =>【モノ】OPコード
                    B12 = Utility.Left(Utility.S_Set(X[25]), 2);
                    // B13 = Utility.Z_Set(X(19))   '【ドラ】表示価格 =>【モノ】税抜金額
                    B13 = Utility.Z_Set(X[30]);
                    B14 = Utility.Z_Set(null);    // 【ドラ】該当なし => 【モノ】該当なし 棚番
                                          // B17 = Z_Set(X(14))  '【ドラ】ＪＡＮコード => 【モノ】ＪＡＮコード
                    B17 = Utility.S_Set(X[23]);
                    B18 = Utility.S_Set(null);    // 【ドラ】該当なし => 【モノ】該当なし 商品番号
                    B19 = B9;                             // 本部原価単価
                    B20 = B10;                            // 本部原価金額
                                                          // B21 = Z_Set(X(18))  '【ドラ】納入単価 =>【モノ】単価
                    B21 = Utility.Z_Set(X[27]);
                    // B22 = Utility.Z_Set(X(19))  '【ドラ】店舗売価 =>【モノ】税込金額
                    B22 = Utility.Z_Set(X[32]);

                    //発注明細ID重複チェック
                    sql = "SELECT T受注ファイル.受注番号, T受注ファイル.受注行番号 FROM T受注ファイル";
                    sql = sql + " INNER JOIN T_monotaRO受注ファイル";
                    sql = sql + " ON T受注ファイル.ドラスタ発注番号 = T_monotaRO受注ファイル.ドラスタ発注番号";
                    sql = sql + " AND T受注ファイル.受注行番号 = T_monotaRO受注ファイル.登録番号";
                    sql = sql + " WHERE T_monotaRO受注ファイル.発注明細ID = " + B3 + " ";

                    table = Utility.GetComboBoxData(sql);

                    Err_FlgZ = "false";
                    Err_FlgZ_JNO = "0";
                    Err_FlgZ_JGYONO = "0";

                    if (table.Rows.Count > 0)
                    {
                        Err_FlgZ = "true";
                        Err_FlgZ_JNO = table.Rows[0][0].ToString();
                        Err_FlgZ_JGYONO = table.Rows[0][1].ToString();
                    }

                    
                    
                    TOK_NM = "";    TAN_CD = "";    URI_KBN = 0;    UZEI_KBN = 0;   BUK_CD = "";
                    SEIKYU_CD = "";

                    sql = "SELECT A.得意先名,A.担当者コード,A.売上切捨区分,A.売上税区分,A.請求先コード,B.部課コード";
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
                        SEIKYU_CD = table.Rows[0]["請求先コード"].ToString();
                    }
                    else
                    {
                        Err_FlgA = "A";
                    }
                    
                    SYO_CD = "";    SYO_NM = "";    KIKAKU = "";    ZAI_KBN = "";    HIN_CD = "";    MEK_CD = "";
                    GENKA = 0;  TNI_CD = 0; GK_RITU = 0;    RITU = 0;   SU_KBN = 0; SYO_RITU = 0;   NSYO_RITU = 0;
                    NSYO_TEKI = "";   MAKER_HIN = ""; TANA_BAN = "";  MAKER_NM = ""; TEIKA = 0;
        
                    SYO_CD = Utility.Right("00000000000"+B4.ToString(),11);
                    
                    //商品変換マスタチェック
                    //sql = "SELECT A.商品コード FROM T商品変換マスタ AS A ";
                    //sql = sql + " WHERE A.得意先コード = '450099'";
                    //sql = sql + " AND A.先方商品コード = '" + B4 + "'";
                    //sql = sql + " WHERE A.得意先コード = '450099'";
                    //sql = sql + " AND A.先方商品コード = '" + B17 + "'";
            
                    //table = Utility.GetComboBoxData(sql);

                    //if (table.Rows.Count > 0)
                    //{
                    //    SYO_CD = table.Rows[0]["商品コード"].ToString();
                    //}else{
                    //    Err_FlgB = "B";
                    //    SYO_CD = B4.ToString();
                    //}                    
                    
                    //商品マスタチェック
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
                        SYO_CD = table.Rows[0]["商品コード"].ToString();
                        SYO_NM = table.Rows[0]["商品名"].ToString();
                        KIKAKU = table.Rows[0]["規格"].ToString();
                        //dataRow["在庫管理区分"] = shTable.Rows[0]["在庫管理区分"].ToString();
                        ZAI_KBN = table.Rows[0]["在庫管理区分"].ToString();
                        HIN_CD = Utility.S_Set(table.Rows[0]["品種コード"].ToString());
                        MEK_CD = Utility.S_Set(table.Rows[0]["メーカーコード"].ToString());
                        GENKA = Utility.Z_Set(table.Rows[0]["原価単価"]);
                        TNI_CD = Convert.ToInt32(Utility.Z_Set(table.Rows[0]["単位コード"]));
                        GK_RITU = Utility.Z_Set(table.Rows[0]["代表原価掛率"]);
                        if (Utility.Z_Set(table.Rows[0]["定価"]).Equals(0) )
                        {
                            RITU = 0;
                        }else{
                            RITU = Convert.ToDecimal((Convert.ToInt32(B9) * 100000 / Utility.Z_Set(table.Rows[0]["定価"]))) / 100000;
                        }
                        SU_KBN = Utility.Z_Set(table.Rows[0]["外内税区分"]);
                        SYO_RITU = Utility.Z_Set(table.Rows[0]["消費税率"]);
                        NSYO_RITU = Utility.Z_Set(table.Rows[0]["新消費税率"]);
                        NSYO_TEKI = Utility.Left( Utility.S_Set(table.Rows[0]["新消費税適用"].ToString()),10);
                        MAKER_HIN = Utility.S_Set(table.Rows[0]["メーカー品番"].ToString());
                        TANA_BAN = Utility.S_Set(table.Rows[0]["棚番"].ToString());
                        MAKER_NM = Utility.S_Set(table.Rows[0]["メーカー名"].ToString());
                        TEIKA = Utility.Z_Set(table.Rows[0]["定価"]);
                        ZAIKO = Utility.Z_Set(table.Rows[0]["現在在庫数"]);
                        JUCHUZAN = Utility.Z_Set(table.Rows[0]["受注残数"]);
                        HACHUZAN = Utility.Z_Set(table.Rows[0]["発注残数"]);
                        B5 = Utility.Z_Set(table.Rows[0]["入数"]);
                
                        //受注残数カウント
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
                            ZAI_KBN = table.Rows[0]["在庫管理区分"].ToString();
                            HIN_CD = Utility.S_Set(table.Rows[0]["品種コード"].ToString());
                            MEK_CD = Utility.S_Set(table.Rows[0]["メーカーコード"].ToString());
                            GENKA = Utility.Z_Set(table.Rows[0]["原価単価"]);
                            TNI_CD = Convert.ToInt32(Utility.Z_Set(table.Rows[0]["単位コード"]));
                            GK_RITU = Utility.Z_Set(table.Rows[0]["代表原価掛率"]);
                            if (Utility.Z_Set(table.Rows[0]["定価"]).Equals(0)) 
                            {
                                RITU = 0;
                            }else{
                                RITU = Convert.ToDecimal((Convert.ToInt32(B9) / Utility.Z_Set(table.Rows[0]["定価"]) * 100000)) / 100000;
                            }    
                            SU_KBN = Utility.Z_Set(table.Rows[0]["外内税区分"]);
                            SYO_RITU = Utility.Z_Set(table.Rows[0]["消費税率"]);
                            NSYO_RITU = Utility.Z_Set(table.Rows[0]["新消費税率"]);
                            NSYO_TEKI = Utility.S_Set(table.Rows[0]["新消費税適用"]);
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
                    
                    if (Utility.S_Set(X[42]) == " ")     //発注時、回答種別は空白
                    {
                        WTドラスタ受注ファイルdataRow = WTドラスタ受注ファイルdataTable.NewRow();

                        WTドラスタ受注ファイルdataRow["本支店区分"] = 1;  //Z_Set(Forms!F_会社基本!本支店区分)
                        WTドラスタ受注ファイルdataRow["処理コード"] = 3;
                        WTドラスタ受注ファイルdataRow["入力区分"] = 1;
                        WTドラスタ受注ファイルdataRow["処理番号"] = 0;    //管理番号より
                        WTドラスタ受注ファイルdataRow["エラーフラグ"] = Err_FlgZ;
                        WTドラスタ受注ファイルdataRow["理由コード"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["受発注行数"] = 0;
                        WTドラスタ受注ファイルdataRow["受注番号"] = 0;
                        //WTドラスタ受注ファイルdataRow["売上伝票番号"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["相手先注文番号"] = H3;
                        WTドラスタ受注ファイルdataRow["自社受付番号"] = null;
                        WTドラスタ受注ファイルdataRow["処理日"] = DateTime.Now;
                        WTドラスタ受注ファイルdataRow["受注日"] = H7;
                        //WTドラスタ受注ファイルdataRow["売上日"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["納入日"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["納期"] = H8;
                        WTドラスタ受注ファイルdataRow["処理区"] = 0;
                        //WTドラスタ受注ファイルdataRow["請求月区分コード"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["得意先コード"] = H56;
                        WTドラスタ受注ファイルdataRow["得名"] = Utility.S_Set(X[9]);
                        //WTドラスタ受注ファイルdataRow["得名"] = "ttt";
                        WTドラスタ受注ファイルdataRow["事業所コード"] = "1";
                        WTドラスタ受注ファイルdataRow["ランク"] = 0;
                        WTドラスタ受注ファイルdataRow["部課コード"] = BUK_CD;
                        if (Utility.S_Set(X[4]) == "1")   //商品種別が「取寄せ」の場合
                        {
                            WTドラスタ受注ファイルdataRow["担当者コード"] = 209;
                        }
                        else if (Utility.S_Set(X[4]) == "2")  //商品種別が「直送」の場合
                        {
                            WTドラスタ受注ファイルdataRow["担当者コード"] = 299;
                        }
                        else if (Utility.S_Set(X[4]) == "3") //商品種別が「在庫」の場合
                        {
                            WTドラスタ受注ファイルdataRow["担当者コード"] = 200;
                        }
                        else
                        {
                            WTドラスタ受注ファイルdataRow["担当者コード"] = TAN_CD;
                        }
                        WTドラスタ受注ファイルdataRow["代理店コード"] = null;
                        WTドラスタ受注ファイルdataRow["代名"] = null;
                        WTドラスタ受注ファイルdataRow["納入先コード"] = null;
                        WTドラスタ受注ファイルdataRow["納名"] = null;
                        WTドラスタ受注ファイルdataRow["請求先コード"] = null;
                        WTドラスタ受注ファイルdataRow["売上切捨区分"] = URI_KBN;
                        WTドラスタ受注ファイルdataRow["売上税区分"] = UZEI_KBN;
                        //直送の場合、納入先名称をセット
                        if (Utility.S_Set(X[4]) == "2")   //商品種別が「直送」の場合
                        {
                            WTドラスタ受注ファイルdataRow["伝票摘要"] = Utility.S_Set(X[9]);
                        }
                        else
                        {
                            WTドラスタ受注ファイルdataRow["伝票摘要"] = null;
                        }
                        WTドラスタ受注ファイルdataRow["配送区分"] = null;
                        WTドラスタ受注ファイルdataRow["商品コード"] = SYO_CD;
                        if (Utility.S_Set(X[4]) == "1")   //商品種別が「取寄せ」の場合
                        {
                            WTドラスタ受注ファイルdataRow["商名"] = SYO_NM;
                        }
                        else if (Utility.S_Set(X[4]) == "2") //商品種別が「直送」の場合
                        {
                            WTドラスタ受注ファイルdataRow["商名"] = B11;
                        }
                        else if (Utility.S_Set(X[4]) == "3") //商品種別が「在庫」の場合
                        {
                            WTドラスタ受注ファイルdataRow["商名"] = SYO_NM;
                        }
                        WTドラスタ受注ファイルdataRow["規格"] = KIKAKU;
                        WTドラスタ受注ファイルdataRow["形式寸法"] = null;
                        WTドラスタ受注ファイルdataRow["材質"] = null;
                        WTドラスタ受注ファイルdataRow["分類"] = 0;
                        WTドラスタ受注ファイルdataRow["在庫管理区分"] = ZAI_KBN;
                        WTドラスタ受注ファイルdataRow["在庫管理INDEX"] = 1;
                        WTドラスタ受注ファイルdataRow["品種コード"] = HIN_CD;
                        WTドラスタ受注ファイルdataRow["メーカーコード"] = MEK_CD;
                        WTドラスタ受注ファイルdataRow["入数"] = Convert.ToInt32(B5);
                        WTドラスタ受注ファイルdataRow["単位コード"] = TNI_CD;
                        WTドラスタ受注ファイルdataRow["倉庫コード"] = "1";
                        WTドラスタ受注ファイルdataRow["ケース数"] = B6;
                        WTドラスタ受注ファイルdataRow["受注数"] = B8;
                        WTドラスタ受注ファイルdataRow["指示累計数"] = 0;
                        WTドラスタ受注ファイルdataRow["売上累計数"] = 0;
                        WTドラスタ受注ファイルdataRow["受注単価"] = B9;
                        WTドラスタ受注ファイルdataRow["受注金額"] = B10;
                        //WTドラスタ受注ファイルdataRow["税抜売上金額"] = 0;
                        WTドラスタ受注ファイルdataRow["原価単価"] = GENKA;
                        WTドラスタ受注ファイルdataRow["原価金額"] = Convert.ToInt32(B8) * GENKA;
                        WTドラスタ受注ファイルdataRow["粗利"] = Convert.ToDecimal(B10) - Convert.ToDecimal((Convert.ToInt32(B8) * GENKA));
                        //WTドラスタ受注ファイルdataRow["消費税"] = 0;
                        //WTドラスタ受注ファイルdataRow["税抜仕入金額"] = 0;
                        WTドラスタ受注ファイルdataRow["外内税区分"] = SU_KBN;
                        WTドラスタ受注ファイルdataRow["消費税率"] = SYO_RITU;
                        WTドラスタ受注ファイルdataRow["新消費税率"] = NSYO_RITU;
                        WTドラスタ受注ファイルdataRow["新消費税適用"] = NSYO_TEKI;  //日付
                        WTドラスタ受注ファイルdataRow["明細摘要"] = Utility.Z_Set(X[0]);
                        WTドラスタ受注ファイルdataRow["発注番号"] = 0;
                        WTドラスタ受注ファイルdataRow["発注連番"] = 0;
                        WTドラスタ受注ファイルdataRow["発注納期"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["仕入伝票番号"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["仕入区分コード"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["仕入先コード"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["仕入システム区分"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["支払月区分コード"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["仕名"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["仕入分類"] = 0;
                        WTドラスタ受注ファイルdataRow["仕入事業所コード"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["仕入担当者コード"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["仕入切捨区分"] = 0;
                        WTドラスタ受注ファイルdataRow["仕入税区分"] = 0;
                        WTドラスタ受注ファイルdataRow["チェック"] = 0;
                        WTドラスタ受注ファイルdataRow["完了フラグ"] = 0;
                        WTドラスタ受注ファイルdataRow["完了INDEX"] = 0;
                        WTドラスタ受注ファイルdataRow["WS_ID"] = "04";
                        WTドラスタ受注ファイルdataRow["オペレーターコード"] = "93";
                        WTドラスタ受注ファイルdataRow["修正オペレーターコード"] = null;
                        WTドラスタ受注ファイルdataRow["受注更新フラグ"] = 0;
                        //WTドラスタ受注ファイルdataRow["得意先更新フラグ"] = 0;
                        //WTドラスタ受注ファイルdataRow["商品更新フラグ"] = 0;
                        //WTドラスタ受注ファイルdataRow["商品倉庫更新フラグ"] = 0;
                        //WTドラスタ受注ファイルdataRow["仕入先更新フラグ"] = 0;
                        WTドラスタ受注ファイルdataRow["受注行"] = gyocnt;
                        WTドラスタ受注ファイルdataRow["処理月日"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["管理年月"] = 0;
                        WTドラスタ受注ファイルdataRow["受注行番号"] = gyocnt;
                        WTドラスタ受注ファイルdataRow["定価"] = TEIKA;
                        WTドラスタ受注ファイルdataRow["納品掛率"] = RITU;
                        WTドラスタ受注ファイルdataRow["原価掛率"] = GK_RITU;
                        WTドラスタ受注ファイルdataRow["発注有無区分"] = 0;
                        WTドラスタ受注ファイルdataRow["回答納期"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["在庫数"] = ZAIKO;
                        WTドラスタ受注ファイルdataRow["受注残数"] = JUCHUZAN;
                        WTドラスタ受注ファイルdataRow["発注残数"] = HACHUZAN;
                        WTドラスタ受注ファイルdataRow["出荷指示発行フラグ"] = 0;
                        WTドラスタ受注ファイルdataRow["商品注意事項"] = null;
                        WTドラスタ受注ファイルdataRow["発注摘要"] = null;
                        WTドラスタ受注ファイルdataRow["売上区分コード"] = 1;
                        WTドラスタ受注ファイルdataRow["システム区分"] = 101;
                        WTドラスタ受注ファイルdataRow["処理区"] = 0;
                        //WTドラスタ受注ファイルdataRow["得意先"] = null;
                        //WTドラスタ受注ファイルdataRow["仕入先"] = null;
                        //WTドラスタ受注ファイルdataRow["商品"] = null;
                        //WTドラスタ受注ファイルdataRow["回答コード"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["回答名"] = null;
                        //WTドラスタ受注ファイルdataRow["前受注数"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["発注書発行フラグ"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["注文書発行フラグ"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["取引先コード"] = H9;
                        WTドラスタ受注ファイルdataRow["社名"] = H11;
                        WTドラスタ受注ファイルdataRow["店名"] = H12;
                        WTドラスタ受注ファイルdataRow["部名"] = H13;
                        WTドラスタ受注ファイルdataRow["発注区分"] = H14;
                        WTドラスタ受注ファイルdataRow["請求月"] = H15;
                        WTドラスタ受注ファイルdataRow["EOS区分"] = H17;
                        WTドラスタ受注ファイルdataRow["帳票区分"] = H2;
                        WTドラスタ受注ファイルdataRow["発注単位"] = B7;
                        WTドラスタ受注ファイルdataRow["EOS商品コード"] = SYO_CD;
                        WTドラスタ受注ファイルdataRow["EOS商品名"] = B11;
                        WTドラスタ受注ファイルdataRow["EOS規格"] = B12;
                        WTドラスタ受注ファイルdataRow["表示価格"] = B13;
                        WTドラスタ受注ファイルdataRow["EOS棚番"] = B14;
                        if (Utility.Z_Set(B17) > 0)
                        {
                            WTドラスタ受注ファイルdataRow["JANコード"] = Utility.Z_Set(B17);
                        }
                        WTドラスタ受注ファイルdataRow["メーカー品番"] = MAKER_HIN;
                        WTドラスタ受注ファイルdataRow["棚番"] = TANA_BAN;
                        WTドラスタ受注ファイルdataRow["メーカー名"] = MAKER_NM;
                        WTドラスタ受注ファイルdataRow["店コード"] = H5;
                        WTドラスタ受注ファイルdataRow["部コード"] = H6;
                        DMY = null;
                        if (Err_FlgA != null)
                        {
                            DMY = Err_FlgA;
                        }
                        if (Err_FlgB != null)
                        {
                            DMY = DMY + Err_FlgB;
                        }
                        if (Err_FlgC != null)
                        {
                            DMY = DMY + Err_FlgC;
                        }
                        WTドラスタ受注ファイルdataRow["エラー区分"] = DMY;
                        //WTドラスタ受注ファイルdataRow["更新ビット"] = DBNull.Value;
                        WTドラスタ受注ファイルdataRow["商品番号"] = B18;
                        WTドラスタ受注ファイルdataRow["社コード"] = H18;
                        WTドラスタ受注ファイルdataRow["店コードB"] = H19;
                        WTドラスタ受注ファイルdataRow["直送区分"] = H21;
                        WTドラスタ受注ファイルdataRow["客注区分"] = H22;
                        WTドラスタ受注ファイルdataRow["経費区分"] = H23;
                        WTドラスタ受注ファイルdataRow["返品区分"] = H24;
                        WTドラスタ受注ファイルdataRow["本部原価単価"] = B19;
                        WTドラスタ受注ファイルdataRow["本部原価金額"] = B20;
                        WTドラスタ受注ファイルdataRow["納入単価"] = B21;
                        WTドラスタ受注ファイルdataRow["店舗売価"] = B22;
                        WTドラスタ受注ファイルdataRow["大分類コード"] = Utility.S_Set(X[13]);
                        //WTドラスタ受注ファイルdataRow["大分類コード"] = 32;
                        //WTドラスタ受注ファイルdataRow["店舗備考"] = Utility.S_Set(X[23]);
                        //WTドラスタ受注ファイルdataRow["仕入先備考"] = DBNull.Value;
                        //WTドラスタ受注ファイルdataRow["単価更新フラグ"] = 0;
                        WTドラスタ受注ファイルdataRow["登録番号"] = gyocnt;
                        WTドラスタ受注ファイルdataRow["ドラスタ発注番号"] = drano;
                        WTドラスタ受注ファイルdataRow["客注番号"] = B3;
                        //WTドラスタ受注ファイルdataRow["発注数"] = 0;

                        WTドラスタ受注ファイルdataTable.Rows.Add(WTドラスタ受注ファイルdataRow);

                        WTドラスタ受注ファイルtableAdapter.Update(WTドラスタ受注ファイルdataRow);

                        //上で取得したオートナンバー型の「受注連番」
                        jrenban = long.Parse(Utility.GetIdent("WTドラスタ受注ファイル"));

                    }
                    else
                    {
                        jrenban = jrenban + 1;
                    }

                    //WT_monotaRO受注ファイル
                    WT_MonotaRO受注ファイルdataRow = WT_MonotaRO受注ファイルdataTable.NewRow();

                    WT_MonotaRO受注ファイルdataRow["受注連番"] = jrenban;
                    WT_MonotaRO受注ファイルdataRow["発注書番号"] = Utility.Z_Set(X[0]);
                    WT_MonotaRO受注ファイルdataRow["発注日時"] = H7;
                    WT_MonotaRO受注ファイルdataRow["サプライヤコード"] = X[2];
                    WT_MonotaRO受注ファイルdataRow["サプライヤ名"] = X[3];
                    WT_MonotaRO受注ファイルdataRow["商品種別"] = X[4];
                    WT_MonotaRO受注ファイルdataRow["代引回収"] = X[5];
                    if (Utility.Z_Set(X[6]) > 0)
                    {
                        WT_MonotaRO受注ファイルdataRow["代引回収金額"] = Utility.Z_Set(X[6]);
                    }
                    WT_MonotaRO受注ファイルdataRow["通貨"] = X[7];
                    WT_MonotaRO受注ファイルdataRow["納入先倉庫コード"] = X[8];
                    WT_MonotaRO受注ファイルdataRow["納入先名称"] = X[9];
                    WT_MonotaRO受注ファイルdataRow["納入先郵便番号"] = X[10];
                    WT_MonotaRO受注ファイルdataRow["納入先住所1"] = X[11];
                    WT_MonotaRO受注ファイルdataRow["納入先住所2"] = X[12];
                    WT_MonotaRO受注ファイルdataRow["納入先担当者"] = X[13];
                    WT_MonotaRO受注ファイルdataRow["納入先電話番号"] = X[14];
                    WT_MonotaRO受注ファイルdataRow["特記事項"] = X[15];
                    WT_MonotaRO受注ファイルdataRow["発注明細ID"] = X[16];
                    WT_MonotaRO受注ファイルdataRow["行区分"] = X[17];
                    WT_MonotaRO受注ファイルdataRow["取消状態"] = X[18];
                    WT_MonotaRO受注ファイルdataRow["MonotaRO品番"] = X[19];
                    WT_MonotaRO受注ファイルdataRow["貴社管理番号"] = X[20];
                    WT_MonotaRO受注ファイルdataRow["メーカー名"] = X[21];
                    WT_MonotaRO受注ファイルdataRow["品名"] = X[22];
                    WT_MonotaRO受注ファイルdataRow["JANコード"] = X[23];
                    WT_MonotaRO受注ファイルdataRow["メーカー品番"] = X[24];
                    WT_MonotaRO受注ファイルdataRow["OPコード"] = X[25];
                    WT_MonotaRO受注ファイルdataRow["入数"] = X[26];
                    WT_MonotaRO受注ファイルdataRow["単価"] = Utility.Z_Set(X[27]);
                    WT_MonotaRO受注ファイルdataRow["税区分"] = X[28];
                    WT_MonotaRO受注ファイルdataRow["数量"] = X[29];
                    WT_MonotaRO受注ファイルdataRow["税抜金額"] = X[30];
                    WT_MonotaRO受注ファイルdataRow["消費税額"] = X[31];
                    WT_MonotaRO受注ファイルdataRow["税込金額"] = X[32];
                    if (X[33] == "")
                    {
                        WT_MonotaRO受注ファイルdataRow["入荷締切日"] = null;
                    }
                    else
                    {
                        WT_MonotaRO受注ファイルdataRow["入荷締切日"] = X[33];
                    }
                    WT_MonotaRO受注ファイルdataRow["注意事項1"] = X[34];
                    WT_MonotaRO受注ファイルdataRow["注意事項2"] = X[35];
                    WT_MonotaRO受注ファイルdataRow["注意事項3"] = X[36];
                    WT_MonotaRO受注ファイルdataRow["注意事項4"] = X[37];
                    WT_MonotaRO受注ファイルdataRow["注意事項5"] = X[38];
                    WT_MonotaRO受注ファイルdataRow["発注明細特記"] = X[39];
                    WT_MonotaRO受注ファイルdataRow["入荷倉庫のロケ名"] = X[40];
                    WT_MonotaRO受注ファイルdataRow["WMSロケーションNo"] = X[41];
                    if (Utility.Z_Set(X[42]) > 0 )
                    {
                        WT_MonotaRO受注ファイルdataRow["回答種別"] = Utility.Z_Set(X[42]);
                    }
                    WT_MonotaRO受注ファイルdataRow["出荷予定日時"] = DBNull.Value;
                    WT_MonotaRO受注ファイルdataRow["出荷日時"] = DBNull.Value;
                    if (Utility.Z_Set(X[45]) > 0 )
                    {
                        WT_MonotaRO受注ファイルdataRow["出荷数量"] = Utility.Z_Set(X[45]);
                    }
                    WT_MonotaRO受注ファイルdataRow["到着予定日時"] = DBNull.Value;
                    WT_MonotaRO受注ファイルdataRow["到着日時"] = DBNull.Value;
                    WT_MonotaRO受注ファイルdataRow["便名"] = X[48];
                    if (Utility.Z_Set(X[49]) > 0 )
                    {
                        WT_MonotaRO受注ファイルdataRow["配送業者コード"] = Utility.Z_Set(X[49]);
                    }
                    WT_MonotaRO受注ファイルdataRow["送り状番号"] = X[50];
                    WT_MonotaRO受注ファイルdataRow["個口数"] = X[51];
                    if (Utility.Z_Set(X[52]) > 0) 
                    {
                        WT_MonotaRO受注ファイルdataRow["その他回答種別"] = Utility.Z_Set(X[52]);
                    }
                    WT_MonotaRO受注ファイルdataRow["その他回答備考"] = X[53];
                    WT_MonotaRO受注ファイルdataRow["本支店区分"] = 1;
                    WT_MonotaRO受注ファイルdataRow["処理コード"] = 3;
                    WT_MonotaRO受注ファイルdataRow["入力区分"] = 1;
                    WT_MonotaRO受注ファイルdataRow["処理番号"] = 0;
                    if (Err_FlgZ=="true")
                    {
                        WT_MonotaRO受注ファイルdataRow["エラーフラグ"] = 1;
                    }
                    else 
                    {
                        WT_MonotaRO受注ファイルdataRow["エラーフラグ"] = 0;
                    }
                    WT_MonotaRO受注ファイルdataRow["受発注行数"] = 0;
                    WT_MonotaRO受注ファイルdataRow["受注番号"] = 0;
                    WT_MonotaRO受注ファイルdataRow["相手先注文番号"] = H3;
                    WT_MonotaRO受注ファイルdataRow["自社受付番号"] = DBNull.Value;
                    WT_MonotaRO受注ファイルdataRow["処理日"] = DateTime.Now;
                    WT_MonotaRO受注ファイルdataRow["ファイル名"] = System.IO.Path.GetFileName(fileName);
                    WT_MonotaRO受注ファイルdataRow["登録番号"] = gyocnt;
                    WT_MonotaRO受注ファイルdataRow["ドラスタ発注番号"] = drano;
                    WT_MonotaRO受注ファイルdataRow["客注番号"] = 0;
                    WT_MonotaRO受注ファイルdataTable.Rows.Add(WT_MonotaRO受注ファイルdataRow);
                    WT_MonotaRO受注ファイルtableAdapter.Update(WT_MonotaRO受注ファイルdataRow);

                    gyocnt = gyocnt + 1;

                }
                    
                sr.Close();

            }



            /*
            string cst = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cst))
            {
                // フィールド名のマッピング
                foreach (var column in WTドラスタ受注ファイルdataTable.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());
                }
                bulkCopy.BulkCopyTimeout = 600; // in seconds
                bulkCopy.DestinationTableName = WTドラスタ受注ファイルdataTable.TableName; // テーブル名をSqlBulkCopyに教える
                bulkCopy.WriteToServer(WTドラスタ受注ファイルdataTable);

                foreach (var column in WT_MonotaRO受注ファイルdataTable.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());
                }
                bulkCopy.BulkCopyTimeout = 600; // in seconds
                bulkCopy.DestinationTableName = WT_MonotaRO受注ファイルdataTable.TableName; // テーブル名をSqlBulkCopyに教える
                bulkCopy.WriteToServer(WT_MonotaRO受注ファイルdataTable);

            }
            */

            //明細件数なしチェック
            string sql2 = "SELECT A.受注連番";
            sql2 = sql2 + " FROM WTドラスタ受注ファイル AS A";

            DataTable table2 = Utility.GetComboBoxData(sql2);

            if (table2.Rows.Count <= 0)
            {
                MessageBox.Show("明細がありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ret = -9;
            }

            //エラー件数カウント
            sql2 = "SELECT Count(A.エラー区分) AS CNT";
            sql2 = sql2 + " FROM WTドラスタ受注ファイル AS A";
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
                    //MessageBox.Show("取り込み時エラーが " + icnt + "件 あります.", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ret = 1;
                }
            }

            if (ret == 0)
            {
                //正常件数カウント
                sql2 = "SELECT Count(A.受注連番) AS CNT";
                sql2 = sql2 + " FROM WTドラスタ受注ファイル AS A";

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
            }

            //using (bulkCopy = new SqlBulkCopy(connectionString)) ;
            //{
            //    bulkCopy.BulkCopyTimeout = 600; // in seconds
            //    bulkCopy.DestinationTableName = "WTドラスタ受注ファイル";
            //    bulkCopy.WriteToServer(WTドラスタ受注ファイルdataTable);
            //}

            return ret;
        }

        public int Jyuchu_Kosin()
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
                if ( dataTable.Rows[0]["処理番号"] == null )
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
                if ( dataTable.Rows[0]["WS番号"] == null )
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
            BEF_CHU_NO = 0;

            strSQL = "SELECT ドラスタ発注番号,受注行番号 FROM WTドラスタ受注ファイル ORDER BY ドラスタ発注番号,受注行番号";

            dataTable = sqlDb.ExecuteSql(strSQL, -1);

            if (dataTable.Rows.Count != 0)
            {
                BEF_CHU_NO = Convert.ToInt64(dataTable.Rows[0]["ドラスタ発注番号"]);
                CHU_GYO = Convert.ToInt32(dataTable.Rows[0]["受注行番号"]);
            }
            else
            {
                strSQL = "INSERT INTO T_MonotaRO受注ファイル( ";
                strSQL = strSQL + "受注連番,発注書番号,発注日時,サプライヤコード,サプライヤ名 ";
                strSQL = strSQL + ",商品種別,代引回収,代引回収金額,通貨,納入先倉庫コード ";
                strSQL = strSQL + ",納入先名称,納入先郵便番号,納入先住所1,納入先住所2,納入先担当者 ";
                strSQL = strSQL + ",納入先電話番号,特記事項,発注明細ID,行区分,取消状態,MonotaRO品番 ";
                strSQL = strSQL + ",貴社管理番号,メーカー名,品名,JANコード,メーカー品番,OPコード ";
                strSQL = strSQL + ",入数,単価,税区分,数量,税抜金額,消費税額,税込金額,入荷締切日 ";
                strSQL = strSQL + ",注意事項1,注意事項2,注意事項3,注意事項4,注意事項5,発注明細特記 ";
                strSQL = strSQL + ",入荷倉庫のロケ名,WMSロケーションNo,回答種別,出荷予定日時 ";
                strSQL = strSQL + ",出荷日時,出荷数量,到着予定日時,到着日時,便名,配送業者コード ";
                strSQL = strSQL + ",送り状番号,個口数,その他回答種別,その他回答備考,本支店区分 ";
                strSQL = strSQL + ",処理コード,入力区分,処理番号,エラーフラグ,受発注行数,受注番号 ";
                strSQL = strSQL + ",相手先注文番号,自社受付番号,処理日,ファイル名,登録番号,ドラスタ発注番号,客注番号 ) ";
                strSQL = strSQL + " SELECT  ";
                strSQL = strSQL + " 受注連番,発注書番号,発注日時,サプライヤコード,サプライヤ名 ";
                strSQL = strSQL + ",商品種別,代引回収,代引回収金額,通貨,納入先倉庫コード ";
                strSQL = strSQL + ",納入先名称,納入先郵便番号,納入先住所1,納入先住所2,納入先担当者 ";
                strSQL = strSQL + ",納入先電話番号,特記事項,発注明細ID,行区分,取消状態,MonotaRO品番 ";
                strSQL = strSQL + ",貴社管理番号,メーカー名,品名,JANコード,メーカー品番,OPコード ";
                strSQL = strSQL + ",入数,単価,税区分,数量,税抜金額,消費税額,税込金額,入荷締切日 ";
                strSQL = strSQL + ",注意事項1,注意事項2,注意事項3,注意事項4,注意事項5,発注明細特記 ";
                strSQL = strSQL + ",入荷倉庫のロケ名,WMSロケーションNo,回答種別,出荷予定日時 ";
                strSQL = strSQL + ",出荷日時,出荷数量,到着予定日時,到着日時,便名,配送業者コード ";
                strSQL = strSQL + ",送り状番号,個口数,その他回答種別,その他回答備考,本支店区分 ";
                strSQL = strSQL + ",処理コード,入力区分,処理番号,エラーフラグ,受発注行数,受注番号 ";
                strSQL = strSQL + ",相手先注文番号,自社受付番号,処理日,ファイル名,登録番号,ドラスタ発注番号,客注番号 ";
                strSQL = strSQL + " FROM WT_MonotaRO受注ファイル;";

                sqlDb.BeginTransaction();
                sqlDb.ExecuteSql(strSQL, -1);
                sqlDb.CommitTransaction();

                return 0;

            }

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
                strSQL = strSQL + ", A.大分類コード ";
                strSQL = strSQL + ", A.店舗備考 ";
                strSQL = strSQL + ", A.仕入先備考 ";
                strSQL = strSQL + ", 1 ";     // 単価更新フラグ
                strSQL = strSQL + ", A.登録番号 ";
                strSQL = strSQL + ", A.ドラスタ発注番号 ";
                strSQL = strSQL + ", A.客注番号 ";
                strSQL = strSQL + " FROM WTドラスタ受注ファイル AS A";
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
                    CUR_CHU_NO = Convert.ToInt64(dataTable.Rows[i+1]["ドラスタ発注番号"]); ;
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

            strSQL = "SET IDENTITY_INSERT T_monotaRO_ドラスタ発注番号 ON";
            sqlDb.ExecuteSql(strSQL, -1);

            strSQL = "INSERT INTO T_monotaRO_ドラスタ発注番号( ";
            strSQL = strSQL + "monotaRO_ドラスタ発注番号,monotaRO発注番号,連番,時間,ファイル名 ) ";
            strSQL = strSQL + " SELECT  ";
            strSQL = strSQL + "monotaRO_ドラスタ発注番号,monotaRO発注番号,連番,時間,ファイル名 ";
            strSQL = strSQL + " FROM WT_monotaRO_ドラスタ発注番号;";
            sqlDb.BeginTransaction();
            sqlDb.ExecuteSql(strSQL, -1);
            sqlDb.CommitTransaction();

            strSQL = "INSERT INTO T_MonotaRO受注ファイル( ";
            strSQL = strSQL + "受注連番,発注書番号,発注日時,サプライヤコード,サプライヤ名 ";
            strSQL = strSQL + ",商品種別,代引回収,代引回収金額,通貨,納入先倉庫コード ";
            strSQL = strSQL + ",納入先名称,納入先郵便番号,納入先住所1,納入先住所2,納入先担当者 ";
            strSQL = strSQL + ",納入先電話番号,特記事項,発注明細ID,行区分,取消状態,MonotaRO品番 ";
            strSQL = strSQL + ",貴社管理番号,メーカー名,品名,JANコード,メーカー品番,OPコード ";
            strSQL = strSQL + ",入数,単価,税区分,数量,税抜金額,消費税額,税込金額,入荷締切日 ";
            strSQL = strSQL + ",注意事項1,注意事項2,注意事項3,注意事項4,注意事項5,発注明細特記 ";
            strSQL = strSQL + ",入荷倉庫のロケ名,WMSロケーションNo,回答種別,出荷予定日時 ";
            strSQL = strSQL + ",出荷日時,出荷数量,到着予定日時,到着日時,便名,配送業者コード ";
            strSQL = strSQL + ",送り状番号,個口数,その他回答種別,その他回答備考,本支店区分 ";
            strSQL = strSQL + ",処理コード,入力区分,処理番号,エラーフラグ,受発注行数,受注番号 ";
            strSQL = strSQL + ",相手先注文番号,自社受付番号,処理日,ファイル名,登録番号,ドラスタ発注番号,客注番号 ) ";
            strSQL = strSQL + " SELECT  ";
            strSQL = strSQL + " 受注連番,発注書番号,発注日時,サプライヤコード,サプライヤ名 ";
            strSQL = strSQL + ",商品種別,代引回収,代引回収金額,通貨,納入先倉庫コード ";
            strSQL = strSQL + ",納入先名称,納入先郵便番号,納入先住所1,納入先住所2,納入先担当者 ";
            strSQL = strSQL + ",納入先電話番号,特記事項,発注明細ID,行区分,取消状態,MonotaRO品番 ";
            strSQL = strSQL + ",貴社管理番号,メーカー名,品名,JANコード,メーカー品番,OPコード ";
            strSQL = strSQL + ",入数,単価,税区分,数量,税抜金額,消費税額,税込金額,入荷締切日 ";
            strSQL = strSQL + ",注意事項1,注意事項2,注意事項3,注意事項4,注意事項5,発注明細特記 ";
            strSQL = strSQL + ",入荷倉庫のロケ名,WMSロケーションNo,回答種別,出荷予定日時 ";
            strSQL = strSQL + ",出荷日時,出荷数量,到着予定日時,到着日時,便名,配送業者コード ";
            strSQL = strSQL + ",送り状番号,個口数,その他回答種別,その他回答備考,本支店区分 ";
            strSQL = strSQL + ",処理コード,入力区分,処理番号,エラーフラグ,受発注行数,受注番号 ";
            strSQL = strSQL + ",相手先注文番号,自社受付番号,処理日,ファイル名,登録番号,ドラスタ発注番号,客注番号 ";
            strSQL = strSQL + " FROM WT_MonotaRO受注ファイル;";
            sqlDb.BeginTransaction();
            sqlDb.ExecuteSql(strSQL, -1);
            sqlDb.CommitTransaction();

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

            return ret;

        }

        public string LF2CRLF(string infile)
        {

            string outfile;
            string line;

            outfile = infile.Replace(".csv", ".txt");

            // ファイルを開く
            StreamReader reader = new StreamReader(@infile, System.Text.Encoding.GetEncoding("shift_jis"));

            // ファイルを開く
            StreamWriter writer = new StreamWriter(@outfile, false, Encoding.GetEncoding("shift_jis"));

            // ファイルの終了まで読み取る
            while ((line = reader.ReadLine()) != null)
            {

                // テキストを書き込む
                writer.WriteLine(line.Replace("\t", ",").Replace("\"",""));

            }

            // ファイルを閉じる
            reader.Close();
            writer.Close();

            return outfile;
            
            //FileStream fs1 = new FileStream(infile, FileMode.Open);
            //byte[] data = new byte[fs1.Length];
            //fs1.Read(data, 0, data.Length);

            //fs1.Close();

            //outfile = infile.Replace(".csv", ".txt");

            //FileStream fs2 = new FileStream(outfile, FileMode.Create);
            //BinaryWriter bw = new BinaryWriter(fs2);
            /*
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 10)
                {
                    bw.Write(13);
                    bw.Write(10);
                }
                else if (data[i] == 9)
                {
                    bw.Write(44);
                }
                else
                {
                    bw.Write(data[i]);
                }
            }

            bw.Close();
            fs2.Close();
            */
//            return outfile;
            //return "nn";
        }    

    }
}
