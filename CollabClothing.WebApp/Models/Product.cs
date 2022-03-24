using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            ProductDetails = new HashSet<ProductDetail>();
            ProductImages = new HashSet<ProductImage>();
            ProductMapCategories = new HashSet<ProductMapCategory>();
            ProductMapSizes = new HashSet<ProductMapSize>();
            Promotions = new HashSet<Promotion>();
        }

        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal PriceCurrent { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal PriceOld { get; set; }
        public int? SaleOff { get; set; }
        [Required]
        [StringLength(10)]
        public string BrandId { get; set; }
        public bool SoldOut { get; set; }
        public int? Installment { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Slug { get; set; }
        public int ViewCount { get; set; }

        [ForeignKey(nameof(BrandId))]
        [InverseProperty("Products")]
        public virtual Brand Brand { get; set; }
        [InverseProperty(nameof(Cart.Product))]
        public virtual ICollection<Cart> Carts { get; set; }
        [InverseProperty(nameof(ProductDetail.Product))]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        [InverseProperty(nameof(ProductImage.Product))]
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        [InverseProperty(nameof(ProductMapCategory.Product))]
        public virtual ICollection<ProductMapCategory> ProductMapCategories { get; set; }
        [InverseProperty(nameof(ProductMapSize.Product))]
        public virtual ICollection<ProductMapSize> ProductMapSizes { get; set; }
        [InverseProperty(nameof(Promotion.Product))]
        public virtual ICollection<Promotion> Promotions { get; set; }
    }
}
