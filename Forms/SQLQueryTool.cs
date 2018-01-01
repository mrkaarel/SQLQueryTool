using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using SqlQueryTool.Connections;
using SqlQueryTool.DatabaseObjects;
using SqlQueryTool.Utils;

namespace SqlQueryTool.Forms
{
    [DebuggerDisplay("Form1")]
    public partial class SQLQueryTool : Form
    {
        private static readonly string AUTOSAVED_QUERIES_FILE_NAME = "AutoSavedQueries";
        private ConnectionData currentConnectionData;
        private IEnumerable<CommandParameter> lastParameterSet = new List<CommandParameter>();

        public SQLQueryTool()
        {
            InitializeComponent();

            connectionManager.OnConnectionInitiated += connectionManager_OnConnectionInitiated;
            databaseObjectBrowser.OnNewQueryInitiated += databaseObjectsViewer_OnNewQueryInitiated;
            databaseObjectBrowser.OnStatusBarTextChangeRequested +=
                databaseObjectsViewer_OnStatusBarTextChangeRequested;

            // Added here because it cannot be added in design view
            grpQueries.MouseDoubleClick += grpQueries_MouseDoubleClick;
        }

        private void ConnectToDatabase(ConnectionData connectionData)
        {
            try
            {
                currentConnectionData = connectionData;
                databaseObjectBrowser.SetConnectionData(currentConnectionData);
                connectionManager.SetConnectionAchieved();

                grpDatabaseObjects.Enabled = true;
                splMainContent.Panel2.Enabled = true;

                lblStatusbarInfo.Text = $"Connected to {connectionData.DatabaseName}@{connectionData.ServerName}";
                Text = $"{connectionData} - SQL Query Tool";

                RestoreAutoSavedQueries(currentConnectionData.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problem connecting to database:\n{ex.Message}", "Connection error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddNewQueryPage(string queryText, string tabName = "")
        {
            var queryEditor = new QueryEditor(queryText) {Name = "queryEditor", Dock = DockStyle.Fill};
            queryEditor.OnRowUpdate += queryEditor_OnRowUpdate;
            queryEditor.OnRowDelete += queryEditor_OnRowDelete;

            tabName = string.IsNullOrEmpty(tabName)
                ? $"Query {tabQueries.TabPages.Count + 1}"
                : tabName;
            var tpQueryPage = new TabPage(tabName) {ImageIndex = 0};

            tpQueryPage.Controls.Add(queryEditor);
            tabQueries.TabPages.Add(tpQueryPage);
            tabQueries.SelectedTab = tpQueryPage;
        }

        private void RunQuery(TabPage currentPage)
        {
            var queryEditor = currentPage.Controls["queryEditor"] as QueryEditor;
            var queryText = queryEditor.QueryText;

            if (string.IsNullOrEmpty(queryText)) return;
            if (!ConfirmQuery(queryText)) return;

            try
            {
                using (var conn = currentConnectionData.GetOpenConnection())
                {
                    var cmd = BuildCommand(conn, queryText);
                    if (cmd == null) return;

                    if (QueryBuilder.IsQueryReturningResults(cmd.CommandText))
                    {
                        var stopWatch = Stopwatch.StartNew();

                        var adapter = DbProviderFactories.GetFactory(currentConnectionData.ProviderName)
                            .CreateDataAdapter();
                        adapter.SelectCommand = cmd;

                        var results = new DataTable();
                        adapter.Fill(results);

                        stopWatch.Stop();
                        var resultTime = decimal.Round(stopWatch.ElapsedMilliseconds / 1000m, 1);
                        lblStatusbarInfo.Text =
                            $"{results.Rows.Count} rows ({resultTime} seconds; {currentPage.Text}, {DateTime.Now:HH:mm:ss})";

                        queryEditor.ShowResults(new BindingSource {DataSource = results});
                    }
                    else
                    {
                        var rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected >= 0)
                            lblStatusbarInfo.Text =
                                $"{rowsAffected} row{(rowsAffected == 1 ? "" : "s")} modified ({currentPage.Text}, {DateTime.Now:HH:mm:ss})";
                        else
                            lblStatusbarInfo.Text = $"Command executed ({currentPage.Text}, {DateTime.Now:HH:mm:ss})";
                    }
                }

                if (QueryBuilder.IsStructureAlteringQuery(queryText))
                    databaseObjectBrowser.SetConnectionData(currentConnectionData);
            }
            catch (Exception ex)
            {
                lblStatusbarInfo.Text = "";
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ConfirmQuery(string queryText)
        {
            queryText = queryText.ToUpper();
            if (QueryBuilder.IsDestroyQuery(queryText) && !queryText.Contains("WHERE"))
                return MessageBox.Show("UPDATE/DELETE query without a WHERE clause, do you wish to proceed?", "Warning",
                           MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK;
            return true;
        }

        private DbCommand BuildCommand(DbConnection conn, string queryText)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = queryText;
            cmd.CommandTimeout = ConnectionData.DefaultTimeout;

            if (QueryBuilder.IsCrudQuery(queryText) && CommandParameter.ParseParameterNames(queryText).Count() > 0)
            {
                var pvp = new ParameterValuesPrompt(CommandParameter.ParseParameterNames(queryText), lastParameterSet);
                if (pvp.ShowDialog() == DialogResult.OK)
                {
                    foreach (var parm in pvp.Parameters)
                    {
                        var p = cmd.CreateParameter();
                        p.ParameterName = parm.Name;
                        p.Value = parm.Value;
                        cmd.Parameters.Add(p);
                    }

                    lastParameterSet = pvp.Parameters;
                }
                else
                {
                    return null;
                }
            }

            return cmd;
        }

        private void CloseQueryPage(TabPage targetTab)
        {
            var closedIndex = tabQueries.TabPages.IndexOf(targetTab);
            tabQueries.TabPages.Remove(targetTab);
            if (tabQueries.TabPages.Count > 0)
                tabQueries.SelectedIndex = Math.Min(closedIndex, tabQueries.TabPages.Count - 1);
        }

        private void SaveOpenQueries(string connectionData)
        {
            var currentQueries = new List<QueryItem>();

            foreach (TabPage tabPage in tabQueries.TabPages)
            {
                var name = tabPage.Text;
                var contents = (tabPage.Controls["queryEditor"] as QueryEditor).QueryText;

                currentQueries.Add(new QueryItem(name, contents, currentConnectionData.ToString()));
            }

            var savedQueries =
                (ProtectedDataStorage.Read(AUTOSAVED_QUERIES_FILE_NAME) as List<QueryItem> ?? new List<QueryItem>())
                .Where(q => q.Connection != connectionData).ToList();
            savedQueries.AddRange(currentQueries);

            ProtectedDataStorage.Write(AUTOSAVED_QUERIES_FILE_NAME, savedQueries);
        }

        private void RestoreAutoSavedQueries(string connectionData)
        {
            var queries = ProtectedDataStorage.Read(AUTOSAVED_QUERIES_FILE_NAME) as List<QueryItem>;

            if (queries == null) return;

            foreach (var query in queries.Where(q => q.Connection == connectionData))
                AddNewQueryPage(query.Contents, query.Name);

            ProtectedDataStorage.Write(AUTOSAVED_QUERIES_FILE_NAME,
                queries.Where(q => q.Connection != connectionData).ToList());
        }

        [Serializable]
        public struct QueryItem
        {
            public string Name { get; }
            public string Contents { get; }
            public string Connection { get; set; }

            public QueryItem(string name, string contents, string connection) : this()
            {
                Name = name;
                Contents = contents;
                Connection = connection;
            }
        }

        #region EventHandlers

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.W))
            {
                if (btnCloseQuery.Enabled)
                {
                    btnCloseQuery.PerformClick();
                    ;
                }

                return true;
            }

            if (keyData == (Keys.Control | Keys.N))
            {
                if (btnAddQuery.Enabled) btnAddQuery.PerformClick();
                return true;
            }

            if (keyData == Keys.F5)
                if (btnRunQuery.Enabled)
                    btnRunQuery.PerformClick();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnAddQuery_Click(object sender, EventArgs e)
        {
            AddNewQueryPage(string.Empty);
        }

        private void btnCloseQuery_Click(object sender, EventArgs e)
        {
            if (tabQueries.SelectedTab != null) CloseQueryPage(tabQueries.SelectedTab);
        }

        private void grpQueries_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnAddQuery.PerformClick();
        }

        private void mniCloseTabpage_Click(object sender, EventArgs e)
        {
            if (tabQueries.Tag != null) CloseQueryPage((TabPage) tabQueries.Tag);
        }

        private void tabQueries_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (var i = 0; i < tabQueries.TabCount; i++)
                    if (tabQueries.GetTabRect(i).Contains(e.Location))
                    {
                        mniCloseTabpage.Text = $"Close {tabQueries.TabPages[i].Text}";

                        tabQueries.Tag = tabQueries.TabPages[i];
                        cmnTabpage.Show(tabQueries, e.Location);

                        return;
                    }

                tabQueries.Tag = null;
            }
        }

        private void tabQueries_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var tabRenamePrompt = new TabRenamePrompt(tabQueries.SelectedTab.Text);

            if (tabRenamePrompt.ShowDialog() == DialogResult.OK) tabQueries.SelectedTab.Text = tabRenamePrompt.TabText;
        }

        private void tabQueries_TabCountChanged(object sender, ControlEventArgs e)
        {
            // The ControlRemoved event is fired before TabPage is actually removed
            // This workaround gives the expected TabPage count
            var hasOpenTabs = tabQueries.Controls.OfType<TabPage>().Count() > 0;
            btnCloseQuery.Enabled = hasOpenTabs;
            btnRunQuery.Enabled = hasOpenTabs;
        }

        private void btnRunQuery_Click(object sender, EventArgs e)
        {
            var currentPage = tabQueries.SelectedTab;
            RunQuery(currentPage);
        }

        private void queryEditor_OnRowUpdate(IEnumerable<SqlCellValue> updateCells, SqlCellValue filterCell)
        {
            var tableName = tabQueries.SelectedTab.Text;
            AddNewQueryPage(QueryBuilder.BuildRowUpdateQuery(tableName, updateCells, filterCell),
                $"{tableName} (u)");
        }

        private void queryEditor_OnRowDelete(IEnumerable<SqlCellValue> filterCells,
            QueryBuilder.SelectionShape selectionShape)
        {
            var tableName = tabQueries.SelectedTab.Text;
            AddNewQueryPage(QueryBuilder.BuildRowDeleteQuery(tableName, filterCells, selectionShape),
                $"{tableName} (d)");
        }

        private void connectionManager_OnConnectionInitiated(ConnectionData connectionData)
        {
            ConnectToDatabase(connectionData);
        }

        private void databaseObjectsViewer_OnNewQueryInitiated(string queryTitle, string queryText)
        {
            AddNewQueryPage(queryText, queryTitle);
        }

        private void databaseObjectsViewer_OnStatusBarTextChangeRequested(string newText)
        {
            lblStatusbarInfo.Text = newText;
        }

        private void SQLQueryTool_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (currentConnectionData != null) SaveOpenQueries(currentConnectionData.ToString());
        }

        #endregion
    }
}