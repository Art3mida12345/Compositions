using System;
using System.ComponentModel.DataAnnotations;

namespace WorkOfFiction.Models
{
    public class Author
    {
        public int? Id { get; set; }

        [MaxLength(127)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(127)]
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Birth")]
        [Required]
        public DateTime DateBirth { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Death")]
        public DateTime? DateDeath { get; set; }

        [Display(Name = "Country")]
        [Required]
        public int? CountryId { get; set; }

        public Country Country { get; set; }

        [MaxLength(127)]
        [Required]
        public string Nickname { get; set; }
    }
}