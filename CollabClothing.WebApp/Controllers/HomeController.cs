using CollabClothing.ApiShared;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBannerApiClient _bannerApiClient;
        private readonly IProductApiClient _productApiClient;

        public HomeController(ILogger<HomeController> logger, IBannerApiClient bannerApiClient, IProductApiClient productApiClient)
        {
            _logger = logger;
            _bannerApiClient = bannerApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {

            var viewModel = new HomeViewModel()
            {
                ListBanner = await _bannerApiClient.GetAll(),
                ListProductFeatured = await _productApiClient.GetFeaturedProducts(12)
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
