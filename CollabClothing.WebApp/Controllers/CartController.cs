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
using System.Security.Claims;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers
{
    public class CartController : Controller
    {
        #region Constructor
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderApiClient;
        private readonly ISizeApiClient _sizeApiClient;
        private readonly IColorApiClient _colorApiClient;
        private readonly IUserApiClient _userApiClient;
        public CartController(IProductApiClient productApiClient, IOrderApiClient orderApiClient, ISizeApiClient sizeApiClient, IColorApiClient colorApiClient, IUserApiClient userApiClient)
        {
            _productApiClient = productApiClient;
            _orderApiClient = orderApiClient;
            _sizeApiClient = sizeApiClient;
            _colorApiClient = colorApiClient;
            _userApiClient = userApiClient;
        }
        #endregion
        #region Index
        public IActionResult Index()
        {
            if (TempData["result"] != null)
            {
                ViewBag.Notify = TempData["result"];
            }
            return View();
        }
        #endregion
        #region Get Cart
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
        #endregion
        #region Add to cart
        public async Task<IActionResult> AddToCart(string id, string sizeId, string colorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var product = await _productApiClient.GetById(id);

            var quantityRemain = await _productApiClient.GetQuantityRemain(id);

            var session = HttpContext.Session.GetString(SystemConstans.SessionCart);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }

            int quantity = 1;
            if (currentCart.Any(x => (x.productId == id && x.Size == sizeId && x.Color == colorId)))
            {
                foreach (var item in currentCart)
                {
                    if (item.productId == id && item.Color == colorId && item.Size == sizeId)
                    {
                        item.Quantity = (currentCart.First(x => x.productId == id && x.Size == sizeId && x.Color == colorId).Quantity + 1);
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
                    Size = sizeId,
                    SizeName = _sizeApiClient.GetNameSize(sizeId).Result.ToString(),
                    Color = colorId,
                    ColorName = _colorApiClient.GetNameColor(colorId).Result.ToString()
                };
                currentCart.Add(cartVm);
            }

            HttpContext.Session.SetString(SystemConstans.SessionCart, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
        #endregion
        #region Update cart ( update quantity, remove product in cart ) 
        public IActionResult UpdateCart(string id, int quantity, string sizeId, string colorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var session = HttpContext.Session.GetString(SystemConstans.SessionCart);
            Task<int> quantityRemainTask;
            if (sizeId == null && colorId == null)
            {
                quantityRemainTask = _productApiClient.GetQuantityRemain(id);
            }
            else
            {
                quantityRemainTask = _productApiClient.GetQuantityRemain(id, sizeId, colorId);
            }
            int quantityRemain = quantityRemainTask.Result;
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            foreach (var item in currentCart)
            {
                if (item.productId == id && item.Color == colorId && item.Size == sizeId)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    if (quantity > quantityRemain)
                    {
                        TempData["result"] = "Sản phẩm trong kho đã hết. Quý khách vui lòng chọn sản phẩm khác";
                    }
                    else if (quantity <= quantityRemain)
                    {
                        item.Quantity = quantity;
                    }
                }
            }
            HttpContext.Session.SetString(SystemConstans.SessionCart, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
        #endregion
        #region Checkout
        public IActionResult Checkout()
        {
            var quantityRemain = _productApiClient.GetQuantityRemain("352e1af2-4a0e-4d58-a0ef-cb07f2a7e74d", "32BD8E51-8D32-4AC5-9E11-6CE8EFE4A88C", "CA95EEB7-94FD-4CAD-9384-A4A2BA78CF96");
            int quantity = quantityRemain.Result;
            return View(GetCheckout());
        }

        [HttpPost]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var getCheckout = GetCheckout();
            var userLogined = HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var userName = HttpContext.User.Identity.Name;
            string userId;
            if (userName != null)
            {
                var user = await _userApiClient.GetByUsername(userName);
                userId = user.ResultObject.Id.ToString();
            }
            else
            {
                //userId = SystemConstans.UsersSetting.UserGuestId;
                userId = null;
            }
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
            var checkoutRequest = new CheckoutRequest()
            {
                Address = request.CheckoutRequest.Address,
                Email = request.CheckoutRequest.Email,
                Name = request.CheckoutRequest.Name,
                PhoneNumber = request.CheckoutRequest.PhoneNumber,
                Status = request.CheckoutRequest.Status,
                UserId = userId,
                OrderDetails = orderDetails
            };

            //api
            var result = await _orderApiClient.CreateOrder(checkoutRequest);
            if (result)
            {
                TempData["result"] = "Đặt hàng thành công quý khách vui lòng xác nhận qua Email hoặc số điện thoại";
                HttpContext.Session.Remove(SystemConstans.SessionCart);
                return RedirectToAction("CheckoutSuccess");
            }
            ModelState.AddModelError("", "Đặt hàng thất bại. Vui lòng thử lại");
            return View(getCheckout);
        }

        private CheckoutViewModel GetCheckout()
        {
            var session = HttpContext.Session.GetString(SystemConstans.SessionCart);
            List<CartItemViewModel> listItemInCart = new List<CartItemViewModel>();
            if (session != null)
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
        #region Done Checkout
        public IActionResult CheckoutSuccess()
        {
            return View();
        }
        #endregion
    }
}
