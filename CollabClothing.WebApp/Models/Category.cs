using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class Category
    {
        public Category()
        {
            ProductMapCategories = new HashSet<ProductMapCategory>();
        }

        public string Id { get; set; }
        public string NameCategory { get; set; }
        public string ParentId { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public bool IsShowWeb { get; set; }
        public string Slug { get; set; }

        public virtual ICollection<ProductMapCategory> ProductMapCategories { get; set; }
    }
}
