namespace SqlQueryTool.DatabaseObjects
{
    public class SqlCellValue
    {
        public SqlCellValue(string columnName, string value, string sqlFormattedValue)
        {
            ColumnName = columnName;
            Value = value;
            SqlFormattedValue = sqlFormattedValue;
        }

        public string ColumnName { get; }
        public string Value { get; }
        public string SqlFormattedValue { get; }
    }
}