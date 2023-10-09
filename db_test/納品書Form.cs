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

namespace db_test
{
    public partial class 納品書Form : Form
    {
        private string receiveDataToksaki = "";
        得意先検索Form fsToksaki;

        int rowcnt = 0;
        int fireKbn = 1;
        string str伝票種類=null;
        string str統一伝票名=null;
        string str区分=null;
        string strオプション=null;
        string str伝票_区分 = null;
        //1:自社_新規、2:ドラスタ_新規、3:統一_新規
        //4:自社_再発行、5:ドラスタ_再発行、6:統一_再発行
        private DataSet dataSet;

        public 納品書Form()
        {
            InitializeComponent();
            fsToksaki = new 得意先検索Form();
            fsToksaki.Owner = this;
        }

        private void 納品書Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 納品書Template();
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
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);

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

            // コンボボックス
            GcComboBoxCell comboBoxCell01 = this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell01.DataSource = Utility.GetComboBoxData("SELECT オペレーターコード, オペレーター名 FROM Tオペレーターマスタ ORDER BY オペレーターコード");
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.ListHeaderPane.Visible = false;
            comboBoxCell01.TextSubItemIndex = 0;
            comboBoxCell01.TextSubItemIndex = 1;
            comboBoxCell01.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell01.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell01.TextFormat = "[1]";
            GcComboBoxCell comboBoxCell02 = this.gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell02.DataSource = Utility.GetComboBoxData("SELECT 担当者コード, 担当者名 FROM T担当者マスタ ORDER BY 担当者コード");
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.ListHeaderPane.Visible = false;
            comboBoxCell02.TextSubItemIndex = 0;
            comboBoxCell02.TextSubItemIndex = 1;
            comboBoxCell02.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell02.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell02.TextFormat = "[1]";
            GcComboBoxCell comboBoxCell03 = this.gcMultiRow1.ColumnHeaders[0].Cells["得分類ＢgcComboBoxCell"] as GcComboBoxCell;
            comboBoxCell03.DataSource = Utility.GetComboBoxData("SELECT 得分類Ｂコード, 得分類Ｂ名 FROM T得分類Ｂマスタ WHERE Len(得分類Ｂコード)=2 ORDER BY 得分類Ｂコード");
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.ListHeaderPane.Visible = false;
            comboBoxCell03.TextSubItemIndex = 0;
            comboBoxCell03.TextSubItemIndex = 1;
            comboBoxCell03.ListColumns.ElementAt(0).AutoWidth = true;
            comboBoxCell03.ListColumns.ElementAt(1).AutoWidth = true;
            comboBoxCell03.TextFormat = "[1]";

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].Value = "1";
            gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].Value = "1";
            gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Value = null;
            gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Value = "*";
            gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Value = "*";

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "伝票種類textBoxCell");

        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "終了buttonCell")
            {
                this.Hide();
            }
            if (e.CellName == "プレビューbuttonCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].Value == "1")
                {
                    if (create納品書Data() == 0)
                    {
                        プレビューForm プレビューform = new プレビューForm();

                        プレビューform.dataSet = dataSet;
                        プレビューform.rptName = "納品書CrystalReport";

                        プレビューform.Show();
                    }
                    else
                    {
                        MessageBox.Show("データがありません", "納品書");
                    }
                }
                else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].Value == "2")
                {
                    if (create納品書ドラスタData() == 0)
                    {
                        プレビューForm プレビューform = new プレビューForm();

                        プレビューform.dataSet = dataSet;
                        プレビューform.rptName = "納品書ドラスタCrystalReport";

                        プレビューform.Show();
                    
                    }
                    else
                    {
                        MessageBox.Show("データがありません", "納品書");
                    }
                }
                else if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].Value == "3")
                {
                    if (create納品書専用Data() == 0)
                    {
                        プレビューForm プレビューform = new プレビューForm();

                        プレビューform.dataSet = dataSet;
                        プレビューform.rptName = "納品書専用CrystalReport";

                        プレビューform.Show();
                    }
                    else
                    {
                        MessageBox.Show("データがありません", "納品書");
                    }
                }



            }
            if (e.CellName == "印刷buttonCell")
            {
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].Value == "1")
                {
                    if (create納品書Data() == 0)
                    {
                        プレビューForm プレビューform = new プレビューForm();

                        プレビューform.dataSet = dataSet;
                        プレビューform.rptName = "納品書CrystalReport";

                        プレビューform.Show();
                    }
                    else
                    {
                        MessageBox.Show("データがありません", "納品書");
                    }
                }
            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "伝票種類textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].EditedFormattedValue != null)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].EditedFormattedValue == "1" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].EditedFormattedValue == "2" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].EditedFormattedValue == "3")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "統一伝票名textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].EditedFormattedValue != null)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].EditedFormattedValue == "1" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].EditedFormattedValue == "2" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].EditedFormattedValue == "3" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].EditedFormattedValue == "4")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "区分textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].EditedFormattedValue != null)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].EditedFormattedValue == "1" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].EditedFormattedValue == "2")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "伝票種類textBoxCell":
                    gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].Value = (string)gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].EditedFormattedValue;
                    str伝票種類 = (string)gcMultiRow1.ColumnHeaders[0].Cells["伝票種類textBoxCell"].Value;
                    break;
                case "統一伝票名textBoxCell":
                    gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].Value = (string)gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].EditedFormattedValue;
                    str統一伝票名 = (string)gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].Value;
                    break;
                case "区分textBoxCell":
                    gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].Value = (string)gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].EditedFormattedValue;
                    str区分 = (string)gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].Value;
                    if (str区分 == "1")
                    {
                        if (str伝票種類 == "1")
                        {
                            str伝票_区分 = "1";
                        }
                        else if (str伝票種類 == "2")
                        {
                            str伝票_区分 = "2";
                        }
                        else if (str伝票種類 == "3")
                        {
                            str伝票_区分 = "3";
                        }
                    }
                    else if (str区分 == "2")
                    {
                        if (str伝票種類 == "1")
                        {
                            str伝票_区分 = "4";
                        }
                        else if (str伝票種類 == "2")
                        {
                            str伝票_区分 = "5";
                        }
                        else if (str伝票種類 == "3")
                        {
                            str伝票_区分 = "6";
                        }
                    }
                    changeEnabled(str伝票_区分);
                    break;
            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "伝票種類textBoxCell":
                    if (str伝票種類 == "3")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "統一伝票名textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "統一伝票名textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "統一伝票名textBoxCell");
                        }
                    }
                    else
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                    }
                    break;
                case "統一伝票名textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                    }
                    break;
            }
            if (str伝票_区分 == "1")
            {
                switch (e.CellName)
                {
                    case "区分textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        break;
                    case "処理日textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        break;
                    case "オペレーターコードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        break;
                    case "オペレーターgcComboBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        break;
                    case "プレビューbuttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターgcComboBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターgcComboBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        break;
                    case "印刷buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        break;
                    case "終了buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        break;
                }
            }
            else if (str伝票_区分 == "2")
            {
                switch (e.CellName)
                {
                    case "区分textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        break;
                    case "処理日textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        break;
                    case "オペレーターコードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        break;
                    case "オペレーターgcComboBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        break;
                    case "担当者コードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        break;
                    case "担当者gcComboBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        break;
                    case "得意先コードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        break;
                    case "プレビューbuttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        break;
                    case "印刷buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        break;
                    case "終了buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        break;
                }
            }
            else if (str伝票_区分 == "3")
            {
                switch (e.CellName)
                {
                    case "区分textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        break;
                    case "処理日textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        break;
                    case "オペレーターコードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "処理日textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        break;
                    case "オペレーターgcComboBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        break;
                    case "プレビューbuttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        break;
                    case "印刷buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        break;
                    case "終了buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        break;
                }
            }
            else if (str伝票_区分 == "4")
            {
                switch (e.CellName)
                {
                    case "区分textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        break;
                    case "オペレーターコードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        break;
                    case "オペレーターgcComboBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        break;
                    case "担当者コードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        break;
                    case "担当者gcComboBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        break;
                    case "伝票番号始textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        break;
                    case "伝票番号終textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日始textBoxCell");
                        }
                        break;
                    case "発行日始textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日終textBoxCell");
                        }
                        break;
                    case "発行日終textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時始textBoxCell");
                        }
                        break;
                    case "発行日時始textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時終textBoxCell");
                        }
                        break;
                    case "発行日時終textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        break;
                    case "プレビューbuttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発行日時終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        break;
                    case "印刷buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        break;
                    case "終了buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        break;
                }
            }
            else if (str伝票_区分 == "5")
            {
                switch (e.CellName)
                {
                    case "区分textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        break;
                    case "オペレーターコードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        break;
                    case "オペレーターgcComboBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者コードtextBoxCell");
                        }
                        break;
                    case "担当者コードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オペレーターコードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        break;
                    case "担当者gcComboBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        break;
                    case "得意先コードtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "担当者textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        break;
                    case "伝票番号始textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        break;
                    case "伝票番号終textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オプションtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オプションtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オプションtextBoxCell");
                        }
                        break;
                    case "オプションtextBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                        }
                        break;
                    case "売上日始textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オプションtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "オプションtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                        }
                        break;
                    case "売上日終textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        break;
                    case "プレビューbuttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "売上日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        break;
                    case "印刷buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        break;
                    case "終了buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        break;
                }
            }
            else if (str伝票_区分 == "6")
            {
                switch (e.CellName)
                {
                    case "区分textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        break;
                    case "伝票番号始textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "区分コードtextBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        break;
                    case "伝票番号終textBoxCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        break;
                    case "プレビューbuttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票番号終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        break;
                    case "印刷buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                        }
                        break;
                    case "終了buttonCell":
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "伝票種類textBoxCell");
                        }
                        break;
                }
            }
        }

        void changeEnabled(string 伝票_区分)
        {
            if (伝票_区分 == "1")
            {
                gcMultiRow1.ColumnHeaders[0].Cells["処理日labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプション凡例labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Visible = false;
            }
            else if (伝票_区分 == "2")
            {
                gcMultiRow1.ColumnHeaders[0].Cells["処理日labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプション凡例labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Visible = false;
            }
            else if (伝票_区分 == "3")
            {
                gcMultiRow1.ColumnHeaders[0].Cells["処理日labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプション凡例labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Visible = false;
            }
            else if (伝票_区分 == "4")
            {
                gcMultiRow1.ColumnHeaders[0].Cells["処理日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプション凡例labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Visible = true;
            }
            else if (伝票_区分 == "5")
            {
                gcMultiRow1.ColumnHeaders[0].Cells["処理日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オプション凡例labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Visible = false;
            }
            else if (伝票_区分 == "6")
            {
                gcMultiRow1.ColumnHeaders[0].Cells["処理日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプション凡例labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時からlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Visible = false;
            }
            else
            {
                gcMultiRow1.ColumnHeaders[0].Cells["処理日labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オペレーターgcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["担当者gcComboBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オプション凡例labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時からlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Visible = true;
            }
        }

        private int create納品書Data()
        {
            int ret = 0;
            string 区分 = null;

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;

            区分 = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].Value;

            if (区分 == "1")
            {
                // ストアド プロシージャ名を指定
                command.CommandText = "納品書_通常";
                command.Parameters.AddWithValue("@得分類Ｂコード", "1");
                command.Parameters.AddWithValue("@処理日", this.gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Value);
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@オペレーターコード", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@オペレーターコード", "*");
                }
            }
            else if (区分 == "2")
            {
                // ストアド プロシージャ名を指定
                command.CommandText = "納品書_再発行";

                command.Parameters.AddWithValue("@得分類Ｂコード", "1");
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@オペレーターコード", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@オペレーターコード", "*");
                }

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@担当者コード", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@担当者コード", "*");
                }

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@売上伝票番号始", this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@売上伝票番号始", "0");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@売上伝票番号終", this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@売上伝票番号終", "99999999");
                }

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Value != null)
                {
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Value != "*" &&
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Value != null)
                    {
                        command.Parameters.AddWithValue("@納品書発行日時始", this.gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Value + " " + this.gcMultiRow1.ColumnHeaders[0].Cells["発行日時始textBoxCell"].Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@納品書発行日時始", this.gcMultiRow1.ColumnHeaders[0].Cells["発行日始textBoxCell"].Value + " " + "00:00:00");
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@納品書発行日時始", "*");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Value != null)
                {
                    if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Value != "*" &&
                        this.gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Value != null)
                    {
                        command.Parameters.AddWithValue("@納品書発行日時終", this.gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Value + " " + this.gcMultiRow1.ColumnHeaders[0].Cells["発行日時終textBoxCell"].Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@納品書発行日時終", this.gcMultiRow1.ColumnHeaders[0].Cells["発行日終textBoxCell"].Value + " " + "23:59:99");
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@納品書発行日時終", "*");
                }
            }
            else
            {
                return 9;
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);

            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            //dataTable.AcceptChanges();
            if (dataTable.Rows.Count <= 0)
            {
                ret = 1;
            }

            return ret;
        }

        private int create納品書ドラスタData()
        {
            int ret = 0;
            string 区分 = null;

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;

            区分 = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].Value;

            if (区分 == "1")
            {
                // ストアド プロシージャ名を指定
                command.CommandText = "納品書ドラスタ_通常";
                //command.Parameters.AddWithValue("@得分類Ｂコード", "1");
                command.Parameters.AddWithValue("@処理月日", this.gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Value);
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["チェックcheckBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@チェック", "1");
                }
                else
                {
                    command.Parameters.AddWithValue("@チェック", "0");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@オペレーターコード", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@オペレーターコード", "*");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@担当者コード", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@担当者コード", "*");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@得意先コード", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@得意先コード", "*");
                }
            }
            else if (区分 == "2")
            {
                // ストアド プロシージャ名を指定
                command.CommandText = "納品書ドラスタ_再発行";

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@オペレーターコード", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@オペレーターコード", "*");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@担当者コード", this.gcMultiRow1.ColumnHeaders[0].Cells["担当者コードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@担当者コード", "*");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@得意先コード", this.gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@得意先コード", "*");
                }

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@売上伝票番号始", this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@売上伝票番号始", "0");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@売上伝票番号終", this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@売上伝票番号終", "99999999");
                }

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@売上日始", this.gcMultiRow1.ColumnHeaders[0].Cells["売上日始textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@売上日始", "*");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@売上日終", this.gcMultiRow1.ColumnHeaders[0].Cells["売上日終textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@売上日終", "*");
                }

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@オプション", this.gcMultiRow1.ColumnHeaders[0].Cells["オプションtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@オプション", "*");
                }
            }
            else
            {
                return 9;
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);

            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            //dataTable.AcceptChanges();
            if (dataTable.Rows.Count <= 0)
            {
                ret = 1;
            }

            return ret;
        }

        private int create納品書専用Data()
        {
            int ret = 0;
            string 区分 = null;

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;

            区分 = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["区分textBoxCell"].Value;

            if (区分 == "1")
            {
                // ストアド プロシージャ名を指定
                command.CommandText = "納品書専用_通常";
                //command.Parameters.AddWithValue("@得分類Ｂコード", "1");
                command.Parameters.AddWithValue("@処理月日", this.gcMultiRow1.ColumnHeaders[0].Cells["処理日textBoxCell"].Value);
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["チェックcheckBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@チェック", "1");
                }
                else
                {
                    command.Parameters.AddWithValue("@チェック", "0");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@オペレーターコード", this.gcMultiRow1.ColumnHeaders[0].Cells["オペレーターコードtextBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@オペレーターコード", "*");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@伝票区分", this.gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@伝票区分", "*");
                }
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["得分類ＢgcComboBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@分類コード２", this.gcMultiRow1.ColumnHeaders[0].Cells["得分類ＢgcComboBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@分類コード２", "*");
                }
            }
            else if (区分 == "2")
            {
                // ストアド プロシージャ名を指定
                command.CommandText = "納品書専用_再発行";

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@売上伝票番号始", this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号始textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@売上伝票番号始", "0");
                }
                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@売上伝票番号終", this.gcMultiRow1.ColumnHeaders[0].Cells["伝票番号終textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@売上伝票番号終", "99999999");
                }

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].Value != "*" &&
                    this.gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@伝票区分", this.gcMultiRow1.ColumnHeaders[0].Cells["統一伝票名textBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@伝票区分", "*");
                }
                if (this.gcMultiRow1.ColumnHeaders[0].Cells["得分類ＢgcComboBoxCell"].Value != null)
                {
                    command.Parameters.AddWithValue("@分類コード２", this.gcMultiRow1.ColumnHeaders[0].Cells["得分類ＢgcComboBoxCell"].Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@分類コード２", "*");
                }
            }
            else
            {
                return 9;
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);

            //DataTableオブジェクトを用意
            DataTable dataTable = dataSet.Tables[0];

            //dataTable.AcceptChanges();
            if (dataTable.Rows.Count <= 0)
            {
                ret = 1;
            }

            return ret;
        }
    }
}
