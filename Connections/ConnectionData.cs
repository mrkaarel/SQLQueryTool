using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace SqlQueryTool.Connections
{
    public class ConnectionData
    {
        public static int DefaultTimeout;

        public ConnectionData(string settingString)
        {
            var settings = settingString.Split('|');
            ServerName = settings[0];
            DatabaseName = settings[1];
            UserName = settings[2];
            Password = settings[3];
            if (settings.Length > 4 && int.Parse(settings[4]) == 1) UseIntegratedSecurity = true;
            Timeout = DefaultTimeout;
        }

        public ConnectionData(string serverName, string databaseName, string userName, string password,
            bool useIntegratedSecurity, int timeout)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;
            UseIntegratedSecurity = useIntegratedSecurity;
            Timeout = timeout;
        }

        public string ServerName { get; }
        public string DatabaseName { get; }
        private string UserName { get; }
        private string Password { get; }
        private bool UseIntegratedSecurity { get; }
        public int Timeout { private get; set; }

        public string ProviderName => "System.Data.SqlClient";

        public string SerializedString => $"{ServerName}|{DatabaseName}|{UserName}|{Password}|{(UseIntegratedSecurity ? 1 : 0)}";

        public DbConnection GetOpenConnection()
        {
            var connectionString = BuildConnectionString(ServerName, DatabaseName, UseIntegratedSecurity, UserName,
                Password, Timeout);

            var conn = new SqlConnection(connectionString);
            conn.Open();

            return conn;
        }

        public static string BuildConnectionString(string server, string dataBase = "",
            bool useIntegratedSecurity = true, string userName = "", string password = "", int timeout = -1)
        {
            var result = new StringBuilder();

            result.AppendFormat("Server = {0}", server);

            if (!string.IsNullOrEmpty(dataBase)) result.AppendFormat("; Database = {0}", dataBase);

            if (useIntegratedSecurity)
                result.Append("; Integrated Security = True");
            else
                result.AppendFormat("; User ID = {0}; Password = {1}", userName, password);

            if (timeout > -1) result.AppendFormat("; Connection Timeout = {0}", timeout);

            return result.ToString();
        }

        public override string ToString()
        {
            return $"{DatabaseName}@{ServerName}";
        }
    }
}