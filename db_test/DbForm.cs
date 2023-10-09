using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace db_test
{
    public partial class DbForm : Form
    {
        public DbForm()
        {
            InitializeComponent();
        }

        // DB検索
        private void button1_Click(object sender, EventArgs e)
        {
            SqlDb db = new SqlDb();
            DataTable tb;
            db.Connect();
            tb = db.ExecuteSql("select col1,col2,col3 from test", -1);
            for (int i = 0; i <= tb.Rows.Count - 1; i++)
            {
                System.Diagnostics.Trace.WriteLine(
                    tb.Rows[i]["col1"].ToString() + ":" +
                    tb.Rows[i]["col2"].ToString() + ":" +
                    tb.Rows[i]["col3"].ToString());
            }
            db.Disconnect();
        }

        // DB追加
        private void button2_Click(object sender, EventArgs e)
        {
            SqlDb db = new SqlDb();
            db.Connect();
            db.BeginTransaction();
            db.ExecuteSql(
                "insert into test(col1,col2,col3) " +
                "values('1234567890',9,'2010-02-14 05:06:07')", -1);
            db.CommitTransaction();
            db.Disconnect();
        }

        // DB更新
        private void button3_Click(object sender, EventArgs e)
        {
            SqlDb db = new SqlDb();
            db.Connect();
            db.BeginTransaction();
            db.ExecuteSql(
                "update test set col2=3,col3='2010-02-15 05:06:07' " +
                "where col1='1234567890'", -1);
            db.CommitTransaction();
            db.Disconnect();
        }

        // DB削除
        private void button4_Click(object sender, EventArgs e)
        {
            SqlDb db = new SqlDb();
            db.Connect();
            db.BeginTransaction();
            db.ExecuteSql(
                "delete from test " +
                "where col1='1234567890'", -1);
            db.CommitTransaction();
            db.Disconnect();
        }
    }
}
