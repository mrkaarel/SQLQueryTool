using SqlQueryTool.Connections;
using SqlQueryTool.DatabaseObjects;
using SqlQueryTool.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace SqlQueryTool.Forms
{
	[System.Diagnostics.DebuggerDisplay("Form1")]
	public partial class SQLQueryTool : Form
	{
		private ConnectionData currentConnectionData;
		private IEnumerable<CommandParameter> lastParameterSet = new List<CommandParameter>();

		public SQLQueryTool()
		{
			InitializeComponent();

			connectionManager.OnConnectionInitiated += connectionManager_OnConnectionInitiated;
			databaseObjectBrowser.OnNewQueryInitiated += databaseObjectsViewer_OnNewQueryInitiated;
			databaseObjectBrowser.OnStatusBarTextChangeRequested += databaseObjectsViewer_OnStatusBarTextChangeRequested;

			// Added here because it cannot be added in design view
			grpQueries.MouseDoubleClick += grpQueries_MouseDoubleClick;
		}

		private void ConnectToDatabase(ConnectionData connectionData)
		{
			try {
				currentConnectionData = connectionData;
				databaseObjectBrowser.SetConnectionData(currentConnectionData);
				connectionManager.SetConnectionAchieved();

				grpDatabaseObjects.Enabled = true;
				splMainContent.Panel2.Enabled = true;

				lblStatusbarInfo.Text = String.Format("Ühendatud {0}@{1}", connectionData.DatabaseName, connectionData.ServerName);
				this.Text = String.Format("{0} - SQL Query Tool", connectionData);
			}
			catch (Exception ex) {
				MessageBox.Show(String.Format("Viga andmebaasiühendusega:\n{0}", ex.Message), "Viga andmebaasiühendusega", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void AddNewQueryPage(string queryText, string tabName = "")
		{
			var queryEditor = new QueryEditor(queryText) { Name = "queryEditor", Dock = DockStyle.Fill };
			queryEditor.OnRowUpdate += queryEditor_OnRowUpdate;

			tabName = String.IsNullOrEmpty(tabName) ? String.Format("Päring {0}", tabQueries.TabPages.Count + 1) : tabName;
			var tpQueryPage = new TabPage(tabName) { ImageIndex = 0 };

			tpQueryPage.Controls.Add(queryEditor);
			tabQueries.TabPages.Add(tpQueryPage);
			tabQueries.SelectedTab = tpQueryPage;
		}

		private void RunQuery(TabPage currentPage)
		{
			var queryEditor = currentPage.Controls["queryEditor"] as QueryEditor;
			string queryText = queryEditor.QueryText;

			if (!ConfirmQuery(queryText)) {
				return;
			}

			try {
				using (var conn = currentConnectionData.GetOpenConnection()) {
					var cmd = BuildCommand(conn, queryText);
					if (cmd == null) {
						return;
					}

					if (QueryBuilder.IsSelectQuery(cmd.CommandText)) {
						var stopWatch = Stopwatch.StartNew();

						var adapter = DbProviderFactories.GetFactory(currentConnectionData.ProviderName).CreateDataAdapter();
						adapter.SelectCommand = cmd;

						var results = new DataTable();
						adapter.Fill(results);

						stopWatch.Stop();
						decimal resultTime = Decimal.Round(stopWatch.ElapsedMilliseconds / 1000m, 1);
						lblStatusbarInfo.Text = String.Format("{0} kirjet ({1} sekundit; {2}, {3:HH:mm:ss})", results.Rows.Count, resultTime, currentPage.Text, DateTime.Now);

						queryEditor.ShowResults(new BindingSource() { DataSource = results });
					}
					else {
						int rowsAffected = cmd.ExecuteNonQuery();
						if (rowsAffected >= 0) {
							lblStatusbarInfo.Text = String.Format("{0} kirje{1} muudetud ({2}, {3:HH:mm:ss})", rowsAffected, rowsAffected == 1 ? "" : "t", currentPage.Text, DateTime.Now);
						}
						else {
							lblStatusbarInfo.Text = String.Format("Käskluse täitmine õnnestus ({0}, {1:HH:mm:ss})", currentPage.Text, DateTime.Now);
						}
					}
				}

				if (QueryBuilder.IsStructureAlteringQuery(queryText)) {
					databaseObjectBrowser.SetConnectionData(currentConnectionData);
				}
			}
			catch (Exception ex) {
				lblStatusbarInfo.Text = "";
				MessageBox.Show(ex.Message, "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool ConfirmQuery(string queryText)
		{
			queryText = queryText.ToUpper();
			if (QueryBuilder.IsDestroyQuery(queryText) && !queryText.Contains("WHERE")) {
				return (MessageBox.Show("UPDATE/DELETE päring ei sisalda WHERE-klauslit, kas soovid jätkata?", "Tähelepanu", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK);
			}
			else {
				return true;
			}
		}

		private DbCommand BuildCommand(DbConnection conn, string queryText)
		{
			var cmd = conn.CreateCommand();
			cmd.CommandText = queryText;
			cmd.CommandTimeout = ConnectionData.DefaultTimeout;

			if (QueryBuilder.IsCrudQuery(queryText) && CommandParameter.ParseParameterNames(queryText).Count() > 0) {
				var pvp = new ParameterValuesPrompt(CommandParameter.ParseParameterNames(queryText), lastParameterSet);
				if (pvp.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					foreach (var parm in pvp.Parameters) {
						var p = cmd.CreateParameter();
						p.ParameterName = parm.Name;
						p.Value = parm.Value;
						cmd.Parameters.Add(p);
					}
					lastParameterSet = pvp.Parameters;
				}
				else {
					return null;
				}
			}

			return cmd;
		}

		private void CloseQueryPage(TabPage targetTab)
		{
			int closedIndex = tabQueries.TabPages.IndexOf(targetTab);
			tabQueries.TabPages.Remove(targetTab);
			if (tabQueries.TabPages.Count > 0) {
				tabQueries.SelectedIndex = Math.Min(closedIndex, tabQueries.TabPages.Count - 1);
			}
		}

		#region EventHandlers

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.W)) {
				if (btnCloseQuery.Enabled) {
					btnCloseQuery.PerformClick(); ;
				}
				return true;
			}
			if (keyData == (Keys.Control | Keys.N)) {
				if (btnAddQuery.Enabled) {
					btnAddQuery.PerformClick();
				}
				return true;
			}
			if (keyData == Keys.F5) {
				if (btnRunQuery.Enabled) {
					btnRunQuery.PerformClick();
				}
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void btnAddQuery_Click(object sender, EventArgs e)
		{
			AddNewQueryPage(String.Empty);
		}

		private void btnCloseQuery_Click(object sender, EventArgs e)
		{
			if (tabQueries.SelectedTab != null) {
				CloseQueryPage(tabQueries.SelectedTab);
			}
		}

		private void grpQueries_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			btnAddQuery.PerformClick();
		}

		private void mniCloseTabpage_Click(object sender, EventArgs e)
		{
			if (tabQueries.Tag != null) {
				CloseQueryPage((TabPage)tabQueries.Tag);
			}
		}

		private void tabQueries_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				for (int i = 0; i < tabQueries.TabCount; i++) {
					if (tabQueries.GetTabRect(i).Contains(e.Location)) {
						mniCloseTabpage.Text = String.Format("Sulge {0}", tabQueries.TabPages[i].Text);

						tabQueries.Tag = tabQueries.TabPages[i];
						cmnTabpage.Show(tabQueries, e.Location);

						return;
					}
				}

				tabQueries.Tag = null;
			}
		}

		private void tabQueries_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var tabRenamePrompt = new TabRenamePrompt(tabQueries.SelectedTab.Text);

			if (tabRenamePrompt.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				tabQueries.SelectedTab.Text = tabRenamePrompt.TabText;
			}
		}

		private void tabQueries_TabCountChanged(object sender, ControlEventArgs e)
		{
			// The ControlRemoved event is fired before TabPage is actually removed
			// This workaround gives the expected TabPage count
			bool hasOpenTabs = tabQueries.Controls.OfType<TabPage>().Count() > 0;
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
			string tableName = tabQueries.SelectedTab.Text;
			AddNewQueryPage(QueryBuilder.BuildRowUpdateQuery(tableName, updateCells, filterCell), String.Format("{0} (u)", tableName));
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

		#endregion
	}
}