using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using System.Data.SqlClient;
//using Intercom.FaxCenter.FaxCenterAPI;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace db_test
{
    public partial class 発注書Form : Form
    {
        string str出力先 = null;       //0:印刷、1:FAX
        string strFAX不可分 = null;
        string str出力区分 = null;     //0:通常、1:再発行      
        string str直送指定 = null;     //0:ALL、1:直送のみ
        string str出力指定 = null;     //0:範囲、1:個別(仕入先)、2:個別(発注No) 
        string str来勘区分 = null;     //0:印字なし、1:印字
        string strキャンセル文 = null; //0:印字なし、1:印字
        
        private DataSet dataSet;
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        private DataTable dataTableFAX;

//        FaxCenterAPI FAXCenterAPICtrl1; // 宣言
//        const string szAppName = "TestApp";
//        string ManagerServerName;
//        int Port;

        public 発注書Form()
        {
            InitializeComponent();
        }

        private void 発注書Form_Load(object sender, EventArgs e)
        {
            this.gcMultiRow1.Template = new 発注書Template();
            this.gcMultiRow1.MultiSelect = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.ScrollBars = ScrollBars.None;

            //セル色の設定
            //非選択状態の色
            //gcMultiRow1.Rows[0].Cells[0].Style.BackColor = Color.Blue;
            //gcMultiRow1.Rows[0].Cells[0].Style.ForeColor = Color.White;

            //選択状態のときの色
            gcMultiRow1.DefaultCellStyle.SelectionBackColor = Color.White;
            gcMultiRow1.DefaultCellStyle.SelectionForeColor = Color.Black;

            //編集状態のときの色
            gcMultiRow1.DefaultCellStyle.EditingBackColor = Color.Yellow;
            gcMultiRow1.DefaultCellStyle.EditingForeColor = Color.Black;

            //無効のときの色
            gcMultiRow1.DefaultCellStyle.DisabledBackColor = Color.White;
            gcMultiRow1.DefaultCellStyle.DisabledForeColor = Color.Black;

            // イベント
            //gcMultiRow1.CellEndEdit += new EventHandler<CellEndEditEventArgs>(gcMultiRow1_CellEndEdit);
            gcMultiRow1.CellContentClick += new EventHandler<CellEventArgs>(gcMultiRow1_CellContentClick);
            //gcMultiRow1.EditingControlShowing += new EventHandler<EditingControlShowingEventArgs>(gcMultiRow1_EditingControlShowing);
            //gcMultiRow1.PreviewKeyDown += new PreviewKeyDownEventHandler(gcMultiRow1_PreviewKeyDown);
            //ButtonCell preViewButtonCell = this.gcMultiRow1.ColumnHeaders[0].Cells["プレビューbuttonCell"] as ButtonCell;
            //preViewButtonCell.GcMultiRow.PreviewKeyDown += new PreviewKeyDownEventHandler(GcMultiRow_PreviewKeyDown);
            //ButtonCell preViewButtonCell1 = this.gcMultiRow1.ColumnHeaders[0].Cells["印刷buttonCell"] as ButtonCell;
            //preViewButtonCell1.GcMultiRow.PreviewKeyDown += new PreviewKeyDownEventHandler(GcMultiRow_PrintPreviewKeyDown);
            //ButtonCell preViewButtonCell2 = this.gcMultiRow1.ColumnHeaders[0].Cells["終了buttonCell"] as ButtonCell;
            //preViewButtonCell2.GcMultiRow.PreviewKeyDown += new PreviewKeyDownEventHandler(GcMultiRow_EndPreviewKeyDown);
            gcMultiRow1.NewCellPositionNeeded += new EventHandler<NewCellPositionNeededEventArgs>(gcMultiRow1_NewCellPositionNeeded);
            gcMultiRow1.CellValidated += new EventHandler<CellEventArgs>(gcMultiRow1_CellValidated);
            gcMultiRow1.CellValidating += new EventHandler<CellValidatingEventArgs>(gcMultiRow1_CellValidating);
            
            // GcMultiRowがフォーカスを失ったときに
            // セルのCellStyle.SelectionBackColorとCellStyle.SelectionForeColorを表示しない場合はtrue。
            // それ以外の場合はfalse。
            gcMultiRow1.HideSelection = true;

            // 編集モード
            gcMultiRow1.EditMode = EditMode.EditOnEnter;

            // ショートカットキーの設定
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCellThenControl, Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right);
            gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveRight, Keys.Right);

            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Down | Keys.Control);
            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Up | Keys.Control);
            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Left | Keys.Control);
            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Right | Keys.Control);

            // 既定のショートカットキーを削除する
            //this.gcMultiRow1.ShortcutKeyManager.Unregister(Keys.F3);

            // 各ファンクションキーに対応するアクションを登録する
            //this.gcMultiRow1.ShortcutKeyManager.Register(new 出荷指示一覧表FunctionKeyAction(Keys.F3), Keys.F3);

            // The cell's in alternating Rows
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            this.gcMultiRow1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // カレント行の枠線の設定
            gcMultiRow1.CurrentCellBorderLine = GrapeCity.Win.MultiRow.Line.Empty;
            gcMultiRow1.CurrentRowBorderLine = new GrapeCity.Win.MultiRow.Line(LineStyle.Medium, Color.Black);

            // セル選択時、常に行全体が選択されるようにする
            gcMultiRow1.ViewMode = GrapeCity.Win.MultiRow.ViewMode.Row;

            // 初期表示
            gcMultiRow1.ColumnHeaders[0].Cells["出力先textBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["出力区分textBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["直送指定textBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["来勘区分textBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["キャンセル文textBoxCell"].Value = 0;
            gcMultiRow1.ColumnHeaders[0].Cells["印刷buttonCell"].Visible = true;
            gcMultiRow1.ColumnHeaders[0].Cells["ＦＡＸbuttonCell"].Visible = false;

            changeEnabled("0");

            gcMultiRow1.Select();
            gcMultiRow1.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(GrapeCity.Win.MultiRow.CellScope.ColumnHeader, 0, "出力先textBoxCell");
/*
            FAXCenterAPICtrl1 = new FaxCenterAPI(); // 初期化
            // イベントを利用する場合、各イベントを+=で追加
            FAXCenterAPICtrl1.NotifyPrintStatus += new EventPrintStatusHandler(FAXCenterAPICtrl1_NotifyPrintStatus);
            FAXCenterAPICtrl1.NotifyReceiveStatus += new EventReceiveStatusHandler(FAXCenterAPICtrl1_NotifyReceiveStatus);
            FAXCenterAPICtrl1.NotifySendStatus += new EventSendStatusHandler(FAXCenterAPICtrl1_NotifySendStatus);
            FAXCenterAPICtrl1.NotifyServerStatus += new EventServerStatusHandler(FAXCenterAPICtrl1_NotifyServerStatus);

            // 管理サーバー名
            ManagerServerName = "user-PC";

            // 接続ポート番号
            Port = 40480;

            if (FAXCenterAPICtrl1.Login(ManagerServerName, Port, szAppName) == 0)
            {
                MessageBox.Show("まいと～く Center Hybrid API のログインに成功");
            }
*/        
        }
/*
        void FAXCenterAPICtrl1_NotifyServerStatus(APIServerType Type, APIServerStatus Status, string ServerName, int ErrorCode)
        {
            throw new NotImplementedException();
        }

        void FAXCenterAPICtrl1_NotifySendStatus(ulong JobID, ulong LogID, APIJobType Type, APIJobStatus Status, int LineID, string FaxNumber, string DestName, string ImageFile, int NumPage, int NumMaxPage, int Speed, string FaxID, int NumRetry, int ErrorCode, string ErrorMessage, string UserParam)
        {
            throw new NotImplementedException();
        }

        void FAXCenterAPICtrl1_NotifyReceiveStatus(ulong JobID, ulong LogID, APIJobType Type, APIJobStatus Status, int LineID, string CIDNumber, string DialinNumber, string ImageFile, int NumPage, int Speed, string FaxID, int ErrorCode, string ErrorMessage)
        {
            throw new NotImplementedException();
        }

        void FAXCenterAPICtrl1_NotifyPrintStatus(APIPrinterStatus PrinterStatus, string FileName)
        {
            throw new NotImplementedException();
        }
*/
        void gcMultiRow1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            switch (e.CellName)
            {
                case "出力先textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["出力先textBoxCell"].EditedFormattedValue != null)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[0].Cells["出力先textBoxCell"].EditedFormattedValue == "0" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["出力先textBoxCell"].EditedFormattedValue == "1")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "出力区分textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["出力区分textBoxCell"].EditedFormattedValue != null)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[0].Cells["出力区分textBoxCell"].EditedFormattedValue == "0" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["出力区分textBoxCell"].EditedFormattedValue == "1")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "直送指定textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["直送指定textBoxCell"].EditedFormattedValue != null)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[0].Cells["直送指定textBoxCell"].EditedFormattedValue == "0" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["直送指定textBoxCell"].EditedFormattedValue == "1")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "出力指定textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].EditedFormattedValue != null)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].EditedFormattedValue == "0" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].EditedFormattedValue == "1" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].EditedFormattedValue == "2")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "来勘区分textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["来勘区分textBoxCell"].EditedFormattedValue != null)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[0].Cells["来勘区分textBoxCell"].EditedFormattedValue == "0" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["来勘区分textBoxCell"].EditedFormattedValue == "1")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
                case "キャンセル文textBoxCell":
                    if (gcMultiRow1.ColumnHeaders[0].Cells["キャンセル文textBoxCell"].EditedFormattedValue != null)
                    {
                        if ((string)gcMultiRow1.ColumnHeaders[0].Cells["キャンセル文textBoxCell"].EditedFormattedValue == "0" ||
                            (string)gcMultiRow1.ColumnHeaders[0].Cells["キャンセル文textBoxCell"].EditedFormattedValue == "1")
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;
            }
        }

        void gcMultiRow1_CellValidated(object sender, CellEventArgs e)
        {
            switch (e.CellName)
            {
                case "出力先textBoxCell":
                    str出力先 = (string)gcMultiRow1.ColumnHeaders[0].Cells["出力先textBoxCell"].EditedFormattedValue;
                    if (str出力先 == "1")
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["印刷buttonCell"].Visible = false;
                        gcMultiRow1.ColumnHeaders[0].Cells["ＦＡＸbuttonCell"].Visible = true;
                    }
                    else
                    {
                        gcMultiRow1.ColumnHeaders[0].Cells["印刷buttonCell"].Visible = true;
                        gcMultiRow1.ColumnHeaders[0].Cells["ＦＡＸbuttonCell"].Visible = false;
                    }
                    break;
                case "出力区分textBoxCell":
                    str出力区分 = (string)gcMultiRow1.ColumnHeaders[0].Cells["出力区分textBoxCell"].EditedFormattedValue;
                    break;
                case "直送指定textBoxCell":
                    str直送指定 = (string)gcMultiRow1.ColumnHeaders[0].Cells["直送指定textBoxCell"].EditedFormattedValue;
                    break;
                case "来勘区分textBoxCell":
                    str来勘区分 = (string)gcMultiRow1.ColumnHeaders[0].Cells["来勘区分textBoxCell"].EditedFormattedValue;
                    break;
                case "キャンセル文textBoxCell":
                    strキャンセル文 = (string)gcMultiRow1.ColumnHeaders[0].Cells["キャンセル文textBoxCell"].EditedFormattedValue;
                    break;
                case "出力指定textBoxCell":
                    str出力指定 = (string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].EditedFormattedValue;
                    changeEnabled(str出力指定);
                    break;
            }
        }

        void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName == "終了buttonCell")
            {
                this.Hide();
            }
            if (e.CellName == "プレビューbuttonCell")
            {
                if (createReportData("0") == 0)
                {
                    プレビューForm プレビューform = new プレビューForm();
                    プレビューform.dataTable = dataTable;
                    プレビューform.rptName = "発注書CrystalReport";
                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "発注書");
                }
            }
            else if (e.CellName == "印刷buttonCell")
            {
                if (createReportData("1") == 0)
                {
                    プレビューForm プレビューform = new プレビューForm();
                    プレビューform.dataTable = dataTable;
                    プレビューform.rptName = "発注書CrystalReport";
                    プレビューform.Show();
                }
                else
                {
                    MessageBox.Show("データがありません", "発注書");
                }
            }
            else if (e.CellName == "ＦＡＸbuttonCell")
            {
                if (createReportData("2") == 0 & createFAXData("2") == 0)
                {

                    string filePath = null;
                    string fileName = null;
                    string crfileName = null;

                    filePath = "C:\\Program Files (x86)\\Intercom\\MyFaxV9C\\CsvShare\\";

                    DataTable OutTable = new DataTable();
                    OutTable.Columns.Add("発注日");
                    OutTable.Columns.Add("仕入先コード");
                    OutTable.Columns.Add("仕名");
                    OutTable.Columns.Add("仕電話１");
                    OutTable.Columns.Add("仕電話２");
                    OutTable.Columns.Add("仕電話３");
                    OutTable.Columns.Add("仕ＦＡＸ電話１");
                    OutTable.Columns.Add("仕ＦＡＸ電話２");
                    OutTable.Columns.Add("仕ＦＡＸ電話３");
                    OutTable.Columns.Add("社名");
                    OutTable.Columns.Add("郵便番号A");
                    OutTable.Columns.Add("都道府県名A");
                    OutTable.Columns.Add("住所１A");
                    OutTable.Columns.Add("住所２A");
                    OutTable.Columns.Add("電話１");
                    OutTable.Columns.Add("電話２");
                    OutTable.Columns.Add("電話３");
                    OutTable.Columns.Add("ＦＡＸ電話１");
                    OutTable.Columns.Add("ＦＡＸ電話２");
                    OutTable.Columns.Add("ＦＡＸ電話３");
                    OutTable.Columns.Add("メーカー名");
                    OutTable.Columns.Add("メーカー品番");
                    OutTable.Columns.Add("商名");
                    OutTable.Columns.Add("明細摘要");
                    OutTable.Columns.Add("発注数");
                    OutTable.Columns.Add("発注番号");
                    OutTable.Columns.Add("発注連番");
                    OutTable.Columns.Add("発注金額");
                    OutTable.Columns.Add("発注行");
                    OutTable.Columns.Add("受注番号");
                    OutTable.Columns.Add("納入先コード");
                    OutTable.Columns.Add("得意先担当者コード");
                    OutTable.Columns.Add("仕入担当者コード");
                    OutTable.Columns.Add("直送先名");
                    OutTable.Columns.Add("郵便番号T");
                    OutTable.Columns.Add("住所１T");
                    OutTable.Columns.Add("住所２T");
                    OutTable.Columns.Add("電話１T");
                    OutTable.Columns.Add("電話２T");
                    OutTable.Columns.Add("電話３T");
                    OutTable.Columns.Add("ＦＡＸ電話１T");
                    OutTable.Columns.Add("ＦＡＸ電話２T");
                    OutTable.Columns.Add("ＦＡＸ電話３T");
                    OutTable.Columns.Add("来勘区分");
                    OutTable.Columns.Add("キャンセル文");
                    OutTable.Columns.Add("担当者コード");

                    string sirsaki_cd = null;
                    string sirsaki_nm = null;
                    string fax_no = null;
                    string strLine = null;

                    System.Text.Encoding enc =
                        System.Text.Encoding.GetEncoding("Shift_JIS");

                    DateTime dt = System.DateTime.Now;
                    fileName = dt.ToString("yyyyMMddHHmmss") + ".csv";

                    //書き込むファイルを開く
                    System.IO.StreamWriter streamWriter =
                        new System.IO.StreamWriter(filePath + fileName, false, enc);

                    for (int i = 0; i < dataTableFAX.Rows.Count; i++)
                    {

                        sirsaki_cd = dataTableFAX.Rows[i][0].ToString();

                        OutTable.Clear();
                        for (int j = 0; j < dataTable.Rows.Count; j++)
                        {
                            if (dataTable.Rows[j]["仕入先コード"].ToString() == sirsaki_cd)
                            {
                                sirsaki_nm = dataTable.Rows[j]["仕名"].ToString();
                                fax_no = "186" + dataTable.Rows[j]["仕ＦＡＸ電話１"].ToString();
                                fax_no += dataTable.Rows[j]["仕ＦＡＸ電話２"].ToString();
                                fax_no += dataTable.Rows[j]["仕ＦＡＸ電話３"].ToString();

                                OutTable.ImportRow(dataTable.Rows[j]);
                            }

                        }
                        発注書CrystalReport cr = new 発注書CrystalReport();
                        //crystalReportViewer1.ReportSource = cr;
                        cr.SetDataSource(OutTable);
                        // PDF形式でファイル出力
                        try
                        {
                            // 出力先ファイル名を指定
                            crfileName = "c:\\work\\" + sirsaki_cd.Trim() + "-" + dt.ToString("yyyyMMddHHmmss") + ".pdf";

                            CrystalDecisions.Shared.DiskFileDestinationOptions fileOption;
                            fileOption = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                            fileOption.DiskFileName = crfileName;

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

                        strLine = "V200";
                        strLine += "," + crfileName;
                        strLine += "," + fax_no;
                        strLine += "," + sirsaki_nm;
                        strLine += "," + "様";
                        streamWriter.WriteLine(strLine);

                    }

                    streamWriter.Close();

                    MessageBox.Show("発注書を出力しました", "発注書");

                }

            }
        }

        void gcMultiRow1_NewCellPositionNeeded(object sender, NewCellPositionNeededEventArgs e)
        {
            switch (e.CellName)
            {
                case "出力先textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力区分textBoxCell");
                    }
                    break;
                case "FAX不可分checkBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力先textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力先textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力区分textBoxCell");
                    }
                    break;
                case "出力区分textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "直送指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力先textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "直送指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力先textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "直送指定textBoxCell");
                    }
                    break;
                case "直送指定textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力指定textBoxCell");
                    }
                    break;
                case "出力指定textBoxCell":
                    str出力指定 = (string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].EditedFormattedValue;
                    changeEnabled(str出力指定);

                    if (gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].EditedFormattedValue == "0")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "直送指定textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード始textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "直送指定textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード始textBoxCell");
                        }
                    }
                    else if ((string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].EditedFormattedValue == "1")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０１textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "直送指定textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０１textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "直送指定textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０１textBoxCell");
                        }
                    }
                    else if ((string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].EditedFormattedValue == "2")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０１textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "直送指定textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０１textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "直送指定textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０１textBoxCell");
                        }
                    }
                    break;
                case "仕入先コード０１textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０２textBoxCell");
                    }
                    break;
                case "仕入先コード０２textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０１textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０１textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０３textBoxCell");
                    }
                    break;
                case "仕入先コード０３textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０４textBoxCell");
                    }
                    break;
                case "仕入先コード０４textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０５textBoxCell");
                    }
                    break;
                case "仕入先コード０５textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０６textBoxCell");
                    }
                    break;
                case "仕入先コード０６textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０７textBoxCell");
                    }
                    break;
                case "仕入先コード０７textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０８textBoxCell");
                    }
                    break;
                case "仕入先コード０８textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０９textBoxCell");
                    }
                    break;
                case "仕入先コード０９textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード１０textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード１０textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード１０textBoxCell");
                    }
                    break;
                case "仕入先コード１０textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "来勘区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "来勘区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "来勘区分textBoxCell");
                    }
                    break;
                case "仕入先コード始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード終textBoxCell");
                    }
                    break;
                case "仕入先コード終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                    }
                    break;
                case "発注番号０１textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０１textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０１textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力指定textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０１textBoxCell");
                    }
                    break;
                case "発注行番号０１textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０１textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０１textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０２textBoxCell");
                    }
                    break;
                case "発注番号０２textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０１textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０１textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０２textBoxCell");
                    }
                    break;
                case "発注行番号０２textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０３textBoxCell");
                    }
                    break;
                case "発注番号０３textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０３textBoxCell");
                    }
                    break;
                case "発注行番号０３textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０２textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０４textBoxCell");
                    }
                    break;
                case "発注番号０４textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０３textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０４textBoxCell");
                    }
                    break;
                case "発注行番号０４textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０５textBoxCell");
                    }
                    break;
                case "発注番号０５textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０４textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０５textBoxCell");
                    }
                    break;
                case "発注行番号０５textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０６textBoxCell");
                    }
                    break;
                case "発注番号０６textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０５textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０６textBoxCell");
                    }
                    break;
                case "発注行番号０６textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０７textBoxCell");
                    }
                    break;
                case "発注番号０７textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０６textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０７textBoxCell");
                    }
                    break;
                case "発注行番号０７textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０８textBoxCell");
                    }
                    break;
                case "発注番号０８textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０７textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０８textBoxCell");
                    }
                    break;
                case "発注行番号０８textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０９textBoxCell");
                    }
                    break;
                case "発注番号０９textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０８textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０９textBoxCell");
                    }
                    break;
                case "発注行番号０９textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号１０textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号１０textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号１０textBoxCell");
                    }
                    break;
                case "発注番号１０textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号１０textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号１０textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号０９textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注行番号１０textBoxCell");
                    }
                    break;
                case "発注行番号１０textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "来勘区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号１０textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "来勘区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号１０textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "来勘区分textBoxCell");
                    }
                    break;
                case "発注日始textBoxCell":
                    if ((string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].Value.ToString() == "0")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                        }
                    }
                    else if ((string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].Value == "1")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード１０textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "仕入先コード１０textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                        }
                    }
                    else if ((string)gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].Value == "2")
                    {
                        if (e.MoveStatus == MoveStatus.MoveDown)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveUp)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号１０textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveRight)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveLeft)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号１０textBoxCell");
                        }
                        else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                        }
                    }
                    break;
                case "発注日終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号始textBoxCell");
                    }
                    break;
                case "発注番号始textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注日終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号終textBoxCell");
                    }
                    break;
                case "発注番号終textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "来勘区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "来勘区分textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号始textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "来勘区分textBoxCell");
                    }
                    break;
                case "来勘区分textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセル文textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセル文textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセル文textBoxCell");
                    }
                    break;
                case "キャンセル文textBoxCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "発注番号終textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    break;
                case "プレビューbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        if (str出力先 == "1")
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ＦＡＸbuttonCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセル文textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        if (str出力先 == "1")
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ＦＡＸbuttonCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "キャンセル文textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        if (str出力先 == "1")
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ＦＡＸbuttonCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                    }
                    break;
                case "印刷buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    break;
                case "ＦＡＸbuttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "プレビューbuttonCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "終了buttonCell");
                    }
                    break;
                case "終了buttonCell":
                    if (e.MoveStatus == MoveStatus.MoveDown)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力先textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveUp)
                    {
                        if (str出力先 == "1")
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ＦＡＸbuttonCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                    }
                    else if (e.MoveStatus == MoveStatus.MoveRight)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力先textBoxCell");
                    }
                    else if (e.MoveStatus == MoveStatus.MoveLeft)
                    {
                        if (str出力先 == "1")
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "ＦＡＸbuttonCell");
                        }
                        else
                        {
                            e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "印刷buttonCell");
                        }
                    }
                    else if (e.MoveStatus == MoveStatus.MoveToNextCellThenControl)
                    {
                        e.NewCellPosition = new CellPosition(CellScope.ColumnHeader, 0, "出力先textBoxCell");
                    }
                    break;
            }
        }

        private int createReportData(string PrintMode)
        {
            int ret = 0;
            string 区分 = null;

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;


            // ストアド プロシージャ名を指定
            command.CommandText = "発注書";

            command.Parameters.AddWithValue("@PrintMode", PrintMode);
            command.Parameters.AddWithValue("@出力区分", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力区分textBoxCell"].Value.ToString());
            if ((bool)this.gcMultiRow1.ColumnHeaders[0].Cells["FAX不可分checkBoxCell"].Value == true)
            {
                command.Parameters.AddWithValue("@FAX不可分", "1");
            }
            else 
            {
                command.Parameters.AddWithValue("@FAX不可分", "0");
            }
            //command.Parameters.AddWithValue("@FAX不可分", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["FAX不可分checkBoxCell"].Value);
            //command.Parameters.AddWithValue("@FAX不可分", "0");
            command.Parameters.AddWithValue("@直送指定", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["直送指定textBoxCell"].Value.ToString());
            command.Parameters.AddWithValue("@出力指定", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].Value.ToString());
            command.Parameters.AddWithValue("@来勘区分", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["来勘区分textBoxCell"].Value.ToString());
            command.Parameters.AddWithValue("@キャンセル文", this.gcMultiRow1.ColumnHeaders[0].Cells["キャンセル文textBoxCell"].Value.ToString());
            //command.Parameters.AddWithValue("@キャンセル文", "0");

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０１", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０１", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０２", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０２", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０３", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０３", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０４", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０４", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０５", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０５", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０６", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０６", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０７", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０７", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０８", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０８", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０９", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０９", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード１０", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード１０", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０１", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０１", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０２", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０２", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０３", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０３", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０４", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０４", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０５", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０５", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０６", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０６", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０７", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０７", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０８", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０８", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０９", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０９", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号１０", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号１０", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード始", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード終", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード終", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注日始", this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注日始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注日終", this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注日終", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号始", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号終", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号終", "*");
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            dataSet = new DataSet();
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);

            //DataTableオブジェクトを用意
            dataTable = new DataTable();
            dataTable = dataSet.Tables[0];

            //dataTable.AcceptChanges();
            if (dataTable.Rows.Count <= 0)
            {
                ret = 1;
            }

            return ret;
        }

        private int createFAXData(string PrintMode)
        {
            int ret = 0;
            string 区分 = null;

            String connectionString = db_test.Properties.Settings.Default.SPEEDDBConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // コネクションを開く
            connection.Open();
            // コマンド作成
            SqlCommand command = connection.CreateCommand();
            // ストアド プロシージャを指定
            command.CommandType = CommandType.StoredProcedure;


            // ストアド プロシージャ名を指定
            command.CommandText = "発注書FAX";

            command.Parameters.AddWithValue("@PrintMode", PrintMode);
            command.Parameters.AddWithValue("@出力区分", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力区分textBoxCell"].Value.ToString());
            if ((bool)this.gcMultiRow1.ColumnHeaders[0].Cells["FAX不可分checkBoxCell"].Value == true)
            {
                command.Parameters.AddWithValue("@FAX不可分", "1");
            }
            else
            {
                command.Parameters.AddWithValue("@FAX不可分", "0");
            }
            //command.Parameters.AddWithValue("@FAX不可分", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["FAX不可分checkBoxCell"].Value);
            //command.Parameters.AddWithValue("@FAX不可分", "0");
            command.Parameters.AddWithValue("@直送指定", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["直送指定textBoxCell"].Value.ToString());
            command.Parameters.AddWithValue("@出力指定", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["出力指定textBoxCell"].Value.ToString());
            command.Parameters.AddWithValue("@来勘区分", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["来勘区分textBoxCell"].Value.ToString());
            command.Parameters.AddWithValue("@キャンセル文", (string)this.gcMultiRow1.ColumnHeaders[0].Cells["キャンセル文textBoxCell"].Value);
            //command.Parameters.AddWithValue("@キャンセル文", "0");

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０１", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０１", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０２", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０２", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０３", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０３", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０４", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０４", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０５", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０５", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０６", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０６", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０７", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０７", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０８", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０８", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード０９", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード０９", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード１０", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード１０", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０１", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０１", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０２", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０２", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０３", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０３", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０４", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０４", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０５", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０５", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０６", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０６", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０７", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０７", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０８", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０８", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号０９", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号０９", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号１０", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号１０", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード始", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@仕入先コード終", this.gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@仕入先コード終", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注日始", this.gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注日始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注日終", this.gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注日終", "*");
            }

            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号始", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号始", "*");
            }
            if ((string)this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Value != "*" &&
                this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Value != null)
            {
                command.Parameters.AddWithValue("@発注番号終", this.gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Value);
            }
            else
            {
                command.Parameters.AddWithValue("@発注番号終", "*");
            }

            // ストアド プロシージャを実行し、SELECT結果をdataSetへ格納
            dataSet = new DataSet();
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);

            //DataTableオブジェクトを用意
            dataTableFAX = new DataTable();
            dataTableFAX = dataSet.Tables[0];

            //dataTable.AcceptChanges();
            if (dataTableFAX.Rows.Count <= 0)
            {
                ret = 1;
            }

            return ret;
        }

        void changeEnabled(string 出力_指定)
        {
            if (出力_指定 == "0")
            {
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード_範囲labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先範囲波labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号_個別labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０１textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０２textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０３textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０４textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０５textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０６textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０７textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０８textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０９textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号１０textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日_範囲labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日波labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号_範囲labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号波labelCell"].Visible = true;
            }
            else if (出力_指定 == "1")
            {
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードlabelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード_範囲labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先範囲波labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号_個別labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０１textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０２textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０３textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０４textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０５textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０６textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０７textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０８textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０９textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号１０textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日_範囲labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日波labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号_範囲labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号波labelCell"].Visible = false;
            }
            else if (出力_指定 == "2")
            {
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード_範囲labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先範囲波labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号_個別labelCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０１textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０２textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０３textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０４textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０５textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０６textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０７textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０８textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０９textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号１０textBoxCell"].Visible = true;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日_範囲labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日波labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号_範囲labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号波labelCell"].Visible = false;
            }
            else
            {
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コードlabelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０１textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０２textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０３textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０４textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０５textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０６textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０７textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０８textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード０９textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード１０textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード_範囲labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先コード終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["仕入先範囲波labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号_個別labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０１textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０１textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０２textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０２textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０３textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０３textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０４textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０４textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０５textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０５textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０６textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０６textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０７textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０７textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０８textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０８textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号０９textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号０９textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号１０textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注行番号１０textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日_範囲labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注日波labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号_範囲labelCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号始textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号終textBoxCell"].Visible = false;
                gcMultiRow1.ColumnHeaders[0].Cells["発注番号波labelCell"].Visible = false;
            }
        }

    }
}
