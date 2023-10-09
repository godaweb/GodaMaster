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
    public partial class 商品問合せForm : Form
    {

        private string receiveDataSyohin = "";
        private string receiveDataToksaki = "";
        商品検索Form fsSyohin;

        public 商品問合せForm()
        {
            InitializeComponent();
            //fsSyohin = new 商品検索Form();
            //fsSyohin.Owner = this;
        }

        private void 商品問合せForm_Load(object sender, EventArgs e)
        {
            // TODO: このコード行はデータを 'sPEEDDBDataSet.T得意先マスタ' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            //this.t商品マスタTableAdapter1.Fill(this.speeddbDataSet1.T商品マスタ);

            this.gcMultiRow1.Template = new 商品問合せTemplate();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = true;
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

            // イベント
            //gcMultiRow1.CellLeave += new EventHandler<CellEventArgs>(gcMultiRow1_CellLeave);
            //gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            //gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            gcMultiRow1.CellContentButtonClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentButtonClick);
            //gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);

            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);

            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Tab);

            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);

            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品問合せFunctionKeyAction(Keys.F3), Keys.F3);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品問合せFunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品問合せFunctionKeyAction(Keys.F9), Keys.F9);
            this.gcMultiRow1.ShortcutKeyManager.Register(new 商品問合せFunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            //gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Default;

            //DataRow[] dataRow = this.speeddbDataSet1.T商品マスタ.Select();
            //this.gcMultiRow1.DataSource = dataRow;

            //gcMultiRow1.ColumnHeaders[0].Cells["メーカー品番textBoxCell"].Selected = true;
            //SendKeys.Send("{ENTER}");
        
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            // Cell.Nameプロパティが"textBoxCell1"の場合 
            if (e.CellName == "品番textBoxCell")
            {
                //string _serchcode = "0";
                //if (gcMultiRow1.GetValue(e.RowIndex, e.CellIndex) != null)
                if (e.FormattedValue != null)
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
                    command.CommandText = "商品問合せ";
                    //command.Parameters.AddWithValue("@商品コード", gcMultiRow1.GetValue(e.RowIndex, e.CellIndex));
                    command.Parameters.AddWithValue("@商品コード", e.FormattedValue);

                    // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataSet);

                    //DataTableオブジェクトを用意
                    DataTable dataTable = dataSet.Tables[0];

                    dataTable.AcceptChanges();

                    //this.gcMultiRow1.DataSource = dataTable;

                    if (dataTable.Rows.Count > 0)
                    {
                        dataSet.Tables[0].Rows[0]["商品名"].ToString();

                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["メーカーtextBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["メーカーコード"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["メーカー品番textBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["メーカー品番"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["車種textBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["車種"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["現在庫numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["現在在庫数"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["発注残数numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["発注残数"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["JANコードtextBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["バーコード"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["掛率numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["掛率"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["商品名textBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["商品名"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["可能数numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["可能数"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["原価numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["原価"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["定価numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["定価"]);
                        this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["棚番textBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["棚番"]);
                    }
                    

                    //this.gcMultiRow1.DataSource = dataTable;

                    // 交互行のスタイルを設定
                    //this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(239, 255, 250);
                    // ユーザーによるリサイズを許可しない
                    //this.gcMultiRow1.AllowUserToResize = false;

                }

            }
        }

        void gcMultiRow1_CellContentButtonClick(object sender, CellEventArgs e)
        {

            if (e.CellName == "クリアbuttonCell")
            {
                this.gcMultiRow1.Rows.Clear();

            }

            if (e.CellName == "終了buttonCell")
            {
                this.Hide();

            }
            
        }

        public void FlushButton(Keys keyCode)
        {
            ButtonCell target = null;
            Button target2 = null;
            switch (keyCode)
            {
                case Keys.F3:
                    target2 = this.ButtonF3;
                    String cname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (cname == "品番textBoxCell")
                    {
                        //GrapeCity.Win.MultiRow.EditingActions.CommitRow.Execute(gcMultiRow1);
                        //gcMultiRow1.RowCount += 1;
                        fsSyohin = new 商品検索Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                    }
                    break;
                case Keys.F5:
                    target2 = this.ButtonF5;
                    cname = gcMultiRow1.CurrentCellPosition.CellName;

                    if (cname == "メーカーtextBoxCell")
                    {
                        object val = this.gcMultiRow1.GetValue(gcMultiRow1.CurrentCellPosition.RowIndex, "品番textBoxCell");
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
                    gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
                    target = (ButtonCell)this.gcMultiRow1.ColumnFooters[0].Cells["クリアbuttonCell"];
                    gcMultiRow1.Rows.Clear(); 
                    break;
                case Keys.F10:
                    gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);
                    target = (ButtonCell)this.gcMultiRow1.ColumnFooters[0].Cells["終了buttonCell"];
                    this.Hide();
                    break;
                default:
                    break;
            }

        }

        /*
                void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
                //        private void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
                {
                    // Cell.Nameプロパティが"textBoxCell1"の場合 
                    if (e.CellName == "品番textBoxCell" )
                    {
                        //string _serchcode = "0";
                        if (gcMultiRow1.GetValue(e.RowIndex,e.CellIndex) != null)
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
                            command.CommandText = "商品問合せ";
                            command.Parameters.AddWithValue("@商品コード", gcMultiRow1.GetValue(e.RowIndex, e.CellIndex));

                            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
                            DataSet dataSet = new DataSet();
                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            adapter.Fill(dataSet);

                            //DataTableオブジェクトを用意
                            DataTable dataTable = dataSet.Tables[0];

                            dataTable.AcceptChanges();

                            //this.gcMultiRow1.DataSource = dataTable;

                            dataSet.Tables[0].Rows[0]["商品名"].ToString();

                            // 種別のドロップダウンリストの設定
                            //ComboBoxCell comboBoxCell = template1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"] as ComboBoxCell;
                            //            comboBoxCell.DataSource = this._ShakaiHokenSample.Tables[tableType];
                            //comboBoxCell.DataSource = dataTable;
                            //comboBoxCell.DisplayMember = "オペレーター名";
                            //comboBoxCell.ValueMember = "オペレーターコード";
                            //comboBoxCell.DataField = "オペレーターコード";
                            //gcMultiRow1.Template = template1;

                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["メーカーtextBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["メーカーコード"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["メーカー品番textBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["メーカー品番"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["車種textBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["車種"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["現在庫numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["現在在庫数"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["発注残数numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["発注残数"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["JANコードtextBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["バーコード"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["掛率numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["掛率"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["商品名textBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["商品名"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["可能数numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["可能数"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["原価numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["原価"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["定価numericUpDownCell"].CellIndex, dataSet.Tables[0].Rows[0]["定価"]);
                            this.gcMultiRow1.SetValue(e.RowIndex, gcMultiRow1.Template.Row.Cells["棚番textBoxCell"].CellIndex, dataSet.Tables[0].Rows[0]["棚番"]);
                    
                            //this.gcMultiRow1.DataSource = dataTable;

                            // 交互行のスタイルを設定
                            //this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(239, 255, 250);
                            // ユーザーによるリサイズを許可しない
                            //this.gcMultiRow1.AllowUserToResize = false;

                        }

                    }

                }
        */
        public string ReceiveDataSyohin
        {
            set
            {
                receiveDataSyohin = value;
                if (value != "")
                {
                    gcMultiRow1.Rows.Add();
                    gcMultiRow1.EndEdit();
                    this.gcMultiRow1.SetValue(gcMultiRow1.CurrentCellPosition.RowIndex - 1, gcMultiRow1.Template.Row.Cells["品番textBoxCell"].CellIndex, receiveDataSyohin.ToString());
                    //SendKeys.Send("{ENTER}");
                    gcMultiRow1.EndEdit();
                    gcMultiRow1.CurrentCellPosition = new CellPosition(gcMultiRow1.CurrentCellPosition.RowIndex-1, gcMultiRow1.Template.Row.Cells["品番textBoxCell"].CellIndex);
                    //gcMultiRow1.RowCount += 1;
                    //gcMultiRow1.Rows.Add();
                }
                //gcMultiRow1.EndEdit();
                //SendKeys.Send("{ENTER}");
                //gcMultiRow1.Rows.Add();
                //gcMultiRow1.RowCount += 1;
            }
            get
            {
                return receiveDataSyohin;
            }
        }

        private void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {

        }

    }
}
