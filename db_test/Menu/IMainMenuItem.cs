using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db_test.Menu
{
    /// <summary>
    ///     メニューアイテムを実装します。
    /// </summary>
    public interface IMainMenuItem
    {
        /// <summary>
        ///     メニュー名を取得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     起動するFormの型を取得します。
        /// </summary>
        Type Type { get; }

        /// <summary>
        ///     Formを表示します。
        /// </summary>
        void ShowForm();
    }
}
