using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkOfFiction.Models
{
    public class CheckBoxViewModel
    {
        public int Id { get; set; }

        public bool IsChecked { get; set; }

        public string Value { get; set; }
    }
}