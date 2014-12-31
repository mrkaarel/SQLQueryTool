using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlQueryTool.DatabaseObjects
{
	public class SqlCellValue
	{
		public string ColumnName { get; set; }
		public string Value { get; set; }
		public string SqlFormattedValue { get; set; }

		public SqlCellValue(string columnName, string value, string sqlFormattedValue)
		{
			this.ColumnName = columnName;
			this.Value = value;
			this.SqlFormattedValue = sqlFormattedValue;
		}
	}
}
