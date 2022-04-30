using CollabClothing.ManageAdminApp.Service;
using CollabClothing.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
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
        public ProductController(IProductApiClient productApiClient, IConfiguration configuration)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetManageProductRequestPaging()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _productApiClient.GetAll(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
        #region Create
        [HttpGet]
        public IActionResult Create()
        {
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
    }
}
