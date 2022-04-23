using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Roles;
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
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        #region Constructor
        public RoleApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        #endregion
        public async Task<ResultApi<List<RoleViewModel>>> GetAll()
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"api/roles");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<RoleViewModel> myDeserializedRoleObject = (List<RoleViewModel>)JsonConvert.DeserializeObject(result, typeof(List<RoleViewModel>));
                return new ResultApiSuccessed<List<RoleViewModel>>(myDeserializedRoleObject);
            }
            return JsonConvert.DeserializeObject<ResultApiError<List<RoleViewModel>>>(result);
        }
    }
}
