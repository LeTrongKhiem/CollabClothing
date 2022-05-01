using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class ProductEditRequest
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Details { get; set; }
        public string Description { get; set; }

        public string BrandId { get; set; }
        public string Slug { get; set; }
        public IFormFile ThumbnailImage { get; set; }
        public string ImagePath { get; set; }
        // public ProductImage productImage { get; set; }
    }
}
