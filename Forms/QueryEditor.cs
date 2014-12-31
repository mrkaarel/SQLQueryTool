using ScintillaNet;
using SqlQueryTool.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SqlQueryTool.Forms
{
	public partial class QueryEditor : UserControl
	{
		public delegate void RowUpdateHandler(IEnumerable<SqlCellValue> updateCells, SqlCellValue filterCell);
		public event RowUpdateHandler OnRowUpdate;

		public QueryEditor(string queryText)
		{
			InitializeComponent();

			this.splQuery.Panel2Collapsed = true;
			BuildScintillaEditor(queryText);
		}

		public string QueryText
		{
			get
			{
				return (this.splQuery.Panel1.Controls["txtQueryText"] as Scintilla).Text;
			}
		}

		private void BuildScintillaEditor(string queryText)
		{
			var txtQueryText = new Scintilla();
			txtQueryText.Name = "txtQueryText";
			txtQueryText.Text = queryText;
			txtQueryText.ConfigurationManager.CustomLocation = "ScintillaNET.xml";
			txtQueryText.ConfigurationManager.Language = "mssql";
			txtQueryText.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
			txtQueryText.Size = new Size(splQuery.Panel1.Width - 3, splQuery.Panel1.Height - 3);
			txtQueryText.Scrolling.HorizontalWidth = txtQueryText.Size.Width;
			txtQueryText.Scrolling.ScrollBars = ScrollBars.Both;

			splQuery.Panel1.Controls.Add(txtQueryText);
		}

		public void ShowResults(BindingSource bindingSource)
		{
			splQuery.Panel2Collapsed = false;

			dgResults.Columns.Clear();
			dgResults.AutoGenerateColumns = true;
			dgResults.SuspendLayout();
			dgResults.DataSource = bindingSource;
			dgResults.AutoResizeColumns();
			dgResults.ResumeLayout();
		}

		private void StartResultsTableDrag(DataGridView resultsTable)
		{
			if (!resultsTable.IsSelectionColumn()) {
				return;
			}

			var selectedCells = resultsTable.GetSelectedCells();
			var dragAndDropValues = selectedCells.OrderBy(c => c.RowIndex).Select(c => c.ToSqlCellValue()).ToList();

			resultsTable.DoDragDrop(dragAndDropValues, DragDropEffects.Copy);
		}

		private void DoListCopy(DataGridView resultsTable)
		{
			if (!resultsTable.IsSelectionColumn()) {
				return;
			}

			var selectedCells = resultsTable.GetSelectedCells().OrderBy(c => c.RowIndex).Select(c => c.ToSqlCellValue());
			string result = String.Join(", ", selectedCells.Select(c => c.SqlFormattedValue).ToArray());
			WinFormsHelper.CopyTextToClipboard(result);
		}

		private void dgResults_SelectionChanged(object sender, EventArgs e)
		{
			(sender as DataGridView).ToggleColumnNamesCopy();
		}

		private void dgResults_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyCode == Keys.C) {
				DoListCopy(sender as DataGridView);
				e.Handled = true;
			}
		}

		private void dgResults_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.C) {
				e.IsInputKey = true;
			}
		}

		private void dgResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (Control.ModifierKeys == Keys.Alt) {
				StartResultsTableDrag(sender as DataGridView);
			}
		}

		private void dgResults_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				var dataGrid = sender as DataGridView;
				var selectedCells = dataGrid.GetSelectedCells();

				if (selectedCells.Count() <= 1) {
					dataGrid.CurrentCell = dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
				}

				cmnQueryResultsCommands.Tag = dataGrid;
				cmnQueryResultsCommands.Show(dataGrid, dataGrid.PointToClient(Cursor.Position));
				mniCreateRowUpdateQuery.Enabled = dataGrid.IsSelectionRow();
			}
		}

		private void dgResults_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			//DO NOTHING (tekkis jama mingite pilditulpade kuvamisega. Las olla nii, kuni midagi paremat teha ei oska)
		}

		private void mniCreateRowUpdateQuery_Click(object sender, EventArgs e)
		{
			var updateCells = dgResults.GetSelectedCells().OrderBy(c => c.ColumnIndex).Select(c => c.ToSqlCellValue());
			var filterCell = dgResults.GetSelectedCells().First().OwningRow.Cells[0].ToSqlCellValue();

			OnRowUpdate(updateCells, filterCell);
		}
	}
}
