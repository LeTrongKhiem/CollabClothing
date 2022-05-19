using CollabClothing.ApiShared;
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
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObject);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _categoryApiClient.Create(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm danh mục mới thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm danh mục thất bại");
            return View(request);
        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var cate = new CategoryDeleteRequest()
            {
                Id = id
            };
            return View(cate);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(CategoryDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _categoryApiClient.Delete(request.Id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Xóa danh mục thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Xóa danh mục thất bại");
            return View(request);
        }
        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var cate = _categoryApiClient.GetById(id);
            if (cate != null)
            {
                var cateResult = cate.Result;
                var editCategory = new CategoryEditRequest()
                {
                    CategoryName = cateResult.CategoryName,
                    IsShowWeb = cateResult.IsShowWeb,
                    Level = cateResult.Level,
                    ParentId = cateResult.ParentId,
                    Slug = cateResult.Slug,
                };
                return View(editCategory);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(string id, [FromForm] CategoryEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _categoryApiClient.Edit(id, request);
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
            var result = await _categoryApiClient.GetById(id);
            if (result == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(result);
        }
        #endregion
    }
}
