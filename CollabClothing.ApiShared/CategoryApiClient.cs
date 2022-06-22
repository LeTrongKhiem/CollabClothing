using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor; //use startup singleton<>
        #region Constructor 
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
             : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion
        public async Task<ResultApi<bool>> Create(CategoryCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();
            if (request.Icon != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Icon.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Icon.OpenReadStream().Length);
                }
                ByteArrayContent byteArrayContent = new ByteArrayContent(data);
                requestContent.Add(byteArrayContent, "icon", request.Icon.FileName);
            }
            requestContent.Add(new StringContent(request.CategoryName), "categoryName");
            requestContent.Add(new StringContent(request.IsShowWeb.ToString()), "isShowWeb");
            requestContent.Add(new StringContent(request.ParentId), "parentId");
            requestContent.Add(new StringContent(request.Level.ToString()), "level");
            requestContent.Add(new StringContent(request.Slug), "slug");

            var response = await client.PostAsync($"/api/categories/", requestContent);
            return new ResultApiSuccessed<bool>();
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

        public async Task<bool> Edit(string cateId, CategoryEditRequest request)
        {

            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent();
            if (request.Icon != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Icon.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Icon.OpenReadStream().Length);
                }
                ByteArrayContent byteArrayContent = new ByteArrayContent(data);
                requestContent.Add(byteArrayContent, "icon", request.Icon.FileName);
            }
            requestContent.Add(new StringContent(request.CategoryName), "categoryName");
            requestContent.Add(new StringContent(request.IsShowWeb.ToString()), "isShowWeb");
            requestContent.Add(new StringContent(request.ParentId), "parentId");
            requestContent.Add(new StringContent(request.Level.ToString()), "level");
            requestContent.Add(new StringContent(request.Slug), "slug");

            var response = await client.PutAsync($"/api/categories/cateId?cateId={cateId}", requestContent);
            return response.IsSuccessStatusCode;
        }
        public async Task<ResultApi<bool>> Delete(string cateId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.DeleteAsync($"api/categories/{cateId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        public async Task<CategoryViewModel> GetById(string cateId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/categories/{cateId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<CategoryViewModel>(result);
            }
            return JsonConvert.DeserializeObject<CategoryViewModel>(result);
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            return await GetListAsync<CategoryViewModel>("/api/categories/");
        }

        public async Task<List<CategoryViewModel>> GetParentCate()
        {
            return await GetListAsync<CategoryViewModel>("/api/categories/parent");
        }

        public async Task<List<CategoryViewModel>> GetCateChild(string parentId)
        {
            return await GetListAsync<CategoryViewModel>($"/api/categories/category/{parentId}");
        }
        public async Task<string> GetParentName(string id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/categories/parentname/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
