using System.Collections.Generic;
using System.Data;
using SqlQueryTool.Connections;

namespace SqlQueryTool.DatabaseObjects
{
    public class ForeignKey
    {
        public string PrimaryTable { get; }
        public string PrimaryColumn { get; }
        public string ForeignTable { get; }
        public string ForeignColumn { get; }

        public ForeignKey(string primaryTable, string primaryColumn, string foreignTable, string foreignColumn)
        {
            PrimaryTable = primaryTable;
            PrimaryColumn = primaryColumn;
            ForeignTable = foreignTable;
            ForeignColumn = foreignColumn;
        }

        public static IEnumerable<ForeignKey> GetForeignKeysToTable(string primaryKeyTable, ConnectionData connectionData)
        {
            return GetForeignKeys(primaryKeyTable, "pktable_name", connectionData);
        }
        
        public static IEnumerable<ForeignKey> GetForeignKeysFromTable(string foreignKeyTable, ConnectionData connectionData)
        {
            return GetForeignKeys(foreignKeyTable, "fktable_name", connectionData);
        }

        private static IEnumerable<ForeignKey> GetForeignKeys(string tableName, string tableParameterName, ConnectionData connectionData)
        {
            using (var conn = connectionData.GetOpenConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_fkeys";
                var p = cmd.CreateParameter();
                p.ParameterName = tableParameterName;
                p.Value = tableName;
                cmd.Parameters.Add(p);
                
                using (var rdr = cmd.ExecuteReader())
                {
                    var results = new List<ForeignKey>();
                    while (rdr.Read())
                    {
                        results.Add(new ForeignKey((string)rdr["pktable_name"], (string)rdr["pkcolumn_name"], (string)rdr["fktable_name"], (string)rdr["fkcolumn_name"]));
                    }

                    return results;
                }
            }
        }
    }
}