using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class ProductMapCategory
    {
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
