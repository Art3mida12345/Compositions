using System.Text;
using WorkOfFiction.Enums;

namespace WorkOfFiction.Helpers
{
    public static class StringHelper
    {
        public static string CreateStringWithSeparator(params string[] values)
        {
            var stringBuilder = new StringBuilder();

            if (values.Length == 1)
            {
                return values[0];
            }

            foreach (var value in values)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(",");
            }

            return stringBuilder.ToString(0, stringBuilder.Length - 1);
        }

        public static string CreateStringWithEquals(TableName tableName, params string[] values)
        {
            if (DbDictionaries.Columns[tableName].Length == values.Length)
            {
                var stringBuilder = new StringBuilder();
                DbDictionaries.Columns[tableName].ForEach(value => values.ForEach(v =>
                    {
                        stringBuilder.Append(value);
                        stringBuilder.Append(" = ");
                        stringBuilder.Append(v);
                        stringBuilder.Append(", ");
                    }
                ));

                return stringBuilder.ToString(0, stringBuilder.Length - 2);
            }

            return null;
        }
    }
}