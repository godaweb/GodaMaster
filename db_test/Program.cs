using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using db_test.Menu;

namespace db_test
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // アプリケーション全体の例外処理を取得します
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, true);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(OnApplicationException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // オペレーター選択画面を表示
            using (var operatorForm = new SelectOperatorDialog())
            {
                if (operatorForm.ShowDialog() == DialogResult.OK)
                {
                    // メインメニューを表示
                    Application.Run(new MainMenuForm());
                }
            }

            //Application.Run(new メニューForm());
            //Application.Run(new 仕入先別発注残明細表Form());
        }

        /// <summary>
        ///     アプリケーション全体の例外をキャッチします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnApplicationException(object sender, ThreadExceptionEventArgs e)
        {
            // 以下行をコメントアウトするとメッセージボックスを表示せずに例外によるクラッシュを回避できます
            MessageBox.Show(e.Exception.StackTrace, e.Exception.Message);
        }
    }
}
