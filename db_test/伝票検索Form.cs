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
using InputManCell = GrapeCity.Win.MultiRow.InputMan;

namespace db_test
{
    public partial class 伝票検索Form : Form
    {

        public String Title = null;

        private string sendData = "";

        DataTable 伝票ヘッダTable = null;
        DataTable 伝票明細Table = null;
        string parm処理コード = "3";
        int parm入力区分 = 0;
        string parm事業所コード = null;
        string parm担当者コード = null;
        string parm取引先コード = null;
        string parm日付 = null;
        string parm管理年月 = null;
        string parm伝票番号 = "1100273";
        string parmフリガナ = null;

        GcComboBoxCell comboBoxCell00 = null;
        GcComboBoxCell comboBoxCell01 = null;
        GcComboBoxCell comboBoxCell02 = null;
        GcComboBoxCell comboBoxCell03 = null;
        GcComboBoxCell comboBoxCell04 = null;

        public 伝票検索Form()
        {
            InitializeComponent();
        }

        private void 伝票検索Form_Load(object sender, EventArgs e)
        {

            this.gcMultiRow1.Template = new 伝票検索_ヘッダTemplate();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.ScrollBars = ScrollBars.None;

            this.gcMultiRow2.Template = new 伝票検索左Template();
            this.gcMultiRow2.MultiSelect = false;
            this.gcMultiRow2.AllowUserToAddRows = false;
            this.gcMultiRow2.ScrollBars = ScrollBars.Vertical;

            this.gcMultiRow3.Template = new 伝票検索右Template();
            this.gcMultiRow3.MultiSelect = false;
            this.gcMultiRow3.AllowUserToAddRows = false;
            this.gcMultiRow3.ScrollBars = ScrollBars.Vertical;

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

            //セル色の設定
            //非選択状態の色
            //gcMultiRow2.Rows[0].Cells[0].Style.BackColor = Color.Blue;
            //gcMultiRow2.Rows[0].Cells[0].Style.ForeColor = Color.White;
            //選択状態のときの色
            gcMultiRow2.DefaultCellStyle.SelectionBackColor = Color.White;
            gcMultiRow2.DefaultCellStyle.SelectionForeColor = Color.Black;
            //編集状態のときの色
            gcMultiRow2.DefaultCellStyle.EditingBackColor = Color.Yellow;
            gcMultiRow2.DefaultCellStyle.EditingForeColor = Color.Black;
            //無効のときの色
            gcMultiRow2.DefaultCellStyle.DisabledBackColor = Color.White;
            gcMultiRow2.DefaultCellStyle.DisabledForeColor = Color.Black;

            //セル色の設定
            //非選択状態の色
            //gcMultiRow3.Rows[0].Cells[0].Style.BackColor = Color.Blue;
            //gcMultiRow3.Rows[0].Cells[0].Style.ForeColor = Color.White;
            //選択状態のときの色
            gcMultiRow3.DefaultCellStyle.SelectionBackColor = Color.White;
            gcMultiRow3.DefaultCellStyle.SelectionForeColor = Color.Black;
            //編集状態のときの色
            gcMultiRow3.DefaultCellStyle.EditingBackColor = Color.Yellow;
            gcMultiRow3.DefaultCellStyle.EditingForeColor = Color.Black;
            //無効のときの色
            gcMultiRow3.DefaultCellStyle.DisabledBackColor = Color.White;
            gcMultiRow3.DefaultCellStyle.DisabledForeColor = Color.Black;

            // イベント
            //gcMultiRow1.CellLeave += new EventHandler<CellEventArgs>(gcMultiRow1_CellLeave);
            ////gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            //gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            //gcMultiRow1.RowEnter += new EventHandler<CellEventArgs>(gcMultiRow1_RowEnter);
            gcMultiRow2.RowEnter+=new EventHandler<CellEventArgs>(gcMultiRow2_RowEnter);
            ///gcMultiRow1.KeyPress += new KeyPressEventHandler(gcMultiRow1_KeyPress);
            ///gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            gcMultiRow1.CellEditedFormattedValueChanged += new EventHandler<CellEditedFormattedValueChangedEventArgs>(gcMultiRow1_CellEditedFormattedValueChanged);
            gcMultiRow2.RowEnter += new EventHandler<CellEventArgs>(gcMultiRow2_RowEnter);
            gcMultiRow2.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow2_EditingControlShowing);
            gcMultiRow2.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow2_PreviewKeyDown);
            gcMultiRow1.EditMode = EditMode.EditOnEnter;
            gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);

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

            //gcMultiRow2.EditMode = EditMode.EditOnEnter;
            gcMultiRow2.EditMode = EditMode.EditOnKeystrokeOrShortcutKey;

            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow2.ShortcutKeyManager.Register(SelectionActions.SelectRow, Keys.Enter);
            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow2.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Tab);
            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Down);
            gcMultiRow2.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);
            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow2.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Left);
            gcMultiRow2.ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);
            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow2.ShortcutKeyManager.Register(SelectionActions.MoveRight, Keys.Right);

            //gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Enter);
            //gcMultiRow2.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);    // 選択
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);    // 検索
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);    // クリア
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);   // 終了

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 伝票検索FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 伝票検索FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 伝票検索FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 伝票検索FunctionKeyAction(Keys.F10), Keys.F10);

            // コンボボックス
            comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["事業所gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT 事業所コード, 事業所名 FROM T事業所マスタ ORDER BY 事業所コード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";

            // コンボボックス
            comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";
            
            switch (Owner.Text)
            {
                case "受注入力Form":
                    // コンボボックス
                    comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"] as GcComboBoxCell;
                    comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 得意先コード, 得意先名 FROM T得意先マスタ ORDER BY 得意先コード");
                    comboBoxCell03.ListHeaderPane.Visible = false;
                    comboBoxCell03.TextSubItemIndex = 0;
                    comboBoxCell03.TextSubItemIndex = 1;
                    comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
                    comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
                    comboBoxCell03.TextFormat = "[1]";

                    gcMultiRow1.ColumnHeaders[0].Cells["取引先labelCell"].Value = "得意先";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell30"].Value = "得意先";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell36"].Value = "受注日";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell40"].Value = "受注番号";
                    gcMultiRow3.ColumnHeaders[0].Cells["columnHeaderCell30"].Value = "受注残";
                    gcMultiRow3.ColumnHeaders[0].Cells["columnHeaderCell36"].Value = "受注残金額";

                
                    break;
                case "売上計上入力Form":
                    // コンボボックス
                    comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"] as GcComboBoxCell;
                    comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 得意先コード, 得意先名 FROM T得意先マスタ ORDER BY 得意先コード");
                    comboBoxCell03.ListHeaderPane.Visible = false;
                    comboBoxCell03.TextSubItemIndex = 0;
                    comboBoxCell03.TextSubItemIndex = 1;
                    comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
                    comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
                    comboBoxCell03.TextFormat = "[1]";

                    gcMultiRow1.ColumnHeaders[0].Cells["取引先labelCell"].Value = "得意先";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell30"].Value = "得意先";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell36"].Value = "受注日";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell40"].Value = "受注番号";
                    gcMultiRow3.ColumnHeaders[0].Cells["columnHeaderCell30"].Value = "受注残";
                    gcMultiRow3.ColumnHeaders[0].Cells["columnHeaderCell36"].Value = "受注残金額";
                    break;
                case "発注入力Form":
                    // コンボボックス
                    comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"] as GcComboBoxCell;
                    comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 仕入先コード, 仕入先名 FROM T仕入先マスタ ORDER BY 仕入先コード");
                    comboBoxCell03.ListHeaderPane.Visible = false;
                    comboBoxCell03.TextSubItemIndex = 0;
                    comboBoxCell03.TextSubItemIndex = 1;
                    comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
                    comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
                    comboBoxCell03.TextFormat = "[1]";

                    gcMultiRow1.ColumnHeaders[0].Cells["取引先labelCell"].Value = "仕入先";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell30"].Value = "仕入先";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell36"].Value = "発注日";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell40"].Value = "発注番号";
                    gcMultiRow3.ColumnHeaders[0].Cells["columnHeaderCell30"].Value = "発注残";
                    gcMultiRow3.ColumnHeaders[0].Cells["columnHeaderCell36"].Value = "発注残金額";
                    
                    
                    TextBoxCell nameCell = new TextBoxCell();
                    nameCell.DataField = "Name";                    
                    break;
                case "仕入計上入力Form":
                    // コンボボックス
                    comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"] as GcComboBoxCell;
                    comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 仕入先コード, 仕入先名 FROM T仕入先マスタ ORDER BY 仕入先コード");
                    comboBoxCell03.ListHeaderPane.Visible = false;
                    comboBoxCell03.TextSubItemIndex = 0;
                    comboBoxCell03.TextSubItemIndex = 1;
                    comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
                    comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
                    comboBoxCell03.TextFormat = "[1]";

                    gcMultiRow1.ColumnHeaders[0].Cells["取引先labelCell"].Value = "仕入先";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell30"].Value = "仕入先";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell36"].Value = "発注日";
                    gcMultiRow2.ColumnHeaders[0].Cells["columnHeaderCell40"].Value = "発注番号";
                    gcMultiRow3.ColumnHeaders[0].Cells["columnHeaderCell30"].Value = "発注残";
                    gcMultiRow3.ColumnHeaders[0].Cells["columnHeaderCell36"].Value = "発注残金額";
                    
                    break;

            }

            //gcMultiRow1.ColumnHeaders[0].Cells["メーカー品番textBoxCell"].Selected = true;
            //SendKeys.Send("{ENTER}");

            //this.gcMultiRow2.ViewMode = ViewMode.Default;
            //this.gcMultiRow2.ViewMode = ViewMode.Display;
            this.gcMultiRow2.ViewMode = ViewMode.Row;
            //this.gcMultiRow2.ViewMode = ViewMode.ListBox;

            gcMultiRow1.ColumnHeaders[0].Cells["日付textBoxCell"].Value = DateTime.Today.ToString("yyyy/MM/dd");

            execProcedure_Head();

            if (gcMultiRow2.Rows.Count > 0)
            {
                gcMultiRow2.Select();
                gcMultiRow2.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, 0);
            }else
            {
                gcMultiRow1.Select();
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
            }

        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            switch (e.CellName)
            {
                case "伝票番号textBoxCell":

                    //EditingActions.CommitRow.Execute(gcMultiRow1);

                    int ret = execProcedure_Head();
                    if (ret > 0)
                    {
                        gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
                        MessageBox.Show("データがありません。", "得意先検索");
                        gcMultiRow1.Select();
                        gcMultiRow1.Focus();
                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 0);
                    }
                    else
                    {
                        gcMultiRow2.Select();
                        gcMultiRow2.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.Row, 0,0);
                        gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Enter);
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "事業所コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["事業所コードtextBoxCell"].Value != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, this.gcMultiRow1.ColumnHeaders[0].Cells["事業所コードtextBoxCell"].Value.ToString()))
                        {
                            this.gcMultiRow1.ColumnHeaders[0].Cells["事業所gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["事業所コードtextBoxCell"].Value.ToString();
                        }
                    }
                    break;
                case ("事業所gcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["事業所gcComboBoxCell"].Value != null)
                    {
                        //this.gcMultiRow1.ColumnHeaders[0].Cells["事業所コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["事業所gcComboBoxCell"].Value.ToString();
                        SendKeys.Send("{LEFT}");
                    }
                    break;
                //SNIPET: 日付チェック(2/2)
                case "日付textBoxCell":
                    if (gcMultiRow1.CurrentCell.EditedFormattedValue != null)
                    {
                        string buf = gcMultiRow1.CurrentCell.EditedFormattedValue.ToString();
                        buf = Utility.getDate(buf);
                        if (buf != "")
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["日付textBoxCell"].Value = buf;
                        }
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "事業所コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {
                        if (Utility.existCombo(comboBoxCell01, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            MessageBox.Show("マスタに存在しません。", "受注入力");
                            e.Cancel = false;
                        }
                    }
                    else 
                    {
                        e.Cancel = false;
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
                            e.Cancel = false;
                        }
                    }
                    else 
                    {
                        e.Cancel = false;
                    }
                    break;
                case "取引先コードtextBoxCell":
                    if (e.FormattedValue != null)
                    {

                        string buf = "";

                        switch (Owner.Text)
                        {
                            case "受注入力Form":
                                buf = Utility.GetCode("T得意先マスタ", "得意先コード", "得意先コード", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["取引先コードtextBoxCell"].EditedFormattedValue);
                                break;
                            case "売上計上入力Form":
                                buf = Utility.GetCode("T得意先マスタ", "得意先コード", "得意先コード", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["取引先コードtextBoxCell"].EditedFormattedValue);
                                break;
                            case "発注入力Form":
                                buf = Utility.GetCode("T仕入先マスタ", "仕入先コード", "仕入先コード", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["取引先コードtextBoxCell"].EditedFormattedValue);
                                break;
                        }
                        
                        if (buf != null)
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"].Value = buf;
                            e.Cancel = false;
                        }
                        else
                        {
                            gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"].Value = null;
                            //MessageBox.Show("マスタに存在しません。", "受注入力");
                            e.Cancel = false;
                        }
                    }
                    else 
                    {
                        e.Cancel = false;
                    }
                    break;
                //SNIPET: 日付チェック(1/2)
                case "日付textBoxCell":
                    if (gcMultiRow1.CurrentCell.EditedFormattedValue != null)
                    {
                        string buf = gcMultiRow1.CurrentCell.EditedFormattedValue.ToString();
                        buf = Utility.getDate(buf);
                        if (buf != "")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = false;
                        }
                    }
                    else
                    {
                        e.Cancel = false;
                    }

                    break;

            }
        }

        void gcMultiRow1_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            switch (e.CellName)
            {
                case ("事業所gcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["事業所gcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["事業所コードtextBoxCell"].Value = Utility.GetCode("T事業所マスタ", "事業所コード", "事業所名", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["事業所gcComboBoxCell"].EditedFormattedValue);
                    }
                    break;
                case ("担当者gcComboBoxCell"):
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].EditedFormattedValue != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = Utility.GetCode("T担当者マスタ", "担当者コード", "担当者名", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].EditedFormattedValue);
                    }
                    break;
                case ("取引先gcComboBoxCell"):
                    switch (Owner.Text)
                    {
                        case "受注入力Form":
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"].EditedFormattedValue != null)
                            {
                                this.gcMultiRow1.ColumnHeaders[0].Cells["取引先コードtextBoxCell"].Value = Utility.GetCode("T得意先マスタ", "得意先コード", "得意先名", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"].EditedFormattedValue);
                            }
                            break;
                        case "売上計上入力Form":
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"].EditedFormattedValue != null)
                            {
                                this.gcMultiRow1.ColumnHeaders[0].Cells["取引先コードtextBoxCell"].Value = Utility.GetCode("T得意先マスタ", "得意先コード", "得意先名", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"].EditedFormattedValue);
                            }
                            break;
                        case "発注入力Form":
                            if (this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"].EditedFormattedValue != null)
                            {
                                this.gcMultiRow1.ColumnHeaders[0].Cells["取引先コードtextBoxCell"].Value = Utility.GetCode("T仕入先マスタ", "仕入先コード", "仕入先名", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["取引先gcComboBoxCell"].EditedFormattedValue);
                            }
                            break;
                    }
                    break;
            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "事業所コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセルbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセルbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    break;
                case "事業所gcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    break;
                case "担当者コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    break;
                case "担当者gcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    break;
                case "取引先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付textBoxCell");
                    }
                    break;
                case "取引先gcComboBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付textBoxCell");
                    }
                    break;
                case "日付textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号textBoxCell");
                    }
                    break;
                case "伝票番号textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "日付textBoxCell");
                    }
                    //else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    //{
                    //    e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    //}
                    break;
                case "検索buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "選択buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "選択buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "取引先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "選択buttonCell");
                    }
                    break;
                case "選択buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセルbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセルbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセルbuttonCell");
                    }
                    break;
                case "キャンセルbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "選択buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "選択buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCell)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "事業所コードtextBoxCell");
                    }
                    break;
            }
        }

        void gcMultiRow2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                switch (Owner.Text)
                {
                    case "受注入力Form":
                        受注入力Form 受注入力form = (受注入力Form)this.Owner;
                        if (受注入力form != null)
                        {
                            object Val = null;
                            foreach (Row row in gcMultiRow2.Rows)
                            {
                                if (row.Selected == true)
                                {
                                    Val = gcMultiRow2.GetValue(row.Index, 6);
                                }
                            }
                            this.Hide();
                            //受注入力form.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                            受注入力form.ReceiveDataDenpyo = Val.ToString().Trim();
                        }
                        break;
                    case "売上計上入力Form":
                        売上計上入力Form 売上計上入力form = (売上計上入力Form)this.Owner;
                        if (売上計上入力form != null)
                        {
                            object Val = null;
                            foreach (Row row in gcMultiRow2.Rows)
                            {
                                if (row.Selected == true)
                                {
                                    Val = gcMultiRow2.GetValue(row.Index, 6);
                                }
                            }
                            this.Hide();
                            //売上計上入力form.ReceiveDataUriageDenpyo = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                            売上計上入力form.ReceiveDataJyutyuDenpyo = Val.ToString().Trim();
                        }
                        break;
                    case "発注入力Form":
                        発注入力Form 発注入力form = (発注入力Form)this.Owner;
                        if (発注入力form != null)
                        {
                            object Val = null;
                            foreach (Row row in gcMultiRow2.Rows)
                            {
                                if (row.Selected == true)
                                {
                                    Val = gcMultiRow2.GetValue(row.Index, 6);
                                }
                            }
                            this.Hide();
                            //発注入力form.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                            発注入力form.ReceiveDataDenpyo = Val.ToString().Trim();
                        }
                        break;
                    case "仕入計上入力Form":
                        仕入計上入力Form 仕入計上入力form = (仕入計上入力Form)this.Owner;
                        if (仕入計上入力form != null)
                        {
                            object Val = null;
                            foreach (Row row in gcMultiRow2.Rows)
                            {
                                if (row.Selected == true)
                                {
                                    Val = gcMultiRow2.GetValue(row.Index, 6);
                                }
                            }
                            this.Hide();
                            //売上計上入力form.ReceiveDataUriageDenpyo = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                            仕入計上入力form.ReceiveDataHatyuDenpyo = Val.ToString().Trim();
                        }
                        break;
                }
            }
        }

        void gcMultiRow2_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            TextBoxEditingControl textBox = e.Control as TextBoxEditingControl;
            if (textBox != null)
            {
                textBox.KeyDown -= new KeyEventHandler(textBox_KeyDown);
                textBox.KeyDown += new KeyEventHandler(textBox_KeyDown); 
            }
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            //if (e.KeyCode == Keys.Enter)
                {
                foreach (Cell cell in this.gcMultiRow2.SelectedCells)
                {

                    if (cell.RowIndex >= 0)
                    {
                        object Val = gcMultiRow2.GetValue(cell.RowIndex, 6);

                        if (Val != null)
                        {
                            if (Owner.Text == "受注入力Form")
                            {
                                受注入力Form 受注入力form = (受注入力Form)this.Owner;
                                if (受注入力form != null)
                                {
                                    //受注入力form.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    受注入力form.ReceiveDataDenpyo = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "売上計上入力Form")
                            {
                                売上計上入力Form 売上計上入力form = (売上計上入力Form)this.Owner;
                                if (売上計上入力form != null)
                                {
                                    //受注入力form.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    売上計上入力form.ReceiveDataJyutyuDenpyo = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "発注入力Form")
                            {
                                発注入力Form 発注入力form = (発注入力Form)this.Owner;
                                if (発注入力form != null)
                                {
                                    //発注入力form.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    発注入力form.ReceiveDataDenpyo = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "仕入計上入力Form")
                            {
                                仕入計上入力Form 仕入計上入力form = (仕入計上入力Form)this.Owner;
                                if (仕入計上入力form != null)
                                {
                                    //発注入力form.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    仕入計上入力form.ReceiveDataHatyuDenpyo = Val.ToString().Trim();
                                }
                            }
                            /*
                            else if (Owner.Text == "得意先別商品別受注残問合せForm")
                            {
                                得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "仕入先別商品別発注残問合せForm")
                            {
                                仕入先別商品別発注残問合せForm fm = (仕入先別商品別発注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "商品問合せForm")
                            {
                                商品問合せForm fm = (商品問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "得意先売上明細参照Form")
                            {
                                得意先売上明細参照Form fm = (得意先売上明細参照Form)this.Owner;
                                if (fm != null)
                                {
                                    fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
                            */
                            //gcMultiRow1.Rows.Clear();
                            //this.Hide();
                            this.Visible = false;

                        }
                    }
                }
            }
        }

        void textBox_KeyDown_(object sender, KeyEventArgs e)
        {
        }

        void gcMultiRow2_RowEnter(object sender, CellEventArgs e)
        {
            if (gcMultiRow2.RowCount > 0)
            {
                string num = gcMultiRow2.GetValue(e.RowIndex, "受注番号textBoxCell").ToString();
                
                int ret = execProcedure_Meisai(num);

            }
        }

        private int execProcedure_Head()
        {

            switch (Owner.Text)
            {
                case "受注入力Form":
                    parm処理コード = "3";
                    parm入力区分 = 0;
                    parm管理年月 = null;
                    parmフリガナ = null;
                    break;
                case "売上計上入力Form":
                    parm処理コード = "3";
                    parm入力区分 = 0;
                    parm管理年月 = null;
                    parmフリガナ = null;
                    break;
                case "発注入力Form":
                    parm処理コード = "8";
                    parm入力区分 = 0;
                    parm管理年月 = null;
                    parmフリガナ = null;
                    break;
                case "仕入計上入力Form":
                    parm処理コード = "8";
                    parm入力区分 = 0;
                    parm管理年月 = null;
                    parmフリガナ = null;
                    break;
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["事業所コードtextBoxCell"].EditedFormattedValue == null)
            {
                parm事業所コード = null;
            }
            else
            {
                parm事業所コード = this.gcMultiRow1.ColumnHeaders[0].Cells["事業所コードtextBoxCell"].EditedFormattedValue.ToString();
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].EditedFormattedValue == null)
            {
                parm担当者コード = null;
            }
            else
            {
                parm担当者コード = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].EditedFormattedValue.ToString();
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["取引先コードtextBoxCell"].EditedFormattedValue == null)
            {
                parm取引先コード = null;
            }
            else
            {
                parm取引先コード = this.gcMultiRow1.ColumnHeaders[0].Cells["取引先コードtextBoxCell"].EditedFormattedValue.ToString();
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["日付textBoxCell"].EditedFormattedValue == null)
            {
                parm日付 = null;
            }
            else
            {
                parm日付 = this.gcMultiRow1.ColumnHeaders[0].Cells["日付textBoxCell"].EditedFormattedValue.ToString();
            }

            if (this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号textBoxCell"].EditedFormattedValue == null)
            {
                parm伝票番号 = null;
            }
            else
            {
                parm伝票番号 = this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号textBoxCell"].EditedFormattedValue.ToString();
            }

            伝票ヘッダTable = this.伝票検索ヘッダTableAdapter.GetData伝票検索ヘッダ(
                parm処理コード, parm入力区分, parm事業所コード, parm担当者コード, parm取引先コード, parm日付, parm管理年月, parm伝票番号, parmフリガナ);
            if (伝票ヘッダTable.Rows.Count == 0)
            {
                return 1;
            }

            gcMultiRow2.SuspendLayout();
            this.gcMultiRow2.DataSource = 伝票ヘッダTable;

            //gcMultiRow1.ColumnHeaders[0].ReadOnly = true;
            //gcMultiRow1.ColumnHeaders[0].Selectable = false;
            //gcMultiRow2.Select();
            //gcMultiRow2.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, 0);

            return 0;

        }

        private int execProcedure_Meisai(string 伝票番号)
        {

            switch (Owner.Text)
            {
                case "受注入力Form":
                    parm処理コード = "3";
                    parm入力区分 = 0;
                    parm管理年月 = null;
                    parmフリガナ = null;
                    break;
                case "売上計上入力Form":
                    parm処理コード = "3";
                    parm入力区分 = 0;
                    parm管理年月 = null;
                    parmフリガナ = null;
                    break;
                case "発注入力Form":
                    parm処理コード = "8";
                    parm入力区分 = 0;
                    parm管理年月 = null;
                    parmフリガナ = null;
                    break;
                case "仕入計上入力Form":
                    parm処理コード = "8";
                    parm入力区分 = 0;
                    parm管理年月 = null;
                    parmフリガナ = null;
                    break;
            }

            gcMultiRow3.SuspendLayout();
            伝票明細Table = this.伝票検索明細TableAdapter.GetData伝票検索明細(parm処理コード, 伝票番号);
            this.gcMultiRow3.DataSource = 伝票明細Table;

            // コントロールの描画を再開する
            gcMultiRow3.ResumeLayout();

            if (gcMultiRow3.Rows.Count == 0)
            {
                return 1;
            }

            //gcMultiRow3.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, 1);

            return 0;

        }

        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F5:

                    target = this.ButtonF5;

                    //EditingActions.CommitRow.Execute(gcMultiRow1);

                    int ret = execProcedure_Head();
                    if (ret > 0)
                    {
                        MessageBox.Show("データがありません。", "得意先検索");
                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 0);
                    }
                    else
                    {
                        gcMultiRow2.Select();
                        gcMultiRow2.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.Row, 0,0);
                        gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Enter);
                    }
                    break;
                case Keys.F3:
                    target = this.ButtonF3;

                    object Val = gcMultiRow1.CurrentRow.Cells[0].Value;

                    if (Val != null)
                    {
                        switch (Owner.Text)
                        {
                            case "受注入力Form":
                                受注入力Form 受注入力form = (受注入力Form)this.Owner;
                                if (受注入力form != null)
                                {
                                    受注入力form.ReceiveDataDenpyo = gcMultiRow2.CurrentRow.Cells[0].Value.ToString();
                                    //受注入力form.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                                
                                }
                                break;
                            case "発注入力Form":
                                発注入力Form 発注入力form = (発注入力Form)this.Owner;
                                if (発注入力form != null)
                                {
                                    発注入力form.ReceiveDataDenpyo = gcMultiRow2.CurrentRow.Cells[0].Value.ToString();
                                    //発注入力form.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                                
                                }
                                break;
                        }
                    }

                    this.Hide();

                    break;
                case Keys.F10:
                    target = this.ButtonF10;
                    this.Hide();
                    break;
                default:
                    break;
            }

            /*
            target.BackColor = SystemColors.ActiveCaption;
            target.ForeColor = SystemColors.ActiveCaptionText;
            target.Refresh();

            //0.2秒間待機
            System.Threading.Thread.Sleep(200);

            target.BackColor = SystemColors.Control;
            target.ForeColor = SystemColors.ControlText;
            */
        }

        void gcMultiRow1_DataError(object sender, DataErrorEventArgs e)
        {
            // The first id cell only can input number, if user input some invalid value, DataError event will be fired.
            // You should handle this event to handle some error cases.
            if ((e.Context & DataErrorContexts.Commit) != 0)
            {
                // When committing value occurs error, show a massage box to notify user, and roll back value.
                MessageBox.Show(e.Exception.Message);
                EditingActions.CancelEdit.Execute(this.gcMultiRow1);
            }
            else
            {
                // Other handle.
            }
        }

        void gcMultiRow2_DataError(object sender, DataErrorEventArgs e)
        {
            // The first id cell only can input number, if user input some invalid value, DataError event will be fired.
            // You should handle this event to handle some error cases.
            if ((e.Context & DataErrorContexts.Commit) != 0)
            {
                // When committing value occurs error, show a massage box to notify user, and roll back value.
                MessageBox.Show(e.Exception.Message);
                EditingActions.CancelEdit.Execute(this.gcMultiRow2);
            }
            else
            {
                // Other handle.
            }
        }

        void gcMultiRow3_DataError(object sender, DataErrorEventArgs e)
        {
            // The first id cell only can input number, if user input some invalid value, DataError event will be fired.
            // You should handle this event to handle some error cases.
            if ((e.Context & DataErrorContexts.Commit) != 0)
            {
                // When committing value occurs error, show a massage box to notify user, and roll back value.
                MessageBox.Show(e.Exception.Message);
                EditingActions.CancelEdit.Execute(this.gcMultiRow3);
            }
            else
            {
                // Other handle.
            }
        }

    }
}
