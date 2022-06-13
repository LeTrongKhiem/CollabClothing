using CollabClothing.ViewModels.Catalog.Sizes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public class SizeApiClient : BaseApiClient, ISizeApiClient
    {
        public SizeApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<SizeViewModel>> GetAll()
        {
            return await GetListAsync<SizeViewModel>("/api/sizes/");
        }
    }
}
