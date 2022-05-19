using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class BannerController : BaseController
    {
        private readonly IBannerApiClient _bannerApiClient;
        private readonly IConfiguration _configuration;
        public BannerController(IBannerApiClient bannerApiClient, IConfiguration configuration)
        {
            _bannerApiClient = bannerApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new PagingWithKeyword()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _bannerApiClient.GetAll(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var banner = new BannerDeleteRequest()
            {
                Id = id
            };
            return View(banner);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(BannerDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _bannerApiClient.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Xóa Banner thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Xóa Banner thất bại");
            return View(request);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(BannerCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _bannerApiClient.Create(request);
            if (result)
            {
                TempData["result"] = "Tạo Banner thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Tạo Banner thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var banner = await _bannerApiClient.GetById(id);
            if (banner != null)
            {
                var resultBanner = banner;
                var editBanner = new BannerEditRequest()
                {
                    Alt = resultBanner.Alt,
                    NameBanner = resultBanner.NameBanner,
                    Text = resultBanner.Text,
                    TypeBannerId = resultBanner.TypeBannerId,
                    Path = resultBanner.Images
                };
                return View(editBanner);
            }
            return RedirectToAction("Error", "Index");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(string id, [FromForm] BannerEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _bannerApiClient.Edit(id, request);
            if (result)
            {
                TempData["result"] = "Sửa Banner thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Sửa Banner thất bại");
            return View(request);
        }
    }
}
