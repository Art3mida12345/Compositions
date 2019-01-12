using System.Collections.Generic;
using System.Data.OracleClient;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;

namespace WorkOfFiction.Services
{
    public class CountryService
    {
        private readonly OracleHelper _oracleHelper;

        public CountryService()
        {
            _oracleHelper = new OracleHelper();
        }

        public IEnumerable<Country> GetAllCountries()
        {
            var queryString = $"select * from {DbDictionaries.Tables[TableName.Countries]}";
            var countries = new List<Country>();

            using (var connection = new OracleConnection(_oracleHelper.Connection))
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

        public Country GetCountry(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $"select {CreateStringWithSeparator(Columns[TableName.Countries])} from {Tables[TableName.Countries]} where country_id = {id}";
                var country = new Country();

                using (var connection = new OracleConnection(Connection))
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

        public void Insert(Country country)
        {

        }
    }
}