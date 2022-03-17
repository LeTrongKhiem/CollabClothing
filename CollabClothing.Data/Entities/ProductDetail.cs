using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class ProductDetail
    {
        public string Id { get; set; }
        public Product ProductId { get; set; }
        public string Detais { get; set; } 
    }
}
