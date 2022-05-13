using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Service
{
    public class BannerApiClient : BaseApiClient, IBannerApiClient
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IConfiguration _configuration;
        public BannerApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public Task<ResultApi<bool>> Create(BannerCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string id)
        {
            return await DeleteAsync($"/api/banners/{id}");
        }

        public async Task<PageResult<BannerViewModel>> GetAll(PagingWithKeyword request)
        {
            return await GetAsync<PageResult<BannerViewModel>>($"/api/banners/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
        }

        public async Task<ResultApi<BannerViewModel>> GetById(string id)
        {
            return await GetAsync<ResultApi<BannerViewModel>>($"/api/banners/{id}");
        }
    }
}
