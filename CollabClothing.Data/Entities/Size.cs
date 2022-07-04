using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class Size
    {
        public Size()
        {
            ProductMapSizes = new HashSet<ProductMapSize>();
        }

        public string Id { get; set; }
        public string NameSize { get; set; }

        public virtual ICollection<ProductMapSize> ProductMapSizes { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
