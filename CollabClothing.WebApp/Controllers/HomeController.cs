using CollabClothing.ApiShared;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IBannerApiClient bannerApiClient, IProductApiClient productApiClient
            , IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _bannerApiClient = bannerApiClient;
            _productApiClient = productApiClient;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("/trang-chu")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {

            //var viewModel = new HomeViewModel()
            //{
            //    ListBanner = await _bannerApiClient.GetAll(),
            //    ListProductFeatured = await _productApiClient.GetFeaturedProducts(12)
            //};
            int rows = 12;
            var session = _httpContextAccessor.HttpContext.Session.GetInt32("data");
            session = rows;
            var data = new HomeViewModel()
            {
                ListProductFeatured = await _productApiClient.GetFeaturedProducts(rows),
                ListProductFeaturedByMen = await _productApiClient.GetFeaturedProductsByCategory("82AAE383-3EDF-45F0-AA5B-9B4A514EA7A9", 4),
                ListProductFeaturedByWoman = await _productApiClient.GetFeaturedProductsByCategory("d93e95a9-38bd-4011-a457-29a763189a81", 8),
                ListProductFeaturedByChild = await _productApiClient.GetFeaturedProductsByCategory("29919101-ff6c-466f-907b-66a2f669be7f", 8),
            };
            return View(data);
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
