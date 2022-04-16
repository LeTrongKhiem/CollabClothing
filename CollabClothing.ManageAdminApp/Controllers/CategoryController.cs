using CollabClothing.ManageAdminApp.Service;
using CollabClothing.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IConfiguration _configuration;
        public CategoryController(ICategoryApiClient categoryApiClient, IConfiguration configuration)
        {
            _categoryApiClient = categoryApiClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 8)
        {
            var request = new GetCategoryRequestPaging()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _categoryApiClient.GetAllPaging(request);

            return View(data.ResultObject);
        }
    }
}
