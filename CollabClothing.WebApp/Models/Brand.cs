using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public string Id { get; set; }
        public string NameBrand { get; set; }
        public string Info { get; set; }
        public string Images { get; set; }
        public string Slug { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
