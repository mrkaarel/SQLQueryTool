using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace SqlQueryTool
{
	public class Table
	{
		public string Name { get; set; }
		public ColumnCollection Columns { get; set; }

		public Table(string name, ConnectionData connectionData)
		{
			this.Name = name;
			this.Columns = new ColumnCollection();
			using (SqlConnection conn = Helper.OpenConnection(connectionData)) {
				SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM sys.extended_properties", conn);
				bool hasExtendedPropertiesTable = false;
				try {
					cmd.ExecuteScalar();
					hasExtendedPropertiesTable = true;
				}
				catch (Exception) {
					// probably no extended_properties table (Azure). leave it like that.
				}
				if (hasExtendedPropertiesTable) {
					cmd.CommandText = String.Format(@"SELECT LOWER(scol.name), stype.name, scol.length, CAST((CASE WHEN scol.status = 128 THEN 1 ELSE 0 END) AS BIT) AS ""IsIdentity"", COALESCE(prop.value, ''), CAST(scol.isnullable AS BIT), COALESCE(syscom.text, '') AS ""DefaultValue"" FROM sysobjects so JOIN syscolumns scol ON (so.id = scol.id) JOIN systypes stype ON (scol.xtype = stype.xtype) LEFT JOIN sys.extended_properties prop ON (prop.major_id = scol.id AND prop.minor_id = scol.colid AND prop.name = 'MS_Description') LEFT JOIN syscomments syscom ON (scol.cdefault > 0 AND scol.cdefault = syscom.id) WHERE so.name = '{0}' AND so.xtype = 'u' AND stype.xusertype != 256 ORDER BY scol.colorder", name);
				}
				else {
					cmd.CommandText = String.Format(@"SELECT LOWER(scol.name), stype.name, scol.length, CAST((CASE WHEN scol.status = 128 THEN 1 ELSE 0 END) AS BIT) AS ""IsIdentity"", '', CAST(scol.isnullable AS BIT), '' AS ""DefaultValue"" FROM sysobjects so JOIN syscolumns scol ON (so.id = scol.id) JOIN systypes stype ON (scol.xtype = stype.xtype) WHERE so.name = '{0}' AND so.xtype = 'u' AND stype.xusertype != 256 ORDER BY scol.colorder", name);
				}
				using (SqlDataReader rdr = cmd.ExecuteReader()) {
					while (rdr.Read()) {
						string colname = rdr.GetString(0);
						string type = rdr.GetString(1);
						short length = rdr.GetInt16(2);
						Columns.Add(new Column(rdr.GetString(0), rdr.GetString(4), rdr.GetString(1), rdr.GetBoolean(3), rdr.GetInt16(2), rdr.GetBoolean(5), rdr.GetString(6)));
					}
				}
			}
		}
	}
}
