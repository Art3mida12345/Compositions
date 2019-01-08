using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Text;
using WebGrease.Css.Extensions;
using WorkOfFiction.Enums;
using WorkOfFiction.Models;
using Type = WorkOfFiction.Models.Type;

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
            {TableName.Genres, "genres_seq.NEXTVAL" }
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
        private readonly Dictionary<TableName, string> _keys = new Dictionary<TableName, string>
        {
            {TableName.Types, "kudriavtseva_types.type_id" },
            {TableName.Authors, "kudriavtseva_authors.author_id"},
            {TableName.Compositions, "kudriavtseva_compositions.composition_id"},
            {TableName.Countries, "kudriavtseva_countries.country_id"},
            {TableName.Languages, "kudriavtseva_languages.language_id"},
            {TableName.Genres, "kudriavtseva_genres.genre_id"}
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

                cmd.CommandText = $"insert into {_tables[tableName]} ({_headers[tableName]}) " +
                                  $"values({_sequences[tableName]}, {valuesString})";
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

                cmd.CommandText = $"delete from {_tables[tableName]} where {_keys[tableName]} = {id}";
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region GetAll
        public IEnumerable<Type> GetAllTypes()
        {
            var queryString = $"select * from {_tables[TableName.Types]}";
            var types = new List<Type>();

            using (var connection = new OracleConnection(_connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(new Type
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return types;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            var queryString = $"select * from {_tables[TableName.Authors]}";
            var authors = new List<Author>();

            using (var connection = new OracleConnection(_connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        authors.Add(new Author
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            DateBirth = reader.GetDateTime(3),
                            CountryId = reader.GetInt32(4),
                            Nickname = reader.GetString(5)
                        });
                    }
                }
            }

            return authors;
        }

        public IEnumerable<Composition> GetAllCompositions()
        {
            var queryString = $"select * from {_tables[TableName.Compositions]}";
            var compositions = new List<Composition>();

            using (var connection = new OracleConnection(_connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        compositions.Add(new Composition
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Annotation = reader.GetString(2),
                            LanguageId = reader.GetInt32(3),
                            TypeId = reader.GetInt32(4)
                        });
                    }
                }
            }

            return compositions;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            var queryString = $"select * from {_tables[TableName.Countries]}";
            var countries = new List<Country>();

            using (var connection = new OracleConnection(_connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countries.Add(new Country
                        {
                            Id = reader.GetInt32(0),
                            CountryName = reader.GetString(1),
                            Exist = reader.GetBoolean(2),
                            Capital = reader.GetString(3)
                        });
                    }
                }
            }

            return countries;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            var queryString = $"select * from {_tables[TableName.Genres]}";
            var genres = new List<Genre>();

            using (var connection = new OracleConnection(_connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        genres.Add(new Genre
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return genres;
        }

        public IEnumerable<Language> GetAllLanguages()
        {
            var queryString = $"select * from {_tables[TableName.Languages]}";
            var languages = new List<Language>();

            using (var connection = new OracleConnection(_connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        languages.Add(new Language
                        {
                            Id = reader.GetInt32(0),
                            ShortCode = reader.GetString(1),
                            Description = reader.GetString(2)
                        });
                    }
                }
            }

            return languages;
        }
        #endregion

        #region GetOne
        public Type GetType(int? id)
        {
            if (id.HasValue)
            {
                var queryString = $"select name from kudriavtseva_types where type_id = {id}";
                var type = new Type();

                using (var connection = new OracleConnection(_connection))
                {
                    var command = new OracleCommand(queryString, connection);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            type.Id = id.Value;
                            type.Name = reader.GetString(0);
                        }
                    }
                }

                return type;
            }

            return null;
        }

        public Genre GetGenre(int? id)
        {
            if (id.HasValue)
            {
                var queryString = $"select name from {_tables[TableName.Genres]} where genre_id = {id}";
                var genre = new Genre();

                using (var connection = new OracleConnection(_connection))
                {
                    var command = new OracleCommand(queryString, connection);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            genre.Id = id.Value;
                            genre.Name = reader.GetString(0);
                        }
                    }
                }

                return genre;
            }

            return null;
        }

        public Language GetLanguage(int? id)
        {
            if (id.HasValue)
            {
                var queryString = $"select {CreateStringWithSeparator(_columns[TableName.Languages])} from {_tables[TableName.Languages]} where {_keys[TableName.Languages]} = {id}";
                var language = new Language();

                using (var connection = new OracleConnection(_connection))
                {
                    var command = new OracleCommand(queryString, connection);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            language.Id = id.Value;
                            language.ShortCode = reader.GetString(0);
                            language.Description = reader.GetString(1);
                        }
                    }
                }

                return language;
            }

            return null;
        }

        public Country GetCountry(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $"select {CreateStringWithSeparator(_columns[TableName.Countries])} from {_tables[TableName.Countries]} where country_id = {id}";
                var country = new Country();

                using (var connection = new OracleConnection(_connection))
                {
                    var command = new OracleCommand(queryString, connection);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            country.Id = id.Value;
                            country.CountryName = reader.GetString(0);
                            country.Exist = reader.GetBoolean(1);
                            country.Capital = reader.GetString(2);
                        }
                    }
                }

                return country;
            }

            return null;
        }

        public Author GetAuthor(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $"select {CreateStringWithSeparator(_columns[TableName.Authors])} from {_tables[TableName.Authors]} where country_id = {id}";
                var author = new Author();

                using (var connection = new OracleConnection(_connection))
                {
                    var command = new OracleCommand(queryString, connection);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            author.Id = id.Value;
                            author.FirstName = reader.GetString(0);
                            author.LastName = reader.GetString(1);
                            author.DateBirth = reader.GetDateTime(2);
                            author.DateDeath = reader.GetDateTime(3);
                            author.CountryId = reader.GetInt32(4);
                            author.Country = GetCountry(author.CountryId.GetValueOrDefault());
                            author.Nickname = reader.GetString(5);
                        }
                    }
                }

                return author;
            }

            return null;
        }

        public Composition GetComposition(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $"select {CreateStringWithSeparator(_columns[TableName.Compositions])} from {_tables[TableName.Compositions]} where composition_id = {id}";
                var composition = new Composition();

                using (var connection = new OracleConnection(_connection))
                {
                    var command = new OracleCommand(queryString, connection);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            composition.Id = id.Value;
                            composition.Title = reader.GetString(0);
                            composition.Annotation= reader.GetString(1);
                            composition.LanguageId = reader.GetInt32(2);
                            composition.TypeId = reader.GetInt32(3);
                        }
                    }
                }

                return composition;
            }

            return null;
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