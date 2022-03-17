using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class ProductImage
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Path { get; set; }
        public string Alt { get; set; }

        public virtual Product Product { get; set; }
    }
}
