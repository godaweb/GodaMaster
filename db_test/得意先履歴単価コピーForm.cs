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
using db_test.得意先履歴単価コピーDataSetTableAdapters;

namespace db_test
{
    public partial class 得意先履歴単価コピーForm : Form
    {
        private string receiveDataToksaki = "";
        得意先検索Form fsToksaki;

        private DataTable dataTable;
        private 得意先履歴単価コピーDataSet 得意先履歴単価コピーdataSet;
        private T売上単価テーブルTableAdapter T売上単価テーブルtableAdapter;
        private W得意先自動生成単価TableAdapter W得意先自動生成単価tableAdapter;

        int rowcnt = 0;
        int fireKbn = 1;

        public 得意先履歴単価コピーForm()
        {
            InitializeComponent();
            fsToksaki = new 得意先検索Form();
            fsToksaki.Owner = this;
        }

        private void 得意先履歴単価コピーForm_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 得意先履歴単価コピーTemplate();
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

            // イベント
            //gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);

            // GcMultiRowがフォーカスを失ったときに
            // セルのCellStyle.SelectionBackColorとCellStyle.SelectionForeColorを表示しない場合はtrue。
            // それ以外の場合はfalse。
            gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveRight, Keys.Right);

            // 既定のショートカットキーを削除する
            this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);

            // 各ファンクションキーに対応するアクションを登録する
            this.gcMultiRow1.ShortcutKeyManager.Register(new 出荷指示一覧表FunctionKeyAction(Keys.F3), Keys.F3);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["元得意先コードtextBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["元得意先名textBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["先得意先コードtextBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["先得意先名textBoxCell"].Value = "";

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "元得意先コードtextBoxCell");

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            string toksakicd_moto;
            string toksakicd_saki;
            string syohincd_from;
            string syohincd_to;

            switch (e.CellName)
            {
                case "検索buttonCell":
                    toksakicd_moto = this.gcMultiRow1.ColumnHeaders[0].Cells["元得意先コードtextBoxCell"].Value.ToString();
                    toksakicd_saki = this.gcMultiRow1.ColumnHeaders[0].Cells["先得意先コードtextBoxCell"].Value.ToString();
                    syohincd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Value.ToString();
                    syohincd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Value.ToString();

                    // コントロールの描画を停止する
                    gcMultiRow1.SuspendLayout();

                    //dataTable = 発注入力dataSet.T発注ファイル;

                    得意先履歴単価コピーdataSet = ((得意先履歴単価コピーDataSet)(this.得意先履歴単価コピーDataSet1));
                    //dataTable = 商品変換マスタメンテdataSet.WT商品変換マスタ;

                    W得意先自動生成単価tableAdapter = new W得意先自動生成単価TableAdapter();

                    try
                    {
                        //myTA.DeleteWK受注Bファイル();
                        //T受注ファイルtableAdapter.FillBy受注番号(受注ファイルdataSet.T受注ファイル, serchcode);
                        dataTable = W得意先自動生成単価tableAdapter.GetDataBy(toksakicd_moto, toksakicd_moto, syohincd_from, syohincd_to, "1");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                        return;
                    }

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("データがありません。");
                        return;
                    }

                    gcMultiRow1.DataSource = dataTable;

                    break;
                case "確認buttonCell":
                    toksakicd_moto = this.gcMultiRow1.ColumnHeaders[0].Cells["元得意先コードtextBoxCell"].Value.ToString();
                    toksakicd_saki = this.gcMultiRow1.ColumnHeaders[0].Cells["先得意先コードtextBoxCell"].Value.ToString();
                    syohincd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Value.ToString();
                    syohincd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Value.ToString();

                    // コントロールの描画を停止する
                    gcMultiRow1.SuspendLayout();

                    得意先履歴単価コピーdataSet = ((得意先履歴単価コピーDataSet)(this.得意先履歴単価コピーDataSet1));

                    W得意先自動生成単価tableAdapter = new W得意先自動生成単価TableAdapter();

                    try
                    {
                        dataTable = W得意先自動生成単価tableAdapter.GetDataBy(toksakicd_moto, toksakicd_moto, syohincd_from, syohincd_to, "1");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                        return;
                    }

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("データがありません。");
                        return;
                    }

                    得意先履歴単価コピーdataSet = ((得意先履歴単価コピーDataSet)(this.得意先履歴単価コピーDataSet1));

                    T売上単価テーブルtableAdapter = new T売上単価テーブルTableAdapter();

                    try
                    {
                        T売上単価テーブルtableAdapter.GetDataBy(toksakicd_saki, syohincd_from, syohincd_to);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                        return;
                    }

                    MessageBox.Show("単価をコピーしました。\n");

                    break;
            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "元得意先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    break;
                case "商品コード始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "元得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "元得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終textBoxCell");
                    }
                    break;
                case "商品コード終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "先得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "先得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "先得意先コードtextBoxCell");
                    }
                    break;
                case "先得意先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    break;
                case "クリアbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "先得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "先得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "確認buttonCell");
                    }
                    break;
                case "確認buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    break;
                case "終了buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "元得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "元得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "元得意先コードtextBoxCell");
                    }
                    break;
            }
        }
    }
}
