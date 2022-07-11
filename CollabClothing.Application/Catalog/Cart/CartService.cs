using CollabClothing.Application.Catalog.Products;
using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.ViewModels.Catalog.Cart;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Cart
{
    public class CartService : ICartService
    {
        private readonly CollabClothingDBContext _context;
        private readonly IManageProductService _manageProductService;
        public CartService(CollabClothingDBContext context, IManageProductService manageProductService)
        {
            _context = context;
            _manageProductService = manageProductService;
        }

        private async Task<int> getQuantityWareHouse(string productId, string sizeId, string colorId)
        {
            var query = from w in _context.WareHouses
                        where w.ProductId == productId && w.SizeId == sizeId && w.ColorId == colorId
                        select w;
            var wareHouse = await query.Select(x => x.Quantity).FirstOrDefaultAsync();
            return wareHouse;
        }

        public async Task<bool> AcceptOrder(string id, bool status)
        {
            var order = await _context.Orders.FindAsync(id);
            order.Status = status;
            //var orderDetails = await _context.OrderDetails.Select(x => new OrderDetail()
            //{
            //    Id = x.Id,
            //    Quantity = x.Quantity,
            //    ProductId = x.ProductId,
            //    SizeId = x.SizeId,
            //    ColorId = x.ColorId
            //}).Where(x => x.OrderId == id).ToListAsync();
            var query = (from od in _context.OrderDetails where od.OrderId == id select od);
            var orderDetails = await query.Select(x => new OrderDetail()
            {
                Id = x.Id,
                Quantity = x.Quantity,
                ProductId = x.ProductId,
                SizeId = x.SizeId,
                ColorId = x.ColorId
            }).ToListAsync();
            foreach (var orderDetail in orderDetails)
            {
                var quantityRemain = await getQuantityWareHouse(orderDetail.ProductId, orderDetail.SizeId, orderDetail.ColorId);
                var quantityBuy = quantityRemain - orderDetail.Quantity;
                var wareHouseRequest = new WareHouseRequest()
                {
                    SizeId = orderDetail.SizeId,
                    ColorId = orderDetail.ColorId,
                    Quantity = quantityBuy
                };
                var wareHouse = await _context.WareHouses.FirstOrDefaultAsync(x => x.ProductId == orderDetail.ProductId && x.SizeId == orderDetail.SizeId && x.ColorId == orderDetail.ColorId);
                //var updateQuantityRemain = await _manageProductService.UpdateQuantityRemainProduct(orderDetail.ProductId, wareHouseRequest);
                wareHouse.Quantity = quantityBuy;
                await _context.SaveChangesAsync();
            }
            return await _context.SaveChangesAsync() > 0;
        }
        #region Create Checkout
        public async Task<string> Create(CheckoutRequest request)
        {
            //Order order;
            Guid gOrder = Guid.NewGuid();

            var orderDetails = new List<OrderDetail>();
            if (request.OrderDetails.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (var item in request.OrderDetails)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    Guid gOrderDetails = Guid.NewGuid();
                    orderDetails.Add(new OrderDetail()
                    {
                        Id = gOrderDetails.ToString(),
                        OrderId = gOrder.ToString(),
                        ProductId = item.ProductId,
                        Price = product.PriceCurrent * item.Quantity,
                        Quantity = item.Quantity,
                        SizeId = item.SizeId ?? null,
                        ColorId = item.ColorId ?? null
                    });
                }
            }
            if (request.UserId != null)
            {
                Guid userId = new Guid(request.UserId);
                var user = await _context.AppUsers.FindAsync(userId);
                Order orderUserId = new Order()
                {
                    Id = gOrder.ToString(),
                    ShipAddress = request.Address,
                    ShipPhoneNumber = request.PhoneNumber,
                    ShipName = request.Name,
                    ShipEmail = request.Email,
                    Status = false,
                    AppUser = user,
                    OrderDetails = orderDetails,
                    OrderDate = DateTime.Now,
                    UserId = userId
                };
                _context.Orders.Add(orderUserId);
            }
            else
            {
                Guid gUserId = Guid.Parse("D74C0DB1-12FB-42A1-AA67-09C660D508F6");
                Order orderNoUserId = new Order()
                {
                    Id = gOrder.ToString(),
                    ShipAddress = request.Address,
                    ShipPhoneNumber = request.PhoneNumber,
                    ShipName = request.Name,
                    ShipEmail = request.Email,
                    Status = false,
                    OrderDetails = orderDetails,
                    OrderDate = DateTime.Now,
                    UserId = gUserId
                };
                _context.Orders.Add(orderNoUserId);
            }
            await _context.SaveChangesAsync();
            return gOrder.ToString();
        }
        #endregion
        public async Task<int> DeleteCheckout(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == id).ToListAsync();
            foreach (var item in orderDetails)
            {
                _context.OrderDetails.Remove(item);
            }
            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync();
        }

        public Task<bool> EditCheckout(string id, CheckoutRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<CheckoutRequest>> GetAllCheckout(PagingCart request)
        {
            var query = from o in _context.Orders
                            //join od in _context.OrderDetails
                            //on o.Id equals od.OrderId into ood
                            //from od in ood.DefaultIfEmpty()
                        select new { o };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.o.ShipName.Contains(request.Keyword) || x.o.ShipAddress.Contains(request.Keyword)
                || x.o.ShipPhoneNumber.Contains(request.Keyword) || x.o.ShipEmail.Contains(request.Keyword));
            }
            if (request.Status == true)
            {
                query = query.Where(x => x.o.Status == true);
            }
            if (request.Status == false)
            {
                query = query.Where(x => x.o.Status == false);
            }
            int countPage = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .Select(x => new CheckoutRequest()
                            {
                                OrderId = x.o.Id,
                                Address = x.o.ShipAddress,
                                Email = x.o.ShipEmail,
                                Name = x.o.ShipName,
                                PhoneNumber = x.o.ShipPhoneNumber,
                                Status = x.o.Status,
                                UserId = x.o.UserId.ToString(),
                                OrderDate = x.o.OrderDate
                            }).ToListAsync();
            var pageResult = new PageResult<CheckoutRequest>()
            {
                Items = data,
                TotalRecord = countPage,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return pageResult;
        }

        public async Task<CheckoutRequest> GetCheckoutById(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            var orderDetails = from od in _context.OrderDetails
                               join o in _context.Orders
                               on od.OrderId equals o.Id
                               where o.Id == id
                               select od;
            List<OrderDetailsViewModel> checkoutOrderDetails = await orderDetails.Select(x => new OrderDetailsViewModel()
            {
                ProductId = x.ProductId,
                Price = x.Price,
                SizeId = x.SizeId ?? "null",
                Quantity = x.Quantity,
                ColorId = x.ColorId ?? "null"
            }).ToListAsync();
            var checkoutVm = new CheckoutRequest()
            {
                Address = order.ShipAddress,
                Email = order.ShipEmail,
                PhoneNumber = order.ShipPhoneNumber,
                Name = order.ShipName,
                Status = order.Status,
                OrderDetails = checkoutOrderDetails,
                OrderId = order.Id
            };
            return checkoutVm;
        }
    }
}
