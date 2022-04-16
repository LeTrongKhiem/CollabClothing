using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Service
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor; //use startup singleton<>
        #region Constructor 
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion
        public Task<ResultApi<string>> Create()
        {
            throw new NotImplementedException();
        }

        public async Task<ResultApi<PageResult<CategoryViewModel>>> GetAllPaging(GetCategoryRequestPaging request)//request khong can serialize sang json
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/categories/paging?pageIndex={request.PageIndex}&" +
                $"pageSize={request.PageSize}&keyword={request.Keyword}");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResultApiSuccessed<PageResult<CategoryViewModel>>>(body);
            return result;
        }
    }
}
