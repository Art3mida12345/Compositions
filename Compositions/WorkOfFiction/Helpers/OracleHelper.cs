using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using WorkOfFiction.Enums;
using WorkOfFiction.Models;
using static WorkOfFiction.Helpers.DbDictionaries;
using Type = WorkOfFiction.Models.Type;

namespace WorkOfFiction.Helpers
{
    public class OracleHelper
    {
        #region Connection
        public string Connection { get; } = 
            "Data Source=" +
            $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Environment.MachineName})" +
            "(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));" +
            "User Id=LYB;Password=123;";
        #endregion

        #region CUD
        public void Insert(TableName tableName, params string[] values)
        {
            using (var conn = new OracleConnection(Connection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                var valuesString = StringHelper.CreateStringWithSeparator(values);

                cmd.CommandText = $"insert into {Tables[tableName]} ({Headers[tableName]}) " +
                                  $"values({Sequences[tableName]}, {valuesString})";
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(TableName tableName, int? id, params string[] values)
        {
            if (id.HasValue)
            {
                var setString = StringHelper.CreateStringWithEquals(tableName, values);
                if (!string.IsNullOrEmpty(setString))
                {
                    using (var conn = new OracleConnection(Connection))
                    {
                        conn.Open();
                        var cmd = conn.CreateCommand();
                        var query = $"update {Tables[tableName]} set {setString} where {Keys[tableName]} = {id.Value}";
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Delete(TableName tableName, int id)
        {
            using (var conn = new OracleConnection(Connection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"delete from {Tables[tableName]} where {Keys[tableName]} = {id}";
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region GetAll
        public IEnumerable<Type> GetAllTypes()
        {
            var queryString = $"select * from {Tables[TableName.Types]}";
            var types = new List<Type>();

            using (var connection = new OracleConnection(Connection))
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
            var queryString = $"select * from {Tables[TableName.Authors]}";
            var authors = new List<Author>();

            using (var connection = new OracleConnection(Connection))
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
            var queryString = $"select * from {Tables[TableName.Compositions]}";
            var compositions = new List<Composition>();

            using (var connection = new OracleConnection(Connection))
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

       

        public IEnumerable<Language> GetAllLanguages()
        {
            var queryString = $"select * from {Tables[TableName.Languages]}";
            var languages = new List<Language>();

            using (var connection = new OracleConnection(Connection))
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

                using (var connection = new OracleConnection(Connection))
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

  

        public Language GetLanguage(int? id)
        {
            if (id.HasValue)
            {
                var queryString = $"select {StringHelper.CreateStringWithSeparator(Columns[TableName.Languages])} from {Tables[TableName.Languages]} where {Keys[TableName.Languages]} = {id}";
                var language = new Language();

                using (var connection = new OracleConnection(Connection))
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

        public Author GetAuthor(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $"select {StringHelper.CreateStringWithSeparator(Columns[TableName.Authors])} from {Tables[TableName.Authors]} where country_id = {id}";
                var author = new Author();

                using (var connection = new OracleConnection(Connection))
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
                            //author.Country = GetCountry(author.CountryId.GetValueOrDefault());
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
                    $"select {StringHelper.CreateStringWithSeparator(Columns[TableName.Compositions])} from {Tables[TableName.Compositions]} where composition_id = {id}";
                var composition = new Composition();

                using (var connection = new OracleConnection(Connection))
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
    }
}