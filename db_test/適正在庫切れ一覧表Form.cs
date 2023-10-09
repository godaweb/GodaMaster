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
    public partial class 適正在庫切れ一覧表Form : Form
    {

        private DataSet dataSet;
        GcComboBoxCell comboBoxCell01 = null;
        GcComboBoxCell comboBoxCell02 = null;
        GcComboBoxCell comboBoxCell03 = null;
        GcComboBoxCell comboBoxCell04 = null;

        public 適正在庫切れ一覧表Form()
        {
            InitializeComponent();
        }

        private void 適正在庫切れ一覧表Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 適正在庫切れ一覧表Template();
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
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);

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
            //this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);

            // 各ファンクションキーに対応するアクションを登録する
            //this.gcMultiRow1.ShortcutKeyManager.Register(new 出荷可能一覧表FunctionKeyAction(Keys.F3), Keys.F3);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // コンボボックス
            comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT 倉庫コード, 倉庫名 FROM T倉庫マスタ ORDER BY 倉庫コード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";
            
            comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 倉庫コード, 倉庫名 FROM T倉庫マスタ ORDER BY 倉庫コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";

            comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT メーカーコード, メーカー名 FROM Tメーカーマスタ ORDER BY メーカーコード");
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.TextSubItemIndex = 0;
            comboBoxCell03.TextSubItemIndex = 1;
            comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell03.TextFormat = "[1]";
            
            comboBoxCell04 = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell04.DataSource = Utility.GetComboBoxData("SELECT メーカーコード, メーカー名 FROM Tメーカーマスタ ORDER BY メーカーコード");
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.TextSubItemIndex = 0;
            comboBoxCell04.TextSubItemIndex = 1;
            comboBoxCell04.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell04.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell04.TextFormat = "[1]";

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value = 0;


            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "倉庫コード始gcTextBoxCell");
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
                                プレビューform.rptName = "適正在庫切れ一覧表CrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "適正在庫切れ一覧表");
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
                                プレビューform.rptName = "適正在庫切れ一覧表CrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "適正在庫切れ一覧表");
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
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value.Equals(0))
                    { // コード
                        プレビューform.rptName = "適正在庫切れ一覧表CrystalReport";
                    }
                    else
                    { // 現在庫の少ない
                        プレビューform.rptName = "適正在庫切れ一覧表_現在庫順CrystalReport";
                    }
                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "適正在庫切れ一覧表");
                }
            }
            if (e.CellName == "印刷buttonCell")
            {
                if (createReportData() == 0)
                {
                    適正在庫切れ一覧表CrystalReport cr = new 適正在庫切れ一覧表CrystalReport();
                    cr.SetDataSource(dataSet.Tables[0]);
                    cr.PrintToPrinter(0, false, 0, 0);
                }
                else
                {
                    MessageBox.Show("データがありません", "適正在庫切れ一覧表");
                }
            }
        }


        private int createReportData()
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
            command.CommandText = "適正在庫切れ一覧表";

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@倉庫始", this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@倉庫始", DBNull.Value);
                command.Parameters.AddWithValue("@倉庫始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@倉庫終", this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@倉庫終", DBNull.Value);
                command.Parameters.AddWithValue("@倉庫終", "*");
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
            else
            {
                gcMultiRow1.DataSource = dataTable;
            }

            return ret;

        }


        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "倉庫コード始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value == "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value;
                        }
                        this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value;
                    }
                    break;
                case "倉庫コード終gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value != null )
                    {
                        if (Utility.existCombo(comboBoxCell01, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value;
                        }
                    }
                    break;
                case "メーカーコード始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value == "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value != "*")
                    {
                        if (Utility.chkCombo("Tメーカーマスタ", "メーカーコード", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value) == 1)
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value;
                        }
                        this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value;
                    }
                    break;
                case "メーカーコード終gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value != null )
                    {
                        if (Utility.chkCombo("Tメーカーマスタ", "メーカーコード", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value) == 1)
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value;
                        }
                    }
                    break;
            }
        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            if (e.CellName == "倉庫コード始gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "倉庫始gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫始gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫始gcComboBoxCell"].Value;
                }
            }
            if (e.CellName == "倉庫コード終gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "倉庫終gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫終gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫終gcComboBoxCell"].Value;
                }
            }

            if (e.CellName == "メーカーコード始gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "メーカー始gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード始gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー始gcComboBoxCell"].Value;
                }
            }
            if (e.CellName == "メーカーコード終gcTextBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value != "*")
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value;
                }
            }
            if (e.CellName == "メーカー終gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["メーカーコード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["メーカー終gcComboBoxCell"].Value;
                }
            }
        }

        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "倉庫コード始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            break;
        //        case "倉庫始gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            break;
        //        case "倉庫コード終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            break;
        //        case "倉庫終gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            break;
        //        case "メーカー始gcComboBoxCell":
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            break;
        //        case "メーカー終gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコード終gcTextBoxCell");
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
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード始gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "倉庫コード始gcTextBoxCell");
        //            }
        //            break;
        //    }
        //}


    }
}
