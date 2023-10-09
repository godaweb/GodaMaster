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
using db_test.得意先履歴単価更新SetTableAdapters;

namespace db_test
{
    public partial class 得意先履歴単価更新Form : Form
    {
        private string receiveDataToksaki = "";
        得意先検索Form fsToksaki;

        private DataTable dataTable;
        private 得意先履歴単価更新Set 得意先履歴単価更新set;
        private T売上単価テーブルTableAdapter T売上単価テーブルtableAdapter;
        private W得意先履歴更新単価_２TableAdapter W得意先履歴更新単価_２tableAdapter;

        int rowcnt = 0;
        int fireKbn = 1;

        public 得意先履歴単価更新Form()
        {
            InitializeComponent();
            fsToksaki = new 得意先検索Form();
            fsToksaki.Owner = this;
        }

        private void 得意先履歴単価更新Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 得意先履歴単価更新Template();
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
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left | Keys.Control);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right | Keys.Control);
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

            // コンボボックス
            GcComboBoxCell comboBoxCell = this.gcMultiRow1.ColumnHeaders[0].Cells["在庫場所始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell.DataSource = Utility.GetComboBoxData("SELECT 倉庫コード, 倉庫名 FROM T倉庫マスタ ORDER BY 倉庫コード");
            comboBoxCell.ListHeaderPane.Visible = false;
            comboBoxCell.ListHeaderPane.Visible = false;
            comboBoxCell.ListHeaderPane.Visible = false;
            comboBoxCell.TextSubItemIndex = 0;
            comboBoxCell.TextSubItemIndex = 1;
            comboBoxCell.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell.TextFormat = "[1]";
            GcComboBoxCell comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["在庫場所終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT 倉庫コード, 倉庫名 FROM T倉庫マスタ ORDER BY 倉庫コード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";

            GcComboBoxCell comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";
            GcComboBoxCell comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.TextSubItemIndex = 0;
            comboBoxCell03.TextSubItemIndex = 1;
            comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell03.TextFormat = "[1]";

            GcComboBoxCell comboBoxCell04 = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell04.DataSource = Utility.GetComboBoxData("SELECT オペレーターコード, オペレーター名 FROM Tオペレーターマスタ ORDER BY オペレーターコード");
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.ListHeaderPane.Visible = false;
            comboBoxCell04.TextSubItemIndex = 0;
            comboBoxCell04.TextSubItemIndex = 1;
            comboBoxCell04.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell04.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell04.TextFormat = "[1]";
            GcComboBoxCell comboBoxCell05 = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーター終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell05.DataSource = Utility.GetComboBoxData("SELECT オペレーターコード, オペレーター名 FROM Tオペレーターマスタ ORDER BY オペレーターコード");
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.ListHeaderPane.Visible = false;
            comboBoxCell05.TextSubItemIndex = 0;
            comboBoxCell05.TextSubItemIndex = 1;
            comboBoxCell05.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell05.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell05.TextFormat = "[1]";

            GcComboBoxCell comboBoxCell06 = this.gcMultiRow1.ColumnHeaders[0].Cells["部課始gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell06.DataSource = Utility.GetComboBoxData("SELECT 部課コード, 部課名 FROM T部課マスタ ORDER BY 部課コード");
            comboBoxCell06.ListHeaderPane.Visible = false;
            comboBoxCell06.ListHeaderPane.Visible = false;
            comboBoxCell06.ListHeaderPane.Visible = false;
            comboBoxCell06.TextSubItemIndex = 0;
            comboBoxCell06.TextSubItemIndex = 1;
            comboBoxCell06.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell06.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell06.TextFormat = "[1]";
            GcComboBoxCell comboBoxCell07 = this.gcMultiRow1.ColumnHeaders[0].Cells["部課終gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell07.DataSource = Utility.GetComboBoxData("SELECT 部課コード, 部課名 FROM T部課マスタ ORDER BY 部課コード");
            comboBoxCell07.ListHeaderPane.Visible = false;
            comboBoxCell07.ListHeaderPane.Visible = false;
            comboBoxCell07.ListHeaderPane.Visible = false;
            comboBoxCell07.TextSubItemIndex = 0;
            comboBoxCell07.TextSubItemIndex = 1;
            comboBoxCell07.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell07.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell07.TextFormat = "[1]";

            // 初期表示
            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["親得意先コードtextBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["親得意先名textBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先名始textBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先名終textBoxCell"].Value = "";

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "親得意先コードtextBoxCell");

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            string toksakicd_oya;
            string toksakicd_from;
            string toksakicd_to;
            string syohincd_from;
            string syohincd_to;

            switch (e.CellName)
            {
                case "検索buttonCell":
                    toksakicd_oya = this.gcMultiRow1.ColumnHeaders[0].Cells["親得意先コードtextBoxCell"].Value.ToString();
                    toksakicd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value.ToString();
                    toksakicd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value.ToString();
                    syohincd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Value.ToString();
                    syohincd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Value.ToString();

                    // コントロールの描画を停止する
                    gcMultiRow1.SuspendLayout();

                    得意先履歴単価更新set = ((得意先履歴単価更新Set)(this.得意先履歴単価更新Set1));

                    W得意先履歴更新単価_２tableAdapter = new W得意先履歴更新単価_２TableAdapter();

                    try
                    {
                        dataTable = W得意先履歴更新単価_２tableAdapter.GetDataBy1(toksakicd_oya, toksakicd_from, toksakicd_to, syohincd_from, syohincd_to, "1");

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
                    toksakicd_oya = this.gcMultiRow1.ColumnHeaders[0].Cells["親得意先コードtextBoxCell"].Value.ToString();
                    toksakicd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value.ToString();
                    toksakicd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value.ToString();
                    syohincd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Value.ToString();
                    syohincd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Value.ToString();

                    // コントロールの描画を停止する
                    gcMultiRow1.SuspendLayout();

                    得意先履歴単価更新set = ((得意先履歴単価更新Set)(this.得意先履歴単価更新Set1));

                    W得意先履歴更新単価_２tableAdapter = new W得意先履歴更新単価_２TableAdapter();

                    try
                    {
                        dataTable = W得意先履歴更新単価_２tableAdapter.GetDataBy1(toksakicd_oya, toksakicd_from, toksakicd_to, syohincd_from, syohincd_to, "1");

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

                    T売上単価テーブルtableAdapter = new T売上単価テーブルTableAdapter();

                    try
                    {
                        T売上単価テーブルtableAdapter.GetDataBy();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                        return;
                    }

                    MessageBox.Show("単価を更新しました。\n");

                    break;
                case "リストbuttonCell":
                    toksakicd_oya = this.gcMultiRow1.ColumnHeaders[0].Cells["親得意先コードtextBoxCell"].Value.ToString();
                    toksakicd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード始textBoxCell"].Value.ToString();
                    toksakicd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コード終textBoxCell"].Value.ToString();
                    syohincd_from = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Value.ToString();
                    syohincd_to = this.gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Value.ToString();

                    // コントロールの描画を停止する
                    gcMultiRow1.SuspendLayout();

                    得意先履歴単価更新set = ((得意先履歴単価更新Set)(this.得意先履歴単価更新Set1));

                    W得意先履歴更新単価_２tableAdapter = new W得意先履歴更新単価_２TableAdapter();

                    try
                    {
                        dataTable = W得意先履歴更新単価_２tableAdapter.GetDataBy1(toksakicd_oya, toksakicd_from, toksakicd_to, syohincd_from, syohincd_to, "1");

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

                    T売上単価テーブルtableAdapter = new T売上単価テーブルTableAdapter();

                    try
                    {
                        dataTable = W得意先履歴更新単価_２tableAdapter.GetDataBy();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ユーザー・データの読み込みでエラーが発生しました。\n" + ex.Message);
                        return;
                    }

                    プレビューForm プレビューform = new プレビューForm();

                    プレビューform.dataTable = dataTable;
                    プレビューform.rptName = "得意先履歴単価更新リストCrystalReport";
                    プレビューform.Show();

                    break;
            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "親得意先コードtextBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    break;
                case "得意先コード始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "親得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "親得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    break;
                case "得意先コード終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード始textBoxCell");
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
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード終textBoxCell");
                    }
                    break;
                case "商品コード終textBoxCell":
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
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
                    }
                    break;
                case "リストbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "クリアbuttonCell");
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
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "リストbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    break;
                case "終了buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "親得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "親得意先コードtextBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "確認buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "親得意先コードtextBoxCell");
                    }
                    break;
            }
        }
    }
}
