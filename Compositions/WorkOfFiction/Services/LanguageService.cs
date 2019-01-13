using System.Collections.Generic;
using System.Data.OracleClient;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;
using static WorkOfFiction.Helpers.DbDictionaries;

namespace WorkOfFiction.Services
{
    public class LanguageService
    {
        private readonly OracleHelper _oracleHelper;

        public LanguageService(OracleHelper oracleHelper)
        {
            _oracleHelper = oracleHelper;
        }

        public bool CheckIfAlreadyExist(Language language)
        {
            return _oracleHelper.CheckIfAlreadyExist(TableName.Languages, "description", language.Description);
        }

        public void Insert(Language language)
        {
            _oracleHelper.Insert(TableName.Languages, language.ToStringExtension(false));
        }

        public void Update(Language language)
        {
            _oracleHelper.Update(TableName.Languages, language.Id, language.ToStringExtension());
        }

        public void Delete(int languageId)
        {
            _oracleHelper.Delete(TableName.Languages, languageId);
        }

        public IEnumerable<Language> GetAllLanguages()
        {
            var queryString = $"select * from {Tables[TableName.Languages]}";
            var languages = new List<Language>();

            using (var connection = new OracleConnection(_oracleHelper.Connection))
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

        public Language GetLanguage(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $"select {StringHelper.CreateStringWithSeparator(Columns[TableName.Languages])} from {Tables[TableName.Languages]} where {Keys[TableName.Languages]} = {id}";
                var language = new Language();

                using (var connection = new OracleConnection(_oracleHelper.Connection))
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
    }
}