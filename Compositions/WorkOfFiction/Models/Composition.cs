using System.ComponentModel.DataAnnotations;

namespace WorkOfFiction.Models
{
    public class Composition
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Annotation { get; set; }

        public int LanguageId { get; set; }

        public Language Language { get; set; }

        public int TypeId { get; set; }

        public Type Type { get; set; }
    }
}