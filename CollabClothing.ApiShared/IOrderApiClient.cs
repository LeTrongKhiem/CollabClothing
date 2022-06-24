using CollabClothing.ViewModels.Catalog.Cart;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface IOrderApiClient
    {
        Task<bool> CreateOrder(CheckoutRequest request);
        Task<PageResult<CheckoutRequest>> GetAll(PagingCart request);
        Task<CheckoutRequest> GetById(string id);
        Task<bool> DeleteOrder(string id);
        Task<bool> EditOrder(string id);
        Task<bool> AcceptOrder(bool status);
    }
}
