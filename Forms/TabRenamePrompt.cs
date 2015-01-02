using System;
using System.Windows.Forms;

namespace SqlQueryTool.Forms
{
	public partial class TabRenamePrompt : Form
	{
		public string TabText { get; set; }

		public TabRenamePrompt(string currentTabText)
		{
			InitializeComponent();

			this.txtTabText.Text = currentTabText;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.TabText = txtTabText.Text;
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}
}
