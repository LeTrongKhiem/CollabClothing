using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products.Manage
{
    public class ProductCreateRequest
    {
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
        public IFormFile ThumbnailImage { get; set; }
        public ProductImage productImage { get; set; }

        public Category Category { get; set; }
        public Size Size { get; set; }

    }
}
