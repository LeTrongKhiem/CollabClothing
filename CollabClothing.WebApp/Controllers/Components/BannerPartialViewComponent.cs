using CollabClothing.ApiShared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers.Components
{
    public class BannerPartialViewComponent : ViewComponent
    {
        private readonly IBannerApiClient _bannerApiClient;
        public BannerPartialViewComponent(IBannerApiClient bannerApiClient)
        {
            _bannerApiClient = bannerApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var banner = await _bannerApiClient.GetById(id);
            return View(banner);
        }
    }
}
