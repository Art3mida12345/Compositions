using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Text;
using WorkOfFiction.Enums;

namespace WorkOfFiction.Helpers
{
    public class OracleHelper
    {
        private readonly Dictionary<TableName, string> _sequences = new Dictionary<TableName, string>
        {
            {TableName.Types, "types_seq.NEXTVAL" }
        };
        private readonly Dictionary<TableName, string> _tables = new Dictionary<TableName, string>
        {
            {TableName.Types, "kudriavtseva_types.type_id, kudriavtseva_types.name"}
        };

        private readonly string _connection =
            "Data Source=" +
            $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Environment.MachineName})" +
            "(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));" +
            "User Id=LYB;Password=123;";

        public void Insert(TableName tableName, params string[] values)
        {
            using (var conn = new OracleConnection(_connection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                var valuesString = CreateStringWithSeparator(values);

                cmd.CommandText = $"insert into kudriavtseva_types ({_tables[tableName]}) " +
                                  $"values({_sequences[tableName]}, {valuesString})";
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Models.Type> GetAllTypes()
        {
            var queryString = "select * from kudriavtseva_types";
            var types = new List<Models.Type>();

            using (var connection = new OracleConnection(_connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(new Models.Type
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return types;
        }

        private string CreateStringWithSeparator(params string[] values)
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
    }
}