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

            using (var connection = new OracleConnection(Connection))
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

        public void Insert(Country country)
        {

        }
    }
}