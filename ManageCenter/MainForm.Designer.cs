namespace ManageCenter
{
    partial class MainForm
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
            this.DeviceDgv = new System.Windows.Forms.DataGridView();
            this.UpdateBtn = new System.Windows.Forms.Button();
            this.devInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.connIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prodNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.devIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastActiveTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isOnlineDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceDgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.devInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // DeviceDgv
            // 
            this.DeviceDgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceDgv.AutoGenerateColumns = false;
            this.DeviceDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DeviceDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.connIdDataGridViewTextBoxColumn,
            this.prodNameDataGridViewTextBoxColumn,
            this.devIdDataGridViewTextBoxColumn,
            this.ConnTime,
            this.LastActiveTime,
            this.isOnlineDataGridViewCheckBoxColumn});
            this.DeviceDgv.DataSource = this.devInfoBindingSource;
            this.DeviceDgv.Location = new System.Drawing.Point(2, 3);
            this.DeviceDgv.Name = "DeviceDgv";
            this.DeviceDgv.RowTemplate.Height = 23;
            this.DeviceDgv.Size = new System.Drawing.Size(654, 316);
            this.DeviceDgv.TabIndex = 0;
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.Location = new System.Drawing.Point(22, 334);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(75, 23);
            this.UpdateBtn.TabIndex = 1;
            this.UpdateBtn.Text = "刷新";
            this.UpdateBtn.UseVisualStyleBackColor = true;
            this.UpdateBtn.Click += new System.EventHandler(this.Button1_Click);
            // 
            // devInfoBindingSource
            // 
            this.devInfoBindingSource.DataSource = typeof(ManageCenter.SvcManageRef.DevInfo);
            // 
            // connIdDataGridViewTextBoxColumn
            // 
            this.connIdDataGridViewTextBoxColumn.DataPropertyName = "ConnId";
            this.connIdDataGridViewTextBoxColumn.HeaderText = "连接ID";
            this.connIdDataGridViewTextBoxColumn.Name = "connIdDataGridViewTextBoxColumn";
            this.connIdDataGridViewTextBoxColumn.Width = 70;
            // 
            // prodNameDataGridViewTextBoxColumn
            // 
            this.prodNameDataGridViewTextBoxColumn.DataPropertyName = "ProdName";
            this.prodNameDataGridViewTextBoxColumn.HeaderText = "产品名称";
            this.prodNameDataGridViewTextBoxColumn.Name = "prodNameDataGridViewTextBoxColumn";
            this.prodNameDataGridViewTextBoxColumn.Width = 80;
            // 
            // devIdDataGridViewTextBoxColumn
            // 
            this.devIdDataGridViewTextBoxColumn.DataPropertyName = "DevId";
            this.devIdDataGridViewTextBoxColumn.HeaderText = "设备ID";
            this.devIdDataGridViewTextBoxColumn.Name = "devIdDataGridViewTextBoxColumn";
            this.devIdDataGridViewTextBoxColumn.Width = 80;
            // 
            // ConnTime
            // 
            this.ConnTime.DataPropertyName = "ConnTime";
            this.ConnTime.HeaderText = "登录时间";
            this.ConnTime.Name = "ConnTime";
            this.ConnTime.ReadOnly = true;
            this.ConnTime.Width = 150;
            // 
            // LastActiveTime
            // 
            this.LastActiveTime.DataPropertyName = "LastActiveTime";
            this.LastActiveTime.HeaderText = "最后活跃时间";
            this.LastActiveTime.Name = "LastActiveTime";
            this.LastActiveTime.ReadOnly = true;
            this.LastActiveTime.Width = 150;
            // 
            // isOnlineDataGridViewCheckBoxColumn
            // 
            this.isOnlineDataGridViewCheckBoxColumn.DataPropertyName = "IsOnline";
            this.isOnlineDataGridViewCheckBoxColumn.HeaderText = "是否在线？";
            this.isOnlineDataGridViewCheckBoxColumn.Name = "isOnlineDataGridViewCheckBoxColumn";
            this.isOnlineDataGridViewCheckBoxColumn.Width = 80;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 369);
            this.Controls.Add(this.UpdateBtn);
            this.Controls.Add(this.DeviceDgv);
            this.Name = "MainForm";
            this.Text = "SCADA管理中心";
            ((System.ComponentModel.ISupportInitialize)(this.DeviceDgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.devInfoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DeviceDgv;
        private System.Windows.Forms.Button UpdateBtn;
        private System.Windows.Forms.BindingSource devInfoBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn connIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prodNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn devIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastActiveTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOnlineDataGridViewCheckBoxColumn;
    }
}