using CollabClothing.ApiShared;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers.Components
{
    public class ProductOutStandingViewComponent : ViewComponent
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        public ProductOutStandingViewComponent(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            int quantity = 8;
            var listProduct = _productApiClient.GetFeaturedProductsByCategory(id, quantity);
            var category = await _categoryApiClient.GetById(id);
            var result = new HomeViewModel()
            {
                GetProductOutStanding = await listProduct,
                categoryId = id,
                slug = category.Slug
            };
            return View(result);
        }
    }
}
