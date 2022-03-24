using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            ProductMapCategories = new HashSet<ProductMapCategory>();
        }

        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string NameCategory { get; set; }
        [Required]
        [StringLength(255)]
        public string ParentId { get; set; }
        [Required]
        [StringLength(255)]
        public string Icon { get; set; }
        public int Level { get; set; }
        public bool IsShowWeb { get; set; }
        [Required]
        [StringLength(255)]
        public string Slug { get; set; }

        [InverseProperty(nameof(ProductMapCategory.Category))]
        public virtual ICollection<ProductMapCategory> ProductMapCategories { get; set; }
    }
}
