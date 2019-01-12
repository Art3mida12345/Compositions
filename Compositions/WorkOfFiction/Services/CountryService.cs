using System.Collections.Generic;
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

        public IEnumerable<Country> GetAll()
        {
            var countries = _oracleHelper.GetAllCountries();

            return countries;
        }

        public Country GetCountry(int? id)
        {
            var country = _oracleHelper.GetCountry(id);

            return country;
        }

        public void Insert(Country country)
        {

        }
    }
}