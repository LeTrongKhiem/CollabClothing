using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("Brand")]
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [StringLength(10)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string NameBrand { get; set; }
        [Required]
        [StringLength(255)]
        public string Info { get; set; }
        [Required]
        [StringLength(255)]
        public string Images { get; set; }
        [Required]
        [StringLength(255)]
        public string Slug { get; set; }

        [InverseProperty(nameof(Product.Brand))]
        public virtual ICollection<Product> Products { get; set; }
    }
}
