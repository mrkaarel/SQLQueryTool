using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using SqlQueryTool.Connections;
using SqlQueryTool.DatabaseObjects;

namespace SqlQueryTool.Forms
{
    public partial class ConnectionSettings : Form
    {
        public ConnectionSettings()
        {
            InitializeComponent();
            Setup();
        }

        public ConnectionData ConnectionData { get; set; }

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
            try
            {
                var connectionData = new ConnectionData(txtServer.Text, selDatabaseName.Text, txtUsername.Text,
                    txtPassword.Text, chkIntegratedSecurity.Checked, 10);
                using (var conn = connectionData.GetOpenConnection())
                {
                    // try if it works
                }

                connectionData.Timeout = ConnectionData.DefaultTimeout;
                ConnectionData = connectionData;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problem connecting to database:\n{ex.Message}", "Connection error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void selDatabaseName_DropDown(object sender, EventArgs e)
        {
            selDatabaseName.Items.Clear();

            if (!string.IsNullOrEmpty(txtServer.Text))
                foreach (var databaseName in GetDatabaseNames(txtServer.Text, chkIntegratedSecurity.Checked,
                    txtUsername.Text, txtPassword.Text))
                    selDatabaseName.Items.Add(databaseName);
        }

        private static IEnumerable<string> GetDatabaseNames(string serverName, bool useIntegratedSecurity, string userName,
            string password)
        {
            var results = new List<string>();
            try
            {
                using (var connection = new SqlConnection(ConnectionData.BuildConnectionString(serverName,
                    useIntegratedSecurity: useIntegratedSecurity, userName: userName, password: password)))
                {
                    connection.Open();

                    var cmd = connection.CreateCommand();
                    cmd.CommandText = QueryBuilder.SystemQueries.GetDatabaseList();


                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read()) results.Add(rdr.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return results;
        }
    }
}