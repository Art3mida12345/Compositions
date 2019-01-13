using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;
using static WorkOfFiction.Helpers.DbDictionaries;

namespace WorkOfFiction.Services
{
    public class CompositionService
    {
        private readonly OracleHelper _oracleHelper;
        private readonly AuthorService _authorService;
        private readonly LanguageService _languageService;
        private readonly TypeService _typeService;
        private readonly GenreService _genreService;

        public CompositionService(OracleHelper oracleHelper, AuthorService authorService, GenreService genreService, TypeService typeService, LanguageService languageService)
        {
            _oracleHelper = oracleHelper;
            _authorService = authorService;
            _genreService = genreService;
            _typeService = typeService;
            _languageService = languageService;
        }

        public CompositionDetails GetComposition(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $@"select kudriavtseva_compositions.title, kudriavtseva_compositions.annotation, kudriavtseva_languages.description, kudriavtseva_types.name from kudriavtseva_compositions 
inner join kudriavtseva_types on kudriavtseva_types.type_id = kudriavtseva_compositions.type_id 
inner join kudriavtseva_languages on kudriavtseva_languages.language_id = kudriavtseva_compositions.language_id
where kudriavtseva_compositions.composition_id ={id}";
                var composition = new CompositionDetails();

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
                            composition.Language = reader.GetString(2);
                            composition.Type = reader.GetString(3);
                        }
                    }
                }
                var authors =  _authorService.GetByComposition(id.Value);
                var temp = authors.Select(a => a.Id + "-" + a.FirstName + " " + a.LastName);
                composition.Authors = string.Join(", ", temp);

                var genres = _genreService.GetGenresByComposition(id.Value);
                temp = genres.Select(g => g.Name);
                composition.Genres = string.Join(", ", temp);

                return composition;
            }

            return null;
        }

        public Composition Get(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $@"select kudriavtseva_compositions.title, kudriavtseva_compositions.annotation, kudriavtseva_compositions.language_id, kudriavtseva_compositions.type_id from kudriavtseva_compositions 
where kudriavtseva_compositions.composition_id ={id}";
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

                composition.Authors = _authorService.GetByComposition(id.Value);
                composition.Language = _languageService.GetLanguage(composition.LanguageId);
                composition.Type = _typeService.GetType(composition.TypeId);
                composition.Genres = _genreService.GetGenresByComposition(id.Value);

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

        public bool Delete(int id)
        {
            try
            {
                var query =
                    $"delete from kudriavtseva_comps_authors where kudriavtseva_comps_authors.composition_id = {id}";
                ExecuteTransaction(query);

                query = $"delete from kudriavtseva_comps_genres where kudriavtseva_comps_genres.composition_id = {id}";
                ExecuteTransaction(query);

                query = $"delete from kudriavtseva_compositions where kudriavtseva_compositions.composition_id  = {id}";
                ExecuteTransaction(query);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Edit(Composition composition)
        {
            if (composition.Id.HasValue)
            {
                Delete(composition.Id.Value);
                Insert(composition);
            }
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

        private void ExecuteTransaction(string query)
        {
            using (var conn = new OracleConnection(_oracleHelper.Connection))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    }
}