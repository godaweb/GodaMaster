using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace db_test
{
    /// <summary>
    ///     オペレーター選択ダイアログ
    /// </summary>
    public partial class SelectOperatorDialog : Form
    {
        /// <summary>
        ///     コンストラクタ。
        /// </summary>
        public SelectOperatorDialog()
        {
            InitializeComponent();

            Operator = new Operator();
        }


        /// <summary>
        ///     オペレーター情報
        /// </summary>
        public static Operator Operator
        {
            get;
            protected set;
        }


        /// <summary>
        ///     OKボタンがクリックされたとき。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkClicked(object sender, EventArgs e)
        {
            Operator = new Operator(comboBoxOperatorCode.Text, textBoxOperatorName.Text, string.Empty);

            if (!Operator.IsSelected)
            {
                MessageBox.Show("オペレーター番号を入力してください", "オペレーター選択", MessageBoxButtons.OK, MessageBoxIcon.Information);
                comboBoxOperatorCode.Focus();
                comboBoxOperatorCode.SelectAll();
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
            
        }

        private void SelectOperatorDialog_Load(object sender, EventArgs e)
        {
            // TODO: このコード行はデータを 'sPEEDDBDataSet1.オペレーターマスタ選択' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            this.オペレーターマスタ選択TableAdapter.Fill(this.sPEEDDBDataSet1.オペレーターマスタ選択);

            comboBoxOperatorCode.Text = "";
        }


        /// <summary>
        ///     オペレーターコードが変更されたとき。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOperatorCodeChanged(object sender, EventArgs e)
        {
            this.textBoxOperatorName.Text = string.Empty;
            try
            {
                var row = this.sPEEDDBDataSet1.オペレーターマスタ選択.FindByオペレーターコード(comboBoxOperatorCode.Text);
                if (!row.Isオペレーター名Null())
                {
                    this.textBoxOperatorName.Text = row.オペレーター名;
                }
            }
            catch
            {
            }
        }
    }
}
