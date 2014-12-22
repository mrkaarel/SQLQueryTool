using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace SqlQueryTool.Connections
{
	public class ConnectionData
	{
		public static int DefaultTimeout;

		public string ServerName { get; set; }
		public string DatabaseName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool UseIntegratedSecurity { get; set; }
		public int Timeout { get; set; }

		public ConnectionData(string settingString)
		{
			string[] settings = settingString.Split('|');
			this.ServerName = settings[0];
			this.DatabaseName = settings[1];
			this.UserName = settings[2];
			this.Password = settings[3];
			if (settings.Length > 4 && Int32.Parse(settings[4]) == 1) {
				this.UseIntegratedSecurity = true;
			}
			this.Timeout = DefaultTimeout;
		}

		public ConnectionData(string serverName, string databaseName, string userName, string password, bool useIntegratedSecurity)
			: this(serverName, databaseName, userName, password, useIntegratedSecurity, DefaultTimeout)
		{ }

		public ConnectionData(string serverName, string databaseName, string userName, string password, bool useIntegratedSecurity, int timeout)
		{
			this.ServerName = serverName;
			this.DatabaseName = databaseName;
			this.UserName = userName;
			this.Password = password;
			this.UseIntegratedSecurity = useIntegratedSecurity;
			this.Timeout = timeout;
		}

		public string ProviderName
		{
			get
			{
				return "System.Data.SqlClient";
			}
		}

		public DbConnection GetOpenConnection()
		{
			string connectionString = BuildConnectionString(this.ServerName, this.DatabaseName, this.UseIntegratedSecurity, this.UserName, this.Password, this.Timeout);

			var conn = new SqlConnection(connectionString);
			conn.Open();

			return conn;
		}

		public static string BuildConnectionString(string server, string dataBase = "", bool useIntegratedSecurity = true, string userName = "", string password = "", int timeout = -1)
		{
			var result = new StringBuilder();

			result.AppendFormat("Server = {0}", server);

			if (!String.IsNullOrEmpty(dataBase)) {
				result.AppendFormat("; Database = {0}", dataBase);
			}

			if (useIntegratedSecurity) {
				result.Append("; Integrated Security = True");
			}
			else {
				result.AppendFormat("; User ID = {0}; Password = {1}", userName, password);
			}

			if (timeout > -1) {
				result.AppendFormat("; Connection Timeout = {0}", timeout);
			}

			return result.ToString();
		}

		public string SerializedString
		{
			get
			{
				return String.Format("{0}|{1}|{2}|{3}|{4}", this.ServerName, this.DatabaseName, this.UserName, this.Password, UseIntegratedSecurity ? 1 : 0);
			}
		}

		public override string ToString()
		{
			return String.Format("{0}@{1}", this.DatabaseName, this.ServerName);
		}
	}
}
