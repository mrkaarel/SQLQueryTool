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
    }
}
