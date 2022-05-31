using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Catalog.ProductImages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        // public string Id { get; set; }
        [Required(ErrorMessage = "You must input product name!!!")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Giá hiện tại")]
        public decimal PriceCurrent { get; set; }
        [Display(Name = "Giá cũ")]
        public decimal PriceOld { get; set; }
        [Display(Name = "Giảm giá (%)")]
        public int? SaleOff { get; set; }
        [Display(Name = "Thương hiệu")]
        public string BrandId { get; set; }
        [Display(Name = "Hết hàng")]
        public bool SoldOut { get; set; }
        [Display(Name = "Trả góp")]
        public int? Installment { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Slug")]
        public string Slug { get; set; }
        [Display(Name = "Chi tiết sản phẩm")]
        public string Details { get; set; }
        [Display(Name = "Ảnh sản phẩm")]
        public List<IFormFile> ThumbnailImage { get; set; }
        [Display(Name = "Danh mục")]
        public string CategoryId { get; set; }
        // public ProductImageViewModel productImage { get; set; }
        // public CategoryViewModel CategoryViewModel { get; set; }
        // public Size Size { get; set; }

    }
}
