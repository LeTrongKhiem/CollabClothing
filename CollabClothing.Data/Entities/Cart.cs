using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class Cart
    {
        public string Id { get; set; }
        public Product ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
