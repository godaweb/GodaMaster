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
    public partial class 売上計上_ドラスタ対応_欠品理由Form : Form
    {
        private string sendドラスタ_欠品理由_Data = null;
        private string receiveドラスタ_欠品理由_Data = null;
        private string arr = null;

        public 売上計上_ドラスタ対応_欠品理由Form()
        {
            InitializeComponent();
        }

        private void 売上計上_ドラスタ対応_欠品理由Form_Load(object sender, EventArgs e)
        {

            this.gcMultiRow1.Template = new 売上計上_ドラスタ対応_欠品理由Template();
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
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注入力FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注入力FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注入力FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注入力FunctionKeyAction(Keys.F10), Keys.F10);

            //gcMultiRow1.EditMode = EditMode.EditOnShortcutKey;
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Tab);

            // イベント
            //gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            //gcMultiRow1.EditingControlShowing+=new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "処理区分textBoxCell");
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            //gcMultiRow1.CellEditedFormattedValueChanged += new EventHandler<CellEditedFormattedValueChangedEventArgs>(gcMultiRow1_CellEditedFormattedValueChanged);
            //SendKeys.Send("{TAB}");
            gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            ///gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);

            //次のタブオーダーのコントロールにフォーカスを移動させる
            //Shiftキーが押されている時は、逆順にする
            //this.SelectNextControl(this.ActiveControl,
            //    ((keyData & Keys.Shift) != Keys.Shift), true, true, true);

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

            // コンボボックス
            GcComboBoxCell comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["欠品理由gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT 欠品理由コード, 欠品理由 FROM T欠品理由マスタ ORDER BY 欠品理由コード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";

            gcMultiRow1.Select();
            //this.gcMultiRow1.ColumnHeaders[0].Cells["社コードtextBoxCell"].Value = arr[0];
            //this.gcMultiRow1.ColumnHeaders[0].Cells["経費区分textBoxCell"].Value = arr[1];
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "大分類コードtextBoxCell");

        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            if (e.CellName == "欠品理由コードtextBoxCell")
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["欠品理由コードtextBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["欠品理由gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["欠品理由コードtextBoxCell"].Value;
                }
            }
            if (e.CellName == "欠品理由gcComboBoxCell")
            {
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["欠品理由gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["欠品理由コードtextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["欠品理由gcComboBoxCell"].Value;
                }
            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "欠品理由コードtextBoxCell":
                    break;
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "欠品理由コードtextBoxCell":
                    if (this.gcMultiRow1.ColumnHeaders[0].Cells["欠品理由コードtextBoxCell"].EditedFormattedValue == null)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        e.Cancel = false;
                    }
                    break;
            }
        }

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "欠品理由コードtextBoxCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "戻るbuttonCell");
                            break;
                    }
                    break;
                case "戻るbuttonCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            売上計上入力Form fm = (売上計上入力Form)this.Owner;
                            if (fm != null)
                            {
                                fm.Receiveドラスタ_欠品理由_Data = this.gcMultiRow1.ColumnHeaders[0].Cells["欠品理由コードtextBoxCell"].Value.ToString();
                                //fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                                this.Hide();
                            }
                            break;
                    }
                    break;
            }
        }

        public string Receiveドラスタ_欠品理由_Data
        {
            set
            {
                receiveドラスタ_欠品理由_Data = value;
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
