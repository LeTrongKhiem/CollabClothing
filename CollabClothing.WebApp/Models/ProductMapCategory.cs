using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("ProductMapCategory")]
    public partial class ProductMapCategory
    {
        [Key]
        [StringLength(255)]
        public string ProductId { get; set; }
        [Key]
        [StringLength(255)]
        public string CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("ProductMapCategories")]
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty("ProductMapCategories")]
        public virtual Product Product { get; set; }
    }
}
