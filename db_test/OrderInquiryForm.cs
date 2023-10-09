using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;


namespace Demo.Layout.Order
{
    public partial class OrderInquiryForm : Demos.DemoBase
    {
        public OrderInquiryForm()
        {
            InitializeComponent();
        }

        private void OrderFormUserControl_Load(object sender, EventArgs e)
        {
            // TODO: このコード行はデータを 'northwindDataSet.受注商品管理' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            this.受注商品管理TableAdapter.Fill(this.northwindDataSet.受注商品管理);

            gcMultiRow1.Template = new OrderInputTemplate();
            gcMultiRow1.AllowUserToAddRows = false;
            gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LemonChiffon;

            gcMultiRow1.CellContentButtonClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentButtonClick);
        }

        private void gcMultiRow1_CellContentButtonClick(object sender, GrapeCity.Win.MultiRow.CellEventArgs e)
        {
            if (e.CellName == "SearchbtnCell")
            {
                string _serchcode = "0";

                if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["SearchtextCell"].Value != null)
                {
                    _serchcode = (string)this.gcMultiRow1.ColumnHeaders[0].Cells["SearchtextCell"].Value;
                }

                DataRow[] dataRow = this.northwindDataSet.受注商品管理.Select("受注コード = " + _serchcode);
                this.gcMultiRow1.DataSource = dataRow;
            }
        }
    }
}
