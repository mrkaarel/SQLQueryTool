namespace SqlQueryTool.DatabaseObjects
{
    public class ColumnDefinition
    {
        private readonly string rawLength;

        public ColumnDefinition(string name, string description, string dataType, bool isIdentity, string length,
            bool isNullable, string defaultValue)
        {
            Name = name;
            Description = description;
            Type = new ColumnType(dataType);
            rawLength = length;
            IsIdentity = isIdentity;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
        }

        public string Name { get; }
        public string Description { get; }
        public ColumnType Type { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsNullable { get; set; }
        public string DefaultValue { get; set; }

        public string Length
        {
            get
            {
                if (rawLength == "-1")
                    return "MAX";
                if (Type.IsUnicode)
                    return (int.Parse(rawLength) / 2).ToString();
                return rawLength;
            }
        }

        public string FormattedValue
        {
            get
            {
                var result = Type.DefaultValue;

                if (Type.UsesQuotes) result = $"'{result}'";

                return result;
            }
        }
    }
}