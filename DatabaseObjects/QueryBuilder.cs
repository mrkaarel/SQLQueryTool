﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlQueryTool.DatabaseObjects
{
	public static class QueryBuilder
	{
		public static string BuildInsertQuery(this TableDefinition table)
		{
			var queryText = new StringBuilder(String.Format("INSERT INTO {0}{1}\t", table.Name, Environment.NewLine));
			
			string columnNames = String.Join(", ", table.Columns.Where(c => !c.IsIdentity).Select(c => c.Name).ToArray());
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
			return String.Format("SELECT{0}\tCOUNT(*){0}FROM{0}\t{1}", Environment.NewLine, tableName);
		}

		public static string BuildSelectQuery(this TableDefinition table, TableSelectLimit selectLimit, string whereClause = "")
		{
			var queryText = new StringBuilder("SELECT ");
			if (selectLimit != TableSelectLimit.None) {
				queryText.Append("TOP 100 ");
			}

			if (table.Columns.Count() > 0) {
				string columns = String.Join(", ", table.Columns.Select(c => String.Format("{0}\t{1}", Environment.NewLine, c.Name)).ToArray());
				queryText.Append(columns);
			}
			else { // for views
				queryText.AppendFormat("{0}\t*", Environment.NewLine);
			}

			queryText.AppendFormat("{0}FROM{0}\t{1}", Environment.NewLine, table.Name);

			if (!String.IsNullOrEmpty(whereClause)) {
				queryText.AppendFormat("{0}WHERE{0}\t{1}", Environment.NewLine, whereClause);
			}

			if (selectLimit == TableSelectLimit.LimitBottom) {
				queryText.AppendFormat("{0}ORDER BY", Environment.NewLine);
				if (table.IdentityColumn != null) {
					queryText.AppendFormat("{0}\t{1} DESC", Environment.NewLine, table.IdentityColumn.Name);
				}
				else {
					queryText.AppendFormat("{0}\t? DESC", Environment.NewLine);
				}
			}

			return queryText.ToString();
		}

		public static string BuildUpdateQuery(this TableDefinition table)
		{
			var queryText = new StringBuilder(String.Format("UPDATE {0}\t{1}{0}SET", Environment.NewLine, table.Name));

			string columns = String.Join(", ", table.Columns.Where(c => !c.IsIdentity).Select(c => String.Format("{0}\t{1} = {2}", Environment.NewLine, c.Name, c.FormattedValue)).ToArray());
			queryText.Append(columns);

			queryText.AppendFormat("{0}WHERE{0}\t{1}", Environment.NewLine, GetWhereClause(table));

			return queryText.ToString();
		}

		public static string BuildDeleteQuery(this TableDefinition table)
		{
			return String.Format("DELETE FROM {0}\t{1}{0}WHERE{0}\t{2}", Environment.NewLine, table.Name, GetWhereClause(table));
		}

		public static string BuildGrantExecuteOnSP(string spName, string userName = "xxx")
		{
			return String.Format("GRANT EXECUTE ON {0} TO {1}", spName, userName);
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
				return String.Format("{0} = ?", table.IdentityColumn.Name);
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

			public static string GetTableList()
			{
				return "SELECT LOWER(name) FROM [sysobjects] WHERE xtype = 'u' AND uid = 1 ORDER BY name";
			}

			public static string GetStoredProcList()
			{
				return "SELECT LOWER(so.name), sc.text FROM sysobjects so JOIN syscomments sc ON (sc.id = so.id) WHERE so.type ='P' AND so.category = 0 ORDER BY so.name, sc.colid";
			}

			public static string GetViewList()
			{
				return "SELECT o.name, m.definition FROM sys.objects o JOIN sys.sql_modules m ON (m.object_id = o.object_id) WHERE o.type = 'V' AND m.definition IS NOT NULL ORDER BY o.name";
			}

			public static string GetTableRowCounts()
			{
				return String.Format("SELECT {0}\tt.name \"Tabel\",{0}\tp.rows \"Ridasid\"{0}FROM {0}\tsys.tables t{0}INNER JOIN{0}\tsys.indexes i ON t.object_id = i.object_id{0}INNER JOIN{0}\tsys.partitions p on i.object_id = p.object_id and i.index_id = p.index_id{0}WHERE{0}\tt.is_ms_shipped = 0{0}GROUP BY{0}\tt.name, p.rows{0}ORDER BY{0}\tt.name{0}", Environment.NewLine);
			}

			public static string FindColumns(string searchString = "xxx")
			{
				return String.Format("SELECT {0}\ttables.name TableName, {0}\tcolumns.name ColumnName, {0}\tstype.name + ' (' + CAST(columns.length AS VARCHAR) + ')'{0}FROM {0}\tsysobjects tables {0}JOIN{0}\tsyscolumns columns ON (tables.id = columns.id) {0}JOIN {0}\tsystypes stype ON (columns.xtype = stype.xusertype){0}WHERE {0}\ttables.xtype = 'U' {0}\tAND tables.name NOT LIKE 'sys%' {0}\tAND columns.name LIKE '%{1}%' {0}ORDER BY {0}\ttables.name{0}", Environment.NewLine, searchString);
			}
		}

		public enum TableSelectLimit
		{
			None,
			LimitTop,
			LimitBottom,
		}
	}
}