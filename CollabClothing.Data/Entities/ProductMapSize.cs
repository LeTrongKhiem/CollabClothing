using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class ProductMapSize
    {
        public Product Product { get; set; }
        public Size Size { get; set; }

        public string ProductId { get; set; }

        public string SizeId { get; set; }
    }
}
