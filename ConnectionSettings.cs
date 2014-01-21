using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

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
                ConnectionData connectionData = new ConnectionData(txtServer.Text, txtDatabase.Text, txtUsername.Text, txtPassword.Text, chkIntegratedSecurity.Checked, 10);
                using (SqlConnection conn = Helper.OpenConnection(connectionData)) {
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


    }
}
