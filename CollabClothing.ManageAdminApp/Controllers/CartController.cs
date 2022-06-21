using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class CartController : BaseController
    {
        private readonly IOrderApiClient _orderApiClient;
        private readonly IConfiguration _configuration;

        public CartController(IOrderApiClient orderApiClient, IConfiguration configuration)
        {
            _orderApiClient = orderApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, bool status = false, int pageIndex = 1, int pageSize = 10)
        {
            var request = new PagingCart()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Status = status,

            };
            ViewBag.Keyword = keyword;
            var result = await _orderApiClient.GetAll(request);
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetDetailOrder(string id)
        {
            if (!ModelState.IsValid)
            {
                return View(id);
            }
            var result = await _orderApiClient.GetById(id);
            if (result != null)
            {
                return View(result);
            }
            ModelState.AddModelError("", "Not found !!!");
            return RedirectToAction("Error", "Home");
        }
    }
}
