using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class ProductMapColor
    {
        public string ProductId { get; set; }
        public string ColorId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Color Color { get; set; }
    }
}
