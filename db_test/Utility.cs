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
    class Utility
    {

        public static string GetIdent(String tableName)
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
                command.CommandText = "SELECT IDENT_CURRENT('" + tableName + "') as ID";

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

        public static String GetCode(String tableName, String fieldName, String whereName, String filedValue)
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
                command.CommandText = "SELECT " + fieldName + " FROM " + tableName + " WHERE " + whereName + " = '" + filedValue + "'";

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

        public static DataTable GetComboBoxData(String sql)
        {
            DataTable table = new DataTable();

            string strcon = db_test.Properties.Settings.Default.SPEEDDBConnectionString;

            SqlConnection connection = new SqlConnection(strcon); 
            
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

        public static void SetComboBoxAppearance(ComboBox cb, DrawItemEventArgs e, int[] fieldWidth, String[] fieldName)
        {
            DataTable dt = (DataTable)cb.DataSource;

            Pen p = new Pen(Color.Gray);
            Brush b = new SolidBrush(e.ForeColor);

            e.DrawBackground();

            int width = 0;

            for (int i = 0; i < fieldName.Length; i++)
            {
                e.Graphics.DrawString(Convert.ToString(dt.Rows[e.Index][fieldName[i]]), e.Font, b, width, e.Bounds.Y);

                e.Graphics.DrawLine(p, width + fieldWidth[i], e.Bounds.Top, width + fieldWidth[i], e.Bounds.Bottom);

                width = width + fieldWidth[i];
            }

            cb.DropDownWidth = width;

            if (Convert.ToBoolean(e.State & DrawItemState.Selected)) ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds);
        }

        public static void SetComboBoxExAppearance(ComboBoxEx cb, DrawItemEventArgs e, int[] fieldWidth, String[] fieldName)
        {
            DataTable dt = (DataTable)cb.DataSource;

            Pen p = new Pen(Color.Gray);
            Brush b = new SolidBrush(e.ForeColor);

            e.DrawBackground();

            int width = 0;

            for (int i = 0; i < fieldName.Length; i++)
            {
                e.Graphics.DrawString(Convert.ToString(dt.Rows[e.Index][fieldName[i]]), e.Font, b, width, e.Bounds.Y);

                e.Graphics.DrawLine(p, width + fieldWidth[i], e.Bounds.Top, width + fieldWidth[i], e.Bounds.Bottom);

                width = width + fieldWidth[i];
            }

            cb.DropDownWidth = width;

            if (Convert.ToBoolean(e.State & DrawItemState.Selected)) ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds);
        }

        public static object Nz(Object p_Moji, Object t_Moji)
        {
            object ret = null;

            if (p_Moji == null)
            {
                ret = t_Moji;
            }
            else
            {
                ret = p_Moji;
            }

            return ret;

        }

        public static string S_Set(Object p_Moji)
        {
            string ret=null;

            if (p_Moji==null)
            {
                ret = " ";
            }else if (p_Moji.ToString() == "")
            {
                ret = " ";
            }else
            {
                ret = p_Moji.ToString();
            }

            return ret;

        }

        public static decimal Z_Set(object p_Su)
        {
            decimal ret = 0;

            if (p_Su == null)
            {
                ret = 0;
            }
            else if (IsNumeric(p_Su))
            {
                ret = Convert.ToDecimal(p_Su);
            }
            else
            {
                ret = 0;
            }

            return ret;
        }

        public static bool IsNumeric(string stTarget)
        {
            double dNullable;

            return double.TryParse(
                stTarget,
                System.Globalization.NumberStyles.Any,
                null,
                out dNullable
            );
        }

        public static bool IsNumeric(object oTarget)
        {
            return IsNumeric(oTarget.ToString());
        }

        /// <summary>
        /// 文字列の指定した位置から指定した長さを取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="start">開始位置</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Mid(string str, int start, int len)
        {
            if (start <= 0)
            {
                throw new ArgumentException("引数'start'は1以上でなければなりません。");
            }
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null || str.Length < start)
            {
                return "";
            }
            if (str.Length < (start + len))
            {
                return str.Substring(start - 1);
            }
            return str.Substring(start - 1, len);
        }

        /// <summary>
        /// 文字列の指定した位置から末尾までを取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="start">開始位置</param>
        /// <returns>取得した文字列</returns>
        public static string Mid(string str, int start)
        {
            return Mid(str, start, str.Length);
        }

        /// <summary>
        /// 文字列の先頭から指定した長さの文字列を取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Left(string str, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null)
            {
                return "";
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(0, len);
        }

        /// <summary>
        /// 文字列の末尾から指定した長さの文字列を取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Right(string str, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null)
            {
                return "";
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(str.Length - len, len);
        }
        public static string getDate(string inDate)
        {
            string ret;
            DateTime dt;
            int num;
            int len;

            ret = inDate;
            ret = ret.Replace("/", "");
            if (!(int.TryParse(ret, out num)))
            {
                ret = "";
            }

            len = ret.Length;

            if (len == 4)
            {
                ret = DateTime.Today.ToString("yyyy") + ret;
            }else if(len == 6 )
            {
                ret = "20" + ret;
            }else if(len ==8 )
            {
                // 何もしない
            }else
            {
                ret = "";
            }

            if (ret != "")
            {

                ret = ret.Insert(4, "/").Insert(7, "/");

                if (DateTime.TryParse(ret, out dt))
                {
                    ret = dt.ToString("yyyy/MM/dd");
                }
                else
                {
                    ret = "";
                }
            }

            return ret;

        }

        public static bool existCombo(GcComboBoxCell gcComboBoxCell, string strTarget)
        {
            bool ret = false;

            if (gcComboBoxCell != null)
            {
                for (int i = 0; i < gcComboBoxCell.Items.Count; ++i)
                {
                    if (gcComboBoxCell.Items[i].Text == strTarget)
                    {
                        ret = true;
                        break;
                    }
                }
            }

            return ret;
        
        }

        public static int getHeadIndex(GcMultiRow gcMultiRow, string cName)
        {

            return gcMultiRow.ColumnHeaders[0].Cells[cName].CellIndex;

        }

        public static int getHeadIntValue(GcMultiRow gcMultiRow, string cName)
        {

            return (int)gcMultiRow.ColumnHeaders[0].Cells[cName].Value;

        }

        public static string getHeadStrValue(GcMultiRow gcMultiRow, string cName)
        {

            return (string)gcMultiRow.ColumnHeaders[0].Cells[cName].Value;

        }

        public static void setHeadIntValue(GcMultiRow gcMultiRow, string cName, int intVal)
        {

            gcMultiRow.ColumnHeaders[0].Cells[cName].Value = intVal;

        }

        public static void setHeadStrValue(GcMultiRow gcMultiRow, string cName, string strVal)
        {

            gcMultiRow.ColumnHeaders[0].Cells[cName].Value = strVal;

        }

        public static int chkCombo(string tableName, string rowName, string keyCode)
        {
            int ret = 0;

            SqlDb sqlDb = new SqlDb();
            DataTable dataTable;
            sqlDb.Connect();

            string strSQL;

            strSQL = "";

            dataTable = sqlDb.ExecuteSql("SELECT " + rowName + " FROM " + tableName + " WHERE " +  rowName + "='" + keyCode + "'", -1);

            if (dataTable.Rows.Count > 0)
            {
                ret = 1;
            }

            sqlDb.Disconnect();

            return ret;

        }

        public static string MidB(string sData, int nStart, int nLen)
        {
            Encoding oSJisEncoding = Encoding.GetEncoding("Shift_JIS");
            byte[] nByteAry = oSJisEncoding.GetBytes(sData);

            // 開始が最大文字数より後ろだった場合、空文字を戻す
            if (nByteAry.Length < nStart)
            {
                return "";
            }

            // nLenが最大文字数を超えないように調整
            if (nByteAry.Length < (nStart - 1) + nLen)
            {
                nLen = nByteAry.Length - (nStart - 1);
            }

            // 指定バイト数取りだし
            string sMidStr = oSJisEncoding.GetString(nByteAry, nStart - 1, nLen);

            // 最初の文字が全角の途中で切れていた場合はカット
            string sLeft = oSJisEncoding.GetString(nByteAry, 0, nStart);
            char sFirstMoji = sData[sLeft.Length - 1];
            if (sMidStr != "" && sFirstMoji != sMidStr[0])
            {
                sMidStr = sMidStr.Substring(1);
            }

            // 最後の文字が全角の途中で切れていた場合はカット
            sLeft = oSJisEncoding.GetString(nByteAry, 0, (nStart - 1) + nLen);
            char sLastMoji = sData[sLeft.Length - 1];
            if (sMidStr != "" && sLastMoji != sMidStr[sMidStr.Length - 1])
            {
                sMidStr = sMidStr.Substring(0, sMidStr.Length - 1);
            }

            return sMidStr;
        }

    }
}
