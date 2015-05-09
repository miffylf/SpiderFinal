namespace Spider
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblBaseUrl = new System.Windows.Forms.Label();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPath = new System.Windows.Forms.Button();
            this.btnUrl = new System.Windows.Forms.Button();
            this.tbxPath = new System.Windows.Forms.TextBox();
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.ListDownload = new System.Windows.Forms.ListView();
            this.URL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PATH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDown = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBaseUrl
            // 
            this.lblBaseUrl.AutoSize = true;
            this.lblBaseUrl.Location = new System.Drawing.Point(3, 21);
            this.lblBaseUrl.Name = "lblBaseUrl";
            this.lblBaseUrl.Size = new System.Drawing.Size(53, 12);
            this.lblBaseUrl.TabIndex = 0;
            this.lblBaseUrl.Text = "Base Url";
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(3, 51);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(59, 12);
            this.lblFilePath.TabIndex = 1;
            this.lblFilePath.Text = "File Path";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnPath);
            this.panel1.Controls.Add(this.btnUrl);
            this.panel1.Controls.Add(this.tbxPath);
            this.panel1.Controls.Add(this.tbxUrl);
            this.panel1.Controls.Add(this.lblFilePath);
            this.panel1.Controls.Add(this.lblBaseUrl);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(495, 85);
            this.panel1.TabIndex = 2;
            // 
            // btnPath
            // 
            this.btnPath.Location = new System.Drawing.Point(447, 43);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(29, 23);
            this.btnPath.TabIndex = 5;
            this.btnPath.Text = "...";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // btnUrl
            // 
            this.btnUrl.Location = new System.Drawing.Point(447, 18);
            this.btnUrl.Name = "btnUrl";
            this.btnUrl.Size = new System.Drawing.Size(29, 23);
            this.btnUrl.TabIndex = 4;
            this.btnUrl.Text = "...";
            this.btnUrl.UseVisualStyleBackColor = true;
            this.btnUrl.Click += new System.EventHandler(this.btnUrl_Click);
            // 
            // tbxPath
            // 
            this.tbxPath.Location = new System.Drawing.Point(68, 45);
            this.tbxPath.Name = "tbxPath";
            this.tbxPath.Size = new System.Drawing.Size(373, 21);
            this.tbxPath.TabIndex = 3;
            // 
            // tbxUrl
            // 
            this.tbxUrl.Location = new System.Drawing.Point(68, 18);
            this.tbxUrl.Name = "tbxUrl";
            this.tbxUrl.Size = new System.Drawing.Size(373, 21);
            this.tbxUrl.TabIndex = 2;
            // 
            // ListDownload
            // 
            this.ListDownload.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.URL,
            this.PATH});
            this.ListDownload.Location = new System.Drawing.Point(12, 104);
            this.ListDownload.Name = "ListDownload";
            this.ListDownload.Size = new System.Drawing.Size(495, 309);
            this.ListDownload.TabIndex = 3;
            this.ListDownload.UseCompatibleStateImageBehavior = false;
            this.ListDownload.View = System.Windows.Forms.View.Details;
            // 
            // URL
            // 
            this.URL.Text = "DownLoad Url";
            this.URL.Width = 246;
            // 
            // PATH
            // 
            this.PATH.Text = "Save File";
            this.PATH.Width = 245;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(12, 420);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(238, 23);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "DownLoad";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(269, 419);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(238, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 445);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.ListDownload);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = " 爬虫";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBaseUrl;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.Button btnUrl;
        private System.Windows.Forms.TextBox tbxPath;
        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.ListView ListDownload;
        private System.Windows.Forms.ColumnHeader URL;
        private System.Windows.Forms.ColumnHeader PATH;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnStop;
    }
}

