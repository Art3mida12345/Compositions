﻿using System.Collections.Generic;
using System.Data.OracleClient;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;
using static WorkOfFiction.Helpers.DbDictionaries;

namespace WorkOfFiction.Services
{
    public class AuthorService
    {
        private readonly OracleHelper _oracleHelper;

        public AuthorService(OracleHelper oracleHelper)
        {
            _oracleHelper = oracleHelper;
        }



        public void Insert(Author author)
        {
            _oracleHelper.Insert(TableName.Authors, author.ToStringExtension(false));
        }

        public void Update(Author author)
        {
            _oracleHelper.Update(TableName.Authors, author.Id, author.ToStringExtension());
        }

        public void Delete(int authorId)
        {
            _oracleHelper.Delete(TableName.Authors, authorId);
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            var queryString = $"select * from {Tables[TableName.Authors]}";
            var authors = new List<Author>();

            using (var connection = new OracleConnection(_oracleHelper.Connection))
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

        public Author GetAuthor(int? id)
        {
            if (id.HasValue)
            {
                var queryString =
                    $"select {StringHelper.CreateStringWithSeparator(Columns[TableName.Authors])} from {Tables[TableName.Authors]} where country_id = {id}";
                var author = new Author();

                using (var connection = new OracleConnection(_oracleHelper.Connection))
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
    }
}