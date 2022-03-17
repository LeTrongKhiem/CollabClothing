using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class SizeMapColor
    {
        public string Sizeid { get; set; }
        public string ColorId { get; set; }
        public int Quantity { get; set; }

        public virtual Color Color { get; set; }
        public virtual Size Size { get; set; }
    }
}
