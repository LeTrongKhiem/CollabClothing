using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Models
{
    public class CartItemViewModel
    {
        public string productId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string Image { get; set; }
        public string BrandName { get; set; }
        public string Type { get; set; }
    }
}
