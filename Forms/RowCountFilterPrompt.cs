using System;
using System.Windows.Forms;

namespace SqlQueryTool.Forms
{
	public partial class RowCountFilterPrompt : Form
	{
		public int RowCount { get; set; }

		public RowCountFilterPrompt(int rowCount)
		{
			InitializeComponent();
			txtMinimumRowCount.Text = rowCount.ToString();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			int result;

			if (!Int32.TryParse(txtMinimumRowCount.Text, out result)) {
				result = 0;
			}
			this.RowCount = result;
			this.DialogResult = DialogResult.OK;
		}
	}
}
