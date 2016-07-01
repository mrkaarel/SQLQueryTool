using SqlQueryTool.Connections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlQueryTool.DatabaseObjects
{
    public class TableDefinition
    {
        public string Name { get; set; }

        private List<ColumnDefinition> columns;
        public IEnumerable<ColumnDefinition> Columns
        {
            get
            {
                return columns.AsReadOnly();
            }
        }

        public ColumnDefinition IdentityColumn
        {
            get
            {
                return columns.SingleOrDefault(c => c.IsIdentity);
            }
        }

        public TableDefinition(string name, ConnectionData connectionData)
        {
            this.Name = name;
            this.columns = new List<ColumnDefinition>();
            using (var conn = connectionData.GetOpenConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM sys.extended_properties";

                bool hasExtendedPropertiesTable = false;
                try
                {
                    cmd.ExecuteScalar();
                    hasExtendedPropertiesTable = true;
                }
                catch (Exception)
                {
                    // probably no extended_properties table (Azure). leave it like that.
                }
                if (hasExtendedPropertiesTable)
                {
                    cmd.CommandText = String.Format(@"SELECT scol.name, stype.name, CASE stype.name WHEN 'decimal' THEN CAST(scol.xprec AS VARCHAR)+','+CAST(scol.xscale AS VARCHAR) ELSE CAST(scol.length AS VARCHAR) END, CAST((CASE WHEN scol.status = 128 THEN 1 ELSE 0 END) AS BIT) AS ""IsIdentity"", COALESCE(prop.value, ''), CAST(scol.isnullable AS BIT), COALESCE(syscom.text, '') AS ""DefaultValue"" FROM sysobjects so JOIN syscolumns scol ON (so.id = scol.id) JOIN systypes stype ON (scol.xtype = stype.xusertype) LEFT JOIN sys.extended_properties prop ON (prop.major_id = scol.id AND prop.minor_id = scol.colid AND prop.name = 'MS_Description') LEFT JOIN syscomments syscom ON (scol.cdefault > 0 AND scol.cdefault = syscom.id) WHERE so.name = '{0}' AND so.xtype = 'u' AND stype.xusertype != 256 ORDER BY scol.colorder", name);
                }
                else
                {
                    cmd.CommandText = String.Format(@"SELECT scol.name, stype.name, CASE stype.name WHEN 'decimal' THEN CAST(scol.xprec AS VARCHAR)+','+CAST(scol.xscale AS VARCHAR) ELSE CAST(scol.length AS VARCHAR) END, CAST((CASE WHEN scol.status = 128 THEN 1 ELSE 0 END) AS BIT) AS ""IsIdentity"", '', CAST(scol.isnullable AS BIT), '' AS ""DefaultValue"" FROM sysobjects so JOIN syscolumns scol ON (so.id = scol.id) JOIN systypes stype ON (scol.xtype = stype.xusertype) WHERE so.name = '{0}' AND so.xtype = 'u' AND stype.xusertype != 256 ORDER BY scol.colorder", name);
                }
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        columns.Add(new ColumnDefinition(name: rdr.GetString(0), description: rdr.GetString(4), dataType: rdr.GetString(1), isIdentity: rdr.GetBoolean(3), length: rdr.GetString(2), isNullable: rdr.GetBoolean(5), defaultValue: rdr.GetString(6)));
                    }
                }
            }
        }
    }
}
