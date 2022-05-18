using CollabClothing.ManageAdminApp.Service;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class BrandController : BaseController
    {
        private readonly IBrandApiClient _brandApiClient;
        private readonly IConfiguration _configuration;
        public BrandController(IBrandApiClient brandApiClient, IConfiguration configuration)
        {
            _brandApiClient = brandApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new PagingWithKeyword()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = await _brandApiClient.GetAllPaging(request);
            if (TempData["result"] != null)
            {
                ViewBag.Brands = TempData["result"];
            }
            return View(result);
        }
    }
}
