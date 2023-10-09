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
using db_test.SPEEDDBTableAdapters;

namespace db_test
{

    public partial class 商品変換マスタ取込Form : Form
    {
        private SqlDb sqlDb;
        private DataSet dataSet;
        private SPEEDDB speedDB;
        private DataTable dataTable;
        private ExcelCsvClass excelCsvClass;
        private 商品変換取込変換商品登録済TableAdapter 商品変換取込変換商品登録済tableAdapter;
        private 商品変換取込変換商品未登録TableAdapter 商品変換取込変換商品未登録tableAdapter;
        private 商品変換取込商品未登録TableAdapter 商品変換取込商品未登録tableAdapter;

        int rowcnt = 0;
        int cancelFlag = 0;
        int idoFlag = 0;
        int henkanFlag = 0;
        string beforeCellName = null;
        string nowCellName = null;
        string afterCellName = null;

        int 処理区分index = 0;

        int 取込区分index = 0;
        int JANCHKindex = 0;
        int 取込ファイル名index = 0;
        int 取込ファイルオープンindex = 0;

        int 得意先コードindex = 0;
        int 得意先名index = 0;
        int 先方商品コード始index = 0;
        int 先方商品コード終index = 0;
        int 商品コード始index = 0;
        int 商品コード終index = 0;
        int 出力ファイルパスindex = 0;
        int 出力ファイルオープンindex = 0;

        int 実行index = 0;
        int 終了index = 0;

        string form名 = null;

        public 商品変換マスタ取込Form()
        {
            InitializeComponent();
        }

        private void 商品変換マスタ取込Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 商品変換マスタ取込Template();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.ScrollBars = ScrollBars.None;

            form名 = Form.ActiveForm.Name;
            
            //セル色の設定
            //非選択状態の色
            //gcMultiRow1.Rows[0].Cells[0].Style.BackColor = Color.Blue;
            //gcMultiRow1.Rows[0].Cells[0].Style.ForeColor = Color.White;

            //選択状態のときの色
            gcMultiRow1.DefaultCellStyle.SelectionBackColor = Color.White;
            gcMultiRow1.DefaultCellStyle.SelectionForeColor = Color.Black;

            //編集状態のときの色
            gcMultiRow1.DefaultCellStyle.EditingBackColor = Color.Yellow;
            gcMultiRow1.DefaultCellStyle.EditingForeColor = Color.Red;

            //無効のときの色
            gcMultiRow1.DefaultCellStyle.DisabledBackColor = Color.White;
            gcMultiRow1.DefaultCellStyle.DisabledForeColor = Color.Yellow;

            // イベント
            //gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            gcMultiRow1.CellEditedFormattedValueChanged += new EventHandler<CellEditedFormattedValueChangedEventArgs>(gcMultiRow1_CellEditedFormattedValueChanged);
            //gcMultiRow1.SelectionChanged += new EventHandler(gcMultiRow1_SelectionChanged);
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            //gcMultiRow1.CellBeginEdit += new EventHandler<CellBeginEditEventArgs>(gcMultiRow1_CellBeginEdit);
            //gcMultiRow1.CellEnter += new EventHandler<CellEventArgs>(gcMultiRow1_CellEnter);
            //gcMultiRow1.CellLeave += new EventHandler<CellEventArgs>(gcMultiRow1_CellLeave);
            //gcMultiRow1.DataError += new EventHandler<DataErrorEventArgs>(gcMultiRow1_DataError);
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);

            // GcMultiRowコントロールがフォーカスを失ったとき
            // セルの選択状態を非表示にする
            gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;
            //gcMultiRow1.EditMode = EditMode.EditOnShortcutKey;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);

            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            //gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Tab);

            //this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            //this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);

            //this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            //this.gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);

            // 既定のショートカットキーを削除する
            //this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F2);
            //this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);
            //this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F9);
            //this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F10);

            // 各ファンクションキーに対応するアクションを登録する
            //this.gcMultiRow1.ShortcutKeyManager.Register(new 受注明細参照FunctionKeyAction(Keys.F3), Keys.F3);
            //this.gcMultiRow1.ShortcutKeyManager.Register(new 受注明細参照FunctionKeyAction(Keys.F9), Keys.F9);
            //this.gcMultiRow1.ShortcutKeyManager.Register(new 受注明細参照FunctionKeyAction(Keys.F10), Keys.F10);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            //gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            //gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Default;

            // インデックスの保存
            処理区分index = gcMultiRow1.ColumnHeaders[0].Cells["処理区分radioGroupCell"].CellIndex;
            
            取込区分index = gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].CellIndex;
            JANCHKindex = gcMultiRow1.ColumnHeaders[0].Cells["JANCHKcheckBoxCell"].CellIndex;
            取込ファイル名index = gcMultiRow1.ColumnHeaders[0].Cells["取込ファイル名textBoxCell"].CellIndex;
            取込ファイルオープンindex = gcMultiRow1.ColumnHeaders[0].Cells["取込ファイルオープンbuttonCell"].CellIndex;

            得意先コードindex = gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].CellIndex;
            得意先名index = gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].CellIndex;
            先方商品コード始index = gcMultiRow1.ColumnHeaders[0].Cells["先方商品コード始textBoxCell"].CellIndex;
            先方商品コード終index = gcMultiRow1.ColumnHeaders[0].Cells["先方商品コード終textBoxCell"].CellIndex;
            商品コード始index = gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].CellIndex;
            商品コード終index = gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].CellIndex;
            出力ファイルパスindex = gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルパスtextBoxCell"].CellIndex;
            出力ファイルオープンindex = gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルオープンbuttonCell"].CellIndex;

            実行index = gcMultiRow1.ColumnHeaders[0].Cells["実行buttonCell"].CellIndex;
            終了index = gcMultiRow1.ColumnHeaders[0].Cells["終了buttonCell"].CellIndex;

            // 初期表示 
            gcMultiRow1.ColumnHeaders[0].Cells["処理区分radioGroupCell"].Value = 0;

            gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].Value = 0;

            gcMultiRow1.ColumnHeaders[0].Cells["OP1checkBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["OP2checkBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["OP3checkBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["OP4checkBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["OP5checkBoxCell"].Value = 0;

            gcMultiRow1.ColumnHeaders[0].Cells["CHK1checkBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["CHK2checkBoxCell"].Value = 0;

            gcMultiRow1.ColumnHeaders[0].Cells["強制実行buttonCell"].Visible = false;

            chgKubun();
            chgKyosei(0);

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "処理区分radioGroupCell");

        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "取込区分textBoxCell":
                    if ((string)gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].EditedFormattedValue  == "0" || (string)gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].EditedFormattedValue  == "1")
                    {
                        e.Cancel = false;
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
                case "取込区分textBoxCell":
                    break;
            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            
            if (gcMultiRow1.CurrentCell.CellIndex == 処理区分index)
            {
                if ((int)gcMultiRow1.ColumnHeaders[0].Cells[処理区分index].EditedFormattedValue == 0)
                {
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 得意先コードindex);
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 終了index);
                    }
                }
                else
                {
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 取込区分index);
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 終了index);
                    }
                }
            }
            else if (gcMultiRow1.CurrentCell.CellIndex == 取込区分index)
            {
                e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, 取込ファイル名index);
            }
            else if (gcMultiRow1.CurrentCell.CellIndex == JANCHKindex)
            {
                if (e.MoveStatus == MoveStatus.MoveDown)
                {
                    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 取込ファイル名index);
                }
                else if (e.MoveStatus == MoveStatus.MoveUp)
                {
                    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 取込区分index);
                }
            }
            else if (gcMultiRow1.CurrentCell.CellIndex == 取込ファイル名index)
            {
                if (e.MoveStatus == MoveStatus.MoveDown)
                {
                    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 実行index);
                }
                else if (e.MoveStatus == MoveStatus.MoveUp)
                {
                    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 取込区分index);
                }
            }
            else if (gcMultiRow1.CurrentCell.CellIndex == 取込ファイルオープンindex)
            {
                if (e.MoveStatus == MoveStatus.MoveDown)
                {
                    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 実行index);
                }
                else if (e.MoveStatus == MoveStatus.MoveUp)
                {
                    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 取込区分index);
                }
            }
            else if (gcMultiRow1.CurrentCell.CellIndex == 実行index)
            {
                if ((int)gcMultiRow1.ColumnHeaders[0].Cells[処理区分index].EditedFormattedValue == 0)
                {
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 処理区分index);
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 出力ファイルパスindex);
                    }
                }
                else
                {
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 処理区分index);
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 取込ファイル名index);
                    }
                }
            }
            else if (gcMultiRow1.CurrentCell.CellIndex == 終了index)
            {
                if (e.MoveStatus == MoveStatus.MoveDown)
                {
                    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 処理区分index);
                }
                else if (e.MoveStatus == MoveStatus.MoveUp)
                {
                    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 実行index);
                }
            }
            else if (gcMultiRow1.CurrentCell.CellIndex == 取込ファイルオープンindex)
            {
                if (e.MoveStatus == MoveStatus.MoveDown)
                {
                    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 実行index);
                }
                else if (e.MoveStatus == MoveStatus.MoveUp)
                {
                    //if ((int)gcMultiRow1.ColumnHeaders[0].Cells[処理区分index].EditedFormattedValue == 0)
                    //{
                        e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 取込ファイル名index);
                    //}
                    //else
                    //{
                    //    e.NewCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, 取込ファイル名index);
                    //}
                }
            }
        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "実行buttonCell":
                    int ret = execProcedure();
                    break;
                case "強制実行buttonCell":
                    if (Create_table(1) == 0)
                    {
                        MessageBox.Show("正常に取込されました。", "");
                    }
                    break;
                case "終了buttonCell":
                    this.Close();
                    break;
                    
            }

        
        }

        void gcMultiRow1_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            switch (e.CellName)
            {
                case "処理区分radioGroupCell":
                    chgKubun();
                    chgKyosei(0);
                    break;
                case "取込区分textBoxCell":
                    chgKubun();
                    chgKyosei(0);
                    break;

            }
        
        }

        private void chgKubun()
        {
            if ((int)gcMultiRow1.ColumnHeaders[0].Cells["処理区分radioGroupCell"].EditedFormattedValue == 0)
            {
                gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["JANCHKcheckBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["取込ファイル名textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["取込ファイルオープンbuttonCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell6"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell8"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell7"].Visible = false;

                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["先方商品コード始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["先方商品コード終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルパスtextBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルオープンbuttonCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell3"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell17"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell1"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell18"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell4"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell5"].Visible = true;

                gcMultiRow1.ColumnHeaders[0].Cells["OP1checkBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["OP2checkBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["OP3checkBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["OP4checkBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["OP5checkBoxCell"].Visible = false;
            
            }
            else
            {
                gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["取込ファイル名textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["取込ファイルオープンbuttonCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell6"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell8"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell7"].Visible = true;

                gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["得意先名textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["先方商品コード始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["先方商品コード終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["商品コード始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["商品コード終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルパスtextBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["出力ファイルオープンbuttonCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell3"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell17"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell1"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell18"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell4"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["labelCell5"].Visible = false;

                if (gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].EditedFormattedValue != null)
                {
                    if (gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].EditedFormattedValue.ToString() == "0")
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["JANCHKcheckBoxCell"].Visible = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP1checkBoxCell"].Visible = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP2checkBoxCell"].Visible = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP3checkBoxCell"].Visible = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP4checkBoxCell"].Visible = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP5checkBoxCell"].Visible = false;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["JANCHKcheckBoxCell"].Visible = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP1checkBoxCell"].Visible = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP2checkBoxCell"].Visible = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP3checkBoxCell"].Visible = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP4checkBoxCell"].Visible = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["OP5checkBoxCell"].Visible = true;
                    }
                }
                
            }
        }

        private void chgKyosei(int intFlg)
        {
            if (intFlg == 0)
            {
                gcMultiRow1.ColumnHeaders[0].Cells["CHK1checkBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["CHK1checkBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["強制実行buttonCell"].Visible = false;

            }
            else
            {
                gcMultiRow1.ColumnHeaders[0].Cells["CHK1checkBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["CHK1checkBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["強制実行buttonCell"].Visible = true;

            }
        }

        private int execProcedure()
        {
            int ret = 0;

            // 0:追加・1:上書き以外エラー
            //if (gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].Value != "0" && gcMultiRow1.ColumnHeaders[0].Cells["取込区分textBoxCell"].Value.ToString() != "1")
            //{
            //    MessageBox.Show("取込区分エラー",form名);
            //    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込区分radioGroupCell");
            //    return 1;
            //}

            if ((int)gcMultiRow1.ColumnHeaders[0].Cells["処理区分radioGroupCell"].Value == 0 && gcMultiRow1.ColumnHeaders[0].Cells["得意先コードtextBoxCell"].Value == null)
            {
                MessageBox.Show("得意先コード未入力です！", form名);
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "得意先コードtextBoxCell");
                return 1;
            }
            /*
            if (Utility.getHeadIntValue(gcMultiRow1, "先方商品コード始textBoxCell") > 0 & Utility.getHeadIntValue(gcMultiRow1, "先方商品コード終textBoxCell") >0)
            {
                if (Utility.getHeadIntValue(gcMultiRow1, "先方商品コード始textBoxCell") > Utility.getHeadIntValue(gcMultiRow1, "先方商品コード終textBoxCell"))
                {
                    MessageBox.Show("先方商品コードＳＴ＞先方商品コードＥＤです！", form名);
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "先方商品コード始textBoxCell");
                    return 1;
                }
            }

            if (Utility.getHeadIntValue(gcMultiRow1, "商品コード始textBoxCell") > 0 & Utility.getHeadIntValue(gcMultiRow1, "商品コード終textBoxCell") > 0)
            {
                if (Utility.getHeadIntValue(gcMultiRow1, "商品コード始textBoxCell") > Utility.getHeadIntValue(gcMultiRow1, "商品コード終textBoxCell"))
                {
                    MessageBox.Show("商品コードＳＴ＞商品コードＥＤです！", form名);
                    gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "商品コード始textBoxCell");
                    return 1;
                }
            }
            */
            if (Utility.getHeadIntValue(gcMultiRow1, "処理区分radioGroupCell") == 1 & Utility.getHeadStrValue(gcMultiRow1, "取込ファイル名textBoxCell") == null)
            {
                MessageBox.Show("取込ファイル名未入力です！", form名);
                gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "取込ファイル名textBoxCell");
                return 1;
            }
            // 出力の時
            if ((int)gcMultiRow1.ColumnHeaders[0].Cells["処理区分radioGroupCell"].Value == 0)
            {
                //if (MessageBox.Show("商品変換マスタを出力します。よろしいですか？", form名))
                //    return 1;

                if (Create_file() == 0)
                {
                    MessageBox.Show("正常に作成されました。", form名);
                    return 0;
                }
                else
                {
                    MessageBox.Show("作成できませんでした。", form名);
                    return 1;
                }
            }
            else
            {
                // 取込の時
                //if (Dir(S_Set(this.txtFileName1), Constants.vbDirectory) == "")
                //{
                //    MsgBox("指定されたファイルが存在しません" + Strings.Chr(9), Constants.vbOKOnly, PRONM);
                //    return;
                //}

                //if (MsgBox("商品変換マスタに取込します。よろしいですか？", 65, PRONM) == 2)
                //    return;

                if (Create_table(0) == 0)
                    MessageBox.Show("正常に取込されました。", "");
                else
                {
                    return 1;
                }
            }

            return 0;

        }

        private int Create_file()
        {
            // *****************************************************************************
            // 商品変換マスタ出力ワークテーブル作成
            // *****************************************************************************

            string W_SQL;
            // 
            string fileName;

            // ファイル存在チェック 
            fileName = Utility.S_Set(Utility.getHeadStrValue(gcMultiRow1, "出力ファイルパスtextBoxCell"));

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            sqlDb = new SqlDb();
            sqlDb.Connect();
            W_SQL = "DELETE FROM  W商品変換マスタ出力";
            sqlDb.BeginTransaction();
            sqlDb.ExecuteSql(W_SQL, -1);
            sqlDb.CommitTransaction();
            sqlDb.Disconnect();

            // ##### 抽出 #####
            W_SQL = "INSERT INTO W商品変換マスタ出力 ";
            W_SQL = W_SQL + "SELECT A.得意先コード,A.先方商品コード,A.商品コード,B.商品名,A.ＪＡＮコード,";
            W_SQL = W_SQL + "A.店舗売価,A.納入単価,A.本部原価 ";
            W_SQL = W_SQL + "FROM T商品変換マスタ AS A ";
            W_SQL = W_SQL + "LEFT JOIN T商品マスタ AS B ON A.商品コード = B.商品コード ";

            if (Utility.Z_Set(Utility.getHeadStrValue(gcMultiRow1, "先方商品コード始textBoxCell")) == 0)
                Utility.setHeadStrValue(gcMultiRow1, "先方商品コード始textBoxCell", "0000000000000");

            if (Utility.Z_Set(Utility.getHeadStrValue(gcMultiRow1, "先方商品コード終textBoxCell")) == 0)
                Utility.setHeadStrValue(gcMultiRow1, "先方商品コード終textBoxCell", "9999999999999");

            if (Utility.Z_Set(Utility.getHeadStrValue(gcMultiRow1, "商品コード始textBoxCell")) == 0)
                Utility.setHeadStrValue(gcMultiRow1, "商品コード始textBoxCell", "00000000000");

            if (Utility.Z_Set(Utility.getHeadStrValue(gcMultiRow1, "商品コード終textBoxCell")) == 0)
                Utility.setHeadStrValue(gcMultiRow1, "商品コード終textBoxCell", "99999999999");

            W_SQL = W_SQL + " WHERE A.得意先コード  = '" + Utility.S_Set(Utility.getHeadStrValue(gcMultiRow1, "得意先コードtextBoxCell")) + "'";
            W_SQL = W_SQL + " AND A.先方商品コード  >= '" + Utility.S_Set(Utility.getHeadStrValue(gcMultiRow1, "先方商品コード始textBoxCell")) + "'";
            W_SQL = W_SQL + " AND A.先方商品コード  <= '" + Utility.S_Set(Utility.getHeadStrValue(gcMultiRow1, "先方商品コード終textBoxCell")) + "'";
            W_SQL = W_SQL + " AND A.商品コード  >= '" + Utility.S_Set(Utility.getHeadStrValue(gcMultiRow1, "商品コード始textBoxCell")) + "'";
            W_SQL = W_SQL + " AND A.商品コード  <= '" + Utility.S_Set(Utility.getHeadStrValue(gcMultiRow1, "商品コード終textBoxCell")) + "'";
            W_SQL = W_SQL + " ORDER BY A.得意先コード,A.先方商品コード;";

            sqlDb.Connect();
            sqlDb.BeginTransaction();
            sqlDb.ExecuteSql(W_SQL, -1);
            sqlDb.CommitTransaction();
            sqlDb.Disconnect();
        
            //DataTable tb;
            sqlDb.Connect();
            dataTable = sqlDb.ExecuteSql("select * from W商品変換マスタ出力", -1);

            if (dataTable.Rows.Count == 0)
            {
                MessageBox.Show("該当データがありません。","");
                return 1;
            }
            if (dataTable.Rows.Count >= 65000)
            {
                MessageBox.Show("許容件数を超えています！！","");
                return 1;
            }

            excelCsvClass = new ExcelCsvClass();

            excelCsvClass.excel_Export(fileName, dataTable);

            return 0;

        }

        public int Create_table(int TorikomiKBN) // TorikomiKBN 0:通常取込、1:自動作成
        {

            // *****************************************************************************
            // 商品変換マスタ取込テーブル作成
            // *****************************************************************************

            string W_SQL;
            DataTable tb;
            DataSet ds;
            string strEnMark;
            string MSG;
            string S_dmy;
            string d_date;

            int upkbn;

            sqlDb = new SqlDb();
            sqlDb.Connect();

            if (TorikomiKBN == 0)
            {

                //db.BeginTransaction();
                // ①-1ワーク削除（W_商品マスタ取込）
                W_SQL = "DELETE FROM  W商品変換マスタ取込";
                sqlDb.ExecuteSql(W_SQL, -1);

                W_SQL = "DELETE FROM  W商品変換取込商品未登録";
                sqlDb.ExecuteSql(W_SQL, -1);

                W_SQL = "DELETE FROM  W商品変換取込変換商品登録済";
                sqlDb.ExecuteSql(W_SQL, -1);

                W_SQL = "DELETE FROM  W商品変換取込変換商品未登録";
                sqlDb.ExecuteSql(W_SQL, -1);

                //db.CommitTransaction();
                //db.Disconnect();

                // ②データ取込
                dataTable = sqlDb.ExecuteSql("select * from W商品変換マスタ取込", -1);

                excelCsvClass = new ExcelCsvClass();
                
                excelCsvClass.excel_Import(Utility.getHeadStrValue(gcMultiRow1,"取込ファイル名textBoxCell"),dataTable);

            }

            if (Utility.getHeadStrValue(gcMultiRow1, "取込区分textBoxCell") == "0" & TorikomiKBN == 0)
            {

                //db.BeginTransaction();
                /*
                // ③商品マスタが存在しない分
                W_SQL = "INSERT INTO W商品変換取込商品未登録 ";
                W_SQL = W_SQL + "SELECT W商品変換マスタ取込.* ";
                W_SQL = W_SQL + "FROM T商品マスタ ";
                W_SQL = W_SQL + "RIGHT JOIN W商品変換マスタ取込 ";
                if ((bool)gcMultiRow1.ColumnHeaders[0].Cells["JANCHKcheckBoxCell"].FormattedValue == false)
                    W_SQL = W_SQL + "ON T商品マスタ.商品コード = W商品変換マスタ取込.商品コード ";
                else
                    W_SQL = W_SQL + "ON T商品マスタ.バーコード = W商品変換マスタ取込.ＪＡＮコード ";

                W_SQL = W_SQL + "WHERE (((T商品マスタ.商品コード) Is Null)) ";

                sqlDb.ExecuteSql(W_SQL, -1);

                商品変換取込商品未登録tableAdapter = new 商品変換取込商品未登録TableAdapter();

                dataTable = 商品変換取込商品未登録tableAdapter;

                if (dataTable.Rows.Count > 0)
                {
                    MessageBox.Show("商品マスタに存在しないデータが存在します。","");

                    ds = new DataSet();
                    ds.Tables.Add(dataTable); 
                    プレビューForm プレビューform = new プレビューForm();
                    プレビューform.dataTable = dataTable;
                    プレビューform.rptName = "商品変換取込商品未登録CrystalReport";
                    プレビューform.Show();

                    return 1;
                }
                */

                // ③得意先マスタが存在しない分
                W_SQL = "SELECT W商品変換マスタ取込.* ";
                W_SQL = W_SQL + "FROM T得意先マスタ ";
                W_SQL = W_SQL + "RIGHT JOIN W商品変換マスタ取込 ";
                W_SQL = W_SQL + "ON T得意先マスタ.得意先コード = W商品変換マスタ取込.得意先コード ";
                W_SQL = W_SQL + "WHERE (((T得意先マスタ.得意先コード) Is Null)) ";

                dataTable = sqlDb.ExecuteSql(W_SQL, -1);

                if (dataTable.Rows.Count > 0)
                {
                    MessageBox.Show("得意先マスタに存在しないデータが存在します。","");

                    return 1;
                }

                // ④商品変換マスタが登録済み（ＫＥＹ＝先方商品コード）
                W_SQL = "INSERT INTO W商品変換取込変換商品登録済 ";
                W_SQL = W_SQL + "SELECT 1 AS 区分, W商品変換マスタ取込.* ";
                W_SQL = W_SQL + "FROM T商品変換マスタ ";
                W_SQL = W_SQL + "INNER JOIN W商品変換マスタ取込 ";
                W_SQL = W_SQL + "ON (T商品変換マスタ.得意先コード = W商品変換マスタ取込.得意先コード) ";
                W_SQL = W_SQL + "AND (T商品変換マスタ.先方商品コード = W商品変換マスタ取込.先方商品コード) ";

                sqlDb.ExecuteSql(W_SQL, -1);

                // ⑤商品変換マスタが登録済み（ＫＥＹ＝商品コード）
                W_SQL = "INSERT INTO W商品変換取込変換商品登録済 ";
                W_SQL = W_SQL + "SELECT 2 AS 区分, W商品変換マスタ取込.* ";
                W_SQL = W_SQL + "FROM T商品変換マスタ ";
                W_SQL = W_SQL + "INNER JOIN W商品変換マスタ取込 ";
                W_SQL = W_SQL + "ON (T商品変換マスタ.得意先コード = W商品変換マスタ取込.得意先コード) ";
                W_SQL = W_SQL + "AND (T商品変換マスタ.商品コード = W商品変換マスタ取込.商品コード) ";

                sqlDb.ExecuteSql(W_SQL, -1);

                商品変換取込変換商品登録済tableAdapter = new 商品変換取込変換商品登録済TableAdapter();

                dataTable = 商品変換取込変換商品登録済tableAdapter.GetData();
                
                if (dataTable.Rows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("商品変換マスタに登録済データが存在します。強制登録する場合、リストを閉じた後、強制上書ボタンを押して下さい。",
                        "質問",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        プレビューForm プレビューform = new プレビューForm();
                        プレビューform.dataTable = dataTable;
                        プレビューform.rptName = "商品変換取込変換商品登録済CrystalReport";
                        プレビューform.Show();

                        gcMultiRow1.ColumnHeaders[0].Cells["強制実行buttonCell"].Visible = true;
                        //chk1.Visible = true;
                        //CHK2.Visible = true;
                        //LBL1.Visible = true;
                        //LBL2.Visible = true;

                        return 0;
                    }
                    else if (result == DialogResult.No)
                    {
                        return 0;
                    }
                }

                // ⑦商品変換ﾏｽﾀ追加
                //if (Utility.getHeadStrValue(gcMultiRow1,"JANCHKcheckBoxCell") == "0")
                if (gcMultiRow1.ColumnHeaders[0].Cells["JANCHKcheckBoxCell"].FormattedValue.ToString() == "False" )
                {
                    W_SQL = "INSERT INTO T商品変換マスタ (得意先コード, 先方商品コード, 商品コード, ＪＡＮコード, 店舗売価, 納入単価, 本部原価) ";
                    W_SQL = W_SQL + "SELECT 得意先コード,先方商品コード,商品コード,ＪＡＮコード,店舗売価,納入単価,本部原価 FROM W商品変換マスタ取込 ";
                }
                else
                {
                    W_SQL = "INSERT INTO T商品変換マスタ (得意先コード, 先方商品コード, 商品コード, ＪＡＮコード, 店舗売価, 納入単価, 本部原価) ";
                    W_SQL = W_SQL + "SELECT W商品変換マスタ取込.得意先コード,W商品変換マスタ取込.先方商品コード,T商品マスタ.商品コード, ";
                    W_SQL = W_SQL + "W商品変換マスタ取込.ＪＡＮコード,W商品変換マスタ取込.店舗売価,W商品変換マスタ取込.納入単価,W商品変換マスタ取込.本部原価 ";
                    W_SQL = W_SQL + "FROM W商品変換マスタ取込 ";
                    W_SQL = W_SQL + "INNER JOIN T商品マスタ ";
                    W_SQL = W_SQL + "ON W商品変換マスタ取込.ＪＡＮコード = T商品マスタ.バーコード ";
                }

                sqlDb.ExecuteSql(W_SQL, -1);

            }
            else
            {
                if (Utility.getHeadStrValue(gcMultiRow1,"取込区分textBoxCell") == "1")
                {
                    /*
                    // ③商品マスタが存在しない分
                    W_SQL = "INSERT INTO W商品変換取込商品未登録 ";
                    W_SQL = W_SQL + "SELECT W商品変換マスタ取込.* ";
                    W_SQL = W_SQL + "FROM T商品マスタ ";
                    W_SQL = W_SQL + "RIGHT JOIN W商品変換マスタ取込 ";
                    W_SQL = W_SQL + "ON T商品マスタ.商品コード = W商品変換マスタ取込.商品コード ";
                    W_SQL = W_SQL + "WHERE (((T商品マスタ.商品コード) Is Null)) ";

                    sqlDb.ExecuteSql(W_SQL, -1);

                    商品変換取込商品未登録tableAdapter = new 商品変換取込商品未登録TableAdapter();
                    dataTable = 商品変換取込商品未登録tableAdapter.GetData();

                    if (dataTable.Rows.Count > 0)
                    {

                        MessageBox.Show("商品マスタに存在しないデータが存在します。","");

                        ds = new DataSet();
                        ds.Tables.Add(dataTable); 
                        プレビューForm プレビューform = new プレビューForm();
                        プレビューform.dataTable = dataTable;
                        プレビューform.rptName = "商品変換取込商品未登録CrystalReport";
                        プレビューform.Show();

                        return 1;
                    }
                    // ④商品変換マスタが未登録（ＫＥＹ＝先方商品コード）
                    W_SQL = "INSERT INTO W商品変換取込変換商品未登録 ";
                    W_SQL = W_SQL + "SELECT W商品変換マスタ取込.* ";
                    W_SQL = W_SQL + "FROM T商品変換マスタ ";
                    W_SQL = W_SQL + "RIGHT JOIN  W商品変換マスタ取込 ";
                    W_SQL = W_SQL + "ON (T商品変換マスタ.得意先コード = W商品変換マスタ取込.得意先コード) ";
                    W_SQL = W_SQL + "AND (T商品変換マスタ.先方商品コード = W商品変換マスタ取込.先方商品コード) ";
                    W_SQL = W_SQL + "WHERE (((T商品変換マスタ.先方商品コード) Is Null)) ";

                    sqlDb.ExecuteSql(W_SQL, -1);

                    商品変換取込変換商品未登録tableAdapter = new 商品変換取込変換商品未登録TableAdapter();
                    dataTable = 商品変換取込変換商品未登録tableAdapter.GetData();

                    if (dataTable.Rows.Count > 0)
                    {
                        MessageBox.Show("商品変換マスタに存在しないデータが存在します。","");

                        ds = new DataSet();
                        ds.Tables.Add(dataTable); 
                        プレビューForm プレビューform = new プレビューForm();
                        プレビューform.dataTable = dataTable;
                        プレビューform.rptName = "商品変換取込変換商品未登録CrystalReport";
                        プレビューform.Show();

                        return 1;
                    }
                    */
                }
                else
                {
                    // 未登録分を追加する
                    W_SQL = "INSERT INTO T商品変換マスタ (得意先コード, 先方商品コード, 商品コード, ＪＡＮコード, 店舗売価, 納入単価, 本部原価) ";
                    W_SQL = W_SQL + "SELECT W商品変換マスタ取込.得意先コード,W商品変換マスタ取込.先方商品コード,W商品変換マスタ取込.商品コード, ";
                    W_SQL = W_SQL + "W商品変換マスタ取込.ＪＡＮコード,W商品変換マスタ取込.店舗売価,W商品変換マスタ取込.納入単価,W商品変換マスタ取込.本部原価 ";
                    W_SQL = W_SQL + "FROM T商品変換マスタ ";
                    W_SQL = W_SQL + "RIGHT JOIN W商品変換マスタ取込 ";
                    W_SQL = W_SQL + "ON (T商品変換マスタ.得意先コード = W商品変換マスタ取込.得意先コード) ";
                    W_SQL = W_SQL + "AND (T商品変換マスタ.先方商品コード = W商品変換マスタ取込.先方商品コード ";
                    W_SQL = W_SQL + "OR  T商品変換マスタ.商品コード = W商品変換マスタ取込.商品コード) ";
                    W_SQL = W_SQL + "WHERE (((T商品変換マスタ.先方商品コード) Is Null)) ";

                    sqlDb.ExecuteSql(W_SQL, -1);
                }


                if (Utility.getHeadStrValue(gcMultiRow1,"取込区分textBoxCell") == "1")
                {
                    dataTable = sqlDb.ExecuteSql("select * from W商品変換マスタ取込", -1);
                }
                else
                {

                    dataTable = sqlDb.ExecuteSql("select * from W商品変換取込変換商品登録済", -1);

                    Utility.setHeadIntValue(gcMultiRow1, "OP1checkBoxCell",1);
                    Utility.setHeadIntValue(gcMultiRow1, "OP2checkBoxCell", 1);
                    Utility.setHeadIntValue(gcMultiRow1, "OP3checkBoxCell", 1);
                    Utility.setHeadIntValue(gcMultiRow1, "OP4checkBoxCell", 1);
                    Utility.setHeadIntValue(gcMultiRow1, "OP5checkBoxCell", 1);

                    for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
                    {
                        upkbn = 0;

                        // 更新する時は、
                        // ①通常上書きモードか
                        if (Utility.getHeadStrValue(gcMultiRow1, "取込区分textBoxCell") == "1")
                        {
                            W_SQL = "UPDATE T商品変換マスタ SET";
                            if (Utility.getHeadIntValue(gcMultiRow1, "OP1checkBoxCell") == 1)
                            {
                                upkbn = 1;
                                if (Utility.getHeadIntValue(gcMultiRow1, "取込区分textBoxCell") == 1)
                                {
                                    W_SQL = W_SQL + " 商品コード ='" + Utility.S_Set(dataTable.Rows[i]["商品コード"].ToString()) + "'";
                                }
                                else
                                {
                                    W_SQL = W_SQL + " 先方商品コード ='" + Utility.S_Set(dataTable.Rows[i]["先方商品コード"].ToString()) + "'";

                                }
                            }
                            if (Utility.getHeadIntValue(gcMultiRow1, "OP1checkBoxCell") == 0)
                            {
                            }
                            else if (Utility.getHeadIntValue(gcMultiRow1, "OP2checkBoxCell") == 1 & upkbn == 0)
                            {
                                W_SQL = W_SQL + " ＪＡＮコード ='" + Utility.S_Set(dataTable.Rows[i]["JANコード"].ToString()) + "'";
                                upkbn = 1;
                            }
                            else
                            {
                                W_SQL = W_SQL + " ,ＪＡＮコード ='" + Utility.S_Set(dataTable.Rows[i]["JANコード"].ToString()) + "'";
                            }
                            if (Utility.getHeadIntValue(gcMultiRow1, "OP3checkBoxCell") == 0)
                            {
                            }
                            else if (Utility.getHeadIntValue(gcMultiRow1, "OP3checkBoxCell") == 1 & upkbn == 0)
                            {
                                W_SQL = W_SQL + " 店舗売価 ='" + Utility.S_Set(dataTable.Rows[i]["店舗売価"].ToString()) + "'";
                                upkbn = 1;
                            }
                            else
                            {
                                W_SQL = W_SQL + " ,店舗売価 ='" + Utility.S_Set(dataTable.Rows[i]["店舗売価"].ToString()) + "'";
                            }
                            if (Utility.getHeadIntValue(gcMultiRow1, "OP4checkBoxCell") == 0)
                            {
                            }
                            else if (Utility.getHeadIntValue(gcMultiRow1, "OP4checkBoxCell") == 1 & upkbn == 0)
                            {
                                W_SQL = W_SQL + " 納入単価 ='" + Utility.S_Set(dataTable.Rows[i]["納入単価"].ToString()) + "'";
                                upkbn = 1;
                            }
                            else
                            {
                                W_SQL = W_SQL + " ,納入単価 ='" + Utility.S_Set(dataTable.Rows[i]["納入単価"].ToString()) + "'";
                            }
                            if (Utility.getHeadIntValue(gcMultiRow1, "OP5checkBoxCell") == 0)
                            {
                            }
                            else if (Utility.getHeadIntValue(gcMultiRow1, "OP5checkBoxCell") == 1 & upkbn == 0)
                            {
                                W_SQL = W_SQL + " 本部原価 ='" + Utility.S_Set(dataTable.Rows[i]["本部原価"].ToString()) + "'";
                                upkbn = 1;
                            }
                            else
                            {
                                W_SQL = W_SQL + " ,本部原価 ='" + Utility.S_Set(dataTable.Rows[i]["本部原価"].ToString()) + "'";
                            }
                            W_SQL = W_SQL + " WHERE 得意先コード = '" + Utility.S_Set(dataTable.Rows[i]["得意先コード"].ToString()) + "'";
                            if (Utility.getHeadIntValue(gcMultiRow1, "取込区分textBoxCell") == 1)
                            {
                                W_SQL = W_SQL + " AND 先方商品コード = '" + Utility.S_Set(dataTable.Rows[i]["先方商品コード"].ToString()) + "'";
                            }
                            else
                            {
                                W_SQL = W_SQL + " AND 商品コード = '" + Utility.S_Set(dataTable.Rows[i]["商品コード"].ToString()) + "'";
                            }
                            sqlDb.ExecuteSql(W_SQL, -1);
                        }
                        else
                        {

                            // ②強制の場合は先方商品エラー分のみ商品エラーのみかすべて
                            if ((Utility.getHeadStrValue(gcMultiRow1, "取込区分textBoxCell") == "0" & Utility.S_Set(dataTable.Rows[i]["区分"].ToString()) == "1" & Utility.getHeadStrValue(gcMultiRow1, "取込区分textBoxCell") == "1")
                                | (Utility.getHeadStrValue(gcMultiRow1, "取込区分textBoxCell") == "0" & Utility.S_Set(dataTable.Rows[i]["区分"].ToString()) == "2" & Utility.getHeadStrValue(gcMultiRow1, "取込区分textBoxCell") == "1"))
                            {
                                W_SQL = "UPDATE T商品変換マスタ SET";
                                if (Utility.getHeadIntValue(gcMultiRow1, "OP1checkBoxCell") == 1)
                                {
                                    upkbn = 1;
                                    if ((Utility.getHeadStrValue(gcMultiRow1, "取込区分textBoxCell") == "0" & Utility.S_Set(dataTable.Rows[i]["区分"].ToString()) == "1"))
                                        W_SQL = W_SQL + " 商品コード ='" + Utility.S_Set(dataTable.Rows[i]["商品コード"].ToString()) + "'";
                                    else
                                        W_SQL = W_SQL + " 先方商品コード ='" + Utility.S_Set(dataTable.Rows[i]["先方商品コード"].ToString()) + "'";
                                }

                                if (Utility.getHeadIntValue(gcMultiRow1, "OP2checkBoxCell") == 0)
                                {
                                }
                                else if (Utility.getHeadIntValue(gcMultiRow1, "OP2checkBoxCell") == 1 & upkbn == 0)
                                {
                                    W_SQL = W_SQL + " ＪＡＮコード ='" + Utility.S_Set(dataTable.Rows[i]["JANコード"].ToString()) + "'";
                                    upkbn = 1;
                                }
                                else
                                {
                                    W_SQL = W_SQL + " ,ＪＡＮコード ='" + Utility.S_Set(dataTable.Rows[i]["JANコード"].ToString()) + "'";
                                }

                                if (Utility.getHeadIntValue(gcMultiRow1, "OP3checkBoxCell") == 0)
                                {
                                }
                                else if (Utility.getHeadIntValue(gcMultiRow1, "OP3checkBoxCell") == 1 & upkbn == 0)
                                {
                                    W_SQL = W_SQL + " 店舗売価 ='" + Utility.S_Set(dataTable.Rows[i]["店舗売価"].ToString()) + "'";
                                    upkbn = 1;
                                }
                                else
                                {
                                    W_SQL = W_SQL + " ,店舗売価 ='" + Utility.S_Set(dataTable.Rows[i]["店舗売価"].ToString()) + "'";
                                }

                                if (Utility.getHeadIntValue(gcMultiRow1, "OP4checkBoxCell") == 0)
                                {
                                }
                                else if (Utility.getHeadIntValue(gcMultiRow1, "OP4checkBoxCell") == 1 & upkbn == 0)
                                {
                                    W_SQL = W_SQL + " 納入単価 ='" + Utility.S_Set(dataTable.Rows[i]["納入単価"].ToString()) + "'";
                                    upkbn = 1;
                                }
                                else
                                {
                                    W_SQL = W_SQL + " ,納入単価 ='" + Utility.S_Set(dataTable.Rows[i]["納入単価"].ToString()) + "'";
                                }

                                if (Utility.getHeadIntValue(gcMultiRow1, "OP5checkBoxCell") == 0)
                                {
                                }
                                else if (Utility.getHeadIntValue(gcMultiRow1, "OP5checkBoxCell") == 1 & upkbn == 0)
                                {
                                    W_SQL = W_SQL + " 本部原価 ='" + Utility.S_Set(dataTable.Rows[i]["本部原価"].ToString()) + "'";
                                    upkbn = 1;
                                }
                                else
                                {
                                    W_SQL = W_SQL + " ,本部原価 ='" + Utility.S_Set(dataTable.Rows[i]["本部原価"].ToString()) + "'";
                                }

                                W_SQL = W_SQL + " WHERE 得意先コード = '" + Utility.S_Set(dataTable.Rows[i]["得意先コード"].ToString()) + "'";
                                if ((Utility.getHeadIntValue(gcMultiRow1, "取込区分textBoxCell") == 0 & Utility.Z_Set(dataTable.Rows[i]["区分"].ToString()) == 1))
                                {
                                    W_SQL = W_SQL + " AND 先方商品コード = '" + Utility.S_Set(dataTable.Rows[i]["先方商品コード"].ToString()) + "'";
                                }
                                else
                                {
                                    W_SQL = W_SQL + " AND 商品コード = '" + Utility.S_Set(dataTable.Rows[i]["商品コード"].ToString()) + "'";
                                }

                                sqlDb.ExecuteSql(W_SQL, -1);
                            }
                        }
                    }
                }
            }

            return 0;

        }

    }
}
