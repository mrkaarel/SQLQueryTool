using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlQueryTool.DatabaseObjects
{
	public class Heuristics
	{
		public static IEnumerable<string> GetIdColumnNames(string tableName)
		{
			var results = new List<string>() { "id" };
			if (!String.IsNullOrEmpty(tableName)) {
				tableName = tableName.ToLower();

				var starts = new List<string>() { tableName };
				if (tableName.EndsWith("s")) {
					starts.Add(tableName.TrimEnd('s'));
				}
				var ends = new List<string>() { "id", "_id" };
				results.AddRange(starts.SelectMany(s => ends.Select(e => s + e)));
			}

			return results;
		}
	}
}
