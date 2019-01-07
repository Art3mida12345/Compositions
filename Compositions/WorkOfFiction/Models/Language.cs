using System.ComponentModel.DataAnnotations;

namespace WorkOfFiction.Models
{
    public class Language
    {
        public int Id { get; set; }

        [MaxLength(2)]
        public string ShortCode { get; set; }

        [MaxLength(40)]
        public string Description { get; set; }
    }
}