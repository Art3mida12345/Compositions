using System;
using System.ComponentModel.DataAnnotations;

namespace WorkOfFiction.Models
{
    public class Author
    {
        public int Id { get; set; }

        [MaxLength(127)]
        public string FirstName { get; set; }

        [MaxLength(127)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateBirth { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateDeath { get; set; }

        public int? CountryId { get; set; }

        public Country Country { get; set; }

        [MaxLength(127)]
        public string Nickname { get; set; }
    }
}