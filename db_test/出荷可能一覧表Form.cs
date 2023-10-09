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
    public partial class 出荷可能一覧表Form : Form
    {
        private string receiveDataSyohin = "";
        private string receiveDataToksaki = "";
      
        //商品検索Form fsSyohin;
        private DataSet dataSet;
        GcComboBoxCell comboBoxCell01 = null;
        GcComboBoxCell comboBoxCell02 = null;
        int radioFlg = 0;
        int rowcnt = 0;

        public 出荷可能一覧表Form()
        {
            InitializeComponent();
            //fsSyohin = new 商品検索Form();
            //fsSyohin.Owner = this;
        }

        // Form_Load
        private void 出荷可能一覧表Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 出荷可能一覧表Template();
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
            this.gcMultiRow1.ShortcutKeyManager.Register(new 出荷可能一覧表FunctionKeyAction(Keys.F3), Keys.F3);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // コンボボックス
            comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";
            comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";

            // 初期表示
            DateTime  dtmTmp = DateTime.Today;

            if ((int)dtmTmp.DayOfWeek == 0)
            { // 日曜日の場合
                gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value = dtmTmp.AddDays(-2).ToString("yyyy/MM/dd");
            }
            else
            {
                gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value = dtmTmp.AddDays(-1).ToString("yyyy/MM/dd");
            }

            gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value = DateTime.Today.ToString("yyyy/MM/dd");

            gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードS"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードE"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value = 1;

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "txt入荷日S");
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
                    {
                        プレビューform.rptName = "出荷可能一覧表CrystalReport";
                    }
                    else
                    {
                        プレビューform.rptName = "出荷可能一覧表_得意先順CrystalReport";
                    }

                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "出荷可能一覧表");
                }
            }
            if (e.CellName == "印刷buttonCell")
            {
                if (createReportData() == 0)
                {
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["表示順radioGroupCell"].Value.Equals(0))
                    {
                        出荷可能一覧表CrystalReport cr = new 出荷可能一覧表CrystalReport();
                        cr.SetDataSource(dataSet.Tables[0]);
                        cr.PrintToPrinter(0, false, 0, 0);
                    }
                    else
                    {
                        出荷可能一覧表_得意先順CrystalReport cr = new 出荷可能一覧表_得意先順CrystalReport();
                        cr.SetDataSource(dataSet.Tables[0]);
                        cr.PrintToPrinter(0, false, 0, 0);
                    }
                }
                else
                {
                    MessageBox.Show("データがありません", "出荷可能一覧表");
                }
            }
        }

        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F3:
                    target = this.ButtonF3;
                    String cname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (cname == "txt得意先コードS" || cname == "txt得意先コードE")
                    {
                        得意先検索Form fsToksaki = new 得意先検索Form();
                        fsToksaki = new 得意先検索Form();
                        fsToksaki.Owner = this;
                        fsToksaki.Show();
                    }
                    else if (cname == "txt商品コードS" || cname == "txt商品コードE")
                    {
                        商品検索Form fsSyohin = new 商品検索Form();
                        fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                    }
                    break;
                default:
                    throw new NotSupportedException();
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
        public string ReceiveDataToksaki
        {
            set
            {
                receiveDataToksaki = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["検索得意先textBoxCell"].Value = receiveDataToksaki.ToString();
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
            command.CommandText = "出荷可能一覧表";

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value != null)
            {
                //DateTime nyukaDate = DateTime.Parse(gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value.ToString());
                command.Parameters.AddWithValue("@入荷日", this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value);
            }
            else
            {
//                command.Parameters.AddWithValue("@入荷日", DBNull.Value);
                command.Parameters.AddWithValue("@入荷日", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value != null)
            {
                command.Parameters.AddWithValue("@入荷日E", this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@入荷日E", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@担当者始", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@担当者始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@担当者終", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@担当者終", "*");
            }
            // 得意先
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードS"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードS"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先S", this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードS"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先S", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードE"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードE"].Value != null)
            {
                command.Parameters.AddWithValue("@得意先E", this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードE"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先E", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@商品始", this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@商品終", this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終gcTextBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@商品終", "*");
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



        // 検証後
        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                //case "txt入荷日S":
                //    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value != "*")
                //    {
                //        string buf = gcMultiRow1.CurrentCell.Value.ToString();
                //        buf = buf.Replace("/", "");
                //        buf = Utility.getDate(buf);
                //        gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value = buf;
                //        gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value = buf;
                //    }
                //    else
                //    {
                //        gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value = null;
                //    }
                //    break;
                case "txt入荷日E":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value != "*")
                    {
                        string buf = gcMultiRow1.CurrentCell.Value.ToString();
                        buf = buf.Replace("/", "");
                        buf = Utility.getDate(buf);
                        gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value = buf;
                    }
                    break;
                case "担当者コード始gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value == null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value == "*")
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value = null;
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value;
                        }
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value;
                    }
                    break;
                case "担当者コード終gcTextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != null )
                    {
                        if (Utility.existCombo(comboBoxCell02, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value;
                        }
                    }
                    break;
                case "txt得意先コードS":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードS"].Value != null)
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードE"].Value = gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードS"].Value;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt得意先コードE"].Value = null;
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

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            string buf;
                    
            //switch (e.CellName)
            //{
            //    case "担当者コード始gcTextBoxCell":
            //        if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value != "*")
            //        {
            //            if (Utility.existCombo(comboBoxCell01, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value))
            //            {
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名始gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード始gcTextBoxCell"].Value;
            //            }
            //        }
            //        break;
            //    case "担当者コード終gcTextBoxCell":
            //        if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value != "*")
            //        {
            //            if (Utility.existCombo(comboBoxCell02, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value))
            //            {
            //                this.gcMultiRow1.ColumnHeaders[0].Cells["担当者名終gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コード終gcTextBoxCell"].Value;
            //            }
            //        }
            //        break;
            //    case "txt入荷日S":
            //        if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value != "*")
            //        {
            //            buf = gcMultiRow1.CurrentCell.Value.ToString();
            //            buf = buf.Replace("/", "");
            //            buf = Utility.getDate(buf);
            //            gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value = buf;
            //        }
            //        break;
            //    case "txt入荷日E":
            //        if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value != "*")
            //        {
            //            buf = gcMultiRow1.CurrentCell.Value.ToString();
            //            buf = buf.Replace("/", "");
            //            buf = Utility.getDate(buf);
            //            gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value = buf;
            //        }
            //        break;
            //}
        }
        void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "txt入荷日S":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value != "*")
                    {
                        string buf = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value;
                        buf = buf.Replace("/", "");
                        buf = Utility.getDate(buf);
                        gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日S"].Value = buf;
                        gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value = buf;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["txt入荷日E"].Value = null;
                    }
                    break;
            }
        }
        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
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

        }
        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "担当者コード始gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード終gcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt入荷日E");
        //            }
        //            break;
        //        case "担当者コード終gcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txt得意先コードS");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コード始gcTextBoxCell");
        //            }

        //            break;
        //    }
        //}

    }
}
