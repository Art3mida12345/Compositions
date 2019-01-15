using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Web;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;
using static WorkOfFiction.Helpers.DbDictionaries;


namespace WorkOfFiction.Services
{
    public class FilterService
    {
        private readonly OracleHelper _oracleHelper;

        public FilterService(OracleHelper oracleHelper)
        {
            _oracleHelper = oracleHelper;
        }

        public IEnumerable<Composition> ApplyFilter(CompositionFilter filter)
        {
            var queryString = CreateQueryString(filter);
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

        private string CreateQueryString(CompositionFilter filter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"select distinct kudriavtseva_compositions.* from {Tables[TableName.Compositions]}");
            bool isConditiondAdded = false;

            if (!filter.SelectedTypes.Any() && !filter.SelectedAuthors.Any() && !filter.SelectedLangs.Any() &&
                !filter.SelectedTypes.Any() && string.IsNullOrWhiteSpace(filter.partialTextTitle))
                return sb.ToString();

            sb.Append($" inner join " +
                      "kudriavtseva_comps_genres on " +
                      "kudriavtseva_comps_genres.composition_id = kudriavtseva_compositions.composition_id " +
                      "inner join " +
                      "kudriavtseva_genres on " +
                      "kudriavtseva_comps_genres.genre_id = kudriavtseva_genres.genre_id " +
                      "inner join " +
                      "kudriavtseva_comps_authors on " +
                      "kudriavtseva_comps_authors.composition_id = kudriavtseva_compositions.composition_id " +
                      "inner join " +
                      "kudriavtseva_authors on kudriavtseva_comps_authors.author_id = " +
                      "kudriavtseva_authors.author_id"
            );

            sb.Append(" where ");


            if (!string.IsNullOrWhiteSpace(filter.partialTextTitle))
            {
                sb.Append(ApplyPartialTextSearchFilter(filter.partialTextTitle, "title"));
                isConditiondAdded = true;
            }

            if (filter.SelectedGenres.Any())
            {
                sb.Append(ApplyStringIdsFilter(filter.SelectedGenres, "kudriavtseva_comps_genres.genre_id",
                    isConditiondAdded));
                isConditiondAdded = true;
            }

            if (filter.SelectedAuthors.Any())
            {
                sb.Append(ApplyStringIdsFilter(filter.SelectedAuthors, "kudriavtseva_comps_authors.author_id",
                    isConditiondAdded));
                isConditiondAdded = true;
            }

            if (filter.SelectedLangs.Any())
            {
                sb.Append(ApplyStringIdsFilter(filter.SelectedLangs, "language_id", isConditiondAdded));
                isConditiondAdded = true;
            }


            if (filter.SelectedTypes.Any())
            {
                sb.Append(ApplyStringIdsFilter(filter.SelectedTypes, "type_id", isConditiondAdded));
                isConditiondAdded = true;
            }


            return sb.ToString();
        }

        private string ApplyPartialTextSearchFilter(string partialText, string columnFiltered)
        {
            StringBuilder sb = new StringBuilder();

            var textList = partialText.Split(new[] {' ', '-', '.', '_'});


            sb.Append($"(upper({columnFiltered}) like upper('%{textList[0]}%')");

            for (int i = 1; i < textList.Length; i++)
            {
                sb.Append(" OR ");
                sb.Append($"{columnFiltered} like '%{textList[i]}%'");
            }

            sb.Append(")");


            return sb.ToString();
        }

        private string ApplyStringIdsFilter(List<int> ids, string columnFiltered, bool isConditiondAdded)
        {
            StringBuilder sb = new StringBuilder();
            ;
            if (ids.Any())
            {
                if (isConditiondAdded)
                    sb.Append(" AND ");

                sb.Append($"({columnFiltered} = '{ids[0]}'");

                for (int i = 1; i < ids.Count; i++)
                {
                    sb.Append(" OR ");
                    sb.Append($"{columnFiltered} = '{ids[i]}'");
                }

                sb.Append(")");
            }

            return sb.ToString();
        }
    }
}