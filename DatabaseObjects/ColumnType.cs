using System;

namespace SqlQueryTool.DatabaseObjects
{
    public class ColumnType
    {
        public string Name { get; set; }

        public ColumnType(string columnTypeName)
        {
            this.Name = columnTypeName;
        }

        public bool IsStringType
        {
            get
            {
                switch (Name)
                {
                    case "char":
                    case "varchar":
                    case "nchar":
                    case "nvarchar":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsUnicode
        {
            get
            {
                switch (Name)
                {
                    case "nchar":
                    case "nvarchar":
                    case "ntext":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool UsesQuotes
        {
            get
            {
                switch (Name)
                {
                    case "char":
                    case "varchar":
                    case "nchar":
                    case "nvarchar":
                    case "text":
                    case "ntext":
                    case "smalldatetime":
                    case "datetime":
                    case "datetime2":
                    case "date":
                    case "time":
                    case "image":
                    case "uniqueidentifier":
                        return true;
                    case "bit":
                    case "tinyint":
                    case "smallint":
                    case "int":
                    case "numeric":
                    case "decimal":
                    case "smallmoney":
                    case "money":
                    case "real":
                    case "float":
                    case "bigint":
                        return false;
                    default:
                        throw new ArgumentException("Unknown DbType: {0}", Name);
                }
            }
        }

        public string DefaultValue
        {
            get
            {
                switch (Name)
                {
                    case "char":
                    case "varchar":
                    case "nchar":
                    case "nvarchar":
                    case "text":
                    case "ntext":
                    case "image":
                        return String.Empty;
                    case "bit":
                    case "tinyint":
                    case "smallint":
                    case "int":
                    case "numeric":
                    case "decimal":
                    case "smallmoney":
                    case "money":
                    case "real":
                    case "float":
                    case "bigint":
                        return "0";
                    case "smalldatetime":
                        return DateTime.Now.ToString("yyyy.MM.dd 00:00");
                    case "datetime":
                    case "datetime2":
                        return DateTime.Now.ToString("yyyy.MM.dd 00:00:00");
                    case "date":
                        return DateTime.Now.ToString("yyyy.MM.dd");
                    case "time":
                        return "00:00:00";
                    case "uniqueidentifier":
                        return Guid.Empty.ToString();
                    default:
                        throw new ArgumentException("Unknown DbType: {0}", Name);
                }
            }
        }
    }
}