using CollabClothing.ApiShared;
using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
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

        public async Task<bool> Create(BannerCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(session);

            var httpContent = new MultipartFormDataContent();
            if (request.Images != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Images.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Images.OpenReadStream().Length);
                }
                ByteArrayContent byteArrayContent = new ByteArrayContent(data);
                httpContent.Add(byteArrayContent, "images", request.Images.FileName);
            }
            httpContent.Add(new StringContent(request.NameBanner), "nameBanner");
            httpContent.Add(new StringContent(request.Alt), "alt");
            httpContent.Add(new StringContent(request.TypeBannerId), "typeBannerId");
            httpContent.Add(new StringContent(request.Text), "text");

            var response = await client.PostAsync($"/api/banners/", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(string id)
        {
            return await DeleteAsync($"/api/banners/{id}");
        }

        public async Task<bool> Edit(string id, BannerEditRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(session);

            var httpContent = new MultipartFormDataContent();
            if (request.Images != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Images.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Images.OpenReadStream().Length);
                }
                ByteArrayContent byteArrayContent = new ByteArrayContent(data);
                httpContent.Add(byteArrayContent, "images", request.Images.FileName);
            }
            httpContent.Add(new StringContent(request.NameBanner), "nameBanner");
            httpContent.Add(new StringContent(request.Alt), "alt");
            httpContent.Add(new StringContent(request.TypeBannerId), "typeBannerId");
            httpContent.Add(new StringContent(request.Text), "text");
            httpContent.Add(new StringContent(request.Path), "path");

            var response = await client.PutAsync($"/api/banners/id?id={id}", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<PageResult<BannerViewModel>> GetAll(PagingWithKeyword request)
        {
            return await GetAsync<PageResult<BannerViewModel>>($"/api/banners/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
        }

        public async Task<BannerViewModel> GetById(string id)
        {
            return await GetAsync<BannerViewModel>($"/api/banners/id?id={id}");
        }
    }
}
