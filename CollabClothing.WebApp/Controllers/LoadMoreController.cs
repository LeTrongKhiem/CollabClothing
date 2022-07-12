using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Common;
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
        public async Task<IActionResult> Index(string slug, string price, string keyword)
        {
            String amount = HttpContext.Request.Query["exits"];
            //String keyword = HttpContext.Request.Query["keyword"];
            int amountI = int.Parse(amount);
            double page = amountI / 5;
            int pageI = (int)Math.Floor(page) + 1;
            //string priceOrder = HttpContext.Request.Query["price"].ToString();
            PageResult<ProductViewModel> product = null;
            if (price == null)
            {
                product = await _productApiClient.GetAll(new GetManageProductRequestPaging()
                {
                    PageIndex = pageI,
                    PageSize = 5,
                    //CategoryId = cateId,
                    Slug = slug,
                    Keyword = keyword
                });
            }
            else
            {
                product = await _productApiClient.GetAll(new GetManageProductRequestPaging()
                {
                    PageIndex = pageI,
                    PageSize = 5,
                    //CategoryId = cateId,
                    Slug = slug,
                    Keyword = keyword,
                    Price = price
                });
            }

            if (amountI >= product.TotalRecord)
            {
                return null;
            }
            return View(product);
        }
        public async Task<IActionResult> Brand(string brandId, string price, string keyword)
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
                Price = price,
                Keyword = keyword
            });
            if (amountI >= product.TotalRecord)
            {
                return null;
            }
            return View(product);
        }
    }
}
