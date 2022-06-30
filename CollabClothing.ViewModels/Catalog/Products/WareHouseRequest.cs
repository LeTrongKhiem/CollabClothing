using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class WareHouseRequest
    {
        public string ProductId { get; set; }
        public string Id { get; set; }
        public string SizeId { get; set; }
        public string ColorId { get; set; }
        public int Quantity { get; set; }
    }
}
