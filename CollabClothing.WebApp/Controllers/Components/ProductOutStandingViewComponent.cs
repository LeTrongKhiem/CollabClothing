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
        public ProductOutStandingViewComponent(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            int quantity = 3;
            var listProduct = _productApiClient.GetFeaturedProductsByCategory(id, quantity);
            var result = new HomeViewModel()
            {
                GetProductOutStanding = await listProduct,
                categoryId = id
            };
            return View(result);
        }
    }
}
