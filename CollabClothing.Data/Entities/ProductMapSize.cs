using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class ProductMapSize
    {
        public string ProductId { get; set; }
        public string SizeId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
    }
}
