using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace db_test.Menu
{
    /// <summary>
    ///     メニューアイテムクラス
    /// </summary>
    /// <typeparam name="T">起動ターゲットFormクラス</typeparam>
    public class MainMenuItem<T> : IMainMenuItem where T : Form
    {
        /// <summary>
        ///     コンストラクタ。
        /// </summary>
        /// <param name="name">メニュー表記</param>
        public MainMenuItem(string name)
        {
            Name = name;
        }


        /// <summary>
        ///     メニュー表記を取得します。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        ///     起動ターゲットのTypeを取得します。
        /// </summary>
        public Type Type
        {
            get { return typeof(T); }
        }


        /// <summary>
        ///     起動ターゲットFormを表示します。
        /// </summary>
        public void ShowForm()
        {
            try
            {
                var form = Activator.CreateInstance<T>() as Form;
                if (form != null)
                {
                    form.Show();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("起動できませんでした", Name);
            }
        }
    }
}
