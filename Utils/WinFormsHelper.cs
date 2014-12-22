using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SqlQueryTool
{
	public static class WinFormsHelper
	{
		public static void MoveTabPage(this TabControl tabControl, TabPage tabPage, MoveDirection direction)
		{
			int selectedIndex = tabControl.SelectedIndex;

			for (int i = 0; i < tabControl.TabCount; i++) {
				if (tabControl.TabPages[i] == tabPage) {
					int newIdx = direction == MoveDirection.Left ? i - 1 : i + 1;

					tabControl.TabPages.Remove(tabPage);
					tabControl.TabPages.Insert(newIdx, tabPage);

					// keep active tab active
					if (selectedIndex == i) {
						tabControl.SelectedIndex = newIdx;
					}

					break;
				}
			}
		}

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
	
	public enum MoveDirection
	{
		Left,
		Right
	}
}
