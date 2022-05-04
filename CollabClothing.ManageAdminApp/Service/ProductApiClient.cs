using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.Products;
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

namespace CollabClothing.ManageAdminApp.Service
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IConfiguration _configuration;
        public ProductApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<bool> Create(ProductCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();
            //chuyen thumbnail image sang binary array
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }
            requestContent.Add(new StringContent(request.ProductName), "productName");
            requestContent.Add(new StringContent(request.PriceCurrent.ToString()), "priceCurrent");
            requestContent.Add(new StringContent(request.PriceOld.ToString()), "priceOld");
            requestContent.Add(new StringContent(request.SaleOff.ToString()), "saleOff");
            requestContent.Add(new StringContent(request.BrandId), "brandId");
            requestContent.Add(new StringContent(request.SoldOut.ToString()), "soldOut");
            requestContent.Add(new StringContent(request.Installment.ToString()), "installment");
            requestContent.Add(new StringContent(request.Description), "description");
            requestContent.Add(new StringContent(request.Slug), "slug");
            requestContent.Add(new StringContent(request.Details), "details");
            requestContent.Add(new StringContent(request.CategoryId), "categoryId");

            var response = await client.PostAsync($"/api/products/", requestContent);
            return response.IsSuccessStatusCode;

        }

        public async Task<bool> Delete(string productId)
        {
            return await DeleteAsync($"/api/products/{productId}");
        }

        public async Task<bool> Edit(ProductEditRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"api/products/{request.Id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }

        public async Task<PageResult<ProductViewModel>> GetAll(GetManageProductRequestPaging request)
        {
            return await GetAsync<PageResult<ProductViewModel>>($"/api/products/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&categoryId={request.CategoryId}");
        }

        public async Task<ProductViewModel> GetById(string id)
        {
            return await GetAsync<ProductViewModel>($"/api/products/{id}");
        }
    }
}
