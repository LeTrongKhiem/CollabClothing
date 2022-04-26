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
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Service
{
    public class RoleApiClient : BaseApiClient, IRoleApiClient
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly IConfiguration _configuration;
        #region Constructor
        public RoleApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            //_httpContextAccessor = httpContextAccessor;
            //_httpClientFactory = httpClientFactory;
            //_configuration = configuration;
        }
        #endregion

        public async Task<ResultApi<bool>> Create(RoleCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync($"/api/roles", httpContent);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        public async Task<ResultApi<bool>> Delete(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.DeleteAsync($"/api/roles/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        public async Task<ResultApi<bool>> Edit(Guid id, RoleEditRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.PutAsync($"/api/roles/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            //var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            //var client = _httpClientFactory.CreateClient();
            //client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            //var response = await client.GetAsync($"api/roles");
            //var result = await response.Content.ReadAsStringAsync();
            //if (response.IsSuccessStatusCode)
            //{
            //    List<RoleViewModel> myDeserializedRoleObject = (List<RoleViewModel>)JsonConvert.DeserializeObject(result, typeof(List<RoleViewModel>));
            //    //return new ResultApiSuccessed<List<RoleViewModel>>(myDeserializedRoleObject);
            //    return myDeserializedRoleObject;
            //    //return JsonConvert.DeserializeObject<ResultApiSuccessed<List<RoleViewModel>>>(result);
            //}
            ////return JsonConvert.DeserializeObject<ResultApiError<List<RoleViewModel>>>(result);
            //throw new Exception(result);
            return await GetListAsync<RoleViewModel>("/api/roles");
        }

        public async Task<ResultApi<RoleViewModel>> GetById(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/roles/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<RoleViewModel>>(body);
            }
            return JsonConvert.DeserializeObject<ResultApiError<RoleViewModel>>(body);
        }
    }
}
