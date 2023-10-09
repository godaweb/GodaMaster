using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using db_test;

namespace db_test
{
    class Common
    {
        public static String GetOPE事業所コード(String オペレーターコード)
        {
            string buf = null;
            DataTable table = new DataTable();
            string connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT 事業所コード FROM Tオペレーターマスタ WHERE オペレーターコード='" + オペレーターコード + "'";
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            finally
            {
                connection.Close();
            }
            if (table.Rows.Count > 0)
            {
                buf = table.Rows[0][0].ToString();
            }

            return buf;

        }

        public static DataTable GetData(String sql)
        {
            DataTable table = new DataTable();

            string connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(table);
            }
            finally
            {
                connection.Close();
            }

            return table;
        }

        public static int chk_dgt(String jancd, int dokbn)
        {
            int ret = 0;
            string wk_cdgcd;
            int i, kisu, gusu, wksu;

            if (dokbn == 1)
            {
                kisu = 0;
                gusu = 0;

                jancd.Trim();

                if (jancd.Length != 13)
                {
                    ret = 1;
                    return ret;
                }

                for (i = 0; i < 12; i++)
                {
                    wksu = Convert.ToInt32(jancd.Substring(i, 1));
                    if (wksu % 2 != 0)
                    {
                        kisu += wksu;
                    }
                    else
                    {
                        gusu += wksu;
                    }
                }

                wk_cdgcd = Utility.Right((10 - Utility.Z_Set(Utility.Right((gusu * 3 + kisu).ToString(), 1))).ToString(), 1);

                if (jancd.Substring(12, 1) != wk_cdgcd)
                {
                    ret = 1;
                }

            }

            return ret;

        }

    }
}
