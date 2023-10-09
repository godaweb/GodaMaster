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
    public partial class 出荷指示一覧表Form : Form
    {
        private string receiveDataToksaki = "";
        得意先検索Form fsToksaki;
        private DataSet dataSet;
        GcComboBoxCell comboBoxCell00 = null;
        GcComboBoxCell comboBoxCell01 = null;
        GcComboBoxCell comboBoxCell02 = null;
        GcComboBoxCell comboBoxCell03 = null;
        GcComboBoxCell comboBoxCell04 = null;
        GcComboBoxCell comboBoxCell05 = null;
        GcComboBoxCell comboBoxCell06 = null;
        GcComboBoxCell comboBoxCell07 = null;
        
        int rowcnt = 0;
        int fireKbn = 1;

        public 出荷指示一覧表Form()
        {
            InitializeComponent();
            fsToksaki = new 得意先検索Form();
            fsToksaki.Owner = this;
        }

        private void 出荷指示一覧表Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 出荷指示一覧表Template();
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
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            //gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            //gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            
            
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
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellInRow, Keys.Up);
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
            comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";
            
            comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.TextSubItemIndex = 0;
            comboBoxCell03.TextSubItemIndex = 1;
            comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell03.TextFormat = "[1]";

            comboBoxCell04 = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell04.DataSource = Utility.GetComboBoxData("SELECT オペレーターコード, オペレーター名 FROM Tオペレーターマスタ ORDER BY オペレーターコード");
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.TextSubItemIndex = 0;
            comboBoxCell04.TextSubItemIndex = 1;
            comboBoxCell04.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell04.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell04.TextFormat = "[1]";
            
            comboBoxCell05 = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell05.DataSource = Utility.GetComboBoxData("SELECT オペレーターコード, オペレーター名 FROM Tオペレーターマスタ ORDER BY オペレーターコード");
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.TextSubItemIndex = 0;
            comboBoxCell05.TextSubItemIndex = 1;
            comboBoxCell05.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell05.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell05.TextFormat = "[1]";
            
            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["出力条件gcTextBoxCell"].Value = 0;
            //gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value = "*";
            //gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["受注NO始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["受注NO終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["棚番始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["棚番終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value = "*";

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "出力条件gcTextBoxCell");

        }


        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "終了buttonCell")
            {
                this.Hide();
            }
            if (e.CellName == "プレビューbuttonCell")
            {
                // データチェック
                if (mCheck() == 1)
                {
                    return;
                }

                if (createReportData() == 0)
                {
                    プレビューForm プレビューform = new プレビューForm();

                    プレビューform.dataSet = dataSet;
                    プレビューform.rptName = "出荷指示一覧表CrystalReport";
                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "出荷指示一覧表");
                }

            }
            if (e.CellName == "印刷buttonCell")
            {
                // データチェック
                if (mCheck() == 1)
                {
                    return;
                }

                if (createReportData() == 0)
                {
                    出荷指示一覧表CrystalReport cr = new 出荷指示一覧表CrystalReport();
                    cr.SetDataSource(dataSet.Tables[0]);
                    cr.PrintToPrinter(0, false, 0, 0);
                }
                else
                {
                    MessageBox.Show("データがありません", "出荷指示一覧表");
                }
            }
        }


        // データチェック
        private int mCheck()
        {

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value == null & this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value == null)
            {
                MessageBox.Show("担当者コードを入力してください。", "出荷指示一覧表");
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
                return 1;
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value == null )
            {
                MessageBox.Show("担当者コードを入力してください。", "出荷指示一覧表");
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
                return 1;
            }
            //if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value == null)
            //{
            //    MessageBox.Show("担当者コードを入力してください。", "出荷指示一覧表");
            //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "担当者gcTextBoxCell");
            //    return 1;
            //}

            return 0;
        }

        private int createReportData()
        {
            int ret = 0;

            string connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;
            // ストアド プロシージャ名を指定
            command.CommandText = "出荷指示一覧表";
                
            command.Parameters.AddWithValue("@出力条件", this.gcMultiRow1.ColumnHeaders[0].Cells["出力条件gcTextBoxCell"].Value);
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value !=null)
            {
                command.Parameters.AddWithValue("@担当者始", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@担当者始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value !=null)
            {
                command.Parameters.AddWithValue("@担当者終", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@担当者終", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value !=null)
            {
                command.Parameters.AddWithValue("@オペレーター始", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@オペレーター始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value !=null) 
            {
                command.Parameters.AddWithValue("@オペレーター終", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@オペレーター終", "*");
            }
            // 得意先
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value !=null)
            {
                command.Parameters.AddWithValue("@得意先始", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先終gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先終gcTextBoxCell"].Value !=null) 
            {
                command.Parameters.AddWithValue("@得意先終", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先終", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始gcTextBoxCell"].Value !=null)
            {
                command.Parameters.AddWithValue("@受注NO始", this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注NO始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終gcTextBoxCell"].Value !=null)
            {
                command.Parameters.AddWithValue("@受注NO終", this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注NO終", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcTextBoxCell"].Value !=null)
            {
                command.Parameters.AddWithValue("@メーカー始", this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@メーカー始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcTextBoxCell"].Value !=null)
            {
                command.Parameters.AddWithValue("@メーカー終", this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@メーカー終", "*");
            }
            // 受注日
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value != null)
            {
                command.Parameters.AddWithValue("@受注日S", this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注日S", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value != null)
            {
                command.Parameters.AddWithValue("@受注日E", this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@受注日E", "*");
            }
            // 商品
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value != null)
            {
                command.Parameters.AddWithValue("@商品S", this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品S", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value != null)
            {
                command.Parameters.AddWithValue("@商品E", this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品E", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["棚番始gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["棚番始gcTextBoxCell"].Value !=null) 
            {
                command.Parameters.AddWithValue("@棚番始", this.gcMultiRow1.ColumnHeaders[0].Cells["棚番始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@棚番始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["棚番終gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["棚番終gcTextBoxCell"].Value !=null)
            {
                command.Parameters.AddWithValue("@棚番終", this.gcMultiRow1.ColumnHeaders[0].Cells["棚番終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@棚番終", "*");
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);

            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            //dataTable.AcceptChanges();
            if (dataTable.Rows.Count <= 0)
            {
                ret = 1;
            }

            return ret;
        }


        void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "担当者始gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value;
                    }
                    break;
                case "担当者終gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value;
                    }
                    break;
                case "オペレーター始gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcComboBoxCell"].Value;
                    }
                    break;
                case "オペレーター終gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcComboBoxCell"].Value;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "在庫場所始gcTextBoxCell":
                    if (e.FormattedValue != null && e.FormattedValue.ToString() != "*")
                    {
                        if (Utility.existCombo(comboBoxCell00, e.FormattedValue.ToString()))
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
                case "担当者始gcTextBoxCell":
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
                case "担当者終gcTextBoxCell":
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
                case "オペレーター始gcTextBoxCell":
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
                case "オペレーター終gcTextBoxCell":
                    if (e.FormattedValue != null && e.FormattedValue.ToString() != "*")
                    {
                        if (Utility.existCombo(comboBoxCell05, e.FormattedValue.ToString()))
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
                case "担当者始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value == "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell02, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value;
                        }

                        // to
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value;
                    }
                    break;
                case "担当者終gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value != null )
                    {
                        if (Utility.existCombo(comboBoxCell03, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value;
                        }
                    }
                    break;
                case "オペレーター始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value != null )
                    {
                        if (Utility.existCombo(comboBoxCell04, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value;
                        }

                        // to
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcComboBoxCell"].Value = null;
                    }
                    break;
                case "オペレーター終gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value != null )
                    {
                        if (Utility.existCombo(comboBoxCell05, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value;
                        }
                    }
                    break;
                case "得意先始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value != null )
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得意先終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得意先終gcTextBoxCell"].Value = null;
                    }
                    break;
                case "受注NO始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始gcTextBoxCell"].Value != null )
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始gcTextBoxCell"].Value;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終gcTextBoxCell"].Value = null;
                    }
                    break;
                case "メーカー始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcTextBoxCell"].Value != null )
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcTextBoxCell"].Value;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcTextBoxCell"].Value = null;
                    }
                    break;
                case "txt受注日S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value != null )
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
                        }

                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日S"].Value;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value = null;
                    }
                    break;
                case "txt受注日E":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt受注日E"].Value != null )
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
                case "txt商品S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value != null )
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品S"].Value;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["txt商品E"].Value =null;
                    }
                    break;
                case "棚番始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["棚番始gcTextBoxCell"].Value != null )
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["棚番終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["棚番始gcTextBoxCell"].Value;
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["棚番終gcTextBoxCell"].Value = null;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            if (e.CellName == "担当者始gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "担当者始gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"].Value;
                }
            }
            if (e.CellName == "担当者終gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "担当者終gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"].Value;
                }
            }

            if (e.CellName == "オペレーター始gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "オペレーター始gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcComboBoxCell"].Value;
                }
            }
            if (e.CellName == "オペレーター終gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "オペレーター終gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcComboBoxCell"].Value;
                }
            }
        }



        void GcMultiRow_EndPreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
            if (fireKbn.Equals(1))
            {
                fireKbn = 0;
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "出力条件gcTextBoxCell");
                        break;
                    case Keys.Up:
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "印刷buttonCell");
                        break;
                    case Keys.Enter:
                        this.Hide();
                        break;
                }
            }
            else
            {
                fireKbn = 1;
            }
        }

        void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            //TextBoxEditingControl textBox = e.Control as TextBoxEditingControl;
            //if (textBox != null)
            //{
            //    textBox.KeyDown -= new KeyEventHandler(textBox_KeyDown);
            //    textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            //}
            // 編集用コントロールのKeyDownイベントの検出
            e.Control.KeyDown -= editor_KeyDown;
            e.Control.KeyDown += editor_KeyDown;
        }

        private void editor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // キー操作による動作の実装
            //if (e.Control)
            //{
            switch (e.KeyCode)
            {
                case Keys.Down:
                    e.SuppressKeyPress = true;
                    SelectionActions.MoveToNextCell.Execute(((IEditingControl)sender).GcMultiRow);
                    break;
                case Keys.Up:
                    e.SuppressKeyPress = true;
                    SelectionActions.MoveToPreviousCell.Execute(((IEditingControl)sender).GcMultiRow);
                    break;
                case Keys.Left:
                    e.SuppressKeyPress = true;
                    SelectionActions.MoveLeft.Execute(((IEditingControl)sender).GcMultiRow);
                    break;
                case Keys.Right:
                    e.SuppressKeyPress = true;
                    SelectionActions.MoveRight.Execute(((IEditingControl)sender).GcMultiRow);
                    break;
            }
            //}
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
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
                    if (cname == "得意先始gcTextBoxCell")
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

        private void 終了button_Click(object sender, EventArgs e)
        {
            this.Hide();

        }

        void GcMultiRow_PrintPreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
            //if (fireKbn.Equals(1))
            //{
            //    fireKbn = 0;
            //    switch (e.KeyCode)
            //    {
            //        case Keys.Down:
            //            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "終了buttonCell");
            //            break;
            //        case Keys.Up:
            //            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "プレビューbuttonCell");
            //            break;
            //        case Keys.Enter:
            //            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            //            SqlConnection connection = new SqlConnection(connectionString);
            //            // コネクションを開く
            //            connection.Open();
            //            // コマンド作成
            //            SqlCommand command = connection.CreateCommand();
            //            // ストアド プロシージャを指定
            //            command.CommandType = CommandType.StoredProcedure;
            //            // ストアド プロシージャ名を指定
            //            command.CommandText = "出荷指示一覧表";

            //            command.Parameters.AddWithValue("@出力条件", this.gcMultiRow1.ColumnHeaders[0].Cells["出力条件gcTextBoxCell"].Value);
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["在庫場所始gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["在庫場所始gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@在庫場所始", this.gcMultiRow1.ColumnHeaders[0].Cells["在庫場所始gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@在庫場所始", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@担当者始", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@担当者始", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@担当者終", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@担当者終", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@オペレーター始", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@オペレーター始", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@オペレーター終", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@オペレーター終", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["部課始gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["部課始gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@部課始", this.gcMultiRow1.ColumnHeaders[0].Cells["部課始gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@部課始", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@得意先始", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先始gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@得意先始", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先終gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先終gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@得意先終", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先終gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@得意先終", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@受注NO始", this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO始gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@受注NO始", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@受注NO終", this.gcMultiRow1.ColumnHeaders[0].Cells["受注NO終gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@受注NO終", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@メーカー始", this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@メーカー始", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@メーカー終", this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@メーカー終", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["棚番始gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["棚番始gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@棚番始", this.gcMultiRow1.ColumnHeaders[0].Cells["棚番始gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@棚番始", "*");
            //            }
            //            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["棚番終gcTextBoxCell"].Value != "*" &&
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["棚番終gcTextBoxCell"].Value != null)
            //            {
            //                command.Parameters.AddWithValue("@棚番終", this.gcMultiRow1.ColumnHeaders[0].Cells["棚番終gcTextBoxCell"].Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@棚番終", "*");
            //            }

            //            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            //            DataSet dataSet = new DataSet();
            //            SqlDataAdapter adapter = new SqlDataAdapter(command);
            //            adapter.Fill(dataSet);

            //            //DataTableオブジェクトを用意
            //            DataTable dataTable = dataSet.Tables[0];

            //            //dataTable.AcceptChanges();

            //            プレビューForm プレビューform = new プレビューForm();

            //            プレビューform.dataSet = dataSet;
            //            プレビューform.rptName = "出荷指示一覧表CrystalReport";
            //            プレビューform.Show();
            //            break;
            //    }
            //}
            //else
            //{
            //    fireKbn = 1;
            //}
        }

        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "出力条件gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所始gcTextBoxCell");
        //            }
        //            break;
        //        case "在庫場所始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力条件gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力条件gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            break;
        //        case "在庫場所始gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            break;
        //        case "在庫場所終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            break;
        //        case "在庫場所終gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            break;
        //        case "担当者始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "在庫場所終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者終gcTextBoxCell");
        //            }
        //            break;
        //        case "担当者始gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者終gcTextBoxCell");
        //            }
        //            break;
        //        case "担当者終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            break;
        //        case "担当者終gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            break;
        //        case "オペレーター始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            break;
        //        case "オペレーター始gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            break;
        //        case "オペレーター終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課始gcTextBoxCell");
        //            }
        //            break;
        //        case "オペレーター終gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課始gcTextBoxCell");
        //            }
        //            break;
        //        case "部課始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            break;
        //        case "部課始gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーター終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            break;
        //        case "部課終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先始gcTextBoxCell");
        //            }
        //            break;
        //        case "部課終gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先始gcTextBoxCell");
        //            }
        //            break;
        //        case "得意先始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "部課終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先終gcTextBoxCell");
        //            }
        //            break;
        //        case "得意先終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始gcTextBoxCell");
        //            }
        //            break;
        //        case "受注NO始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終gcTextBoxCell");
        //            }
        //            break;
        //        case "受注NO終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー始gcTextBoxCell");
        //            }
        //            break;
        //        case "メーカー始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注NO終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー終gcTextBoxCell");
        //            }
        //            break;
        //        case "メーカー終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番始gcTextBoxCell");
        //            }
        //            break;
        //        case "棚番始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番終gcTextBoxCell");
        //            }
        //            break;
        //        case "棚番終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番始gcTextBoxCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "棚番終gcTextBoxCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力条件gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力条件gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力条件gcTextBoxCell");
        //            }
        //            break;
        //    }
        //}
    }
}
