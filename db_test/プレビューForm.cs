using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;

namespace db_test
{
    public partial class プレビューForm : Form
    {
        public ReportDocument printImage = new ReportDocument();
        public DataSet dataSet;
        public DataTable dataTable;
        public String rptName;

        public プレビューForm()
        {
            InitializeComponent();
        }

        private void プレビューForm_Load(object sender, EventArgs e)
        {

            if (rptName == "仕入先別発注残明細表CrystalReport")
            {
                仕入先別発注残明細表CrystalReport cr = new 仕入先別発注残明細表CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            }
            else if (rptName == "出荷指示一覧表CrystalReport")
            {
                出荷指示一覧表CrystalReport cr = new 出荷指示一覧表CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            }
            else if (rptName == "出荷可能一覧表CrystalReport")
            {
                出荷可能一覧表CrystalReport cr = new 出荷可能一覧表CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            }
            else if (rptName == "出荷可能一覧表_得意先順CrystalReport")
            {
                出荷可能一覧表_得意先順CrystalReport cr = new 出荷可能一覧表_得意先順CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            }
            else if (rptName == "適正在庫切れ一覧表CrystalReport")
            {
                適正在庫切れ一覧表CrystalReport cr = new 適正在庫切れ一覧表CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            }
            else if (rptName == "オンライン発注リストCrystalReport")
            {
                オンライン発注リストCrystalReport cr = new オンライン発注リストCrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            }
            else if (rptName == "得意先別商品別掛率リストCrystalReport")
            {
                得意先別商品別掛率リストCrystalReport cr = new 得意先別商品別掛率リストCrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            
            }
            else if (rptName == "バックオーダーリストCrystalReport")
            {
                バックオーダーリストCrystalReport cr = new バックオーダーリストCrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            }
            else if (rptName == "バックオーダーリスト_商品順CrystalReport")
            {
                バックオーダーリスト_商品順CrystalReport cr = new バックオーダーリスト_商品順CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            }
            else if (rptName == "商品変換取込変換商品登録済CrystalReport")
            {
                商品変換取込変換商品登録済CrystalReport cr = new 商品変換取込変換商品登録済CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                //cr.SetDataSource(dataSet.Tables[0]);
                cr.SetDataSource(dataTable);
            }
            else if (rptName == "納品書CrystalReport")
            {
                納品書CrystalReport cr = new 納品書CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataSet.Tables[0]);
            }
            else if (rptName == "ドラスタ受注エラーリストCrystalReport")
            {
                ドラスタ受注エラーリストCrystalReport cr = new ドラスタ受注エラーリストCrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
            }
            else if (rptName == "EXCEL受注取込チェックリストCrystalReport")
            {
                EXCEL受注取込チェックリストCrystalReport cr = new EXCEL受注取込チェックリストCrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
            }
            else if (rptName == "EXCEL受注取込エラーリストCrystalReport")
            {
                EXCEL受注取込エラーリストCrystalReport cr = new EXCEL受注取込エラーリストCrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
            }
            else if (rptName == "見積書CrystalReport")
            {
                見積書CrystalReport cr = new 見積書CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
            }
            else if (rptName == "発注データ作成CrystalReport")
            {
                発注データ作成CrystalReport cr = new 発注データ作成CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
            }
            else if (rptName == "発注書CrystalReport")
            {
                発注書CrystalReport cr = new 発注書CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
                // PDF形式でファイル出力
                try
                {
                    // 出力先ファイル名を指定
                    CrystalDecisions.Shared.DiskFileDestinationOptions fileOption;
                    fileOption = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                    fileOption.DiskFileName = "c:\\work\\output.pdf";

                    // 外部ファイル出力をPDF出力として定義する
                    CrystalDecisions.Shared.ExportOptions option;
                    option = cr.ExportOptions;
                    option.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                    option.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                    option.FormatOptions = new CrystalDecisions.Shared.PdfRtfWordFormatOptions();
                    option.DestinationOptions = fileOption;

                    // pdfとして外部ファイル出力を行う
                    cr.Export();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (rptName == "monotaRO受注取込チェックリストCrystalReport")
            {
                monotaRO受注取込チェックリストCrystalReport cr = new monotaRO受注取込チェックリストCrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
            }
            else if (rptName == "monotaRO受注取込エラーリストCrystalReport")
            {
                monotaRO受注取込エラーリストCrystalReport cr = new monotaRO受注取込エラーリストCrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
            }
            else if (rptName == "EOSドラスタ送信データ作成CrystalReport")
            {
                EOSドラスタ送信データ作成CrystalReport cr = new EOSドラスタ送信データ作成CrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
            }
            else if (rptName == "得意先履歴単価更新リストCrystalReport")
            {
                得意先履歴単価更新リストCrystalReport cr = new 得意先履歴単価更新リストCrystalReport();
                crystalReportViewer1.ReportSource = cr;
                cr.SetDataSource(dataTable);
            }

            //ステータス バーを表示するかどうかを取得または設定します。
            crystalReportViewer1.DisplayStatusBar = true;

            //レポートをビュー ウィンドウの枠から離して表示するかどうかを判別します。False = 左側とTOPがくっつく
            crystalReportViewer1.DisplayBackgroundEdge = true;

            crystalReportViewer1.EnableDrillDown = true;

            //グループツリー非表示
            crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
