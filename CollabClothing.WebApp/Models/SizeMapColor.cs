using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Keyless]
    [Table("SizeMapColor")]
    public partial class SizeMapColor
    {
        [Required]
        [StringLength(255)]
        public string Sizeid { get; set; }
        [Required]
        [StringLength(255)]
        public string ColorId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(ColorId))]
        public virtual Color Color { get; set; }
        [ForeignKey(nameof(Sizeid))]
        public virtual Size Size { get; set; }
    }
}
