using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Cart
{
    public class OrderDetailsViewModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string? SizeId { get; set; }
        public string? ColorId { get; set; }
        public decimal Price { get; set; }
    }
}
