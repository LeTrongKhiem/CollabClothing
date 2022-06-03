using CollabClothing.ApiShared;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers.Components
{
    public class BannerViewComponent : ViewComponent
    {
        private readonly IBannerApiClient _bannerApiClient;
        public BannerViewComponent(IBannerApiClient bannerApiClient)
        {
            _bannerApiClient = bannerApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banner = new HomeViewModel()
            {
                ListBanner = await _bannerApiClient.GetAll()
            };
            return View(banner);
        }
    }
}
