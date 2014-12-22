using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SqlQueryTool.DatabaseObjects
{
	public class ColumnDefinition
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public ColumnType Type { get; set; }
		public bool IsIdentity { get; set; }
		public bool IsNullable { get; set; }
		public string DefaultValue { get; set; }

		public ColumnDefinition(string name, string description, string dataType, bool isIdentity, string length, bool isNullable, string defaultValue)
		{
			this.Name = name;
			this.Description = description;
			this.Type = new ColumnType(dataType);
			this.rawLength = length;
			this.IsIdentity = isIdentity;
			this.IsNullable = isNullable;
			this.DefaultValue = defaultValue;
		}

		private string rawLength;
		public string Length
		{
			get {
				if (this.Type.IsStringType && this.rawLength == "-1") {
					return "MAX";
				}
				else if (this.Type.IsUnicode) {
					return (Int32.Parse(rawLength) / 2).ToString();
				}
				else {
					return rawLength.ToString();
				}
			}
		}

		public string FormattedValue
		{
			get
			{
				string result = this.Type.DefaultValue;

				if (this.Type.UsesQuotes) {
					result = String.Format("'{0}'", result);
				}

				return result;
			}
		}
	}
}
