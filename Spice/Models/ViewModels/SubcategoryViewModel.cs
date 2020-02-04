using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.ViewModels
{
    public class SubcategoryViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }
        public Subcategory subcategory { get; set; }
        public List<string> subcategoryList { get; set; }
        public string StatusMessage { get; set; }
    }
}
