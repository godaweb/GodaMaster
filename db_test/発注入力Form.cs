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
using InputManCell = GrapeCity.Win.MultiRow.InputMan;
using db_test.SPEEDDB管理DataSetTableAdapters;
using db_test.SPEEDDBVIEWDataSetTableAdapters;
using db_test.発注ファイルDataSetTableAdapters;

namespace db_test
{
    public partial class 発注入力Form : Form
    {
        private int forceFlg = 0;
        int int処理区分 = 0;
        private int beforeIndex = 0;
        private int currentIndex = 0;

        private DataSet dataSet;
        DataTable dataTable = null;

        private SqlCommandBuilder myCB;
        private 発注入力DataSet 発注入力dataSet;  // データセット
        private T発注ファイルTableAdapter T発注ファイルtableAdapter;  // Userテーブルアダプタ
        private T発注戻しファイルTableAdapter T発注戻しファイルtableAdapter;  // Userテーブルアダプタ
        private T処理履歴テーブルTableAdapter T処理履歴テーブルtableAdapter;  // Userテーブルアダプタ
        private SPEEDDB管理DataSet SPEEDDB管理dataSet;
        private vw_ShohinTableAdapter vw_ShohintableAdapter;
        private vw_ShoTourokuTableAdapter vw_ShoTourokutableAdapter;
        private vw_SiiresakiTableAdapter vw_SiiresakitableAdapter;
        private vw_SiiTourokuTableAdapter vw_SiiTourokutableAdapter;
        private vw_Ryakumei_SirTableAdapter vw_Ryakumei_SirtableAdapter;
        private T会社基本TableAdapter T会社基本tableAdapter;
        private 発注ファイルDataSet 発注ファイルdataSet;  // データセット

        GcComboBoxCell comboBoxCell01 = null;
        GcComboBoxCell comboBoxCell02 = null;
        private string receiveDataSirsaki = "";
        private string receiveDataSyohin = "";
        private string receiveDataDenpyo = "";

        public 発注入力Form()
        {
            InitializeComponent();
        }

        private void 発注入力Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 発注入力Template();
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

            // イベント
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            //gcMultiRow1.CellEditedFormattedValueChanged += new EventHandler<CellEditedFormattedValueChangedEventArgs>(gcMultiRow1_CellEditedFormattedValueChanged);
            gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);

            // GcMultiRowコントロールがフォーカスを失ったとき
            // セルの選択状態を非表示にする
            gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);
            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            //gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Tab);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveRight, Keys.Right);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 発注入力FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 発注入力FunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 発注入力FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 発注入力FunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Default;

            // コンボボックス
            comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT オペレーターコード, オペレーター名 FROM Tオペレーターマスタ ORDER BY オペレーターコード");
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
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["発注日textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");
            gcMultiRow1.ColumnHeaders[0].Cells["発注番号textBoxCell"].Value = "000000";

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "処理区分textBoxCell");

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
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            if (forceFlg == 1)
            {
                return;
            }
            switch (e.CellName)
            {
                case "処理区分textBoxCell":
                    gcMultiRow1.EndEdit();
                    gcMultiRow1.NotifyCurrentCellDirty(true);
                    break;
                case "オペレーターコードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;
                        }
                    }
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
                case "仕入先コードtextBoxCell":
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].EditedFormattedValue;
                    // XAMLファイル側で作成されたデータセットを取得
                    発注入力dataSet = ((発注入力DataSet)(this.発注入力DataSet1));
                    dataTable = 発注入力dataSet.T発注戻しファイル;
                    gcMultiRow1.DataSource = dataTable;

                    // コントロールの描画を再開する
                    gcMultiRow1.ResumeLayout();

                    //gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;
                    gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = '1';
                    //SendKeys.Send("{ENTER}");

                    break;
                case "商品コードtextBoxCell":
                    //this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].EditedFormattedValue;
                    break;

                case "発注行番号textBoxCell":
                    object _serchcode = 0;
                    int serchcode = 0;

                    if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value != null)
                    {
                        _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value;
                        serchcode = Int32.Parse(_serchcode.ToString());

                        if (gcMultiRow1.Rows.Count >= serchcode)
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商品コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注数");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "定価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "原価掛率");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "原価単価");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注金額");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答納期");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答コード");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商名");
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "明細摘要");
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                        }
                    }
                    //gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                    break;

            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            if (forceFlg == 1)
            {
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
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号textBoxCell");
                        }
                    }

                    break;
                case "発注番号textBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue == "1")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                        }
                    }
                    else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue == "2")
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
                case "発注日textBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].EditedFormattedValue == "0")
                    {
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
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                    }

                    break;
                case "オペレーターコードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    break;
                case "オペレーターgcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    break;
                case "仕入先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    break;
                case "担当者コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    break;
                case "担当者gcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
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
                case "発注数numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価掛率numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価掛率numericUpDownCell");
                    }
                    break;
                case "原価掛率numericUpDownCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価単価numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注数numericUpDownCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "原価単価numericUpDownCell");
                    }
                    break;
                case "原価単価numericUpDownCell":
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
                case "明細摘要textBoxCell":
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
                case "確認textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "明細摘要textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    break;
                case "回答名textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "回答コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "回答コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    break;
                case "回答コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "回答コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "回答コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
                    }
                    break;
            }
        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "終了buttonCell")
            {
                forceFlg = 1;
                this.Hide();
            }
        }

        //void gcMultiRow1_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            if (forceFlg == 1)
            {
                return;
            }

            if (e.CellName == "担当者gcComboBoxCell")
            {
                string _serchcode = "0";

                if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].EditedFormattedValue != null)
                {

                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value;

                    _serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value;
                    //DataRow[] dataRow = this.sPEEDDBDataSet.Tオペレーターマスタ.Select("オペレーターコード = " + _serchcode);
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(1);
                    //Object opname = this.queriesTableAdapter1.オペレータ名ScalarQuery(_serchcode);
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = opname ;
                    //gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                }

            }
            if (e.CellName == "オペレーターgcComboBoxCell")
            {
                string _serchcode = "0";

                if (this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].EditedFormattedValue != null)
                {

                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value;

                    _serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value;
                    //DataRow[] dataRow = this.sPEEDDBDataSet.Tオペレーターマスタ.Select("オペレーターコード = " + _serchcode);
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(1);
                    //Object opname = this.queriesTableAdapter1.オペレータ名ScalarQuery(_serchcode);
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター名textBoxCell"].Value = opname ;
                    //gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");
                }

            }
            if (e.CellName == "仕入先コードtextBoxCell")
            {
                string _serchcode = "0";

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value != null)
                {
                    int buf = Convert.ToInt32(this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value.ToString());
                    _serchcode = String.Format("{0:000000}", buf);
                    this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = _serchcode;

                    //Object nm = this.t得意先マスタ1TableAdapter.得意先名ScalarQuery(_serchcode);
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["得名textBoxCell"].Value=nm;
                    vw_SiiresakiTableAdapter vw_SiiresakitableAdapter = new vw_SiiresakiTableAdapter();
                    DataTable 仕入先dataTable = vw_SiiresakitableAdapter.GetDataBy仕入先コード(_serchcode);
                    this.gcMultiRow1.ColumnHeaders[0].Cells["仕名textBoxCell"].Value = 仕入先dataTable.Rows[0]["仕入先名"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = 仕入先dataTable.Rows[0]["担当者コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = 仕入先dataTable.Rows[0]["担当者コード"];

                    //comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"] as GcComboBoxCell;
                    //comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ WHERE 担当者コード='" + 仕入先dataTable.Rows[0]["担当者コード"] + "' ORDER BY 担当者コード");
                    //comboBoxCell02.ListHeaderPane.Visible = false;
                    //comboBoxCell02.ListHeaderPane.Visible = false;
                    //comboBoxCell02.ListHeaderPane.Visible = false;
                    //comboBoxCell02.TextSubItemIndex = 0;
                    //comboBoxCell02.TextSubItemIndex = 1;
                    //comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
                    //comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
                    //comboBoxCell02.TextFormat = "[1]";
                    //gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Selected = true;
                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "売上区分コードtextBoxCell");
                    //SendKeys.Send("{ENTER}");
                    /*
                    // XAMLファイル側で作成されたデータセットを取得
                    発注入力dataSet = ((発注入力DataSet)(this.発注入力DataSet1));
                    dataTable = 発注入力dataSet.T発注戻しファイル;
                    gcMultiRow1.DataSource = dataTable;

                    // コントロールの描画を再開する
                    gcMultiRow1.ResumeLayout();

                    //gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;
                    gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = '1';
                    //SendKeys.Send("{ENTER}");
                    */
                }

            }

            if (e.CellName == "発注番号textBoxCell")
            {
                string _serchcode = null;
                int serchcode = 0;

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号textBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号textBoxCell"].Value != "000000")
                {
                    _serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号textBoxCell"].Value;
                    serchcode = Int32.Parse(_serchcode);
                    // コントロールの描画を停止する
                    gcMultiRow1.SuspendLayout();

                    //dataTable = 発注入力dataSet.T発注ファイル;

                    発注ファイルdataSet = ((発注ファイルDataSet)(this.発注ファイルDataSet1));
                    dataTable = 発注ファイルdataSet.T発注戻しファイル;

                    T発注戻しファイルtableAdapter = new T発注戻しファイルTableAdapter();

                    try
                    {
                        //myTA.DeleteWK受注Bファイル();
                        //T受注ファイルtableAdapter.FillBy受注番号(受注ファイルdataSet.T受注ファイル, serchcode);
                        dataTable = T発注戻しファイルtableAdapter.GetDataBy発注ファイル選択by発注番号(serchcode);

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

                        forceFlg = 1;
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発注日textBoxCell"].Value = dataTable.Rows[0]["発注日"].ToString();
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = dataTable.Rows[0]["仕入先コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["仕名textBoxCell"].Value = dataTable.Rows[0]["仕名"];
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["仕入担当者コード"];
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["仕入担当者コード"];
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                        //if (dataTable.Rows[0]["納入先コード"] != null)
                        //{
                        //    this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                        //}
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["納名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(23);

                        //gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;

                        if (gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "1")
                        {
                            gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = 1;
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                        }
                        else if (gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "2")
                        {
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                        }

                        // コントロールの描画を再開する
                        gcMultiRow1.ResumeLayout();

                        forceFlg = 0;

                        //SendKeys.Send("{ENTER}");

                    }
                }
            }
            /*
            if (e.CellName == "発注行番号textBoxCell")
            {
                object _serchcode = 0;
                int serchcode = 0;

                if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value != null)
                {
                    _serchcode = this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value;
                    serchcode = Int32.Parse(_serchcode.ToString());

                    if (gcMultiRow1.Rows.Count >= serchcode)
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商品コード");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注数");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "定価");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "原価掛率");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "原価単価");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "発注金額");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答納期");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答コード");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "回答名");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "商名");
                        this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = this.gcMultiRow1.GetValue(serchcode - 1, "明細摘要");
                    }
                    else
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                    }
                }
                //gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Selected = true;
                //SendKeys.Send("{ENTER}");

            }
            */
            
            if (e.CellName == "商品コードtextBoxCell")
            {
                string _serchcode = "0";

                if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                {
                    _serchcode = (string)this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value;
                    vw_ShohintableAdapter = new vw_ShohinTableAdapter();
                    DataTable 商品dataTable = vw_ShohintableAdapter.GetDataBy商品コード(_serchcode);
                    T会社基本tableAdapter = new T会社基本TableAdapter();
                    DataTable 会社基本dataTable = T会社基本tableAdapter.GetDataByKE(1);
                    //if (商品dataTable.Rows[0]["諸口区分"].Equals(false))
                    //{
                    //    this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["原価単価"];
                    //}
                    //if (会社基本dataTable.Rows[0]["単価管理"].Equals(true) & 会社基本dataTable.Rows[0]["得掛率管理"].Equals(true) & 商品dataTable.Rows[0]["諸口区分"].Equals(false))
                    //{
                    //    this.gcMultiRow1.ColumnHeaders[2].Cells["店舗売価numericUpDownCell"].Value = Math.Truncate((double)商品dataTable.Rows[0]["定価"] * (double)会社基本dataTable.Rows[0]["得掛率"]);
                    //}

                    this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = 商品dataTable.Rows[0]["定価"];
                    this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["代表原価掛率"] * 100;
                    this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = 商品dataTable.Rows[0]["原価単価"];
                    this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = 商品dataTable.Rows[0]["商品名"].ToString();
                    this.gcMultiRow1.ColumnHeaders[0].Cells["商品注意事項textBoxCell"].Value = 商品dataTable.Rows[0]["商品注意事項"].ToString();
                    this.gcMultiRow1.ColumnHeaders[0].Cells["M現在在庫数numericUpDownCell"].Value = 商品dataTable.Rows[0]["現在在庫数"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["M発注残数numericUpDownCell"].Value = 商品dataTable.Rows[0]["発注残数"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["出荷可能数numericUpDownCell"].Value = (decimal)商品dataTable.Rows[0]["現在在庫数"] - (decimal)商品dataTable.Rows[0]["受注残数"];
                    //gcMultiRow1.ColumnHeaders[2].Cells["受注数numericUpDownCell"].Selected = true;
                    //SendKeys.Send("{ENTER}");

                }

            }
            
            if (e.CellName == "発注数numericUpDownCell")
            {
                if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value != null &
                    this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value =
                        (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value;
                }
            }

            if (e.CellName == "定価numericUpDownCell")
            {

                if (this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                {
                    if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value > 0)
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value =
                            (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value / (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * 100;
                    }
                }
                if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value != null)
                {
                    if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value > 0)
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value =
                            (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value;
                    }
                }
                //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Selected = true;
                //SendKeys.Send("{ENTER}");
            }

            if (e.CellName == "原価掛率numericUpDownCell")
            {
                if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null & this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value =
                        (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value / 100;
                }
                if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value != null)
                {
                    if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value > 0)
                    {
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value =
                            (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value * (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value;
                    }
                }
                //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Selected = true;
                //SendKeys.Send("{ENTER}");
            }

            if (e.CellName == "原価単価numericUpDownCell")
            {
                if (this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value != null )
                {
                    if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value > 0)
                    {
                        if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                        {
                            if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value > 0)
                            {
                                this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value =
                                    (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value /
                                    (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value * 100;
                            }
                        }
                        if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value != null)
                        {
                            if ((decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value > 0)
                            {
                                this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value =
                                    (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value *
                                    (decimal)this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value;
                            }
                        }
                    }
                }
                //gcMultiRow1.ColumnHeaders[2].Cells["発注有無区分numericUpDownCell"].Selected = true;
                //SendKeys.Send("{ENTER}");
            }
            if (e.CellName == "回答コードtextBoxCell")
            {

                //事業所コード、部課コード、処理日の時刻、請求先コード
                if (this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value != null)
                {
                    DataTable kaitoTable = null;
                    string kaitocd = this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value.ToString();
                    if (kaitocd.Length > 0)
                    {
                        SqlDb sqlDb = new SqlDb();
                        sqlDb.Connect();　　//
                        string sql = null;

                        sql = "SELECT 取消理由 ";
                        sql += "FROM T取消理由マスタ ";
                        sql += "WHERE 理由コード=" + kaitocd;
                        kaitoTable = sqlDb.ExecuteSql(sql, -1);
                        if (kaitoTable.Rows.Count > 0)
                        {
                            string riyu = kaitoTable.Rows[0]["取消理由"].ToString();
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = riyu;
                        }
                        else
                        {
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                        }
                        sqlDb.Disconnect();
                    }
                }
                gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 2, "確認textBoxCell");
            }
            if (e.CellName == "確認textBoxCell")
            {
                if (gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].EditedFormattedValue == null)
                {
                    return;
                }
                else
                {
                    if (gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].EditedFormattedValue.ToString() == "0")
                    {
                        int index = Int32.Parse(gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].EditedFormattedValue.ToString());
                        if (gcMultiRow1.Rows.Count < index & gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].EditedFormattedValue != null)
                        {
                            //gcMultiRow1.Rows.Add();
                            DataRow dataRow = dataTable.NewRow();

                            dataRow["本支店区分"] = 1;
                            dataRow["処理コード"] = 8;
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

                            dataRow["事業所コード"] = "1";
                            dataRow["倉庫コード"] = "1";
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号textBoxCell"].Value != null)
                            {
                                dataRow["発注番号"] = this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号textBoxCell"].Value.ToString();
                            }
                            dataRow["発注日"] = this.gcMultiRow1.ColumnHeaders[0].Cells["発注日textBoxCell"].Value.ToString();
                            dataRow["仕入先コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value.ToString();
                            dataRow["仕名"] = this.gcMultiRow1.ColumnHeaders[0].Cells["仕名textBoxCell"].Value.ToString();
                            dataRow["仕入担当者コード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value.ToString();
                            dataRow["納期"] = this.gcMultiRow1.ColumnHeaders[0].Cells["発注日textBoxCell"].Value.ToString();
                            dataRow["エラーフラグ"] = 0;
                            dataRow["チェック"] = 0;
                            dataRow["完了フラグ"] = 0;
                            dataRow["WS_ID"] = "04";
                            dataRow["オペレーターコード"] = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value.ToString();
                            dataRow["伝票摘要"] = this.gcMultiRow1.ColumnHeaders[0].Cells["商品注意事項textBoxCell"].Value.ToString();

                            dataRow["エラーフラグ"] = 0;
                            dataRow["納期回答"] = 0;
                            dataRow["在庫管理区分"] = 0;
                            dataRow["チェック"] = 0;
                            dataRow["完了フラグ"] = 0;

                            dataTable.Rows.Add(dataRow);
                        }

                        if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value != null)
                        {
                            if (index == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注行番号", 1);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注行番号", index);
                            }

                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商品コード", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "商品コード", this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注数", this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注数", DBNull.Value);
                            }
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value != null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "定価", this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "定価", DBNull.Value);
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
                            if (this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value == null)
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注金額", DBNull.Value);
                            }
                            else
                            {
                                this.gcMultiRow1.SetValue(index - 1, "発注金額", this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value);
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
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = index + 1;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                            this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;

                            gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Selected = true;
                            SendKeys.Send("{ENTER}");
                        }

                    }
                }
            }

            if (e.CellName == "伝票確認textBoxCell")
            {
                if ((string)gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue == "9")
                {
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
                }
                else if ((string)gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue == "0")
                {
                    gcMultiRow1.EndEdit();
                    // 編集した行のコミット処理
                    GrapeCity.Win.MultiRow.EditingActions.CommitRow.Execute(gcMultiRow1);

                    //string cst = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
                    //using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cst))
                    //{
                    //    bulkCopy.BulkCopyTimeout = 600; // in seconds
                    //    bulkCopy.DestinationTableName = "T発注戻しファイル";
                    //    bulkCopy.WriteToServer(dataTable);
                    //}

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
                            dataTable.Rows[i]["発注行"] = dataTable.Rows[i]["発注行番号"];
                        }
                        bulkCopy.BulkCopyTimeout = 600; // in seconds
                        //bulkCopy.DestinationTableName = dataTable.TableName; // テーブル名をSqlBulkCopyに教える
                        bulkCopy.DestinationTableName = "T発注戻しファイル";
                        bulkCopy.WriteToServer(dataTable);

                        // ----------------------------
                        // T_処理履歴テーブルセット
                        // ----------------------------
                        strSQL = "INSERT INTO T処理履歴テーブル ( ";
                        strSQL = strSQL + " 本支店区分, 処理コード, 処理名, 入力区分, 事業所コード, 処理番号,";
                        //strSQL = strSQL + " 売上伝票番号, 仕入伝票番号, 入金番号, システム区分, 処理日, 更新フラグ,";
                        strSQL = strSQL + " 発注番号, 仕入伝票番号, 入金番号, システム区分, 処理日, 更新フラグ,";
                        strSQL = strSQL + " オペレーターコード )";
                        strSQL = strSQL + " SELECT A.本支店区分, 8 AS 処理ＣＤ,";
                        strSQL = strSQL + "'発注入力' AS 処理名称,";
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
                        }
                        else
                        {
                            strSQL = strSQL + "'" + this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号textBoxCell"].Value.ToString() + "' AS 番号,";
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
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注数numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["定価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価掛率numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["原価単価numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["発注金額numericUpDownCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答納期textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答コードtextBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["回答名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["商名textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["明細摘要textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["確認textBoxCell"].Value = null;
                        this.gcMultiRow1.ColumnHeaders[2].Cells["checkBoxCell1"].Value = null;
                        this.gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].Value = null;
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
                        //this.gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].Value = null;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");

                    }
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
                    if (cname == "仕入先コードtextBoxCell")
                    {
                        仕入先検索Form fsSirsaki = new 仕入先検索Form();
                        fsSirsaki.Owner = this;
                        fsSirsaki.Show();
                        break;
                    }
                    else if (cname == "商品コードtextBoxCell")
                    {
                        商品検索Form fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                        break;
                    }
                    else if (cname == "発注番号textBoxCell")
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
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                        forceFlg = 0;
                    }
                    break;
                case Keys.F9:
                    target = this.ButtonF9;
                    //gcMultiRow1.Rows.Clear();
                    try
                    {
                        if (gcMultiRow1.Rows.Count > 0)
                        {
                            DataTable dataTable = (DataTable)gcMultiRow1.DataSource;
                            dataTable.Clear();
                        }
                    }
                    catch (DataException e)
                    {
                        // Process exception and return.
                        Console.WriteLine("Exception of type {0} occurred.",
                            e.GetType());
                    }
                    this.Refresh();
                    this.gcMultiRow1.Select();
                    this.gcMultiRow1.EditMode = EditMode.EditOnEnter;
                    this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "処理区分textBoxCell");
                    break;
                case Keys.F10:
                    forceFlg = 1;
                    target = this.ButtonF10;
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

        private string chkDate(string buf)
        {
            string ret = null;
            string dt = null;
            string buf2 = null;
            buf2 = buf.Replace("/", "");
            if (buf2.Length == 4)
            {
                dt = DateTime.Today.ToString("yyyy") + buf2;
            }
            else if (buf2.Length == 6)
            {
                dt = DateTime.Today.ToString("yy") + buf2;
            }
            else if (buf2.Length == 8)
            {
                dt = buf2;
            }
            dt = dt.Substring(0, 4) + "/" + dt.Substring(4, 2) + "/" + dt.Substring(6, 2);
            DateTime DT;
            // DateTimeに変換できるかチェック
            if (DateTime.TryParse(dt, out DT))
            {
                ret = dt;
            }
            else
            {
                ret = buf;
            }

            return ret;

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

        public string ReceiveDataSyohin
        {
            set
            {
                receiveDataSyohin = value;
                this.gcMultiRow1.ColumnHeaders[2].Cells["商品コードtextBoxCell"].Value = receiveDataSyohin.ToString();
                gcMultiRow1.Select();
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "商品コードtextBoxCell");
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataSyohin;
            }
        }

        public string ReceiveDataDenpyo
        {
            set
            {
                receiveDataDenpyo = value;
                forceFlg = 1;
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号textBoxCell"].Value = receiveDataDenpyo;
                forceFlg = 0;
                //                gcMultiRow1.Select();
                //                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "受注番号textBoxCell");
                SendKeys.Send("{ENTER}");
                int serchcode = Int32.Parse(receiveDataDenpyo);

                // コントロールの描画を停止する
                gcMultiRow1.SuspendLayout();

                //発注ファイルdataSet = new 発注ファイルDataSet();
                //dataTable = 発注ファイルdataSet.T発注戻しファイル;
                //dataTable = 受注ファイルdataSet.T受注ファイル;

                // Userテーブルアダプタで、データセットにUserデータを読み込む
                //発注ファイルdataSet.T発注戻しファイル
                発注ファイルdataSet = ((発注ファイルDataSet)(this.発注ファイルDataSet1));
                dataTable = 発注ファイルdataSet.T発注戻しファイル;

                T発注戻しファイルtableAdapter = new T発注戻しファイルTableAdapter();
                //T受注ファイルtableAdapter = new T受注ファイルTableAdapter();

                try
                {
                    //myTA.DeleteWK受注Bファイル();
                    //T受注ファイルtableAdapter.FillBy受注番号(受注ファイルdataSet.T受注ファイル, serchcode);
                    //dataTable = T受注ファイルtableAdapter.GetDataBy受注番号(serchcode);
                    dataTable = T発注戻しファイルtableAdapter.GetDataBy発注ファイル選択by発注番号(serchcode);
                    
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

                    forceFlg = 1;
                    this.gcMultiRow1.ColumnHeaders[0].Cells["発注日textBoxCell"].Value = dataTable.Rows[0]["発注日"].ToString();
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = dataTable.Rows[0]["オペレーターコード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = dataTable.Rows[0]["仕入先コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["仕名textBoxCell"].Value = dataTable.Rows[0]["仕名"];
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分コードtextBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["売上区分gcComboBoxCell"].Value = dataTable.Rows[0]["売上区分コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = dataTable.Rows[0]["仕入担当者コード"];
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = dataTable.Rows[0]["仕入担当者コード"];
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["納入先コードtextBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                    //if (dataTable.Rows[0]["納入先コード"] != null)
                    //{
                    //    this.gcMultiRow1.ColumnHeaders[0].Cells["納入先gcComboBoxCell"].Value = dataTable.Rows[0]["納入先コード"];
                    //}
                    //this.gcMultiRow1.ColumnHeaders[0].Cells["納名textBoxCell"].Value = dataRow[0].ItemArray.GetValue(23);

                    //gcMultiRow1.ColumnHeaders[2].Cells["受注行番号textBoxCell"].Selected = true;

                    if (gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "1")
                    {
                        gcMultiRow1.ColumnHeaders[2].Cells["発注行番号textBoxCell"].Value = 1;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "発注行番号textBoxCell");
                    }
                    else if (gcMultiRow1.ColumnHeaders[0].Cells["処理区分textBoxCell"].Value.ToString() == "2")
                    {
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                    }

                    forceFlg = 0;

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
            get
            {
                return receiveDataDenpyo;
            }
        }

    }
}
