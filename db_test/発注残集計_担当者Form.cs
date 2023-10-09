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

namespace db_test
{
    public partial class 発注残集計_担当者Form : Form
    {
        private string receiveDataTantosya = "";

        public 発注残集計_担当者Form()
        {
            InitializeComponent();
        }

        private void 発注残集計_担当者Form_Load(object sender, EventArgs e)
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
            gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //gcMultiRow1.DataError += new EventHandler<DataErrorEventArgs>(gcMultiRow1_DataError);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
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
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注残集計_商品FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注残集計_商品FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 受注残集計_商品FunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            GcComboBoxCell comboBoxCell1 = this.gcMultiRow1.ColumnHeaders[0].Cells["検索事業所gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell1.DataSource = Utility.GetComboBoxData("SELECT 事業所コード, 事業所名 FROM T事業所マスタ");
            comboBoxCell1.ListHeaderPane.Visible = false;
            comboBoxCell1.TextSubItemIndex = 0;
            comboBoxCell1.TextSubItemIndex = 1;
            comboBoxCell1.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell1.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell1.TextFormat = "[1]";

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            //gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            this.gcMultiRow1.ViewMode = ViewMode.Default;

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["検索事業所textBoxCell"].Value = "*";

            if (createData() != 0)
            {
                MessageBox.Show("データがありません。", "受注残集計");
            }

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "検索事業所textBoxCell");

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "終了buttonCell":
                    this.Hide();
                    break;

            }
        }

        void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            TextBoxEditingControl textBox = e.Control as TextBoxEditingControl;
            if (textBox != null)
            //if (e.Control.Name != null && e.Control.Name != "")
            {
                textBox.KeyDown -= new KeyEventHandler(textBox_KeyDown);
                textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            }
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                /*
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
                */
                case Keys.Enter:
                    e.SuppressKeyPress = true;
                    foreach (Cell cell in this.gcMultiRow1.SelectedCells)
                    {
                        if (cell.RowIndex >= 0)
                        {
                            object Val = gcMultiRow1.GetValue(cell.RowIndex, 0);

                            if (Val != null)
                            {
                                発注残集計_担当者内訳Form fm = new 発注残集計_担当者内訳Form();
                                fm.Owner = this;
                                fm.ReceiveDataTantosya = Val.ToString().Trim();
                                fm.Show();
                                return;
                            }
                        }
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "検索事業所textBoxCell":
                    GcComboBoxCell comboBoxCell1 = gcMultiRow1.ColumnHeaders[0].Cells["検索事業所gcComboBoxCell"] as GcComboBoxCell;
                    /*
                    if (e.FormattedValue != null)
                    {
                        if (comboBoxCell1.Items.FindString(e.FormattedValue.ToString(), 0, 0) < 0)
                        {
                            e.Cancel = true;
                        }
                    }
                    */
                    break;

            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "検索事業所textBoxCell":
                    GcComboBoxCell comboBoxCell1 = gcMultiRow1.ColumnHeaders[0].Cells["検索事業所gcComboBoxCell"] as GcComboBoxCell;
                    if (e.FormattedValue != null)
                    {
                        if (comboBoxCell1.Items.FindString(e.FormattedValue.ToString(), 0, 0) < 0)
                        {
                            e.Cancel = true;
                        }
                    }
                    break;
            }
        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            if (e.CellName == "検索事業所gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["検索事業所gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["検索事業所textBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["検索事業所gcComboBoxCell"].Value;
                }

            }

            // Cell.Nameプロパティが"textBoxCell1"の場合 
            if (e.CellName == "検索事業所textBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["検索事業所textBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["検索事業所gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["検索事業所textBoxCell"].Value;
                }
                if (createData() != 0)
                {
                    MessageBox.Show("データがありません。", "受注残集計");
                }
            }
        }

        private int createData()
        {
            int ret = 0;

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);

            //Template template1 = new 商品問合せTemplate();
            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;
            // ストアド プロシージャ名を指定
            command.CommandText = "発注残集計_担当者";
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["検索事業所textBoxCell"].Value != null && (String)gcMultiRow1.ColumnHeaders[0].Cells["検索事業所textBoxCell"].Value != "*")
            {
                command.Parameters.AddWithValue("@事業所コード", (String)gcMultiRow1.ColumnHeaders[0].Cells["検索事業所textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@事業所コード", DBNull.Value);
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            //adapter.FillError +=new FillErrorEventHandler(adapter_FillError);

            try
            {
                adapter.Fill(dataSet);
            }
            catch (SqlException e)
            {
                if (e.Number == 547)
                {
                    throw;
                }
                if (e.Number == 241)
                {
                    return 9;
                    throw;
                }
            }


            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            dataTable.AcceptChanges();

            this.gcMultiRow1.DataSource = dataTable;

            //dataSet.Tables[0].Rows[0]["得意先コード"].ToString();

            if (dataTable.Rows == null)
            {
                return 1;
            }
            else
            {
                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("データがありません", "受注残集計");
                    gcMultiRow1.Select();
                    //                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "表示順radioGroupCell");
                    return 1;
                }
                else
                {
                    //gcMultiRow1.ColumnHeaders[0].ReadOnly = true;
                    //gcMultiRow1.ColumnHeaders[0].Selectable = false;
                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "textBoxCell2");
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "仕入担当者コード");
                }
            }
            return 0;
        }

    }

}
