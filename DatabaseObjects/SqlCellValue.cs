namespace SqlQueryTool.DatabaseObjects
{
    public class SqlCellValue
    {
        public SqlCellValue(string columnName, string value, string sqlFormattedValue, string tableName = null) 
        {
            ColumnName = columnName;
            Value = value;
            SqlFormattedValue = sqlFormattedValue;
            TableName = tableName;
        }

        public string ColumnName { get; }
        public string TableName { get; }
        public string Value { get; }
        public string SqlFormattedValue { get; }
    }
}