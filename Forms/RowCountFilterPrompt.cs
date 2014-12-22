using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SqlQueryTool
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
