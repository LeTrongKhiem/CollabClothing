using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers
{
    public class LoadMoreController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        public LoadMoreController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }
        //[HttpGet]
        public async Task<IActionResult> Index(string cateId)
        {
            String amount = HttpContext.Request.Query["exits"];
            int amountI = int.Parse(amount);
            double page = amountI / 2;
            int pageI = (int)Math.Floor(page) + 1;

            var product = await _productApiClient.GetAll(new GetManageProductRequestPaging()
            {
                PageIndex = pageI,
                PageSize = 2,
                CategoryId = cateId,
            });
            if (amountI >= product.TotalRecord)
            {
                return null;
            }
            return View(product);
        }
        public async Task<IActionResult> Brand(string brandId)
        {
            String amount = HttpContext.Request.Query["exits"];
            int amountI = int.Parse(amount);
            double page = amountI / 5;
            int pageI = (int)Math.Floor(page) + 1;
            var product = await _productApiClient.GetAll(new GetManageProductRequestPaging()
            {
                PageIndex = pageI,
                PageSize = 5,
                BrandId = brandId,
            });
            if (amountI >= product.TotalRecord)
            {
                return null;
            }
            return View(product);
        }
    }
}
