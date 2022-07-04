using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Catalog.Brands;
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
        [HttpGet]
        public IActionResult Create()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            return View();
        }
        [HttpPost]
        [Consumes("mutilpart/form-data")]
        public async Task<IActionResult> Create([FromForm] BrandCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _brandApiClient.Create(request);
            if (result)
            {
                TempData["result"] = "Thêm thương hiệu mới thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm thương hiệu mới thất bại");
            return View(request);
        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var brand = new BrandDeleteRequest()
            {
                Id = id
            };
            return View(brand);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(BrandDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _brandApiClient.Delete(request);
            if (result)
            {
                TempData["result"] = "Xóa thương hiệu thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Xóa thương hiệu thất bại");
            return View(request);
        }
        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var brand = _brandApiClient.GetById(id);
            if (brand != null)
            {
                var brandResult = brand.Result;
                var editBrand = new BrandEditRequest()
                {
                    Info = brandResult.Info,
                    NameBrand = brandResult.NameBrand,
                    Slug = brandResult.Slug,
                    ImagesPath = brandResult.Images
                };
                return View(editBrand);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(string id, [FromForm] BrandEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _brandApiClient.Edit(id, request);
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
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _brandApiClient.GetById(id);
            if (result == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(result);
        }
        #endregion
    }
}
