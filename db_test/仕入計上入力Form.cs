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
using db_test.発注ファイルDataSetTableAdapters;
using db_test.SPEEDDB管理DataSetTableAdapters;
using db_test.SPEEDDBVIEWDataSetTableAdapters;

namespace db_test
{
    public partial class 仕入計上入力Form : Form
    {
        private int forceFlg = 0;
        int int処理区分 = 0;
        private int beforeIndex = 0;
        private int currentIndex = 0;

        //商品検索Form fsSyohin;
        private string receiveDataSyohin = "";
        //仕入先検索Form fsToksaki;
        private string receiveDataSirsaki = "";
        //仕入伝票番号検索連携用
        private string receiveDataSirDenpyo = "";
        //発注番号検索連携用
        private string receiveDataHatyuDenpyo = "";

        private DataSet dataSet;
        DataTable dataTable = null;

        private string M仕入先名 = null;
        private string M仕入先コード = null;
        private string M事業所コード = null;
        private string M担当者コード = null;
        private string M仕入切捨区分 = null;
        private string M仕入税区分 = null;
        private string M仕入分類 = null;
        private string M諸口区分 = null;
        private string M掛率 = null;
        private string M最終支払年月日 = null;

        private string OPE事業所コード = null;

        private string 取引先コード = null;
        private string 発注区分 = null;
        private string 店コード = null;
        private string 店コードB = null;
        private string 納入先コード = null;
        private string 納名 = null;
        private string 大分類コード = null;
        private string ＪＡＮコード = null;
        private string ＥＯＳ商品コード = null;

        private 発注ファイルDataSet 発注ファイルdataSet;  // データセット
        private T発注ファイルTableAdapter T発注ファイルtableAdapter;  // Userテーブルアダプタ
        private T発注戻しファイルTableAdapter T発注戻しファイルtableAdapter;  // Userテーブルアダプタ
        private T処理履歴テーブルTableAdapter T処理履歴テーブルtableAdapter;  // Userテーブルアダプタ
        private SPEEDDB管理DataSet SPEEDDB管理dataSet;
        private vw_ShohinTableAdapter vw_ShohintableAdapter;
        private vw_ShoTourokuTableAdapter vw_ShoTourokutableAdapter;
        private vw_TokuisakiTableAdapter vw_TokuisakitableAdapter;
        private vw_Ryakumei_SirTableAdapter vw_Ryakumei_SirtableAdapter;
        private vw_SiiTourokuTableAdapter vw_SiiTourokutableAdapter;
        private vw_SiiresakiTableAdapter vw_SiiresakitableAdapter;
        private T会社基本TableAdapter T会社基本tableAdapter;

        GcComboBoxCell comboBoxCell01 = null;
        GcComboBoxCell comboBoxCell02 = null;
        GcComboBoxCell comboBoxCell03 = null;
        GcComboBoxCell comboBoxCell04 = null;
        GcComboBoxCell comboBoxCell05 = null;
        GcComboBoxCell comboBoxCell06 = null;

        public 仕入計上入力Form()
        {
            InitializeComponent();
        }

        private void 仕入計上入力Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 仕入計上入力Template();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.ScrollBars = ScrollBars.Vertical;

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
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入計上入力FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入計上入力FunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入計上入力FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入計上入力FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入計上入力FunctionKeyAction(Keys.F10), Keys.F10);

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
            gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
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

            comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入担当者gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";

            comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 仕入区分コード, 仕入区分名 FROM T仕入区分マスタ ORDER BY 仕入区分コード");
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.TextSubItemIndex = 0;
            comboBoxCell03.TextSubItemIndex = 1;
            comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell03.TextFormat = "[1]";

            comboBoxCell04 = this.gcMultiRow1.ColumnHeaders[0].Cells["支払月区分gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell04.DataSource = Utility.GetComboBoxData("SELECT 支払月区分コード, 支払月区分名 FROM T支払月区分マスタ");
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.TextSubItemIndex = 0;
            comboBoxCell04.TextSubItemIndex = 1;
            comboBoxCell04.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell04.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell04.TextFormat = "[1]";

            // 1;通常;2;返品;3;値引
            comboBoxCell05 = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入担当者gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell05.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
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

            forceFlg = 1;
            gcMultiRow1.ColumnHeaders[0].Cells["仕入日textBoxCell"].Value = DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day;
            gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value = "000000";
            gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Value = "000000";
            gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value = 1;
            gcMultiRow1.ColumnHeaders[0].Cells["仕入区分gcComboBoxCell"].Value = 1;
            gcMultiRow1.ColumnHeaders[0].Cells["支払月区分コードtextBoxCell"].Value = 1;
            gcMultiRow1.ColumnHeaders[0].Cells["支払月区分gcComboBoxCell"].Value = 1;
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

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (forceFlg == 1)
            {
                //e.Cancel = false;
                //forceFlg = 0;
                return;
            }
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
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

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            if (forceFlg == 1)
            {
                //e.Cancel = false;
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
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕伝NOtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕伝NOtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕伝NOtextBoxCell");
                        }
                    }

                    break;
                case "オペレーターコードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NOtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NOtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NOtextBoxCell");
                    }
                    break;
                case "仕伝NOtextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue == "1")
                    {
                        if (Convert.ToInt32(Utility.Nz(this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value, 0)) == 0)
                        {
                            if (e.MoveStatus == MoveStatus.MoveDown)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入日textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveUp)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveRight)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入日textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveLeft)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入日textBoxCell");
                            }
                        }
                        else
                        {
                            if (e.MoveStatus == MoveStatus.MoveDown)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入日textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveUp)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveRight)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入日textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveLeft)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入日textBoxCell");
                            }
                        }
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue == "2")
                    {
                        if (Convert.ToInt32(Utility.Nz(this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].EditedFormattedValue, 0)) == 0)
                        {
                            if (e.MoveStatus == MoveStatus.MoveDown)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕伝NOtextBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveUp)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveRight)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕伝NOtextBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveLeft)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕伝NOtextBoxCell");
                            }
                        }
                        else
                        {
                            if (e.MoveStatus == MoveStatus.MoveDown)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveUp)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveRight)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveLeft)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                            }
                            else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                            }
                        }
                    }
                    break;
                case "発注NOtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入日textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入日textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入日textBoxCell");
                    }
                    break;
                case "仕入日textBoxCell":
                    if (Convert.ToInt32(Utility.Nz(this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value, 0)) == 0)
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NOtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注NOtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入区分コードtextBoxCell");
                        }
                    }
                    else
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕伝NOtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕伝NOtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                        }
                    }
                    break;
                case "伝票摘要textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "支払月区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "支払月区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "支払月区分コードtextBoxCell");
                    }
                    break;
                case "支払月区分コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入行番号textBoxCell");
                    }
                    break;
                /*
                case "仕入行番号textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    break;
                case "発注番号textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    break;
                case "発注行番号textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    break;
                case "商品コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
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
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "数量numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "数量numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価掛率numericUpDownCell");
                    }
                    break;
                case "原価掛率numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "定価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "定価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入単価numericUpDownCell");
                    }
                    break;
                case "仕入単価numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    break;
                /*
                case "仕入金額numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    break;
                */
                case "明細摘要textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    break;
                case "確認textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入行番号textBoxCell");
                    }
                    break;
                /*
                case "完了フラグcheckBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    break;
                 */
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            string strSQL = null;
            int len_sir = 0;
            string sircd = null;
            string opecd = null;
            string opejigyocd = null;
            int hatyuno = 0;

            if (forceFlg == 1)
            {
                //forceFlg = 0;
                return;
            }
            switch (e.CellName)
            {
                case "確認textBoxCell":
                    if (e.FormattedValue == null)
                    {
                        e.Cancel = true;
                        this.gcMultiRow1.EndEdit();
                    }
                    else if (e.FormattedValue.ToString() == "0")
                    {
                        e.Cancel = false;
                    }
                    else if (e.FormattedValue.ToString() == "9")
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                        this.gcMultiRow1.EndEdit();
                    }
                    break;
                case "処理区分textBoxCell":
                    if (e.FormattedValue == null)
                    {
                        e.Cancel = true;
                        this.gcMultiRow1.EndEdit();
                    }
                    else if (e.FormattedValue.ToString() == "0")
                    {
                        e.Cancel = false;
                    }
                    else if (e.FormattedValue.ToString() == "1")
                    {
                        e.Cancel = false;
                    }
                    else if (e.FormattedValue.ToString() == "2")
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
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
                            MessageBox.Show("マスタに存在しません。", "仕入計上入力");
                            e.Cancel = true;
                        }
                    }
                    break;
                case "仕入担当者コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell02, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            MessageBox.Show("マスタに存在しません。", "仕入計上入力");
                            e.Cancel = true;
                        }
                    }
                    break;
                case "仕入区分コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell03, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            MessageBox.Show("マスタに存在しません。", "仕入計上入力");
                            e.Cancel = true;
                        }
                    }
                    break;
                case "支払月区分コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell04, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            MessageBox.Show("マスタに存在しません。", "仕入計上入力");
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
                            MessageBox.Show("マスタに存在しません。", "仕入計上入力");
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
                            MessageBox.Show("マスタに存在しません。", "仕入計上入力");
                            e.Cancel = true;
                        }
                    }
                    else 
                    {
                        if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                        {
                            vw_ShohintableAdapter = new vw_ShohinTableAdapter();
                            DataTable 商品dataTable = vw_ShohintableAdapter.GetDataBy商品コード(this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value.ToString());
                            if (商品dataTable.Rows.Count > 0)
                            {
                                e.Cancel = false;
                            }
                            else
                            {
                                MessageBox.Show("マスタに存在しません。", "仕入計上入力");
                                e.Cancel = true;
                            }
                        }
                    }

                    break;
                case "仕入先コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        vw_SiiresakitableAdapter = new vw_SiiresakiTableAdapter();
                        int buf = Convert.ToInt32(e.FormattedValue.ToString());
                        string buf2 = String.Format("{0:000000}", buf);
                        DataTable 仕入先dataTable = vw_SiiresakitableAdapter.GetDataBy仕入先コード(buf2);
                        if (仕入先dataTable.Rows.Count > 0)
                        {
                            if (Convert.ToDateTime(this.gcMultiRow1.ColumnHeaders[0].Cells["仕入日textBoxCell"].Value) <= Convert.ToDateTime(仕入先dataTable.Rows[0]["最終支払年月日"]))
                            {
                                MessageBox.Show("支払締切日以前を、入力する事は出来ません。", "仕入計上入力");
                                e.Cancel = true;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("マスタに存在しません。", "仕入計上入力");
                            e.Cancel = true;
                        }
                    }

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
                case "発注NOtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].EditedFormattedValue == null)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].EditedFormattedValue.ToString() == "000000")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            string jno = this.gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].EditedFormattedValue.ToString();
                            string sql = "SELECT count(*) FROM T発注ファイル WHERE 発注番号='" + jno + "'";
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

                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            if (forceFlg == 1)
            {
                //e.Cancel = false;
                //forceFlg = 0;
                return;
            }
            switch (e.CellName)
            {
                case "確認textBoxCell":
                    //gcMultiRow1.EndEdit();
                    //gcMultiRow1.NotifyCurrentCellDirty(true);
                    break;
                case "処理区分textBoxCell":
                    //gcMultiRow1.EndEdit();
                    //gcMultiRow1.NotifyCurrentCellDirty(true);
                    break;
                case "オペレーターコードtextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;
                        }
                    }
                    break;
                case "担当者コードtextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell02, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value;
                        }
                    }
                    break;
                case "仕入区分コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell03, this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value.ToString()))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value;

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
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell04, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["請求月区分コードtextBoxCell"].Value;

                        }
                    }
                    break;
                case "ドラスタ区分コードtextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell05, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["ドラスタ区分コードtextBoxCell"].Value;

                        }
                    }
                    break;
                case "倉庫コードtextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell06, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value;

                        }
                    }
                    break;
                case "納入先コードtextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell04, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value;
                        }
                    }
                    break;
                case "仕入先コードtextBoxCell":
                    //this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].EditedFormattedValue;
                    break;
                case "商品コードtextBoxCell":
                    //this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].EditedFormattedValue;
                    break;
                case "支払月区分コードtextBoxCell":
                    gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].Value = '1';
                    break;
                case "仕入行番号textBoxCell":
                    //this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].EditedFormattedValue;
                    break;

            }
        }

        void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            if (forceFlg == 1)
            {
                //e.Cancel = false;
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
                case "オペレーター名textBoxCell":
                    _serchcode = "0";

                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString()))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;
                        }
                    }
                    break;
                case "処理区分textBoxCell":
                    //string _serchcode = "0";
                    
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value == "1" )
                    {
                        //gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Selected = true;
                        //SendKeys.Send("{ENTER}");
                        gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].ReadOnly = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Selectable = false;

                        gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].ReadOnly = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Selectable = false;

                        gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].ReadOnly = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Selectable = true;

                        string cellName = "発注NOtextBoxCell";
                        int cellIndex = gcMultiRow1.Template.ColumnHeaders[0].Cells[cellName].CellIndex;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, cellIndex);

                    }
                    else
                    {

                        gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].ReadOnly = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Selectable = true;

                        gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].ReadOnly = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Selectable = true;

                        gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].ReadOnly = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Selectable = true;


                        //string cellName = "オペレーターコードtextBoxCell";
                        //int cellIndex = gcMultiRow1.Template.Row.Cells[cellName].CellIndex;

                        //gcMultiRow1.Focus();
                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, cellIndex);

                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, cellIndex);
                        //SendKeys.Send("{ENTER}");

                    }
                    break;
                case "仕入日textBoxCell":
                    strName = "オペレーターコードtextBoxCell";
                    intIndex = gcMultiRow1.Template.ColumnHeaders[0].Cells[strName].CellIndex;
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, intIndex);
                    break;
                case "オペレーターコードtextBoxCell":
                    _serchcode = "0";

                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                    {
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;
                        //_serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;
                        //DataRow[] dataRow = this.sPEEDDBDataSet.Tオペレーターマスタ.Select("オペレーターコード = " + _serchcode);
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(1);
                        //Object opname = this.queriesTableAdapter1.オペレータ名ScalarQuery(_serchcode);
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = opname ;
                        //gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Selected = true;
                        //SendKeys.Send("{ENTER}");
                    }
                    break;
                case "仕入先コードtextBoxCell":
                    _serchcode = "0";

                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value != null)
                    {
                        int buf = Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value.ToString());
                        string buf2 = String.Format("{0:000000}", buf);
                        _serchcode = buf2;

                        vw_SiiresakiTableAdapter vw_SiiresakitableAdapter = new vw_SiiresakiTableAdapter();
                        DataTable 仕入先dataTable = vw_SiiresakitableAdapter.GetDataBy仕入先コード(_serchcode);
                        if (仕入先dataTable.Rows.Count > 0)
                        {
                            M仕入先コード = 仕入先dataTable.Rows[0]["仕入先コード"].ToString();
                            M仕入先名 = 仕入先dataTable.Rows[0]["仕入先名"].ToString();
                            M仕入分類 = 仕入先dataTable.Rows[0]["仕入分類"].ToString();
                            M事業所コード = 仕入先dataTable.Rows[0]["事業所コード"].ToString();
                            M担当者コード = 仕入先dataTable.Rows[0]["担当者コード"].ToString();
                            M仕入切捨区分 = 仕入先dataTable.Rows[0]["仕入切捨区分"].ToString();
                            M仕入税区分 = 仕入先dataTable.Rows[0]["仕入税区分"].ToString();
                            M諸口区分 = 仕入先dataTable.Rows[0]["諸口区分"].ToString();
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕名textBoxCell"].Value = M仕入先名;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入担当者コードtextBoxCell"].Value = M担当者コード;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入担当者gcComboBoxCell"].Value = M担当者コード;
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value != null)
                            {
                               this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value=1;
                               this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫gcComboBoxCell"].Value = 1;
                            }
                            forceFlg = 0;
                        }
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入区分コードtextBoxCell");
                        //SendKeys.Send("{ENTER}");
                    }
                    break;
                case "仕入区分コードtextBoxCell":
                    gcMultiRow1.ColumnHeaders[1].Cells["仕入行番号textBoxCell"].Selected = true;
                    gcMultiRow1.ColumnHeaders[1].Cells["仕入行番号textBoxCell"].Value = '1';
                    SendKeys.Send("{ENTER}");
                    break;
                case "発注NOtextBoxCell":
                    _serchcode = null;
                    serchcode = 0;

                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Value != null)
                    {
                        _serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Value;
                        serchcode = Int32.Parse(_serchcode);

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();
                        dataTable = this.t仕入修正戻ファイルTableAdapter1.GetDataBy仕入修正戻ファイルfrom発注ファイル(serchcode);
                    
                        this.gcMultiRow1.DataSource = dataTable;

                        if (dataTable.Rows.Count > 0)
                        {
                            gcMultiRow1.SuspendLayout();

                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入日textBoxCell"].Value = dataTable.Rows[0]["仕入日"].ToString();
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = dataTable.Rows[0]["仕入先コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕名textBoxCell"].Value = dataTable.Rows[0]["仕名"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value = dataTable.Rows[0]["仕入区分コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分gcComboBoxCell"].Value = dataTable.Rows[0]["仕入区分コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入担当者コードtextBoxCell"].Value = dataTable.Rows[0]["仕入担当者コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入担当者gcComboBoxCell"].Value = dataTable.Rows[0]["仕入担当者コード"];
                            forceFlg = 0;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                            //if (dataTable.Rows[0]["納入先コード"] != null)
                            //{
                            //    this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                            //}
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["納名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(23);

                            gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].Selected = true;

                            gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].Value = 1;

                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "仕入行番号textBoxCell");

                            // コントロールの描画を再開する
                            gcMultiRow1.ResumeLayout();

                            //SendKeys.Send("{ENTER}");
                        }
                        else
                        {
                            MessageBox.Show("データがありません");
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "発注NOtextBoxCell");
                        }

                    }
                    break;
                case "仕伝NOtextBoxCell":
                    _serchcode = null;
                    serchcode = 0;

                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value != null)
                    {
                        _serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value;
                        serchcode = Int32.Parse(_serchcode);

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();
                        dataTable = this.t仕入修正戻ファイルTableAdapter1.GetDataBy仕入修正戻ファイルfrom仕入明細ファイル(serchcode);

                        if (dataTable.Rows.Count > 0)
                        {
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入日textBoxCell"].Value = dataTable.Rows[0]["仕入日"].ToString();
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = dataTable.Rows[0]["仕入先コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕名textBoxCell"].Value = dataTable.Rows[0]["仕名"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value = dataTable.Rows[0]["仕入区分コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分gcComboBoxCell"].Value = dataTable.Rows[0]["仕入区分コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入担当者コードtextBoxCell"].Value = dataTable.Rows[0]["仕入担当者コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["仕入担当者gcComboBoxCell"].Value = dataTable.Rows[0]["仕入担当者コード"];
                            forceFlg = 0;

                            this.gcMultiRow1.DataSource = dataTable;

                            // コントロールの描画を再開する
                            gcMultiRow1.ResumeLayout();
                        }

                        //gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Selected = true;

                        //his.gcMultiRow1.ColumnHeaders[1].Cells["受注行番号textBoxCell"].Value = 1;

                        //SendKeys.Send("{ENTER}");

                    }
                    break;
                case "仕入行番号textBoxCell":
                    _serchcode = "0";
                    serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].Value.ToString();
                        serchcode = Int32.Parse(_serchcode);

                        if (gcMultiRow1.Rows.Count >= serchcode)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注番号");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注行番号");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商品コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号発注残数numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注番号発注残数");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "数量");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "定価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "原価掛率");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "仕入単価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "仕入金額");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答納期");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "明細摘要");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "完了フラグ");
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号発注残数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = null;
                        }

                        //this.gcMultiRow1.SetValue(serchcode2 - 1, 0,this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value);
                        /*
                        DataTable dataTable = this.t受注ファイルTableAdapter1.GetDataBy受注番号(serchcode)this.gcMultiRow1.SetValue(serchcode2 - 1, 0);

                        this.gcMultiRow1.DataSource = dataTable;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["仕入日textBoxCell"].Value = dataRow[0].ItemArray.GetValue(11);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(68);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(14);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(15);
                        this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(85);
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
                case "発注番号textBoxCell":
                    _serchcode = null;
                    serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value.ToString();
                        serchcode = Int32.Parse(_serchcode);

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout();
                        dataTable = this.t仕入修正戻ファイルTableAdapter1.GetDataBy仕入修正戻ファイルfrom発注ファイル(serchcode);
                        //this.gcMultiRow1.DataSource = dataTable;

                        if (dataTable.Rows.Count > 0)
                        {

                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value != dataTable.Rows[0]["仕入先コード"])
                            {
                                MessageBox.Show("仕入先コードが違います");
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "発注番号textBoxCell");
                            }

                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "発注行番号textBoxCell");

                        }
                        else
                        {
                            MessageBox.Show("データがありません");
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "発注番号textBoxCell");
                        }

                    }
                    break;
                case "発注行番号textBoxCell":
                    _serchcode = null;
                    serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value != null)
                    {
                        if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value != null)
                        {
                            _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value.ToString();
                            serchcode = Int32.Parse(_serchcode);
                            _serchcode2 = this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value.ToString();
                            serchcode2 = Int32.Parse(_serchcode2);

                            // コントロールの描画を停止する
                            gcMultiRow1.SuspendLayout();
                            dataTable = this.t仕入修正戻ファイルTableAdapter1.GetDataBy仕入修正戻ファイルfrom発注ファイル明細(serchcode, serchcode2);
                            //this.gcMultiRow1.DataSource = dataTable;

                            if (dataTable.Rows.Count > 0)
                            {

                                this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value = dataTable.Rows[0]["発注番号"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = dataTable.Rows[0]["発注行番号"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = dataTable.Rows[0]["商品コード"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号発注残数numericUpDownCell"].Value = dataTable.Rows[0]["発注番号発注残数"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = dataTable.Rows[0]["数量"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = dataTable.Rows[0]["定価"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = dataTable.Rows[0]["原価掛率"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value = dataTable.Rows[0]["仕入単価"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value = dataTable.Rows[0]["仕入金額"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = dataTable.Rows[0]["回答納期"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = dataTable.Rows[0]["回答コード"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = dataTable.Rows[0]["回答名"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = dataTable.Rows[0]["商名"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = dataTable.Rows[0]["明細摘要"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = dataTable.Rows[0]["完了フラグ"];

                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "数量numericUpDownCell");

                            }
                            else
                            {
                                MessageBox.Show("データがありません");
                                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                            }
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
                            dataTable = this.t仕入修正戻ファイルTableAdapter1.GetDataBy();
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
                                this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["単価"];
                                //this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["原価単価"];
                            }
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = 商品dataTable.Rows[0]["定価"];
                            //this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["代表納品掛率"] * 100;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["単価"];
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["代表原価掛率"] * 100;
                            //this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["原価単価"];
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = 商品dataTable.Rows[0]["商品名"].ToString();
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["在庫数numericUpDownCell"].Value = 商品dataTable.Rows[0]["現在在庫数"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["M発注残数numericUpDownCell"].Value = 商品dataTable.Rows[0]["発注残数"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["現在在庫数"] - (decimal)商品dataTable.Rows[0]["受注残数"]; ;
                        }
                        if (会社基本dataTable.Rows.Count > 0)
                        {
                            if (会社基本dataTable.Rows[0]["単価管理"].Equals(true) & 会社基本dataTable.Rows[0]["得掛率管理"].Equals(true) & 商品dataTable.Rows[0]["諸口区分"].Equals(false))
                            {
                                this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value = Math.Truncate((double)商品dataTable.Rows[0]["定価"] * (double)会社基本dataTable.Rows[0]["得掛率"]);
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
                        if ((string)gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].EditedFormattedValue == "0")
                        {
                            int index = Int32.Parse(gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].EditedFormattedValue.ToString());
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
                                //dataRow["発注区分"] = 1;
                                dataRow["処理コード"] = 9;
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
                                //if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value != null)
                                //{
                                //    dataRow["処理区"] = "1";
                                //}
                                //else
                                //{
                                //    dataRow["処理区"] = "0";
                                //}
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value != null)
                                {
                                    dataRow["仕入伝票番号"] = Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value);
                                }
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Value != null)
                                {
                                    dataRow["発注番号"] = Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Value);
                                }
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["仕入日textBoxCell"].Value != null)
                                {
                                    dataRow["仕入日"] = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入日textBoxCell"].Value.ToString();
                                }
                                dataRow["仕入先コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value.ToString();
                                dataRow["仕名"] = this.gcMultiRow1.ColumnHeaders[0].Cells["仕名textBoxCell"].Value.ToString();
                                dataRow["納期"] = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入日textBoxCell"].Value.ToString();
                                dataRow["エラーフラグ"] = 0;
                                dataRow["在庫管理区分"] = 0;
                                dataRow["チェック"] = 0;
                                dataRow["完了フラグ"] = 0;
                                dataRow["システム区分"] = 101;
                                dataRow["WS_ID"] = "04";
                                //dataRow["単価更新フラグ"] = 0;
                                //dataRow["返品区分"] = 0;
                                //dataRow["入金チェック"] = 0;
                                //dataRow["検収チェック"] = 0;
                                //dataRow["発行済フラグ"] = 0;
                                dataRow["発注更新フラグ"] = 0;
                                dataRow["入荷更新フラグ"] = 0;
                                dataRow["仕入先更新フラグ"] = 0;
                                dataRow["商品更新フラグ"] = 0;
                                dataRow["商品倉庫更新フラグ"] = 0;
                                dataRow["モニター発行フラグ"] = 0;
                                dataRow["オペレーターコード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString();
                                dataRow["仕入担当者コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入担当者コードtextBoxCell"].Value.ToString();
                                dataRow["仕入区分コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value.ToString();
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["支払月区分コードtextBoxCell"].Value != null)
                                {
                                    dataRow["支払月区分コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["支払月区分コードtextBoxCell"].Value.ToString();
                                }
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value != null)
                                {
                                    dataRow["倉庫コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["倉庫コードtextBoxCell"].Value.ToString();
                                }

                                //if (this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value != null)
                                //{
                                //    dataRow["納入先コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value.ToString();
                                //}
                                //if (this.gcMultiRow1.ColumnFooters[0].Cells["伝票摘要textBoxCell"].Value != null)
                                //{
                                //    dataRow["伝票摘要"] = this.gcMultiRow1.ColumnHeaders[0].Cells["伝票摘要textBoxCell"].Value.ToString();
                                //}
                                //if (this.gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Value != null)
                                //{
                                //    dataRow["店舗備考"] = this.gcMultiRow1.ColumnHeaders[0].Cells["店舗備考textBoxCell"].Value.ToString();
                                //}

                                //dataRow["仕入累計"] = 0;

                                SqlDb sqlDb = new SqlDb();
                                sqlDb.Connect();　　//

                                //事業所コード、部課コード、処理日の時刻、請求先コード
                                if (this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value != null)
                                {
                                    DataTable sirTable = null;
                                    string sircd = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value.ToString();
                                    sql = "SELECT SIR.事業所コード,SIR.仕入分類,SIR.仕入切捨区分,SIR.仕入税区分 ";
                                    sql += "FROM vw_Siiresaki SIR ";
                                    sql += "LEFT JOIN T担当者マスタ TAN ON TAN.担当者コード=SIR.担当者コード ";
                                    sql += "WHERE SIR.仕入先コード='" + sircd + "'";
                                    sirTable = sqlDb.ExecuteSql(sql, -1);
                                    if (sirTable.Rows.Count > 0)
                                    {
                                        dataRow["事業所コード"] = sirTable.Rows[0]["事業所コード"].ToString();
                                        dataRow["仕入分類"] = sirTable.Rows[0]["仕入分類"].ToString();
                                        dataRow["仕入切捨区分"] = sirTable.Rows[0]["仕入切捨区分"].ToString();
                                        dataRow["仕入税区分"] = sirTable.Rows[0]["仕入税区分"].ToString();
                                    }
                                    sirTable.Dispose();
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
                                        //dataRow["原価単価"] = shTable.Rows[0]["原価単価"];
                                        //dataRow["原価金額"] = (decimal)shTable.Rows[0]["原価単価"] * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value;
                                        //dataRow["粗利"] = (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value - (decimal)shTable.Rows[0]["原価単価"] * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value;
                                        dataRow["消費税率"] = shTable.Rows[0]["消費税率"];
                                        dataRow["新消費税率"] = shTable.Rows[0]["新消費税率"];
                                        dataRow["新消費税適用"] = shTable.Rows[0]["新消費税適用"].ToString();
                                        dataRow["品種コード"] = shTable.Rows[0]["品種コード"].ToString();
                                        dataRow["在庫数"] = shTable.Rows[0]["現在在庫数"];
                                        dataRow["受注残数"] = shTable.Rows[0]["受注残数"];
                                        dataRow["発注残数"] = shTable.Rows[0]["発注残数"];
                                        //dataRow["メーカー名"] = shTable.Rows[0]["メーカー名"].ToString();
                                        //dataRow["店コード"] = Utility.Right(dataRow["得意先コード"].ToString(), 4);
                                        //dataRow["更新ビット"] = 0;
                                        //dataRow["社コード"] = 1;
                                        dataRow["分類"] = shTable.Rows[0]["分類"];
                                        dataRow["倉庫コード"] = shTable.Rows[0]["倉庫コード"].ToString();
                                    }
                                }

                                sqlDb.Disconnect();

                                dataTable.Rows.Add(dataRow);
                                this.gcMultiRow1.DataSource = dataTable;

                            }

                            this.gcMultiRow1.Refresh();

                            if (index == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕入行番号", 1);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕入行番号", index);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商品コード", this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商品コード", DBNull.Value);
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
                            //if (this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value.ToString() == null)
                            //{
                            //    this.gcMultiRow1.SetValue(index - 1, "納品掛率", DBNull.Value);
                            //}
                            //else
                            //{
                            //    this.gcMultiRow1.SetValue(index - 1, "納品掛率", this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value);
                            //}
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value.ToString() == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕入単価", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕入単価", this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value);
                            }
                            //if (this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value.ToString() == null)
                            //{
                            //    this.gcMultiRow1.SetValue(index - 1, "原価掛率", DBNull.Value);
                            //}
                            //else
                            //{
                            //    this.gcMultiRow1.SetValue(index - 1, "原価掛率", this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value);
                            //}
                            //if (this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value.ToString() == null)
                            //{
                            //    this.gcMultiRow1.SetValue(index - 1, "原価単価", DBNull.Value);
                            //}
                            //else
                            //{
                            //    this.gcMultiRow1.SetValue(index - 1, "原価単価", this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value);
                            //}
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕入金額", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "仕入金額", this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答納期", this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答納期", DBNull.Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答コード", this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答コード", DBNull.Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答名", this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答名", DBNull.Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商名", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商名", this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "明細摘要", this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "明細摘要", DBNull.Value);
                            }
                            /*
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注有無区分", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注有無区分", this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Value);
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
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注数", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注数", this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注摘要", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注摘要", this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value);
                            }
                            */
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].Value = index + 1;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            /*
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = null;
                            */
                            this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;

                            gcMultiRow1.Select();
                            //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "仕入行番号textBoxCell");

                            gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].Selected = true;
                            SendKeys.Send("{ENTER}");
                            
                        }
                    }
                    break;
                case "伝票確認textBoxCell":
                    if ((string)gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue == "9")
                    {
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if ((string)gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue == "0")
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
                            Uriden_no = Convert.ToInt32( this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value);
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
                                if (dataTable.Rows[i]["仕入先コード"] == null || dataTable.Rows[i]["仕入先コード"].ToString() == "")
                                {
                                    dataTable.Rows[i].Delete();
                                }
                                else
                                {
                                    dataTable.Rows[i]["修正処理年月日"] = DateTime.Now;
                                    dataTable.Rows[i]["仕入伝票番号"] = Uriden_no;
                                    dataTable.Rows[i]["処理コード"] = 9;
                                    dataTable.Rows[i]["入力区分"] = nkubun;
                                    dataTable.Rows[i]["処理日"] = DateTime.Now;
                                    dataTable.Rows[i]["処理番号"] = W_Syori_no;
                                    dataTable.Rows[i]["仕入日"] = DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day;
                                    //dataTable.Rows[i]["納入日"] = DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day;
                                    //dataTable.Rows[i]["受注行"] = dataTable.Rows[i]["受注行番号"];
                                    //dataTable.Rows[i]["原価掛率"] = (decimal)dataTable.Rows[i]["原価掛率"]/100;
                                    //dataTable.Rows[i]["納品掛率"] = (decimal)dataTable.Rows[i]["納品掛率"] / 100;

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
                            strSQL = strSQL + " SELECT A.本支店区分, 9 AS 処理ＣＤ,";
                            strSQL = strSQL + "'仕入計上' AS 処理名称,";
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
                            strSQL = strSQL + "0 AS 売上番,";
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "0")
                            {
                                strSQL = strSQL + "0 AS 番号,";
                            }else{
                                strSQL = strSQL + "'" + this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value.ToString() + "' AS 番号,";
                            }
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
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = null;
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

                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");

                            connection.Close();
                        }
                    }
                    break;
                case "数量numericUpDownCell":
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value != null &
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value =
                            (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value;
                    }
                    break;
                case "定価numericUpDownCell":
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value / (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * 100;
                        }
                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value;
                        }
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case "原価掛率numericUpDownCell":
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value =
                            (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value / 100;
                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value;
                        }
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case "仕入単価numericUpDownCell":
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value != null )
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value > 0)
                        {
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                            {
                                if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value > 0)
                                {
                                    this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value =
                                        (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value /
                                        (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * 100;
                                }
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value != null)
                            {
                                if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value > 0)
                                {
                                    this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value =
                                        (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value *
                                        (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value;
                                }
                            }
                        }
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
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
                    if (cname == "仕入先コードtextBoxCell")
                    {
                        仕入先検索Form fsSirsaki;
                        fsSirsaki = new 仕入先検索Form();
                        fsSirsaki.Owner = this;
                        fsSirsaki.Show();
                    }
                    else if (cname == "商品コードtextBoxCell")
                    {
                        商品検索Form fsSyohin;
                        fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                    }
                    else if (cname == "発注NOtextBoxCell")
                    {
                        伝票検索Form fsDenpyo;
                        fsDenpyo = new 伝票検索Form();
                        fsDenpyo.Owner = this;
                        fsDenpyo.Show();
                    }
                    else if (cname == "仕伝NOtextBoxCell")
                    {
                        伝票検索計上Form fsDenpyo;
                        fsDenpyo = new 伝票検索計上Form();
                        fsDenpyo.Owner = this;
                        fsDenpyo.Show();
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
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入行番号textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注番号発注残数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["数量numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入単価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入金額numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["完了フラグcheckBoxCell"].Value = 0;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value = "000000";
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Value = "000000";
                        this.gcMultiRow1.ColumnHeaders[0].Cells["M現在在庫数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["M発注残数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].Value = null;
                        /*
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["納入先コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["納入先gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["伝票摘要textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["店舗備考textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["仕入先備考textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].Value = null;
                        */
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");

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

        public string ReceiveDataSirsaki
        {
            set
            {
                receiveDataSirsaki = value;
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = receiveDataSirsaki;
            }
            get
            {
                return receiveDataSirsaki;
            }
        }

        public string ReceiveDataHatyuDenpyo
        {
            set
            {
                receiveDataHatyuDenpyo = value;
                forceFlg = 0;
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注NOtextBoxCell"].Value = receiveDataHatyuDenpyo;
                //                gcMultiRow1.Select();
                //                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                SendKeys.Send("{ENTER}");
                int serchcode = Int32.Parse(receiveDataHatyuDenpyo);

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
                return receiveDataHatyuDenpyo;
            }
        }

        public string ReceiveDataSirDenpyo
        {
            set
            {
                receiveDataSirDenpyo = value;
                forceFlg = 0;
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕伝NOtextBoxCell"].Value = receiveDataSirDenpyo;
                //                gcMultiRow1.Select();
                //                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                SendKeys.Send("{ENTER}");
                int serchcode = Int32.Parse(receiveDataSirDenpyo);

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

                    this.gcMultiRow1.ColumnHeaders[0].Cells["仕入日textBoxCell"].Value = dataTable.Rows[0]["受注日"].ToString();
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                    forceFlg = 1;
                    this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = dataTable.Rows[0]["仕入先コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                    forceFlg = 1;
                    this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分コードtextBoxCell"].Value = dataTable.Rows[0]["仕入区分コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["仕入区分gcComboBoxCell"].Value = dataTable.Rows[0]["仕入区分コード"];
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
                return receiveDataSirDenpyo;
            }
        }

    
    
    }
}
