using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Appication.Catalog.Products.Dtos.Manage
{
    public class ProductEditRequest
    {
        public string Id { get; set; }
        public string Details { get; set; }
        public string Description { get; set; }

        public string BrandId { get; set; }
        public string Slug { get; set; }
    }
}
