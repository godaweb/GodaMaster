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
    public partial class 得意先検索Form : Form
    {

        String tancd = null;
        String tokcd = null;
        String kana = null;
        GcComboBoxCell comboBoxCell01;

        public String mTextBox = null;

        public 得意先検索Form()
        {
            InitializeComponent();
        }

        private void 得意先Form_Load(object sender, EventArgs e)
        {
            gcMultiRow1.ScrollBars = ScrollBars.Vertical;

            // TODO: このコード行はデータを 'sPEEDDBDataSet.T得意先マスタ' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            //this.t得意先マスタTableAdapter.Fill(this.speeddbDataSet.T得意先マスタ);

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
            gcMultiRow1.KeyPress += new KeyPressEventHandler(gcMultiRow1_KeyPress);
            gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            //gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);

            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellInRow, Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellInRow, Keys.Left);

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先検索FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先検索FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先検索FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 得意先検索FunctionKeyAction(Keys.F10), Keys.F10);

            // セル選択時、常に行全体が選択されるようにする
            //gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            this.gcMultiRow1.ViewMode = ViewMode.Default;
            //this.gcMultiRow1.ViewMode = ViewMode.Display;
            //this.gcMultiRow1.ViewMode = ViewMode.Row;
            //this.gcMultiRow1.ViewMode = ViewMode.ListBox;

            // コンボボックス
            comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");

        }


        void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "検索buttonCell":
                    int ret = execProcedure();
                    if (ret == 0)
                    {
                        EditingActions.CommitRow.Execute(gcMultiRow1);

                        gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
                        gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "textBoxCell1");
                    }
                    else if (ret == 1)
                    {
                        MessageBox.Show(this,"データがありません。", "得意先検索");
                    }
                    else
                    {
                        MessageBox.Show("データ取得エラー。", "得意先検索");
                    }

                    //EditingActions.CommitRow.Execute(gcMultiRow1);

                    //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
                    //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                    //gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);

                    //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "textBoxCell1");


                    break;
            }

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "決定buttonCell")
            {
                object Val = gcMultiRow1.CurrentRow.Cells[0].Value;

                if (Val != null)
                {
                    if (Owner.Text == "受注入力Form")
                    {
                        受注入力Form fm = (受注入力Form)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataToksaki = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            //fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                        }
                    }
                    else if (Owner.Text == "得意先別商品別受注残問合せForm")
                    {
                        得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataToksaki = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            //fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                        }
                    }
                }

                this.Hide();
            }
            else if (e.CellName == "終了buttonCell")
            {
                this.Close();
                return;
            }
            else if (e.CellName == "リストbuttonCell")
            {
                /*
                if (createReportData() == 0)
                {
                    プレビューForm プレビューform = new プレビューForm();

                    プレビューform.dataSet = dataSet;
                    プレビューform.rptName = "受注明細参照リストCrystalReport";
                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "仕入先別発注残明細表");
                }
                */
            }
            else if (e.CellName == "クリアbuttonCell")
            {
                if (gcMultiRow1.Rows != null)
                {
                    gcMultiRow1.DataSource = null;
                }
            }
        }

        void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            TextBoxEditingControl textBox = e.Control as TextBoxEditingControl;
            if (textBox != null)
            //if (e.Control.Name != null)
            {
                textBox.KeyDown -= new KeyEventHandler(textBox_KeyDown);
                textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                foreach (Cell cell in this.gcMultiRow1.SelectedCells)
                {
                    if (cell.RowIndex > 0)
                    {
                        object Val = gcMultiRow1.GetValue(cell.RowIndex, 0);

                        if (Val != null)
                        {
                            if (Owner.Text == "受注入力Form")  // kokogaugoku
                            {
                                受注入力Form fm = (受注入力Form)this.Owner;
                                if (fm != null)
                                {
                                    this.Hide();
                                    //ここがあやしい
                                    fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    fm.Activate();
                                }
                            }
                            else if (Owner.Text == "得意先別商品別受注残問合せForm")
                            {
                                得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    this.Hide();
                                    fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    fm.Activate();
                                }
                            }
                            else if (Owner.Text == "仕入先別商品別発注残問合せForm")
                            {
                                仕入先別商品別発注残問合せForm fm = (仕入先別商品別発注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    this.Hide();
                                    if (mTextBox == "txt得意先S")
                                        fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    else
                                        fm.ReceiveDataToksaki_E = Val.ToString().Trim();
                                    fm.Activate();
                                }
                            }
                            else if (Owner.Text == "得意先売上明細参照Form")
                            {
                                得意先売上明細参照Form fm = (得意先売上明細参照Form)this.Owner;
                                if (fm != null)
                                {
                                    this.Hide();
                                    fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    fm.Activate();
                                }
                            }
                            else if (Owner.Text == "得意先別商品別掛率リストForm")
                            {
                                得意先別商品別掛率リストForm fm = (得意先別商品別掛率リストForm)this.Owner;
                                if (fm != null)
                                {
                                    this.Hide();
                                    fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    fm.Activate();
                                }
                            }
                            else if (Owner.Text == "バックオーダーリストForm")
                            {
                                バックオーダーリストForm fm = (バックオーダーリストForm)this.Owner;
                                if (fm != null)
                                {
                                    this.Hide();
                                    fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    fm.Activate();
                                }
                            }
                            else if (Owner.Text == "受注明細参照Form")
                            {
                                受注明細参照Form fm = (受注明細参照Form)this.Owner;
                                if (fm != null)
                                {
                                    this.Hide();
                                    if (mTextBox == "txt得意先S")
                                        fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    else
                                        fm.ReceiveDataToksaki_E = Val.ToString().Trim();
                                    fm.Activate();
                                }
                            }
                            else if (Owner.Text == "売上計上入力Form")
                            {
                                売上計上入力Form fm = (売上計上入力Form)this.Owner;
                                if (fm != null)
                                {
                                    this.Hide();
                                    fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    fm.Activate();
                                }
                            }
                        }
                    }
                }
            }
        }

        void gcMultiRow1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                // Get all selected row list.
                foreach (Row row in this.gcMultiRow1.SelectedRows)
                {
                    if (row.Index >= 0)
                    {
                        object Val = gcMultiRow1.GetValue(row.Index, 0);

                        if (Val != null)
                        {
                            //this.Hide();
                            this.Close();
                            if (Owner.Text == "受注入力Form")
                            {
                                受注入力Form fm = (受注入力Form)this.Owner;
                                if (fm != null)
                                {
                                    fm.ReceiveDataToksaki = Val.ToString();
                                    //fm.ReceiveDataSyohin = Val.ToString();
                                    fm.Activate();
                                }
                            }
                            else if (Owner.Text == "得意先別商品別受注残問合せForm")
                            {
                                得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    fm.ReceiveDataToksaki = Val.ToString();
                                    //fm.ReceiveDataSyohin = Val.ToString();
                                }
                            }
                            //gcMultiRow1.Rows.Clear();
                        }
                    }
                }

            }
        }


        void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "担当者gcComboBoxCell":
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value != null)
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "担当者コードgcTextBoxCell":
                    if (e.FormattedValue != null && e.FormattedValue.ToString() != "*")
                    {
                        if (Utility.existCombo(comboBoxCell01, e.FormattedValue.ToString()))
                        {
                            e.Cancel = false;
                        }
                        //else
                        //{
                        //    MessageBox.Show("マスタに存在しません。", "仕入先別発注残明細表");
                        //    e.Cancel = true;
                        //}
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            if (e.CellName == "担当者コードgcTextBoxCell")  //担当者gcComboBoxCell
            {
                //GcComboBoxCell gcComboBoxCell = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"] as GcComboBoxCell;
                //string selectedValue = (string)gcComboBoxCell.Value;
                //int selectedIndex = -1;
                //if (!string.IsNullOrEmpty(selectedValue))
                //{
                //    for (int i = 0; i < gcComboBoxCell.Items.Count; i++)
                //    {
                //        if (gcComboBoxCell.Items[i].Text == selectedValue)
                //        {
                //            selectedIndex = i;
                //            break;
                //        }
                //    }
                //}
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value != "*")
                {
                    if (Utility.existCombo(comboBoxCell01, (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value))
                    {
                        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value;
                    }
                }
            }
        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            //SqlCommand command = null;

            //if (e.CellName == "担当者コードgcTextBoxCell")
            //{
            //    //if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value != "*")
            //    //{
            //    //    try
            //    //    {
            //    //        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value;
            //    //    }
            //    //    catch (Exception)
            //    //    {
            //    //        MessageBox.Show("Error!!");
            //    //    }
            //    //}
            //    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value != null && (string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value != "*")
            //    {
            //        this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value;
            //    }
            //}
            if (e.CellName == "担当者gcComboBoxCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value != null)
                {
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].Value = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value;
                }
            }
        }





        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F5:

                    target = this.ButtonF5;

                    EditingActions.CommitRow.Execute(gcMultiRow1);

                    gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
                    gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                    gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);

                    target = this.ButtonF5;

                    int ret = execProcedure();
                    if (ret > 0)
                    {
                        MessageBox.Show("データがありません。", "得意先検索");

                        EditingActions.CommitRow.Execute(gcMultiRow1);

                        gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellInRow, Keys.Up);

                        // gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 0);
                    }
                    else
                    {
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "textBoxCell1");
                    }
                    break;
                case Keys.F3:
                    target = this.ButtonF3;

                    if (gcMultiRow1.CurrentRow == null)
                    {
                        break;
                    }

                    object Val = gcMultiRow1.CurrentRow.Cells[0].Value;

                    if (Val != null)
                    {
                        if (Owner.Text == "受注入力Form")
                        {
                            受注入力Form fm = (受注入力Form)this.Owner;
                            if (fm != null)
                            {
                                fm.ReceiveDataToksaki = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                                //fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            }
                        }
                        else if (Owner.Text == "得意先別商品別受注残問合せForm")
                        {
                            得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                            if (fm != null)
                            {
                                fm.ReceiveDataToksaki = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                                //fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            }
                        }
                    }

                    this.Hide();
                    this.Owner.Activate();
                    break;
                case Keys.F10:
                    target = this.ButtonF10;
                    this.Hide();
                    this.Owner.Activate();
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

        private int execProcedure()
        {

            // 接続文字列。環境に合わせて修正してください
            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            // コネクションを開く
            connection.Open();

            // コマンド作成
            SqlCommand command = connection.CreateCommand();

            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;

            // ストアド プロシージャ名を指定
            command.CommandText = "得意先マスタ検索";
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].EditedFormattedValue == null)
            {
                command.Parameters.AddWithValue("@担当者コード", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@担当者コード", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードgcTextBoxCell"].EditedFormattedValue.ToString());
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードgcTextBoxCell"].EditedFormattedValue == null)
            {
                command.Parameters.AddWithValue("@得意先コード", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@得意先コード", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードgcTextBoxCell"].EditedFormattedValue.ToString());
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["得意先カナgcTextBoxCell"].EditedFormattedValue == null)
            {
                command.Parameters.AddWithValue("@フリガナ", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@フリガナ", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先カナgcTextBoxCell"].EditedFormattedValue.ToString());
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["txtTEL"].EditedFormattedValue == null)
            {
                command.Parameters.AddWithValue("@TEL", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@TEL", this.gcMultiRow1.ColumnHeaders[0].Cells["txtTEL"].EditedFormattedValue.ToString());
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["txtFAX"].EditedFormattedValue == null)
            {
                command.Parameters.AddWithValue("@FAX", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@FAX", this.gcMultiRow1.ColumnHeaders[0].Cells["txtFAX"].EditedFormattedValue.ToString());
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            try
            {
                adapter.Fill(dataSet);
            }
            catch (SqlException e)
            {
                if (e.Number == 547)
                {
                    return 9;
                    throw;
                }
                if (e.Number == 241)
                {
                    return 9;
                    throw;
                }
                if (e.Number == 242)
                {
                    return 9;
                    throw;
                }
            }
            finally
            {
                connection.Close();
            }

            // 結果表示
            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            dataTable.AcceptChanges();

            if (dataTable.Rows.Count > 0)
            {
                this.gcMultiRow1.DataSource = dataTable;

                //gcMultiRow1.ColumnHeaders[0].ReadOnly = true;
                //gcMultiRow1.ColumnHeaders[0].Selectable = false;
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "textBoxCell1");
            }
            else
            {
                return 1;
            }

            return 0;

        }




        void gcMultiRow1_DataError(object sender, DataErrorEventArgs e)
        {

            MessageBox.Show(e.CellName.ToString() + " " + e.Context);
            e.Cancel = true;

        }

        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "担当者コードgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードgcTextBoxCell");
        //            }
        //            break;
        //        case "担当者gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードgcTextBoxCell");
        //            }
        //            break;
        //        case "得意先コードgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先カナgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先カナgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先カナgcTextBoxCell");
        //            }
        //            break;
        //        case "得意先カナgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txtTEL");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txtTEL");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txtTEL");
        //            }
        //            break;
        //        case "txtTEL":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txtFAX");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先カナgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txtFAX");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先カナgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txtFAX");
        //            }
        //            break;
        //        case "txtFAX":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txtTEL");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "txtTEL");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "検索buttonCell");
        //            }
        //            break;
        //    }
        //}

    }

}