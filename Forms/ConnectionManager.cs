using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SqlQueryTool.Connections;

namespace SqlQueryTool.Forms
{
    public partial class ConnectionManager : UserControl
    {
        public delegate void ConnectionInitiatedHandler(ConnectionData connectionData);

        public ConnectionManager()
        {
            InitializeComponent();

            BuildPreviousConnections(ConnectionDataStorage.LoadSavedSettings());
        }

        public event ConnectionInitiatedHandler OnConnectionInitiated;

        public void SetConnectionAchieved()
        {
            btnConnect.Enabled = false;
        }

        private void BuildPreviousConnections(IEnumerable<ConnectionData> connections)
        {
            selPreviousConnections.Items.Clear();
            foreach (var setting in connections) selPreviousConnections.Items.Add(setting);

            selPreviousConnections.Items.Insert(0, string.Empty);
            ToggleSelectedConnectionButtons();
        }

        private void ToggleSelectedConnectionButtons()
        {
            var enableButtons = selPreviousConnections.Items.Count > 0 && selPreviousConnections.SelectedItem != null &&
                                !string.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString());
            btnDeleteSelectedConnection.Enabled = enableButtons;
            btnConnect.Enabled = enableButtons;
        }

        private void btnDeleteSelectedConnection_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString()))
            {
                var settings =
                    ConnectionDataStorage.DeleteSetting(selPreviousConnections.SelectedItem as ConnectionData);
                BuildPreviousConnections(settings);
            }
        }

        private void btnAddConnection_Click(object sender, EventArgs e)
        {
            var connectionSettingsPrompt = new ConnectionSettings();
            if (connectionSettingsPrompt.ShowDialog() == DialogResult.OK)
            {
                var newConnectionData = connectionSettingsPrompt.ConnectionData;

                var settings = ConnectionDataStorage.AddSetting(newConnectionData);
                BuildPreviousConnections(settings);
                selPreviousConnections.SelectedIndex = 1;
                ToggleSelectedConnectionButtons();

                OnConnectionInitiated(connectionSettingsPrompt.ConnectionData);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (selPreviousConnections.SelectedItem != null &&
                !string.IsNullOrEmpty(selPreviousConnections.SelectedItem.ToString()))
            {
                var connectionData = selPreviousConnections.SelectedItem as ConnectionData;
                ConnectionDataStorage.MoveFirst(connectionData);

                OnConnectionInitiated(connectionData);
            }
        }

        private void selPreviousConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleSelectedConnectionButtons();
        }
    }
}