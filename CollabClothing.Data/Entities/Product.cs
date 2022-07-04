using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            ProductDetails = new HashSet<ProductDetail>();
            ProductImages = new HashSet<ProductImage>();
            ProductMapCategories = new HashSet<ProductMapCategory>();
            ProductMapSizes = new HashSet<ProductMapSize>();
            ProductMapColors = new HashSet<ProductMapColor>();
            Promotions = new HashSet<Promotion>();
        }

        public string Id { get; set; }
        public string ProductName { get; set; }
        public decimal PriceCurrent { get; set; }
        public decimal PriceOld { get; set; }
        public int? SaleOff { get; set; }
        public string BrandId { get; set; }
        public bool SoldOut { get; set; }
        public int? Installment { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string Details { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductMapCategory> ProductMapCategories { get; set; }
        public virtual ICollection<ProductMapSize> ProductMapSizes { get; set; }
        public virtual ICollection<ProductMapColor> ProductMapColors { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }
    }
}
