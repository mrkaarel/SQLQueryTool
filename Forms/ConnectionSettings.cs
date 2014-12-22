using SqlQueryTool.Connections;
using SqlQueryTool.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SqlQueryTool
{
	public partial class ConnectionSettings : Form
	{
		public ConnectionData ConnectionData { get; set; }

		public ConnectionSettings()
		{
			InitializeComponent();
			Setup();
		}

		private void Setup()
		{
			chkIntegratedSecurity.Checked = true;
		}

		private void chkIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
		{
			txtUsername.Enabled = !chkIntegratedSecurity.Checked;
			txtPassword.Enabled = !chkIntegratedSecurity.Checked;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			try {
				var connectionData = new ConnectionData(txtServer.Text, selDatabaseName.Text, txtUsername.Text, txtPassword.Text, chkIntegratedSecurity.Checked, 10);
				using (var conn = connectionData.GetOpenConnection()) {
					// try if it works
				}
				connectionData.Timeout = ConnectionData.DefaultTimeout;
				this.ConnectionData = connectionData;
				this.DialogResult = DialogResult.OK;
			}
			catch (Exception ex) {
				MessageBox.Show(String.Format("Viga andmebaasiühendusega:\n{0}", ex.Message), "Viga andmebaasiühendusega", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void selDatabaseName_DropDown(object sender, EventArgs e)
		{
			selDatabaseName.Items.Clear();

			if (!String.IsNullOrEmpty(txtServer.Text)) {
				foreach (var databaseName in GetDatabaseNames(txtServer.Text, chkIntegratedSecurity.Checked, txtUsername.Text, txtPassword.Text)) {
					selDatabaseName.Items.Add(databaseName);
				}
			}
		}

		private IEnumerable<string> GetDatabaseNames(string serverName, bool useIntegratedSecurity, string userName, string password)
		{
			using (var connection = new SqlConnection(ConnectionData.BuildConnectionString(server: serverName, useIntegratedSecurity: useIntegratedSecurity, userName: userName, password: password))) {
				connection.Open();

				var cmd = connection.CreateCommand();
				cmd.CommandText = QueryBuilder.SystemQueries.GetDatabaseList();

				using (var rdr = cmd.ExecuteReader()) {
					while (rdr.Read()) {
						yield return rdr.GetString(0);
					}
				}
			}
		}
	}
}
