﻿using System.Text;
using WebGrease.Css.Extensions;
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
                for (var i = 0; i < DbDictionaries.Columns[tableName].Length; i++)
                {
                        stringBuilder.Append(DbDictionaries.Columns[tableName][i]);
                        stringBuilder.Append(" = ");
                        stringBuilder.Append(values[i]);
                        stringBuilder.Append(", ");
                }

                return stringBuilder.ToString(0, stringBuilder.Length - 2);
            }

            return null;
        }
    }
}