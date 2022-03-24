using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("ProductImage")]
    public partial class ProductImage
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string ProductId { get; set; }
        [StringLength(255)]
        public string Path { get; set; }
        [StringLength(255)]
        public string Alt { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("ProductImages")]
        public virtual Product Product { get; set; }
    }
}
