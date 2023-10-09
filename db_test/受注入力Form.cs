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
using db_test.WK受注BファイルDataSetTableAdapters;
using db_test.受注ファイルDataSetTableAdapters;
using db_test.SPEEDDB管理DataSetTableAdapters;
using db_test.SPEEDDBVIEWDataSetTableAdapters;

namespace db_test
{
    public partial class 受注入力Form : Form
    {
        private int forceFlg = 0;
        //商品検索連携用
        private string receiveDataSyohin = "";
        //得意先検索連携用
        private string receiveDataToksaki = "";
        //伝票番号検索連携用
        private string receiveDataDenpyo = "";

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
        private string ドラスタ_単価更新_単価更新フラグ = null;
        private string receiveドラスタ_単価更新_Data = null;
        private string sendドラスタ_単価更新_Data = null;
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
        DataTable 得意先dataTable = null;

        private WK受注BファイルDataSet myDS;  // データセット
        //private WK受注BファイルTableAdapter myTA;  // Userテーブルアダプタ
        private SqlCommandBuilder myCB;
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

        GcComboBoxCell comboBoxCell00 = null;
        GcComboBoxCell comboBoxCell01 = null;
        GcComboBoxCell comboBoxCell02 = null;
        GcComboBoxCell comboBoxCell03 = null;
        GcComboBoxCell comboBoxCell04 = null;

        private int beforeIndex = 0;
        private int currentIndex = 0;

        public 受注入力Form()
        {
            InitializeComponent();
        }

        private void 受注入力Form_Load(object sender, EventArgs e)
        {
            vw_ShohintableAdapter = new vw_ShohinTableAdapter();

            // TODO: このコード行はデータを '受注戻しファイルDataSet.T受注戻しファイル' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            //this.t受注戻しファイルTableAdapter2.Fill(this.受注戻しファイルDataSet.T受注戻しファイル);
            this.gcMultiRow1.Template = new 受注入力Template();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.ScrollBars = ScrollBars.None;
            this.gcMultiRow1.HideSelection = true;

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
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注入力FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注入力FunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注入力FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注入力FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注入力FunctionKeyAction(Keys.F10), Keys.F10);

            //gcMultiRow1.EditMode = EditMode.EditOnShortcutKey;
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Tab);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveRight, Keys.Right);

            // イベント
            gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            //gcMultiRow1.EditingControlShowing+=new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "処理区分textBoxCell");
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            gcMultiRow1.CellEditedFormattedValueChanged += new EventHandler<CellEditedFormattedValueChangedEventArgs>(gcMultiRow1_CellEditedFormattedValueChanged);
            //SendKeys.Send("{TAB}");
            gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.CellContentClick+=new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            
            //次のタブオーダーのコントロールにフォーカスを移動させる
            //Shiftキーが押されている時は、逆順にする
            //this.SelectNextControl(this.ActiveControl,
            //    ((keyData & Keys.Shift) != Keys.Shift), true, true, true);

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

            comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 売上区分コード, 売上区分名 FROM T売上区分マスタ WHERE システム区分=101 ORDER BY 売上区分コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ValueSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";
            

            comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.TextSubItemIndex = 0;
            comboBoxCell03.TextSubItemIndex = 1;
            comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell03.TextFormat = "[1]";
            /*
            comboBoxCell04 = this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell04.DataSource = Utility.GetComboBoxData("SELECT 納入先コード, 納入先名 FROM T納入先マスタ WHERE 得意先コード=@得意先コード ORDER BY 納入先コード");
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.TextSubItemIndex = 0;
            comboBoxCell04.TextSubItemIndex = 1;
            comboBoxCell04.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell04.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell04.TextFormat = "[1]";
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

            gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "処理区分textBoxCell");

            vw_TokuisakitableAdapter = new vw_TokuisakiTableAdapter();

        }

        void gcMultiRow1_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (forceFlg == 1)
            {
                //e.Cancel = false;
                return;
                
            }
            switch (e.CellName)
            {
                case ("オペレーターgcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = Utility.GetCode("Tオペレーターマスタ", "オペレーターコード", "オペレーター名", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].EditedFormattedValue.ToString());
                    }
                    break;
                case ("売上区分gcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = Utility.GetCode("T売上区分マスタ", "売上区分コード", "売上区分名", this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].EditedFormattedValue.ToString());
                    }
                    break;
                case ("担当者gcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = Utility.GetCode("T担当者マスタ", "担当者コード", "担当者名", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].EditedFormattedValue.ToString());
                    }
                    break;
                case ("納入先gcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = Utility.GetCode("T納入先マスタ", "納入先コード", "納入先名", this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].EditedFormattedValue.ToString());
                    }
                    break;

            }

        }

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (forceFlg == 1)
            {
                //e.Cancel = false;
                return;
            }
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "処理区分textBoxCell":
                    object kedata = e.KeyCode;
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
                //SNIPET: 終了などエンタキー押下
                case "終了buttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            forceFlg = 1;
                            this.Close();
                            break;
                    }
                    break;
                case "受注単価numericUpDownCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            if (ドラスタ区分 == "1" & ChangeSkipFlag == false)
                            {
                                受注入力_ドラスタ対応_明細Form fsDrasta_Meisai;
                                fsDrasta_Meisai = new 受注入力_ドラスタ対応_明細Form();
                                fsDrasta_Meisai.Owner = this;
                                fsDrasta_Meisai.ShowDialog();
                                if (fsDrasta_Meisai != null)
                                {
                                    //fsDrasta_Head.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    fsDrasta_Meisai.Sendドラスタ_明細_Data = "1,,2,21,";
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            if (forceFlg == 1)
            {
                e.Cancel = false;
                return;
            }
            switch (e.CellName)
            {
                case "処理区分textBoxCell":
                    if (e.FormattedValue == null)
                    {
                        e.Cancel = true;
                    }
                    else if ((string)e.FormattedValue == "0")
                    {
                        e.Cancel = false;
                    }
                    else if ((string)e.FormattedValue == "1")
                    {
                        e.Cancel = false;
                    }
                    else if ((string)e.FormattedValue == "2")
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "受注番号textBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Value != null)
                    {
                        string _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Value.ToString();
                        Int32 serchcode = Int32.Parse(_serchcode);

                        if (Int32.TryParse(_serchcode, out serchcode))
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
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
                case "売上区分コードtextBoxCell":
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
                /*
                case "売上区分gcComboBoxCell":
                    if (e.FormattedValue != null)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                */
                case "担当者コードtextBoxCell":
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
                case "納入先コードtextBoxCell":
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
                                MessageBox.Show("マスタに存在しません。", "受注入力");
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                        }

                    }

                    break;
                case "得意先コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
//                      vw_TokuisakitableAdapter = new vw_TokuisakiTableAdapter();
                        得意先dataTable = vw_TokuisakitableAdapter.GetDataBy得意先コード(e.FormattedValue.ToString());
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

                    break;
                case "発注有無区分textBoxCell":
                    if ((string)e.FormattedValue == "1")
                    {
                        if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                        {
                            string _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value.ToString();
                            vw_ShohintableAdapter = new vw_ShohinTableAdapter();
                            DataTable 商品dataTable = vw_ShohintableAdapter.GetDataBy商品コード(_serchcode);
                            if (商品dataTable.Rows.Count > 0)
                            {
                                this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = 商品dataTable.Rows[0]["主要仕入先"];
                                vw_SiiresakitableAdapter = new vw_SiiresakiTableAdapter();
                                DataTable 仕入先dataTable = vw_SiiresakitableAdapter.GetDataBy仕入先コード(商品dataTable.Rows[0]["主要仕入先"].ToString());
                                if (仕入先dataTable.Rows.Count > 0)
                                {
                                    this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = 仕入先dataTable.Rows[0]["仕入先名"];
                                }
                            }
                        }
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                    }
                    break;
                case "受注数numericUpDownCell":
                    if (e.FormattedValue == null) 
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        e.Cancel = false;
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
                            e.Cancel = false;
                        }
                        else
                        {
                            MessageBox.Show("マスタに存在しません。", "受注入力");
                            e.Cancel = true;
                        }
                    }
                    break;
                case "明細摘要textBoxCell":
                    if (ドラスタ区分 == "1")
                    {
                        if (e.FormattedValue == null)
                        {
                              MessageBox.Show("ＪＡＮコードが１３桁ではありません。");
                        }else
                        {
                            if (e.FormattedValue.ToString().Length !=13)
                            {
                                MessageBox.Show("ＪＡＮコードが１３桁ではありません。");
                            }else if (Common.chk_dgt(e.FormattedValue.ToString(),1) != 0)
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
                //e.Cancel = false;
                return;
            }
            switch (e.CellName)
            {
                case "処理区分textBoxCell":
                    //gcMultiRow1.EndEdit();
                    //gcMultiRow1.NotifyCurrentCellDirty(true);
                    break;
                case "受注番号textBoxCell":
                    break;
                case "オペレーターコードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString()))
                        {
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;
                            forceFlg = 0;
                        }
                    }
                    break;
                /*
                case ("オペレーターgcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value.ToString();
                        //SendKeys.Send("{LEFT}");
                    }
                    break;
                */
                case "売上区分コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell02, this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value.ToString()))
                        {
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value;
                            forceFlg = 0;

                        }
                        /*
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "0")
                        {
                            // XAMLファイル側で作成されたデータセットを取得
                            受注ファイルdataSet = ((受注ファイルDataSet)(this.受注ファイルDataSet1));
                            dataTable = 受注ファイルdataSet.T受注戻しファイル;
                            gcMultiRow1.DataSource = dataTable;
                            // コントロールの描画を再開する
                            gcMultiRow1.ResumeLayout();
                        }

                        gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;
                        gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = '1';
                        SendKeys.Send("{ENTER}");
                        */
                    }
                    //else
                    //{
                        //forceFlg = 1;
                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上区分gcComboBoxCell");
                        //SendKeys.Send("{ENTER}");
                    //}
                    break;
                case "担当者コードtextBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell03, this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value.ToString()))
                        {
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value;
                            forceFlg = 0;
                        }
                    }
                    break;
                case "納入先コードtextBoxCell":
                    //BUG: nullではなく、空文字が入っている
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value != null)
                    {
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value.ToString() == "")
                        {
                            break;
                        }
                    }
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell04, this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value.ToString()))
                        {
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value;
                            forceFlg = 0;
                        }
                    }
                    break;
                case "得意先コードtextBoxCell":
                    //this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[2].Cells["得意先コードtextBoxCell"].EditedFormattedValue;
                    break;
                case "商品コードtextBoxCell":
                    //this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].EditedFormattedValue;
                    SendKeys.Send("{ENTER}");
                    break;
                case "受注行番号textBoxCell":
                    object _serchcode = 0;
                    int serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value;
                        serchcode = Int32.Parse(_serchcode.ToString());

                        if (gcMultiRow1.Rows.Count >= serchcode)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商品コード");
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "受注数");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "定価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = (decimal)this.gcMultiRow1.GetValue(serchcode - 1, "納品掛率");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "受注単価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = (decimal)this.gcMultiRow1.GetValue(serchcode - 1, "原価掛率");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "原価単価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "受注金額");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答納期");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "明細摘要");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注有無区分");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "仕入先コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "仕名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注数");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注摘要");
                            forceFlg = 0;
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = null;
                        }

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
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = dataRow[0].ItemArray.GetValue(78);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(87);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(88);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(30);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = dataRow[0].ItemArray.GetValue(54);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value = dataRow[0].ItemArray.GetValue(77);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(58);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(59);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(130);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = dataRow[0].ItemArray.GetValue(84);
                        */
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");1
                    break;
            }
        }
        /*
        void gcMultiRow1_CellEditedFormattedValueChanged_(object sender, CellEditedFormattedValueChangedEventArgs e)
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
                //e.Cancel = false;
                return;
            }
            switch (e.CellName)
            {
                case "処理区分textBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue == null)
                    {
                        break;
                    }
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue.ToString() == "0")
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
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue.ToString() == "1")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].ReadOnly = false;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Selectable = true;
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                        }
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue.ToString() == "2")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].ReadOnly = false;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Selectable = true;
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                        }
                    }
                    break;
                case "受注番号textBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue.ToString() == "0")
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
                    else if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue.ToString() == "1")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                        }
                    }
                    else if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue.ToString() == "2")
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

                    break;
                case "受注日textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    }
                    break;
                case "オペレーターコードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターgcComboBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "受注日textBoxCell");
                    }
                    break;
                case "オペレーターgcComboBoxCell":
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
                    break;
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
                case "売上区分コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].EditedFormattedValue != null)
                    {
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].EditedFormattedValue.ToString() == "4")
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["直送labelCell"].Visible = true;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Style.BackColor = Color.Red;
                            if (comboBoxCell04.Items.Count > 0)
                            {
                                string buf = Utility.GetCode("T納入先マスタ", "納入先コード", "得意先コード", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString());
                                this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = buf;
                                this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = buf;
                            }
                            //forceFlg= 1;
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");

                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["直送labelCell"].Visible = false;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Style.BackColor = Color.White;
                            //forceFlg = 1;
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                        }
                    }
                    //if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].EditedFormattedValue != null)
                    //{
                        //if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].EditedFormattedValue == null)
                        //{
                        //    e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                        //}
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            //if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].EditedFormattedValue == null)
                            //{
                                //forceFlg = 1;
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");

                            //}
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            //if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].EditedFormattedValue == null)
                            //{
                                //forceFlg = 1;
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分gcComboBoxCell");

                            //}
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            //forceFlg = 1;
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            //if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].EditedFormattedValue == null)
                            //{
                                //forceFlg = 1;
                            //    e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分gcComboBoxCell");

                            //}
                        }
                    //}
                    //else
                    //{
                    //    e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分gcComboBoxCell");
                    //}
                    break;
                case "売上区分gcComboBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].EditedFormattedValue != null)
                    {
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].EditedFormattedValue.ToString() == "直送売上")
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["直送labelCell"].Visible = true;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Style.BackColor = Color.Red;
                            if (comboBoxCell04.Items.Count > 0)
                            {
                                string buf = Utility.GetCode("T納入先マスタ", "納入先コード", "得意先コード", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString());
                                this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = buf;
                                this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = buf;
                            }
                            //forceFlg= 1;
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");

                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["直送labelCell"].Visible = false;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Style.BackColor = Color.White;
                            //forceFlg = 1;
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                        }
                    }
                    //if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].EditedFormattedValue != null)
                    //{
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            //forceFlg = 1;
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                        }
                    //}
                    break;
                case "担当者コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "納入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者gcComboBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "納入先コードtextBoxCell");
                    }
                    break;
                case "担当者gcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "納入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "納入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "納入先コードtextBoxCell");
                    }
                    break;
                case "納入先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "納入先gcComboBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    break;
                case "納入先gcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "納入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "納入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    break;
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
                        forceFlg = 1;
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                        forceFlg = 0;
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
                case "受注数numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "納品掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "納品掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "納品掛率numericUpDownCell");
                    }
                    break;
                case "納品掛率numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        if (ChangeSkipFlag == false)
                        {
                            受注入力_ドラスタ対応_単価更新Form fsDrasta_Tanka;
                            fsDrasta_Tanka = new 受注入力_ドラスタ対応_単価更新Form();
                            fsDrasta_Tanka.Owner = this;
                            if (fsDrasta_Tanka != null)
                            {
                                //fsDrasta_Head.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                fsDrasta_Tanka.Sendドラスタ_単価更新_Data = "1";
                            }
                            fsDrasta_Tanka.ShowDialog();
                        }
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注単価numericUpDownCell");
                    }
                    break;
                case "受注単価numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注有無区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "納品掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注有無区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "納品掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        if (ドラスタ区分 == "1" & ChangeSkipFlag == false)
                        {
                            受注入力_ドラスタ対応_明細Form fsDrasta_Meisai;
                            fsDrasta_Meisai = new 受注入力_ドラスタ対応_明細Form();
                            fsDrasta_Meisai.Owner = this;
                            if (fsDrasta_Meisai != null)
                            {
                                //fsDrasta_Head.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                fsDrasta_Meisai.Sendドラスタ_明細_Data = null;
                            }
                            fsDrasta_Meisai.ShowDialog();
                        }
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注有無区分textBoxCell");
                    }
                    break;
                case "発注有無区分textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        if (gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value != null)
                        {
                            if ((int)gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value == 1)
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
                            }
                            else
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                            }
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
                        if (gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value != null)
                        {
                            if (gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value.ToString() == "1")
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
                            }
                            else
                            {
                                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                            }
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                        }
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        if (gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value != null)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "受注単価numericUpDownCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "回答納期textBoxCell");
                        }
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].EditedFormattedValue == "1")
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                        }
                    }
                    break;
                case "回答納期textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "回答コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "回答コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "回答コードtextBoxCell");
                    }
                    break;
                case "仕入先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注有無区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注有無区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
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
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "仕入先コードtextBoxCell");
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
                case "摘要textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                    }
                    break;
                case "終了buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    //else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    //{
                    //    e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                    //}
                    break;
                /*
                case "伝票確認textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnFooter, 0, "摘要textBoxCell");
                    }
                    break;
                 */
            }
        }

        private void 受注入力Form_Shown(object sender, EventArgs e)
        {
            this.Activate();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "受注番号textBoxCell");
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
        private void text_KeyDown(object sender, KeyEventArgs e)
        {
            TextBoxEditingControl editor = (TextBoxEditingControl)gcMultiRow1.EditingControl;
            Console.WriteLine(editor.SelectionStart);
        }
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
       //             gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Selected = true;
       //         }
       //         else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value == "0")
       //         {
       //             gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value = DateTime.Today;
       //             gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Selected = true;
       //         }
       //     }

       // }

        private void gcMultiRow1_CellContentClick(object sender, GrapeCity.Win.MultiRow.CellEventArgs e)
        {
            if (forceFlg == 1)
            {
                //e.Cancel = false;
                return;
            }
            switch (e.CellName)
            {
                case "オペレーターgcComboBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value.ToString();
                    }
                    break;
                case "終了buttonCell":
                    forceFlg = 1;
                    this.Hide();
                    break;
            }
        }

        //private void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        private void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        //void gcMultiRow1_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (forceFlg == 1)
            {
                //e.Cancel = false;
                return;
                
            }

            string _serchcode = "0";
            
            switch (e.CellName)
            {
                case ("オペレーターgcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = Utility.GetCode("Tオペレーターマスタ", "オペレーターコード", "オペレーター名", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].EditedFormattedValue.ToString());
                    }
                    break;
                case ("オペレーターコードtextBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString();
                        DataTable table = Common.GetData("SELECT オペレーター名 FROM Tオペレーターマスタ WHERE オペレーターコード='" + _serchcode + "'");
                        if (table.Rows.Count > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = _serchcode;
                        }
                        //DataRow[] dataRow = this.sPEEDDBDataSet.Tオペレーターマスタ.Select("オペレーターコード = " + _serchcode);
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(1);
                        //Object opname = this.queriesTableAdapter1.オペレータ名ScalarQuery(_serchcode);
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = opname ;
                        //gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Selected = true;
                        //SendKeys.Send("{ENTER}");
                    }
                    break;
                case ("売上区分gcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = Utility.GetCode("T売上区分マスタ", "売上区分コード", "売上区分名", this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].EditedFormattedValue.ToString());
                    }
                    break;
                case ("担当者gcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = Utility.GetCode("T担当者マスタ", "担当者コード", "担当者名", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].EditedFormattedValue.ToString());
                    }
                    break;
                case ("納入先gcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = Utility.GetCode("T納入先マスタ", "納入先コード", "納入先名", this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].EditedFormattedValue.ToString());
                    }
                    break;
                case ("処理区分textBoxCell"):
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value == "1" )
                    {
                        //gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Selected = true;
                        //SendKeys.Send("{ENTER}");
                        //gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].ReadOnly = true;
                        //gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Selectable = false;

                        //gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].ReadOnly = true;
                        //gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Selectable = false;

                        //gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].ReadOnly = false;
                        //gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Selectable = true;

                        string cellName = "受注番号textBoxCell";
                        int cellIndex = gcMultiRow1.Template.ColumnHeaders[0].Cells[cellName].CellIndex;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, cellIndex);

                    }
                    else
                    {

                        //gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].ReadOnly = true;
                        //gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Selectable = false;

                        //gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].ReadOnly = false;
                        //gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Selectable = true;

                        //gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].ReadOnly = false;
                        //gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Selectable = true;

                        //gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value = DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day;


                        //string cellName = "オペレーターコードtextBoxCell";
                        //int cellIndex = gcMultiRow1.Template.Row.Cells[cellName].CellIndex;

                        //gcMultiRow1.Focus();
                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, cellIndex);

                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, cellIndex);
                        //SendKeys.Send("{ENTER}");

                    }
                    break;
                case ("売上区分コードtextBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value != null)
                    {
                        /*
                        if (Utility.existCombo(comboBoxCell02, this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value.ToString()))
                        {
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value;

                        }
                        */
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "0")
                        {
                            受注ファイルdataSet = ((受注ファイルDataSet)(this.受注ファイルDataSet1));
                            dataTable = 受注ファイルdataSet.T受注戻しファイル;
                            gcMultiRow1.DataSource = dataTable;
                            gcMultiRow1.ResumeLayout();
                        }

                        gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;
                        gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = '1';
                        //SendKeys.Send("{ENTER}");

                    }
                    break;
                case ("得意先コードtextBoxCell"):
                    
                    _serchcode = "0";

                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != null)
                    {
                        _serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value;

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

                            受注入力_ドラスタ対応_ヘッダForm fsDrasta_Head;
                            fsDrasta_Head = new 受注入力_ドラスタ対応_ヘッダForm();
                            fsDrasta_Head.Owner = this;
                            if (fsDrasta_Head != null)
                            {
                                fsDrasta_Head.Sendドラスタ_ヘッダ_Data = "1,,2,21,";
                            }
                            fsDrasta_Head.ShowDialog();

                        }else{
                            取引先コード = null;
                            ドラスタ区分 = null;
                        }

                        comboBoxCell04 = this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"] as GcComboBoxCell;
                        comboBoxCell04.DataSource = Utility.GetComboBoxData("SELECT 納入先コード, 納入先名 FROM T納入先マスタ WHERE 得意先コード='" + _serchcode + "' ORDER BY 納入先コード");
                        comboBoxCell04.ListHeaderPane.Visible = false;
                        comboBoxCell04.ListHeaderPane.Visible = false;
                        comboBoxCell04.ListHeaderPane.Visible = false;
                        comboBoxCell04.TextSubItemIndex = 0;
                        comboBoxCell04.TextSubItemIndex = 1;
                        comboBoxCell04.ListColumns.ElementAt(0).AutoWidth = true;
                        comboBoxCell04.ListColumns.ElementAt(1).AutoWidth = true;
                        comboBoxCell04.TextFormat = "[1]";
                    }
                    break;
                case ("納入先コードtextBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value != null)
                    {
                        if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value != null)
                        {
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value.ToString() == "4")
                            {
                                this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Style.BackColor = Color.Red;
                            }
                            else
                            {
                                this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Style.BackColor = Color.White;
                            }
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Style.BackColor = Color.White;
                        }
                    }
                    break;
                case ("受注番号textBoxCell"):
                
                    _serchcode = null;
                    int serchcode = 0;

                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Value != null )
                    {
                        _serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Value;
                        //                    DataRow[] dataRow = this.northwindDataSet.受注商品管理.Select("受注コード = " + _serchcode);
                        //DataRow[] dataRow = this.sPEEDDBDataSet.T受注戻しファイル.Select("受注番号 = " + _serchcode);
                        serchcode = Int32.Parse(_serchcode);

                        // コントロールの描画を停止する
                        gcMultiRow1.SuspendLayout(); 

                        int id = System.Diagnostics.Process.GetCurrentProcess().Id;


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
                            this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = dataTable.Rows[0]["得意先コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value = dataTable.Rows[0]["得名"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                            //BUG: コンボボックスにマスタに存在しない値をセットしている。
                            //TODO: 登録時にマスタに存在する値をセットする。
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                            if (dataTable.Rows[0]["納入先コード"] != null)
                            {
                                this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                            }
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["納名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(23);

                            gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;

                            gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = null;

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
                
                    }
                    break;
                /*
                case ("受注行番号textBoxCell"):
                    object _serchcode = 0;
                    int serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value;
                        serchcode = Int32.Parse(_serchcode.ToString());

                        if (gcMultiRow1.Rows.Count >= serchcode)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商品コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "受注数");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "定価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = (decimal)this.gcMultiRow1.GetValue(serchcode - 1, "納品掛率")*100;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "受注単価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = (decimal)this.gcMultiRow1.GetValue(serchcode - 1, "原価掛率")*100;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "原価単価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "受注金額");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答納期");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "明細摘要");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注有無区分");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "仕入先コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "仕名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注数");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注摘要");
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = null;
                        }
                    
                        //this.gcMultiRow1.SetValue(serchcode2 - 1, 0,this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value);
                        /
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
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = dataRow[0].ItemArray.GetValue(78);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(87);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(88);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(30);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = dataRow[0].ItemArray.GetValue(54);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value = dataRow[0].ItemArray.GetValue(77);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = dataRow[0].ItemArray.GetValue(58);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(59);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = dataRow[0].ItemArray.GetValue(130);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = dataRow[0].ItemArray.GetValue(84);
                        /
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                */
                case ("商品コードtextBoxCell"):
                    _serchcode = "0";

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                    {
                        _serchcode = (string)this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value;
    //                    vw_ShohintableAdapter = new vw_ShohinTableAdapter();
                        DataTable 商品dataTable = vw_ShohintableAdapter.GetDataBy商品コード(_serchcode);
                        T会社基本tableAdapter = new T会社基本TableAdapter();
                        DataTable 会社基本dataTable = T会社基本tableAdapter.GetDataByKE(1);
                        ChangeSkipFlag = true;
                        forceFlg = 1;
                        if (商品dataTable.Rows.Count > 0)
                        {
                            if (商品dataTable.Rows[0]["諸口区分"].Equals(false))
                            {
                                this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["単価"];
                                this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["原価単価"];
                            }
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = 商品dataTable.Rows[0]["定価"];
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["代表納品掛率"] * 100;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["単価"];
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["代表原価掛率"] * 100;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["原価単価"];
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = 商品dataTable.Rows[0]["商品名"].ToString();
                            this.gcMultiRow1.ColumnHeaders[0].Cells["在庫数numericUpDownCell"].Value = 商品dataTable.Rows[0]["現在在庫数"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["発注残数numericUpDownCell"].Value = 商品dataTable.Rows[0]["発注残数"];
                            this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["現在在庫数"] - (decimal)商品dataTable.Rows[0]["受注残数"]; ;
                        }
                        if (会社基本dataTable.Rows.Count > 0)
                        {
                            if (会社基本dataTable.Rows[0]["単価管理"].Equals(true) & 会社基本dataTable.Rows[0]["得掛率管理"].Equals(true) & 商品dataTable.Rows[0]["諸口区分"].Equals(false))
                            {
                                this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = Math.Truncate((double)商品dataTable.Rows[0]["定価"] * (double)会社基本dataTable.Rows[0]["得掛率"]);
                            }
                        }
                        forceFlg = 0;
                        ChangeSkipFlag = false;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                        //gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Selected = true;
                        SendKeys.Send("{ENTER}");

                    }
                    break;
                case ("受注数numericUpDownCell"):
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value !=null)
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value =
                            (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value;
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case ("定価numericUpDownCell"):
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value / (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * 100;
                        }
                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value;
                        }
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case ("納品掛率numericUpDownCell"):
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value =
                            (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value / 100;
                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value;
                        }
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case ("受注単価numericUpDownCell"):
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value < (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Style.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Style.BackColor = Color.White;
                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value / (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * 100;
                        }
                    }
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value > 0)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value =
                                (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value;
                        }
                    }
                    /*
                    if (ドラスタ区分 == "1" & ChangeSkipFlag == false)
                    {
                        受注入力_ドラスタ対応_明細Form fsDrasta_Meisai;
                        fsDrasta_Meisai = new 受注入力_ドラスタ対応_明細Form();
                        fsDrasta_Meisai.Owner = this;
                        fsDrasta_Meisai.ShowDialog();
                        if (fsDrasta_Meisai != null)
                        {
                            //fsDrasta_Head.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                            fsDrasta_Meisai.Sendドラスタ_明細_Data = "1";
                        }
                    }
                    */
                    //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                /*
                case ("発注有無区分textBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value=="0")
                    {
                        gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Selected = true;
                        SendKeys.Send("{ENTER}");
                    }else{
                        gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Selected = true;
                        SendKeys.Send("{ENTER}");
                    }
                    break;
                */
                /*
                case ("受注数numericUpDownCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value != null)
                    {
                        if ((decimal)this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value - (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value < 0)
                        {                    
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Style.BackColor = Color.Red;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Style.BackColor = Color.White;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Style.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Style.BackColor = Color.White;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Style.BackColor = Color.White;
                    }
                    break;
                */
                case ("明細摘要textBoxCell"):
                    //gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;
                case ("仕入先コードtextBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value != null)
                    {
                        _serchcode = "0";
                        int buf = Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value.ToString());
                        _serchcode = String.Format("{0:000000}", buf);
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = _serchcode;
                    }
                    break;
                case ("確認textBoxCell"):
                    if ((string)gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].EditedFormattedValue == "0")
                    {
                        int index = Int32.Parse(gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].EditedFormattedValue.ToString());
                        if (gcMultiRow1.Rows.Count < index & gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].EditedFormattedValue != null)
                        {
                            //gcMultiRow1.Rows.Add();
                            if (dataTable == null)
                            {

                                MessageBox.Show("なんじゃこりゃ");
                            }

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
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Value != null)
                            {
                                dataRow["受注番号"] = this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Value.ToString();
                            }
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value != null)
                            {
                                dataRow["受注日"] = this.gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value.ToString();
                            }
                            dataRow["得意先コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value.ToString();
                            dataRow["得名"] = this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value.ToString();
                            dataRow["納期"] = this.gcMultiRow1.ColumnHeaders[0].Cells["受注日textBoxCell"].Value.ToString();
                            dataRow["エラーフラグ"] = 0;
                            dataRow["在庫管理区分"] = 0;
                            dataRow["チェック"] = 0;
                            dataRow["完了フラグ"] = 0;
                            dataRow["システム区分"] = 101;
                            dataRow["WS_ID"] = "04";
                            dataRow["単価更新フラグ"] = 0;
                            dataRow["返品区分"] = 0;
                            dataRow["オペレーターコード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString();
                            dataRow["担当者コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value.ToString();
                            dataRow["売上区分コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value.ToString();
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value != null)
                            {
                                dataRow["納入先コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value.ToString();
                            }
                            //T受注ファイル
                            //dataRow["在庫管理INDEX"] = 0;
                            dataRow["指示累計数"] = 0;
                            dataRow["売上累計数"] = 0;
                            //dataRow["完了INDEX"] = 0;

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
                                    dataRow["原価単価"] = shTable.Rows[0]["原価単価"];
                                    dataRow["原価金額"] = (decimal)shTable.Rows[0]["原価単価"] * Convert.ToDecimal(Utility.Nz(this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value,0));
                                    dataRow["粗利"] = (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value - (decimal)shTable.Rows[0]["原価単価"] * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value;
                                    dataRow["消費税率"] = shTable.Rows[0]["消費税率"];
                                    dataRow["新消費税率"] = shTable.Rows[0]["新消費税率"];
                                    dataRow["新消費税適用"] = shTable.Rows[0]["新消費税適用"].ToString();
                                    dataRow["品種コード"] = shTable.Rows[0]["品種コード"].ToString();
                                    dataRow["在庫数"] = shTable.Rows[0]["現在在庫数"];
                                    dataRow["受注残数"] = shTable.Rows[0]["受注残数"];
                                    dataRow["発注残数"] = shTable.Rows[0]["発注残数"];
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
                                        dataRow["表示価格"] = shTable.Rows[0]["表示価格"].ToString();
                                        dataRow["店舗売価"] = shTable.Rows[0]["店舗売価"].ToString();
                                    }
                                    else
                                    {
                                        if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                                        {
                                            dataRow["表示価格"] = Convert.ToInt32(  this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value) * 108 / 100;
                                            dataRow["店舗売価"] = Convert.ToInt32( this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value) * 108 / 100;
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
                                        dataRow["ＥＯＳ商品名"] = Utility.MidB(buf, 1, 20);
                                        buf = dataRow["規格"].ToString();
                                        buf = CSharp.Japanese.Kanaxs.Kana.ToHankaku(buf);
                                        buf = CSharp.Japanese.Kanaxs.Kana.ToHankakuKana(buf);
                                        dataRow["ＥＯＳ規格"] = Utility.MidB(buf, 1, 9);
                                        dataRow["店コード"] = Utility.Right(dataRow["得意先コード"].ToString(), 4);
                                        dataRow["更新ビット"] = 0;
                                        dataRow["社コード"] = 1;
                                        dataRow["店コードＢ"] = Utility.Right(dataRow["得意先コード"].ToString(), 4);
                                        dataRow["本部原価単価"] = this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value;
                                        dataRow["本部原価金額"] = this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value;
                                        dataRow["納入単価"] = this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value;
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

                            if (ドラスタ_単価更新_単価更新フラグ == "1")
                            {
                                dataRow["単価更新フラグ"] = true;
                            }
                            else
                            {
                                dataRow["単価更新フラグ"] = false;
                            }

                            dataTable.Rows.Add(dataRow);


                        }

                        if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                        {
                            if (index == 1)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注行番号", 1);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注行番号", index);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商品コード", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商品コード", this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注数", this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注数", DBNull.Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "定価", this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "定価", DBNull.Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "納品掛率", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "納品掛率", this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注単価", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注単価", this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "原価掛率", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "原価掛率", this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "原価単価", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "原価単価", this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注金額", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "受注金額", this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答納期", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答納期", this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答コード", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答コード", this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答名", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "回答名", this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value);
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
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注有無区分", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注有無区分", this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value);
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

                            //this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = index+1;
                            forceFlg = 1;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = int.Parse( this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].EditedFormattedValue.ToString()) + 1;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;
                            forceFlg = 0;

                            this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;

                            gcMultiRow1.Select();

                            gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;
                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;
                case ("伝票確認textBoxCell"):
                    if ((string)gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue == "9")
                    {
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }
                    else if ((string)gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue == "0")
                    {
                        gcMultiRow1.EndEdit();
                        // 編集した行のコミット処理
                        GrapeCity.Win.MultiRow.EditingActions.CommitRow.Execute(gcMultiRow1);
                        //this.wK受注BファイルTableAdapter1.Update(this.wK受注BファイルDataSet1.WK受注Bファイル);
                        //myCB = SqlCommandBuilder(myTA);

                        // XAMLファイル側で作成されたデータセットを取得
                        //myDS = ((WK受注BファイルDataSet)(this.wK受注BファイルDataSet1));
                        // Userテーブルアダプタで、データセットにUserデータを読み込む
                        //myTA = new WK受注BファイルTableAdapter();
                        //myTA.Update(myDS.WK受注Bファイル);
                        /*
                        受注ファイルdataSet = ((受注ファイルDataSet)(this.受注ファイルDataSet1));
                        T受注戻しファイルtableAdapter = new T受注戻しファイルTableAdapter();
                        T受注戻しファイルtableAdapter.Insert(
                            1  //本支店区分
                            ,11111  //処理コード
                            ,1  //入力区分
                            ,1234   //処理番号
                            ,true  //エラーフラグ
                            ,3      //受発注行数
                            ,7890   //受注番号
                            ,"9990" //相手先注文番号
                            ,"8880" //自社受付番号
                            ,null  //処理日
                            ,null  //受注日
                            ,null  //納期
                            ,1  //処理区
                            ,"450167"   //得意先コード
                            ,"とくいさききき"  //得名
                            ,"1"    //事業所コード
                            ,1      //ランク
                            ,"01"   //部課コード
                            ,"001"  //担当者コード
                            ,"999"  //代理店コード
                            ,"代理名"   //代名
                            ,"888"  //納入先コード
                            ,"納名"   //納名
                            ,"777"  //請求先コード
                            ,1  //売上切捨区分
                            ,1  //売上税区分
                            ,"tekiyooooo"   //伝票摘要
                            ,"1"    //配送区分
                            ,"8888888"  //商品コード
                            ,"syouhinnmeii" //商名
                            ,"kikaku"   //規格
                            ,"sunpo"    //寸法
                            ,"zaisitsu"     //材質
                            ,1      //分類
                            ,true   //在庫管理区分
                            ,"01"   //品種コード
                            ,"666"  //メーカーコード
                            ,10     //入数
                            ,1      //単位コード
                            ,"1"    //倉庫コード
                            ,3      //ケース数
                            ,2      //受注数
                            ,10     //指示累計数
                            ,8      //売上累計数
                            ,1400   //受注単価
                            ,4200   //受注金額
                            ,600    //原価単価
                            ,1200   //原価金額
                            ,210    //粗利
                            ,0      //外内税区分
                            ,8   //消費税率
                            ,8   //新消費税率
                            ,null  //新消費税摘要
                            ,"明細摘要"     //明細摘要
                            ,9000           //発注番号
                            ,290            //発注連番
                            , null   //発注納期
                            ,"3333"     //仕入先コード
                            ,"仕入先名"     //仕名
                            ,1          //仕入分類
                            ,"001"          //仕入事業所コード
                            ,"002"          //仕入担当者コード
                            ,1              //仕入切捨区分
                            ,1              //仕入税区分
                            ,true           //チェック
                            ,false          //完了フラグ
                            ,103            //WS_ID
                            ,"001"          //オペレーターコード
                            ,"001"          //修正オペレーターコード
                            ,1              //受注行
                            ,null   //処理月日
                            ,10             //管理年月
                            ,1              //受注行番号
                            ,1200           //定価
                            ,75           //納品掛率
                            ,55           //原価掛率
                            ,0              //発注有無区分
                            ,null   //回答納期
                            ,32             //在庫数
                            ,11             //受注残数
                            ,55             //発注残数
                            ,0              //出荷指示発行フラグ
                            ,"ちゅううううい"  //商品注意事項
                            ,"発注てきいいい"  //発注摘要
                            ,1          //売上区分コード
                            ,1           //システム区分コード
                            ,1          //回答コード
                            ,"kaitooo"  //回答名
                            ,0          //注文書発行フラグ
                            ,11         //取引先コード
                            ,"kaisyyy"  //社名
                            ,"miseeee"      //店名
                            ,"buuuu"        //部名
                            ,1          //発注区分
                            ,10         //請求月
                            ,"21"       //EOS区分
                            ,1          //帳票区分
                            ,"unit"     //発注単位
                            ,"111111"   //EOS商品コード
                            ,"eosssss"      //EOS商品名
                            ,"eoskikakk"   //EOS規格
                            ,100            //表示価格
                            ,100            //EOS棚番
                            ,1234567890123  //JANコード
                            ,"233"          //メーカー品番
                            ,"tana"         //棚番
                            ,"maker"        //メーカー名
                            ,11             //店コード
                            ,1              //部コード
                            ,"0"            //エラー区分
                            ,0              //更新ビット
                            ,"3456"         //商品番号
                            ,10             //社コード
                            ,11             //店コードB
                            ,0              //直送区分
                            ,0              //客注区分
                            ,0              //経費区分
                            ,0              //返品区分
                            ,100            //本部原価単価
                            ,200            //本部原価金額
                            ,1000           //納入単価
                            ,1500           //受注単価
                            ,"208"          //大分類コード
                            ,"tennpobikooo" //店舗備考
                            ,"siiibikooo"   //仕入先備考
                            ,true           //単価更新フラグ
                            ,11111          //登録番号
                            ,444            //ドラスタ発注番号
                            ,"999"           //客注番号
                            ,3              //発注数
                            );
                        */

                        SqlDb sqlDb = new SqlDb();
                        sqlDb.Connect();

                        Form F_Kihon;         // F_会社基本
                        DateTime S_date;          // ｼｽﾃﾑ日付
                        string strSQL;
                        long W_Syori_no;      // 処理番号

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
                    
                        string cst = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cst))
                        {
                            // フィールド名のマッピング
                            foreach (var column in dataTable.Columns)
                            {
                                if (column.ToString() != "在庫管理INDEX" || column.ToString() != "完了INDEX")
                                {
                                    bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());
                                }
                            }
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                dataTable.Rows[i]["入力区分"] = nkubun;
                                dataTable.Rows[i]["処理日"] = DateTime.Now;
                                dataTable.Rows[i]["処理番号"] = W_Syori_no;
                                dataTable.Rows[i]["受注行"] = dataTable.Rows[i]["受注行番号"];
                                dataTable.Rows[i]["原価掛率"] = (decimal)dataTable.Rows[i]["原価掛率"]/100;
                                dataTable.Rows[i]["納品掛率"] = (decimal)dataTable.Rows[i]["納品掛率"] / 100;
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
                            //strSQL = strSQL + " 売上伝票番号, 仕入伝票番号, 入金番号, システム区分, 処理日, 更新フラグ,";
                            strSQL = strSQL + " 受注番号, 仕入伝票番号, 入金番号, システム区分, 処理日, 更新フラグ,";
                            strSQL = strSQL + " オペレーターコード )";
                            strSQL = strSQL + " SELECT A.本支店区分, 3 AS 処理ＣＤ,";
                            strSQL = strSQL + "'受注入力' AS 処理名称,";
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
                                strSQL = strSQL + "'" + this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Value.ToString() + "' AS 番号,";
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
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["textBoxCell8"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オンライン納入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["第2仕入先コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["在庫数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["発注残数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = null;
                            //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].Value = null;
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");

                            connection.Close();
                        }
                    }
                    break;
            }
            
        }

        //SNIPET: 終了などファンクションキー処理
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
                        得意先検索Form fsToksaki = new 得意先検索Form();
                        //fsToksaki = new 得意先検索Form();
                        fsToksaki.Owner = this;
                        fsToksaki.mTextBox = cname;
                        fsToksaki.Show();
                    }
                    else if (cname == "商品コードtextBoxCell")
                    {
                        商品検索Form fsSyohin = new 商品検索Form();
                        //fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.mTextBox = cname;
                        fsSyohin.Show();
                    }
                    else if (cname == "受注番号textBoxCell")
                    {
                        伝票検索Form fsDenpyo;
                        fsDenpyo = new 伝票検索Form();
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
                        forceFlg = 1;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnFooter, 0, "摘要textBoxCell");
                        forceFlg = 0;
                    }
                    break;
                case Keys.F5:
                    target = this.ButtonF5;
                    cname = gcMultiRow1.CurrentCellPosition.CellName;

                    if (cname == "受注数numericUpDownCell")
                    {
                        object val = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value;
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
                    try
                    {
                        forceFlg = 1;

                        if (gcMultiRow1.Rows.Count > 0)
                        {
                            dataTable = (DataTable)gcMultiRow1.DataSource;
                            dataTable.Clear();
                        }
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["納品掛率numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注単価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["受注金額numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕入先コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["仕名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注摘要textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["textBoxCell8"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オンライン納入先コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["第2仕入先コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["在庫数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注残数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = null;

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");

                        forceFlg = 0;

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
                gcMultiRow1.EndEdit();
                //forceFlg = 1;
                this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = receiveDataSyohin;
                //forceFlg = 0;
                //gcMultiRow1.Select();
                //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                //SendKeys.Send("{RIGHT}");
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

        public string ReceiveDataDenpyo
        {
            set
            {
                receiveDataDenpyo = value;
                forceFlg = 1;
                this.gcMultiRow1.ColumnHeaders[0].Cells["受注番号textBoxCell"].Value = receiveDataDenpyo;
                forceFlg = 0;
                //                gcMultiRow1.Select();
//                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                SendKeys.Send("{ENTER}");
                int serchcode = Int32.Parse(receiveDataDenpyo);

                // コントロールの描画を停止する
                gcMultiRow1.SuspendLayout();

                int id = System.Diagnostics.Process.GetCurrentProcess().Id;


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
                    this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["担当者コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                    if (dataTable.Rows[0]["納入先コード"] != null)
                    {
                    //    this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                    }
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["納名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(23);

                    //gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;

                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "2")
                    {
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Value = 1;

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                    }

                    forceFlg = 0;

                    // コントロールの描画を再開する
                    gcMultiRow1.ResumeLayout();

                    forceFlg = 0;

                    //SendKeys.Send("{ENTER}");

                }
                else
                {
                    MessageBox.Show("データがありません");
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                }
            }
            get
            {
                return receiveDataDenpyo;
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

        public string Receiveドラスタ_単価更新_Data
        {
            set
            {
                receiveドラスタ_単価更新_Data = value;
                ドラスタ_単価更新_単価更新フラグ = value.ToString();

            }
            get
            {
                return receiveドラスタ_単価更新_Data;
            }
        }

        public string Sendドラスタ_単価更新_Data
        {
            set
            {
                sendドラスタ_単価更新_Data = value;
            }
            get
            {
                return sendドラスタ_単価更新_Data;
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

    }
}
