using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spider
{
    public partial class SpiderProperty : Form
    {
        private int maxDepth;

        public int MaxDepth
        {
            get { return maxDepth; }
            set { maxDepth = value; }
        }
        private int maxConnextion;

        public int MaxConnextion
        {
            get { return maxConnextion; }
            set { maxConnextion = value; }
        }
        public SpiderProperty()
        {
            InitializeComponent();
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void SpiderProperty_Load(object sender, EventArgs e)
        {
            tbxConn.Text = MaxConnextion.ToString();
            tbxDepth.Text = MaxDepth.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                MaxConnextion = int.Parse(tbxConn.Text);
                MaxDepth = int.Parse(tbxDepth.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入正确数字");
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            Close();
        }
    }
}
