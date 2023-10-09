using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using System.Data.SqlClient;
using InputManCell = GrapeCity.Win.MultiRow.InputMan;

namespace db_test
{
    public partial class 商品別受注出荷履歴照会Form : Form
    {

        private string receiveDataSyukaRireki = "";

        public string ReceiveDataSyukaRireki
        {
            set
            {
                receiveDataSyukaRireki = value;
                //this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value = receiveDataSirsaki.ToString();
                int ret = createData();
                SendKeys.Send("{ENTER}");
            }
            get
            {
                return receiveDataSyukaRireki;
            }
        }

        public 商品別受注出荷履歴照会Form()
        {
            InitializeComponent();
        }

        private void 商品別受注出荷履歴照会Form_Load(object sender, EventArgs e)
        {
            //this.gcMultiRow1.Template = new 受注残集計_商品Template();
            //this.gcMultiRow1.MultiSelect = false;
            //this.gcMultiRow1.AllowUserToAddRows = false;
            //this.gcMultiRow1.ScrollBars = ScrollBars.None;

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
            //gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            //gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            //gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //gcMultiRow1.DataError += new EventHandler<DataErrorEventArgs>(gcMultiRow1_DataError);
            //gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            // GcMultiRowがフォーカスを失ったときに
            // セルのCellStyle.SelectionBackColorとCellStyle.SelectionForeColorを表示しない場合はtrue。
            // それ以外の場合はfalse。
            //gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            // ショートカットキーの設定
            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            //gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Tab);

            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);

            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品別受注出荷履歴照会FunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            //gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            this.gcMultiRow1.ViewMode = ViewMode.Default;

            // 初期表示
            //gcMultiRow1.ColumnHeaders[0].Cells["検索事業所textBoxCell"].Value = "*";

            if (createData() != 0)
            {
                MessageBox.Show("データがありません。", "得意先別商品別受注残問合せ");
            }

            gcMultiRow1.Select();
            //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "検索事業所textBoxCell");

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "終了buttonCell")
            {
                this.Hide();
            }
        }

        private int createData()
        {

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);

            DataTable dataTable = this.商品別受注出荷履歴照会TableAdapter.GetData(receiveDataSyukaRireki);

            dataTable.AcceptChanges();

            this.gcMultiRow1.DataSource = dataTable;

            if (dataTable.Rows == null)
            {
                return 1;
            }
            else
            {
                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("データがありません", "得意先別商品受注残問合せ");
                    gcMultiRow1.Select();
                    return 1;
                }
                else
                {
                    gcMultiRow1.ColumnHeaders[0].ReadOnly = true;
                    gcMultiRow1.ColumnHeaders[0].Selectable = false;
                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "dateTimePickerCell1");
                }
            }
            return 0;
        }

        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F10:
                    target = this.ButtonF10;
                    this.Hide();
                    break;
                default:
                    break;
            }

        }
    
    }
}
