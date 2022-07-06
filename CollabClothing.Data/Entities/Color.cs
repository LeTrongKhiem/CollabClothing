using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class Color
    {
        public Color()
        {
            ProductMapColors = new HashSet<ProductMapColor>();
        }
        public string Id { get; set; }
        public string NameColor { get; set; }
        public virtual ICollection<ProductMapColor> ProductMapColors { get; set; }
    }
}
