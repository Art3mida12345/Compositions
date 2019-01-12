

using System.Collections.Generic;

namespace WorkOfFiction.Models
{
    public class FilterCompositionsHelperModel
    {
        public FilterCompositionsHelperModel(IEnumerable<Composition> compositions)
        {
            Compositions = compositions;
        }

        public IEnumerable<Composition> Compositions { get; set; }

        public CompositionFilter FilterModel { get; set; }
    }
}