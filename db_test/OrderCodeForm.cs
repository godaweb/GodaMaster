using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demo.Layout.Order
{
    public partial class OrderCodeForm : Form
    {
        private string _code;


        public OrderCodeForm()
        {
            InitializeComponent();
        }

        private void OrderCodeForm_Load(object sender, EventArgs e)
        {
            // TODO: このコード行はデータを 'northwindDataSet.受注商品管理' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            this.受注商品管理TableAdapter.Fill(this.northwindDataSet.受注商品管理);

            gcMultiRow1.Template = new OrderCodeTemplate();
            gcMultiRow1.AllowUserToAddRows = false;
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;
            gcMultiRow1.ReadOnly = true;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.productCode = gcMultiRow1.CurrentRow.Cells[0].Value.ToString();
            this.DialogResult = DialogResult.OK;

        }

        public string productCode
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }

    }
}
