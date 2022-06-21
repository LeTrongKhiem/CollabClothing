using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.Cart;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public class OrderApiClient : BaseApiClient, IOrderApiClient
    {
        public OrderApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public Task<bool> AcceptOrder(bool status)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateOrder(CheckoutRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var orderDetailsJson = JsonConvert.SerializeObject(request.OrderDetails);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            var response = await client.PostAsync($"/api/carts", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteOrder(string id)
        {
            return await DeleteAsync($"/api/Carts/{id}");
        }

        public Task<bool> EditOrder(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<CheckoutRequest>> GetAll(PagingCart request)
        {
            return await GetAsync<PageResult<CheckoutRequest>>($"/api/carts?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&status={request.Status}");
        }

        public async Task<CheckoutRequest> GetById(string id)
        {
            return await GetAsync<CheckoutRequest>($"/api/carts/orderdetails/{id}");
        }
    }
}
