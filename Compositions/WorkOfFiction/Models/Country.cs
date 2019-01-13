using System.ComponentModel.DataAnnotations;

namespace WorkOfFiction.Models
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(255)]
        [Display(Name = "Country Name")]
        [Required]
        public string CountryName { get; set; }

        public bool Exist { get; set; }

        [MaxLength(60)]
        [Required]
        public string Capital { get; set; }
    }
}