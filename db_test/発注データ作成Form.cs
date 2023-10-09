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
    public partial class 発注データ作成Form : Form
    {
        private string receiveDataSirsaki = "";
        仕入先検索Form fsSirsaki;
        private string iniFile = null;
        private Encoding SJIS = Encoding.GetEncoding("Shift-JIS");
        DialogClass dialogClass = new DialogClass();
        string fileFilter = "CSVファイル(*.csv;*.txt)|*.csv;*.txt|すべてのファイル(*.*)|*.*";
        string importFile = null;
        bool fireFLG;
        private DataSet dataSet;
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        
        public 発注データ作成Form()
        {
            InitializeComponent();
        }

        private void 発注データ作成Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 発注データ作成Template();
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
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            
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
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellThenControl, Keys.Up);

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
            GcComboBoxCell comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";
            GcComboBoxCell comboBoxCell02= this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = "004400";
            gcMultiRow1.ColumnHeaders[0].Cells["発行区分textBoxCell"].Value = "0";
            //gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");
            //gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");
            gcMultiRow1.ColumnHeaders[0].Cells["発注NO始textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["発注NO終textBoxCell"].Value = "*";

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");

        }

        void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "仕入先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行区分textBoxCell");
                    }
                    break;
                case "発行区分textBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["発行区分textBoxCell"].Value.ToString() =="0")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                        }
                    }
                    else
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                        }
                    }

                    break;
                case "発注日始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                    }
                    break;
                case "発注日終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                    }
                    break;
                case "担当者コード始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終textBoxCell");
                    }
                    break;
                case "担当者始gcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終textBoxCell");
                    }
                    break;
                case "担当者コード終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO始textBoxCell");
                    }
                    break;
                case "担当者終gcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO始textBoxCell");
                    }
                    break;
                case "発注NO始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO終textBoxCell");
                    }
                    break;
                case "発注NO終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力ファイル名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO終textBoxCell");
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
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NO終textBoxCell");
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
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    break;
            }
        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            if (e.CellName == "担当者コード始textBoxCell")
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始textBoxCell"].Value != null)
                {
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始textBoxCell"].Value.ToString() != "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始textBoxCell"].Value;
                    }
                }
            }
            if (e.CellName == "担当者始gcComboBoxCell")
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value != null)
                {
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value.ToString() != "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始textBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value;
                    }
                }
            }
            if (e.CellName == "担当者コード終textBoxCell")
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終textBoxCell"].Value != null)
                {
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終textBoxCell"].Value.ToString() != "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終textBoxCell"].Value;
                    }
                }
            }
            if (e.CellName == "担当者終gcComboBoxCell")
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value != null)
                {
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value.ToString() != "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終textBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value;
                    }
                }
            }
        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "CSV出力buttonCell")
            {
                if (Create_CSV(0) == 0)
                {
                    MessageBox.Show("ファイルを作成しました", "発注データ作成");
                }
                else
                {
                    MessageBox.Show("データがありません", "発注データ作成");
                }

                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");

            }
            if (e.CellName == "終了buttonCell")
            {
                this.Hide();
            }
            if (e.CellName == "プレビューbuttonCell")
            {
                if (Create_CSV(9) == 0)
                {
                    プレビューForm プレビューform = new プレビューForm();

                    //プレビューform.dataSet = dataSet;
                    プレビューform.dataTable = dataTable;
                    プレビューform.rptName = "発注データ作成CrystalReport";
                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "発注データ作成");
                }
            }
            if (e.CellName == "ファイルオープンbuttonCell")
            {

                importFile = dialogClass.OpenFileByDialog(iniFile, fileFilter);
                gcMultiRow1.ColumnHeaders[0].Cells["出力ファイル名textBoxCell"].Value = importFile;
                
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
                    //gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルパスtextBoxCell"].Value = System.IO.Path.GetDirectoryName(@openFileDialog1.FileName);
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
            }
        }

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "CSV出力buttonCell":
                    switch (e.KeyCode)
                    {
                        //case Keys.Down:
                        //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");
                        //    break;
                        //case Keys.Up:
                        //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注ファイルtextBoxCell");
                        //    break;
                        case Keys.Enter:

                            if (Create_CSV(0) == 0)
                            {
                                MessageBox.Show("ファイルを作成しました", "発注データ作成");
                            }
                            else 
                            {
                                MessageBox.Show("データがありません", "発注データ作成");
                            }

                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");

                            break;
                    }
                    break;

                case "ファイルオープンbuttonCell":
                    switch (e.KeyCode)
                    {
                        //case Keys.Down:
                        //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "CSV出力buttonCell");
                        //    break;
                        //case Keys.Up:
                        //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                        //    break;
                        case Keys.Enter:
                            importFile = dialogClass.OpenFileByDialog(iniFile, fileFilter);
                            gcMultiRow1.ColumnHeaders[0].Cells["出力ファイル名textBoxCell"].Value = importFile;

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
                            if (Create_CSV(9) == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();

                                プレビューform.dataSet = dataSet;
                                プレビューform.rptName = "発注データ作成CrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "発注データ作成");
                            }
                            break;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            string buf = "";

            switch (e.CellName)
            {
                case "発行区分textBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["発行区分textBoxCell"].Value.ToString() == "0")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Enabled = false;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Enabled = false;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO始textBoxCell"].Enabled = false;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO終textBoxCell"].Enabled = false;
                    }
                    else if (this.gcMultiRow1.ColumnHeaders[0].Cells["発行区分textBoxCell"].Value.ToString() == "1")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Enabled = true;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Enabled = true;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO始textBoxCell"].Enabled = true;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO終textBoxCell"].Enabled = true;
                    }
                    break;
                case "担当者始gcComboBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始textBoxCell"].Value = "*";
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始textBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value;
                    }
                    break;
                case "仕入先コードtextBoxCell":
                    buf = Utility.GetCode("T仕入先マスタ", "決済コメント", "仕入先コード", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].EditedFormattedValue.ToString());
                    if (buf == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["決済コメント"].Value = null;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["決済コメント"].Value = buf;
                    }

                    break;
                
                case "発注日始textBoxCell":

                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value = "*";
                    }else
                    {
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value.ToString() != "*")
                        {
                            buf = "";
                            buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value;
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
                                this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value = buf;
                            }
                            else
                            {
                                MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            }
                        }
                    }
                    break;
                case "発注日終textBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value = "*";
                    }else
                    {
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value.ToString() != "*")
                        {
                            buf = "";
                            buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value;
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
                                this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value = buf;
                            }
                            else
                            {
                                MessageBox.Show("日付を(YYYYMMDD)形式で入力してください", "タイトル", MessageBoxButtons.YesNo);
                            }
                        }
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            string buf = null;

            switch (e.CellName)
            {
                case "発行区分textBoxCell":
                    if (e.FormattedValue == null)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else if (e.FormattedValue.ToString() == "0" || e.FormattedValue.ToString() == "1")
                    {
                        e.Cancel = false;
                        break;
                    }
                    else
                    {
                        e.Cancel = true;
                        break;
                    }
                    break;
                case "担当者始gcComboBoxCell":
                    if (e.FormattedValue == null)
                    {
                        e.Cancel = false;
                        break;
                    }else if (e.FormattedValue.ToString() == "*")
                    {
                        e.Cancel = false;
                        break;
                    }
                    break;
                case "仕入先コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        buf = Utility.GetCode("T仕入先マスタ", "仕入先コード", "仕入先コード", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].EditedFormattedValue.ToString());
                        if (buf == null)
                        {
                            MessageBox.Show("仕入先が未入力です。", "タイトル", MessageBoxButtons.OK);
                            e.Cancel = true;
                            return;
                        }

                        e.Cancel = false;

                    }
                    else
                    {
                        MessageBox.Show("仕入先が未入力です。", "タイトル", MessageBoxButtons.OK);
                        e.Cancel = true;
                        return;
                    }
                    break;
                case "発注日始textBoxCell":

                    if (e.FormattedValue == null)
                    {
                        e.Cancel = false;
                        break;
                    }
                    else
                    {
                        if (e.FormattedValue.ToString() == "*")
                        {
                            e.Cancel = false;
                            break;
                        }
                        else
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
                    }
                    break;

                case "発注日終textBoxCell":

                    if (e.FormattedValue == null)
                    {
                        e.Cancel = false;
                        break;
                    }
                    else
                    {
                        if (e.FormattedValue.ToString() == "*")
                        {
                            e.Cancel = false;
                            break;
                        }
                        else
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
                    }
                    break;

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

            if (Create_CSV(0) == 0)
                MessageBox.Show("正常に作成されました。", "ドラスタ納品データ送信");

            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "送信区分textBoxCell");

        }

        public int Create_CSV(int param)
        {
            // *****************************************************************************
            // ワークテーブル作成
            // *****************************************************************************
            SqlDb sqlDb = new SqlDb();
            //DataTable dataTable;
            sqlDb.Connect(); 
            int ret = 0;
            string strSQL;
            //Recordset Rds;
            string enMark;
            //Variant varFileExist;
            //Database db;
            //Currency CurKin;
            string MSG;
            // 
            //Variant strLine;
            string S_dmy;
            string d_date="";

            string AiteNo;
            long gyo;
            int Opn_flg;

            //Variant DMY;
            //Create_CSV = 1;

            string filePath = "";
            string fileName = "";

            string W_発注NO_ST;
            string W_発注NO_ED;

            string W_発注日_ST;
            string W_発注日_ED;
            string W発注摘要;
            int lenmemo;

            enMark = @"\";
            Opn_flg = 0;
            
            //filePath = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルパスtextBoxCell"].Value;
            fileName = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力ファイル名textBoxCell"].Value;

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(@fileName);
            }

            AiteNo = " ";

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;


            // ストアド プロシージャ名を指定
            command.CommandText = "発注データ作成";

            command.Parameters.AddWithValue("@仕入先コード", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value.ToString());
            
            command.Parameters.AddWithValue("@発行区分", this.gcMultiRow1.ColumnHeaders[0].Cells["発行区分textBoxCell"].Value.ToString());

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value != null)
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value.ToString() != "*")
                {
                    command.Parameters.AddWithValue("@発注日始", this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@発注日始", "*");
                }
            }
            else
            {
                command.Parameters.AddWithValue("@発注日始", "*");
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value != null)
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value.ToString() != "*")
                {
                    command.Parameters.AddWithValue("@発注日終", this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@発注日終", "*");
                }
            }
            else
            {
                command.Parameters.AddWithValue("@発注日終", "*");
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO始textBoxCell"].Value != null)
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO始textBoxCell"].Value.ToString() != "*")
                {
                    command.Parameters.AddWithValue("@発注番号始", this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO始textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@発注番号始", "*");
                }
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号始", "*");
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO終textBoxCell"].Value != null)
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO終textBoxCell"].Value.ToString() != "*")
                {
                    command.Parameters.AddWithValue("@発注番号終", this.gcMultiRow1.ColumnHeaders[0].Cells["発注NO終textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@発注番号終", "*");
                }
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号終", "*");
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始textBoxCell"].Value == null)
            {
                command.Parameters.AddWithValue("担当者コード始", "*");
            }
            else
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始textBoxCell"].Value.ToString() != "*")
                {
                    command.Parameters.AddWithValue("@担当者コード始", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("担当者コード始", "*");
                }
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終textBoxCell"].Value == null)
            {
                command.Parameters.AddWithValue("担当者コード終", "*");
            }
            else
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終textBoxCell"].Value.ToString() != "*")
                {
                    command.Parameters.AddWithValue("@担当者コード終", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("担当者コード終", "*");
                }
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
                sqlDb.Disconnect();
                return ret;

            }

            if (param == 0)
            {
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
                    // ===== 明細 =====
                    strLine = "";
                    // 納入先ｺｰﾄﾞ
                    if (Utility.S_Set(dataTable.Rows[i]["オンライン納入先CD"]) != " ")
                        strLine = strLine + Utility.Left(Utility.S_Set(dataTable.Rows[i]["オンライン納入先CD)"]).ToString().TrimEnd() + String.Format("     "), 5) + ",";
                    else if (Utility.S_Set(dataTable.Rows[i]["オーダ"]) == "E")
                        strLine = strLine + "B1369,";
                    else if (Utility.S_Set(dataTable.Rows[i]["オーダ"]) == "M")
                        strLine = strLine + "B4188,";
                    // 品番
                    if ((Utility.S_Set(dataTable.Rows[i]["メーカー品番"]).ToString().TrimEnd().Length) > 5)
                        strLine = strLine + Utility.Left(Utility.S_Set(dataTable.Rows[i]["メーカー品番"]).ToString().TrimEnd(), 5) + ",";
                    else
                        strLine = strLine + Utility.S_Set(dataTable.Rows[i]["メーカー品番"]).ToString().TrimEnd() + ",";
                    // 数量
                    if (Utility.Z_Set(Utility.S_Set(dataTable.Rows[i]["発注数"])) > 999)
                        strLine = strLine + "999,";
                    else
                        //strLine = strLine + Utility.Z_Set(Utility.S_Set(dataTable.Rows[i]["発注数"])) + ",";
                        strLine = strLine + String.Format("{0:#}", Utility.Z_Set(Utility.S_Set(dataTable.Rows[i]["発注数"]))) + ",";
                    W発注摘要 = Utility.S_Set(dataTable.Rows[i]["発注摘要"]);

                    lenmemo = W発注摘要.Length;

                    if (lenmemo >= 15)
                        lenmemo = 15;

                    switch (Utility.S_Set(dataTable.Rows[i]["オーダ"]))
                    {
                        case "M":
                            {
                                if (Utility.Z_Set(dataTable.Rows[i]["仕入担当者コード"]) == 0)
                                {
                                    //strLine = strLine + W発注摘要 + " "
                                    strLine = strLine
                                  + "M"
                                  + Utility.S_Set(dataTable.Rows[i]["発注番号"]) + ""
                                  + Utility.S_Set(dataTable.Rows[i]["発注行番号"]);
                                }
                                else
                                {
                                    //strLine = strLine + W発注摘要 + " "
                                    strLine = strLine
                                  + Utility.Z_Set(dataTable.Rows[i]["仕入担当者コード"]) + "M"
                                  + Utility.S_Set(dataTable.Rows[i]["発注番号"]) + ""
                                  + Utility.S_Set(dataTable.Rows[i]["発注行番号"]);
                                }

                                break;
                            }

                        case "E":
                            {
                                strLine = strLine
                              + Utility.Z_Set(dataTable.Rows[i]["得意先担当者コード"]) + "E"
                              + Utility.S_Set(dataTable.Rows[i]["発注番号"]) + ""
                              + Utility.S_Set(dataTable.Rows[i]["発注行番号"]);
                                break;
                            }
                    };

                    streamWriter.WriteLine(strLine);
                }

                streamWriter.Close();
            }
            else
            {
                if (dataTable != null)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["オンライン納入先CD"].ToString() == "")
                        {
                            if (Utility.S_Set(row["オーダ"]) == "E")
                            {
                                row["オンライン納入先CD"] = "B1369";
                            }
                            else if (Utility.S_Set(row["オーダ"]) == "M")
                            {
                                row["オンライン納入先CD"] = "B4188";
                            }
                        }
                    }
                }

            }

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


            // ##### ワーク更新 #####
            //strSQL = "UPDATE T売上明細ファイル AS A ";
            //strSQL = strSQL + " INNER JOIN WTドラスタ納品データ抽出_チェックリスト AS B ";
            //strSQL = strSQL + " ON (A.売上連番 = B.売上連番)  ";
            //strSQL = strSQL + " SET A.更新ビット = 1;";

            strSQL = "UPDATE T売上明細ファイル ";
            strSQL = strSQL + " SET 更新ビット = 1 ";
            strSQL = strSQL + " WHERE 売上連番 IN ";
            strSQL = strSQL + " (SELECT A.売上連番 from T売上明細ファイル AS A ";
            strSQL = strSQL + " INNER JOIN WTドラスタ納品データ抽出_チェックリスト AS B ON A.売上連番 = B.売上連番) "; 
            
            dataTable = sqlDb.ExecuteSql(strSQL, -1);
            */

            sqlDb.Disconnect();

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
                    if (cname == "仕入先コードtextBoxCell")
                    {
                        fsSirsaki = new 仕入先検索Form();
                        fsSirsaki.Owner = this;
                        fsSirsaki.Show();
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }

        }

        public string ReceiveDataSirsaki
        {
            set
            {
                receiveDataSirsaki = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = receiveDataSirsaki.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataSirsaki;
            }
        }
    }
}
