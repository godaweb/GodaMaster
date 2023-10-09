using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrapeCity.Win.MultiRow;
using System.Windows;

namespace db_test
{
    /// <summary>
    ///     GcMultiRowの拡張メソッドを定義します。
    /// </summary>
    public static class GcMultiRowExtension
    {
        /// <summary>
        ///     エラーハンドリングを設定します。
        /// </summary>
        /// <param name="gcMultiRow"></param>
        public static void SetErrorHandle(this GcMultiRow gcMultiRow)
        {
            gcMultiRow.DataError += (s, e) =>
            {
                // メッセージボックスで詳細を表示
                MessageBox.Show(e.Exception.StackTrace, e.Exception.Message);

                // 例外を止める
                e.ThrowException = false;
            };
        }
    }
}
