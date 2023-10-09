using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;
using System.Data.SqlClient;
using GrapeCity.Win.MultiRow.InputMan;
using db_test.受注ファイルDataSetTableAdapters;
using db_test.SPEEDDB管理DataSetTableAdapters;
using db_test.SPEEDDBVIEWDataSetTableAdapters;

namespace db_test
{
    public partial class 売上計上入力Form : Form
    {
        private int forceFlg = 0;

        //商品検索Form fsSyohin;
        private string receiveDataSyohin = "";
        //得意先検索Form fsToksaki;
        private string receiveDataToksaki = "";
        //売上伝票番号検索連携用
        private string receiveDataUriageDenpyo = "";
        //受注番号検索連携用
        private string receiveDataJyutyuDenpyo = "";

        //ドラスタ_ヘッダ連携用
        private bool ChangeSkipFlag = false;
        private string ドラスタ区分 = null;
        private string ドラスタ_ヘッダ_社コード = null;
        private string ドラスタ_ヘッダ_経費区分 = null;
        private string ドラスタ_ヘッダ_直送区分 = null;
        private string ドラスタ_ヘッダ_ＥＯＳ区分 = null;
        private string ドラスタ_ヘッダ_客注区分 = null;
        private string receiveドラスタ_ヘッダ_Data = null;
        private string sendドラスタ_ヘッダ_Data = null;
        private string[] arr = null;
        private string ドラスタ_欠品理由_欠品理由コード = null;
        private string receiveドラスタ_欠品理由_Data = null;
        private string sendドラスタ_欠品理由_Data = null;
        private string ドラスタ_明細_大分類コード = null;
        private string receiveドラスタ_明細_Data = null;
        private string sendドラスタ_明細_Data = null;

        private string M得意先名 = null;
        private string M得意先コード = null;
        private string M事業所コード = null;
        private string OPE事業所コード = null;
        private string M担当者コード = null;
        private string M部課コード = null;
        private string Mランク = null;
        private string M売上切捨区分 = null;
        private string M売上税区分 = null;
        private string M請求先コード = null;
        private string M諸口区分 = null;
        private string M掛率 = null;
        private string 取引先コード = null;
        private string 発注区分 = null;
        private string 店コード = null;
        private string 店コードB = null;
        private string 納入先コード = null;
        private string 納名 = null;

        private string 大分類コード = null;

        private string ＪＡＮコード = null;
        private string ＥＯＳ商品コード = null;

        private DataSet dataSet;
        DataTable dataTable = null;

        private 受注ファイルDataSet 受注ファイルdataSet;  // データセット
        private T受注ファイルTableAdapter T受注ファイルtableAdapter;  // Userテーブルアダプタ
        private T受注戻しファイルTableAdapter T受注戻しファイルtableAdapter;  // Userテーブルアダプタ
        private T処理履歴テーブルTableAdapter T処理履歴テーブルtableAdapter;  // Userテーブルアダプタ
        private SPEEDDB管理DataSet SPEEDDB管理dataSet;
        private vw_ShohinTableAdapter vw_ShohintableAdapter;
        private vw_ShoTourokuTableAdapter vw_ShoTourokutableAdapter;
        private vw_TokuisakiTableAdapter vw_TokuisakitableAdapter;
        private vw_Ryakumei_TokTableAdapter vw_Ryakumei_ToktableAdapter;
        private vw_TokTourokuTableAdapter vw_TokTourokutableAdapter;
        private vw_SiiresakiTableAdapter vw_SiiresakitableAdapter;
        private T会社基本TableAdapter T会社基本tableAdapter;
        private T商品変換マスタTableAdapter T商品変換マスタtableAdapter;

        GcComboBoxCell comboBoxCell01 = null;
        GcComboBoxCell comboBoxCell02 = null;
        GcComboBoxCell comboBoxCell03 = null;
        GcComboBoxCell comboBoxCell04 = null;
        GcComboBoxCell comboBoxCell05 = null;
        GcComboBoxCell comboBoxCell06 = null;
        GcComboBoxCell comboBoxCell07 = null;

        private int beforeIndex = 0;
        private int currentIndex = 0;

        public 売上計上入力Form()
        {
            InitializeComponent();
        }

        private void 売上計上入力Form_Load(object sender, EventArgs e)
        {
            //this.w受注ファイルTableAdapter1.Fill(this.w受注ファイルDataSet1.W受注ファイル);

            this.gcMultiRow1.Template = new 売上計上入力Template();
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

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F2);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 売上計上入力FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 売上計上入力FunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 売上計上入力FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 売上計上入力FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 売上計上入力FunctionKeyAction(Keys.F10), Keys.F10);

            //gcMultiRow1.EditMode = EditMode.EditOnShortcutKey;
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Tab);

            // イベント
            gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            //gcMultiRow1.EditingControlShowing+=new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "処理区分textBoxCell");
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            //gcMultiRow1.CellEditedFormattedValueChanged += new EventHandler<CellEditedFormattedValueChangedEventArgs>(gcMultiRow1_CellEditedFormattedValueChanged);
            //SendKeys.Send("{TAB}");
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //gcMultiRow1.CellLeave += new EventHandler<CellEventArgs>(gcMultiRow1_CellLeave);
            //gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            //gcMultiRow1.CellParsing += new EventHandler<CellParsingEventArgs>(gcMultiRow1_CellParsing);            
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);            
            //次のタブオーダーのコントロールにフォーカスを移動させる
            //Shiftキーが押されている時は、逆順にする
            //this.SelectNextControl(this.ActiveControl,
            //    ((keyData & Keys.Shift) != Keys.Shift), true, true, true);

//            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "処理区分textBoxCell");
//            gcMultiRow1.FirstDisplayedCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "処理区分textBoxCell");

            comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT オペレーターコード, オペレーター名 FROM Tオペレーターマスタ ORDER BY オペレーターコード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";

            comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";

            comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 売上区分コード, 売上区分名 FROM T売上区分マスタ WHERE システム区分=101 ORDER BY 売上区分コード");
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.TextSubItemIndex = 0;
            comboBoxCell03.TextSubItemIndex = 1;
            comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell03.TextFormat = "[1]";

            comboBoxCell04 = this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell04.DataSource = Utility.GetComboBoxData("SELECT 請求月区分コード, 請求月区分名 FROM T請求月区分マスタ");
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.TextSubItemIndex = 0;
            comboBoxCell04.TextSubItemIndex = 1;
            comboBoxCell04.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell04.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell04.TextFormat = "[1]";

            // 1;通常;2;返品;3;値引
            comboBoxCell05 = this.gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell05.DataSource = Utility.GetComboBoxData("SELECT 売上区分コード, 売上区分名 FROM T売上区分マスタ WHERE システム区分=101 ORDER BY 売上区分コード");
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.TextSubItemIndex = 0;
            comboBoxCell05.TextSubItemIndex = 1;
            comboBoxCell05.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell05.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell05.TextFormat = "[1]";

            comboBoxCell06 = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell06.DataSource = Utility.GetComboBoxData("SELECT 倉庫コード, 倉庫名 FROM T倉庫マスタ ORDER BY 倉庫コード");
            comboBoxCell06.ListHeaderPane.Visible = false;
            comboBoxCell06.ListHeaderPane.Visible = false;
            comboBoxCell06.ListHeaderPane.Visible = false;
            comboBoxCell06.TextSubItemIndex = 0;
            comboBoxCell06.TextSubItemIndex = 1;
            comboBoxCell06.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell06.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell06.TextFormat = "[1]";

            /*
            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;
            // ストアド プロシージャ名を指定
            command.CommandText = "オペレーターマスタ選択";
            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);
            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];
            //dataTable.AcceptChanges();
            //this.gcMultiRow1.DataSource = _ShakaiHokenSample.Tables[tableEmployee];
            */

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Default;

            // 交互行のスタイルを設定
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(239, 255, 250);
            // ユーザーによるリサイズを許可しない
            this.gcMultiRow1.AllowUserToResize = false;

            gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value = DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day;
            forceFlg = 1;
            gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value = "000000";
            gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value = "000000";
            gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = "1";
            gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = "1";
            gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Value = "1";
            gcMultiRow1.ColumnHeaders[0].Cells["請求月区分gcComboBoxCell"].Value = "1";

            gcMultiRow1.ColumnFooters[0].Cells["labelCell23"].Visible = false;
            gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Visible=false;
            gcMultiRow1.ColumnFooters[0].Cells["labelCell24"].Visible = false;
            gcMultiRow1.ColumnFooters[0].Cells["仕入先備考textBoxCell"].Visible = false;
            //gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Enabled = false;
            //gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分gcComboBoxCell"].Enabled = false;
            ItemLock(0);

            forceFlg = 0;

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "処理区分textBoxCell");

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "終了buttonCell")
            {
                forceFlg = 1;
                this.Hide();
            }
        }

        void gcMultiRow1_CellParsing(object sender, CellParsingEventArgs e)
        {
            if (forceFlg == 1)
            {
                //forceFlg = 0;
                return;
            }
            switch (e.CellName)
            {
                case "得意先コードtextBoxCell":
                    break;
                case "受注NOtextBoxCell":
                    if (Utility.Nz(gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value, "") != "")
                    {
                        if (gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != null)
                        {
                            string sql = "SELECT 業態コード, 地区コード FROM T得意先マスタ WHERE 得意先コード='";
                            sql = sql + gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString() + "'";
                            DataTable tokTable = Utility.GetData(sql);
                            if (tokTable.Rows.Count > 0)
                            {
                                //if (tokTable.Rows[0]["地区コード"] == "100" & Utility.Nz(tokTable.Rows[0]["業態コード"], "") == "")
                                //{
                                //    gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Enabled = true;
                                //    gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分gcComboBoxCell"].Enabled = true;
                                //}
                                //else
                                //{
                                //    gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Enabled = false;
                                //    gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分gcComboBoxCell"].Enabled = false;
                                //}
                            }
                        }
                    }
                    break;
            }
        }

        void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            if (forceFlg == 1)
            {
                //forceFlg = 0;
                return;
            }
            switch (e.CellName)
            {
                case "売伝NOtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value != null)
                    {
                        string _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value.ToString();
                        int serchcode = Int32.Parse(_serchcode);

                        if (serchcode == 0)
                        {
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                            break;
                        }

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();
                        dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy売上修正戻ファイルfrom売上明細ファイル(serchcode);

                        forceFlg = 1;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value = dataTable.Rows[0]["売上日"].ToString();
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataTable.Rows[0]["得意先コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                        forceFlg = 0;

                        this.gcMultiRow1.DataSource = dataTable;

                        // コントロールの描画を再開する
                        gcMultiRow1.ResumeLayout();

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日textBoxCell");

                        //gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Selected = true;

                        //his.gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Value = 1;

                        //SendKeys.Send("{ENTER}");

                    }
                    break;
                case "得意先コードtextBoxCell":
                    break;
            }            
        }

        void gcMultiRow1_CellLeave(object sender, CellEventArgs e)
        {
            if (forceFlg == 1)
            {
                //forceFlg = 0;
                return;
            }
            switch (e.CellName)
            {
                case "売伝NOtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value != null)
                    {
                        string _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value.ToString();
                        int serchcode = Int32.Parse(_serchcode);

                        if (serchcode == 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Enabled = true;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].ReadOnly = false;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Selectable = true;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Enabled = true;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].ReadOnly = false;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Selectable = true;
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                            break;
                        }

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();
                        dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy売上修正戻ファイルfrom売上明細ファイル(serchcode);

                        forceFlg = 1;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value = dataTable.Rows[0]["売上日"].ToString();
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataTable.Rows[0]["得意先コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                        forceFlg = 0;

                        this.gcMultiRow1.DataSource = dataTable;

                        // コントロールの描画を再開する
                        gcMultiRow1.ResumeLayout();

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日textBoxCell");

                        //gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Selected = true;

                        //his.gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Value = 1;

                        //SendKeys.Send("{ENTER}");

                    }
                    break;
                case "受注NOtextBoxCell":
                    if (Utility.Nz(gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value,"")!="")
                    {
                        if (gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != null)
                        {
                            string sql = "SELECT 業態コード, 地区コード FROM T得意先マスタ WHERE 得意先コード='";
                            sql = sql + gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString() + "'";
                            DataTable tokTable = Utility.GetData(sql);
                            if (tokTable.Rows.Count>0 )
                            {
                                //if (tokTable.Rows[0]["地区コード"] == "100" & Utility.Nz(tokTable.Rows[0]["業態コード"], "") == "")
                                //{
                                //    gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Enabled=true;
                                //    gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分gcComboBoxCell"].Enabled = true;
                                //}else{
                                //    gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Enabled=false;
                                //    gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分gcComboBoxCell"].Enabled = false;
                                //}
                            }
                        }
                    }
                    break;
            }
        }


        void ItemLock(int LockUnlockKBN)
        {
            if (LockUnlockKBN.Equals(0))
            { 
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["請求月区分gcComboBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["倉庫gcComboBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Enabled = true;
                gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Enabled = true;
            }
            else 
            {
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["請求月区分gcComboBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["倉庫gcComboBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Enabled = false;
                gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Enabled = false;
            }


        }

        void deleteproc()
        {
            forceFlg = 1;
            gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Enabled = false;
            gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Enabled = true;
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売伝NOtextBoxCell");
            forceFlg = 0;
            /*
            DoCmd.SetWarnings False
            If (False) Then
                ' ヘッダ部：C_排他＿共通＿削除 ***00/08/02削除
                DoCmd.OpenQuery "C_排他＿共通＿削除", acViewNormal, acEdit
            End If
            If (DCount("売上連番", "W_売上明細Ｃファイル") <> 0) Then
                ' ヘッダ部：削除確認
                Beep
                MsgBox "画面上にデータが残っています。内容を確認してから、再度やり直して下さい。", vbOKOnly, "削除確認"
                ' ヘッダ部：
                DoCmd.CancelEvent
                ' ヘッダ部：
                Exit Function
            End If
            ' ヘッダ部：[Forms]![F_会社基本]![入力区分]に、３セット
            Forms!F_会社基本!入力区分 = 3
            ' ヘッダ部：[受注番号].[Enabled] No
            .受注番号.Enabled = False
            ' ヘッダ部：[受注番号].[Locked] Yes
            .受注番号.Locked = True
            ' ヘッダ部：[売上伝票番号].[Enabled] Yes
            .売上伝票番号.Enabled = True
            ' ヘッダ部：[売上伝票番号].[Locked] No
            .売上伝票番号.Locked = False
            ' ヘッダ部：売上伝票番号
            DoCmd.GoToControl "売上伝票番号"
            */
        }

        void editproc()
        {
            forceFlg = 1;
            gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Enabled = false;
            gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Enabled = true;
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売伝NOtextBoxCell");
            forceFlg = 0;
            /*
            DoCmd.SetWarnings False
            If (False) Then
                ' ヘッダ部：C_排他＿共通＿削除 ***00/08/02削除
                DoCmd.OpenQuery "C_排他＿共通＿削除", acViewNormal, acEdit
            End If
            If (DCount("売上連番", "W_売上明細Ｃファイル") <> 0) Then
                ' ヘッダ部：修正確認
                Beep
                MsgBox "画面上にデータが残っています。内容を確認してから、再度やり直して下さい。", vbOKOnly, "修正確認"
                ' ヘッダ部：
                DoCmd.CancelEvent
                ' ヘッダ部：
                Exit Function
            End If
            ' ヘッダ部：[Forms]![F_会社基本]![入力区分]に、２セット
            Forms!F_会社基本!入力区分 = 2
            ' ヘッダ部：[受注番号].[Enabled] No
            .受注番号.Enabled = False
            ' ヘッダ部：[受注番号].[Locked] Yes
            .受注番号.Locked = True
            ' ヘッダ部：[売上伝票番号].[Enabled] Yes
            .売上伝票番号.Enabled = True
            ' ヘッダ部：[売上伝票番号].[Locked] No
            .売上伝票番号.Locked = False
            ' ヘッダ部：売上伝票番号
            DoCmd.GoToControl "売上伝票番号"
            */
        }

        void newproc()
        {
            forceFlg = 1;
            gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Enabled = true;
            gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Enabled = false;
            gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Enabled = true;
            gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Enabled = true;
            gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = 1;
            gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = 1;
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
            forceFlg = 0;
            /*
            ' ヘッダ部：
            DoCmd.SetWarnings False
            If (False) Then
                ' ヘッダ部：C_排他＿共通＿削除 ***00/08/02削除
                DoCmd.OpenQuery "C_排他＿共通＿削除", acViewNormal, acEdit
            End If
            If (DCount("売上連番", "W_売上明細Ｃファイル") <> 0) Then
                ' ヘッダ部：計上確認
                Beep
                MsgBox "画面上にデータが残っています。内容を確認してから、再度やり直して下さい。", vbOKOnly, "計上確認"
                ' ヘッダ部：
                DoCmd.CancelEvent
                ' ヘッダ部：
                Exit Function
            End If
            ' ヘッダ部：[Forms]![F_会社基本]![入力区分]に、１セット
            Forms!F_会社基本!入力区分 = 1
            ' ヘッダ部：[受注番号].[Enabled] Yes
            .受注番号.Enabled = True
            ' ヘッダ部：[受注番号].[Locked] No
            .受注番号.Locked = False
            ' ヘッダ部：[売上伝票番号].[Enabled] No
            .売上伝票番号.Enabled = False
            ' ヘッダ部：[売上伝票番号].[Locked] Yes
            .売上伝票番号.Locked = True
            ' ヘッダ部：オペレーター番号
            DoCmd.GoToControl "オペレーター番号"
            ' ヘッダ部：[売上区分コード]セット
            .売上区分コード = Forms!F_会社基本!売上区分コード
            ' ヘッダ部：[システム区分]セット
            .システム区分 = Forms!F_会社基本!システム区分
            ' ヘッダ部：[Forms]![F_会社基本]![シス区分]
            Forms!F_会社基本!シス区分 = Forms!F_会社基本!システム区分
            */
        }


        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (forceFlg == 1)
            {
                //forceFlg = 0;
                return;
            }
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "売伝NOtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value != null)
                    {
                        string _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value.ToString();
                        int serchcode = Int32.Parse(_serchcode);

                        if (serchcode == 0)
                        {
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                            break;
                        }
                        
                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();
                        dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy売上修正戻ファイルfrom売上明細ファイル(serchcode);

                        forceFlg = 1;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value = dataTable.Rows[0]["売上日"].ToString();
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataTable.Rows[0]["得意先コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                        forceFlg = 0;

                        this.gcMultiRow1.DataSource = dataTable;

                        // コントロールの描画を再開する
                        gcMultiRow1.ResumeLayout();

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日textBoxCell");

                        //gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Selected = true;

                        //his.gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Value = 1;

                        //SendKeys.Send("{ENTER}");

                    }
                    break;
                case "受注番号textBoxCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            /*
                            if (createReportData() == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();

                                プレビューform.dataSet = dataSet;
                                プレビューform.rptName = "仕入先別発注残明細表CrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "仕入先別発注残明細表");
                            }
                            */
                            break;
                    }
                    break;
                case "プレビューbuttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            /*
                            if (createReportData() == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();

                                プレビューform.dataSet = dataSet;
                                プレビューform.rptName = "仕入先別発注残明細表CrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "仕入先別発注残明細表");
                            }
                            */
                            break;
                    }
                    break;
                case "印刷buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            /*
                            if (createReportData() == 0)
                            {
                                プレビューForm プレビューform = new プレビューForm();

                                プレビューform.dataSet = dataSet;
                                プレビューform.rptName = "仕入先別発注残明細表CrystalReport";
                                プレビューform.Show();
                            }
                            else
                            {
                                MessageBox.Show("データがありません", "仕入先別発注残明細表");
                            }
                            */
                            break;
                    }
                    break;
                case "終了buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            //this.Close();
                            this.Hide();
                            break;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            string strSQL = null;
            int len_tok = 0;
            string tokcd = null;
            string opecd = null;
            string opejigyocd = null;
            int jyutyuno = 0;

            if (forceFlg == 1)
            {
                //forceFlg = 0;
                return;
            }

            switch (e.CellName)
            {
                case "処理区分textBoxCell":
                    if (e.FormattedValue == null)
                    {
                        e.Cancel = true;

                        this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].ErrorText = "未入力です";
                        this.gcMultiRow1.EndEdit();
                    }
                    else if (e.FormattedValue.ToString() == "0")
                    {
                        e.Cancel = false;

                        this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].ErrorText = string.Empty;

                    }
                    else if (e.FormattedValue.ToString() == "1")
                    {
                        e.Cancel = false;

                        this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].ErrorText = string.Empty;
                    }
                    else if (e.FormattedValue.ToString() == "2")
                    {
                        e.Cancel = false;

                        this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].ErrorText = string.Empty;
                    }
                    else
                    {
                        e.Cancel = true;

                        this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].ErrorText = "0、1、または2 です";
                        this.gcMultiRow1.EndEdit();
                    }
                    break;
                case "オペレーターコードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, e.FormattedValue.ToString()))
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
                case "担当者コードtextBoxCell":
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
                case "売上区分コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell03, e.FormattedValue.ToString()))
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
                case "請求月区分コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell04, e.FormattedValue.ToString()))
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
                case "ドラスタ区分コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell05, e.FormattedValue.ToString()))
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
                case "倉庫コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell06, e.FormattedValue.ToString()))
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
                case "商品コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        vw_ShohintableAdapter = new vw_ShohinTableAdapter();
                        DataTable 商品dataTable = vw_ShohintableAdapter.GetDataBy商品コード(e.FormattedValue.ToString());
                        if (商品dataTable.Rows.Count > 0)
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
                case "得意先コードtextBoxCell":

                    if (gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].EditedFormattedValue != null)
                    {
                        len_tok = gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].EditedFormattedValue.ToString().Length;
                        tokcd = gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].EditedFormattedValue.ToString();
                        opecd = null;
                        opejigyocd = null;
                        if (gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                        {
                            opecd = gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString();
                        }
                        else
                        {
                            return;
                        }
                        opejigyocd = Common.GetOPE事業所コード(opecd);
                        if (len_tok != 7 & opejigyocd == "1" & Utility.Left(tokcd,1)!="R")
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード"].Value = String.Format("{0:000000}", Convert.ToInt64(tokcd));
                        }
                        if (len_tok != 7 & opejigyocd == "2" & Utility.Left(tokcd, 1) != "R")
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード"].Value = String.Format("{0:R00000}", Convert.ToInt64(tokcd));
                        }
                        if (len_tok != 7 & opejigyocd == "2" & Utility.Left(tokcd, 1) == "R")
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード"].Value = tokcd;
                        }
                        if (len_tok == 7)
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード"].Value = tokcd;
                        }
                        strSQL = "SELECT * FROM vw_Tokuisaki WHERE 得意先コード='" + tokcd + "'";
                        DataTable tokTable = Utility.GetData(strSQL);
                        if (tokTable.Rows.Count == 0)
                        {
                            MessageBox.Show("入力されたコードは、未登録です。","");
                            e.Cancel = true;
                            break;
                        }
                        gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value=tokTable.Rows[0]["得意先名"];
                        gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value="1";
                        gcMultiRow1.ColumnHeaders[0].Cells["倉庫gcComboBoxCell"].Value = "1";
                        gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = tokTable.Rows[0]["担当者コード"];
                        gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value=tokTable.Rows[0]["担当者コード"];
                        /* どっかで設定しないと。。。
                        Forms!F_会社基本!事業所コード = .事業所コード
                        .部課コード = .M部課コード
                        .ランク = .Mランク
                        .売上切捨区分 = .M売上切捨区分
                        .売上税区分 = .M売上税区分
                        .請求先コード = .M請求先コード
                        */
                    }


                    /*
                    ' ヘッダ部：得意先マスタチェック
                    If (Len(Forms!F_受注入力メイン!得意先) <> 7 And .OPE事業所コード = 1) Then
                        ' '2012/1 h.yamamoto chg ヘッダ部：Format([Forms]![F_受注入力メイン]![得意先],"000000")を[Forms]![F_受注入力メイン]![得意先コード]に代入
                        Forms!F_受注入力メイン!得意先コード = Format(Forms!F_受注入力メイン!得意先, "000000")
                    End If
                    If (Len(Forms!F_受注入力メイン!得意先) <> 7 And .OPE事業所コード = 2 And Left(.得意先, 1) <> "R") Then
                        ' '2012/1 h.yamamoto chg ヘッダ部：Format([Forms]![F_受注入力メイン]![得意先],"\R00000")を[Forms]![F_受注入力メイン]![得意先コード]に代入
                        Forms!F_受注入力メイン!得意先コード = Format(Forms!F_受注入力メイン!得意先, "\R00000")
                    End If
                    If (Len(Forms!F_受注入力メイン!得意先) <> 7 And .OPE事業所コード = 2 And Left(.得意先, 1) = "R") Then
                        ' '2012/1 h.yamamoto chg ヘッダ部：[Forms]![F_受注入力メイン]![得意先]を[Forms]![F_受注入力メイン]![得意先コード]に代入
                        Forms!F_受注入力メイン!得意先コード = Forms!F_受注入力メイン!得意先
                    End If
                    If (Len(Forms!F_受注入力メイン!得意先) = 7) Then
                        ' '2012/1 h.yamamoto add ヘッダ部：[Forms]![F_受注入力メイン]![得意先]を[Forms]![F_受注入力メイン]![得意先コード]に代入
                        Forms!F_受注入力メイン!得意先コード = Forms!F_受注入力メイン!得意先
                    End If
                    If (Eval("DLookUp(""得意先コード"",""vw_Tokuisaki"",""得意先コード=form.得意先コード"") Is Null")) Then
                        ' ヘッダ部：    ***** 00/05/25 追加
                        ' ヘッダ部：コード未登録確認
                        Beep
                        MsgBox "入力されたコードは、未登録です。", vbOKOnly, "コード未登録確認"
                        ' ヘッダ部：
                        DoCmd.CancelEvent
                        ' ヘッダ部：
                        Exit Function
                    End If
                    ' ヘッダ部：[得名]
                    .得名 = .M得意先名
                    ' ヘッダ部：[事業所コード]
                    .事業所コード = .M事業所コード
                    ' ヘッダ部：[Forms]![F_会社基本]![事業所コード]セット
                    Forms!F_会社基本!事業所コード = .事業所コード
                    ' ヘッダ部：[担当者コード]
                    .担当者コード = .M担当者コード
                    ' ヘッダ部：[部課コード]
                    .部課コード = .M部課コード
                    ' ヘッダ部：[ランク]
                    .ランク = .Mランク
                    ' ヘッダ部：[売上切捨区分]
                    .売上切捨区分 = .M売上切捨区分
                    ' ヘッダ部：[売上税区分]
                    .売上税区分 = .M売上税区分
                    ' ヘッダ部：[請求先コード]
                    .請求先コード = .M請求先コード
                    */
                    /*
                    if (e.FormattedValue != null)
                    {
                        vw_TokuisakitableAdapter = new vw_TokuisakiTableAdapter();
                        DataTable 得意先dataTable = vw_TokuisakitableAdapter.GetDataBy得意先コード(e.FormattedValue.ToString());
                        if (得意先dataTable.Rows.Count > 0)
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            MessageBox.Show("マスタに存在しません。", "受注入力");
                            e.Cancel = true;
                        }
                    }
                    */
                    break;
                case "発注有無区分numericUpDownCell":
                    if ((string)e.FormattedValue == "1")
                    {
                        string _serchcode = (string)this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value;
                        vw_ShohintableAdapter = new vw_ShohinTableAdapter();
                        DataTable 商品dataTable = vw_ShohintableAdapter.GetDataBy商品コード(_serchcode);
                        if (商品dataTable.Rows.Count > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = 商品dataTable.Rows[0]["主要仕入先"];
                            vw_SiiresakitableAdapter = new vw_SiiresakiTableAdapter();
                            DataTable 仕入先dataTable = vw_SiiresakitableAdapter.GetDataBy仕入先コード((string)商品dataTable.Rows[0]["主要仕入先"]);
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = 仕入先dataTable.Rows[0]["仕入先名"];
                        }
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                    }
                    break;
                case "仕入先コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        vw_SiiresakitableAdapter = new vw_SiiresakiTableAdapter();
                        DataTable 仕入先dataTable = vw_SiiresakitableAdapter.GetDataBy仕入先コード(e.FormattedValue.ToString());
                        if (仕入先dataTable.Rows.Count > 0)
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

                case "売伝NOtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value != null)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "受注NOtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].EditedFormattedValue != null)
                    {
                        if (Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].EditedFormattedValue) == 0)
                        {
                            e.Cancel = false;
                            break;
                        }

                    }
                    else
                    {
                        e.Cancel = true;
                        break;
                    }
                    if (gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].EditedFormattedValue != null)
                    {
                        opecd = gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].EditedFormattedValue.ToString();
                    }
                    else
                    {
                        e.Cancel = true;
                        break;
                    }
                    opejigyocd = Common.GetOPE事業所コード(opecd);
                    jyutyuno = Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].EditedFormattedValue);
                    strSQL = "SELECT DISTINCT 受注番号, 完了フラグ, 受注連番 ";
                    strSQL = strSQL + "FROM T受注ファイル ";
                    strSQL = strSQL + "WHERE 受注番号=" + jyutyuno + " ";
                    strSQL = strSQL + "AND 完了フラグ=0 "; 
                    strSQL = strSQL + "AND 事業所コード='" + opejigyocd + "'";
                    DataTable jyutyuTable = Utility.GetData(strSQL);
                    if (jyutyuTable.Rows.Count==0)
                    {
                        MessageBox.Show("入力された受注番号は、エラー番号です。");
                        jyutyuTable.Dispose();
                        e.Cancel = true;
                        break;
                    }
                    strSQL = "SELECT DISTINCT 受注番号, 計上フラグ, 出荷連番 ";
                    strSQL = strSQL + "FROM T出荷ファイル ";
                    strSQL = strSQL + "WHERE 受注番号=" + jyutyuno + " ";
                    strSQL = strSQL + "AND 計上フラグ=0";
                    jyutyuTable.Dispose();

                    DataTable syukaTable = Utility.GetData(strSQL);
                    if (syukaTable.Rows.Count>0)
                    {
                        MessageBox.Show("入力された受注番号は、出荷データが存在します。");
                        syukaTable.Dispose();
                        e.Cancel = true;
                        break;
                    }
                    syukaTable.Dispose();





                    /*
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].EditedFormattedValue == null)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].EditedFormattedValue.ToString() == "000000")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            string jno = this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].EditedFormattedValue.ToString();
                            string sql = "SELECT count(*) FROM T受注ファイル WHERE 受注番号='" + jno + "'";
                            DataTable dt = Utility.GetData(sql);
                            if (dt.Rows.Count > 0)
                            {
                                e.Cancel = false;
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                    */
                    break;
                case "明細摘要textBoxCell":
                    if (ドラスタ区分 == "1")
                    {
                        if (e.FormattedValue == null)
                        {
                            MessageBox.Show("ＪＡＮコードが１３桁ではありません。");
                        }
                        else
                        {
                            if (e.FormattedValue.ToString().Length != 13)
                            {
                                MessageBox.Show("ＪＡＮコードが１３桁ではありません。");
                            }
                            else if (Common.chk_dgt(e.FormattedValue.ToString(), 1) != 0)
                            {
                                MessageBox.Show("ＪＡＮコードが不正です。");
                            }
                        }
                    }
                    e.Cancel = false;
                    break;
                case "確認textBoxCell":
                    if (e.FormattedValue == null)
                    {
                        e.Cancel = true;
                    }
                    else if ((string)e.FormattedValue == "0")
                    {
                        e.Cancel = false;
                    }
                    else if ((string)e.FormattedValue == "9")
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "伝票確認textBoxCell":
                    if (e.FormattedValue == null)
                    {
                        e.Cancel = true;
                    }
                    else if ((string)e.FormattedValue == "0")
                    {
                        e.Cancel = false;
                    }
                    else if ((string)e.FormattedValue == "9")
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {

            if (forceFlg == 1)
            {
                //forceFlg = 0;
                return;
            }

            string _serchcode = null;
            int serchcode = 0;

            switch (e.CellName)
            {
                case "処理区分textBoxCell":
                    forceFlg = 1;
                    string syorikbn = gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString();
                    if (syorikbn == "0")
                    {
                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        newproc();
                    }
                    else if (syorikbn == "1")
                    {
                        editproc();
                    }
                    else if (syorikbn == "2")
                    {
                        deleteproc();
                    }
                    else
                    {
                        // e.Cancel = true;
                    }
                    forceFlg = 0;
                    break;
                case "オペレーターコードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString()))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;
                        }
                    }
                    break;
                case "売伝NOtextBoxCell":
                    /*
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value.ToString();
                        serchcode = Int32.Parse(_serchcode);

                        if (serchcode == 0)
                        {
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Enabled = true;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].ReadOnly = false;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Selectable = true;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Enabled = true;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].ReadOnly = false;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Selectable = true;
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                            forceFlg = 0;
                            break;
                        }

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();
                        dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy売上修正戻ファイルfrom売上明細ファイル(serchcode);

                        forceFlg = 1;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value = dataTable.Rows[0]["売上日"].ToString();
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataTable.Rows[0]["得意先コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                        forceFlg = 0;

                        this.gcMultiRow1.DataSource = dataTable;

                        // コントロールの描画を再開する
                        gcMultiRow1.ResumeLayout();

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日textBoxCell");

                        //gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Selected = true;

                        //his.gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Value = 1;

                        //SendKeys.Send("{ENTER}");

                    }
                    */
                    break;
                case "受注NOtextBoxCell":
                    break;
                    /*
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value != null)
                    {
                        if (Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value) == 0)
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                    _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value.ToString();
                    serchcode = Int32.Parse(_serchcode);

                    // コントロールの描画を停止する
                    gcMultiRow1.SuspendLayout(); 
                    dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy売上修正戻ファイルfrom受注ファイル(serchcode);
                    
                   
                    this.gcMultiRow1.DataSource = dataTable;

                    */
                    // コントロールの描画を再開する
                    ////gcMultiRow1.ResumeLayout();

                    ////gcMultiRow1.ColumnHeaders[1].Cells["売上行番号textBoxCell"].Selected = true;
                    ////gcMultiRow1.ColumnHeaders[1].Cells["売上行番号textBoxCell"].Value = 1;
                    
                    /*
                    Forms!F_会社基本!得意先コード = .得意先コード
                    Forms!F_会社基本!事業所コード = .事業所コード
                    Forms!F_会社基本!入力中 = True
                    Forms!F_会社基本!シス区分 = .システム区分
                    .月数 = DateDiff("m", Forms!F_会社基本!期首年月日, IIf(Day(.売上日) <= 20, DateSerial(Year(.売上日), Month(.売上日), 21), DateSerial(Year(DateAdd("m", 1, .売上日)), Month(DateAdd("m", 1, .売上日)), 20)))
                    Call Sum_UriageKeijo(1)
                    .売計.Visible = True
                    .売上累計.Visible = True
                    .売上日.Enabled = True
                    .F_売上計上サブ.Form!原価単価.Enabled = False
                    .F_売上計上サブ.Form!原価単価.Locked = True
                    .月数 = DateDiff("m", Forms!F_会社基本!期首年月日, IIf(Day(.売上日) <= 20, DateSerial(Year(.売上日), Month(.売上日), 21), DateSerial(Year(DateAdd("m", 1, .売上日)), Month(DateAdd("m", 1, .売上日)), 20)))
                    If (.売上区分 = 4) Then
                        ' ヘッダ部：[売上区分]=4の時、[区分].[Visible] Yes
                        .区分.Visible = True
                        ' ヘッダ部：納入先コード　背景色　赤
                        .納入先コード.BackColor = 8421631
                        ' ヘッダ部：[納入先コード]にDMin("納入先コード","T_納入先マスタ","得意先コード=form.得意先コード")
                        .納入先コード = DMin("納入先コード", "T_納入先マスタ", "得意先コード=form.得意先コード")
                        ' ヘッダ部：納入先コード
                        DoCmd.Requery "納入先コード"
                        ' ヘッダ部：[納入先コード].[Column](1)を[納名]にｾｯﾄ
                        .納名 = .納入先コード.Column(1)
                    End If
                    If (.売上区分 <> 4) Then
                        ' ヘッダ部：[売上区分]=4の時、[区分].[Visible] No
                        .区分.Visible = False
                        ' ヘッダ部：納入先コード　背景色　白
                        .納入先コード.BackColor = 16777215
                        ' ヘッダ部：[納入先コード]にNULL
                        .納入先コード = Null
                        ' ヘッダ部：[納名]を[Null
                        .納名 = Null
                    End If
                    ' ヘッダ部：
                    DoCmd.SetWarnings False
                    ' ヘッダ部：QS_障害更新＿入力中＿明細
                    DoCmd.OpenQuery "QS_障害更新＿入力中＿明細", acViewNormal, acEdit
                    */
                    break;
                    
                case "担当者コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell02, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value;
                        }
                    }
                    break;

                case "売上区分コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell03, this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value.ToString()))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value;
                        }
                        /*
                        受注ファイルdataSet = ((受注ファイルDataSet)(this.受注ファイルDataSet1));
                        dataTable = 受注ファイルdataSet.T受注戻しファイル;
                        gcMultiRow1.DataSource = dataTable;
                        // コントロールの描画を再開する
                        gcMultiRow1.ResumeLayout();
                        //gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;
                        gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = '1';
                        //SendKeys.Send("{ENTER}");
                        */
                    }
                    break;
                case "請求月区分コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell04, this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Value.ToString()))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Value;

                        }
                    }
                    break;
                case "ドラスタ区分コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell05, this.gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Value.ToString()))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Value;

                        }
                    }
                    break;
                case "倉庫コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell06, this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value.ToString()))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value;

                        }
                    }
                    break;
                case "納入先コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell07, this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value.ToString()))
                        {
                            this.gcMultiRow1.ColumnFooters[0].Cells["納入先gcComboBoxCell"].Value = this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value.ToString();
                        }
                    }
                    break;
                case "得意先コードtextBoxCell":
                    /*
                    '2012/1 h.yamamoto add str *------*
                    If Me![処理区分] = 0 Then  '新規
                        Me.社コード = Null
                        Me.[EOS区分] = Null
                        Me.経費区分 = Null
                        Me.客注区分 = Null
                        Me.直送区分 = Null
        
                        On Error Resume Next
                        'ワークファイルの確定
                        DoCmd.RunCommand acCmdRefresh
        
                        '新規時に画面外で更新される項目
                        W_SQL = "UPDATE W_売上明細Ｈファイル AS WH "
                        W_SQL = W_SQL & " SET WH.取引先コード = NULL "
                        W_SQL = W_SQL & " ,   WH.発注区分 = NULL "
                        W_SQL = W_SQL & " ,   WH.店コード = NULL "
                        W_SQL = W_SQL & " ,   WH.店コードＢ = NULL "
    
                        dbMain.Execute W_SQL, dbFailOnError
        
                    End If
    
    
                    If Forms!F_売上計上メイン![処理区分] = 0 Then  '新規
    
                        If DLookup("地区コード", "T_得意先マスタ", "得意先コード='" & Me.得意先コード & "'") = "100" Then
    
                            On Error Resume Next
                            'ワークファイルの確定
                            DoCmd.RunCommand acCmdRefresh
    
                            '2012/3 h.yamamoto chg str *------*
                '''            W_SQL = "UPDATE W_売上明細Ｈファイル AS WH "
                '''            W_SQL = W_SQL & " INNER JOIN T_得意先マスタ AS TOK ON TOK.[得意先コード] = WH.[得意先コード] "
                '''            W_SQL = W_SQL & " SET WH.[取引先コード] = TOK.[取引先コード] "
                '''
                '''            dbMain.Execute W_SQL, dbFailOnError
            
                            W_SQL = "UPDATE W_売上明細Ｈファイル AS WH "
                            W_SQL = W_SQL & " SET WH.[取引先コード２] = '54711' "
        
                            dbMain.Execute W_SQL, dbFailOnError
                            '*------* 2012/3 h.yamamoto chg end
            

                            '2012/3 h.yamamoto chg str *------*
                '''            W_SQL = "UPDATE W_売上明細Ｈファイル AS WH "
                '''            W_SQL = W_SQL & " SET WH.発注区分 = 1 "
                '''            W_SQL = W_SQL & " ,   WH.[ＥＯＳ区分] = '21' "
                '''            W_SQL = W_SQL & " ,   WH.店コード = RIGHT(WH.得意先コード,4) "
                '''            W_SQL = W_SQL & " ,   WH.社コード = 1 "
                '''            W_SQL = W_SQL & " ,   WH.店コードＢ = RIGHT(WH.得意先コード,4) "
                '''
                '''            dbMain.Execute W_SQL, dbFailOnError
            
                            If Me.オペレーター番号 = 92 Then
            
                                W_SQL = "UPDATE W_売上明細Ｈファイル AS WH "
                                W_SQL = W_SQL & " SET WH.発注区分 = 1 "
                                W_SQL = W_SQL & " ,   WH.[ＥＯＳ区分] = '21' "
                                W_SQL = W_SQL & " ,   WH.店コード = RIGHT(WH.得意先コード,4) "
                                W_SQL = W_SQL & " ,   WH.社コード = 1 "
                                W_SQL = W_SQL & " ,   WH.店コードＢ = RIGHT(WH.得意先コード,4) "
                                W_SQL = W_SQL & " ,   WH.直送区分 = 2 "
            
                                dbMain.Execute W_SQL, dbFailOnError
                
                            Else
            
                                W_得意先コード = DLookup("得意先コード", "T_得意先マスタ", "業態コード='" & Me.得意先コード & "'")
            
                                W_SQL = "UPDATE W_売上明細Ｈファイル AS WH "
                                W_SQL = W_SQL & " SET WH.発注区分 = 1 "
                                W_SQL = W_SQL & " ,   WH.[ＥＯＳ区分] = '21' "
                                W_SQL = W_SQL & " ,   WH.店コード = " & IIf(Nz(W_得意先コード) <> "", Right(W_得意先コード, 4), "Null")
                                W_SQL = W_SQL & " ,   WH.社コード = " & IIf(Nz(W_得意先コード) <> "", Left(W_得意先コード, 3), "Null")
                                W_SQL = W_SQL & " ,   WH.店コードＢ = " & IIf(Nz(W_得意先コード) <> "", Right(W_得意先コード, 4), "Null")
                                W_SQL = W_SQL & " ,   WH.直送区分 = 2 "
            
                                dbMain.Execute W_SQL, dbFailOnError
            
                            End If
                            '*------* 2012/3 h.yamamoto chg end
    
                        End If
        
                    End If
    
    
                    '2012/4 h.yamamoto chg str *------*
                '    If Nz(DLookup("地区コード", "T_得意先マスタ", "得意先コード='" & Me.得意先コード & "'"), "") = "100" _
                '      And (Me.売上区分 = 1 Or Me.売上区分 = 4) _
                '      And Nz(DMax("オペレーターコード", "T_受注ファイル", "受注番号=" & Me.受注番号), "") <> "92" Then    '条件追加 '2012/3 h.yamamoto add
      
                    W_業態コード = Nz(DLookup("業態コード", "T_得意先マスタ", "得意先コード='" & Me.得意先コード & "'"), "")
        
                    If DLookup("地区コード", "T_得意先マスタ", "得意先コード='" & Me.得意先コード & "'") = "100" _
                       And Nz(W_業態コード, "") = "" Then
                    '*------* 2012/4 h.yamamoto chg end
      
                        '入力可能
                        Me.ドラスタ区分.Enabled = True
                        Me.ドラスタ区分.Locked = False
                    Else

                        On Error Resume Next
                        'ワークファイルの確定
                        DoCmd.RunCommand acCmdRefresh

                        W_SQL = "UPDATE W_売上明細Ｈファイル AS WH "
                        W_SQL = W_SQL & " SET WH.ドラスタ区分 = null "
    
                        dbMain.Execute W_SQL, dbFailOnError


                        Me.ドラスタ区分名.Requery
                        '入力不可
                        Me.ドラスタ区分.Enabled = False
                        Me.ドラスタ区分.Locked = True
                    End If
                    '*------* 2012/1 h.yamamoto add end
                    */
                    //this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[2].Cells["得意先コードtextBoxCell"].EditedFormattedValue;
                    break;
                case "商品コードtextBoxCell":
                    //this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].EditedFormattedValue;
                    break;
            }
        }

        /*
        void gcMultiRow1_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            GcMultiRow gcMultiRow = sender as GcMultiRow;
            Cell currentCell = null;

            switch (e.Scope)
            {
                case CellScope.ColumnHeader:
                    // 列ヘッダセクションの場合
                    currentCell = gcMultiRow.ColumnHeaders[e.SectionIndex].Cells[e.CellIndex];

                    if (e.CellName == "処理区分textBoxCell")
                    {
                    //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 4);
                    }
                    break;
                case CellScope.Row:
                    // 行の場合
                    currentCell = gcMultiRow.Rows[e.RowIndex].Cells[e.CellIndex];

                    break;
                case CellScope.ColumnFooter:
                    // 列フッタセクションの場合
                    currentCell = gcMultiRow.ColumnFooters[e.SectionIndex].Cells[e.CellIndex];

                    break;
            }

            //if (currentCell != null && currentCell.EditedFormattedValue != null)
            //    Console.WriteLine(currentCell.EditedFormattedValue); 
            
        }
        */

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            if (forceFlg == 1)
            {
                //forceFlg = 0;
                return;
            }
            switch (e.CellName)
            {
                case "処理区分textBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue == "0")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue == "1" ||
                        (string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue == "2")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売伝NOtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売伝NOtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売伝NOtextBoxCell");
                        }
                    }

                    break;
                case "売伝NOtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].EditedFormattedValue != null)
                    {
                        string _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].EditedFormattedValue.ToString();
                        int serchcode = Int32.Parse(_serchcode);

                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "2")
                        {
                            if (serchcode == 0)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売伝NOtextBoxCell");
                            }
                            else
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                            }
                        }
                        else
                        {
                            if (serchcode == 0)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                            }
                            else
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日textBoxCell");
                            }
                        }

                    }
                    break;
                case "受注NOtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                    }
                    else
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                    }
                    break;
                /*
                case "得意先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    else
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    break;
                */
                case "売上区分コードtextBoxCell":
                    forceFlg = 1;
                    if (ドラスタ区分 == "1")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ドラスタ区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ドラスタ区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ドラスタ区分コードtextBoxCell");
                        }
                    }
                    else
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "請求月区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "請求月区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "請求月区分コードtextBoxCell");
                        }
                    }
                    forceFlg = 0;
                    break;
                case "ドラスタ区分コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "請求月区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "請求月区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "請求月区分コードtextBoxCell");
                    }
                    break;
                case "請求月区分コードtextBoxCell":
                    if (ドラスタ区分 == "1")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "売上行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ドラスタ区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "売上行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ドラスタ区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "売上行番号textBoxCell");
                        }
                    }
                    else
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "売上行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "売上行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "売上行番号textBoxCell");
                        }
                    }
                    break;
                /*
                case "受注行番号textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    break;
                case "商品コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注数numericUpDownCell");
                    }
                    else
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注数numericUpDownCell");
                    }
                    break;
                */
                case "数量numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "定価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "定価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "定価numericUpDownCell");
                    }
                    break;
                case "定価numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "納品掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "数量numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "納品掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "数量numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "納品掛率numericUpDownCell");
                    }
                    break;
                case "納品掛率numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "売上単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "定価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "売上単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "定価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "売上単価numericUpDownCell");
                    }
                    break;
                /*
                case "発注有無区分numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        if ((int)gcMultiRow1.ColumnHeaders[0].Cells["発注有無区分numericUpDownCell"].Value == 1)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                        }
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        if ((int)gcMultiRow1.ColumnHeaders[0].Cells["発注有無区分numericUpDownCell"].Value == 1)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                        }
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].EditedFormattedValue == "1")
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                        }
                    }
                    break;
                case "仕入先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注有無区分numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注有無区分numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕名textBoxCell");
                    }
                    break;
                case "仕名textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    break;
                case "発注数numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕名textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    break;
                case "発注摘要textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    break;
                case "明細摘要textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    break;
                */
                case "確認textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    break;
            }
        }

        private void 売上計上入力Form_Shown(object sender, EventArgs e)
        {
            this.Activate();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "受注NOtextBoxCell");
            SendKeys.Send("{ENTER}");
        }

        /*
        private void gcMultiRow1_EditingControlShowing(object sender, GrapeCity.Win.MultiRow.EditingControlShowingEventArgs e)
        {
            if (e.Control is TextBoxEditingControl)
            {
                TextBoxEditingControl text = e.Control as TextBoxEditingControl;
                text.KeyDown -= new KeyEventHandler(text_KeyDown);
                text.KeyDown += new KeyEventHandler(text_KeyDown);
            }
        }
        */

        /*
        private void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            TextBoxEditingControl textBox = e.Control as TextBoxEditingControl;
            if (textBox != null)
            {
                textBox.KeyDown -= new KeyEventHandler(得意先コードtextBoxCell_KeyDown);
                textBox.KeyDown += new KeyEventHandler(得意先コードtextBoxCell_KeyDown);
            }
        }

        private void 得意先コードtextBoxCell_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 )
            {
                得意先検索Form jform = new 得意先検索Form();
                jform.Show();
            }
        }
*/
        //private void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        //{
            //if (gcMultiRow1.CurrentCell is TextBoxCell)
            //gcMultiRow1.BeginEdit(false);

            // Cell.Nameプロパティが"textBoxCell1"の場合 
            
       //     if (e.CellName == "処理区分textBoxCell")
       //     {
       //         if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value == "1")
       //         {
       //             gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Selected = true;
       //         }
       //         else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value == "0")
       //         {
       //             gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value = DateTime.Today;
       //             gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Selected = true;
       //         }
       //     }

       // }

        //private void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        private void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {

            if (forceFlg == 1)
            {
                //forceFlg = 0;
                return;
            }

            string _serchcode = null;
            int serchcode = 0;
            string _serchcode2 = null;
            int serchcode2 = 0;
            string strName = null;
            int intIndex = 0;

            switch (e.CellName)
            {
                case "オペレーターコードtextBoxCell":
                    _serchcode = "0";

                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                    {

                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;

                        //_serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;
                        //DataRow[] dataRow = this.sPEEDDBDataSet.Tオペレーターマスタ.Select("オペレーターコード = " + _serchcode);
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(1);
                        //Object opname = this.queriesTableAdapter1.オペレータ名ScalarQuery(_serchcode);
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = opname ;
                        //gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Selected = true;
                        //SendKeys.Send("{ENTER}");
                    }
                    break;
                case "処理区分textBoxCell":
                    //string _serchcode = "0";
                    /*
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "1")
                    {
                        //gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Selected = true;
                        //SendKeys.Send("{ENTER}");
                        gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].ReadOnly = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Selectable = false;

                        gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].ReadOnly = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Selectable = false;

                        gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].ReadOnly = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Selectable = true;

                        string cellName = "受注NOtextBoxCell";
                        int cellIndex = gcMultiRow1.Template.ColumnHeaders[0].Cells[cellName].CellIndex;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, cellIndex);

                    }
                    else
                    {

                        gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].ReadOnly = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Selectable = true;

                        gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].ReadOnly = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Selectable = true;

                        gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].ReadOnly = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Selectable = true;


                        //string cellName = "オペレーターコードtextBoxCell";
                        //int cellIndex = gcMultiRow1.Template.Row.Cells[cellName].CellIndex;

                        //gcMultiRow1.Focus();
                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, cellIndex);

                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, cellIndex);
                        //SendKeys.Send("{ENTER}");

                    }
                    */
                    break;
                case "受注日textBoxCell":
                    strName = "オペレーターコードtextBoxCell";
                    intIndex = gcMultiRow1.Template.ColumnHeaders[0].Cells[strName].CellIndex;
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, intIndex);
                    break;
                case "得意先コードtextBoxCell":
                    
                    _serchcode = "0";

                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString();

                        //Object nm = this.t得意先マスタ1TableAdapter.得意先名ScalarQuery(_serchcode);
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value=nm;
                        SPEEDDB.得意先マスタ検索DataTable temp = new SPEEDDB.得意先マスタ検索DataTable();
                        temp = this.得意先マスタ検索TableAdapter1.GetDataBy得意先select(_serchcode);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = temp[0].得意先名;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = temp[0].担当者コード;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = temp[0].担当者コード;

                        ドラスタ_ヘッダ_社コード = null;
                        ドラスタ_ヘッダ_経費区分 = null;
                        ドラスタ_ヘッダ_直送区分 = null;
                        ドラスタ_ヘッダ_ＥＯＳ区分 = null;
                        ドラスタ_ヘッダ_客注区分 = null;
                        取引先コード = null;
                        発注区分 = null;
                        店コード = null;
                        店コードB = null;
                    
                        if (temp[0].地区コード == "100")
                        {
                            取引先コード = "54711";
                            ドラスタ区分 = "1";
                            this.gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Visible = true;
                            this.gcMultiRow1.ColumnFooters[0].Cells["仕入先備考textBoxCell"].Visible = true;

                            売上計上_ドラスタ対応_ヘッダForm fsDrasta_Head;
                            fsDrasta_Head = new 売上計上_ドラスタ対応_ヘッダForm();
                            fsDrasta_Head.Owner = this;
                            if (fsDrasta_Head != null)
                            {
                                fsDrasta_Head.Sendドラスタ_ヘッダ_Data = "1,,2,21,";
                            }
                            fsDrasta_Head.ShowDialog();

                        }else{
                            取引先コード = null;
                            ドラスタ区分 = null;
                            this.gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Visible = false;
                            this.gcMultiRow1.ColumnFooters[0].Cells["仕入先備考textBoxCell"].Visible = false;
                        }

                        comboBoxCell07 = this.gcMultiRow1.ColumnFooters[0].Cells["納入先gcComboBoxCell"] as GcComboBoxCell;
                        comboBoxCell07.DataSource = Utility.GetComboBoxData("SELECT 納入先コード, 納入先名 FROM T納入先マスタ WHERE 得意先コード='" + _serchcode + "' ORDER BY 納入先コード");
                        comboBoxCell07.ListHeaderPane.Visible = false;
                        comboBoxCell07.ListHeaderPane.Visible = false;
                        comboBoxCell07.ListHeaderPane.Visible = false;
                        comboBoxCell07.TextSubItemIndex = 0;
                        comboBoxCell07.TextSubItemIndex = 1;
                        comboBoxCell07.ListColumns.ElementAt(0).AutoWidth = true;
                        comboBoxCell07.ListColumns.ElementAt(1).AutoWidth = true;
                        comboBoxCell07.TextFormat = "[1]";
                    }
                    break;
                case "売上区分コードtextBoxCell":
                    gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Selected = true;
                    gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Value = '1';
                    SendKeys.Send("{ENTER}");
                    break;
                case "請求月区分コードtextBoxCell":
                    gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Selected = true;
                    gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Value = '1';
                    SendKeys.Send("{ENTER}");
                    break;
                case "受注NOtextBoxCell":
                    _serchcode = null;
                    serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value != null )
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value.ToString();
                        serchcode = Int32.Parse(_serchcode);

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout(); 
                        dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy売上修正戻ファイルfrom受注ファイル(serchcode);
                        this.gcMultiRow1.DataSource = dataTable;

                        if (dataTable.Rows.Count > 0)
                        {
                            gcMultiRow1.SuspendLayout();

                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value = dataTable.Rows[0]["売上日"].ToString();
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataTable.Rows[0]["得意先コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                            forceFlg = 0;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                            //if (dataTable.Rows[0]["納入先コード"] != null)
                            //{
                            //    this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                            //}
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["納名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(23);

                            gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Selected = true;

                            gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Value = null;

                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "売上行番号textBoxCell");

                            // コントロールの描画を再開する
                            gcMultiRow1.ResumeLayout();

                            //SendKeys.Send("{ENTER}");
                        }
                        else
                        {
                            MessageBox.Show("データがありません");
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注NOtextBoxCell");
                        }
                
                    }
                    break;
                case "売伝NOtextBoxCell":
                    _serchcode = null;
                    serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value.ToString();
                        serchcode = Int32.Parse(_serchcode);

                        if (serchcode == 0)
                        {
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                            break;
                        }
                        
                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();
                        dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy売上修正戻ファイルfrom売上明細ファイル(serchcode);

                        if (dataTable.Rows.Count > 0)
                        {
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value = dataTable.Rows[0]["売上日"].ToString();
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataTable.Rows[0]["得意先コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                            forceFlg = 0;

                            this.gcMultiRow1.DataSource = dataTable;

                            // コントロールの描画を再開する
                            gcMultiRow1.ResumeLayout();
                        }

                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日textBoxCell");

                        //gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Selected = true;

                        //his.gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Value = 1;

                        //SendKeys.Send("{ENTER}");

                    }
                    break;
                case "売上行番号textBoxCell":
                    _serchcode = "0";
                    serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Value.ToString();
                        serchcode = Int32.Parse(_serchcode.ToString());

                        forceFlg = 1;

                        if (gcMultiRow1.Rows.Count >= serchcode)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "受注番号");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "受注行番号");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商品コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号受注残数numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "受注番号受注残数");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "数量");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "定価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "納品掛率");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "売上単価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "原価掛率");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "原価単価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "売上金額");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "完了フラグ");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "明細摘要");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "仕入先コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "仕名");
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号受注残数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                        }

                        forceFlg = 0;

                        //this.gcMultiRow1.SetValue(serchcode2 - 1, 0,this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value);
                        /*
                        DataTable dataTable = this.t受注ファイルTableAdapter1.GetDataBy受注番号(serchcode)this.gcMultiRow1.SetValue(serchcode2 - 1, 0);

                        this.gcMultiRow1.DataSource = dataTable;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value = dataRow[0].ItemArray.GetValue(11);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(68);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(14);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(15);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(85);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者textBoxCell"].Value = dataRow[0].ItemArray.GetValue(19);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["氏名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(29);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(22);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["納名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(23);

                        this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(29);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(42);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(74);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(75);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(122);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(76);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(42);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(46);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期dateTimePickerCell"].Value = dataRow[0].ItemArray.GetValue(78);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードnumericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(87);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(88);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(30);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = dataRow[0].ItemArray.GetValue(54);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(77);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(58);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(59);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(130);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = dataRow[0].ItemArray.GetValue(84);
                        */
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case "受注番号textBoxCell":
                    _serchcode = null;
                    serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value.ToString();
                        serchcode = Int32.Parse(_serchcode);

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();
                        dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy売上修正戻ファイルfrom受注ファイル(serchcode);
                        //this.gcMultiRow1.DataSource = dataTable;

                        if (dataTable.Rows.Count > 0)
                        {

                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != dataTable.Rows[0]["得意先コード"])
                            {
                                MessageBox.Show("得意先コードが違います");
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注番号textBoxCell");
                            }

                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");

                        }
                        else
                        {
                            MessageBox.Show("データがありません");
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注番号textBoxCell");
                        }

                    }
                    break;
                case "受注行番号textBoxCell":
                    _serchcode = null;
                    serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value != null)
                    {
                        if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value != null)
                        {
                            _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value.ToString();
                            serchcode = Int32.Parse(_serchcode);
                            _serchcode2 = this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value.ToString();
                            serchcode2 = Int32.Parse(_serchcode2);

                            // コントロールの描画を停止する
                            gcMultiRow1.SuspendLayout();
                            dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy売上修正戻ファイルfrom受注ファイル明細(serchcode, serchcode2);
                            //this.gcMultiRow1.DataSource = dataTable;

                            forceFlg = 1;

                            if (dataTable.Rows.Count > 0)
                            {

                                this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value = dataTable.Rows[0]["受注番号"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = dataTable.Rows[0]["受注行番号"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = dataTable.Rows[0]["商品コード"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号受注残数numericUpDownCell"].Value = dataTable.Rows[0]["受注番号受注残数"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = dataTable.Rows[0]["数量"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = dataTable.Rows[0]["定価"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = dataTable.Rows[0]["納品掛率"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value = dataTable.Rows[0]["売上単価"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = dataTable.Rows[0]["原価掛率"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = dataTable.Rows[0]["原価単価"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value = dataTable.Rows[0]["売上金額"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = dataTable.Rows[0]["完了フラグ"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = dataTable.Rows[0]["商名"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = dataTable.Rows[0]["明細摘要"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = dataTable.Rows[0]["仕入先コード"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = dataTable.Rows[0]["仕名"];

                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "数量numericUpDownCell");

                            }
                            else
                            {
                                MessageBox.Show("データがありません");
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                            }

                            forceFlg = 0;

                        }

                    }
                    break;
                case "商品コードtextBoxCell":
                    _serchcode = "0";

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                    {

                        if (dataTable == null)
                        {
                            gcMultiRow1.SuspendLayout();
                            dataTable = this.t売上修正戻ファイルTableAdapter1.GetDataBy();
                        }

                        _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value.ToString();
                        vw_ShohintableAdapter = new vw_ShohinTableAdapter();
                        DataTable 商品dataTable = vw_ShohintableAdapter.GetDataBy商品コード(_serchcode);
                        T会社基本tableAdapter = new T会社基本TableAdapter();
                        DataTable 会社基本dataTable = T会社基本tableAdapter.GetDataByKE(1);

                        forceFlg = 1;

                        if (商品dataTable.Rows.Count > 0)
                        {
                            if (商品dataTable.Rows[0]["諸口区分"].Equals(false))
                            {
                                this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["単価"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["原価単価"];
                            }
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = 商品dataTable.Rows[0]["定価"];
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["代表納品掛率"] * 100;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["単価"];
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["代表原価掛率"] * 100;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["原価単価"];
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = 商品dataTable.Rows[0]["商品名"].ToString();
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["在庫数numericUpDownCell"].Value = 商品dataTable.Rows[0]["現在在庫数"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["発注残数numericUpDownCell"].Value = 商品dataTable.Rows[0]["発注残数"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["現在在庫数"] - (decimal)商品dataTable.Rows[0]["受注残数"]; ;
                        }
                        if (会社基本dataTable.Rows.Count > 0)
                        {
                            if (会社基本dataTable.Rows[0]["単価管理"].Equals(true) & 会社基本dataTable.Rows[0]["得掛率管理"].Equals(true) & 商品dataTable.Rows[0]["諸口区分"].Equals(false))
                            {
                                this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value = Math.Truncate((double)商品dataTable.Rows[0]["定価"] * (double)会社基本dataTable.Rows[0]["得掛率"]);
                            }
                        }

                        forceFlg = 0;

                        //gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Selected = true;
                        //SendKeys.Send("{ENTER}");

                    }
                    break;
                case "確認textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].EditedFormattedValue != null)
                    {
                        if (gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].EditedFormattedValue.ToString() == "0")
                        {
                            int index = Int32.Parse(Utility.Nz(gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].EditedFormattedValue,0).ToString());
                            if (index == 0)
                            {
                                break;
                            }
                            if (gcMultiRow1.Rows.Count < index & gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].EditedFormattedValue != null)
                            {
                                if (index == 1)
                                {
                                    dataTable.Clear();
                                }
                                //gcMultiRow1.Rows.Add();
                                DataRow dataRow = dataTable.NewRow();
                                string sql = null;

                                dataRow["本支店区分"] = 2;
                                dataRow["発注区分"] = 1;
                                dataRow["処理コード"] = 3;
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "0")
                                {
                                    dataRow["入力区分"] = 1;
                                }
                                else if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "1")
                                {
                                    dataRow["入力区分"] = 2;
                                }
                                else if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "2")
                                {
                                    dataRow["入力区分"] = 3;
                                }
                                else
                                {
                                    dataRow["入力区分"] = 5;
                                }
                                if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value != null)
                                {
                                    dataRow["処理区"] = "1";
                                }
                                else
                                {
                                    dataRow["処理区"] = "0";
                                }
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value != null)
                                {
                                    dataRow["売上伝票番号"] = Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value);
                                }
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value != null)
                                {
                                    dataRow["受注番号"] = Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value);
                                }
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value != null)
                                {
                                    dataRow["売上日"] = this.gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value.ToString();
                                }
                                dataRow["得意先コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString();
                                dataRow["得名"] = this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value.ToString();
                                dataRow["納期"] = this.gcMultiRow1.ColumnHeaders[0].Cells["売上日textBoxCell"].Value.ToString();
                                dataRow["エラーフラグ"] = 0;
                                dataRow["在庫管理区分"] = 0;
                                dataRow["チェック"] = 0;
                                dataRow["完了フラグ"] = 0;
                                dataRow["システム区分"] = 101;
                                dataRow["WS_ID"] = "04";
                                //dataRow["単価更新フラグ"] = 0;
                                dataRow["返品区分"] = 0;
                                dataRow["入金チェック"] = 0;
                                dataRow["検収チェック"] = 0;
                                dataRow["発行済フラグ"] = 0;
                                dataRow["受注更新フラグ"] = 0;
                                dataRow["出荷更新フラグ"] = 0;
                                dataRow["得意先更新フラグ"] = 0;
                                dataRow["商品更新フラグ"] = 0;
                                dataRow["商品倉庫更新フラグ"] = 0;
                                dataRow["商品取引更新フラグ"] = 0;
                                dataRow["オペレーターコード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString();
                                dataRow["担当者コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value.ToString();
                                dataRow["売上区分コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value.ToString();
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Value != null)
                                {
                                    dataRow["請求月区分コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Value.ToString();
                                }
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value != null)
                                {
                                    dataRow["倉庫コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value.ToString();
                                }

                                if (this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value != null)
                                {
                                    dataRow["納入先コード"] = this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value.ToString();
                                }
                                if (this.gcMultiRow1.ColumnFooters[0].Cells["伝票摘要textBoxCell"].Value != null)
                                {
                                    dataRow["伝票摘要"] = this.gcMultiRow1.ColumnFooters[0].Cells["伝票摘要textBoxCell"].Value.ToString();
                                }
                                if (this.gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Value != null)
                                {
                                    dataRow["店舗備考"] = this.gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Value.ToString();
                                }

                                dataRow["売上累計"] = 0;

                                SqlDb sqlDb = new SqlDb();
                                sqlDb.Connect();　　//

                                //事業所コード、部課コード、処理日の時刻、請求先コード
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != null)
                                {
                                    DataTable tokTable = null;
                                    string tokcd = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString();
                                    sql = "SELECT TOK.事業所コード,TAN.部課コード,TOK.請求先コード,TOK.売上切捨区分,TOK.売上税区分 ";
                                    sql += "FROM vw_Tokuisaki TOK ";
                                    sql += "LEFT JOIN T担当者マスタ TAN ON TOK.担当者コード=TAN.担当者コード ";
                                    sql += "WHERE TOK.得意先コード='" + tokcd + "'";
                                    tokTable = sqlDb.ExecuteSql(sql, -1);
                                    if (tokTable.Rows.Count > 0)
                                    {
                                        dataRow["事業所コード"] = tokTable.Rows[0]["事業所コード"].ToString();
                                        dataRow["部課コード"] = tokTable.Rows[0]["部課コード"].ToString();
                                        dataRow["請求先コード"] = tokTable.Rows[0]["請求先コード"].ToString();
                                        dataRow["売上切捨区分"] = tokTable.Rows[0]["売上切捨区分"].ToString();
                                        dataRow["売上税区分"] = tokTable.Rows[0]["売上税区分"].ToString();
                                    }
                                    tokTable.Dispose();
                                }

                                DataTable shTable = null;
                                string scd = null;

                                if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                                {
                                    scd = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value.ToString();

                                    sql = "SELECT A.商品コード,A.商品名,A.規格,A.在庫管理区分,A.品種コード,A.メーカーコード,";
                                    sql = sql + " A.原価単価,A.単位コード,A.代表原価掛率,1 AS 掛率,";
                                    sql = sql + " A.外内税区分,A.消費税率,A.新消費税率,A.新消費税適用,";
                                    sql = sql + " A.メーカー品番,A.棚番,C.メーカー名,A.定価";
                                    sql = sql + " ,A.現在在庫数,A.受注残数,A.発注残数";
                                    sql = sql + " ,A.入数,A.分類, A.倉庫コード";                                          //2012/1 h.yamamoto add
                                    sql = sql + " FROM T商品マスタ AS A ";
                                    sql = sql + " LEFT JOIN Tメーカーマスタ AS C ON A.メーカーコード = C.メーカーコード";
                                    sql = sql + " WHERE A.商品コード = '" + scd + "'";

                                    shTable = sqlDb.ExecuteSql(sql, -1);
                                    if (shTable.Rows.Count > 0)
                                    {
                                        dataRow["原価単価"] = Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["原価単価"], 0));
                                        dataRow["原価金額"] = Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["原価単価"],0)) * Convert.ToDecimal(Utility.Nz(this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value,0));
                                        dataRow["粗利"] = Convert.ToDecimal(Utility.Nz(this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value, 0)) - Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["原価単価"], 0)) * Convert.ToDecimal(Utility.Nz(this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value, 0));
                                        dataRow["消費税率"] = Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["消費税率"],0));
                                        dataRow["新消費税率"] = Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["新消費税率"],0));
                                        dataRow["新消費税適用"] = shTable.Rows[0]["新消費税適用"].ToString();
                                        dataRow["品種コード"] = shTable.Rows[0]["品種コード"].ToString();
                                        dataRow["在庫数"] = Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["現在在庫数"],0));
                                        dataRow["受注残数"] = Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["受注残数"],0));
                                        dataRow["発注残数"] = Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["発注残数"],0));
                                        dataRow["メーカー名"] = shTable.Rows[0]["メーカー名"].ToString();
                                        dataRow["店コード"] = Utility.Right(dataRow["得意先コード"].ToString(), 4);
                                        dataRow["更新ビット"] = 0;
                                        dataRow["社コード"] = 1;
                                        dataRow["分類"] = shTable.Rows[0]["分類"];
                                        dataRow["倉庫コード"] = shTable.Rows[0]["倉庫コード"].ToString();
                                    }
                                }

                                if (ドラスタ区分 == "1")
                                {
                                    dataRow["社コード"] = ドラスタ_ヘッダ_社コード;
                                    dataRow["経費区分"] = ドラスタ_ヘッダ_経費区分;
                                    dataRow["直送区分"] = ドラスタ_ヘッダ_直送区分;
                                    dataRow["ＥＯＳ区分"] = ドラスタ_ヘッダ_ＥＯＳ区分;
                                    dataRow["客注区分"] = ドラスタ_ヘッダ_客注区分;
                                    dataRow["大分類コード"] = ドラスタ_明細_大分類コード;
                                    
                                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                                    {
                                        scd = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value.ToString();
                                        shTable = this.t商品変換マスタTableAdapter1.GetData(scd);
                                        if (shTable.Rows.Count > 0)
                                        {
                                            dataRow["ＥＯＳ商品コード"] = shTable.Rows[0]["ＥＯＳ商品コード"].ToString();
                                            dataRow["ＪＡＮコード"] = shTable.Rows[0]["ＥＯＳ商品コード"].ToString();
                                            dataRow["表示価格"] = Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["表示価格"].ToString(),0));
                                            dataRow["店舗売価"] = Convert.ToDecimal(Utility.Nz(shTable.Rows[0]["店舗売価"].ToString(),0));
                                        }
                                        else
                                        {
                                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                                            {
                                                dataRow["表示価格"] = Convert.ToDecimal(Utility.Nz(this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value,0)) * 108 / 100;
                                                dataRow["店舗売価"] = Convert.ToDecimal(Utility.Nz(this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value,0)) * 108 / 100;
                                            }
                                            else
                                            {
                                                dataRow["表示価格"] = 0;
                                                dataRow["店舗売価"] = 0;
                                            }
                                        }
                                        sql = "SELECT SYO.商品名, SYO.メーカー品番, SYO.メーカーコード, SYO.棚番, SYO.規格, SYO.品種コード, ";
                                        sql += "SYO.分類, SYO.倉庫コード, SYO.形式寸法, SYO.材質, SYO.在庫管理区分, MEK.メーカー名 ";
                                        sql += "FROM T商品マスタ SYO ";
                                        sql += "LEFT JOIN Tメーカーマスタ MEK ON MEK.メーカーコード=SYO.メーカーコード ";
                                        sql += "WHERE SYO.商品コード='" + scd + "'";
                                        shTable = sqlDb.ExecuteSql(sql, -1);
                                        if (shTable.Rows.Count > 0)
                                        {
                                            dataRow["メーカー品番"] = shTable.Rows[0]["メーカー品番"].ToString();
                                            dataRow["メーカーコード"] = shTable.Rows[0]["メーカーコード"].ToString();
                                            dataRow["棚番"] = shTable.Rows[0]["棚番"].ToString();
                                            dataRow["規格"] = shTable.Rows[0]["規格"].ToString();
                                            dataRow["品種コード"] = shTable.Rows[0]["品種コード"].ToString();
                                            dataRow["形式寸法"] = shTable.Rows[0]["形式寸法"].ToString();
                                            dataRow["材質"] = shTable.Rows[0]["材質"].ToString();
                                            dataRow["在庫管理区分"] = shTable.Rows[0]["在庫管理区分"].ToString();
                                            dataRow["メーカー名"] = shTable.Rows[0]["メーカー名"].ToString();
                                            string buf = shTable.Rows[0]["商品名"].ToString();
                                            buf = CSharp.Japanese.Kanaxs.Kana.ToHankaku(buf);
                                            buf = CSharp.Japanese.Kanaxs.Kana.ToHankakuKana(buf);
                                            dataRow["ＥＯＳ商品名"] = Utility.Left(buf, 20);
                                            buf = dataRow["規格"].ToString();
                                            buf = CSharp.Japanese.Kanaxs.Kana.ToHankaku(buf);
                                            buf = CSharp.Japanese.Kanaxs.Kana.ToHankakuKana(buf);
                                            dataRow["ＥＯＳ規格"] = Utility.Left(buf, 9);
                                            dataRow["店コード"] = Utility.Right(dataRow["得意先コード"].ToString(), 4);
                                            dataRow["更新ビット"] = 0;
                                            dataRow["社コード"] = 1;
                                            dataRow["店コードＢ"] = Utility.Right(dataRow["得意先コード"].ToString(), 4);
                                            dataRow["本部原価単価"] = Convert.ToDecimal(Utility.Nz(this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value,0));
                                            dataRow["本部原価金額"] = Convert.ToDecimal(Utility.Nz(this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value,0));
                                            dataRow["納入単価"] = Convert.ToDecimal(Utility.Nz(this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value,0));
                                            //dataRow["分類"] = Utility.Right(dataRow["分類"].ToString(), 4);
                                            dataRow["分類"] = shTable.Rows[0]["分類"];
                                            //dataRow["倉庫コード"] = Utility.Right(dataRow["倉庫コード"].ToString(), 4);
                                            dataRow["倉庫コード"] = shTable.Rows[0]["倉庫コード"].ToString();
                                        }
                                    }
                                    
                                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString() == "92")
                                    {
                                        dataRow["発注区分"] = 1;
                                        dataRow["店コード"] = Utility.Right(dataRow["得意先コード"].ToString(), 4);
                                        dataRow["店コードＢ"] = Utility.Right(dataRow["得意先コード"].ToString(), 4);
                                        dataRow["社コード"] = 1;
                                        dataRow["ＥＯＳ区分"] = "21";
                                        dataRow["直送区分"] = 2;
                                    }
                                    else
                                    {
                                        string W_得意先コード = Utility.GetCode("T得意先マスタ", "得意先コード", "業態コード", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString());
                                        if (W_得意先コード == null)
                                        {
                                            W_得意先コード = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString();
                                        }
                                        dataRow["発注区分"] = 1;
                                        dataRow["店コード"] = Utility.Right(W_得意先コード, 4);
                                        dataRow["店コードＢ"] = Utility.Right(W_得意先コード, 4);
                                        dataRow["社コード"] = Utility.Left(W_得意先コード, 3);
                                        dataRow["ＥＯＳ区分"] = "21";
                                        dataRow["直送区分"] = 2;
                                    }

                                    shTable.Dispose();
                                    //sqlDb.Disconnect();

                                }
                        
                                sqlDb.Disconnect();
                                /*
                                if (ドラスタ_単価更新_単価更新フラグ == "1")
                                {
                                    dataRow["単価更新フラグ"] = true;
                                }
                                else
                                {
                                    dataRow["単価更新フラグ"] = false;
                                }
                                */
                                ////dataTable.Rows.Add(dataRow);


                                dataTable.Rows.Add(dataRow);
                                this.gcMultiRow1.DataSource = dataTable;

                            }

                            this.gcMultiRow1.Refresh();

                            if (index == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "売上行番号", 1);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "売上行番号", index);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注番号", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注番号", this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注行番号", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注行番号", this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商品コード", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商品コード", this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "数量", this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "数量", DBNull.Value);
                            }
                            if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "定価", this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "定価", DBNull.Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value.ToString() == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "納品掛率", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "納品掛率", this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value.ToString() == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "売上単価", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "売上単価", this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value.ToString() == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "原価掛率", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "原価掛率", this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value.ToString() == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "原価単価", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "原価単価", this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "売上金額", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "売上金額", this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答納期", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答納期", this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商名", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商名", this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "明細摘要", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "明細摘要", this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕入先コード", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕入先コード", this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕名", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕名", this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value);
                            }

                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Value = index + 1;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;
                            forceFlg = 0;

                            this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;

                            gcMultiRow1.Select();
                            gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Selected = true;
                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;
                case "伝票確認textBoxCell":
                    if (gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue == null)
                    {
                        return;
                    }
                    if (gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue.ToString() == "9")
                    {
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if (gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue.ToString() == "0")
                    {
                        gcMultiRow1.EndEdit();
                        // 編集した行のコミット処理
                        GrapeCity.Win.MultiRow.EditingActions.CommitRow.Execute(gcMultiRow1);

                        SqlDb sqlDb = new SqlDb();
                        sqlDb.Connect();

                        Form F_Kihon;         // F_会社基本
                        DateTime S_date;          // ｼｽﾃﾑ日付
                        
                        string strSQL;
                        long W_Syori_no;      // 処理番号
                        int Uriden_no;

                        string nonyusaki_cd = null;
                        string denpyo_tekiyo = null;
                        string tenpo_biko = null;
                        string sirsaki_biko = null;

                        DataTable WS = sqlDb.ExecuteSql("SELECT 処理番号,WS番号 FROM TＷＳ番号 WHERE ＫＥＹ=1", -1);

                        if (WS.Rows.Count > 0)
                        {
                            if (WS.Rows[0]["処理番号"] == null)
                            {
                                W_Syori_no = 1;
                            }
                            else
                            {
                                W_Syori_no = Convert.ToInt64(WS.Rows[0]["処理番号"]) + 1;
                            }
                        }
                        else
                        {
                            if (WS.Rows[0]["WS番号"] == null)
                            {
                                W_Syori_no = 1;
                            }
                            else
                            {
                                W_Syori_no = Convert.ToInt64(WS.Rows[0]["WS番号"]) * 10000000 + 1;
                            }
                        }

                        WS.Dispose();

                        string nkubun = null;
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "0")
                        {
                            nkubun = "1";
                        }
                        else if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "1")
                        {
                            nkubun = "2";
                        }
                        else if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "2")
                        {
                            nkubun = "3";
                        }
                        else
                        {
                            nkubun = "5";
                        }

                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "0")
                        {
                            Uriden_no = 0;
                        }
                        else
                        {
                            Uriden_no = Convert.ToInt32( this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value);
                        }


                        
                        
                        if (this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value != null)
                        {
                            nonyusaki_cd = this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value.ToString();
                        }
                        if (this.gcMultiRow1.ColumnFooters[0].Cells["伝票摘要textBoxCell"].Value != null)
                        {
                            denpyo_tekiyo = this.gcMultiRow1.ColumnFooters[0].Cells["伝票摘要textBoxCell"].Value.ToString();
                        }
                        if (this.gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Value != null)
                        {
                            tenpo_biko = this.gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Value.ToString();
                        }
                        if (this.gcMultiRow1.ColumnFooters[0].Cells["仕入先備考textBoxCell"].Value != null)
                        {
                            sirsaki_biko = this.gcMultiRow1.ColumnFooters[0].Cells["仕入先備考textBoxCell"].Value.ToString();
                        }

                        string cst = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cst))
                        {
                            // フィールド名のマッピング
                            int j = 0;
                            foreach (var column in dataTable.Columns)
                            {
                                //if (column.ToString() != "処理年月" || column.ToString() != "粗利")
                                if (j<155)
                                {
                                    bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());
                                }
                                j++;
                            }
                            
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                if (dataTable.Rows[i]["得意先コード"] == null || dataTable.Rows[i]["得意先コード"].ToString() == "")
                                {
                                    dataTable.Rows[i].Delete();
                                }
                                else
                                {
                                    dataTable.Rows[i]["修正処理年月日"] = DateTime.Now;
                                    dataTable.Rows[i]["売上伝票番号"] = Uriden_no;
                                    dataTable.Rows[i]["処理コード"] = 4;
                                    dataTable.Rows[i]["入力区分"] = nkubun;
                                    dataTable.Rows[i]["処理日"] = DateTime.Now;
                                    dataTable.Rows[i]["処理番号"] = W_Syori_no;
                                    dataTable.Rows[i]["売上日"] = DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day;
                                    dataTable.Rows[i]["納入日"] = DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day;
                                    dataTable.Rows[i]["受注行"] = dataTable.Rows[i]["受注行番号"];
                                    dataTable.Rows[i]["原価掛率"] = (decimal)dataTable.Rows[i]["原価掛率"]/100;
                                    dataTable.Rows[i]["納品掛率"] = (decimal)dataTable.Rows[i]["納品掛率"] / 100;
                                    dataTable.Rows[i]["納入先コード"] = nkubun;
                                    dataTable.Rows[i]["伝票摘要"] = nkubun;
                                    dataTable.Rows[i]["店舗備考"] = nkubun;
                                    dataTable.Rows[i]["仕入先備考"] = nkubun;

                                }

                            }
                            bulkCopy.BulkCopyTimeout = 600; // in seconds
                            bulkCopy.DestinationTableName = dataTable.TableName; // テーブル名をSqlBulkCopyに教える
                            bulkCopy.WriteToServer(dataTable);

                            //bulkCopy.BulkCopyTimeout = 600; // in seconds
                            //bulkCopy.DestinationTableName = "T受注戻しファイル";
                            //bulkCopy.WriteToServer(dataTable);

                            // ----------------------------
                            // T_処理履歴テーブルセット
                            // ----------------------------
                            strSQL = "INSERT INTO T処理履歴テーブル ( ";
                            strSQL = strSQL + " 本支店区分, 処理コード, 処理名, 入力区分, 事業所コード, 処理番号,";
                            strSQL = strSQL + " 売上伝票番号, 仕入伝票番号, 入金番号, システム区分, 処理日, 更新フラグ,";
                            //strSQL = strSQL + " 受注番号, 仕入伝票番号, 入金番号, システム区分, 処理日, 更新フラグ,";
                            strSQL = strSQL + " オペレーターコード )";
                            strSQL = strSQL + " SELECT A.本支店区分, 4 AS 処理ＣＤ,";
                            strSQL = strSQL + "'売上計上' AS 処理名称,";
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "0")
                            {
                                strSQL = strSQL + "1 AS 区分,";
                            }
                            else if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "1")
                            {
                                strSQL = strSQL + "2 AS 区分,";
                            }
                            else if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "2")
                            {
                                strSQL = strSQL + "3 AS 区分,";
                            }
                            else
                            {
                                strSQL = strSQL + "5 AS 区分,";
                            }
                            strSQL = strSQL + "'1' AS 事業所, ";
                            strSQL = strSQL + W_Syori_no + " AS 処理番,";
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "0")
                            {
                                strSQL = strSQL + "0 AS 番号,";
                            }else{
                                strSQL = strSQL + "'" + this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value.ToString() + "' AS 番号,";
                            }
                            strSQL = strSQL + "0 AS 仕入番,";
                            strSQL = strSQL + "0 AS 入金番, ";
                            strSQL = strSQL + "101 AS システム,";
                            strSQL = strSQL + " GETDATE() AS 日付, ";
                            strSQL = strSQL + "1 AS 更新,";
                            strSQL = strSQL + "'" + this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString() + "' AS ＯＰ";
                            strSQL = strSQL + " FROM T会社基本 AS A;";
                            sqlDb.BeginTransaction();
                            sqlDb.ExecuteSql(strSQL, -1);
                            sqlDb.CommitTransaction();

                            strSQL = " UPDATE TＷＳ番号 SET 処理番号 = " + W_Syori_no.ToString();
                            strSQL = strSQL + " WHERE ＫＥＹ = 1;";
                            sqlDb.BeginTransaction();
                            sqlDb.ExecuteSql(strSQL, -1);
                            sqlDb.CommitTransaction();

                            sqlDb.Disconnect();

                            string connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
                            SqlConnection connection = new SqlConnection(connectionString);
                            // コネクションを開く
                            connection.Open();
                            // コマンド作成
                            SqlCommand command = connection.CreateCommand();
                            // ストアド プロシージャを指定
                            command.CommandType = CommandType.StoredProcedure;
                            // ストアド プロシージャ名を指定
                            command.CommandText = "pr_BG";
                            //command.BeginExecuteNonQuery();
                            command.ExecuteNonQuery();

                            if (gcMultiRow1.Rows.Count > 0)
                            {
                                dataTable = (DataTable)gcMultiRow1.DataSource;
                                dataTable.Clear();
                            }
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注番号受注残数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = 0;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["オンライン納入先コードtextBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["第2仕入先コードtextBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["在庫数numericUpDownCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["発注残数numericUpDownCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].Value = null;
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");

                            connection.Close();
                        }
                    }
                    break;
                case "数量numericUpDownCell":
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value !=null)
                    {
                        forceFlg = 1;

                        this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value =
                            (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value;

                        forceFlg = 0;

                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case "定価numericUpDownCell":
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value > 0)
                        {
                            forceFlg = 1;

                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value / (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * 100;

                            forceFlg = 0;

                        }
                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value > 0)
                        {
                            forceFlg = 1;

                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value;

                            forceFlg = 0;

                        }
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case "納品掛率numericUpDownCell":
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value != null)
                    {
                        forceFlg = 1;

                        this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value =
                            (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value / 100;

                        forceFlg = 0;

                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value > 0)
                        {
                            forceFlg = 1;

                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value;

                            forceFlg = 0;

                        }
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case "売上単価numericUpDownCell":

                    forceFlg = 1;
                    
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value != null)
                    {

                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value < (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Style.BackColor = Color.White;
                        }

                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Style.BackColor = Color.White;
                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value / (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * 100;
                        }
                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value;
                        }
                    }

                    forceFlg = 0;

                    break;

            }

        }

        public void FlushButton(Keys keyCode)
        {
            Button target = null;

            switch (keyCode)
            {
                case Keys.F3:
                    target = this.ButtonF3;
                    EditingActions.CommitRow.Execute(gcMultiRow1);
                    String cname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (cname == "得意先コードtextBoxCell")
                    {
                        得意先検索Form fsToksaki;
                        fsToksaki = new 得意先検索Form();
                        fsToksaki.Owner = this;
                        fsToksaki.Show();
                    }
                    else if (cname == "商品コードtextBoxCell")
                    {
                        商品検索Form fsSyohin;
                        fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                    }
                    else if (cname == "受注NOtextBoxCell")
                    {
                        伝票検索Form fsDenpyo;
                        fsDenpyo = new 伝票検索Form();
                        fsDenpyo.Owner = this;
                        fsDenpyo.Show();
                    }
                    else if (cname == "売伝NOtextBoxCell")
                    {
                        伝票検索計上Form fsDenpyoKeijyo;
                        fsDenpyoKeijyo = new 伝票検索計上Form();
                        fsDenpyoKeijyo.Owner = this;
                        fsDenpyoKeijyo.Show();
                    }
                    break;
                case Keys.F4:
                    target = this.ButtonF4;
                    EditingActions.CommitRow.Execute(gcMultiRow1);
                    String fname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (fname == "商品コードtextBoxCell" & gcMultiRow1.Rows.Count > 0)
                    {
                        forceFlg = 0;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                    }
                    break;
                case Keys.F5:
                    target = this.ButtonF5;
                    cname = gcMultiRow1.CurrentCellPosition.CellName;

                    if (cname == "商品コードtextBoxCell")
                    {
                        object val = gcMultiRow1.GetValue(gcMultiRow1.CurrentCellPosition.RowIndex, gcMultiRow1.CurrentCellPosition.CellIndex);
                        if (val != null)
                        {
                            商品別受注出荷履歴照会Form fsSyukaRireki;
                            fsSyukaRireki = new 商品別受注出荷履歴照会Form();
                            fsSyukaRireki.Owner = this;
                            fsSyukaRireki.ReceiveDataSyukaRireki = val.ToString();
                            fsSyukaRireki.Show();
                        }
                    }
                    break;
                case Keys.F9:
                    target = this.ButtonF9;
                    forceFlg = 1;
                    try
                    {
                        if (gcMultiRow1.Rows.Count > 0)
                        {
                            dataTable = (DataTable)gcMultiRow1.DataSource;
                            dataTable.Clear();
                        }
                        this.gcMultiRow1.ColumnHeaders[2].Cells["売上行番号textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["売上単価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["売上金額numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = 0;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value = "000000";
                        this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value = "000000";
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注残数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["納入先gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["伝票摘要textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["仕入先備考textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].Value = null;

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");

                    }
                    catch (DataException e)
                    {
                        // Process exception and return.
                        Console.WriteLine("Exception of type {0} occurred.",
                            e.GetType());
                    }
                    this.Refresh();
                    break;
                case Keys.F10:
                    target = this.ButtonF10;
                    forceFlg = 1;
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

        private void button1_Click(object sender, EventArgs e)
        {
            //fsSyohin.SendData = "Penguin";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //fsToksaki.SendData = "Penguin";
        }

        public string ReceiveDataSyohin
        {
            set
            {
                receiveDataSyohin = value;
                this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = receiveDataSyohin;
                gcMultiRow1.Select();
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
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
                this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = receiveDataToksaki;
                this.gcMultiRow1.EndEdit();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataToksaki;
            }
        }

        public string ReceiveDataJyutyuDenpyo
        {
            set
            {
                receiveDataJyutyuDenpyo = value;
                forceFlg = 0;
                this.gcMultiRow1.ColumnHeaders[0].Cells["受注NOtextBoxCell"].Value = receiveDataJyutyuDenpyo;
                //                gcMultiRow1.Select();
                //                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                SendKeys.Send("{ENTER}");
                int serchcode = Int32.Parse(receiveDataJyutyuDenpyo);

                // コントロールの描画を停止する
                gcMultiRow1.SuspendLayout();

                int id = System.Diagnostics.Process.GetCurrentProcess().Id;

                /*
                // XAMLファイル側で作成されたデータセットを取得
                受注ファイルdataSet = ((受注ファイルDataSet)(this.受注ファイルDataSet1));
                dataTable = 受注ファイルdataSet.T受注戻しファイル;
                //dataTable = 受注ファイルdataSet.T受注ファイル;

                // Userテーブルアダプタで、データセットにUserデータを読み込む
                T受注戻しファイルtableAdapter = new T受注戻しファイルTableAdapter();
                //T受注ファイルtableAdapter = new T受注ファイルTableAdapter();

                try
                {
                    //myTA.DeleteWK受注Bファイル();
                    //T受注ファイルtableAdapter.FillBy受注番号(受注ファイルdataSet.T受注ファイル, serchcode);
                    //dataTable = T受注ファイルtableAdapter.GetDataBy受注番号(serchcode);
                    dataTable = T受注戻しファイルtableAdapter.GetDataBy受注ファイルBy受注番号(serchcode);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                    return;
                }

                //dataTable = this.t受注ファイルTableAdapter1.GetDataBy受注番号(serchcode);
                //dataTable = this.wK受注BファイルTableAdapter1.GetData();
                //dataTable = myTA.GetData();

                //gcMultiRow1.DataSource = dataTable;
                //gcMultiRow1.DataSource = 受注ファイルdataSet.T受注ファイル;
                gcMultiRow1.DataSource = dataTable;

                if (dataTable.Rows.Count > 0)
                {
                    gcMultiRow1.SuspendLayout();

                    this.gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value = dataTable.Rows[0]["受注日"].ToString();
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                    forceFlg = 1;
                    this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataTable.Rows[0]["得意先コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                    forceFlg = 1;
                    this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                    if (dataTable.Rows[0]["納入先コード"] != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                    }
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["納名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(23);

                    //gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;

                    forceFlg = 1;
                    gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = 1;

                    forceFlg = 1;
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");

                    // コントロールの描画を再開する
                    gcMultiRow1.ResumeLayout();

                    //SendKeys.Send("{ENTER}");

                }
                else
                {
                    MessageBox.Show("データがありません");
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                }
                */
            }
            get
            {
                return receiveDataJyutyuDenpyo;
            }
        }

        public string ReceiveDataUriageDenpyo
        {
            set
            {
                receiveDataUriageDenpyo = value;
                forceFlg = 0;
                this.gcMultiRow1.ColumnHeaders[0].Cells["売伝NOtextBoxCell"].Value = receiveDataUriageDenpyo;
                //                gcMultiRow1.Select();
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "1")
                {
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上日textBoxCell");
                }
                else if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "2")
                {
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                }
                //SendKeys.Send("{ENTER}");
                //int serchcode = Int32.Parse(receiveDataUriageDenpyo);

                // コントロールの描画を停止する
                //gcMultiRow1.SuspendLayout();

                //int id = System.Diagnostics.Process.GetCurrentProcess().Id;

                /*
                // XAMLファイル側で作成されたデータセットを取得
                受注ファイルdataSet = ((受注ファイルDataSet)(this.受注ファイルDataSet1));
                dataTable = 受注ファイルdataSet.T受注戻しファイル;
                //dataTable = 受注ファイルdataSet.T受注ファイル;

                // Userテーブルアダプタで、データセットにUserデータを読み込む
                T受注戻しファイルtableAdapter = new T受注戻しファイルTableAdapter();
                //T受注ファイルtableAdapter = new T受注ファイルTableAdapter();

                try
                {
                    //myTA.DeleteWK受注Bファイル();
                    //T受注ファイルtableAdapter.FillBy受注番号(受注ファイルdataSet.T受注ファイル, serchcode);
                    //dataTable = T受注ファイルtableAdapter.GetDataBy受注番号(serchcode);
                    dataTable = T受注戻しファイルtableAdapter.GetDataBy受注ファイルBy受注番号(serchcode);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                    return;
                }

                //dataTable = this.t受注ファイルTableAdapter1.GetDataBy受注番号(serchcode);
                //dataTable = this.wK受注BファイルTableAdapter1.GetData();
                //dataTable = myTA.GetData();

                //gcMultiRow1.DataSource = dataTable;
                //gcMultiRow1.DataSource = 受注ファイルdataSet.T受注ファイル;
                gcMultiRow1.DataSource = dataTable;

                if (dataTable.Rows.Count > 0)
                {
                    gcMultiRow1.SuspendLayout();

                    this.gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value = dataTable.Rows[0]["受注日"].ToString();
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                    forceFlg = 1;
                    this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataTable.Rows[0]["得意先コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                    forceFlg = 1;
                    this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                    if (dataTable.Rows[0]["納入先コード"] != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                    }
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["納名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(23);

                    //gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;

                    forceFlg = 1;
                    gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = 1;

                    forceFlg = 1;
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");

                    // コントロールの描画を再開する
                    gcMultiRow1.ResumeLayout();

                    //SendKeys.Send("{ENTER}");

                }
                else
                {
                    MessageBox.Show("データがありません");
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                }
                */
            }
            get
            {
                return receiveDataUriageDenpyo;
            }
        }

        public string Receiveドラスタ_ヘッダ_Data
        {
            set
            {
                receiveドラスタ_ヘッダ_Data = value;
                arr = value.ToString().Split(',');
                ドラスタ_ヘッダ_社コード = arr[0];
                ドラスタ_ヘッダ_経費区分 = arr[1];
                ドラスタ_ヘッダ_直送区分 = arr[2];
                ドラスタ_ヘッダ_ＥＯＳ区分 = arr[3];
                ドラスタ_ヘッダ_客注区分 = arr[4];

            }
            get
            {
                return receiveドラスタ_ヘッダ_Data;
            }
        }

        public string Sendドラスタ_ヘッダ_Data
        {
            set
            {
                sendドラスタ_ヘッダ_Data = value;
            }
            get
            {
                return sendドラスタ_ヘッダ_Data;
            }
        }

        public string Receiveドラスタ_明細_Data
        {
            set
            {
                receiveドラスタ_明細_Data = value;
                ドラスタ_明細_大分類コード = value.ToString();

            }
            get
            {
                return receiveドラスタ_明細_Data;
            }
        }

        public string Sendドラスタ_明細_Data
        {
            set
            {
                sendドラスタ_明細_Data = value;
            }
            get
            {
                return sendドラスタ_明細_Data;
            }
        }

        public string Receiveドラスタ_欠品理由_Data
        {
            set
            {
                receiveドラスタ_欠品理由_Data = value;
                ドラスタ_欠品理由_欠品理由コード = value.ToString();

            }
            get
            {
                return receiveドラスタ_欠品理由_Data;
            }
        }

        public string Sendドラスタ_欠品理由_Data
        {
            set
            {
                sendドラスタ_欠品理由_Data = value;
            }
            get
            {
                return sendドラスタ_欠品理由_Data;
            }
        }

    }
}
