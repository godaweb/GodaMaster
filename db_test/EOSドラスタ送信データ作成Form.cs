using System;
using System.IO;
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
using db_test.EOSドラスタ送信データ作成DataSetTableAdapters;

namespace db_test
{
    public partial class EOSドラスタ送信データ作成Form : Form
    {
        DataTable dataTable;
        private string receiveDataToksaki = "";
        得意先検索Form fsToksaki;
        private string iniFile = null;
        private Encoding SJIS = Encoding.GetEncoding("Shift-JIS");
        DialogClass dialogClass = new DialogClass();
        string fileFilter = "CSVファイル(*.csv;*.txt)|*.csv;*.txt|すべてのファイル(*.*)|*.*";
        string importFile = null;
        bool fireFLG;
        private WTEOSドラスタ送信データ作成チェックリストTableAdapter wtEOSドラスタ送信データ作成チェックリストTableAdapter = new WTEOSドラスタ送信データ作成チェックリストTableAdapter();

        public EOSドラスタ送信データ作成Form()
        {
            InitializeComponent();
            fsToksaki = new 得意先検索Form();
            fsToksaki.Owner = this;
        }

        private void EOSドラスタ送信データ作成Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new EOSドラスタ送信データ作成Template();
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
            gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);

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
            //this.gcMultiRow1.ShortcutKeyManager.Register(new EOSドラスタ送信データ作成FunctionKeyAction(Keys.F3), Keys.F3);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // コンボボックス
            GcComboBoxCell comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value = "1";
            gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");
            gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "送信区分textBoxCell");

        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "送信区分textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    break;
                case "担当者コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "送信区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "送信区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    break;
                case "担当者名gcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    break;
                case "得意先コード始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    break;
                case "得意先コード終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
                    }
                    break;
                case "受注NO始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
                    }
                    break;
                case "受注NO終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                    }
                    break;
                case "売上日始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                    }
                    break;
                case "売上日終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
                    }
                    break;
                case "出力ファイル名textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                    }
                    break;
                case "ファイルオープンbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                    }
                    break;
                case "CSV出力buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
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
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "CSV出力buttonCell");
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
                case "終了buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "送信区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "送信区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "送信区分textBoxCell");
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            if (e.CellName == "売上日始textBoxCell")
            {
                if (gcMultiRow1.CurrentCell.Value != null)
                {
                    string buf = gcMultiRow1.CurrentCell.Value.ToString();
                    buf = buf.Replace("/", "");
                    buf = Utility.getDate(buf);
                    gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value = buf;
                }
            }
            if (e.CellName == "売上日終textBoxCell")
            {
                if (gcMultiRow1.CurrentCell.Value != null)
                {
                    string buf = gcMultiRow1.CurrentCell.Value.ToString();
                    buf = buf.Replace("/", "");
                    buf = Utility.getDate(buf);
                    gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value = buf;
                }
            }
        }

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "CSV出力buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Down:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");
                            break;
                        case Keys.Up:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
                            break;
                        case Keys.Enter:

                            Create_CSV(1);

                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");

                            break;
                    }
                    break;

                case "ファイルオープンbuttonCell":

                    int ret;

                    switch (e.KeyCode)
                    {
                        case Keys.Down:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                            break;
                        case Keys.Up:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                            break;
                        case Keys.Enter:

                            importFile = dialogClass.OpenFileByDialog(iniFile, fileFilter);
                            gcMultiRow1.ColumnHeaders[0].Cells["出力ファイル名textBoxCell"].Value = importFile;
                            break;

                            /*
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
                                gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルパスtextBoxCell"].Value = System.IO.Path.GetDirectoryName(@openFileDialog1.FileName);
                                gcMultiRow1.ColumnHeaders[0].Cells["出力ファイル名textBoxCell"].Value = System.IO.Path.GetFileName(@openFileDialog1.FileName);
                                //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");

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
                             */
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
                            ret = Create_CSV(0);
                            if (ret == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();

                                プレビューform.dataTable = dataTable;
                                プレビューform.rptName = "EOSドラスタ送信データ作成CrystalReport";
                                プレビューform.Show();
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                            }

                            break;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            int ret;

            GcMultiRow gcMultiRow = (GcMultiRow)sender;
            if (gcMultiRow.CurrentCell is ButtonCell)
            {
                switch (e.CellName)
                {
                    case "プレビューbuttonCell":

                        ret = Create_CSV(0);
                        if (ret == 0)
                        {
                            プレビューForm プレビューform = new プレビューForm();

                            プレビューform.dataTable = dataTable;
                            プレビューform.rptName = "EOSドラスタ送信データ作成CrystalReport";
                            プレビューform.Show();
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                        }

                        break;
                    case "CSV出力buttonCell":

                        Create_CSV(1);

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");

                        break;
                    case "ファイルオープンbuttonCell":
                        importFile = dialogClass.OpenFileByDialog(iniFile, fileFilter);
                        gcMultiRow1.ColumnHeaders[0].Cells["出力ファイル名textBoxCell"].Value = importFile;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                        break;
                    case "終了buttonCell":
                        this.Hide();
                        break;
                }
            }
        }

        void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            if (e.CellName == "担当者コードtextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null )
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value;
                }
            }
            if (e.CellName == "担当者名gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名gcComboBoxCell"].Value;
                }
            }
        }

        private void csvOutCheck()
        {
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value == null )
            {
                MessageBox.Show("送信区分を入力して下さい｡", "ドラスタ納品データ送信");
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "送信区分textBoxCell");
                return;
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value == null )
            {
                MessageBox.Show("担当者コードを入力して下さい｡", "ドラスタ納品データ送信");
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                return;
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value != "0" && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value != "1")
            {
                MessageBox.Show("送信区分を正しく入力して下さい｡", "ドラスタ納品データ送信");
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "送信区分textBoxCell");
                return;
            }

            if (Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value) > 999 )
            {
                MessageBox.Show("担当者コード入力エラーです。", "ドラスタ納品データ送信");
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                return;
            }

            if ( Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value) > 9999999 )
            {
                MessageBox.Show("得意先コード入力エラーです。", "ドラスタ納品データ送信");
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                return;
            }

            if ( Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value) > 9999999 )
            {
                MessageBox.Show("得意先コード入力エラーです。", "ドラスタ納品データ送信");
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                return;
            }


            if ( Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value) != 0 &&
                Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value) != 0 )
            {
                if ( Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value) >
                    Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value) )
                {
                    MessageBox.Show("得意先コード入力エラーです。", "ドラスタ納品データ送信");
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    return;
                }
            }

            if ( Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value) != 0 &&
                Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value) != 0 )
            {
                if ( Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value) >
                    Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value) )
                {
                    MessageBox.Show("売上伝票番号入力エラーです。", "ドラスタ納品データ送信");
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注NO始textBoxCell");
                    return;
                }
            }
            
            /*
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value == null )
            {
                if (!IsDate(this.売上日_ST))
                {
                    Interaction.MsgBox("売上日入力エラーです。" + Strings.Chr(9), Constants.vbOKOnly, "ドラスタ納品データ送信");
                    this.売上日_ST.SetFocus();
                    return;0
                }
            }
            */

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルパスtextBoxCell"].Value == null )
            {
                MessageBox.Show("フォルダ名を入力して下さい｡", "ドラスタ納品データ送信");
                return;
            }

            //if (Dir(this.txtFilePATH1, Constants.vbDirectory) == "")
            //{
            //    Interaction.MsgBox("指定されたフォルが存在しません" + Strings.Chr(9), Constants.vbOKOnly, "ドラスタ納品データ送信");
            //    return;
            //}


            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力ファイル名textBoxCell"].Value == null )
            {
                MessageBox.Show("ファイル名を入力して下さい｡", "ドラスタ納品データ送信");
                return;
            }

            //if (MessageBox.Show("送信データを作成します.よろしいですか。", 65, "ドラスタ受注取込") == 2)
            //    return;

            if (Create_CSV(1) == 0)
                MessageBox.Show("正常に作成されました。", "ドラスタ納品データ送信");

            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "送信区分textBoxCell");

        }

        public int Create_CSV(int fileFlg)
        {
            // *****************************************************************************
            // ワークテーブル作成
            // *****************************************************************************
            int ret = 0;
            string strSQL;
            //Recordset Rds;
            string enMark;
            //Variant varFileExist;
            //Database db;
            //Currency CurKin;
            string MSG;
            // 
            //Variant TextLine;
            string S_dmy;
            string d_date="";

            string AiteNo;
            long gyo;
            int Opn_flg;

            //Variant DMY;
            //Create_CSV = 1;

            string filePath = "";
            string fileName = "";

            if (fileFlg == 1)
            {

                enMark = @"\";
                Opn_flg = 0;

                //filePath = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルパスtextBoxCell"].Value;
                fileName = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力ファイル名textBoxCell"].Value;

                filePath = Path.GetDirectoryName(@fileName);

                if (!(System.IO.Directory.Exists(filePath)))
                {
                    MessageBox.Show("ファイルを入力してください。", "ドラスタ送信データ作成");
                    return 0;
                }

                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
            }

            /*
                SqlDb sqlDb = new SqlDb();
                sqlDb.Connect(); 

                AiteNo = " ";

                // ##### ＣＳＶ抽出 #####
                strSQL = " ";
                strSQL = strSQL + "SELECT A.売上連番 ";
                strSQL = strSQL + ",CASE WHEN ISNULL(A.登録番号,0) = 0 THEN 0 ELSE A.登録番号 END AS 登録番号 ";
                strSQL = strSQL + ",CASE WHEN ISNULL(A.登録番号,0) = 0 THEN 0 ELSE A.ドラスタ発注番号 END AS 発注番号 ";
                strSQL = strSQL + ",CASE WHEN ISNULL(A.登録番号,0) = 0 THEN 0 ELSE ISNULL(J.部コード,0) END AS 明細番号 ";
                strSQL = strSQL + ",CASE WHEN ISNULL(A.登録番号,0) = 0 THEN (CASE WHEN J.受注連番 IS NULL THEN A.売上日 ELSE J.受注日 END) ELSE J.受注日 END AS 発注日 ";
                strSQL = strSQL + ",CASE WHEN ISNULL(A.登録番号,0) = 0 THEN RIGHT(A.売上伝票番号,6) ELSE RIGHT(A.相手先注文番号,6) END AS 仕入先伝票番号";
                strSQL = strSQL + ",CASE WHEN ISNULL(A.登録番号,0) = 0 THEN 21 ELSE A.ＥＯＳ区分 END AS ＥＯＳ区分 ";
                strSQL = strSQL + ",A.客注区分 AS 伝票区分 ";
                strSQL = strSQL + ",CASE WHEN ISNULL(A.登録番号,0) = 0 THEN 1 ELSE A.発注区分 END AS 発注区分 ";
                strSQL = strSQL + ",A.直送区分 ";
                strSQL = strSQL + ",CASE WHEN ISNULL(A.登録番号,0) = 0 THEN A.伝票摘要 ELSE ISNULL(A.客注番号,'') END AS 客注番号 ";
                strSQL = strSQL + ",A.取引先コード２ ";
                strSQL = strSQL + ",A.得意先コード ";
                strSQL = strSQL + ",A.得名 ";
                strSQL = strSQL + ",A.社コード AS 企業コード ";
                strSQL = strSQL + ",CASE WHEN ISNULL(A.登録番号,0) = 0 THEN A.店コードＢ ELSE A.店コード END AS 店舗コード ";
                strSQL = strSQL + ",A.大分類コード ";
                strSQL = strSQL + ",A.ＪＡＮコード ";
                strSQL = strSQL + ",A.ＥＯＳ商品コード ";
                strSQL = strSQL + ",A.ＥＯＳ商品名 ";
                strSQL = strSQL + ",A.ＥＯＳ規格";
                strSQL = strSQL + ",ISNULL(A.本部原価単価,0) AS 本部原価単価 ";
                strSQL = strSQL + ",ISNULL(A.本部原価金額,0) AS 本部原価金額 ";
                strSQL = strSQL + ",ISNULL(A.納入単価,0) AS 納入単価 ";
                strSQL = strSQL + ",ISNULL(FLOOR(A.定価*1.08),0) AS 店舗売価 ";
                strSQL = strSQL + ",ISNULL(J.受注数,0) AS 発注数量 ";
                strSQL = strSQL + ",ISNULL(A.数量,0) AS 出荷数量 ";
                strSQL = strSQL + ",ISNULL(A.欠品理由,0) AS 欠品理由 ";
                strSQL = strSQL + ",A.店舗備考 ";
                strSQL = strSQL + ",A.仕入先備考 ";
                strSQL = strSQL + " FROM (SELECT U.* ";
                strSQL = strSQL + "       FROM T売上明細ファイル AS U ";

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value == "0")
                    strSQL = strSQL + " WHERE ISNULL(U.更新ビット,0) = 0 ";
                else
                    strSQL = strSQL + " WHERE ISNULL(U.更新ビット,0) in (1,0) ";

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null)
                    strSQL = strSQL + " AND U.担当者コード  = '" + this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value + "'";

                if ((string)(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value) != null)
                    strSQL = strSQL + " AND U.得意先コード  >= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value + "'";

                if ((string)(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value) != null)
                    strSQL = strSQL + " AND U.得意先コード  >= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value + "'";

                if ( Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value) != 0 )
                    strSQL = strSQL + " AND U.売上伝票番号 >= '" + Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value) + "'";

                if (Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value) != 0)
                    strSQL = strSQL + " AND U.売上伝票番号 >= '" + Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value) + "'";

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value == "0")
                {
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value != null)
                        strSQL = strSQL + " AND U.売上日  <= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value + "' " ;
                    }
                    else
                    {
                        if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value != null)
                            strSQL = strSQL + " AND U.売上日  >= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value + "' ";

                        if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value != null)
                            strSQL = strSQL + " AND U.売上日  <= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value + "' ";
                    }

                strSQL = strSQL + " AND ISNULL(U.ドラスタ区分,0) = 1 ";
                strSQL = strSQL + "      ) AS A ";
                strSQL = strSQL + " LEFT JOIN T受注ファイル AS J ";
                strSQL = strSQL + " ON J.受注連番 = A.受注連番 ";
                strSQL = strSQL + " INNER JOIN T得意先マスタ AS TOK ";
                strSQL = strSQL + " ON TOK.[得意先コード] = A.[得意先コード] ";
                strSQL = strSQL + " WHERE TOK.地区コード = '100' ";

                //strSQL = strSQL + "  ) as B ";
                //strSQL = strSQL + " ORDER BY  ISNULL(B.発注番号,0), B.売上伝票番号 , B.受注行番号, B.売上連番 ";

            
                // INSERT時にORDERBYはつけたらNG
                //strSQL = strSQL + " ORDER BY  ISNULL(A.発注番号,0), A.売上伝票番号 , A.受注行番号, A.売上連番 ";

                dataTable = sqlDb.ExecuteSql(strSQL, -1);


                dataTable.AcceptChanges();


                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value == "0")
                    strSQL = strSQL + " WHERE ISNULL(U.更新ビット,0) = 0 ";
                else
                    strSQL = strSQL + " WHERE ISNULL(U.更新ビット,0) in (1,0) ";

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null)
                    strSQL = strSQL + " AND U.担当者コード  = '" + this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value + "'";

                if ((string)(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value) != null)
                    strSQL = strSQL + " AND U.得意先コード  >= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value + "'";

                if ((string)(this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value) != null)
                    strSQL = strSQL + " AND U.得意先コード  >= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value + "'";

                if (Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value) != 0)
                    strSQL = strSQL + " AND U.売上伝票番号 >= '" + Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value) + "'";

                if (Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value) != 0)
                    strSQL = strSQL + " AND U.売上伝票番号 >= '" + Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value) + "'";

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value == "0")
                {
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value != null)
                        strSQL = strSQL + " AND U.売上日  <= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value + "' ";
                }
                else
                {
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value != null)
                        strSQL = strSQL + " AND U.売上日  >= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value + "' ";

                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value != null)
                        strSQL = strSQL + " AND U.売上日  <= '" + this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value + "' ";
                }
            */

            string 送信区分;
            string 担当者コード;
            string 得意先コード始;
            string 得意先コード終;
            string 受注NO始;
            string 受注NO終;
            string 売上日始;
            string 売上日終;

            送信区分 = this.gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value.ToString();
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null)
            {
                担当者コード = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value.ToString();
            }
            else 
            {
                担当者コード = null;
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value != null)
            {
                得意先コード始 = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value.ToString();
            }
            else
            {
                得意先コード始 = null;
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value != null)
            {
                得意先コード終 = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value.ToString();
            }
            else
            {
                得意先コード終 = null;
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value != null)
            {
                受注NO始 = this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始textBoxCell"].Value.ToString();
            }
            else
            {
                受注NO始 = null;
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value != null)
            {
                受注NO終 = this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終textBoxCell"].Value.ToString();
            }
            else
            {
                受注NO終 = null;
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value != null)
            {
                売上日始 = this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value.ToString();
            }
            else
            {
                売上日始 = null;
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value != null)
            {
                売上日終 = this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value.ToString();
            }
            else
            {
                売上日終 = null;
            }
            this.gcMultiRow1.ColumnHeaders[0].Cells["送信区分textBoxCell"].Value.ToString();
            dataTable = wtEOSドラスタ送信データ作成チェックリストTableAdapter.GetDataBy(
                    送信区分,
                    担当者コード,
                    得意先コード始,
                    得意先コード終,
                    受注NO始,
                    受注NO終,
                    売上日始,
                    売上日終
                );

            /*
            */

            if (dataTable.Rows.Count== 0)
            {
//                sqlDb.Disconnect();
                MessageBox.Show("データがありません。", "ドラスタ送信データ作成");
                return 0;
            }

            if (fileFlg == 0)
            {
                return 0;
            }
            
            // CSVファイルの出力
            string strLine;

            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter streamWriter =
                new System.IO.StreamWriter(@fileName, false, enc);

            for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
            {
                strLine = "";
                //strLine = strLine + withBlock.Fields(1) + "," + withBlock.Fields(2) + "," + withBlock.Fields(3) + "," + Replace(Nz(withBlock.Fields(4), ""), "/", "") + "," + withBlock.Fields(5) + "," + withBlock.Fields(6) + ",";
                //strLine = strLine + withBlock.Fields(7) + "," + withBlock.Fields(8) + "," + withBlock.Fields(9) + "," + withBlock.Fields(10) + "," + withBlock.Fields(11) + ",";
                //strLine = strLine + withBlock.Fields(14) + "," + withBlock.Fields(15) + "," + withBlock.Fields(16) + "," + Format(withBlock.Fields(17), "0000000000000") + "," + withBlock.Fields(19) + ",";
                //strLine = strLine + withBlock.Fields(20) + "," + withBlock.Fields(21) + "," + withBlock.Fields(23) + "," + withBlock.Fields(24) + "," + withBlock.Fields(25) + ",";
                //strLine = strLine + withBlock.Fields(26) + "," + withBlock.Fields(27) + ",E" + "," + Format<DateTime, > + "," + Format(DateTime + 1, "YYYYMMDD") + "," + withBlock.Fields(29) + "," + withBlock.Fields(28);
                strLine = strLine + dataTable.Rows[i]["登録番号"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["発注番号"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["明細番号"].ToString() + ",";
                string buf = null;
                buf = Utility.Mid(dataTable.Rows[i]["発注日"].ToString(), 1, 4) + Utility.Mid(dataTable.Rows[i]["発注日"].ToString(), 6, 2) + Utility.Mid(dataTable.Rows[i]["発注日"].ToString(), 9, 2);
                strLine = strLine + buf + ",";
                strLine = strLine + dataTable.Rows[i]["仕入先伝票番号"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["ＥＯＳ区分"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["伝票区分"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["発注区分"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["直送区分"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["客注番号"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["取引先コード２"].ToString() + ",";
                //strLine = strLine + dataTable.Rows[i]["得意先コード"].ToString() + ",";
                //strLine = strLine + dataTable.Rows[i]["得名"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["企業コード"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["店舗コード"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["大分類コード"].ToString() + ",";
                //strLine = strLine + dataTable.Rows[i]["ＪＡＮコード"].ToString() + ",";
                strLine = strLine + String.Format("{0:0}", dataTable.Rows[i]["ＪＡＮコード"]) + ",";
                //strLine = strLine + dataTable.Rows[i]["ＥＯＳ商品コード"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["ＥＯＳ商品名"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["ＥＯＳ規格"].ToString() + ",";
                //strLine = strLine + dataTable.Rows[i]["本部原価単価"].ToString() + ",";
                strLine = strLine + String.Format("{0:0}", dataTable.Rows[i]["本部原価単価"].ToString().Replace(",","")) + ",";
                ////strLine = strLine + dataTable.Rows[i]["本部原価金額"].ToString() + ",";
                strLine = strLine + String.Format("{0:0}", dataTable.Rows[i]["本部原価金額"].ToString().Replace(",", "")) + ",";
                //strLine = strLine + dataTable.Rows[i]["納入単価"].ToString() + ",";
                strLine = strLine + String.Format("{0:0}", dataTable.Rows[i]["納入単価"].ToString().Replace(",", "")) + ",";
                //strLine = strLine + dataTable.Rows[i]["店舗売価"].ToString() + ",";
                strLine = strLine + String.Format("{0:0}", dataTable.Rows[i]["店舗売価"].ToString().Replace(",", "")) + ",";
                //strLine = strLine + dataTable.Rows[i]["発注数量"].ToString() + ",";
                strLine = strLine + String.Format("{0:0}", dataTable.Rows[i]["発注数量"].ToString().Replace(",", "")) + ",";
                //strLine = strLine + dataTable.Rows[i]["出荷数量"].ToString() + ",";
                strLine = strLine + String.Format("{0:0}", dataTable.Rows[i]["出荷数量"].ToString().Replace(",", "")) + ",";
                strLine = strLine + dataTable.Rows[i]["欠品理由"].ToString() + "," + "E" + ",";
                strLine = strLine + System.DateTime.Today.ToString("yyyyMMdd") + ",";
                strLine = strLine + System.DateTime.Today.AddDays(1).ToString("yyyyMMdd") + ",";
                strLine = strLine + dataTable.Rows[i]["店舗備考"].ToString() + ",";
                strLine = strLine + dataTable.Rows[i]["仕入先備考"].ToString();
                streamWriter.WriteLine(strLine);
            }

            streamWriter.Close();

            /*
            string Q_ドラスタ納品データ抽出 = "(" + strSQL + ") as B";
            // ##### ワーク削除 #####
            strSQL = "DELETE FROM WTドラスタ納品データ抽出_チェックリスト";

            dataTable = sqlDb.ExecuteSql(strSQL, -1);

            d_date = System.DateTime.Today.ToString("yyyy/MM/dd hh:mm:ss");

            // ##### ワーク追加 ##### money 値を nvarchar に変換した結果を格納するための領域が不足しています。
            strSQL = "INSERT INTO WTドラスタ納品データ抽出_チェックリスト ";
            strSQL = strSQL + "( 受注日, 納期, 相手先注文番号, 店名, 支払月, 商品コード, 商名,";
            strSQL = strSQL + "出荷数量, 原価単価, 原価金額, 発注数, 欠品数, 受注行, 作成日時, 得意先コード, ＥＯＳ商品コード, 表示価格, ";
            strSQL = strSQL + "得名,棚番,規格";
            strSQL = strSQL + ",売上連番";
            strSQL = strSQL + ",欠品理由";
            strSQL = strSQL + " )";
            strSQL = strSQL + " SELECT 発注日, ";
            strSQL = strSQL + "        NULL AS 納期,";
            strSQL = strSQL + "        仕入先伝票番号, 店舗コード, NULL AS 支払月, ＥＯＳ商品コード, ＥＯＳ商品名,";
            strSQL = strSQL + "出荷数量 AS 出荷数, 本部原価単価, 本部原価金額,";
            strSQL = strSQL + "発注数量 AS 発注数, 発注数量-出荷数量 AS 欠品数, 明細番号,";
            strSQL = strSQL + "'" + d_date + "' AS 作成日時, 得意先コード, ＪＡＮコード, 店舗売価, 得名,NULL,ＥＯＳ規格";
            strSQL = strSQL + ",売上連番";
            strSQL = strSQL + ",欠品理由";
            strSQL = strSQL + " FROM " + Q_ドラスタ納品データ抽出 + ";";

            dataTable = sqlDb.ExecuteSql(strSQL, -1);

            */

            // ##### ワーク更新 #####
            //strSQL = "UPDATE T売上明細ファイル AS A ";
            //strSQL = strSQL + " INNER JOIN WTドラスタ納品データ抽出_チェックリスト AS B ";
            //strSQL = strSQL + " ON (A.売上連番 = B.売上連番)  ";
            //strSQL = strSQL + " SET A.更新ビット = 1;";

            SqlDb sqlDb = new SqlDb();
            sqlDb.Connect();

            strSQL = "UPDATE T売上明細ファイル ";
            strSQL = strSQL + " SET 更新ビット = 1 ";
            strSQL = strSQL + " WHERE 売上連番 IN ";
            strSQL = strSQL + " (SELECT A.売上連番 from T売上明細ファイル AS A ";
            strSQL = strSQL + " INNER JOIN WTEOSドラスタ送信データ作成チェックリスト AS B ON A.売上連番 = B.売上連番) "; 
            
            dataTable = sqlDb.ExecuteSql(strSQL, -1);

            sqlDb.Disconnect();

            MessageBox.Show("ファイルを作成しました。", "ドラスタ送信データ作成");

            return 0;

        }

        
        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F3:
                    //target = this.ButtonF3;
                    String cname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (cname == "得意先コード始textBoxCell")
                    {
                        //仕入先検索Form jform = new 仕入先検索Form();
                        //jform.Show();
                        fsToksaki.Show();
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }

        }

        public string ReceiveDataToksaki
        {
            set
            {
                receiveDataToksaki = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始gcTextBoxCell"].Value = receiveDataToksaki.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataToksaki;
            }
        }


    }

}
