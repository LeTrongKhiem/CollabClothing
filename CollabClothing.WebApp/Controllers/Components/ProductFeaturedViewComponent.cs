using CollabClothing.ApiShared;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers.Components
{
    public class ProductFeaturedViewComponent : ViewComponent
    {
        private readonly IProductApiClient _productApiClient;
        public ProductFeaturedViewComponent(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int rows = 12; // show 12 product
            var productFeatured = new HomeViewModel()
            {
                ListProductFeatured = await _productApiClient.GetFeaturedProducts(rows)
            };
            return View(productFeatured);
        }
    }
}
