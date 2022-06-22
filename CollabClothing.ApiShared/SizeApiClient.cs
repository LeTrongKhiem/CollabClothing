using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.Sizes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<string> GetNameSize(string id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/sizes/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
