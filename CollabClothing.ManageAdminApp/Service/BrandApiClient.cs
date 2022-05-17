using CollabClothing.ViewModels.Catalog.Brands;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Service
{
    public class BrandApiClient : BaseApiClient, IBrandApiClient
    {
        public BrandApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<BrandViewModel>> GetAllBrand()
        {
            return await GetListAsync<BrandViewModel>($"/api/brands/");
        }
    }
}
