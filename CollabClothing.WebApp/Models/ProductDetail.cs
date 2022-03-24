using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("ProductDetail")]
    public partial class ProductDetail
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string ProductId { get; set; }
        [Column(TypeName = "text")]
        public string Details { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("ProductDetails")]
        public virtual Product Product { get; set; }
    }
}
