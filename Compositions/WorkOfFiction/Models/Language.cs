using System.ComponentModel.DataAnnotations;

namespace WorkOfFiction.Models
{
    public class Language
    {
        public int? Id { get; set; }

        [MaxLength(2)]
        [Display(Name = "Short Code")]
        [Required]
        public string ShortCode { get; set; }

        [MaxLength(40)]
        [Required]
        public string Description { get; set; }
    }
}