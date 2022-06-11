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
        private readonly IBrandApiClient _brandApiClient;
        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IBrandApiClient brandApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _brandApiClient = brandApiClient;
        }
        public async Task<IActionResult> Category(string id, string slug, string keyword, string brandId, int pageIndex = 1)
        {
            if (id == null)
            {
                id = "all";
            }
            var products = await _productApiClient.GetAll(new GetManageProductRequestPaging()
            {
                CategoryId = id,
                PageIndex = pageIndex,
                PageSize = 5,
                Slug = slug,
                Keyword = keyword,
                BrandId = brandId
            });
            ViewBag.Keyword = keyword;
            var viewModel = new ProductCategoryViewModel()
            {
                Category = !id.Equals("all") ? await _categoryApiClient.GetById(id) : null,
                Products = products,
                Brand = brandId != null ? await _brandApiClient.GetById(brandId) : null
            };
            return View(viewModel);
        }
        public async Task<IActionResult> Detail(string id)
        {
            var product = await _productApiClient.GetById(id);
            var productDetails = new ProductDetailsViewModel()
            {
                Products = product,
                ProductsImages = await _productApiClient.GetAllImages(id),
                ProductRelated = await _productApiClient.GetFeaturedProducts(8),
            };
            return View(productDetails);
        }
    }
}
