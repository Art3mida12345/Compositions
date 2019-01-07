using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Text;
using WebGrease.Css.Extensions;
using WorkOfFiction.Enums;

namespace WorkOfFiction.Helpers
{
    public class OracleHelper
    {
        private readonly Dictionary<TableName, string> _sequences = new Dictionary<TableName, string>
        {
            {TableName.Types, "types_seq.NEXTVAL" },
            {TableName.Authors, "authors_seq.NEXTVAL" },
            {TableName.Compositions, "compositions_seq.NEXTVAL" },
            {TableName.Countries, "countries_seq.NEXTVAL" },
            {TableName.Languages, "languages_seq.NEXTVAL" },
            {TableName.Genres, "genre_seq.NEXTVAL" }
        };
        private readonly Dictionary<TableName, string> _headers = new Dictionary<TableName, string>
        {
            {TableName.Types, "kudriavtseva_types.type_id, kudriavtseva_types.name"},
            {TableName.Authors, @"kudriavtseva_authors.author_id, kudriavtseva_authors.first_name, kudriavtseva_authors.last_name,
kudriavtseva_authors.date_birth, kudriavtseva_authors.date_death, kudriavtseva_authors.country_id, kudriavtseva_authors.nickname" },
            {TableName.Compositions, @"kudriavtseva_compositions.composition_id, kudriavtseva_compositions.title, kudriavtseva_compositions.annotation,
kudriavtseva_compositions.language_id, kudriavtseva_compositions.type_id"},
            {TableName.Countries, "kudriavtseva_countries.country_id, kudriavtseva_countries.country_name, kudriavtseva_countries.exist, kudriavtseva_countries.capital"},
            {TableName.Languages, "kudriavtseva_languages.language_id, kudriavtseva_languages.short_code, kudriavtseva_languages.description"},
            {TableName.Genres, "kudriavtseva_genres.genre_id, kudriavtseva_genres.name" }
        };
        private readonly Dictionary<TableName, string[]> _columns = new Dictionary<TableName, string[]>
        {
            {TableName.Types, new [] {"name"} },
            {TableName.Authors, new []{"first_name", "last_name", "date_birth", "date_death", "country_id", "nickname"}},
            {TableName.Compositions,new []{"title", "annotation", "language_id", "type_id"}},
            {TableName.Countries, new []{"country_name", "exist", "capital"}},
            {TableName.Languages, new []{"short_code", "description"}},
            {TableName.Genres, new []{"name"} }
        };
        private readonly Dictionary<TableName, string> _tables = new Dictionary<TableName, string>
        {
            {TableName.Types, "kudriavtseva_types" },
            {TableName.Authors, "kudriavtseva_authors"},
            {TableName.Compositions, "kudriavtseva_compositions"},
            {TableName.Countries, "kudriavtseva_countries"},
            {TableName.Languages, "kudriavtseva_languages"},
            {TableName.Genres, "kudriavtseva_genres"}
        };

        #region Connection
        private readonly string _connection =
            "Data Source=" +
            $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Environment.MachineName})" +
            "(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));" +
            "User Id=LYB;Password=123;";
        #endregion

        #region CUD
        public void Insert(TableName tableName, params string[] values)
        {
            using (var conn = new OracleConnection(_connection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                var valuesString = CreateStringWithSeparator(values);

                cmd.CommandText = $"insert into kudriavtseva_types ({_headers[tableName]}) " +
                                  $"values({_sequences[tableName]}, '{valuesString}')";
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(TableName tableName, params string[] values)
        {
            var setString = CreateStringWithEquals(tableName, values);
            if (!string.IsNullOrEmpty(setString))
            {
                using (var conn = new OracleConnection(_connection))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    var query = $"update {_tables[tableName]} set {setString}";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(TableName tableName, int id)
        {
            using (var conn = new OracleConnection(_connection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"delete from {_tables[tableName]} where {_headers[tableName][0]} = {id}";
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region GetAll
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
        #endregion

        #region GetOne
        public Models.Type GetType(int id)
        {
            var queryString = $"select name from kudriavtseva_types where type_id = {id}";
            var type = new Models.Type();

            using (var connection = new OracleConnection(_connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        type.Id = id;
                        type.Name = reader.GetString(0);
                    }
                }
            }

            return type;
        }
        #endregion

        #region Tools
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

        private string CreateStringWithEquals(TableName tableName, params string[] values)
        {
            if (_columns[tableName].Length == values.Length)
            {
                var stringBuilder = new StringBuilder();
                _columns[tableName].ForEach(value => values.ForEach(v =>
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
        #endregion
    }
}