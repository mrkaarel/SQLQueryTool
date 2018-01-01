using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlQueryTool.DatabaseObjects
{
    public class CommandParameter
    {
        public static readonly string[] SupportedTypes = {"string", "int", "decimal", "bit", "date", "NULL"};

        public CommandParameter(string name, string rawValue, string type)
        {
            Name = name;
            Value = GetParameterValue(rawValue, type);

            OriginalValue = rawValue;
        }

        public string Name { get; }
        public object Value { get; }

        public string OriginalValue { get; }

        private static object GetParameterValue(string value, string type)
        {
            switch (type)
            {
                case "int":
                    return int.Parse(value);
                case "bit":
                    if (value == "1") return true;
                    if (value == "0") return false;
                    throw new Exception($"Invalid bit value: {value}");
                case "decimal":
                    return decimal.Parse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                        new CultureInfo("").NumberFormat);
                case "date":
                    return DateTime.Parse(value);
                case "NULL":
                    return DBNull.Value;
                case "string":
                    return value;
                default:
                    throw new ArgumentException($"Unimplemented type: {type}");
            }
        }

        public static string GuessValueType(string value)
        {
            if (value == "0" || value == "1") return "bit";
            if (Regex.IsMatch(value, @"^-?\d{1,10}$")) return "int";
            if (Regex.IsMatch(value, @"^-?\d{0,10}\.?\d{0,10}$")) return "decimal";
            if (Regex.IsMatch(value, @"^\d{4}\.\d{1,2}\.\d{1,2}(?: \d{2}:\d{2}(?::\d{2})?)?$")) return "date";
            if (value == "NULL") return "NULL";

            return "string";
        }

        public static IEnumerable<string> ParseParameterNames(string queryText)
        {
            var queryTextCleaned = Regex.Replace(queryText, "'.*?'", "");
            return Regex.Matches(queryTextCleaned, "@([a-zA-Z0-9_]+)").Cast<Match>().Select(m => m.Groups[1].Value)
                .Distinct();
        }
    }
}