using CollabClothing.ApiShared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers.Components
{
    public class BrandNavBarViewComponent : ViewComponent
    {
        private readonly IBrandApiClient _brandApiClient;
        public BrandNavBarViewComponent(IBrandApiClient brandApiClient)
        {
            _brandApiClient = brandApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _brandApiClient.GetAllBrand();
            return View(items);
        }
    }
}
