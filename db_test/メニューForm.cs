using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace db_test
{
    public partial class メニューForm : Form
    {
        public メニューForm()
        {
            InitializeComponent();


        }


        private void 受注入力button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            受注入力Form 受注入力form = new 受注入力Form();
            受注入力form.Show();
        }

        private void 得意先受注残問合せbutton_Click(object sender, EventArgs e)
        {
            得意先別商品別受注残問合せForm 得意先別商品別受注残form = new 得意先別商品別受注残問合せForm();
            得意先別商品別受注残form.Show();
        }

        private void 仕入先発注残問合せbutton_Click(object sender, EventArgs e)
        {
            仕入先別商品別発注残問合せForm 仕入先別商品別発注残問合せform = new 仕入先別商品別発注残問合せForm();
            仕入先別商品別発注残問合せform.Show();
        }

        private void 得意先売上明細参照button_Click(object sender, EventArgs e)
        {
            得意先売上明細参照Form 得意先売上明細参照form = new 得意先売上明細参照Form();
            得意先売上明細参照form.Show();
        }

        private void 受注明細参照button_Click(object sender, EventArgs e)
        {
            受注明細参照Form 受注明細参照form = new 受注明細参照Form();
            受注明細参照form.Show();
        }

        private void 売上計上明細参照button_Click(object sender, EventArgs e)
        {
            売上計上明細参照Form 売上計上明細参照form = new 売上計上明細参照Form();
            売上計上明細参照form.Show();
        }

        private void 仕入計上明細参照button_Click(object sender, EventArgs e)
        {
            仕入計上明細参照Form 仕入計上明細参照form = new 仕入計上明細参照Form();
            仕入計上明細参照form.Show();
        }

        private void 修了button_Click(object sender, EventArgs e)
        {
            //アプリケーションを終了する
            Application.Exit();
        }

        private void 商品問合せbutton_Click(object sender, EventArgs e)
        {
            商品問合せForm 商品問合せform = new 商品問合せForm();
            商品問合せform.Show();
        }

        private void 仕入先別発注残明細表button_Click(object sender, EventArgs e)
        {
            仕入先別発注残明細表Form 仕入先別発注残明細表form = new 仕入先別発注残明細表Form();
            仕入先別発注残明細表form.Show();
        }

        private void 出荷指示一覧表button_Click(object sender, EventArgs e)
        {
            出荷指示一覧表Form 出荷指示一覧表form = new 出荷指示一覧表Form();
            出荷指示一覧表form.Show();

        }

        private void 適正在庫切れ一覧表button_Click(object sender, EventArgs e)
        {
            適正在庫切れ一覧表Form 適正在庫切れ一覧表form = new 適正在庫切れ一覧表Form();
            適正在庫切れ一覧表form.Show();

        }

        private void 得意先別商品別掛率リストbutton_Click(object sender, EventArgs e)
        {
            得意先別商品別掛率リストForm 得意先別商品別掛率リストform = new 得意先別商品別掛率リストForm();
            得意先別商品別掛率リストform.Show();

        }

        private void 出荷可能一覧表button_Click(object sender, EventArgs e)
        {
            出荷可能一覧表Form 出荷可能一覧表form = new 出荷可能一覧表Form();
            出荷可能一覧表form.Show();

        }

        private void EOSドラスタ受注取込button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            EOSドラスタ受注取込Form EOSドラスタ受注取込form = new EOSドラスタ受注取込Form();
            EOSドラスタ受注取込form.Show();

        }

        private void EXCEL受注取込button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            EXCEL受注取込Form EXCEL受注取込form = new EXCEL受注取込Form();
            EXCEL受注取込form.Show();

        }

        private void EOSドラスタ送信データ作成button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            EOSドラスタ送信データ作成Form EOSドラスタ送信データ作成form = new EOSドラスタ送信データ作成Form();
            EOSドラスタ送信データ作成form.Show();

        }

        private void EOSドラスタ納品書発行button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            EOSドラスタ納品書発行Form EOSドラスタ納品書発行form = new EOSドラスタ納品書発行Form();
            EOSドラスタ納品書発行form.Show();

        }

        private void 商品変換マスタメンテbutton_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            商品変換マスタメンテForm 商品変換マスタメンテform = new 商品変換マスタメンテForm();
            商品変換マスタメンテform.Show();

        }

        private void 発注データ作成button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            発注データ作成Form 発注データ作成form = new 発注データ作成Form();
            発注データ作成form.Show();

        }

        private void 商品変換マスタ取込button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            商品変換マスタ取込Form 商品変換マスタ取込form = new 商品変換マスタ取込Form();
            商品変換マスタ取込form.Show();

        }

        private void バックオーダーリストbutton_Click(object sender, EventArgs e)
        {
            バックオーダーリストForm バックオーダーリストform = new バックオーダーリストForm();
            バックオーダーリストform.Show();

        }

        private void 見積書button_Click(object sender, EventArgs e)
        {
            見積書Form 見積書form = new 見積書Form();
            見積書form.Show();

        }

        private void 受注残集計button_Click(object sender, EventArgs e)
        {
            受注残集計Form 受注残集計form = new 受注残集計Form();
            受注残集計form.Show();

        }

        private void 得意先履歴単価更新処理button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            得意先履歴単価更新Form 得意先履歴単価更新form = new 得意先履歴単価更新Form();
            得意先履歴単価更新form.Show();

        }

        private void 得意先履歴単価コピー処理button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            得意先履歴単価コピーForm 得意先履歴単価コピーform = new 得意先履歴単価コピーForm();
            得意先履歴単価コピーform.Show();

        }

        private void 発注書button_Click(object sender, EventArgs e)
        {
            発注書Form 発注書form = new 発注書Form();
            発注書form.Show();

        }

        private void 納品書button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            納品書Form 納品書form = new 納品書Form();
            納品書form.Show();

        }

        private void オンライン発注リストbutton_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            オンライン発注リストForm オンライン発注リストform = new オンライン発注リストForm();
            オンライン発注リストform.Show();
        }

        private void 発注予定リスト作成button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            新受注入力Form 新受注入力form = new 新受注入力Form();
            新受注入力form.Show();

        }

        private void 発注入力button_Click(object sender, EventArgs e)
        {
            発注入力Form 発注入力form = new 発注入力Form();
            発注入力form.Show();

        }

        private void 売上計上入力button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            売上計上入力Form 売上計上入力form = new 売上計上入力Form();
            売上計上入力form.Show();

        }

        private void 仕入計上button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            仕入計上入力Form 仕入計上入力form = new 仕入計上入力Form();
            仕入計上入力form.Show();

        }

        private void 発注残集計button_Click_1(object sender, EventArgs e)
        {
            発注残集計Form 発注残集計form = new 発注残集計Form();
            発注残集計form.Show();

        }

        private void monotaRO受注取込button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            monotaRO受注取込Form monotaRO受注取込form = new monotaRO受注取込Form();
            monotaRO受注取込form.Show();
        }

        private void monotaRO一括発注button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            monotaRO一括発注Form monotaRO一括発注form = new monotaRO一括発注Form();
            monotaRO一括発注form.Show();

        }

        private void ハンディ発注入力button_Click(object sender, EventArgs e)
        {
            //本番ではうごかない
            発注入力Form 発注入力form = new 発注入力Form();
            発注入力form.Show();

        }

        private void monotaRO回答データ送信button_Click(object sender, EventArgs e)
        {
            monotaRO回答データ作成Form monotaRO回答データ作成form = new monotaRO回答データ作成Form();
            monotaRO回答データ作成form.Show();
        }

        private void monotaRO受注問合せ入力button_Click(object sender, EventArgs e)
        {
            monotaRO受注問合せ入力Form monotaRO受注問合せ入力form = new monotaRO受注問合せ入力Form();
            monotaRO受注問合せ入力form.Show();

        }

        private void monotaRO出荷指示一覧表button_Click(object sender, EventArgs e)
        {
            monotaRO出荷指示一覧表Form monotaRO出荷指示一覧表form = new monotaRO出荷指示一覧表Form();
            monotaRO出荷指示一覧表form.Show();

        }

        private void monotaRO売上データ送信button_Click(object sender, EventArgs e)
        {
            monotaRO売上データ作成Form monotaRO売上データ作成form = new monotaRO売上データ作成Form();
            monotaRO売上データ作成form.Show();

        }

        private void monotaRO発注データ作成button_Click(object sender, EventArgs e)
        {
            monotaRO発注送信データ作成Form monotaRO発注送信データ作成form = new monotaRO発注送信データ作成Form();
            monotaRO発注送信データ作成form.Show();

        }

        private void monotaROオンライン発注リストbutton_Click(object sender, EventArgs e)
        {
            monotaROオンライン発注リストForm monotaROオンライン発注リストform = new monotaROオンライン発注リストForm();
            monotaROオンライン発注リストform.Show();

        }


    }
}
