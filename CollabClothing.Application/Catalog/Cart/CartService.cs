using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.ViewModels.Catalog.Cart;
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
        public CartService(CollabClothingDBContext context)
        {
            _context = context;
        }

        public Task<bool> AcceptOrder(string id, bool status)
        {
            throw new NotImplementedException();
        }
        #region Create Checkout
        public async Task<string> Create(CheckoutRequest request)
        {
            Order order;
            Guid gOrder = Guid.NewGuid();
            Guid gOrderDetails = Guid.NewGuid();
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
                order = new Order()
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
            }
            else
            {
                order = new Order()
                {
                    Id = gOrder.ToString(),
                    ShipAddress = request.Address,
                    ShipPhoneNumber = request.PhoneNumber,
                    ShipName = request.Name,
                    ShipEmail = request.Email,
                    Status = false,
                    OrderDetails = orderDetails,
                    OrderDate = DateTime.Now,
                    UserId = null
                };

            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }
        #endregion
        public Task<bool> DeleteCheckout(string id)
        {
            throw new NotImplementedException();
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
                                UserId = x.o.UserId.ToString()
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
                OrderDetails = checkoutOrderDetails
            };
            return checkoutVm;
        }
    }
}
