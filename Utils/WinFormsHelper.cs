using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SqlQueryTool.DatabaseObjects;

namespace SqlQueryTool.Utils
{
    public static class WinFormsHelper
    {
        public static void ToggleColumnNamesCopy(this DataGridView dataGridView)
        {
            if (dataGridView.IsSelectionColumn())
                dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            else
                dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
        }

        public static bool IsSelectionColumn(this DataGridView dataGridView)
        {
            var selectedCells = dataGridView.GetSelectedCells();

            if (selectedCells.Count() == 0) return false;

            var columnIndex = selectedCells.First().ColumnIndex;

            return selectedCells.All(c => c.ColumnIndex == columnIndex);
        }

        public static bool IsSelectionRow(this DataGridView dataGridView)
        {
            var selectedCells = dataGridView.GetSelectedCells();

            if (selectedCells.Count() == 0) return false;

            var rowIndex = selectedCells.First().RowIndex;

            return selectedCells.All(c => c.RowIndex == rowIndex);
        }

        public static IEnumerable<DataGridViewCell> GetSelectedCells(this DataGridView dataGridView)
        {
            return dataGridView.SelectedCells.Cast<DataGridViewCell>();
        }

        public static SqlCellValue ToSqlCellValue(this DataGridViewCell cell)
        {
            var value = cell.FormattedValue.ToString();
            var valueTypeString = cell.ValueType.ToString();

            var useQuotes = valueTypeString == "System.String" || valueTypeString == "System.DateTime" ||
                            valueTypeString == "System.Guid";
            if (valueTypeString == "System.Boolean") value = value == bool.TrueString ? "1" : "0";
            if (valueTypeString == "System.Decimal") value = value.Replace(",", ".");

            value = value.Replace("'", "''");
            var sqlFormattedValue = string.Format("{0}{1}{0}", useQuotes ? "'" : "", value);
            return new SqlCellValue(cell.OwningColumn.Name, value, sqlFormattedValue, cell.DataGridView.Tag as string);
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