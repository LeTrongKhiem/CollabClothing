using CollabClothing.ManageAdminApp.Service;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IBrandApiClient _brandApiClient;
        public ProductController(IProductApiClient productApiClient, IConfiguration configuration, ICategoryApiClient categoryApiClient, IBrandApiClient brandApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
            _brandApiClient = brandApiClient;
        }
        public async Task<IActionResult> Index(string keyword, string categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetManageProductRequestPaging()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            var data = await _productApiClient.GetAll(request);
            ViewBag.Keyword = keyword;
            var categories = await _categoryApiClient.GetAll();
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.CategoryId,
                Selected = categoryId == x.CategoryId && categoryId != null
            });
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryApiClient.GetAll();
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.CategoryId,
                Selected = x.CategoryId != null
            });
            var brand = await _brandApiClient.GetAllBrand();
            ViewBag.Brands = brand.Select(x => new SelectListItem()
            {
                Text = x.NameBrand,
                Value = x.Id,
                Selected = x.Id != null
            });
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.Create(request);
            if (result)
            {
                TempData["result"] = "Thêm sản phẩm mới thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm sản phẩm mới thất bại");
            return View(request);
        }
        #endregion
        #region Delete
        [HttpGet]
        public IActionResult Delete(string Id)
        {
            var product = new ProductDeleteRequest()
            {
                Id = Id
            };
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Xóa thành công sản phẩm";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Xóa thất bại sản phẩm");
            return View(request);
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var product = _productApiClient.GetById(id);
            if (product != null)
            {
                var productResult = product.Result;
                var editProduct = new ProductEditRequest()
                {
                    ProductName = productResult.ProductName,
                    Details = productResult.Details,
                    Description = productResult.Description,
                    BrandId = productResult.BrandId,
                    Slug = productResult.Slug,
                    ImagePath = productResult.ThumbnailImage
                };
                return View(editProduct);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(string id, [FromForm] ProductEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.Edit(id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }
        #endregion
        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var product = await _productApiClient.GetById(id);
            if (product == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(product);
        }
        #endregion
        #region CategoryAssign
        [HttpGet]
        public async Task<IActionResult> CategoriesAssign(string id)
        {
            var categoriesAssignRequest = await GetCategoriesAssignRequest(id);
            return View(categoriesAssignRequest);
        }

        private async Task<CategoryAssignRequest> GetCategoriesAssignRequest(string id)
        {
            var product = await _productApiClient.GetById(id);
            var categories = await _categoryApiClient.GetAll();
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var category in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = category.CategoryId,
                    Name = category.CategoryName,
                    Selected = product.Categories.Contains(category.CategoryName)
                });
            }
            return categoryAssignRequest;
        }
        [HttpPost]
        public async Task<IActionResult> CategoriesAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.CategoryAssign(request.Id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật danh mục sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "");
            var rolesAssignRequest = GetCategoriesAssignRequest(request.Id);
            return View(rolesAssignRequest);
        }
        #endregion
    }
}
