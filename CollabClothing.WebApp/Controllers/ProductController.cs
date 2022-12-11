using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
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
        public async Task<IActionResult> Category(string id, string slug, string keyword, string brandId, string priceOrder, int pageIndex = 1)
        {
            if (slug == null)
            {
                slug = "all";
            }
            var products = await _productApiClient.GetAll(new GetManageProductRequestPaging()
            {
                CategoryId = id,
                PageIndex = pageIndex,
                PageSize = 5,
                Slug = slug,
                Keyword = keyword,
                BrandId = brandId,
                Price = priceOrder
            });

            ViewBag.Keyword = keyword;
            ViewBag.Slug = slug;
            ViewBag.priceOrder = priceOrder;
            var viewModel = new ProductCategoryViewModel()
            {
                //Category = !id.Equals("all") ? await _categoryApiClient.GetById(id) : null,
                Category = !slug.Equals("all") ? await _categoryApiClient.GetBySlug(slug) : null,
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

        public async Task<IActionResult> GetHistoryOrder(int pageIndex = 1, int pageSize = 10, string keyword = null)
        {
            var request = new GetManageProductRequestPaging()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };
            var userIdGuid = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId)
                ? userId
                : throw new Exception("Not authenticated");
            var result = await _productApiClient.GetHistoryOrder(request, userId);
            return View(result);
        }

        public async Task<IActionResult> CancelOrder(string orderId)
        {
            var result = await _productApiClient.CancelOrder(orderId);

            return RedirectToAction("GetHistoryOrder");
        }
    }
}
