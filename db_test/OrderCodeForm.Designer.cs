namespace Demo.Layout.Order
{
    partial class OrderCodeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.button1 = new System.Windows.Forms.Button();
            this.northwindDataSet = new Demos.NorthwindDataSet();
            this.受注商品管理BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.受注商品管理TableAdapter = new Demos.NorthwindDataSetTableAdapters.受注商品管理TableAdapter();
            this.tableAdapterManager = new Demos.NorthwindDataSetTableAdapters.TableAdapterManager();
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.northwindDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注商品管理BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.DataSource = this.受注商品管理BindingSource;
            this.gcMultiRow1.Location = new System.Drawing.Point(12, 12);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.Size = new System.Drawing.Size(578, 218);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Text = "gcMultiRow1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(515, 236);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "設定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // OrderCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 267);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gcMultiRow1);
            this.Name = "OrderCodeForm";
            this.Text = "OrderCodeForm";
            this.Load += new System.EventHandler(this.OrderCodeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcMultiRow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.northwindDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.受注商品管理BindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private System.Windows.Forms.Button button1;
        private Demos.NorthwindDataSet northwindDataSet;
        private System.Windows.Forms.BindingSource 受注商品管理BindingSource;
        private Demos.NorthwindDataSetTableAdapters.受注商品管理TableAdapter 受注商品管理TableAdapter;
        private Demos.NorthwindDataSetTableAdapters.TableAdapterManager tableAdapterManager;
    }
}