using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlQueryTool.DatabaseObjects
{
	public static class QueryBuilder
	{
		public static string BuildInsertQuery(this TableDefinition table)
		{
			var queryText = new StringBuilder(String.Format("INSERT INTO {0}{1}\t", table.Name.ForQueries(), Environment.NewLine));
			
			string columnNames = String.Join(", ", table.Columns.Where(c => !c.IsIdentity).Select(c => c.Name.ForQueries()).ToArray());
			queryText.AppendFormat("({0})", columnNames);

			queryText.AppendFormat("{0}VALUES{0}\t", Environment.NewLine);

			string defaultColumnValues = String.Join(", ", table.Columns.Where(c => !c.IsIdentity).Select(c => c.FormattedValue).ToArray());
			queryText.AppendFormat("({0})", defaultColumnValues);

			return queryText.ToString();
		}

		public static string BuildSelectRowCountQuery(this TableDefinition table)
		{
			return BuildSelectRowCountQuery(table.Name);
		}

		public static string BuildSelectRowCountQuery(string tableName)
		{
			return String.Format("SELECT{0}\tCOUNT(*){0}FROM{0}\t{1}", Environment.NewLine, tableName.ForQueries());
		}

		public static string BuildSelectQuery(this TableDefinition table, TableSelectLimit selectLimit, string whereClause = "")
		{
			var queryText = new StringBuilder("SELECT ");
			if (selectLimit != TableSelectLimit.None) {
				queryText.Append("TOP 100 ");
			}

			if (table.Columns.Count() > 0) {
				string columns = String.Join(", ", table.Columns.Select(c => String.Format("{0}\t{1}", Environment.NewLine, c.Name.ForQueries())).ToArray());
				queryText.Append(columns);
			}
			else { // for views
				queryText.AppendFormat("{0}\t*", Environment.NewLine);
			}

			queryText.AppendFormat("{0}FROM{0}\t{1}", Environment.NewLine, table.Name.ForQueries());

			if (!String.IsNullOrEmpty(whereClause)) {
				queryText.AppendFormat("{0}WHERE{0}\t{1}", Environment.NewLine, whereClause);
			}

			if (selectLimit == TableSelectLimit.LimitBottom) {
				queryText.AppendFormat("{0}ORDER BY", Environment.NewLine);
				if (table.IdentityColumn != null) {
					queryText.AppendFormat("{0}\t{1} DESC", Environment.NewLine, table.IdentityColumn.Name.ForQueries());
				}
				else {
					queryText.AppendFormat("{0}\t? DESC", Environment.NewLine);
				}
			}

			return queryText.ToString();
		}

		public static string BuildUpdateQuery(this TableDefinition table)
		{
			var queryText = new StringBuilder(String.Format("UPDATE {0}\t{1}{0}SET", Environment.NewLine, table.Name.ForQueries()));

			string columns = String.Join(", ", table.Columns.Where(c => !c.IsIdentity).Select(c => String.Format("{0}\t{1} = {2}", Environment.NewLine, c.Name.ForQueries(), c.FormattedValue)).ToArray());
			queryText.Append(columns);

			queryText.AppendFormat("{0}WHERE{0}\t{1}", Environment.NewLine, GetWhereClause(table));

			return queryText.ToString();
		}

		public static string BuildRowUpdateQuery(string tableName, IEnumerable<SqlCellValue> updateCells, SqlCellValue filterCell)
		{
			var queryText = new StringBuilder(String.Format("UPDATE {0}\t{1}{0}SET", Environment.NewLine, tableName.ForQueries()));

			foreach (var cell in updateCells) {
				queryText.AppendFormat("{0}\t{1} = {2}, ", Environment.NewLine, cell.ColumnName.ForQueries(), cell.SqlFormattedValue);
			}
			queryText.Remove(queryText.Length - 2, 2);

			queryText.AppendFormat("{0}WHERE{0}\t{1} = {2}", Environment.NewLine, filterCell.ColumnName.ForQueries(), filterCell.SqlFormattedValue);
			if (!Heuristics.GetIdColumnNames(tableName).Contains(filterCell.ColumnName.ToLower())) {
				queryText.AppendFormat("{0}\tAND 1 = 0 /* Review the WHERE clause! */", Environment.NewLine);
			}

			return queryText.ToString();
		}

		public static string BuildRowDeleteQuery(string tableName, IEnumerable<SqlCellValue> filterCells, SelectionShape filterCellsType)
		{
			var queryText = new StringBuilder(String.Format("DELETE FROM{0}\t{1}{0}WHERE{0}\t", Environment.NewLine, tableName.ForQueries()));

			if (filterCellsType == SelectionShape.Column) {
				queryText.AppendFormat("{0} IN ({1})", filterCells.First().ColumnName.ForQueries(), String.Join(", ", filterCells.Select(f => f.SqlFormattedValue).ToArray()));
			}
			else if (filterCellsType == SelectionShape.Row) {
				queryText.AppendFormat("({0})", String.Join(" AND ", filterCells.Select(f => String.Format("{0} = {1}", f.ColumnName.ForQueries(), f.SqlFormattedValue)).ToArray()));
			}
			else {
				queryText.Append("1 = 0");
			}

			return queryText.ToString();
		}

		public static string BuildDeleteQuery(this TableDefinition table)
		{
			return String.Format("DELETE FROM {0}\t{1}{0}WHERE{0}\t{2}", Environment.NewLine, table.Name.ForQueries(), GetWhereClause(table));
		}

		public static string BuildGrantExecuteOnSP(string spName, string userName = "xxx")
		{
			return String.Format("GRANT EXECUTE {0}ON {1} {0}TO {2}", Environment.NewLine, spName, userName);
		}

		public static bool IsCrudQuery(string queryText)
		{
			return QueryStartsWithKeyword(queryText, new string[] { "INSERT", "SELECT", "UPDATE", "DELETE" });
		}

		public static bool IsSelectQuery(string queryText)
		{
			return QueryStartsWithKeyword(queryText, new string[] { "SELECT" });
		}

		public static bool IsDestroyQuery(string queryText)
		{
			return QueryStartsWithKeyword(queryText, new string[] { "UPDATE", "DELETE" });
		}

		public static bool IsStructureAlteringQuery(string queryText)
		{
			return QueryStartsWithKeyword(queryText, new string[] { "ALTER", "DROP" });
		}

		private static bool QueryStartsWithKeyword(string queryText, IEnumerable<string> keywords)
		{
			return keywords.Any(s => (queryText ?? "").ToUpper().TrimStart().StartsWith(s.ToUpper()));
		}

		private static string GetWhereClause(TableDefinition table)
		{
			if (table.IdentityColumn != null) {
				return String.Format("{0} = ?", table.IdentityColumn.Name.ForQueries());
			}
			else {
				return "?";
			}
		}

		public static class SystemQueries
		{
			public static string GetDatabaseList()
			{
				return String.Format("SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb')");
			}

			public static string GetTableListWithRowCounts()
			{
				return String.Format("SELECT DISTINCT{0}\tLOWER(t.name) \"Table\",{0}\tp.rows \"Rows\"{0}FROM {0}\tsys.tables t{0}INNER JOIN{0}\tsys.partitions p ON (t.object_id = p.object_id AND p.index_id < 2){0}WHERE{0}\tt.is_ms_shipped = 0{0}ORDER BY{0}\tLOWER(t.name)", Environment.NewLine);
			}

			public static string GetStoredProcList()
			{
				return "SELECT LOWER(so.name), sc.text FROM sysobjects so JOIN syscomments sc ON (sc.id = so.id) WHERE so.type ='P' AND so.category = 0 ORDER BY so.name, sc.colid";
			}

			public static string GetViewList()
			{
				return @"
					SELECT 
						o.name, 
						COALESCE(m.definition, '') AS definiton 
					FROM 
						sys.objects o 
					LEFT JOIN 
						sys.sql_modules m ON (m.object_id = o.object_id) 
					WHERE 
						o.type = 'V'
					ORDER BY 
						o.name";
			}

			public static string FindColumns()
			{
				return String.Format("SELECT {0}\ttables.name \"Table\", {0}\tcolumns.name \"Column\", {0}\tstype.name + ' (' + CAST(columns.length AS VARCHAR) + ')' \"Column definition\"{0}FROM {0}\tsysobjects tables {0}JOIN{0}\tsyscolumns columns ON (tables.id = columns.id) {0}JOIN {0}\tsystypes stype ON (columns.xtype = stype.xusertype){0}WHERE {0}\ttables.xtype = 'U' {0}\tAND tables.name NOT LIKE 'sys%' {0}\tAND columns.name LIKE @SearchString {0}ORDER BY {0}\ttables.name{0}", Environment.NewLine);
			}
		}

		public enum TableSelectLimit
		{
			None,
			LimitTop,
			LimitBottom,
		}

		public enum SelectionShape
		{ 
			Column,
			Row
		}
	}
}
