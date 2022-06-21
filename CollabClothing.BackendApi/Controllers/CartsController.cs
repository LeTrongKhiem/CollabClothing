using CollabClothing.Application.Catalog.Cart;
using CollabClothing.ViewModels.Catalog.Cart;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }
        #region Create Checkout
        #endregion

        #region GetAllOrder
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllOrder([FromQuery] PagingCart request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartService.GetAllCheckout(request);
            if (result == null)
            {
                return BadRequest("Not found");
            }
            return Ok(result);
        }
        #endregion
        #region GetOrderDetails
        [HttpGet("orderdetails/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderDetails(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartService.GetCheckoutById(id);
            if (result == null)
            {
                return BadRequest("Not found");
            }
            return Ok(result);
        }
        #endregion
        #region Create Checkout
        [HttpPost]
        [AllowAnonymous]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateOrder([FromBody] CheckoutRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderId = await _cartService.Create(request);
            if (orderId == null || orderId.Equals(""))
            {
                return BadRequest("Don't Create");
            }
            var orderDetail = await _cartService.GetCheckoutById(orderId);

            return CreatedAtAction(nameof(orderId), new { id = orderId }, orderDetail);
        }
        #endregion

        #region Delete Order
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartService.DeleteCheckout(id);
            if (result == 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        #endregion
    }
}
