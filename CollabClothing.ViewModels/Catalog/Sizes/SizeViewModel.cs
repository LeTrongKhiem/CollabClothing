using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Sizes
{
    public class SizeViewModel
    {
        [Display(Name = "Ma size")]
        public string Id { get; set; }
        [Display(Name = "Ten size")]
        public string Name { get; set; }
    }
}
