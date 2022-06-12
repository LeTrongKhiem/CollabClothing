using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class ProductDetail
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Details { get; set; }

        public bool Consumer { get; set; }
        public string Type { get; set; }
        public string Form { get; set; }
        public bool Cotton { get; set; }
        public string MadeIn { get; set; }
        public virtual Product Product { get; set; }
    }
}
