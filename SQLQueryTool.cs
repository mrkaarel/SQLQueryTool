using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScintillaNet;
using SqlQueryTool.Properties;

namespace SqlQueryTool
{
	[System.Diagnostics.DebuggerDisplay("Form1")]
	public partial class SQLQueryTool : Form
	{
		private const string EMPTY_SEARCHBOX_TEXT = "otsi...";
		private int Settings_MinimumRowCount = 0;

		private ConnectionData currentConnectionData;
		private List<TableInfo> tableNames;
		private List<StoredProc> procNames;

		public SQLQueryTool()
		{
			InitializeComponent();
			FillPreviousSettings();
			ToggleCurrentQueryControls(tabQueries.TabPages.Count > 0);
		}

		private void AddNewQueryPage(string queryText)
		{
			AddNewQueryPage(queryText, String.Empty);
		}

		private void AddNewQueryPage(string queryText, string tabName)
		{
			TabPage tpQueryPage = new TabPage();
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
			dgResults.DataError += dgResults_DataError;

			splQuery.Panel1.Controls.Add(txtQueryText);

			pnlHelper.Controls.Add(dgResults);
			splQuery.Panel2.Controls.Add(pnlHelper);

			tpQueryPage.Controls.Add(splQuery);
			tabQueries.TabPages.Add(tpQueryPage);
			tabQueries.SelectedTab = tpQueryPage;

			ToggleCurrentQueryControls(tabQueries.TabPages.Count > 0);
		}

		private void ToggleDatagridHeaderCopy(DataGridView datagrid)
		{
			if (datagrid.SelectedCells.Count == 1) {
				datagrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			}
			else {
				datagrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
			}
		}

		private void DoListCopy(IEnumerable<DataGridViewCell> selectedCells)
		{
			if (selectedCells.Count() == 0) {
				return;
			}

			int columnIndex = selectedCells.First().ColumnIndex;
			if (selectedCells.Any(c => c.ColumnIndex != columnIndex)) {
				return;
			}

			Type columnType = selectedCells.First().ValueType;
			bool useQuotes = false;
			if (columnType.ToString() == "System.String" || columnType.ToString() == "System.DateTime") {
				useQuotes = true;
			}

			string result = String.Join(", ", selectedCells.OrderBy(c => c.RowIndex).Select(c => String.Format("{0}{1}{0}", useQuotes ? "'" : "", c.FormattedValue)).ToArray());
			System.Windows.Forms.Clipboard.SetDataObject(result);
		}

		private string FormatSqlQuery(string queryText)
		{
			return queryText.Replace("\r", Environment.NewLine).Replace("   ", "\t");
		}

		private void ConnectToDatabase(ConnectionData connectionData)
		{
			try {
				currentConnectionData = connectionData;
				using (SqlConnection conn = Helper.OpenConnection(connectionData)) {
					FillDatabaseObjects(conn);
				}
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

		private void FillDatabaseObjects(SqlConnection conn)
		{
			tableNames = new List<TableInfo>();
			procNames = new List<StoredProc>();

			SqlCommand cmd = new SqlCommand("SELECT LOWER(name) FROM [sysobjects] WHERE xtype = 'u' AND uid = 1 ORDER BY name", conn);
			using (SqlDataReader rdr = cmd.ExecuteReader()) {
				while (rdr.Read()) {
					tableNames.Add(new TableInfo() { Name = rdr.GetString(0), RowCount = Int32.MaxValue });
				}
			}

			try {
				cmd.CommandText = "SELECT LOWER(so.name), LOWER(sc.text) FROM sysobjects so JOIN syscomments sc ON (sc.id = so.id) WHERE so.type ='P' AND so.category = 0 ORDER BY so.name";
				using (SqlDataReader rdr = cmd.ExecuteReader()) {
					while (rdr.Read()) {
						procNames.Add(new StoredProc() { Name = rdr.GetString(0), Content = rdr.GetString(1) });
					}
				}
			}
			catch (Exception) {
				MessageBox.Show("Probleem SP-de laadimisega", "Hoiatus", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			BuildVisibleDatabaseObjectList(txtSearch.Text);
		}

		private void AddRowCountsToTableInfos(SqlConnection conn)
		{
			var cmd = conn.CreateCommand();
			cmd.CommandText = "SELECT LOWER(t.name), p.rows FROM sys.tables t INNER JOIN sys.indexes i ON t.object_id = i.object_id INNER JOIN sys.partitions p on i.object_id = p.object_id and i.index_id = p.index_id WHERE t.is_ms_shipped = 0 GROUP BY t.name, p.rows ORDER BY t.name";

			using (var rdr = cmd.ExecuteReader()) {
				while (rdr.Read()) {
					var tableName = rdr.GetString(0);
					var rowCount = rdr.GetInt64(1);

					tableNames.SingleOrDefault(t => t.Name == tableName).RowCount = rowCount;
				}
			}
		}

		private void BuildVisibleDatabaseObjectList(string filterText)
		{
			trvDatabaseObjects.Nodes.Clear();
			trvDatabaseObjects.Nodes.Add("Tables", "Tabelid");
			trvDatabaseObjects.Nodes.Add("Procs", "Protseduurid");

			if (filterText == EMPTY_SEARCHBOX_TEXT) {
				filterText = String.Empty;
			}
			filterText = filterText.ToLower();

			foreach (var tableInfo in tableNames.Where(t => t.Name.Contains(filterText) && t.RowCount >= Settings_MinimumRowCount)) {
				trvDatabaseObjects.Nodes["Tables"].Nodes.Add(tableInfo.Name, tableInfo.Name);
			}
			foreach (var proc in procNames.Where(p => p.Name.Contains(filterText) || (chkSearchSPContents.Checked && p.Content.Contains(filterText))).Select(p => p)) {
				trvDatabaseObjects.Nodes["Procs"].Nodes.Add(proc.Name, proc.Name);
			}

			if (!String.IsNullOrEmpty(filterText)) {
				trvDatabaseObjects.Nodes["Tables"].Expand();
				trvDatabaseObjects.Nodes["Procs"].Expand();
			}

			trvDatabaseObjects.SelectedNode = trvDatabaseObjects.Nodes["Tables"];
		}

		private void RunQuery(TabPage currentPage)
		{
			using (SqlConnection conn = Helper.OpenConnection(currentConnectionData)) {
				SplitContainer splCurrentContainer = (SplitContainer)currentPage.Controls["splQuery"];
				string queryText = ((Scintilla)splCurrentContainer.Panel1.Controls["txtQueryText"]).Text;

				SqlCommand cmd = new SqlCommand(queryText, conn);
				cmd.CommandTimeout = ConnectionData.DefaultTimeout;
				if (queryText.TrimStart().ToUpper().StartsWith("SELECT")) {
					DataGridView dgResults = (DataGridView)((Panel)splCurrentContainer.Panel2.Controls["pnlHelper"]).Controls["dgResults"];

					BindingSource bindingSource = new BindingSource();
					DataTable results = new System.Data.DataTable();

					lblStatusbarInfo.Text = "Sooritan päringut...";

					DateTime start = DateTime.Now;
					SqlDataAdapter adapter = new SqlDataAdapter(cmd);
					adapter.Fill(results);
					int resultTime = DateTime.Now.Subtract(start).Milliseconds + (DateTime.Now.Subtract(start).Seconds * 1000) + (DateTime.Now.Subtract(start).Minutes * 60000);
					lblStatusbarInfo.Text = String.Format("{0} kirjet ({1} sekundit; {2}, {3:HH:mm:ss})", results.Rows.Count, (decimal)resultTime / 1000m, currentPage.Text, DateTime.Now);

					splCurrentContainer.Panel2Collapsed = false;

					bindingSource.DataSource = results;
					dgResults.Columns.Clear();
					dgResults.AutoGenerateColumns = true;
					dgResults.SuspendLayout();
					dgResults.DataSource = bindingSource;
					dgResults.AutoResizeColumns();
					dgResults.ResumeLayout();
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

		private void FillPreviousSettings()
		{
			List<ConnectionData> previousSettings = ConnectionData.LoadSavedSettings();

			selPreviousConnections.Items.Clear();
			foreach (ConnectionData setting in previousSettings) {
				selPreviousConnections.Items.Add(setting);
			}
			selPreviousConnections.Items.Insert(0, String.Empty);
			ToggleSelectedConnectionButtons();
		}

		private void SelectSettingInListbox(ConnectionData setting)
		{
			for (int i = 0; i < selPreviousConnections.Items.Count; i++) {
				Object item = selPreviousConnections.Items[i];
				if (!String.IsNullOrEmpty(item.ToString()) && ((ConnectionData)item).SettingString == setting.SettingString) {
					selPreviousConnections.SelectedIndex = i;
					ToggleSelectedConnectionButtons();
					break;
				}
			}
		}

		private void ToggleSelectedConnectionButtons()
		{
			btnDeleteSelectedConnection.Enabled = selPreviousConnections.Items.Count > 0 && selPreviousConnections.SelectedItem != null && !String.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString());
			btnConnect.Enabled = selPreviousConnections.Items.Count > 0 && selPreviousConnections.SelectedItem != null && !String.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString());
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
			Table table = new Table(tableName, currentConnectionData);
			splDatabaseObjects.Panel2Collapsed = false;

			dgvTableFields.SuspendLayout();
			dgvTableFields.Rows.Clear();
			foreach (Column col in table.Columns) {
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

		private void BuildInsertQueryTabPage(string tableName)
		{
			Table table = new Table(tableName, currentConnectionData);
			StringBuilder queryText = new StringBuilder(String.Format("INSERT INTO {0}{1}\t(", table.Name, Environment.NewLine));
			foreach (Column col in table.Columns) {
				if (!col.IsIdentity) {
					queryText.AppendFormat("{0}, ", col.Name);
				}
			}
			queryText.Remove(queryText.Length - 2, 2);
			queryText.AppendFormat("){0}VALUES{0}\t(", Environment.NewLine);
			foreach (Column col in table.Columns) {
				if (!col.IsIdentity) {
					queryText.AppendFormat("{0}, ", col.FormattedValue);
				}
			}
			queryText.Remove(queryText.Length - 2, 2);
			queryText.Append(")");

			AddNewQueryPage(queryText.ToString(), String.Format("{0} (i)", tableName));
		}

		private void BuildUpdateQueryTabPage(string tableName)
		{
			Table table = new Table(tableName, currentConnectionData);
			StringBuilder queryText = new StringBuilder(String.Format("UPDATE {0}\t{1}{0}SET", Environment.NewLine, table.Name));
			foreach (Column col in table.Columns) {
				if (!col.IsIdentity) {
					queryText.AppendFormat("{0}\t{1} = {2}, ", Environment.NewLine, col.Name, col.FormattedValue);
				}
			}
			queryText.Remove(queryText.Length - 2, 2);
			queryText.AppendFormat("{0}WHERE{0}\t{1}", Environment.NewLine, GetWhereClause(table.Columns));

			AddNewQueryPage(queryText.ToString(), String.Format("{0} (u)", tableName));
		}

		private void BuildDeleteQueryTabPage(string tableName)
		{
			Table table = new Table(tableName, currentConnectionData);
			StringBuilder queryText = new StringBuilder(String.Format("DELETE FROM {0}\t{1}{0}WHERE{0}\t{2}", Environment.NewLine, table.Name, GetWhereClause(table.Columns)));
			AddNewQueryPage(queryText.ToString(), String.Format("{0} (d)", tableName));
		}

		private static string GetWhereClause(ColumnCollection columns)
		{
			if (columns.IdentityColumn != null) {
				return String.Format("{0} = ?", columns.IdentityColumn.Name);
			}
			else {
				return "?";
			}
		}

		private void BuildSelectQueryTabPage(string tableName, TableSelectLimit selectLimit)
		{
			Table table = new Table(tableName, currentConnectionData);
			StringBuilder queryText = new StringBuilder("SELECT ");
			if (selectLimit != TableSelectLimit.None) {
				queryText.Append("TOP 100 ");
			}
			foreach (Column col in table.Columns) {
				queryText.AppendFormat("{0}\t{1}, ", Environment.NewLine, col.Name); ;
			}
			queryText.Remove(queryText.Length - 2, 2);
			queryText.AppendFormat("{0}FROM{0}\t{1}", Environment.NewLine, tableName);
			if (selectLimit == TableSelectLimit.LimitBottom) {
				queryText.AppendFormat("{0}ORDER BY", Environment.NewLine);
				if (table.Columns.IdentityColumn != null) {
					queryText.AppendFormat("{0}\t{1} DESC", Environment.NewLine, table.Columns.IdentityColumn.Name);
				}
				else {
					queryText.AppendFormat("{0}\t? DESC", Environment.NewLine);
				}
			}

			AddNewQueryPage(queryText.ToString(), tableName);
		}

		private void BuildStoredProcTabPage(string procName)
		{
			procName = procName.Replace("'", "''");
			using (SqlConnection conn = Helper.OpenConnection(currentConnectionData)) {
				SqlCommand cmd = new SqlCommand(String.Format(@"SELECT sc.text FROM syscomments sc JOIN sysobjects so ON (sc.id = so.id) WHERE so.type ='P' AND so.category = 0 AND so.name = '{0}'", procName), conn);

				AddNewQueryPage((string)cmd.ExecuteScalar(), procName);
			}
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
			if ((queryText.StartsWith("DELETE") || queryText.StartsWith("UPDATE")) && !queryText.Contains("WHERE")) {
				return (MessageBox.Show("UPDATE/DELETE päring ei sisalda WHERE-klauslit, kas soovid jätkata?", "Tähelepanu", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK);
			}
			else {
				return true;
			}
		}

		private string GetQueryText(TabPage tabPage)
		{
			return ((Scintilla)((SplitContainer)tabPage.Controls["splQuery"]).Panel1.Controls["txtQueryText"]).Text;
		}

		private void HideEmptyTables()
		{
			if (Settings_MinimumRowCount > 0 && tableNames.All(t => t.RowCount == Int32.MaxValue)) {
				using (SqlConnection conn = Helper.OpenConnection(currentConnectionData)) {
					AddRowCountsToTableInfos(conn);
				}
			}

			BuildVisibleDatabaseObjectList(txtSearch.Text);
			lblStatusbarInfo.Text = String.Format("Näitan {0} tabelit {1}st", trvDatabaseObjects.Nodes["Tables"].GetNodeCount(false), tableNames.Count);
		}

		#region EventHandlers

		private void btnConnect_Click(object sender, EventArgs e)
		{
			if (selPreviousConnections.SelectedItem != null && !String.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString())) {
				MoveConncetionDataToBeginning((ConnectionData)selPreviousConnections.SelectedItem);
				ConnectToDatabase((ConnectionData)selPreviousConnections.SelectedItem);
			}
		}

		private void MoveConncetionDataToBeginning(ConnectionData connectionData)
		{
			ConnectionData.DeleteSetting(connectionData);
			ConnectionData.AddSetting(connectionData);
			ToggleSelectedConnectionButtons();
		}

		private void btnRunQuery_Click(object sender, EventArgs e)
		{
			TabPage currentPage = tabQueries.SelectedTab;
			if (currentPage == null) {
				return;
			}
			if (!ConfirmQuery(GetQueryText(currentPage))) {
				return;
			}

			try {
				RunQuery(currentPage);
			}
			catch (Exception ex) {
				lblStatusbarInfo.Text = "";
				MessageBox.Show(ex.Message, "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnDeleteSelectedConnection_Click(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString())) {
				ConnectionData.DeleteSetting((ConnectionData)selPreviousConnections.SelectedItem);
				FillPreviousSettings();
				ToggleSelectedConnectionButtons();
			}
		}

		private void btnAddConnection_Click(object sender, EventArgs e)
		{
			ConnectionSettings connectionSettings = new ConnectionSettings();
			if (connectionSettings.ShowDialog() == DialogResult.OK) {
				ConnectionData.AddSetting(connectionSettings.ConnectionData);
				ToggleSelectedConnectionButtons();
				FillPreviousSettings();
				SelectSettingInListbox(connectionSettings.ConnectionData);
				ConnectToDatabase(connectionSettings.ConnectionData);
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
			}
			else {
				HideTableFieldsOverview();
			}
		}

		private void trvDatabaseObjects_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (trvDatabaseObjects.SelectedNode != null && trvDatabaseObjects.SelectedNode.Parent != null) {
				if (trvDatabaseObjects.SelectedNode.Parent.Name == "Tables") {
					BuildSelectQueryTabPage(trvDatabaseObjects.SelectedNode.Name, TableSelectLimit.None);
				}
				else if (trvDatabaseObjects.SelectedNode.Parent.Name == "Procs") {
					BuildStoredProcTabPage(trvDatabaseObjects.SelectedNode.Name);
				}
			}
		}

		private void dgResults_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			//DO NOTHING (tekkis jama mingite pilditulpade kuvamisega. Las olla nii, kuni midagi paremat teha ei oska)
		}

		private void btnAddQuery_Click(object sender, EventArgs e)
		{
			AddNewQueryPage(String.Empty, String.Empty);
		}

		private void btnDeleteQuery_Click(object sender, EventArgs e)
		{
			if (tabQueries.SelectedTab != null) {
				CloseQuery(tabQueries.SelectedTab);
			}
		}

		private void mniCloseTabpage_Click(object sender, EventArgs e)
		{
			if (mniCloseTabpage.Tag != null) {
				CloseQuery((TabPage)mniCloseTabpage.Tag);
			}
		}

		private void tabQueries_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				for (int i = 0; i < tabQueries.TabCount; i++) {
					Rectangle r = tabQueries.GetTabRect(i);
					if (r.Contains(e.Location)) {
						mniCloseTabpage.Text = String.Format("Sulge {0}", tabQueries.TabPages[i].Text);
						mniCloseTabpage.Tag = tabQueries.TabPages[i];
						cmnTabpage.Show(tabQueries, e.X, e.Y);

						return;
					}
				}

				mniCloseTabpage.Tag = null;
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			ConnectToDatabase(currentConnectionData);
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			BuildVisibleDatabaseObjectList(txtSearch.Text);
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

		private void trvDatabaseObjects_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				trvDatabaseObjects.SelectedNode = trvDatabaseObjects.GetNodeAt(e.X, e.Y);
				if (trvDatabaseObjects.SelectedNode.Parent != null) {
					if (trvDatabaseObjects.SelectedNode.Parent.Name == "Tables") {
						cmnTableCommands.Show((Control)sender, new Point(e.X, e.Y));
					}
					else if (trvDatabaseObjects.SelectedNode.Parent.Name == "Procs") {
						cmnStoredProcCommands.Show((Control)sender, new Point(e.X, e.Y));
					}
				}
				else if (trvDatabaseObjects.SelectedNode.Name == "Tables") {
					cmnTableCommandsGlobal.Show((Control)sender, new Point(e.X, e.Y));
				}
			}
		}

		private void mniCreateSelectQuery_Click(object sender, EventArgs e)
		{
			BuildSelectQueryTabPage(trvDatabaseObjects.SelectedNode.Name, TableSelectLimit.None);
		}

		private void mniSelectTopRows_Click(object sender, EventArgs e)
		{
			BuildSelectQueryTabPage(trvDatabaseObjects.SelectedNode.Name, TableSelectLimit.LimitTop);
		}

		private void mniSelectBottomRows_Click(object sender, EventArgs e)
		{
			BuildSelectQueryTabPage(trvDatabaseObjects.SelectedNode.Name, TableSelectLimit.LimitBottom);
		}

		private void mniSelectRowCount_Click(object sender, EventArgs e)
		{
			AddNewQueryPage(String.Format("SELECT{0}\tCOUNT(*){0}FROM{0}\t{1}", Environment.NewLine, trvDatabaseObjects.SelectedNode.Name));
		}

		private void mniCreateInsertQuery_Click(object sender, EventArgs e)
		{
			BuildInsertQueryTabPage(trvDatabaseObjects.SelectedNode.Name);
		}

		private void mniCreateDeleteQuery_Click(object sender, EventArgs e)
		{
			BuildDeleteQueryTabPage(trvDatabaseObjects.SelectedNode.Name);
		}

		private void mniCreateUpdateQuery_Click(object sender, EventArgs e)
		{
			BuildUpdateQueryTabPage(trvDatabaseObjects.SelectedNode.Name);
		}

		void dgResults_SelectionChanged(object sender, EventArgs e)
		{
			ToggleDatagridHeaderCopy((DataGridView)sender);
		}

		void dgResults_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.C) {
				e.IsInputKey = true;
			}
		}

		void dgResults_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyCode == Keys.C) {
				DoListCopy(((DataGridView)sender).SelectedCells.Cast<DataGridViewCell>());
				e.Handled = true;
			}
		}

		private void mniCopyStoredProcName_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.Clipboard.SetText(trvDatabaseObjects.SelectedNode.Name);
		}

		private void mniGrantExecuteOnSP_Click(object sender, EventArgs e)
		{
			AddNewQueryPage(String.Format("GRANT EXECUTE ON {0} TO xxx", trvDatabaseObjects.SelectedNode.Name));
		}

		private void mniShowTableRowCounts_Click(object sender, EventArgs e)
		{
			AddNewQueryPage(String.Format("SELECT {0}\tt.name \"Tabel\",{0}\tp.rows \"Ridasid\"{0}FROM {0}\tsys.tables t{0}INNER JOIN{0}\tsys.indexes i ON t.object_id = i.object_id{0}INNER JOIN{0}\tsys.partitions p on i.object_id = p.object_id and i.index_id = p.index_id{0}WHERE{0}\tt.is_ms_shipped = 0{0}GROUP BY{0}\tt.name, p.rows{0}ORDER BY{0}\tt.name{0}", Environment.NewLine));
		}

		private void mniHideEmptyTables_Click(object sender, EventArgs e)
		{
			RowCountFilterPrompt prompt = new RowCountFilterPrompt(Settings_MinimumRowCount);
			prompt.RowCount = Settings_MinimumRowCount;
			if (prompt.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				Settings_MinimumRowCount = prompt.RowCount;
			}
			HideEmptyTables();
		}

		#endregion

		enum TableSelectLimit
		{
			None,
			LimitTop,
			LimitBottom,
		}

		struct StoredProc
		{
			public string Name { get; set; }
			public string Content { get; set; }
		}

		class TableInfo
		{
			public string Name { get; set; }
			public long RowCount { get; set; }
		}

	}
}