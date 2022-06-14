using CollabClothing.ApiShared;
using CollabClothing.Utilities.Constants;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        public CartController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetCart()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var session = HttpContext.Session.GetString(SystemConstans.SessionCart);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();

            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            return Ok(currentCart);
        }
        public async Task<IActionResult> AddToCart(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var product = await _productApiClient.GetById(id);
            var session = HttpContext.Session.GetString(SystemConstans.SessionCart);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }

            int quantity = 1;
            if (currentCart.Any(x => x.productId == id))
            {
                //quantity = currentCart.FirstOrDefault(x => x.productId == id).Quantity + 1;
                foreach (var item in currentCart)
                {
                    if (item.productId == id)
                    {
                        item.Quantity = (currentCart.First(x => x.productId == id).Quantity + 1);
                    }
                }
            }
            else
            {
                var cartVm = new CartItemViewModel()
                {
                    productId = id,
                    Quantity = quantity,
                    Name = product.ProductName,
                    Price = product.PriceCurrent,
                    Image = product.ThumbnailImage,
                    BrandName = await _productApiClient.GetBrandNameByProductId(product.Id),
                    Type = product.Type

                };
                currentCart.Add(cartVm);
            }

            HttpContext.Session.SetString(SystemConstans.SessionCart, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
    }
}
