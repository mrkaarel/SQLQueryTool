using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SqlQueryTool.Connections;
using SqlQueryTool.DatabaseObjects;
using SqlQueryTool.Properties;
using SqlQueryTool.Utils;

namespace SqlQueryTool.Forms
{
    public partial class DatabaseObjectBrowser : UserControl
    {
        public delegate void NewQueryHandler(string queryTitle, string queryText);

        public delegate void StatusBarTextChangeHandler(string newText);

        private readonly string EMPTY_SEARCHBOX_TEXT;
        private readonly string EMPTY_TABLE_FIELDS_TITLE;

        private ConnectionData currentConnectionData;
        private List<StoredProc> procs;
        private int Settings_MinimumRowCount;

        private List<TableInfo> tables;
        private List<View> views;

        public DatabaseObjectBrowser()
        {
            InitializeComponent();

            EMPTY_SEARCHBOX_TEXT = txtSearch.Text;
            EMPTY_TABLE_FIELDS_TITLE = lblTableFieldsTitle.Text;
        }

        public event NewQueryHandler OnNewQueryInitiated;
        public event StatusBarTextChangeHandler OnStatusBarTextChangeRequested;

        public void SetConnectionData(ConnectionData connectionData)
        {
            if (currentConnectionData != null && currentConnectionData.ToString() != connectionData.ToString())
                Settings_MinimumRowCount = 0;
            currentConnectionData = connectionData;
            LoadDatabaseObjects(currentConnectionData);
        }

        private void LoadDatabaseObjects(ConnectionData connectionData)
        {
            tables = new List<TableInfo>();
            procs = new List<StoredProc>();
            views = new List<View>();

            using (var conn = connectionData.GetOpenConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = QueryBuilder.SystemQueries.GetTableListWithRowCounts();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read()) tables.Add(new TableInfo {Schema = rdr.GetString(0), Name = rdr.GetString(1), Id = rdr.GetInt32(2), RowCount = rdr.GetInt64(3)});
                }

                tables = tables.OrderBy(t => t.NameWithNonDefaultSchema).ToList();

                try
                {
                    cmd.CommandText = QueryBuilder.SystemQueries.GetStoredProcList();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var procName = rdr.GetString(0);
                            var proc = procs.SingleOrDefault(p => p.Name == procName);
                            if (proc == null)
                            {
                                proc = new StoredProc {Name = procName, Content = ""};
                                procs.Add(proc);
                            }

                            proc.Content += rdr.GetString(1);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Problem loading stored procs", "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                cmd.CommandText = QueryBuilder.SystemQueries.GetViewList();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read()) views.Add(new View {Name = rdr.GetString(0), Definition = rdr.GetString(1)});
                }
            }

            BuildVisibleDatabaseObjectList(txtSearch.Text);
        }

        private void BuildVisibleDatabaseObjectList(string filterText)
        {
            if (filterText == EMPTY_SEARCHBOX_TEXT) filterText = string.Empty;
            filterText = filterText.ToLower();

            trvDatabaseObjects.Nodes.Clear();

            if (tables.Count > 0)
            {
                var tablesRootNode = new TreeNode
                {
                    Name = "Tables",
                    Text = Settings_MinimumRowCount == 0
                        ? "Tables"
                        : $"Tables (>= {Settings_MinimumRowCount} rows)",
                    ContextMenuStrip = cmnTableCommandsGlobal
                };
                foreach (var tableInfo in
                    tables.Where(t => t.NameWithNonDefaultSchema.ToLower().Contains(filterText) && t.RowCount >= Settings_MinimumRowCount))
                    tablesRootNode.Nodes.Add(new TreeNode
                    {
                        Name = tableInfo.NameWithNonDefaultSchema,
                        Text = tableInfo.RowCount < int.MaxValue
                            ? $"{tableInfo.NameWithNonDefaultSchema} ({tableInfo.RowCount})"
                            : tableInfo.NameWithNonDefaultSchema,
                        ContextMenuStrip = cmnTableCommands,
                        Tag = tableInfo.Id
                    });
                trvDatabaseObjects.Nodes.Add(tablesRootNode);
            }

            if (procs.Count > 0)
            {
                var procsRootNode = new TreeNode {Name = "Procs", Text = "Stored procedures"};
                foreach (var proc in procs.Where(p =>
                    p.Name.ToLower().Contains(filterText) ||
                    chkSearchSPContents.Checked && p.Content.ToLower().Contains(filterText)))
                    procsRootNode.Nodes.Add(new TreeNode
                    {
                        Name = proc.Name,
                        Text = proc.Name,
                        ContextMenuStrip = cmnStoredProcCommands
                    });
                trvDatabaseObjects.Nodes.Add(procsRootNode);
            }

            if (views.Count > 0)
            {
                var viewsRootNode = new TreeNode {Name = "Views", Text = "Views"};
                foreach (var view in views.Where(v =>
                    v.Name.ToLower().Contains(filterText) ||
                    chkSearchSPContents.Checked && v.Definition.ToLower().Contains(filterText)))
                    viewsRootNode.Nodes.Add(new TreeNode
                    {
                        Name = view.Name,
                        Text = view.Name,
                        ContextMenuStrip = cmnViewCommands
                    });
                trvDatabaseObjects.Nodes.Add(viewsRootNode);
            }

            trvDatabaseObjects.Nodes.Cast<TreeNode>()
                .Where(n => n.Parent == null && (n.Name == "Tables" || !string.IsNullOrEmpty(filterText))).ToList()
                .ForEach(n => n.Expand());
            trvDatabaseObjects.SelectedNode = trvDatabaseObjects.Nodes["Tables"];
        }

        private void BuildTableFieldsOverview(string tableName, long id)
        {
            var table = new TableDefinition(tableName, id, currentConnectionData);
            splDatabaseObjects.Panel2Collapsed = false;

            dgvTableFields.SuspendLayout();
            dgvTableFields.Rows.Clear();
            foreach (var col in table.Columns)
            {
                var descriptionParts = new string[3];
                descriptionParts[0] = !string.IsNullOrEmpty(col.DefaultValue) ? $"Default: {col.DefaultValue}" : string.Empty;
                descriptionParts[1] = col.Description;
                descriptionParts[2] = col.ForeignKey != null ? $"{col.ForeignKey.PrimaryTable}.{col.ForeignKey.PrimaryColumn}" : string.Empty;
                var description = string.Join(Environment.NewLine, descriptionParts.Where(s => !string.IsNullOrEmpty(s)).ToArray());
                
                dgvTableFields.Rows.Add(col.Name,
                    $"{col.Type.Name} ({col.Length}){(col.IsNullable ? string.Empty : "*")}",
                    col.IsIdentity ? Resources.key : 
                        col.ForeignKey != null ? Resources.key_silver :
                            !string.IsNullOrEmpty(description) ? Resources.information :
                                new Bitmap(16, 16));
                dgvTableFields.Rows[dgvTableFields.RowCount - 1].Cells["colDescription"].ToolTipText = description;
            }

            dgvTableFields.AutoResizeColumns();
            dgvTableFields.ResumeLayout();

            lblTableFieldsTitle.Text = $"{trvDatabaseObjects.SelectedNode.Name}:";
            lblTableFieldsTitle.Enabled = true;
        }

        private void HideTableFieldsOverview()
        {
            dgvTableFields.DataSource = null;
            lblTableFieldsTitle.Text = EMPTY_TABLE_FIELDS_TITLE;
            lblTableFieldsTitle.Enabled = false;
            splDatabaseObjects.Panel2Collapsed = true;
        }

        private void SetTableRowFilter()
        {
            var prompt = new RowCountFilterPrompt(Settings_MinimumRowCount);
            prompt.RowCount = Settings_MinimumRowCount;
            if (prompt.ShowDialog() == DialogResult.OK) Settings_MinimumRowCount = prompt.RowCount;

            BuildVisibleDatabaseObjectList(txtSearch.Text);
            OnStatusBarTextChangeRequested(
                $"Showing {trvDatabaseObjects.Nodes["Tables"].GetNodeCount(false)} tables out of {tables.Count}");
        }

        private void ToggleSearchFieldFilterView()
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
                txtSearch.BackColor = SystemColors.Window;
            else
                txtSearch.BackColor = Color.Gold;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
            ToggleSearchFieldFilterView();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == EMPTY_SEARCHBOX_TEXT)
            {
                txtSearch.Text = string.Empty;
                txtSearch.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) txtSearch.Text = string.Empty;
        }

        private void txtSearch_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(typeof(List<SqlCellValue>)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void txtSearch_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text)) txtSearch.Text = e.Data.GetData(DataFormats.Text).ToString();
            if (e.Data.GetDataPresent(typeof(List<SqlCellValue>)))
            {
                var values = e.Data.GetData(typeof(List<SqlCellValue>)) as List<SqlCellValue>;
                txtSearch.Text = values.First().Value;
            }
        }

        private void searchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            BuildVisibleDatabaseObjectList(txtSearch.Text);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDatabaseObjects(currentConnectionData);
        }

        private void chkSearchSPContents_CheckedChanged(object sender, EventArgs e)
        {
            BuildVisibleDatabaseObjectList(txtSearch.Text);
        }

        private void trvDatabaseObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (trvDatabaseObjects.SelectedNode != null && trvDatabaseObjects.SelectedNode.Parent != null &&
                trvDatabaseObjects.SelectedNode.Parent.Name == "Tables")
            {
                BuildTableFieldsOverview(trvDatabaseObjects.SelectedNode.Name, (long)trvDatabaseObjects.SelectedNode.Tag);
                trvDatabaseObjects.SelectedNode.EnsureVisible();
            }
            else
            {
                HideTableFieldsOverview();
            }
        }

        private void trvDatabaseObjects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((TreeView) sender).SelectedNode = e.Node;
        }

        private void trvDatabaseObjects_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (trvDatabaseObjects.SelectedNode != null && trvDatabaseObjects.SelectedNode.Parent != null)
            {
                var nodeName = trvDatabaseObjects.SelectedNode.Name;
                if (trvDatabaseObjects.SelectedNode.Parent.Name == "Tables")
                    OnNewQueryInitiated(nodeName,
                        new TableDefinition(nodeName, (long)trvDatabaseObjects.SelectedNode.Tag, currentConnectionData).BuildSelectQuery(QueryBuilder
                            .TableSelectLimit.None));
                else if (trvDatabaseObjects.SelectedNode.Parent.Name == "Procs")
                    OnNewQueryInitiated(nodeName, procs.Single(p => p.Name == nodeName).Content);
                else if (trvDatabaseObjects.SelectedNode.Parent.Name == "Views")
                    OnNewQueryInitiated(nodeName,
                        new TableDefinition(nodeName, (long)trvDatabaseObjects.SelectedNode.Tag, currentConnectionData).BuildSelectQuery(QueryBuilder
                            .TableSelectLimit.None));
            }
        }

        private void trvDatabaseObjects_DragOver(object sender, DragEventArgs e)
        {
            var node = trvDatabaseObjects.GetHoverNode(e.X, e.Y);

            if (node != null && node.Parent != null && node.Parent.Name == "Tables" &&
                e.Data.GetDataPresent(typeof(List<SqlCellValue>)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void trvDatabaseObjects_DragDrop(object sender, DragEventArgs e)
        {
            var node = trvDatabaseObjects.GetHoverNode(e.X, e.Y);

            var values = e.Data.GetData(typeof(List<SqlCellValue>)) as List<SqlCellValue>;
            var targetTable = new TableDefinition(node.Name, (long)node.Tag, currentConnectionData);
            var specimenValue = values.First();  
            
            OnNewQueryInitiated(node.Name,
                targetTable.BuildSelectQuery(
                    QueryBuilder.TableSelectLimit.None,
                    $"{GetColumnNameForDragDropSelectQuery(specimenValue.TableName, specimenValue.ColumnName, targetTable.Name)} IN ({string.Join(", ", values.Select(v => v.SqlFormattedValue).ToArray())})"));
        }

        private string GetColumnNameForDragDropSelectQuery(string sourceTable, string sourceColumn, string targetTable)
        {
            var foreignKeysToTable = ForeignKey.GetForeignKeysToTable(targetTable, currentConnectionData);
            var keyMatch = foreignKeysToTable.FirstOrDefault(k => k.ForeignTable == sourceTable && k.ForeignColumn == sourceColumn);
            if (keyMatch != null)
            {
                return keyMatch.PrimaryColumn;
            }
            
            var foreignKeysFromTable = ForeignKey.GetForeignKeysFromTable(targetTable, currentConnectionData);
            keyMatch = foreignKeysFromTable.FirstOrDefault(k => k.PrimaryTable == sourceTable && k.PrimaryColumn == sourceColumn);
            if (keyMatch != null)
            {
                return keyMatch.ForeignColumn;
            }

            return sourceColumn;
        }

        private void mniBuildTableRowCountsQuery_Click(object sender, EventArgs e)
        {
            OnNewQueryInitiated("Table row counts", QueryBuilder.SystemQueries.GetTableListWithRowCounts());
        }

        private void mniSetTableRowFilter_Click(object sender, EventArgs e)
        {
            SetTableRowFilter();
        }

        private void mniFindColumns_Click(object sender, EventArgs e)
        {
            OnNewQueryInitiated("Find columns…", QueryBuilder.SystemQueries.FindColumns());
        }

        private void mniSelectAllRows_Click(object sender, EventArgs e)
        {
            var tableName = trvDatabaseObjects.SelectedNode.Name;
            OnNewQueryInitiated(tableName,
                new TableDefinition(tableName, (long)trvDatabaseObjects.SelectedNode.Tag, currentConnectionData).BuildSelectQuery(QueryBuilder.TableSelectLimit
                    .None));
        }

        private void mniSelectRowCount_Click(object sender, EventArgs e)
        {
            var tableName = trvDatabaseObjects.SelectedNode.Name;
            OnNewQueryInitiated(tableName, QueryBuilder.BuildSelectRowCountQuery(tableName));
        }

        private void mniSelectTopRows_Click(object sender, EventArgs e)
        {
            var tableName = trvDatabaseObjects.SelectedNode.Name;
            OnNewQueryInitiated(tableName,
                new TableDefinition(tableName, (long)trvDatabaseObjects.SelectedNode.Tag, currentConnectionData).BuildSelectQuery(QueryBuilder.TableSelectLimit
                    .LimitTop));
        }

        private void mniSelectBottomRows_Click(object sender, EventArgs e)
        {
            var tableName = trvDatabaseObjects.SelectedNode.Name;
            OnNewQueryInitiated(tableName,
                new TableDefinition(tableName, (long)trvDatabaseObjects.SelectedNode.Tag, currentConnectionData).BuildSelectQuery(QueryBuilder.TableSelectLimit
                    .LimitBottom));
        }

        private void mniCreateInsertQuery_Click(object sender, EventArgs e)
        {
            var tableName = trvDatabaseObjects.SelectedNode.Name;
            OnNewQueryInitiated($"{tableName} (i)",
                new TableDefinition(tableName, (long)trvDatabaseObjects.SelectedNode.Tag, currentConnectionData).BuildInsertQuery());
        }

        private void mniCreateUpdateQuery_Click(object sender, EventArgs e)
        {
            var tableName = trvDatabaseObjects.SelectedNode.Name;
            OnNewQueryInitiated($"{tableName} (u)",
                new TableDefinition(tableName, (long)trvDatabaseObjects.SelectedNode.Tag, currentConnectionData).BuildUpdateQuery());
        }

        private void mniCreateDeleteQuery_Click(object sender, EventArgs e)
        {
            var tableName = trvDatabaseObjects.SelectedNode.Name;
            OnNewQueryInitiated($"{tableName} (d)",
                new TableDefinition(tableName, (long)trvDatabaseObjects.SelectedNode.Tag, currentConnectionData).BuildDeleteQuery());
        }

        private void mniShowViewDefinition_Click(object sender, EventArgs e)
        {
            var view = views.Single(v => v.Name == trvDatabaseObjects.SelectedNode.Name);
            OnNewQueryInitiated(view.Name, view.Definition);
        }

        private void mniCopyStoredProcName_Click(object sender, EventArgs e)
        {
            WinFormsHelper.CopyTextToClipboard(trvDatabaseObjects.SelectedNode.Name);
        }

        private void mniGrantExecuteOnSP_Click(object sender, EventArgs e)
        {
            var spName = trvDatabaseObjects.SelectedNode.Name;
            OnNewQueryInitiated("GRANT EXECUTE", QueryBuilder.BuildGrantExecuteOnSP(spName));
        }

        private class TableInfo
        {
            public long Id { get; set; }
            public string Schema { get; set; }
            public string Name { get; set; }
            public long RowCount { get; set; }
            public string NameWithNonDefaultSchema => Schema == "dbo" ? Name : $"{Schema}.{Name}";
        }

        private class StoredProc
        {
            public string Name { get; set; }
            public string Content { get; set; }
        }

        private class View
        {
            public string Name { get; set; }
            public string Definition { get; set; }
        }
    }
}