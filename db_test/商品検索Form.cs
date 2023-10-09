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

namespace db_test
{
    public partial class 商品検索Form : Form
    {
        private string sendData = "";
        public 受注入力Form formMain;
        private int skipflg = 0;
        private string info="";

        public string mTextBox = null;

        public 商品検索Form()
        {
            InitializeComponent();
        }

        private void 商品検索Form_Load(object sender, EventArgs e)
        {

            //this.gcMultiRow1.Template = new 商品検索Template();
            //this.gcMultiRow1.MultiSelect = false;
            //this.gcMultiRow1.AllowUserToAddRows = false;

            gcMultiRow1.ScrollBars = ScrollBars.Vertical;
            this.WindowState = FormWindowState.Maximized;

            // TODO: このコード行はデータを 'sPEEDDBDataSet.T得意先マスタ' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            //this.t商品マスタTableAdapter1.Fill(this.speeddbDataSet1.T商品マスタ);

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
            gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            //gcMultiRow1.RowEnter+=new EventHandler<CellEventArgs>(gcMultiRow1_RowEnter);

            // 編集モード
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
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);    // 選択
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);    // 検索
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);    // クリア
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);   // 終了

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品検索FunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品検索FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品検索FunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品検索FunctionKeyAction(Keys.F10), Keys.F10);


            // セル選択時、常に行全体が選択されるようにする
            this.gcMultiRow1.ViewMode = ViewMode.Default;

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "メーカー品番textBoxCell");

            //this.gcMultiRow1.ViewMode = ViewMode.Default;

        }

        void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
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
            if (e.KeyCode == Keys.Enter)
            {
                if (this.gcMultiRow1.Rows.Count > 0)
                {
                    SendKeys.Send("{F3}");
                    return;

                    foreach (Cell cell in this.gcMultiRow1.SelectedCells)
                    {
                        //11.30 ハンドルエラー対応 >=がNG
                        if (cell.RowIndex >= 0)
                        {
//                            object Val = gcMultiRow1.GetValue(gcMultiRow1.CurrentRow.Index, 14);
                            object Val = gcMultiRow1.GetValue(cell.RowIndex, 0);

                            if (Val != null)
                            {
                                if (Owner.Text == "受注入力Form")
                                {
                                    受注入力Form fm = (受注入力Form)this.Owner;
                                    if (fm != null)
                                    {
                                        //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                        fm.ReceiveDataSyohin = Val.ToString().Trim();
                                    }
                                }
                                else if (Owner.Text == "発注入力Form")
                                {
                                    発注入力Form fm = (発注入力Form)this.Owner;
                                    if (fm != null)
                                    {
                                        //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                        fm.ReceiveDataSyohin = Val.ToString().Trim();
                                    }
                                }
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
                                else if (Owner.Text == "受注明細参照Form")
                                {
                                    受注明細参照Form fm = (受注明細参照Form)this.Owner;
                                    if (fm != null)
                                    {
                                        if (mTextBox == "txt商品S")
                                            fm.ReceiveDataSyohin = Val.ToString().Trim();
                                        else
                                            fm.ReceiveDataSyohin_E = Val.ToString().Trim();
                                    }
                                }
                                else if (Owner.Text == "売上計上入力Form")
                                {
                                    売上計上入力Form fm = (売上計上入力Form)this.Owner;
                                    if (fm != null)
                                    {
                                        fm.ReceiveDataSyohin = Val.ToString().Trim();
                                    }
                                }
                                else if (Owner.Text == "仕入計上入力Form")
                                {
                                    仕入計上入力Form fm = (仕入計上入力Form)this.Owner;
                                    if (fm != null)
                                    {
                                        fm.ReceiveDataSyohin = Val.ToString().Trim();
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
        }


        void gcMultiRow1_EditingControlShowing_(object sender, EditingControlShowingEventArgs e)
        {
            TextBoxEditingControl textBox = e.Control as TextBoxEditingControl;
            if (textBox != null)
            //if (e.Control.Name != null && e.Control.Name != "")
            {
                textBox.KeyDown -= new KeyEventHandler(textBox_KeyDown);
                textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            }
        }

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            // Cell.Nameプロパティが"textBoxCell1"の場合 
            if (e.CellName == "棚番textBoxCell")
            {
                SendKeys.Send("{F5}");

                return;

                EditingActions.CommitRow.Execute(gcMultiRow1);

                gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
                gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);

                int ret = execProcedure();
                if (ret == 0)
                {
                    skipflg = 1;
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "txtメーカー名");
                }
                else if (ret == 1)
                {
                    MessageBox.Show("データがありません。", "商品検索");
                    gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
                    return;
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 0);
                }
                else
                {
                    MessageBox.Show("データがありません。", "商品検索");
                    gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
                    return;
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 0);
                }

                gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);

                return;

            }

            return;

        }
        private void textBox_KeyDown_(object sender, KeyEventArgs e)
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
                            if (Owner.Text == "受注入力Form")
                            {
                                受注入力Form fm = (受注入力Form)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
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
                            else if (Owner.Text == "売上計上入力Form")
                            {
                                売上計上入力Form fm = (売上計上入力Form)this.Owner;
                                if (fm != null)
                                {
                                    fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
                            else if (Owner.Text == "仕入計上入力Form")
                            {
                                仕入計上入力Form fm = (仕入計上入力Form)this.Owner;
                                if (fm != null)
                                {
                                    fm.ReceiveDataSyohin = Val.ToString().Trim();
                                }
                            }
                            //gcMultiRow1.Rows.Clear();
                            //this.Hide();
                            //this.Visible = false;
                            this.Close();

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
                    if (row.Index > 0)
                    {
                        object Val = gcMultiRow1.GetValue(row.Index,0);

                        if (Val != null)
                        {
                            if (Owner.Text == "受注入力Form")
                            {
                                受注入力Form fm = (受注入力Form)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    fm.ReceiveDataSyohin = Val.ToString();
                                }
                            }
                            else if (Owner.Text == "得意先別商品別受注残問合せForm")
                            {
                                得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    fm.ReceiveDataSyohin = Val.ToString();
                                }
                            }
                            else if (Owner.Text == "仕入先別商品別発注残問合せForm")
                            {
                                仕入先別商品別発注残問合せForm fm = (仕入先別商品別発注残問合せForm)this.Owner;
                                if (fm != null)
                                {
                                    //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                    fm.ReceiveDataSyohin = Val.ToString();
                                }
                            }
                            //gcMultiRow1.Rows.Clear();
                            //this.Hide();
                            this.Close();
                        }
                    }
                }
            }
        }

        void gcMultiRow1_RowEnter(object sender, CellEventArgs e)
        {
            if (skipflg == 0)
            {
                if (this.gcMultiRow1.RowCount > 0)
                {
                    if (e.CellName == "txtメーカー名")
                    {
                        if (skipflg == 0)
                        {
                            object Val = gcMultiRow1.GetValue(e.RowIndex, 0);

                            if (Val != null)
                            {
                                if (Owner.Text == "受注入力Form")
                                {
                                    受注入力Form fm = (受注入力Form)this.Owner;
                                    if (fm != null)
                                    {
                                        //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                        fm.ReceiveDataSyohin = Val.ToString();
                                    }
                                }
                                else if (Owner.Text == "得意先別商品別受注残問合せForm")
                                {
                                    得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                                    if (fm != null)
                                    {
                                        //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                        fm.ReceiveDataSyohin = Val.ToString();
                                    }
                                }
                                else if (Owner.Text == "仕入先別商品別発注残問合せForm")
                                {
                                    仕入先別商品別発注残問合せForm fm = (仕入先別商品別発注残問合せForm)this.Owner;
                                    if (fm != null)
                                    {
                                        //fm.ReceiveDataToksaki = gcMultiRow1.GetValue(e.RowIndex, e.CellIndex).ToString();
                                        fm.ReceiveDataSyohin = Val.ToString();
                                    }
                                }

                                this.Close();
                            }

                        }
                        else
                        {
                            skipflg = 0;
                        }
                    }
                }
            }


           
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
                        MessageBox.Show("データがありません。", "得意先検索");
                        gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
                        gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("データ取得エラー。", "得意先検索");
                        gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
                        gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
                        return;
                    }

                    EditingActions.CommitRow.Execute(gcMultiRow1);


                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "txtメーカー名");

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


        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F5:

                    EditingActions.CommitRow.Execute(gcMultiRow1);

                    gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
                    gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
                    gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);

                    target = this.ButtonF5;

                    int ret = execProcedure();
                    if (ret == 0)
                    {
                        skipflg = 1;
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "txtメーカー名");
                    }
                    else if (ret == 1)
                    {
                        MessageBox.Show("データがありません。", "商品検索");
                        this.Activate();
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);

                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 0);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
                    }
                    else
                    {
                        MessageBox.Show("データがありません。", "商品検索");
                        this.Activate();
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 0);
                        gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
                    }


                    break;
                case Keys.F3:
                    target = this.ButtonF3;
                    if (gcMultiRow1.CurrentRow == null)
                    {
                        MessageBox.Show("商品データを選択してください。");
                        break;
                    }
                    if (gcMultiRow1.CurrentRow.Cells[0].Value == null)
                    {
                        MessageBox.Show("商品データを選択してください。");
                        break;
                    }
                    if (Owner.Text == "受注入力Form")
                    {
                        受注入力Form fm = (受注入力Form)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            fm.Activate();
                            this.Close();
                        }
                    }
                    else if (Owner.Text == "発注入力Form")
                    {
                        発注入力Form fm = (発注入力Form)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            fm.Activate();
                            this.Close();
                        }
                    }
                    else if (Owner.Text == "商品問合せForm")
                    {
                        商品問合せForm fm = (商品問合せForm)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            fm.Activate();
                            this.Close();
                        }
                    }
                    else if (Owner.Text == "得意先別商品別受注残問合せForm")
                    {
                        得意先別商品別受注残問合せForm fm = (得意先別商品別受注残問合せForm)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            fm.Activate();
                            this.Close();
                        }
                    }
                    else if (Owner.Text == "仕入先別商品別発注残問合せForm")
                    {
                        仕入先別商品別発注残問合せForm fm = (仕入先別商品別発注残問合せForm)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            fm.Activate();
                            this.Close();
                        }
                    }
                    else if (Owner.Text == "受注明細参照Form")
                    {
                        受注明細参照Form fm = (受注明細参照Form)this.Owner;
                        if (fm != null)
                        {
                            if (mTextBox == "txt商品S")
                            {
                                fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                                fm.Activate();
                                this.Close();
                            }
                            else 
                            {
                                fm.ReceiveDataSyohin_E = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                                fm.Activate();
                                this.Close();
                            }
                        }
                    }
                    else if (Owner.Text == "得意先売上明細参照Form")
                    {
                        得意先売上明細参照Form fm = (得意先売上明細参照Form)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            fm.Activate();
                            this.Close();
                        }
                    }
                    else if (Owner.Text == "売上計上入力Form")
                    {
                        売上計上入力Form fm = (売上計上入力Form)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            fm.Activate();
                            this.Close();
                        }
                    }
                    else if (Owner.Text == "仕入計上入力Form")
                    {
                        仕入計上入力Form fm = (仕入計上入力Form)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            fm.Activate();
                            this.Close();
                        }
                    }
                    else if (Owner.Text == "出荷可能一覧表Form")
                    {
                        出荷可能一覧表Form fm = (出荷可能一覧表Form)this.Owner;
                        if (fm != null)
                        {
                            fm.ReceiveDataSyohin = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
                            fm.Activate();
                            this.Close();
                        }
                    }
                    this.Close();
                    break;
                case Keys.F9:
                    /*
                    target = this.ButtonF9;
                    gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
                    gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);
                    gcMultiRow1.ColumnHeaders[0].ReadOnly = false;
                    gcMultiRow1.ColumnHeaders[0].Selectable = true;
                    gcMultiRow1.Rows.Clear();
                    this.Refresh();
                    this.gcMultiRow1.Select();
                    this.gcMultiRow1.EditMode = EditMode.EditOnEnter;
                    this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "メーカー品番textBoxCell");
                    */
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

        public string SendData
        {
            set
            {
                sendData = value;
                //textBox1.Text = sendData;
            }
            get
            {
                return sendData;
            }
        }

        private int execProcedure()
        {
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
            command.CommandText = "商品マスタ検索";
            if (gcMultiRow1.ColumnHeaders[0].Cells["メーカー品番textBoxCell"].EditedFormattedValue != null)
            {
                command.Parameters.AddWithValue("@メーカー品番", (String)gcMultiRow1.ColumnHeaders[0].Cells["メーカー品番textBoxCell"].EditedFormattedValue);
            }
            else
            {
                command.Parameters.AddWithValue("@メーカー品番", DBNull.Value);
            }

            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["メーカーコードtextBoxCell"].EditedFormattedValue != null)
            {
                command.Parameters.AddWithValue("@メーカーコード", (String)gcMultiRow1.ColumnHeaders[0].Cells["メーカーコードtextBoxCell"].EditedFormattedValue);
            }
            else
            {
                command.Parameters.AddWithValue("@メーカーコード", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["品種コードtextBoxCell"].EditedFormattedValue != null)
            {
                command.Parameters.AddWithValue("@品種コード", (String)gcMultiRow1.ColumnHeaders[0].Cells["品種コードtextBoxCell"].EditedFormattedValue);
            }
            else
            {
                command.Parameters.AddWithValue("@品種コード", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["連番textBoxCell"].EditedFormattedValue != null)
            {
                command.Parameters.AddWithValue("@連番", (String)gcMultiRow1.ColumnHeaders[0].Cells["連番textBoxCell"].EditedFormattedValue);
            }
            else
            {
                command.Parameters.AddWithValue("@連番", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["車種textBoxCell"].EditedFormattedValue != null)
            {
                command.Parameters.AddWithValue("@車種", (String)gcMultiRow1.ColumnHeaders[0].Cells["車種textBoxCell"].EditedFormattedValue);
            }
            else
            {
                command.Parameters.AddWithValue("@車種", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["品名textBoxCell"].EditedFormattedValue != null)
            {
                command.Parameters.AddWithValue("@品名", (String)gcMultiRow1.ColumnHeaders[0].Cells["品名textBoxCell"].EditedFormattedValue);
            }
            else
            {
                command.Parameters.AddWithValue("@品名", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["バーコードtextBoxCell"].EditedFormattedValue != null)
            {
                command.Parameters.AddWithValue("@バーコード", (String)gcMultiRow1.ColumnHeaders[0].Cells["バーコードtextBoxCell"].EditedFormattedValue);
            }
            else
            {
                command.Parameters.AddWithValue("@バーコード", DBNull.Value);
            }
            if ((String)gcMultiRow1.ColumnHeaders[0].Cells["棚番textBoxCell"].EditedFormattedValue != null)
            {
                command.Parameters.AddWithValue("@棚番", (String)gcMultiRow1.ColumnHeaders[0].Cells["棚番textBoxCell"].EditedFormattedValue);
            }
            else
            {
                command.Parameters.AddWithValue("@棚番", DBNull.Value);
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

            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            dataTable.AcceptChanges();

            if (dataTable.Rows.Count == 0)
            {
                return 1;
            }
            else
            {
                this.gcMultiRow1.DataSource = dataTable;

                //gcMultiRow1.EditMode = GrapeCity.Win.MultiRow.EditMode.EditOnKeystrokeOrShortcutKey;

                //var rowcnt = 0;
                //var jyutyuzan = 0;
                //var zaiko = 0;
                //foreach (DataRow dr in dataTable.Rows)
                //{
                //    //if (dataTable.Rows.Count == rowcnt - 1)
                //    //{
                //    //    Console.WriteLine("最終データ");
                //    //    gcMultiRow1.EndEdit();
                //    //    connection.Close();
                //    //    break;
                //    //}

                //    this.gcMultiRow1.Rows.Insert(rowcnt);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txt商品コード"].CellIndex, dr["商品コード"]);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txtメーカー名"].CellIndex, dr["メーカー名"]);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txt品名"].CellIndex, dr["商品名"]);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txtメーカー品番"].CellIndex, dr["メーカー品番"]);

                //    zaiko = Convert.ToInt32(dr["在庫数"]);
                //    jyutyuzan = Convert.ToInt32(dr["発注残数"]);

                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["num可能数"].CellIndex, zaiko - jyutyuzan);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["num在庫数"].CellIndex, dr["在庫数"]);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["num発注残数"].CellIndex, dr["発注残数"]);

                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txt車種"].CellIndex, dr["車種"]);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txt棚番"].CellIndex, dr["棚番"]);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["num定価"].CellIndex, dr["定価"]);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["num原価掛率"].CellIndex, dr["原価掛率"]);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txtJANCD"].CellIndex, dr["JANCD"]);
                //    this.gcMultiRow1.SetValue(rowcnt, gcMultiRow1.Template.Row.Cells["txt商品注意事項"].CellIndex, dr["商品注意事項"]);

                //    rowcnt++;
                //}

                //gcMultiRow1.EndEdit();
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "txtメーカー名");
            }


            //gcMultiRow1.ColumnHeaders[0].ReadOnly = true;
            //gcMultiRow1.ColumnHeaders[0].Selectable = false;
            
            
            //gcMultiRow1.EndEdit();
            dataTable.Dispose();
            connection.Close();

            ////選択状態のときの色
            //gcMultiRow1.DefaultCellStyle.SelectionBackColor = Color.White;
            //gcMultiRow1.DefaultCellStyle.SelectionForeColor = Color.Black;
            ////編集状態のときの色
            //gcMultiRow1.DefaultCellStyle.EditingBackColor = Color.Yellow;
            //gcMultiRow1.DefaultCellStyle.EditingForeColor = Color.Black;
            ////無効のときの色
            //gcMultiRow1.DefaultCellStyle.DisabledBackColor = Color.White;
            //gcMultiRow1.DefaultCellStyle.DisabledForeColor = Color.Black;

            return 0;

        }

        //void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        //{
        //    switch (e.CellName)
        //    {
        //        case "メーカー品番textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコードtextBoxCell");
        //            }
        //            break;
        //        case "メーカーコードtextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "品種コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー品番textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "品種コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカー品番textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "品種コードtextBoxCell");
        //            }
        //            break;
        //        case "品種コードtextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "連番textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "連番textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "メーカーコードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "連番textBoxCell");
        //            }
        //            break;
        //        case "連番textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "車種textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "品種コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "車種textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "品種コードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "車種textBoxCell");
        //            }
        //            break;
        //        case "車種textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "品名textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "連番textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveRight)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "品名textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveLeft)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "連番textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "品名textBoxCell");
        //            }
        //            break;
        //        case "品名textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "バーコードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "車種textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "バーコードtextBoxCell");
        //            }
        //            break;
        //        case "棚番textBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "メーカー品番textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "バーコードtextBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "メーカー品番textBoxCell");
        //            }
        //            break;
        //        case "バーコードtextBoxCell":
        //            if (e.MoveStatus == MoveStatus.MoveDown)
        //            {
        //                e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "棚番textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveUp)
        //            {
        //                e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "品名textBoxCell");
        //            }
        //            else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
        //            {
        //                e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "棚番textBoxCell");
        //            }
        //            break;
        //    }
        //}


    }
}
