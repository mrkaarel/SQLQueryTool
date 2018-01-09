namespace SqlQueryTool.DatabaseObjects
{
    public class ColumnDefinition
    {
        private readonly string _rawLength;

        public ColumnDefinition(string name, string description, string dataType, bool isIdentity, string length,
            bool isNullable, string defaultValue)
        {
            Name = name;
            Description = description;
            Type = new ColumnType(dataType);
            _rawLength = length;
            IsIdentity = isIdentity;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
        }

        public string Name { get; }
        public string Description { get; }
        public ColumnType Type { get; }
        public bool IsIdentity { get; }
        public bool IsNullable { get; }
        public string DefaultValue { get; }
        public ForeignKey ForeignKey { get; set; }

        public string Length
        {
            get
            {
                if (_rawLength == "-1")
                    return "MAX";
                if (Type.IsUnicode)
                    return (int.Parse(_rawLength) / 2).ToString();
                return _rawLength;
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