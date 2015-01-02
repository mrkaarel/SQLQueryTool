using SqlQueryTool.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SqlQueryTool.Utils
{
	public static class WinFormsHelper
	{
		public static void ToggleColumnNamesCopy(this DataGridView dataGridView)
		{
			if (dataGridView.IsSelectionColumn()) {
				dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			}
			else {
				dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
			}
		}

		public static bool IsSelectionColumn(this DataGridView dataGridView)
		{
			var selectedCells = dataGridView.GetSelectedCells();

			if (selectedCells.Count() == 0) {
				return false;
			}

			int columnIndex = selectedCells.First().ColumnIndex;

			return selectedCells.All(c => c.ColumnIndex == columnIndex);
		}

		public static bool IsSelectionRow(this DataGridView dataGridView)
		{
			var selectedCells = dataGridView.GetSelectedCells();

			if (selectedCells.Count() == 0) {
				return false;
			}

			int rowIndex = selectedCells.First().RowIndex;

			return selectedCells.All(c => c.RowIndex == rowIndex);
		}

		public static IEnumerable<DataGridViewCell> GetSelectedCells(this DataGridView dataGridView)
		{
			return dataGridView.SelectedCells.Cast<DataGridViewCell>();
		}

		public static SqlCellValue ToSqlCellValue(this DataGridViewCell cell)
		{
			string value = cell.FormattedValue.ToString();
			string valueTypeString = cell.ValueType.ToString();

			bool useQuotes = (valueTypeString == "System.String" || valueTypeString == "System.DateTime" || valueTypeString == "System.Guid");
			if (valueTypeString == "System.Boolean") {
				value = value == Boolean.TrueString ? "1" : "0";
			}
			string sqlFormattedValue = String.Format("{0}{1}{0}", useQuotes ? "'" : "", value);

			return new SqlCellValue(cell.OwningColumn.Name, value, sqlFormattedValue);
		}

		public static TreeNode GetHoverNode(this TreeView treeView, int x, int y)
		{
			var pos = treeView.PointToClient(new Point(x, y));
			var hit = treeView.HitTest(pos);

			return hit.Node;
		}

		public static void CopyTextToClipboard(string text)
		{
			Clipboard.SetText(text);
		}
	}
}
