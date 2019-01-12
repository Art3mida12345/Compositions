using System.Collections.Generic;
using System.Data.OracleClient;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;
using static WorkOfFiction.Helpers.DbDictionaries;

namespace WorkOfFiction.Services
{
    public class CompositionService
    {
        private readonly OracleHelper _oracleHelper;

        public CompositionService(OracleHelper oracleHelper)
        {
            _oracleHelper = oracleHelper;
        }

        public Composition GetComposition(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $"select {StringHelper.CreateStringWithSeparator(Columns[TableName.Compositions])} from {Tables[TableName.Compositions]} where composition_id = {id}";
                var composition = new Composition();

                using (var connection = new OracleConnection(_oracleHelper.Connection))
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

        public IEnumerable<Composition> GetAllCompositions()
        {
            var queryString = $"select * from {Tables[TableName.Compositions]}";
            var compositions = new List<Composition>();

            using (var connection = new OracleConnection(_oracleHelper.Connection))
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
                            Annotation = reader.GetString(2)
                        });
                    }
                }
            }

            return compositions;
        }

        public void Insert(Composition composition)
        {
            var query =
                "insert into kudriavtseva_compositions " +
                "(kudriavtseva_compositions.composition_id, " +
                "kudriavtseva_compositions.title," +
                " kudriavtseva_compositions.annotation," +
                " kudriavtseva_compositions.language_id," +
                " kudriavtseva_compositions.type_id) " +
                $"values(compositions_seq.NEXTVAL, '{composition.Title}', '{composition.Annotation}', {composition.LanguageId}, {composition.TypeId})";

            Execute(query);

            var index = GetLastIndex();

            query = "insert all ";
            foreach (var id in composition.AuthorsIds)
            {
                query +=
                " into kudriavtseva_comps_authors " +
                "(kudriavtseva_comps_authors.composition_id," +
                " kudriavtseva_comps_authors.author_id) " +
                $"values({index}, {id})";
            }

            query += " SELECT * FROM dual";
            Execute(query);

            query = "insert all ";

            foreach (var id in composition.GenresIds)
            {
                query += " into kudriavtseva_comps_genres " +
                         "(kudriavtseva_comps_genres.composition_id, kudriavtseva_comps_genres.genre_id) " +
                         $"values({index}, {id})";
            }

            query += " SELECT * FROM dual";
            Execute(query);
        }

        private int GetLastIndex()
        {
            using (var connection = new OracleConnection(_oracleHelper.Connection))
            {
                var queryString = "select compositions_seq.CURRVAL from dual";
                var command = new OracleCommand(queryString, connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }

                return -1;
            }
        }

        private void Execute(string query)
        {
            using (var conn = new OracleConnection(_oracleHelper.Connection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }
    }
}