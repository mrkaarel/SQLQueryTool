using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SqlQueryTool
{
	public class Column
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public ColumnType Type { get; set; }
		public string Value { get; set; }
		public bool IsIdentity { get; set; }
		public bool IsNullable { get; set; }
		public string DefaultValue { get; set; }

		public Column(string name, string description, string dataType, bool isIdentity, int length, bool isNullable, string defaultValue)
			: this(name, description, dataType, isIdentity, length, isNullable, defaultValue, null)
		{ }

		public Column(string name, string description, string dataType, bool isIdentity, int length, bool isNullable, string defaultValue, string value)
		{
			this.Name = name;
			this.Description = description;
			this.Type = new ColumnType(dataType);
			this.numericLength = length;
			this.Value = value;
			this.IsIdentity = isIdentity;
			this.IsNullable = isNullable;
			this.DefaultValue = defaultValue;
		}

		private int numericLength;
		public string Length
		{
			get {
				if (this.Type.IsStringType && this.numericLength == -1) {
					return "MAX";
				}
				else if (this.Type.IsUnicode) {
					return (numericLength / 2).ToString();
				}
				else {
					return numericLength.ToString();
				}
			}
		}

		public string FormattedValue
		{
			get
			{
				string result = this.Value;
				if (result == null) {
					result = this.Type.DefaultValue;
				}
				if (this.Type.UsesQuotes) {
					result = String.Format("'{0}'", result);
				}

				return result;
			}
		}


		public class ColumnType
		{
			public string Name { get; set; }

			public ColumnType(string columnTypeName)
			{
				this.Name = columnTypeName;
			}

			public bool IsStringType {
				get {
					switch (Name) {
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
				get {
					switch (Name) {
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
					switch (Name) {
						case "char":
						case "varchar":
						case "nchar":
						case "nvarchar":
						case "text":
						case "ntext":
						case "smalldatetime":
						case "datetime":
						case "date":
						case "time":
						case "image":
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
					switch (Name) {
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
							return DateTime.Now.ToString("yyyy.MM.dd 00:00:00");
						case "date":
							return DateTime.Now.ToString("yyyy.MM.dd");
						case "time":
							return "00:00:00";
						default:
							throw new ArgumentException("Unknown DbType: {0}", Name);
					}
				}
			}
		}
	}
}
