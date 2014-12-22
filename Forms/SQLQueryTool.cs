using ScintillaNet;
using SqlQueryTool.Connections;
using SqlQueryTool.DatabaseObjects;
using SqlQueryTool.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SqlQueryTool
{
	[System.Diagnostics.DebuggerDisplay("Form1")]
	public partial class SQLQueryTool : Form
	{
		private const string EMPTY_SEARCHBOX_TEXT = "otsi…";
		private int Settings_MinimumRowCount = 0;

		private ConnectionData currentConnectionData;
		private List<TableInfo> tables;
		private List<StoredProc> procs;
		private List<View> views;
		private IEnumerable<CommandParameter> lastParameterSet = new List<CommandParameter>();

		public SQLQueryTool()
		{
			InitializeComponent();
			BuildConnectionsDropdown(ConnectionDataStorage.LoadSavedSettings());
			ToggleCurrentQueryControls(tabQueries.TabPages.Count > 0);
		}

		private void AddNewQueryPage(string queryText, string tabName = "")
		{
			var tpQueryPage = new TabPage();
			if (String.IsNullOrEmpty(tabName)) {
				tpQueryPage.Text = String.Format("Päring {0}", tabQueries.TabPages.Count + 1);
			}
			else {
				tpQueryPage.Text = tabName;
			}
			tpQueryPage.ImageIndex = 0;

			SplitContainer splQuery = new SplitContainer();
			splQuery.Name = "splQuery";
			splQuery.Orientation = Orientation.Horizontal;
			splQuery.Dock = DockStyle.Fill;
			splQuery.SplitterWidth = 8;
			splQuery.Panel2Collapsed = true;

			Scintilla txtQueryText = new Scintilla();
			txtQueryText.Name = "txtQueryText";
			txtQueryText.Text = queryText;
			txtQueryText.ConfigurationManager.CustomLocation = "ScintillaNET.xml";
			txtQueryText.ConfigurationManager.Language = "mssql";
			txtQueryText.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
			txtQueryText.Size = new Size(splQuery.Panel1.Width - 3, splQuery.Panel1.Height - 3);
			txtQueryText.Scrolling.HorizontalWidth = txtQueryText.Size.Width;
			txtQueryText.Scrolling.ScrollBars = ScrollBars.Both;

			Panel pnlHelper = new Panel();
			pnlHelper.Name = "pnlHelper";
			pnlHelper.Dock = DockStyle.Fill;

			DataGridView dgResults = new DataGridView();
			dgResults.Name = "dgResults";
			dgResults.AllowUserToAddRows = false;
			dgResults.AllowUserToDeleteRows = false;
			dgResults.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
			dgResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgResults.ReadOnly = true;
			dgResults.RowHeadersVisible = false;
			dgResults.Dock = DockStyle.Fill;
			dgResults.ScrollBars = ScrollBars.Both;
			dgResults.SelectionChanged += new EventHandler(dgResults_SelectionChanged);
			dgResults.KeyDown += new KeyEventHandler(dgResults_KeyDown);
			dgResults.PreviewKeyDown += new PreviewKeyDownEventHandler(dgResults_PreviewKeyDown);
			dgResults.CellMouseDown += dgResults_CellMouseDown;
			dgResults.CellMouseClick += dgResults_CellMouseClick;
			dgResults.DataError += dgResults_DataError;

			splQuery.Panel1.Controls.Add(txtQueryText);

			pnlHelper.Controls.Add(dgResults);
			splQuery.Panel2.Controls.Add(pnlHelper);

			tpQueryPage.Controls.Add(splQuery);
			tabQueries.TabPages.Add(tpQueryPage);
			tabQueries.SelectedTab = tpQueryPage;

			ToggleCurrentQueryControls(tabQueries.TabPages.Count > 0);
		}

		private void StartResultsTableDrag(DataGridView resultsTable)
		{
			if (!resultsTable.IsSelectionColumn()) {
				return;
			}

			var selectedCells = resultsTable.GetSelectedCells();
			string columnName = selectedCells.First().OwningColumn.Name;
			var dragAndDropValues = selectedCells.OrderBy(c => c.RowIndex).Select(c => new DragDropCellValue() { ColumnName = columnName, Value = c.FormattedValue.ToString(), SqlFormattedValue = GetSQLFormattedValue(c) }).ToList();

			resultsTable.DoDragDrop(dragAndDropValues, DragDropEffects.Copy);
		}

		private void DoListCopy(DataGridView resultsTable)
		{
			if (!resultsTable.IsSelectionColumn()) {
				return;
			}

			var selectedCells = resultsTable.GetSelectedCells();
			string result = String.Join(", ", selectedCells.OrderBy(c => c.RowIndex).Select(GetSQLFormattedValue).ToArray());
			WinFormsHelper.CopyTextToClipboard(result);
		}

		private string GetSQLFormattedValue(DataGridViewCell cell)
		{
			string cellType = cell.ValueType.ToString();
			string value = cell.FormattedValue.ToString();

			bool useQuotes = (cellType == "System.String" || cellType == "System.DateTime" || cellType == "System.Guid");
			if (cellType == "System.Boolean") {
				value = (bool)cell.Value ? "1" : "0";
			}


			return String.Format("{0}{1}{0}", useQuotes ? "'" : "", value);
		}

		private string FormatSqlQuery(string queryText)
		{
			return queryText.Replace("\r", Environment.NewLine).Replace("   ", "\t");
		}

		private void ConnectToDatabase(ConnectionData connectionData)
		{
			try {
				currentConnectionData = connectionData;
				FillDatabaseObjects(connectionData);
				this.Text = String.Format("{0} - SQL Query Tool", connectionData);
				grpDatabaseObjects.Enabled = true;
				splMainContent.Panel2.Enabled = true;
				btnConnect.Enabled = false;
				lblStatusbarInfo.Text = String.Format("Ühendatud {0}@{1}", connectionData.DatabaseName, connectionData.ServerName);
			}
			catch (Exception ex) {
				MessageBox.Show(String.Format("Viga andmebaasiühendusega:\n{0}", ex.Message), "Viga andmebaasiühendusega", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		private void FillDatabaseObjects(ConnectionData connectionData)
		{
			tables = new List<TableInfo>();
			procs = new List<StoredProc>();
			views = new List<View>();

			using (var conn = connectionData.GetOpenConnection()) {
				var cmd = conn.CreateCommand();
				cmd.CommandText = QueryBuilder.SystemQueries.GetTableList();
				using (var rdr = cmd.ExecuteReader()) {
					while (rdr.Read()) {
						tables.Add(new TableInfo() { Name = rdr.GetString(0), RowCount = Int32.MaxValue });
					}
				}

				try {
					cmd.CommandText = QueryBuilder.SystemQueries.GetStoredProcList();
					using (var rdr = cmd.ExecuteReader()) {
						while (rdr.Read()) {
							string procName = rdr.GetString(0);
							var proc = procs.SingleOrDefault(p => p.Name == procName);
							if (proc == null) {
								proc = new StoredProc() { Name = procName, Content = "" };
								procs.Add(proc);
							}
							proc.Content += rdr.GetString(1);
						}
					}
				}
				catch (Exception) {
					MessageBox.Show("Probleem SP-de laadimisega", "Hoiatus", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}

				cmd.CommandText = QueryBuilder.SystemQueries.GetViewList();
				using (var rdr = cmd.ExecuteReader()) {
					while (rdr.Read()) {
						views.Add(new View() { Name = rdr.GetString(0), Definition = rdr.GetString(1) });
					}
				}
			}

			BuildVisibleDatabaseObjectList(txtSearch.Text);
		}

		private void AddRowCountsToTableInfos(DbConnection conn)
		{
			var cmd = conn.CreateCommand();
			cmd.CommandText = QueryBuilder.SystemQueries.GetTableRowCounts();

			using (var rdr = cmd.ExecuteReader()) {
				while (rdr.Read()) {
					var tableName = rdr.GetString(0).ToLower();
					var rowCount = rdr.GetInt64(1);

					tables.SingleOrDefault(t => t.Name == tableName).RowCount = rowCount;
				}
			}
		}

		private void BuildVisibleDatabaseObjectList(string filterText)
		{
			if (filterText == EMPTY_SEARCHBOX_TEXT) {
				filterText = String.Empty;
			}
			filterText = filterText.ToLower();

			trvDatabaseObjects.Nodes.Clear();

			if (tables.Count > 0) {
				var tablesRootNode = new TreeNode() { Name = "Tables", Text = "Tabelid", ContextMenuStrip = cmnTableCommandsGlobal };
				foreach (var tableInfo in tables.Where(t => t.Name.Contains(filterText) && t.RowCount >= Settings_MinimumRowCount)) {
					tablesRootNode.Nodes.Add(new TreeNode() { Name = tableInfo.Name, Text =  tableInfo.RowCount < Int32.MaxValue ? String.Format("{0} ({1})", tableInfo.Name, tableInfo.RowCount) : tableInfo.Name, ContextMenuStrip = cmnTableCommands });
				}
				trvDatabaseObjects.Nodes.Add(tablesRootNode);
			}

			if (procs.Count > 0) {
				var procsRootNode = new TreeNode() { Name = "Procs", Text = "Protseduurid" };
				foreach (var proc in procs.Where(p => p.Name.Contains(filterText) || (chkSearchSPContents.Checked && p.Content.ToLower().Contains(filterText)))) {
					procsRootNode.Nodes.Add(new TreeNode() { Name = proc.Name, Text = proc.Name, ContextMenuStrip = cmnStoredProcCommands });
				}
				trvDatabaseObjects.Nodes.Add(procsRootNode);
			}

			if (views.Count > 0) {
				var viewsRootNode = new TreeNode() { Name = "Views", Text = "Vaated" };
				foreach (var view in views.Where(v => v.Name.Contains(filterText) || (chkSearchSPContents.Checked && v.Definition.ToLower().Contains(filterText)))) {
					viewsRootNode.Nodes.Add(new TreeNode() { Name = view.Name, Text = view.Name, ContextMenuStrip = cmnViewCommands });
				}
				trvDatabaseObjects.Nodes.Add(viewsRootNode);
			}

			trvDatabaseObjects.Nodes.Cast<TreeNode>().Where(n => n.Parent == null && (n.Name == "Tables" || !String.IsNullOrEmpty(filterText))).ToList().ForEach(n => n.Expand());
			trvDatabaseObjects.SelectedNode = trvDatabaseObjects.Nodes["Tables"];
		}

		private void RunQuery(TabPage currentPage)
		{
			var splCurrentContainer = (SplitContainer)currentPage.Controls["splQuery"];
			string queryText = ((Scintilla)splCurrentContainer.Panel1.Controls["txtQueryText"]).Text;

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

					splCurrentContainer.Panel2Collapsed = false;
					BuildQueryResultsTable((splCurrentContainer.Panel2.Controls["pnlHelper"]).Controls["dgResults"] as DataGridView, new BindingSource() { DataSource = results });
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
		}

		private void BuildQueryResultsTable(DataGridView dgResults, BindingSource bindingSource)
		{
			dgResults.Columns.Clear();
			dgResults.AutoGenerateColumns = true;
			dgResults.SuspendLayout();
			dgResults.DataSource = bindingSource;
			dgResults.AutoResizeColumns();
			dgResults.ResumeLayout();
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

		private void BuildConnectionsDropdown(IEnumerable<ConnectionData> connections)
		{
			selPreviousConnections.Items.Clear();
			foreach (ConnectionData setting in connections) {
				selPreviousConnections.Items.Add(setting);
			}

			selPreviousConnections.Items.Insert(0, String.Empty);
			ToggleSelectedConnectionButtons();
		}

		private void ToggleSelectedConnectionButtons()
		{
			var enableButtons = selPreviousConnections.Items.Count > 0 && selPreviousConnections.SelectedItem != null && !String.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString());
			btnDeleteSelectedConnection.Enabled = enableButtons;
			btnConnect.Enabled = enableButtons;
		}

		private void HideTableFieldsOverview()
		{
			dgvTableFields.DataSource = null;
			lblTableFieldsTitle.Text = "Tabeli väljad:";
			lblTableFieldsTitle.Enabled = false;
			splDatabaseObjects.Panel2Collapsed = true;
		}

		private void BuildTableFieldsOverview(string tableName)
		{
			TableDefinition table = new TableDefinition(tableName, currentConnectionData);
			splDatabaseObjects.Panel2Collapsed = false;

			dgvTableFields.SuspendLayout();
			dgvTableFields.Rows.Clear();
			foreach (ColumnDefinition col in table.Columns) {
				string description = String.Format("{0}{1}{2}",
					!String.IsNullOrEmpty(col.DefaultValue) ? String.Format("Vaikeväärtus: {0}", col.DefaultValue) : String.Empty,
					(String.IsNullOrEmpty(col.DefaultValue) || String.IsNullOrEmpty(col.Description)) ? String.Empty : Environment.NewLine,
					col.Description
				);
				dgvTableFields.Rows.Add(col.Name, String.Format("{0} ({1}){2}", col.Type.Name, col.Length, col.IsNullable ? String.Empty : "*"), !String.IsNullOrEmpty(description) ? Resources.information : col.IsIdentity ? Resources.key : new Bitmap(16, 16));
				dgvTableFields.Rows[dgvTableFields.RowCount - 1].Cells["colDescription"].ToolTipText = description;
			}
			dgvTableFields.AutoResizeColumns();
			dgvTableFields.ResumeLayout();

			lblTableFieldsTitle.Text = String.Format("{0}:", trvDatabaseObjects.SelectedNode.Name);
			lblTableFieldsTitle.Enabled = true;
		}

		private void BuildSelectQueryTabPage(string tableName, QueryBuilder.TableSelectLimit selectLimit, string whereClause = "")
		{
			AddNewQueryPage(new TableDefinition(tableName, currentConnectionData).BuildSelectQuery(selectLimit, whereClause), tableName);
		}

		private void BuildStoredProcTabPage(string procName)
		{
			AddNewQueryPage(procs.Single(p => p.Name == procName).Content, procName);
		}

		private void BuildViewTabPage(string viewName)
		{
			AddNewQueryPage(views.Single(v => v.Name == viewName).Definition, viewName);
		}

		private void CloseQuery(TabPage targetTab)
		{
			int closedIndex = tabQueries.TabPages.IndexOf(targetTab);
			tabQueries.TabPages.Remove(targetTab);
			if (tabQueries.TabPages.Count > 0) {
				tabQueries.SelectedIndex = Math.Min(closedIndex, tabQueries.TabPages.Count - 1);
			}

			ToggleCurrentQueryControls(tabQueries.TabPages.Count > 0);
		}

		private void ToggleCurrentQueryControls(bool enabled)
		{
			btnDeleteQuery.Enabled = enabled;
			btnRunQuery.Enabled = enabled;
		}

		private void ToggleFieldAppearance(TextBox txtSearch)
		{
			if (String.IsNullOrEmpty(txtSearch.Text)) {
				txtSearch.BackColor = SystemColors.Window;
			}
			else {
				txtSearch.BackColor = Color.Gold;
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

		private void HideEmptyTables()
		{
			if (tables.All(t => t.RowCount == Int32.MaxValue)) {
				using (var conn = currentConnectionData.GetOpenConnection()) {
					AddRowCountsToTableInfos(conn);
				}
			}

			BuildVisibleDatabaseObjectList(txtSearch.Text);
			lblStatusbarInfo.Text = String.Format("Näitan {0} tabelit {1}st", trvDatabaseObjects.Nodes["Tables"].GetNodeCount(false), tables.Count);
		}

		private string BuildRowUpdateQuery(string tableName, IEnumerable<DataGridViewCell> selectedCells)
		{
			var queryText = new StringBuilder(String.Format("UPDATE {0}\t{1}{0}SET", Environment.NewLine, tableName));

			foreach (var cell in selectedCells.OrderBy(c => c.ColumnIndex)) {
				string columnName = cell.OwningColumn.Name;
				queryText.AppendFormat("{0}\t{1} = {2}, ", Environment.NewLine, columnName, GetSQLFormattedValue(cell));
			}
			queryText.Remove(queryText.Length - 2, 2);

			var firstCellInRow = selectedCells.First().OwningRow.Cells[0];

			queryText.AppendFormat("{0}WHERE{0}\t{1} = {2}", Environment.NewLine, firstCellInRow.OwningColumn.Name, GetSQLFormattedValue(firstCellInRow));

			return queryText.ToString();
		}

		#region EventHandlers

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.W)) {
				if (btnDeleteQuery.Enabled) {
					btnDeleteQuery.PerformClick(); ;
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

		private void btnConnect_Click(object sender, EventArgs e)
		{
			if (selPreviousConnections.SelectedItem != null && !String.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString())) {
				var connectionData = selPreviousConnections.SelectedItem as ConnectionData;
				ConnectionDataStorage.MoveFirst(connectionData);
				ConnectToDatabase(connectionData);
			}
		}

		private void btnRunQuery_Click(object sender, EventArgs e)
		{
			var currentPage = tabQueries.SelectedTab;
			if (currentPage == null) {
				return;
			}

			string queryText = ((currentPage.Controls["splQuery"] as SplitContainer).Panel1.Controls["txtQueryText"] as Scintilla).Text;
			if (!ConfirmQuery(queryText)) {
				return;
			}

			try {
				RunQuery(currentPage);
				if (QueryBuilder.IsStructureAlteringQuery(queryText)) {
					FillDatabaseObjects(currentConnectionData);
				}
			}
			catch (Exception ex) {
				lblStatusbarInfo.Text = "";
				MessageBox.Show(ex.Message, "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnDeleteSelectedConnection_Click(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString())) {
				var settings = ConnectionDataStorage.DeleteSetting(selPreviousConnections.SelectedItem as ConnectionData);
				BuildConnectionsDropdown(settings);
			}
		}

		private void btnAddConnection_Click(object sender, EventArgs e)
		{
			var connectionSettingsPrompt = new ConnectionSettings();
			if (connectionSettingsPrompt.ShowDialog() == DialogResult.OK) {
				var newConnectionData = connectionSettingsPrompt.ConnectionData;

				var settings = ConnectionDataStorage.AddSetting(newConnectionData);
				BuildConnectionsDropdown(settings);
				selPreviousConnections.SelectedIndex = 1;
				ToggleSelectedConnectionButtons();
				ConnectToDatabase(connectionSettingsPrompt.ConnectionData);
			}
		}

		private void selPreviousConnections_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToggleSelectedConnectionButtons();
		}

		private void trvDatabaseObjects_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (trvDatabaseObjects.SelectedNode != null && trvDatabaseObjects.SelectedNode.Parent != null && trvDatabaseObjects.SelectedNode.Parent.Name == "Tables") {
				BuildTableFieldsOverview(trvDatabaseObjects.SelectedNode.Name);
				trvDatabaseObjects.SelectedNode.EnsureVisible();
			}
			else {
				HideTableFieldsOverview();
			}
		}

		private void trvDatabaseObjects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			((TreeView)sender).SelectedNode = e.Node;
		}

		private void trvDatabaseObjects_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (trvDatabaseObjects.SelectedNode != null && trvDatabaseObjects.SelectedNode.Parent != null) {
				if (trvDatabaseObjects.SelectedNode.Parent.Name == "Tables") {
					BuildSelectQueryTabPage(trvDatabaseObjects.SelectedNode.Name, QueryBuilder.TableSelectLimit.None);
				}
				else if (trvDatabaseObjects.SelectedNode.Parent.Name == "Procs") {
					BuildStoredProcTabPage(trvDatabaseObjects.SelectedNode.Name);
				}
				else if (trvDatabaseObjects.SelectedNode.Parent.Name == "Views") {
					BuildSelectQueryTabPage(trvDatabaseObjects.SelectedNode.Name, QueryBuilder.TableSelectLimit.None);
				}
			}
		}

		private void btnAddQuery_Click(object sender, EventArgs e)
		{
			AddNewQueryPage(String.Empty);
		}

		private void btnDeleteQuery_Click(object sender, EventArgs e)
		{
			if (tabQueries.SelectedTab != null) {
				CloseQuery(tabQueries.SelectedTab);
			}
		}

		private void mniCloseTabpage_Click(object sender, EventArgs e)
		{
			if (tabQueries.Tag != null) {
				CloseQuery((TabPage)tabQueries.Tag);
			}
		}

		private void mniMoveTabLeft_Click(object sender, EventArgs e)
		{
			tabQueries.MoveTabPage(tabQueries.Tag as TabPage, MoveDirection.Left);
		}

		private void mniMoveTabRight_Click(object sender, EventArgs e)
		{
			tabQueries.MoveTabPage(tabQueries.Tag as TabPage, MoveDirection.Right);
		}

		private void tabQueries_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				for (int i = 0; i < tabQueries.TabCount; i++) {
					if (tabQueries.GetTabRect(i).Contains(e.Location)) {
						mniCloseTabpage.Text = String.Format("Sulge {0}", tabQueries.TabPages[i].Text);

						mniMoveTabLeft.Enabled = i > 0;
						mniMoveTabRight.Enabled = i < tabQueries.TabCount - 1;

						tabQueries.Tag = tabQueries.TabPages[i];
						cmnTabpage.Show(tabQueries, e.Location);

						return;
					}
				}

				tabQueries.Tag = null;
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			FillDatabaseObjects(currentConnectionData);
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			searchTimer.Stop();
			searchTimer.Start();
			ToggleFieldAppearance(txtSearch);
		}

		private void chkSearchSPContents_CheckedChanged(object sender, EventArgs e)
		{
			BuildVisibleDatabaseObjectList(txtSearch.Text);
		}

		private void txtSearch_Enter(object sender, EventArgs e)
		{
			if (txtSearch.Text == EMPTY_SEARCHBOX_TEXT) {
				txtSearch.Text = String.Empty;
				txtSearch.ForeColor = SystemColors.WindowText;
			}
		}

		private void txtSearch_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) {
				txtSearch.Text = String.Empty;
			}
		}

		private void mniShowViewDefinition_Click(object sender, EventArgs e)
		{
			BuildViewTabPage(trvDatabaseObjects.SelectedNode.Name);
		}

		private void mniCreateSelectQuery_Click(object sender, EventArgs e)
		{
			BuildSelectQueryTabPage(trvDatabaseObjects.SelectedNode.Name, QueryBuilder.TableSelectLimit.None);
		}

		private void mniSelectTopRows_Click(object sender, EventArgs e)
		{
			BuildSelectQueryTabPage(trvDatabaseObjects.SelectedNode.Name, QueryBuilder.TableSelectLimit.LimitTop);
		}

		private void mniSelectBottomRows_Click(object sender, EventArgs e)
		{
			BuildSelectQueryTabPage(trvDatabaseObjects.SelectedNode.Name, QueryBuilder.TableSelectLimit.LimitBottom);
		}

		private void mniSelectRowCount_Click(object sender, EventArgs e)
		{
			string tableName = trvDatabaseObjects.SelectedNode.Name;
			AddNewQueryPage(QueryBuilder.BuildSelectRowCountQuery(tableName));
		}

		private void mniCreateInsertQuery_Click(object sender, EventArgs e)
		{
			string tableName = trvDatabaseObjects.SelectedNode.Name;
			AddNewQueryPage(new TableDefinition(tableName, currentConnectionData).BuildInsertQuery(), String.Format("{0} (i)", tableName));
		}

		private void mniCreateDeleteQuery_Click(object sender, EventArgs e)
		{
			string tableName = trvDatabaseObjects.SelectedNode.Name;
			AddNewQueryPage(new TableDefinition(tableName, currentConnectionData).BuildDeleteQuery(), String.Format("{0} (d)", tableName));
		}

		private void mniCreateUpdateQuery_Click(object sender, EventArgs e)
		{
			string tableName = trvDatabaseObjects.SelectedNode.Name;
			AddNewQueryPage(new TableDefinition(tableName, currentConnectionData).BuildUpdateQuery(), String.Format("{0} (u)", tableName));
		}

		private void mniCopyStoredProcName_Click(object sender, EventArgs e)
		{
			WinFormsHelper.CopyTextToClipboard(trvDatabaseObjects.SelectedNode.Name);
		}

		private void mniGrantExecuteOnSP_Click(object sender, EventArgs e)
		{
			AddNewQueryPage(QueryBuilder.BuildGrantExecuteOnSP(trvDatabaseObjects.SelectedNode.Name));
		}

		private void mniShowTableRowCounts_Click(object sender, EventArgs e)
		{
			AddNewQueryPage(QueryBuilder.SystemQueries.GetTableRowCounts(), "Table row counts");
		}

		private void mniHideEmptyTables_Click(object sender, EventArgs e)
		{
			var prompt = new RowCountFilterPrompt(Settings_MinimumRowCount);
			prompt.RowCount = Settings_MinimumRowCount;
			if (prompt.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				Settings_MinimumRowCount = prompt.RowCount;
			}
			HideEmptyTables();
		}

		private void mniFindColumns_Click(object sender, EventArgs e)
		{
			AddNewQueryPage(QueryBuilder.SystemQueries.FindColumns(), "Find columns…");
		}

		private void txtSearch_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(typeof(List<DragDropCellValue>))) {
				e.Effect = DragDropEffects.Copy;
			}
			else {
				e.Effect = DragDropEffects.None;
			}
		}

		private void txtSearch_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text)) {
				txtSearch.Text = e.Data.GetData(DataFormats.Text).ToString();
			}
			if (e.Data.GetDataPresent(typeof(List<DragDropCellValue>))) {
				var values = e.Data.GetData(typeof(List<DragDropCellValue>)) as List<DragDropCellValue>;
				txtSearch.Text = values.First().Value;
			}
		}

		private void trvDatabaseObjects_DragOver(object sender, DragEventArgs e)
		{
			var node = trvDatabaseObjects.GetHoverNode(e.X, e.Y);

			if (node != null && node.Parent != null && node.Parent.Name == "Tables" && e.Data.GetDataPresent(typeof(List<DragDropCellValue>))) {
				e.Effect = DragDropEffects.Copy;
			}
			else {
				e.Effect = DragDropEffects.None;
			}
		}

		private void trvDatabaseObjects_DragDrop(object sender, DragEventArgs e)
		{
			var node = trvDatabaseObjects.GetHoverNode(e.X, e.Y);

			var values = e.Data.GetData(typeof(List<DragDropCellValue>)) as List<DragDropCellValue>;
			BuildSelectQueryTabPage(node.Name, QueryBuilder.TableSelectLimit.None, String.Format("{0} IN ({1})", values.First().ColumnName, String.Join(", ", values.Select(v => v.SqlFormattedValue).ToArray())));
		}

		private void mniCreateRowUpdateQuery_Click(object sender, EventArgs e)
		{
			string tableName = tabQueries.SelectedTab.Text;
			AddNewQueryPage(BuildRowUpdateQuery(tableName, (cmnQueryResultsCommands.Tag as DataGridView).GetSelectedCells()), String.Format("{0} (u)", tableName));
		}

		private void searchTimer_Tick(object sender, EventArgs e)
		{
			searchTimer.Stop();
			BuildVisibleDatabaseObjectList(txtSearch.Text);
		}

		private void dgResults_SelectionChanged(object sender, EventArgs e)
		{
			(sender as DataGridView).ToggleColumnNamesCopy();
		}

		private void dgResults_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.C) {
				e.IsInputKey = true;
			}
		}

		private void dgResults_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyCode == Keys.C) {
				DoListCopy(sender as DataGridView);
				e.Handled = true;
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

		private void dgResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (Control.ModifierKeys == Keys.Alt) {
				StartResultsTableDrag(sender as DataGridView);
			}
		}

		private void dgResults_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			//DO NOTHING (tekkis jama mingite pilditulpade kuvamisega. Las olla nii, kuni midagi paremat teha ei oska)
		}

		#endregion

		class StoredProc
		{
			public string Name { get; set; }
			public string Content { get; set; }
		}

		class View
		{
			public string Name { get; set; }
			public string Definition { get; set; }
		}

		class TableInfo
		{
			public string Name { get; set; }
			public long RowCount { get; set; }
		}

		class DragDropCellValue
		{
			public string ColumnName { get; set; }
			public string Value { get; set; }
			public string SqlFormattedValue { get; set; }
		}
	}
}