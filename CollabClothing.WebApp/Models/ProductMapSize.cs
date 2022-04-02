using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class ProductMapSize
    {
        public string ProductId { get; set; }
        public string SizeId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
    }
}
