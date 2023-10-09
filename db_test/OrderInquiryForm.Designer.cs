namespace Demo.Layout.Order
{
    partial class OrderInquiryForm
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.northwindDataSet = new Demos.NorthwindDataSet();
            this.受注商品管理BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.受注商品管理TableAdapter = new Demos.NorthwindDataSetTableAdapters.受注商品管理TableAdapter();
            this.tableAdapterManager = new Demos.NorthwindDataSetTableAdapters.TableAdapterManager();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.northwindDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注商品管理BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gcMultiRow1);
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMultiRow1.Location = new System.Drawing.Point(0, 0);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(708, 400);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // northwindDataSet
            // 
            this.northwindDataSet.DataSetName = "NorthwindDataSet";
            this.northwindDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 受注商品管理BindingSource
            // 
            this.受注商品管理BindingSource.DataMember = "受注商品管理";
            this.受注商品管理BindingSource.DataSource = this.northwindDataSet;
            // 
            // 受注商品管理TableAdapter
            // 
            this.受注商品管理TableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.UpdateOrder = Demos.NorthwindDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.仕入先TableAdapter = null;
            this.tableAdapterManager.受注TableAdapter = null;
            this.tableAdapterManager.受注明細TableAdapter = null;
            this.tableAdapterManager.商品TableAdapter = null;
            this.tableAdapterManager.商品区分TableAdapter = null;
            this.tableAdapterManager.得意先TableAdapter = null;
            this.tableAdapterManager.社員TableAdapter = null;
            this.tableAdapterManager.運送会社TableAdapter = null;
            this.tableAdapterManager.都道府県TableAdapter = null;
            // 
            // OrderInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Description = "受注内容の照会画面を模したサンプルです。受注コードから受注伝票を表示することができます。";
            this.Name = "OrderInputForm";
            this.Title = "受注照会画面";
            this.Load += new System.EventHandler(this.OrderFormUserControl_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.northwindDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注商品管理BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private Demos.NorthwindDataSet northwindDataSet;
        private System.Windows.Forms.BindingSource 受注商品管理BindingSource;
        private Demos.NorthwindDataSetTableAdapters.受注商品管理TableAdapter 受注商品管理TableAdapter;
        private Demos.NorthwindDataSetTableAdapters.TableAdapterManager tableAdapterManager;
    }
}
