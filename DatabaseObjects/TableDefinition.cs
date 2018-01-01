using System;
using System.Collections.Generic;
using System.Linq;
using SqlQueryTool.Connections;

namespace SqlQueryTool.DatabaseObjects
{
    public class TableDefinition
    {
        private readonly List<ColumnDefinition> columns;

        public TableDefinition(string name, ConnectionData connectionData)
        {
            Name = name;
            columns = new List<ColumnDefinition>();
            using (var conn = connectionData.GetOpenConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM sys.extended_properties";

                var hasExtendedPropertiesTable = false;
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
                    cmd.CommandText =
                        $@"SELECT scol.name, stype.name, CASE stype.name WHEN 'decimal' THEN CAST(scol.xprec AS VARCHAR)+','+CAST(scol.xscale AS VARCHAR) ELSE CAST(scol.length AS VARCHAR) END, CAST((CASE WHEN scol.status = 128 THEN 1 ELSE 0 END) AS BIT) AS ""IsIdentity"", COALESCE(prop.value, ''), CAST(scol.isnullable AS BIT), COALESCE(syscom.text, '') AS ""DefaultValue"" FROM sysobjects so JOIN syscolumns scol ON (so.id = scol.id) JOIN systypes stype ON (scol.xtype = stype.xusertype) LEFT JOIN sys.extended_properties prop ON (prop.major_id = scol.id AND prop.minor_id = scol.colid AND prop.name = 'MS_Description') LEFT JOIN syscomments syscom ON (scol.cdefault > 0 AND scol.cdefault = syscom.id) WHERE so.name = '{
                            name
                        }' AND so.xtype = 'u' AND stype.xusertype != 256 ORDER BY scol.colorder";
                else
                    cmd.CommandText =
                        $@"SELECT scol.name, stype.name, CASE stype.name WHEN 'decimal' THEN CAST(scol.xprec AS VARCHAR)+','+CAST(scol.xscale AS VARCHAR) ELSE CAST(scol.length AS VARCHAR) END, CAST((CASE WHEN scol.status = 128 THEN 1 ELSE 0 END) AS BIT) AS ""IsIdentity"", '', CAST(scol.isnullable AS BIT), '' AS ""DefaultValue"" FROM sysobjects so JOIN syscolumns scol ON (so.id = scol.id) JOIN systypes stype ON (scol.xtype = stype.xusertype) WHERE so.name = '{
                            name
                        }' AND so.xtype = 'u' AND stype.xusertype != 256 ORDER BY scol.colorder";
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        columns.Add(new ColumnDefinition(rdr.GetString(0), rdr.GetString(4), rdr.GetString(1),
                            rdr.GetBoolean(3), rdr.GetString(2), rdr.GetBoolean(5), rdr.GetString(6)));
                }
            }
        }

        public string Name { get; set; }

        public IEnumerable<ColumnDefinition> Columns => columns.AsReadOnly();

        public ColumnDefinition IdentityColumn
        {
            get { return columns.SingleOrDefault(c => c.IsIdentity); }
        }
    }
}