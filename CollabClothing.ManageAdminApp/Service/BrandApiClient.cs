using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<bool> Create(BrandCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();
            if (request.Images != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Images.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Images.OpenReadStream().Length);
                }
                ByteArrayContent byteArrayContent = new ByteArrayContent(data);
                requestContent.Add(byteArrayContent, "images", request.Images.FileName);
            }
            requestContent.Add(new StringContent("nameBrand"), request.NameBrand);
            requestContent.Add(new StringContent("info"), request.Info);
            requestContent.Add(new StringContent("slug"), request.Slug);

            var response = await client.PostAsync($"/api/brands/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public Task<bool> Delete(BrandDeleteRequest request)
        {
            return DeleteAsync($"/api/brands/{request.Id}");
        }

        public async Task<bool> Edit(string id, BrandEditRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();
            if (request.Images != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Images.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Images.OpenReadStream().Length);
                }
                ByteArrayContent byteArrayContent = new ByteArrayContent(data);
                requestContent.Add(byteArrayContent, "images", request.Images.FileName);
            }
            requestContent.Add(new StringContent("nameBrand"), request.NameBrand);
            requestContent.Add(new StringContent("info"), request.Info);
            requestContent.Add(new StringContent("slug"), request.Slug);

            var response = await client.PutAsync($"/api/brands/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<BrandViewModel>> GetAllBrand()
        {
            return await GetListAsync<BrandViewModel>($"/api/brands/");
        }

        public Task<PageResult<BrandViewModel>> GetAllPaging(PagingWithKeyword request)
        {
            return GetAsync<PageResult<BrandViewModel>>($"/api/brands/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
        }

        public Task<BrandViewModel> GetById(string id)
        {
            return GetAsync<BrandViewModel>($"/api/brands/{id}");
        }
    }
}
