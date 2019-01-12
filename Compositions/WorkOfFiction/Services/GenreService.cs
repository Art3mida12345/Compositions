
using System.Collections.Generic;
using System.Data.OracleClient;
using WorkOfFiction.Constants;
using WorkOfFiction.Enums;
using WorkOfFiction.Models;
using static WorkOfFiction.Helpers.DbDictionaries;

namespace WorkOfFiction.Services
{
    public class GenreService
    {
        public bool CheckIfAlreadyExist(Genre genre)
        {
            var queryString = $"select 1 from {_tables[TableName.Genres]} where name = {genre.Name}";
            var countries = new List<Country>();

            using (var connection = new OracleConnection(GlobalConstants.Connection))
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

    }
}