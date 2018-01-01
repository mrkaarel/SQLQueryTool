using System.Linq;

namespace SqlQueryTool.DatabaseObjects
{
    public static class DatabaseExtensions
    {
        public static string ForQueries(this string name)
        {
            if (DatabaseConstants.ReservedWords.Contains(name.ToLower())) 
                return $"\"{name}\"";

            return name;
        }

        public static string ForSelectQueries(this ColumnDefinition columnDefinition)
        {
            var name = ForQueries(columnDefinition.Name);
            if (columnDefinition.Type.IsBinary) 
                return string.Format("CAST({0} AS VARCHAR(MAX)) {0}", name);

            return name;
        }
    }
}