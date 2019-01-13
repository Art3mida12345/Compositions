using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkOfFiction.Models
{
    public class Composition
    {
        public int? Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string Title { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Annotation { get; set; }

        [Required]
        [Display(Name = "Language")]
        public int? LanguageId { get; set; }

        public Language Language { get; set; }

        [Required]
        [Display(Name = "Type")]
        public int? TypeId { get; set; }

        public Type Type { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        [Display(Name = "Genres")]
        [Required]
        public int?[] GenresIds { get; set; }

        public IEnumerable<Author> Authors { get; set; }

        [Required]
        [Display(Name = "Authors")]
        public int?[] AuthorsIds { get; set; }
    }
}