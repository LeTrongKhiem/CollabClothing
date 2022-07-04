using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.Color;
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
    public class ColorApiClient : BaseApiClient, IColorApiClient
    {
        public ColorApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<ColorViewModel>> GetAll()
        {
            return await GetListAsync<ColorViewModel>("/api/colors/");
        }

        public async Task<string> GetNameColor(string id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/colors/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
