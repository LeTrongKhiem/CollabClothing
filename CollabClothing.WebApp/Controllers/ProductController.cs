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

        public async Task<IActionResult> Detail(string id)
        {
            var product = await _productApiClient.GetById(id);
            var productDetails = new ProductDetailsViewModel()
            {
                Products = product,
                ProductsImages = await _productApiClient.GetAllImages(id),
                ProductRelated = await _productApiClient.GetFeaturedProducts(8)
            };
            return View(productDetails);
        }
    }
}
