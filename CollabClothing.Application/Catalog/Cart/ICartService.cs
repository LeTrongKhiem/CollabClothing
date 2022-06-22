using CollabClothing.ViewModels.Catalog.Cart;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Cart
{
    public interface ICartService
    {
        Task<PageResult<CheckoutRequest>> GetAllCheckout(PagingCart request);
        Task<string> Create(CheckoutRequest request);
        Task<bool> AcceptOrder(string id, bool status);
        Task<int> DeleteCheckout(string id);
        Task<bool> EditCheckout(string id, CheckoutRequest request);
        Task<CheckoutRequest> GetCheckoutById(string id);
    }
}
