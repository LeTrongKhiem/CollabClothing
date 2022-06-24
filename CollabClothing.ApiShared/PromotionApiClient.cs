using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.Promotions;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public class PromotionApiClient : BaseApiClient, IPromotionApiClient
    {
        public PromotionApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) :
            base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<bool> Create(PromotionCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.PostAsync($"/api/promotions/", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(string id)
        {
            return await DeleteAsync($"/api/promotions/{id}");
        }

        public async Task<bool> Edit(string id, PromotionEditRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"/api/promotions/", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<PromotionViewModel>> GetAll()
        {
            return await GetListAsync<PromotionViewModel>($"/api/promotions/");
        }

        public async Task<PageResult<PromotionViewModel>> GetAllPaging(PromotionPaging request)
        {
            return await GetAsync<PageResult<PromotionViewModel>>($"/api/promotions/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&online={request.Online}&more={request.More}");
        }
    }
}
