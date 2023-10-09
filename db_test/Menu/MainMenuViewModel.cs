using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db_test.Menu
{
    /// <summary>
    ///     メインメニューのビューモデル
    /// </summary>
    public class MainMenuViewModel
    {
        /// <summary>
        ///     コンストラクタ。
        ///     メニューを構築します。
        /// </summary>
        public MainMenuViewModel()
        {
            Categories = new MainMenuList()
            {
                {
                    "受発注業務",
                    new MainMenuItemList()
                    {
                        new MainMenuItem<受注入力Form>("受注入力"),
                        new MainMenuItem<売上計上入力Form>("売上計上入力"),
                        new MainMenuItem<発注入力Form>("発注入力"),
                        new MainMenuItem<仕入計上入力Form>("仕入計上"),

                        new MainMenuItem<商品問合せForm>("商品問合せ"),

                        new MainMenuItem<得意先別商品別受注残問合せForm>("得意先別商品別受注残問合せ"),

                        new MainMenuItem<EOSドラスタ受注取込Form>("EOSドラスタ受注取込"),
                    }
                },
                {
                    "販売業務",
                    new MainMenuItemList()
                    {
                        new MainMenuItem<納品書Form>("納品書"),
                        new MainMenuItem<見積書Form>("見積書"),
                        new MainMenuItem<受注残集計Form>("受注残集計"),
                        new MainMenuItem<発注残集計Form>("発注残集計"),
                        new MainMenuItem<見積書Form>("見積書"),
                        new MainMenuItem<得意先履歴単価更新Form>("得意先履歴単価更新処理"),
                        new MainMenuItem<得意先履歴単価コピーForm>("得意先履歴単価コピー処理"),

                        new MainMenuItem<発注書Form>("発注書"),
                        new MainMenuItem<発注入力Form>("発注入力"),
                        new MainMenuItem<売上計上入力Form>("売上計上入力"),
                        //new MainMenuItem<仕入計上Form>("仕入計上"),
                    }
                },
                {
                    "マスタメンテ",
                    new MainMenuItemList()
                    {
                    }
                },
                {
                    "その他",
                    new MainMenuItemList()
                    {
                        new MainMenuItem<メニューForm>("旧メニュー"),
                    }
                }
            };
        }


        /// <summary>
        ///     メインメニューの構造を取得します。
        /// </summary>
        public Dictionary<string, List<IMainMenuItem>> Categories
        {
            get;
            private set;
        }
    }
}
