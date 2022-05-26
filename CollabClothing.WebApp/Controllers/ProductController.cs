using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IActionResult> Category(string id, int pageIndex = 1)
        {
            var products = await _productApiClient.GetAll(new GetManageProductRequestPaging()
            {
                CategoryId = id,
                PageIndex = pageIndex,
                PageSize = 10
            });
            return View(new ProductCategoryViewModel()
            {
                Category = await _categoryApiClient.GetById(id),
                Products = products
            });
        }

        public IActionResult Detail(string id)
        {
            return View();
        }
    }
}
