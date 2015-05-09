using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Spider
{
    public partial class Form1 : Form
    {
        private SpiderLogic mSpider = null;
        private delegate void CSHandler(string arg0, string arg1);
        private delegate void DFHandler(int arg1);
        public Form1()
        {
            InitializeComponent();
            mSpider = new SpiderLogic();
            btnStop.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
            mSpider.ContentsSaved += new Spider.SpiderLogic.ContentsSavedHandler(Spider_ContentsSaved);
            mSpider.DownloadFinish += new Spider.SpiderLogic.DownloadFinishHandler(Spider_DownloadFinish);
        }

        private void btnUrl_Click(object sender, EventArgs e)
        {
            SpiderProperty sp = new SpiderProperty()
            {
                MaxConnextion = mSpider.MaxConnection,
                MaxDepth = mSpider.MaxDepth,
                StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            };
            if (sp.ShowDialog() == DialogResult.OK)
            {
                mSpider.MaxConnection = sp.MaxConnextion;
                mSpider.MaxDepth = sp.MaxDepth;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbxUrl.Text = "cust.edu.cn";
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "选择文件夹";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            var result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = fbd.SelectedPath;
                tbxPath.Text = path;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxUrl.Text))
            {
                mSpider.RootUrl = tbxUrl.Text;
            }
            else
            {
                MessageBox.Show("请输入正确网址");
            }
            Thread thread = new Thread(new ParameterizedThreadStart(DownLoad));
            thread.Start(tbxPath.Text);
            btnDown.Enabled = false;
            btnDown.Text = "downloading...";
            btnStop.Enabled = true;
        }

        private void DownLoad(object param)
        {
            try
            {
                mSpider.DownLoad(param.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Spider_ContentsSaved(string path, string url)
        {
            //ListDownload.Items.Add(new { Url = u, File = p });
            CSHandler h = (p, u) =>
            {
                ListDownload.Items.Add(new ListViewItem(new string[] { url, path }));
            };
            Dispatcher.CurrentDispatcher.Invoke(h, path, url);
        }

        void Spider_DownloadFinish(int count)
        {
            btnDown.Enabled = true;
            btnDown.Text = "Download";
            btnStop.Enabled = false;
            MessageBox.Show("Finished " + count.ToString());
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnDown.Enabled = true;
            btnDown.Text = "Download";
            btnStop.Enabled = false;
            mSpider.Stop = true;
            MessageBox.Show("已下载" + mSpider.Read.Keys.Count);
            mSpider.Unread.Clear();
        }
    }
}
