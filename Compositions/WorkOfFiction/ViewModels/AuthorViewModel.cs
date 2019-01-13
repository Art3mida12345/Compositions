using System.ComponentModel.DataAnnotations;

namespace WorkOfFiction.ViewModels
{
    public class AuthorViewModel
    {
        public int? Id { get; set; }
        [Display(Name="Full Name")]
        public string FullName { get; set; }
    }
}