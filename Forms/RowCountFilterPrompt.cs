using System;
using System.Windows.Forms;

namespace SqlQueryTool.Forms
{
    public partial class RowCountFilterPrompt : Form
    {
        public RowCountFilterPrompt(int rowCount)
        {
            InitializeComponent();
            txtMinimumRowCount.Text = rowCount.ToString();
        }

        public int RowCount { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int result;

            if (!int.TryParse(txtMinimumRowCount.Text, out result)) result = 0;
            RowCount = result;
            DialogResult = DialogResult.OK;
        }
    }
}