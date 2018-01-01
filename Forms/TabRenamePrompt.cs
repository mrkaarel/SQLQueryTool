using System;
using System.Windows.Forms;

namespace SqlQueryTool.Forms
{
    public partial class TabRenamePrompt : Form
    {
        public TabRenamePrompt(string currentTabText)
        {
            InitializeComponent();

            txtTabText.Text = currentTabText;
        }

        public string TabText { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TabText = txtTabText.Text;
            DialogResult = DialogResult.OK;
        }
    }
}