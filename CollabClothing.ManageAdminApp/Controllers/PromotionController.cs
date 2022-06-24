using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Catalog.Promotions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class PromotionController : BaseController
    {
        #region Constructor
        private readonly IPromotionApiClient _promotionApiClient;
        public PromotionController(IPromotionApiClient promotionApiClient)
        {
            _promotionApiClient = promotionApiClient;
        }
        #endregion
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10, bool online = true, bool more = true)
        {
            var paging = new PromotionPaging()
            {
                Keyword = keyword,
                More = more,
                Online = online,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = await _promotionApiClient.GetAllPaging(paging);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PromotionCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _promotionApiClient.Create(request);
            if (result)
            {
                TempData["result"] = "Thêm sản phẩm mới thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm sản phẩm mới thất bại");
            return View(request);
        }
        #region Delete
        [HttpGet]
        public IActionResult Delete(string Id)
        {
            var promotion = new PromotionDeleteRequest()
            {
                Id = Id
            };
            return View(promotion);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(PromotionDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _promotionApiClient.Delete(request.Id);
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
