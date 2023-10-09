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
using db_test.monotaRO一括発注DataSetTableAdapters;
using System.Drawing.Printing;

namespace db_test
{
    public partial class monotaRO一括発注Form : Form
    {
        DataTable dataTable;
        private monotaRO一括発注DataSet monotaRO一括発注dataSet;  // データセット
        int skipFlg=0;

        public monotaRO一括発注Form()
        {
            InitializeComponent();

            gcMultiRow1.SetErrorHandle();
        }

        private void monotaRO一括発注Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new monotaRO一括発注Template();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            gcMultiRow1.ScrollBars = ScrollBars.Vertical;

            //20190414 Start
            //this.WindowState = FormWindowState.Maximized;
            //20190414 End

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
            gcMultiRow1.CellValueChanged += new EventHandler<CellEventArgs>(gcMultiRow1_CellValueChanged);
            gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            //gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            //gcMultiRow1.DataError +=new EventHandler<DataErrorEventArgs>(gcMultiRow1_DataError);
            //gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            //gcMultiRow1.PreviewKeyDown +=new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            //gcMultiRow1.CellClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellClick);
            //gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);

            // GcMultiRowがフォーカスを失ったときに
            // セルのCellStyle.SelectionBackColorとCellStyle.SelectionForeColorを表示しない場合はtrue。
            // それ以外の場合はfalse。
            gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);
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
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F4);    // 検索実行
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F5);    // 検索実行
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F6);    // 画面クリア
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);   // 終了

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new monotaRO一括発注FunctionKeyAction(Keys.F4), Keys.F4);
            this.gcMultiRow1.ShortcutKeyManager.Register(new monotaRO一括発注FunctionKeyAction(Keys.F5), Keys.F5);
            this.gcMultiRow1.ShortcutKeyManager.Register(new monotaRO一括発注FunctionKeyAction(Keys.F6), Keys.F6);
            this.gcMultiRow1.ShortcutKeyManager.Register(new monotaRO一括発注FunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // 初期表示
            //gcMultiRow1.Select();
            skipFlg = 1;
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "実行buttonCell");
        
        }

        void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.CellName == "伝票確認textBoxCell")
            {
                if ((string)gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue == "9")
                {
//                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 2, "受注行番号textBoxCell");
//                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "実行buttonCell");
                }
                else if ((string)gcMultiRow1.ColumnFooters[0].Cells["伝票確認textBoxCell"].EditedFormattedValue == "0")
                {
                    gcMultiRow1.EndEdit();
                    // 編集した行のコミット処理
                    GrapeCity.Win.MultiRow.EditingActions.CommitRow.Execute(gcMultiRow1);
                    for (int i = 0; i < gcMultiRow1.RowCount; i++)
                    {

                        if (Convert.ToInt32(gcMultiRow1.GetValue(i, "算出発注数")) <= 0 )
                        {
                            continue;
                        }

                        string strSQL = null;
                        long W_Syori_no;      // 処理番号
                        string hatyusu;
                        string hatyurenban;
                        //dataTable.Rows[i]["発注数"] = Convert.ToInt32(gcMultiRow1.GetValue(i, "算出発注数"));

                        SqlDb sqlDb = new SqlDb();
                        sqlDb.Connect();

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

                        hatyusu = gcMultiRow1.GetValue(i, "算出発注数").ToString();
                        hatyurenban = (i + 1).ToString();

                        strSQL = "INSERT INTO T発注戻しファイル (";
                        strSQL = strSQL + " 本支店区分, 処理コード, 入力区分, 処理番号,";
                        strSQL = strSQL + " エラーフラグ,受発注行数,発注番号,相手先受付番号,自社発注番号,";
                        strSQL = strSQL + " 処理日,発注日,納期,回答納期,納期回答,";
                        strSQL = strSQL + " 処理区,仕入先コード,仕名,仕入分類,事業所コード,";
                        strSQL = strSQL + " 仕入担当者コード,仕入切捨区分,仕入税区分,伝票摘要,商品コード,";
                        strSQL = strSQL + " 商名,規格,形式寸法,材質,分類,";
                        strSQL = strSQL + " 在庫管理区分,品種コード,メーカーコード,入数,単位コード,";
                        strSQL = strSQL + " 倉庫コード,ケース数,発注数,入荷累計数,仕入累計数,";
                        strSQL = strSQL + " 発注単価,発注金額,外内税区分,消費税率,新消費税率,";
                        strSQL = strSQL + " 新消費税適用,明細摘要,受注番号,受注納期,得意先コード,";
                        strSQL = strSQL + " 得名,得意先担当者コード,チェック,完了フラグ,WS_ID,";
                        strSQL = strSQL + " オペレーターコード,修正オペレーターコード,発注行,処理月日,管理年月,";
                        strSQL = strSQL + " 発注行番号,定価,原価掛率,メーカー品番,納入先コード,";
                        strSQL = strSQL + " 回答コード,回答名,発注有無区分,発注摘要,在庫数,";
                        strSQL = strSQL + " 受注残数,発注残数,発注書発行フラグ,オンラインフラグ)";
                        strSQL = strSQL + " SELECT ";
                        strSQL = strSQL + " 本支店区分, 処理コード, 入力区分, " + W_Syori_no.ToString() + " AS 処理番号,";
                        strSQL = strSQL + " エラーフラグ,受発注行数,発注番号,相手先受付番号,自社発注番号,";
                        strSQL = strSQL + " 処理日,発注日,納期,回答納期,納期回答,";
                        strSQL = strSQL + " 処理区,仕入先コード,仕名,仕入分類,事業所コード,";
                        strSQL = strSQL + " 仕入担当者コード,仕入切捨区分,仕入税区分,伝票摘要,商品コード,";
                        strSQL = strSQL + " 商名,規格,形式寸法,材質,分類,";
                        strSQL = strSQL + " 在庫管理区分,品種コード,メーカーコード,入数,単位コード,";
                        strSQL = strSQL + " 倉庫コード,ケース数," + hatyusu + " AS 発注数,入荷累計数,仕入累計数,";
                        strSQL = strSQL + " 発注単価,発注金額,外内税区分,消費税率,新消費税率,";
                        strSQL = strSQL + " 新消費税適用,明細摘要,受注番号,受注納期,得意先コード,";
                        strSQL = strSQL + " 得名,得意先担当者コード,チェック,完了フラグ,WS_ID,";
                        strSQL = strSQL + " オペレーターコード,修正オペレーターコード,発注行,処理月日,管理年月,";
                        strSQL = strSQL + " 発注行番号,定価,原価掛率,メーカー品番,納入先コード,";
                        strSQL = strSQL + " 回答コード,回答名,発注有無区分,発注摘要,在庫数,";
                        strSQL = strSQL + " 受注残数,発注残数,発注書発行フラグ,オンラインフラグ";
                        strSQL = strSQL + " FROM WT発注戻しファイル";
                        strSQL = strSQL + " WHERE 発注連番=" + hatyurenban;

                        sqlDb.BeginTransaction();
                        sqlDb.ExecuteSql(strSQL, -1);
                        sqlDb.CommitTransaction();

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
                        strSQL = strSQL + "1 AS 区分,";
                        strSQL = strSQL + "'1' AS 事業所, ";
                        strSQL = strSQL + W_Syori_no + " AS 処理番,";
                        strSQL = strSQL + "0 AS 番号,";
                        strSQL = strSQL + "0 AS 仕入番,";
                        strSQL = strSQL + "0 AS 入金番, ";
                        strSQL = strSQL + "101 AS システム,";
                        strSQL = strSQL + " GETDATE() AS 日付, ";
                        strSQL = strSQL + "1 AS 更新,";
                        strSQL = strSQL + "'01' AS ＯＰ";
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
                        /*
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
                        */
                    }

                }
            }
        }

        void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "実行buttonCell":
                    if (skipFlg == 1)
                    {
                        skipFlg = 0;
                    }
                    else 
                    {
                        if (createData() != 0)
                        {
                            MessageBox.Show("データがありません。", "monotaRO一括発注");
                            this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "実行buttonCell");
                        }
                        else
                        {
                            MessageBox.Show("データを作成しました。", "monotaRO一括発注");
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "算出発注数");
                        }
                    }
                    break;
                case "クリアbuttonCell":
                    if (gcMultiRow1.Rows.Count > 0)
                    {
                        dataTable = (DataTable)gcMultiRow1.DataSource;
                        dataTable.Clear();
                    }
                    break;
                case "終了buttonCell":
                    this.Close();
                    break;

                case "プレビューbuttonCell":
                    PrintInitialize();
                    gcMultiRow1.ShowPrintPreview();
                    break;

                case "印刷buttonCell":
                    PrintInitialize();
                    gcMultiRow1.ShowPrint();
                    break;
            }
        }


        private void PrintInitialize()
        {
            gcMultiRow1.PrintSettings.AutoFitWidth = true;
            gcMultiRow1.PrintSettings.PrintStyle = PrintStyle.Compact;
            gcMultiRow1.ColumnHeaders[0].Printable = false;
            gcMultiRow1.ColumnFooters[0].Printable = false;
        }


        private int createData()
        {
            int ret = 0;

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;
            // ストアド プロシージャ名を指定
            command.CommandText = "monotaRO一括発注作成";
            // ストアド プロシージャを実行
            command.ExecuteNonQuery();

            // コントロールの描画を停止する
            gcMultiRow1.SuspendLayout();

            monotaRO一括発注dataSet = ((monotaRO一括発注DataSet)(this.monotaRO一括発注DataSet1));
            dataTable = monotaRO一括発注dataSet.WT発注戻しファイル;

            try
            {
                dataTable = this.wT発注戻しファイルTableAdapter1.GetDataBy();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                return 1;
            }

            if (dataTable.Rows.Count == 0)
            {
                MessageBox.Show("データのがありません。\n");
                return 1;
            }

            string shohincd = dataTable.Rows[0]["商品コード"].ToString();
            int lotsu = Convert.ToInt32(dataTable.Rows[0]["単位コード"]);
            int kanosu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[0]["可能数"]));
            int jyusu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[0]["受注数"]));
            int hatsu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[0]["発注数"]));
            int jyuzansu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[0]["受注残数"]));
            int hatzansu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[0]["発注残数"]));
            int zaikosu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[0]["在庫数"]));

            if (lotsu > 1)
            {
                if (kanosu - jyusu < 0)
                {
                    dataTable.Rows[0]["発注数"] = lotsu;
                    dataTable.Rows[0]["算出発注数"] = lotsu;
                }
                else
                {
                    dataTable.Rows[0]["発注数"] = 0;
                    dataTable.Rows[0]["算出発注数"] = 0;
                }
            }
            else
            {
                if (kanosu - jyusu < 0)
                {
                    dataTable.Rows[0]["発注数"] = jyusu;
                    dataTable.Rows[0]["算出発注数"] = jyusu;
                }
                else
                {
                    dataTable.Rows[0]["発注数"] = 0;
                    dataTable.Rows[0]["算出発注数"] = 0;
                }
            }

            dataTable.Rows[0]["可能数"] = zaikosu - jyuzansu - jyusu;

            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                if (shohincd == dataTable.Rows[i]["商品コード"].ToString()) 
                {
                    if (lotsu > 0)
                    {
                        dataTable.Rows[i]["在庫数"] = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[i]["在庫数"])) + (lotsu - jyusu);
                    }
                }
                shohincd = dataTable.Rows[i]["商品コード"].ToString();
//                kanosu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[i]["可能数"]));
                jyusu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[i]["受注数"]));
                hatsu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[i]["発注数"]));
                jyuzansu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[i]["受注残数"]));
                hatzansu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[i]["発注残数"]));
                zaikosu = Convert.ToInt32(Convert.ToDecimal(dataTable.Rows[i]["在庫数"]));
                lotsu = Convert.ToInt32(dataTable.Rows[i]["単位コード"]);
                kanosu = zaikosu - jyuzansu - jyusu;
                dataTable.Rows[i]["可能数"] = zaikosu - jyuzansu - jyusu;

                if (lotsu > 1)
                {
                    if (kanosu - jyusu < 0)
                    {
                        dataTable.Rows[i]["発注数"] = lotsu;
                        dataTable.Rows[i]["算出発注数"] = lotsu;
                    }
                    else
                    {
                        dataTable.Rows[i]["発注数"] = 0;
                        dataTable.Rows[i]["算出発注数"] = 0;
                    }
                }
                else
                {
                    if (kanosu - jyusu < 0)
                    {
                        dataTable.Rows[i]["発注数"] = jyusu;
                        dataTable.Rows[i]["算出発注数"] = jyusu;
                    }
                    else
                    {
                        dataTable.Rows[i]["発注数"] = 0;
                        dataTable.Rows[i]["算出発注数"] = 0;
                    }
                }


            }

            gcMultiRow1.DataSource = dataTable;

            return 0;
        }

        public void FlushButton(Keys keyCode)
        {
            Button target = null;
            switch (keyCode)
            {
                case Keys.F4:
                    target = this.ButtonF4;
                    EditingActions.CommitRow.Execute(gcMultiRow1);
                    String fname = gcMultiRow1.CurrentCellPosition.CellName;
                    if (fname == "算出発注数" & gcMultiRow1.Rows.Count > 0)
                    {
                        gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnFooter, 0, "伝票確認textBoxCell");
                    }
                    break;
                case Keys.F5:
                    target = this.ButtonF5;
                    if (skipFlg == 1)
                    {
                        skipFlg = 0;
                    }
                    else 
                    {
                        if (createData() != 0)
                        {
                            MessageBox.Show("データがありません。", "monotaRO一括発注");
                            this.gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "実行buttonCell");
                        }
                        else
                        {
                            MessageBox.Show("データを作成しました。", "monotaRO一括発注");
                            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(0, "算出発注数");
                        }
                    }
                    break;
                case Keys.F6:
                    target = this.ButtonF6;
                    try
                    {
                        if (gcMultiRow1.Rows.Count > 0)
                        {
                            dataTable = (DataTable)gcMultiRow1.DataSource;
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
                    break;
                case Keys.F10:
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

    }
}
