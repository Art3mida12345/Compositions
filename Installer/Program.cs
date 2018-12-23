using System;
using Oracle.ManagedDataAccess.Client;

namespace Installer
{
    class Program
    {
        private static readonly string ConnectionToSys =
            $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Environment.MachineName})(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));User Id=SYS;Password=123;DBA Privilege=SYSDBA";
        private static readonly string ConnectionToLyb =
            $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Environment.MachineName})(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));User Id=LYB;Password=123;";

        private static void CreateUser()
        {
            var conn = new OracleConnection(ConnectionToSys);
            Console.WriteLine("Start to create user...");
            conn.Open();
            var cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = "create user lyb identified by 123";
                cmd.ExecuteNonQuery();
                Console.WriteLine("User was created successfully...");
            }
            catch
            {
                try
                {
                    Console.WriteLine("User already exist...");
                    Console.WriteLine("Grant privileges to user...");

                    cmd.CommandText = "grant connect, resource to lyb";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "grant create view to lyb";
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Empty scheme was created successfully...");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            finally
            {
                conn.Close();
            }
        }

        private static void CreateTables()
        {
            var conn = new OracleConnection(ConnectionToLyb);
            try
            {
                Console.WriteLine("Start to create tables...");
                conn.Open();
                var cmd = conn.CreateCommand();

                try
                {
                    cmd.CommandText = @"create table kudriavtseva_countries (
    country_id int,
    country_name varchar(255) not null,
    exist number(1) default 0 not null,
    capital varchar(60),
    constraint countries_pk primary key(country_id)
)
";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("1 was created successfully...");
                }
                catch
                {
                    Console.WriteLine("1-Table already exists");
                }//Countries-1

                try
                {
                    cmd.CommandText = @"create table kudriavtseva_languages(
    language_id integer,
    short_code varchar(2) not null,
    description varchar(40) not null,
    constraint languages_pk primary key(language_id)
)
";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("2 was created successfully...");
                }
                catch
                {
                    Console.WriteLine("2-Table already exists");
                }//Languages-2

                try
                {
                    cmd.CommandText = @"create table kudriavtseva_types (
    type_id int,
    name varchar(50) not null,
    constraint types_pk  primary key(type_id)
)
";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("3 was created successfully...");
                }
                catch
                {
                    Console.WriteLine("3-Table already exists");
                }//Types-3

                try
                {
                    cmd.CommandText = @"create table kudriavtseva_genres(
  genre_id integer, 
  name varchar(50) not null,
  constraint genres_pk PRIMARY KEY(genre_id)
)
";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("4 was created successfully...");
                }
                catch
                {
                    Console.WriteLine("4-Table already exists");
                }//Genres-4

                try
                {
                    cmd.CommandText = @"create table kudriavtseva_authors (
  author_id int not null ,
  first_name varchar(127) not null,
  last_name varchar(127) not null,
  date_birth date,
  date_death date,
  country_id integer,
  nickname varchar(127),
  constraint authors_author_id_pk primary key (author_id),
  constraint authors_country_id_fk foreign key (country_id) references kudriavtseva_countries(country_id)
)
";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("5 was created successfully...");
                }
                catch
                {
                    Console.WriteLine("5-Table already exists");
                }//Authors-5

                try
                {
                    cmd.CommandText = @"create table kudriavtseva_compositions (
  composition_id int not null ,
  title varchar(255) not null,
  annotation varchar(1000) not null,
  language_id int not null,
  type_id int not null,
  constraint composition_pk primary key (composition_id),
  constraint language_fk foreign key (language_id) references kudriavtseva_languages,
  constraint types_fk foreign key (type_id) references kudriavtseva_types
)
";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("6 was created successfully...");
                }
                catch
                {
                    Console.WriteLine("6-Table already exists");
                }//Compositions-6

                try
                {
                    cmd.CommandText = @"create table kudriavtseva_comps_genres(
  genre_id integer,
  composition_id integer,
  constraint comps_genres_pk primary key(genre_id, composition_id),
  constraint genres_fk foreign key (genre_id) references kudriavtseva_genres,
  constraint composition_fk foreign key (composition_id) references kudriavtseva_compositions
)
";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("7 was created successfully...");
                }
                catch
                {
                    Console.WriteLine("7-Table already exists");
                }//Compositions-Authors-7

                try
                {
                    cmd.CommandText = @"create table kudriavtseva_comps_authors(
  composition_id integer,
  author_id integer,
  constraint comps_authors_pk primary key(author_id, composition_id),
  constraint author_fk foreign key (author_id) references kudriavtseva_authors,
  constraint compositions_authors_fk foreign key (composition_id) references kudriavtseva_compositions
)
";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("8 was created successfully...");
                }
                catch
                {
                    Console.WriteLine("8-Table already exists");
                }//Compositions-Genres-8

                Console.WriteLine("Tables creates successfully...");
            }
            catch
            {
                Console.WriteLine("Something went wrong( Try again");
            }
            finally
            {
                conn.Close();
            }
        }

        private static void CreateSequences()
        {

        }

        static void Main()
        {
            CreateUser();
            CreateTables();
            Console.ReadLine();
        }
    }
}