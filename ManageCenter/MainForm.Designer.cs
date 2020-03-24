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
            this.button1 = new System.Windows.Forms.Button();
            this.devInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.connIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.devIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isOnlineDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.prodNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceDgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.devInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // DeviceDgv
            // 
            this.DeviceDgv.AutoGenerateColumns = false;
            this.DeviceDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DeviceDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.connIdDataGridViewTextBoxColumn,
            this.devIdDataGridViewTextBoxColumn,
            this.isOnlineDataGridViewCheckBoxColumn,
            this.prodNameDataGridViewTextBoxColumn});
            this.DeviceDgv.DataSource = this.devInfoBindingSource;
            this.DeviceDgv.Location = new System.Drawing.Point(12, 12);
            this.DeviceDgv.Name = "DeviceDgv";
            this.DeviceDgv.RowTemplate.Height = 23;
            this.DeviceDgv.Size = new System.Drawing.Size(517, 249);
            this.DeviceDgv.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(75, 294);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // devInfoBindingSource
            // 
            this.devInfoBindingSource.DataSource = typeof(ManageCenter.SvcManageRef.DevInfo);
            // 
            // connIdDataGridViewTextBoxColumn
            // 
            this.connIdDataGridViewTextBoxColumn.DataPropertyName = "ConnId";
            this.connIdDataGridViewTextBoxColumn.HeaderText = "ConnId";
            this.connIdDataGridViewTextBoxColumn.Name = "connIdDataGridViewTextBoxColumn";
            // 
            // devIdDataGridViewTextBoxColumn
            // 
            this.devIdDataGridViewTextBoxColumn.DataPropertyName = "DevId";
            this.devIdDataGridViewTextBoxColumn.HeaderText = "DevId";
            this.devIdDataGridViewTextBoxColumn.Name = "devIdDataGridViewTextBoxColumn";
            // 
            // isOnlineDataGridViewCheckBoxColumn
            // 
            this.isOnlineDataGridViewCheckBoxColumn.DataPropertyName = "IsOnline";
            this.isOnlineDataGridViewCheckBoxColumn.HeaderText = "IsOnline";
            this.isOnlineDataGridViewCheckBoxColumn.Name = "isOnlineDataGridViewCheckBoxColumn";
            // 
            // prodNameDataGridViewTextBoxColumn
            // 
            this.prodNameDataGridViewTextBoxColumn.DataPropertyName = "ProdName";
            this.prodNameDataGridViewTextBoxColumn.HeaderText = "ProdName";
            this.prodNameDataGridViewTextBoxColumn.Name = "prodNameDataGridViewTextBoxColumn";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 369);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DeviceDgv);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.DeviceDgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.devInfoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DeviceDgv;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource devInfoBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn connIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn devIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOnlineDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prodNameDataGridViewTextBoxColumn;
    }
}