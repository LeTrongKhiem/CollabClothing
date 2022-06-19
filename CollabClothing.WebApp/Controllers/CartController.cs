using CollabClothing.ApiShared;
using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.Cart;
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
                    Type = product.Type,

                };
                currentCart.Add(cartVm);
            }

            HttpContext.Session.SetString(SystemConstans.SessionCart, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
        public IActionResult UpdateCart(string id, int quantity)
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
            foreach (var item in currentCart)
            {
                if (item.productId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                }
            }
            HttpContext.Session.SetString(SystemConstans.SessionCart, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
        #region Checkout
        public IActionResult Checkout()
        {
            return View(GetCheckout());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var getCheckout = GetCheckout();
            var orderDetails = new List<OrderDetailsViewModel>();
            foreach (var item in getCheckout.CartItems)
            {
                orderDetails.Add(new OrderDetailsViewModel()
                {
                    ProductId = item.productId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SizeId = item.Size,
                    ColorId = item.Color
                });
            }
            var checkoutRrequest = new CheckoutRequest()
            {
                Address = request.CheckoutRequest.Address,
                Email = request.CheckoutRequest.Email,
                Name = request.CheckoutRequest.Name,
                PhoneNumber = request.CheckoutRequest.PhoneNumber,
                Status = request.CheckoutRequest.Status,
                OrderDetails = orderDetails
            };

            //api

            TempData["result"] = "Đặt hàng thành công quý khách vui lòng xác nhận qua Email hoặc số điện thoại";
            return View(getCheckout);
        }

        private CheckoutViewModel GetCheckout()
        {
            var session = HttpContext.Session.GetString(SystemConstans.SessionCart);
            List<CartItemViewModel> listItemInCart = new List<CartItemViewModel>();
            if (session == null)
            {
                listItemInCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            var checkoutVm = new CheckoutViewModel()
            {
                CheckoutRequest = new CheckoutRequest(),
                CartItems = listItemInCart

            };
            return checkoutVm;
        }
        #endregion
    }
}
