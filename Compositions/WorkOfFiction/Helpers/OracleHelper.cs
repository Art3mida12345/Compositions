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

        public bool CheckIfAlreadyExist(TableName tableName, string columnName, string columnValue)
        {
            var queryString = $"select 1 from {Tables[tableName]} where {columnName} = '{columnValue}'";

            using (var connection = new OracleConnection(Connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        #endregion

        #region GetAll

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
        #endregion

        #region GetOne

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
                            composition.Annotation = reader.GetString(1);
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