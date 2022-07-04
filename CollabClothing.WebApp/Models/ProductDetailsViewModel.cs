using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Catalog.Color;
using CollabClothing.ViewModels.Catalog.ProductImages;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Catalog.Sizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Models
{
    public class ProductDetailsViewModel
    {
        public CategoryViewModel Categories { get; set; }
        public ProductViewModel Products { get; set; }
        public List<ProductViewModel> ProductRelated { get; set; }
        public List<ProductImageViewModel> ProductsImages { get; set; }
        public BrandViewModel Brands { get; set; }
        public SizeViewModel Size { get; set; }
        public ColorViewModel Color { get; set; }
    }
}
