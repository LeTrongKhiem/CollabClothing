using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("ProductMapSize")]
    public partial class ProductMapSize
    {
        [Key]
        [StringLength(255)]
        public string ProductId { get; set; }
        [Key]
        [StringLength(255)]
        public string SizeId { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("ProductMapSizes")]
        public virtual Product Product { get; set; }
        [ForeignKey(nameof(SizeId))]
        [InverseProperty("ProductMapSizes")]
        public virtual Size Size { get; set; }
    }
}
