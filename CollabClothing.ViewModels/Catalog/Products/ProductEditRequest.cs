using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class ProductEditRequest
    {
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Chi tiết")]
        public string Details { get; set; }
        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; }
        [Display(Name = "Thương hiệu")]

        public string BrandId { get; set; }
        [Display(Name = "Slug")]
        public string Slug { get; set; }
        [Display(Name = "Hình ảnh mới")]
        public IFormFile ThumbnailImage { get; set; }
        [Display(Name = "Hình ảnh")]
        public string ImagePath { get; set; }
        // public ProductImage productImage { get; set; }
        [Display(Name = "Giá hiện tại")]
        public decimal PriceCurrent { get; set; }
        [Display(Name = "Giá cũ")]
        public decimal PriceOld { get; set; }
        [Display(Name = "Người sử dụng")]
        public bool Consumer { get; set; }
        [Display(Name = "Loại sản phẩm")]
        public string? Type { get; set; }
        [Display(Name = "Kiểu dáng")]
        public string? Form { get; set; }
        [Display(Name = "Chất liệu vải")]
        public bool Cotton { get; set; }
        [Display(Name = "Sản xuất tại")]
        public string? MadeIn { get; set; }
    }
}
