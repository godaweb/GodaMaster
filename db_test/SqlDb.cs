using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace db_test
{
    class SqlDb
    {
        /// <summary>
        /// SQLコネクション
        /// </summary>
        private SqlConnection _con = null;

        /// <summary>
        /// トランザクション・オブジェクト
        /// </summary>
        /// <remarks></remarks>
        private SqlTransaction _trn = null;

        /// <summary>
        /// DB接続
        /// </summary>
        /// <param name="svr">サーバー名／IP</param>
        /// <param name="dbn">データベース名</param>
        /// <param name="uid">ユーザーID</param>
        /// <param name="pas">パスワード</param>
        /// <param name="tot">タイムアウト値</param>
        /// <remarks></remarks>

        public void Connect(
            String svr, String dbn, String uid, String pas, int tot)
        {

            try
            {
                if (_con == null)
                {
                    _con = new SqlConnection();
                }

                String cst = "";
                cst = cst + "Server=" + svr;
                cst = cst + ";Database=" + dbn;
                cst = cst + ";User ID=" + uid;
                cst = cst + ";Password=" + pas;
                if (tot > -1)
                {
                    //_con.ConnectionTimeout = tot;
                    cst = cst + ";Connect Timeout=" + tot.ToString();
                }
                cst = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
                
                _con.ConnectionString = cst;


                _con.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Connect Error", ex);
            }
        }

        public void Connect()
        {

            try
            {
                if (_con == null)
                {
                    _con = new SqlConnection();
                }

                String cst = db_test.Properties.Settings.Default.SPEEDDBConnectionString;

                _con.ConnectionString = cst;

                _con.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Connect Error", ex);
            }
        }

        /// <summary>
        /// DB切断
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (_con.State == ConnectionState.Open)
                {
                    _con.Close();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Disconnect Error", ex);
            }
        }

        /// <summary>
        /// SQLの実行
        /// </summary>
        /// <param name="sql">SQL文</param>
        /// <param name="tot">タイムアウト値</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable ExecuteSql(String sql, int tot)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand sqlCommand = new SqlCommand(sql, _con, _trn);

                if (tot > -1)
                {
                    sqlCommand.CommandTimeout = tot;
                }

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                adapter.Fill(dt);
                adapter.Dispose();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("ExecuteSql Error", ex);
            }

            return dt;
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        /// <remarks></remarks>
        public void BeginTransaction()
        {
            try
            {
                _trn = _con.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception("BeginTransaction Error", ex);
            }
        }

        /// <summary>
        /// コミット
        /// </summary>
        /// <remarks></remarks>
        public void CommitTransaction()
        {
            try
            {
                if (_trn != null)
                {
                    _trn.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CommitTransaction Error", ex);
            }
            finally
            {
                _trn = null;
            }
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        /// <remarks></remarks>
        public void RollbackTransaction()
        {
            try
            {
                if (_trn != null)
                {
                    _trn.Rollback();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("RollbackTransaction Error", ex);
            }
            finally
            {
                _trn = null;
            }
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        /// <remarks></remarks>
        ~SqlDb()
        {
            Disconnect();
        }
    }
}
