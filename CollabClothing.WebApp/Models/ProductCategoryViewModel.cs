using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Models
{
    public class ProductCategoryViewModel
    {
        public CategoryViewModel Category { get; set; }
        public PageResult<ProductViewModel> Products { get; set; }
        public BrandViewModel Brand { get; set; }
    }
}
