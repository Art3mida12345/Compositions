using System.Collections.Generic;
using WorkOfFiction.Enums;

namespace WorkOfFiction.Helpers
{
    public static class DbDictionaries
    {
        public static readonly Dictionary<TableName, string> Sequences = new Dictionary<TableName, string>
        {
            {TableName.Types, "types_seq.NEXTVAL"},
            {TableName.Authors, "authors_seq.NEXTVAL"},
            {TableName.Compositions, "compositions_seq.NEXTVAL"},
            {TableName.Countries, "countries_seq.NEXTVAL"},
            {TableName.Languages, "languages_seq.NEXTVAL"},
            {TableName.Genres, "genres_seq.NEXTVAL"}
        };

        public static readonly Dictionary<TableName, string> Headers = new Dictionary<TableName, string>
        {
            {TableName.Types, "kudriavtseva_types.type_id, kudriavtseva_types.name"},
            {
                TableName.Authors,
                @"kudriavtseva_authors.author_id, kudriavtseva_authors.first_name, kudriavtseva_authors.last_name,
kudriavtseva_authors.date_birth, kudriavtseva_authors.date_death, kudriavtseva_authors.country_id, kudriavtseva_authors.nickname"
            },
            {
                TableName.Compositions,
                @"kudriavtseva_compositions.composition_id, kudriavtseva_compositions.title, kudriavtseva_compositions.annotation,
kudriavtseva_compositions.language_id, kudriavtseva_compositions.type_id"
            },
            {
                TableName.Countries,
                "kudriavtseva_countries.country_id, kudriavtseva_countries.country_name, kudriavtseva_countries.exist, kudriavtseva_countries.capital"
            },
            {
                TableName.Languages,
                "kudriavtseva_languages.language_id, kudriavtseva_languages.short_code, kudriavtseva_languages.description"
            },
            {TableName.Genres, "kudriavtseva_genres.genre_id, kudriavtseva_genres.name"}
        };

        public static readonly Dictionary<TableName, string[]> Columns = new Dictionary<TableName, string[]>
        {
            {TableName.Types, new[] {"name"}},
            {
                TableName.Authors,
                new[] {"first_name", "last_name", "date_birth", "date_death", "country_id", "nickname"}
            },
            {TableName.Compositions, new[] {"title", "annotation", "language_id", "type_id"}},
            {TableName.Countries, new[] {"country_name", "exist", "capital"}},
            {TableName.Languages, new[] {"short_code", "description"}},
            {TableName.Genres, new[] {"name"}}
        };

        public static readonly Dictionary<TableName, string> Tables = new Dictionary<TableName, string>
        {
            {TableName.Types, "kudriavtseva_types"},
            {TableName.Authors, "kudriavtseva_authors"},
            {TableName.Compositions, "kudriavtseva_compositions"},
            {TableName.Countries, "kudriavtseva_countries"},
            {TableName.Languages, "kudriavtseva_languages"},
            {TableName.Genres, "kudriavtseva_genres"}
        };

        public static readonly Dictionary<TableName, string> Keys = new Dictionary<TableName, string>
        {
            {TableName.Types, "kudriavtseva_types.type_id"},
            {TableName.Authors, "kudriavtseva_authors.author_id"},
            {TableName.Compositions, "kudriavtseva_compositions.composition_id"},
            {TableName.Countries, "kudriavtseva_countries.country_id"},
            {TableName.Languages, "kudriavtseva_languages.language_id"},
            {TableName.Genres, "kudriavtseva_genres.genre_id"}
        };
    }
}