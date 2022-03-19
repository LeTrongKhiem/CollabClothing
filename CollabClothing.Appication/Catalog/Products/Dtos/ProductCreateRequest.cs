using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Appication.Catalog.Products.Dtos
{
    public class ProductCreateRequest
    {
        public int PriceCurrent { get; set; }
        public int PriceOld { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
    }
}
