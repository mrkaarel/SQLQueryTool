using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlQueryTool.DatabaseObjects
{
    public static class DatabaseExtensions
    {
        public static string ForQueries(this string name)
        {
            if (DatabaseConstants.ReservedWords.Contains(name.ToLower()))
            {
                return string.Format("\"{0}\"", name);
            }

            return name;
        }
        
        public static string ForSelectQueries(this ColumnDefinition columnDefinition)
        {
            var name = ForQueries(columnDefinition.Name);
            if (columnDefinition.Type.IsBinary)
            {
                return string.Format("CAST({0} AS VARCHAR(MAX)) {0}", name);
            }

            return name;
        }
    }
}
