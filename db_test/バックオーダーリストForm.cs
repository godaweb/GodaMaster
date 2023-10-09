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
    public partial class バックオーダーリストForm : Form
    {
        private string receiveDataToksaki = "";
        得意先検索Form fsToksaki;
        private string receiveDataSyohin = "";
        商品検索Form fsSyohin;
        private DataSet dataSet;
        GcComboBoxCell comboBoxCell01 = null;
        GcComboBoxCell comboBoxCell02 = null;
        GcComboBoxCell comboBoxCell03 = null;
        GcComboBoxCell comboBoxCell04 = null;
        GcComboBoxCell comboBoxCell05 = null;
        GcComboBoxCell comboBoxCell06 = null;

        int rowcnt = 0;

        public バックオーダーリストForm()
        {
            InitializeComponent();
            fsToksaki = new 得意先検索Form();
            fsToksaki.Owner = this;
            fsSyohin = new 商品検索Form();
            fsSyohin.Owner = this;
        }

        private void バックオーダーリストForm_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new バックオーダーリストTemplate();
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
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            //gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            //gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);

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
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new バックオーダーリストFunctionKeyAction(Keys.F3), Keys.F3);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // コンボボックス
            comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.TextSubItemIndex = 0;
            comboBoxCell03.TextSubItemIndex = 1;
            comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell03.TextFormat = "[1]";
            
            comboBoxCell04 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell04.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.TextSubItemIndex = 0;
            comboBoxCell04.TextSubItemIndex = 1;
            comboBoxCell04.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell04.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell04.TextFormat = "[1]";

            comboBoxCell05 = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell05.DataSource = Utility.GetComboBoxData("SELECT メーカーコード, メーカー名 FROM Tメーカーマスタ ORDER BY メーカーコード");
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.TextSubItemIndex = 0;
            comboBoxCell05.TextSubItemIndex = 1;
            comboBoxCell05.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell05.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell05.TextFormat = "[1]";
            
            comboBoxCell06 = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell06.DataSource = Utility.GetComboBoxData("SELECT メーカーコード, メーカー名 FROM Tメーカーマスタ ORDER BY メーカーコード");
            comboBoxCell06.ListHeaderPane.Visible = false;
            comboBoxCell06.TextSubItemIndex = 0;
            comboBoxCell06.TextSubItemIndex = 1;
            comboBoxCell06.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell06.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell06.TextFormat = "[1]";

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["受注日始textBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["受注日終textBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号S"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号E"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["表示順checkBoxCell"].Value = false;
            gcMultiRow1.ColumnHeaders[0].Cells["chk印刷区分"].Value = false;

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");

        }

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "プレビューbuttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:

                            if (createReportData() == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();

                                プレビューform.dataSet = dataSet;
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["表示順checkBoxCell"].Value.Equals(0))
                                {
                                    プレビューform.rptName = "バックオーダーリストCrystalReport";
                                }
                                else
                                {
                                    プレビューform.rptName = "バックオーダーリスト_商品順CrystalReport";
                                }

                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "バックオーダーリスト");
                            }
                            break;
                    }
                    break;
                case "印刷buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            if (createReportData() == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();

                                プレビューform.dataSet = dataSet;
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["表示順checkBoxCell"].Value.Equals(0))
                                {
                                    プレビューform.rptName = "バックオーダーリストCrystalReport";
                                }
                                else
                                {
                                    プレビューform.rptName = "バックオーダーリスト_商品順CrystalReport";
                                }

                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "バックオーダーリスト");
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
                if (createReportData() == 0)
                {
                    プレビューForm プレビューform = new プレビューForm();

                    プレビューform.dataSet = dataSet;
                    if ((Boolean)this.gcMultiRow1.ColumnHeaders[0].Cells["表示順checkBoxCell"].Value == false)
                    {
                        プレビューform.rptName = "バックオーダーリストCrystalReport";
                    }
                    else
                    {
                        プレビューform.rptName = "バックオーダーリスト_商品順CrystalReport";
                    }

                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "バックオーダーリスト");
                }
            }
            if (e.CellName == "印刷buttonCell")
            {
                if (createReportData() == 0)
                {
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["表示順checkBoxCell"].Value.Equals(0))
                    {
                        バックオーダーリストCrystalReport cr = new バックオーダーリストCrystalReport();
                        cr.SetDataSource(dataSet.Tables[0]);
                        cr.PrintToPrinter(0, false, 0, 0);
                    }
                    else
                    {
                        バックオーダーリスト_商品順CrystalReport cr = new バックオーダーリスト_商品順CrystalReport();
                        cr.SetDataSource(dataSet.Tables[0]);
                        cr.PrintToPrinter(0, false, 0, 0);
                    }
                }
                else
                {
                    MessageBox.Show("データがありません", "バックオーダーリスト");
                }
            }
        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            if (e.CellName == "部課コード始gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード始gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード始gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["部課名始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード始gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "部課名始gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["部課名始gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["部課名始gcComboBoxCell"].Value;
                }
            }
            if (e.CellName == "部課コード終gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード終gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード終gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["部課名終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード終gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "部課名終gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["部課名終gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["部課名終gcComboBoxCell"].Value;
                }
            }

            if (e.CellName == "担当者コード始gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "担当者名始gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value;
                }
            }
            if (e.CellName == "担当者コード終gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "担当者名終gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value;
                }
            }

            if (e.CellName == "メーカーコード始gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "メーカー名始gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名始gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名始gcComboBoxCell"].Value;
                }
            }
            if (e.CellName == "メーカーコード終gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "メーカー名終gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名終gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名終gcComboBoxCell"].Value;
                }
            }
        }

        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F3:
                    /*
                    target = this.ButtonF3;
                    String cname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (cname == "商品コード始gcTextBoxCell")
                    {
                        //得意先検索Form jform = new 得意先検索Form();
                        //jform.Show();
                        fsSyohin.Show();
                    }
                    else if (cname == "得意先始gcTextBoxCell")
                    {
                        //得意先検索Form jform = new 得意先検索Form();
                        //jform.Show();
                        fsToksaki.Show();
                    }
                    */
                    break;
                default:
                    break;
            }

        }

        public string ReceiveDataToksaki
        {
            set
            {
                receiveDataToksaki = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value = receiveDataToksaki.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataToksaki;
            }
        }

        public string ReceiveDataSyohin
        {
            set
            {
                receiveDataSyohin = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value = receiveDataSyohin.ToString();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataSyohin;
            }
        }
        
        private int createReportData()
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
            command.CommandText = "バックオーダーリスト";

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != "*" && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@担当者始", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@担当者始", DBNull.Value);
                command.Parameters.AddWithValue("@担当者始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != "*" && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@担当者終", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@担当者終", DBNull.Value);
                command.Parameters.AddWithValue("@担当者終", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始gcTextBoxCell"].Value != "*" && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先始", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@得意先始", DBNull.Value);
                command.Parameters.AddWithValue("@得意先始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終gcTextBoxCell"].Value != "*" && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先終", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@得意先終", DBNull.Value);
                command.Parameters.AddWithValue("@得意先終", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value != "*" && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@メーカー始", this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@メーカー始", DBNull.Value);
                command.Parameters.AddWithValue("@メーカー始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value != "*" && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@メーカー終", this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@メーカー終", DBNull.Value);
                command.Parameters.AddWithValue("@メーカー終", "*");
            }
            // 受注日
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注日始textBoxCell"].Value != null && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["受注日始textBoxCell"].Value.ToString() != string.Empty)
            {
                command.Parameters.AddWithValue("@受注日始", this.gcMultiRow1.ColumnHeaders[0].Cells["受注日始textBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@受注日始", DBNull.Value);
                command.Parameters.AddWithValue("@受注日始", "*");
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注日終textBoxCell"].Value != null && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["受注日終textBoxCell"].Value.ToString() != string.Empty)
            {
                command.Parameters.AddWithValue("@受注日終", this.gcMultiRow1.ColumnHeaders[0].Cells["受注日終textBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@受注日終", DBNull.Value);
                command.Parameters.AddWithValue("@受注日終", "*");
            }
            // 受注番号
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号S"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号S"].Value.ToString() != null)
            {
                command.Parameters.AddWithValue("@受注番号S", this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注番号S", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号E"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号E"].Value.ToString() != null)
            {
                command.Parameters.AddWithValue("@受注番号E", this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注番号E", "*");
            }
            // 商品
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value != "*" && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@商品始", this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@商品始", DBNull.Value);
                command.Parameters.AddWithValue("@商品始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value != "*" && 
                this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@商品終", this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@商品終", DBNull.Value);
                command.Parameters.AddWithValue("@商品終", "*");
            }

            if ((Boolean)gcMultiRow1.ColumnHeaders[0].Cells["表示順checkBoxCell"].Value == false)
            {
                command.Parameters.AddWithValue("@表示順", "0");
            }
            else
            {
                command.Parameters.AddWithValue("@表示順", "1");
            }


            if ((Boolean)gcMultiRow1.ColumnHeaders[0].Cells["chk印刷区分"].Value == false)
            {
                command.Parameters.AddWithValue("@印刷区分", "0");
            }
            else
            {
                command.Parameters.AddWithValue("@印刷区分", "1");
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            dataSet = new DataSet();
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

            if (gcMultiRow1.Rows.Count == 0)
            {
                return 1;
            }

            return 0;

        }

        void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "部課名始gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["部課名始gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["部課名始gcComboBoxCell"].Value;
                    }
                    break;
                case "部課名終gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["部課名終gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["部課コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["部課名終gcComboBoxCell"].Value;
                    }
                    break;
                case "担当者名始gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value;
                    }
                    break;
                case "担当者名終gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value;
                    }
                    break;
                case "メーカー名始gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名始gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名始gcComboBoxCell"].Value;
                    }
                    break;
                case "メーカー名終gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名終gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名終gcComboBoxCell"].Value;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "部課コード始gcTextBoxCell":
                    if (e.FormattedValue != null && e.FormattedValue.ToString() != "*")
                    {
                        if (Utility.existCombo(comboBoxCell01, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        //else
                        //{
                        //    MessageBox.Show("マスタに存在しません。", "仕入先別発注残明細表");
                        //    e.Cancel = true;
                        //}
                    }
                    break;
                case "部課コード終gcTextBoxCell":
                    if (e.FormattedValue != null && e.FormattedValue.ToString() != "*")
                    {
                        if (Utility.existCombo(comboBoxCell02, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        //else
                        //{
                        //    MessageBox.Show("マスタに存在しません。", "仕入先別発注残明細表");
                        //    e.Cancel = true;
                        //}
                    }
                    break;
                case "担当者コード始gcTextBoxCell":
                    if (e.FormattedValue != null && e.FormattedValue.ToString() != "*")
                    {
                        if (Utility.existCombo(comboBoxCell03, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        //else
                        //{
                        //    MessageBox.Show("マスタに存在しません。", "仕入先別発注残明細表");
                        //    e.Cancel = true;
                        //}
                    }
                    break;
                case "担当者コード終gcTextBoxCell":
                    if (e.FormattedValue != null && e.FormattedValue.ToString() != "*")
                    {
                        if (Utility.existCombo(comboBoxCell04, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        //else
                        //{
                        //    MessageBox.Show("マスタに存在しません。", "仕入先別発注残明細表");
                        //    e.Cancel = true;
                        //}
                    }
                    break;
                case "メーカーコード始gcTextBoxCell":
                    if (e.FormattedValue != null && e.FormattedValue.ToString() != "*")
                    {
                        if (Utility.chkCombo("Tメーカーマスタ", "メーカーコード", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value) == 1)
                        {
                            e.Cancel = false;
                        }
                        //else
                        //{
                        //    MessageBox.Show("マスタに存在しません。", "仕入先別発注残明細表");
                        //    e.Cancel = true;
                        //}
                    }
                    break;
                case "メーカーコード終gcTextBoxCell":
                    if (e.FormattedValue != null && e.FormattedValue.ToString() != "*")
                    {
                        if (Utility.chkCombo("Tメーカーマスタ", "メーカーコード", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value) == 1)
                        {
                            e.Cancel = false;
                        }
                        //else
                        //{
                        //    MessageBox.Show("マスタに存在しません。", "仕入先別発注残明細表");
                        //    e.Cancel = true;
                        //}
                    }
                    break;
                case "印刷buttonCell":
                    break;
                case "終了buttonCell":
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "担当者コード始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value =null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value == "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != null )
                    {
                        if (Utility.existCombo(comboBoxCell03, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value;
                        }
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value;
                    }
                    break;
                case "担当者コード終gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != null )
                    {
                        if (Utility.existCombo(comboBoxCell04, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value;
                        }
                    }
                    break;
                case "得意先コード始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始gcTextBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始gcTextBoxCell"].Value;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終gcTextBoxCell"].Value = null;
                    }
                    break;
                case "メーカーコード始gcTextBoxCell":
                    if((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名終gcComboBoxCell"].Value = null;
                    }
                    else if((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value == "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value != null )
                    {
                        if (Utility.chkCombo("Tメーカーマスタ", "メーカーコード", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value) == 1)
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value;
                        }
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value;
                    }
                    break;
                case "メーカーコード終gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value != null)
                    {
                        if (Utility.chkCombo("Tメーカーマスタ", "メーカーコード", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value) == 1)
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー名終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value;
                        }
                    }
                    break;
                case "受注日始textBoxCell":
                    if (gcMultiRow1.CurrentCell.Value != null)
                    {
                        string buf = gcMultiRow1.CurrentCell.Value.ToString();
                        buf = buf.Replace("/", "");
                        buf = Utility.getDate(buf);
                        gcMultiRow1.ColumnHeaders[0].Cells["受注日始textBoxCell"].Value = buf;
                        gcMultiRow1.ColumnHeaders[0].Cells["受注日終textBoxCell"].Value = buf;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["受注日終textBoxCell"].Value = null;
                    }
                    break;
                case "受注日終textBoxCell":
                    if (gcMultiRow1.CurrentCell.Value != null)
                    {
                        string buf = gcMultiRow1.CurrentCell.Value.ToString();
                        buf = buf.Replace("/", "");
                        buf = Utility.getDate(buf);
                        gcMultiRow1.ColumnHeaders[0].Cells["受注日終textBoxCell"].Value = buf;
                    }
                    break;
                case "txt受注番号S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号S"].Value != null )
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号E"].Value = gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号S"].Value;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt受注番号E"].Value = null;
                    }
                    break;
                case "商品コード始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value != null )
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value = gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value = null;
                    }
                    break;
            }
        }

        private void gcMultiRow1_CellContentClick_1(object sender, CellEventArgs e)
        {

        }


        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "部課コード始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            break;
        //        case "部課名始gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            break;
        //        case "部課コード終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");
        //            }
        //            break;
        //        case "部課名終gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");
        //            }
        //            break;
        //        case "担当者コード始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            break;
        //        case "担当者名始gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            break;
        //        case "担当者コード終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
        //            }
        //            break;
        //        case "担当者名終gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
        //            }
        //            break;
        //        case "得意先コード始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終gcTextBoxCell");
        //            }
        //            break;
        //        case "得意先コード終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            break;
        //        case "メーカーコード始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            break;
        //        case "メーカー名始gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            break;
        //        case "メーカーコード終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日始textBoxCell");
        //            }
        //            break;
        //        case "メーカー名終gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日始textBoxCell");
        //            }
        //            break;
        //        case "受注日始textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日終textBoxCell");
        //            }
        //            break;
        //        case "受注日終textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日始textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始gcTextBoxCell");
        //            }
        //            break;
        //        case "商品コード始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日終textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終gcTextBoxCell");
        //            }
        //            break;
        //        case "商品コード終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始gcTextBoxCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            break;
        //        case "印刷buttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            break;
        //        case "終了buttonCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課コード始gcTextBoxCell");
        //            }
        //            break;
        //    }
        //}

    }
}
