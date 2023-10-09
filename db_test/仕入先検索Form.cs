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

namespace db_test
{
    public partial class 仕入先検索Form : Form
    {
        public 仕入先検索Form()
        {
            InitializeComponent();
        }

        private void 仕入先検索Form_Load(object sender, EventArgs e)
        {
            gcMultiRow1.ScrollBars = ScrollBars.Vertical;

            //セル色の設定
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
            gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            gcMultiRow1.KeyPress += new KeyPressEventHandler(gcMultiRow1_KeyPress);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);


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
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先検索FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先検索FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先検索FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 仕入先検索FunctionKeyAction(Keys.F10), Keys.F10);

            // セル選択時、常に行全体が選択されるようにする
            //gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            this.gcMultiRow1.ViewMode = ViewMode.Default;

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");

        }


        void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "検索buttonCell":
                    int ret = execProcedure();
                    if (ret == 0)
                    {
                    }
                    else if (ret == 1)
                    {
                        MessageBox.Show("データがありません。", "仕入先検索");
                    }
                    else
                    {
                        MessageBox.Show("データ取得エラー。", "仕入先検索");
                    }

                    EditingActions.CommitRow.Execute(gcMultiRow1);

                    gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
                    gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                    gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);


                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "textBoxCell1");

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
                    else if (Owner.Text == "発注入力Form")
                    {
                        発注入力Form fm = (発注入力Form)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSirsaki = Val.ToString();
                            //fm.ReceiveDataSyohin = Val.ToString();
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
            else if (e.CellName == "クリアbuttonCell")
            {
                if (gcMultiRow1.Rows != null)
                {
                    gcMultiRow1.DataSource = null;
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
                        MessageBox.Show("データがありません。", "仕入先検索");
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);
                        //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 0);

                        break;
                    }

                    //gcMultiRow1.RowCount = 0;
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "textBoxCell1");
                    gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

                    break;
                case Keys.F3:
                    target = this.ButtonF3;

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
                        else if (Owner.Text == "仕入先別商品別発注残問合せForm")
                        {
                            仕入先別商品別発注残問合せForm fm = (仕入先別商品別発注残問合せForm)this.Owner;
                            if (fm != null)
                            {
                                fm.ReceiveDataSirsaki = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                                //fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            }
                        }
                        else if (Owner.Text == "発注入力Form")
                        {
                            発注入力Form fm = (発注入力Form)this.Owner;
                            if (fm != null)
                            {
                                fm.ReceiveDataSirsaki = Val.ToString();
                                //fm.ReceiveDataSyohin = Val.ToString();
                            }
                        }

                    }

                    this.Hide();

                    break;

                case Keys.F10:
                    target = this.ButtonF10;
                    this.Hide();
                    this.Owner.Activate();
                    break;

                default:
                    break;
            }

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
            command.CommandText = "仕入先マスタ検索";
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].EditedFormattedValue == null)
            {
                command.Parameters.AddWithValue("@仕入先コード", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].EditedFormattedValue.ToString());
            }
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先カナtextBoxCell"].EditedFormattedValue == null)
            {
                command.Parameters.AddWithValue("@フリガナ", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@フリガナ", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先カナtextBoxCell"].EditedFormattedValue.ToString());
            }
            // TEL
            if (this.gcMultiRow1.ColumnHeaders[0].Cells["txtTEL"].EditedFormattedValue == null)
            {
                command.Parameters.AddWithValue("@TEL", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@TEL", this.gcMultiRow1.ColumnHeaders[0].Cells["txtTEL"].EditedFormattedValue.ToString());
            }
            // FAX
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

            if (dataTable.Rows.Count>0)
            {
                this.gcMultiRow1.DataSource = dataTable;

                //gcMultiRow1.ColumnHeaders[0].ReadOnly = true;
                //gcMultiRow1.ColumnHeaders[0].Selectable = false;

                //gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "textBoxCell1");
                gcMultiRow1.CurrentCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードtextBoxCell");
            }
            else
            {
                return 1;
            }
            
            return 0;
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
                            if (Owner.Text == "受注入力Form")
                            {
                                受注入力Form fm = (受注入力Form)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = Val.ToString();
                                    fm.ReceiveDataSyohin = Val.ToString();
                                }
                            }
                            else if (Owner.Text == "得意先別商品別受注残問合せForm")
                            {
                                得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = Val.ToString();
                                    fm.ReceiveDataSyohin = Val.ToString();
                                }
                            }
                            else if (Owner.Text == "仕入先別商品別発注残問合せForm")
                            {
                                仕入先別商品別発注残問合せForm fm = (仕入先別商品別発注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    fm.ReceiveDataSirsaki = Val.ToString();
                                    //fm.ReceiveDataSyohin = Val.ToString();
                                }
                            }
                            else if (Owner.Text == "発注入力Form")
                            {
                                発注入力Form fm = (発注入力Form)this.Owner;
                                if (fm != null)
                                {
                                    fm.ReceiveDataSirsaki = Val.ToString();
                                    //fm.ReceiveDataSyohin = Val.ToString();
                                }
                            }
                            //gcMultiRow1.Rows.Clear();
                            this.Hide();
                        }
                    }
                }

            }
        }


        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            if (e.CellName == "担当者コードtextBoxCell")  //担当者gcComboBoxCell
            {
                GcComboBoxCell gcComboBoxCell = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"] as GcComboBoxCell;
                string selectedValue = (string)e.FormattedValue;
                int selectedIndex = -1;
                if (!string.IsNullOrEmpty(selectedValue))
                {
                    for (int i = 0; i < gcComboBoxCell.Items.Count; i++)
                    {
                        if (gcComboBoxCell.Items[i].Text == selectedValue)
                        {
                            selectedIndex = i;
                            break;
                        }
                    }
                    if (selectedIndex < 0)
                    {
                        MessageBox.Show("担当者マスタにありません。", "仕入先検索");
                        e.Cancel = true;
                    }
                }
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            // 仕入先
            if (e.CellName == "仕入先コードtextBoxCell") 
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value != null)
                {
                
                    DataTable table = new DataTable();
                    String sql = "SELECT 仕入先名 ";
                    sql = sql + "FROM T仕入先マスタ ";
                    sql = sql + "WHERE 仕入先コード='" + (string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードtextBoxCell"].Value + "'";

                    table = Utility.GetComboBoxData(sql);

                    if (table.Rows.Count > 0)
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["仕入先名textBoxCell"].Value = (string)(DBNull.Value.Equals(table.Rows[0]["仕入先名"]) ? null : table.Rows[0]["仕入先名"]);
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["仕入先名textBoxCell"].Value = DBNull.Value;
                    }
                }
                else
                {
                    gcMultiRow1.ColumnHeaders[0].Cells["仕入先名textBoxCell"].Value = DBNull.Value;
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
                    if (cell.RowIndex >= 0)
                    {
                        object Val = gcMultiRow1.GetValue(cell.RowIndex, 0);

                        if (Val != null)
                        {
                            if (Owner.Text == "受注入力Form")
                            {
                                受注入力Form fm = (受注入力Form)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "発注入力Form")
                            {
                                発注入力Form fm = (発注入力Form)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    fm.ReceiveDataSirsaki = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "得意先別商品別受注残問合せForm")
                            {
                                得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = Val.ToString().Trim();
                                    fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "仕入先別商品別発注残問合せForm")
                            {
                                仕入先別商品別発注残問合せForm fm = (仕入先別商品別発注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    fm.ReceiveDataSirsaki = Val.ToString().Trim();
                                    //fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
                            //gcMultiRow1.Rows.Clear();
                            //this.Hide();
                            this.Visible = false;

                        }
                    }
                }
            }
        }



        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "担当者コードgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            break;
        //        case "担当者gcComboBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            break;
        //        case "仕入先コードgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先カナgcTextBoxCell");
        //            }R
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先カナgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先カナgcTextBoxCell");
        //            }
        //            break;
        //        case "仕入先カナgcTextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コードgcTextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードgcTextBoxCell");
        //            }
        //            break;
        //    }
        //}
    }
}
