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
    public partial class 受注残集計Form : Form
    {
        public 受注残集計Form()
        {
            InitializeComponent();
        }

        private void 受注残集計Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 受注残集計Template();
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
            gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            //gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellEditedFormattedValueChanged += new EventHandler<CellEditedFormattedValueChangedEventArgs>(gcMultiRow1_CellEditedFormattedValueChanged);

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
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCellThenControl, Keys.Up);

            // 既定のショートカットキーを削除する
            //this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);

            // 各ファンクションキーに対応するアクションを登録する
            //this.gcMultiRow1.ShortcutKeyManager.Register(new 出荷可能一覧表FunctionKeyAction(Keys.F3), Keys.F3);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["担当者checkBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["得意先checkBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["商品checkBoxCell"].Value = 0;

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "担当者checkBoxCell");
        }

        void gcMultiRow1_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            GcMultiRow gcMultiRow = sender as GcMultiRow;
            Cell currentCell = null;
            CheckBoxCell checkBoxCell1 = null;
            受注残集計_商品Form fsSyohin;
            受注残集計_得意先Form fsToksaki;
            受注残集計_担当者Form fsTantosya;
            switch (e.CellName)
            {
                //SNIPET: チェックボックスクリック時はこのイベント
                case "商品checkBoxCell":
                    currentCell = gcMultiRow1.ColumnHeaders[0].Cells["商品checkBoxCell"];
                    checkBoxCell1 = currentCell as CheckBoxCell;
                    if ((bool)checkBoxCell1.EditedFormattedValue)
                    {
                        fsSyohin = new 受注残集計_商品Form();
                        fsSyohin.Owner = this;
                        fsSyohin.Show();
                    }
                    else
                    {
                        foreach (Form frm in Application.OpenForms)
                        {
                            if (frm.Name == "受注残集計_商品Form")
                            {
                                frm.Close();
                                return;
                            }
                        }    

                    }
                    break;
                case "得意先checkBoxCell":
                    currentCell = gcMultiRow1.ColumnHeaders[0].Cells["得意先checkBoxCell"];
                    checkBoxCell1 = currentCell as CheckBoxCell;
                    if ((bool)checkBoxCell1.EditedFormattedValue)
                    {
                        fsToksaki = new 受注残集計_得意先Form();
                        fsToksaki.Owner = this;
                        fsToksaki.Show();
                    }
                    else
                    {
                        foreach (Form frm in Application.OpenForms)
                        {
                            if (frm.Name == "受注残集計_得意先Form")
                            {
                                frm.Close();
                                return;
                            }
                        }

                    }
                    break;
                case "担当者checkBoxCell":
                    currentCell = gcMultiRow1.ColumnHeaders[0].Cells["担当者checkBoxCell"];
                    checkBoxCell1 = currentCell as CheckBoxCell;
                    if ((bool)checkBoxCell1.EditedFormattedValue)
                    {
                        fsTantosya = new 受注残集計_担当者Form();
                        fsTantosya.Owner = this;
                        fsTantosya.Show();
                    }
                    else
                    {
                        foreach (Form frm in Application.OpenForms)
                        {
                            if (frm.Name == "受注残集計_担当者Form")
                            {
                                frm.Close();
                                return;
                            }
                        }

                    }
                    break;
            }
            
            

            if (currentCell.IsInEditMode)
            {
                if (currentCell is CheckBoxCell)
                {
                }
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            if (e.CellName == "商品checkBoxCell")
            {

                if (gcMultiRow1.ColumnHeaders[0].Cells["商品checkBoxCell"].Value.ToString() == "1")
                {
                    受注残集計_商品Form fsSyohin;
                    fsSyohin = new 受注残集計_商品Form();
                    fsSyohin.Owner = this;
                    fsSyohin.Show();
                }
                else
                {
                }

            }
        }

        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            if (e.CellName == "商品checkBoxCell")
            {
                e.Cancel = false;
            }
        }

        void gcMultiRow1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (gcMultiRow1.CurrentCellPosition.CellName)
            {
                case "商品checkBoxCell":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            if (gcMultiRow1.ColumnHeaders[0].Cells["商品checkBoxCell"].Value.ToString() == "1")
                            {
                                受注残集計_商品Form fsSyohin;
                                fsSyohin = new 受注残集計_商品Form();
                                fsSyohin.Owner = this;
                                fsSyohin.Show();
                            }
                            else
                            {
                            }
                            break;
                    }
                    break;
            }
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

        void gcMultiRow1_CellEndEdit(object sender, CellEndEditEventArgs e)
        {

        }
    }
}
