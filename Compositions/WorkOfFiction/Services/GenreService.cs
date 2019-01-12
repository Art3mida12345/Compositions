using System.Collections.Generic;
using System.Data.OracleClient;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;
using static WorkOfFiction.Helpers.DbDictionaries;

namespace WorkOfFiction.Services
{
    public class GenreService
    {
        private readonly OracleHelper _oracleHelper;

        public GenreService(OracleHelper oracleHelper)
        {
            _oracleHelper = oracleHelper;
        }

        public bool CheckIfAlreadyExist(Genre genre)
        {
            var queryString = $"select 1 from {Tables[TableName.Genres]} where name = {genre.Name}";
            var countries = new List<Country>();

            using (var connection = new OracleConnection(_oracleHelper.Connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }

            return false;
        }

        public void Insert(Genre genre)
        {
            _oracleHelper.Insert(TableName.Genres, genre.ToStringExtension(false));
        }

        public void Update(Genre genre)
        {
            _oracleHelper.Update(TableName.Genres, genre.ToStringExtension());
        }

        public void Delete(int genreId)
        {
            _oracleHelper.Delete(TableName.Genres, genreId);
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            var queryString = $"select * from {Tables[TableName.Genres]}";
            var genres = new List<Genre>();

            using (var connection = new OracleConnection(_oracleHelper.Connection))
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

        public Genre GetGenre(int? id)
        {
            if (id.HasValue)
            {
                var queryString = $"select name from {Tables[TableName.Genres]} where genre_id = {id}";
                var genre = new Genre();

                using (var connection = new OracleConnection(_oracleHelper.Connection))
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
    }
}