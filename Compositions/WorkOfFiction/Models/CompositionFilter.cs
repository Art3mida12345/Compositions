using System.Collections.Generic;


namespace WorkOfFiction.Models
{
    public class CompositionFilter
    {
        public string partialTextTitle { get; set; }
        public List<CheckBoxViewModel> Genres { get; set; }

        public List<CheckBoxViewModel> Authors { get; set; }

        public List<CheckBoxViewModel> Types { get; set; }

        public List<CheckBoxViewModel> Langs { get; set; }


        public List<int> SelectedGenres { get; set; }

        public List<int> SelectedAuthors { get; set; }

        public List<int> SelectedTypes { get; set; }

        public List<int> SelectedLangs { get; set; }

        public  CompositionFilter()
        {
            Genres = new List<CheckBoxViewModel>();
            Authors = new List<CheckBoxViewModel>();
            Types = new List<CheckBoxViewModel>();
            Langs = new List<CheckBoxViewModel>();

            SelectedGenres= new List<int>();
            SelectedAuthors= new List<int>();
            SelectedTypes= new List<int>();
            SelectedLangs= new List<int>();
        }

    }
}