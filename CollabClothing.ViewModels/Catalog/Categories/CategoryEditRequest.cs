using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Categories
{
    public class CategoryEditRequest
    {
        public string CategoryName { get; set; }
        public string ParentId { get; set; }
        public int Level { get; set; }
        public bool IsShowWeb { get; set; }
        public string Slug { get; set; }
        public IFormFile Icon { get; set; }
    }
}
