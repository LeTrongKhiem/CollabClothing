using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("Cart")]
    public partial class Cart
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("Carts")]
        public virtual Product Product { get; set; }
    }
}
