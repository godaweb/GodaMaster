using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Reflection;

namespace db_test
{
    class ExcelCsvClass
    {
        public void excel_Export(string fileName, DataTable dataTable)
        {
          //Excelオブジェクトの初期化
          Excel.Application ExcelApp = null;
          Excel.Workbooks wbs = null;
          Excel.Workbook wb = null;
          Excel.Sheets shs = null;
          Excel.Worksheet ws = null;

          try
          {
            //Excelシートのインスタンスを作る
            ExcelApp = new Excel.Application();
            wbs = ExcelApp.Workbooks;
            wb = wbs.Add();

            shs = wb.Sheets;
            ws = shs[1];
            ws.Select(Type.Missing);

            ExcelApp.Visible = false;

            // エクセルファイルにデータをセットする
            for (int i = 1; i <= dataTable.Rows.Count-1; i++)
            {
              // Excelのcell指定
              Excel.Range w_rgn = ws.Cells;
              Excel.Range rgn = w_rgn[i, dataTable.Columns.Count];

              try
              {

                  //for (int j = 1; j <= dataTable.Columns.Count - 1; j++)
                  //{

                      //rgn = w_rgn[i, j];
                      //rgn.Value2 = dataTable.Rows[i - 1][j].ToString();
                      // Excelにデータをセット
                      rgn = w_rgn[i, 1];
                      rgn.Value = dataTable.Rows[i - 1]["得意先コード"].ToString();
                      rgn = w_rgn[i, 2];
                      rgn.Value = dataTable.Rows[i - 1]["先方商品コード"].ToString();
                      rgn = w_rgn[i, 3];
                      rgn.Value = dataTable.Rows[i - 1]["商品コード"].ToString();
                      rgn = w_rgn[i, 4];
                      rgn.Value = dataTable.Rows[i - 1]["商品名"].ToString();
                      rgn = w_rgn[i, 5];
                      rgn.Value = dataTable.Rows[i - 1]["ＪＡＮコード"].ToString();
                      rgn = w_rgn[i, 6];
                      rgn.Value = dataTable.Rows[i - 1]["店舗売価"].ToString();
                      rgn = w_rgn[i, 7];
                      rgn.Value = dataTable.Rows[i - 1]["納入単価"].ToString();
                      rgn = w_rgn[i, 8];
                      rgn.Value = dataTable.Rows[i - 1]["本部原価"].ToString();
                  //}
              }
              finally
              {
                  // Excelのオブジェクトはループごとに開放する
                  Marshal.ReleaseComObject(w_rgn);
                  Marshal.ReleaseComObject(rgn);
                  w_rgn = null;
                  rgn = null;
              }
            }

            //excelファイルの保存
//            wb.SaveAs(@"HOGE:\huge\sample.xlsx");
            wb.SaveAs(fileName);
            wb.Close(false);
            ExcelApp.Quit();
          }
          finally
          {
             //Excelのオブジェクトを開放し忘れているとプロセスが落ちないため注意
             Marshal.ReleaseComObject(ws);
             Marshal.ReleaseComObject(shs);
             Marshal.ReleaseComObject(wb);
             Marshal.ReleaseComObject(wbs);
             Marshal.ReleaseComObject(ExcelApp);
             ws = null;
             shs = null;
             wb = null;
             wbs = null;
             ExcelApp = null;

             GC.Collect();
          }
        }

        public void excel_Import(string fileName, DataTable dataTable)
        {
            //Microsoft.Office.Interop.Excel.Application ExcelApp
            //  = new Microsoft.Office.Interop.Excel.Application();

            //Excelオブジェクトの初期化
            Excel.Application ExcelApp = null;
            //Excel.Workbooks wbs = null;
            //Excel.Workbook wb = null;
            //Excel.Sheets shs = null;
            //Excel.Worksheet ws = null;

            //Excelシートのインスタンスを作る
            ExcelApp = new Excel.Application();
            //wbs = ExcelApp.Workbooks;
            
            ExcelApp.Visible = false;
            Excel.Workbook wb = ExcelApp.Workbooks.Open(fileName);

            Excel.Worksheet ws1 = wb.Sheets[1];
            ws1.Select(Type.Missing);
            int chk = 0;
            for (int i = 1; i < 10000; i++)
            {
                DataRow dr=null;
                Excel.Range rgn = null;
                dynamic val = null;

                rgn = ws1.Cells[i, 1];
                val = rgn.Value2;
                if (val == null)
                {
                    break;
                }

                dr = dataTable.NewRow();
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    rgn = ws1.Cells[i, j+1];
                    val = rgn.Value2;
                    // [""]は列名
                    dr[j] = val;

                }

                // 行を追加
                dataTable.Rows.Add(dr);

            }
            wb.Close(false);
            ExcelApp.Quit();

            if (dataTable.Rows.Count > 0)
            {
                string connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString))
                {
                    bulkCopy.BulkCopyTimeout = 600; // in seconds
                    bulkCopy.DestinationTableName = "W商品変換マスタ取込";
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        public void excel_RunMacro(string filepath)
        {
            //Excelマクロファイルパス
            //string strMacroPath = @"C:\OBIC7S4\EXCEL受注取込\EXCEL受注取込元データ.xls";
            string strMacroPath = @filepath;

            // Excel操作用COMオブジェクトを生成する
            //ApplicationClass oExcel = new ApplicationClass();
            object oExcel = CreateObject("Excel.Application");

            //ワークブックコレクションオブジェクトを生成する。
            //Workbooks oBooks = oExcel.Workbooks;
            object oBooks = oExcel.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, oExcel, null);

            //Excelファイルのオープン
            //Workbook oBook = oBooks.Open(strMacroPath);
            object oBook = oBooks.GetType().InvokeMember(
                                  "Open", BindingFlags.InvokeMethod, null,
                                  oBooks, new object[] { 
                                                               strMacroPath
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing
                                                             , Type.Missing 
                                                             });

            // Excelファイルの表示
            //oExcel.Visible = true;
            oExcel.GetType().InvokeMember("Visible", BindingFlags.SetProperty, null, oExcel, new object[] { true });

            //マクロ実行(Testというサブプロシージャを実行する)
            //oExcel.Run("Test");
            oExcel.GetType().InvokeMember("Run", BindingFlags.InvokeMethod, null, oExcel, new object[] { "writeCSV" });

            //閉じる
            //oBook.Close(false);
            oExcel.GetType().InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, oExcel, null); 

            //COM解放
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBooks);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
            oBook = null;
            oBooks = null;
            oExcel = null;
        }

        /// <summary>
        /// COMオブジェクトへの参照を作成および取得します
        /// </summary>
        /// <param name="progId">作成するオブジェクトのプログラムID</param>
        /// <param name="serverName">
        /// オブジェクトが作成されるネットワークサーバー名
        /// </param>
        /// <returns>作成されたCOMオブジェクト</returns>
        public static object CreateObject(string progId, string serverName)
        {
            Type t;
            if (serverName == null || serverName.Length == 0)
                t = Type.GetTypeFromProgID(progId);
            else
                t = Type.GetTypeFromProgID(progId, serverName, true);

            return Activator.CreateInstance(t);
        }

        /// <summary>
        /// COMオブジェクトへの参照を作成および取得します
        /// </summary>
        /// <param name="progId">作成するオブジェクトのプログラムID</param>
        /// <returns>作成されたCOMオブジェクト</returns>
        public static object CreateObject(string progId)
        {
            return CreateObject(progId, null);
        }
    }
}
