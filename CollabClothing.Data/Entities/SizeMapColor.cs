using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class SizeMapColor
    {
        public string ColorId { get; set; }
        public string SizeId { get; set; }
        public Size Size { get; set; }
        public Color Color { get; set; }
        public int Quantity { get; set; }
    }
}
