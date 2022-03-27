using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Catalog.ProductImages;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "You must input product name!!!")]
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
        public IFormFile ThumbnailImage { get; set; }
        // public ProductImageViewModel productImage { get; set; }
        // public CategoryViewModel CategoryViewModel { get; set; }
        // public Size Size { get; set; }

    }
}
