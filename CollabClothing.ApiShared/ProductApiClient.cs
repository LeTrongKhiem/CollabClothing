using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Catalog.ProductImages;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Catalog.Sizes;
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
                foreach (var item in request.ThumbnailImage)
                {
                    using (var br = new BinaryReader(item.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)item.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "thumbnailImage", item.FileName);
                }

            }
            requestContent.Add(new StringContent(request.ProductName), "productName");
            requestContent.Add(new StringContent(request.PriceCurrent.ToString()), "priceCurrent");
            requestContent.Add(new StringContent(request.PriceOld.ToString()), "priceOld");
            requestContent.Add(new StringContent(request.SaleOff.ToString()), "saleOff");
            requestContent.Add(new StringContent(request.BrandId), "brandId");
            requestContent.Add(new StringContent(request.SoldOut.ToString()), "soldOut");
            requestContent.Add(new StringContent(request.Installment.ToString()), "installment");
            requestContent.Add(new StringContent(request.Description), "description");
            //requestContent.Add(new StringContent(request.Slug), "slug");
            requestContent.Add(new StringContent(request.Details), "details");
            foreach (var item in request.CategoryId)
            {
                requestContent.Add(new StringContent(item.ToString()), "categoryId");
            }

            requestContent.Add(new StringContent(request.Consumer.ToString()), "consumer");
            requestContent.Add(new StringContent(request.Form), "form");
            requestContent.Add(new StringContent(request.Type), "type");
            requestContent.Add(new StringContent(request.MadeIn), "madeIn");
            requestContent.Add(new StringContent(request.Cotton.ToString()), "cotton");
            var response = await client.PostAsync($"/api/products/", requestContent);
            return response.IsSuccessStatusCode;

        }
        public async Task<bool> Edit(string id, ProductEditRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent byteArrayContent = new ByteArrayContent(data);
                requestContent.Add(byteArrayContent, "thumbnailImage", request.ThumbnailImage.FileName);

            }
            requestContent.Add(new StringContent(request.ProductName), "productName");
            requestContent.Add(new StringContent(request.Details), "details");
            requestContent.Add(new StringContent(request.Description), "description");
            requestContent.Add(new StringContent(request.Slug), "slug");
            //requestContent.Add(new StringContent(request.ImagePath), "imagePath");
            requestContent.Add(new StringContent(request.BrandId), "brandId");

            requestContent.Add(new StringContent(request.Consumer.ToString()), "consumer");
            requestContent.Add(new StringContent(request.Form), "form");
            requestContent.Add(new StringContent(request.Type), "type");
            requestContent.Add(new StringContent(request.MadeIn), "madeIn");
            requestContent.Add(new StringContent(request.Cotton.ToString()), "cotton");
            var response = await client.PutAsync($"/api/products/{id}", requestContent);
            return response.IsSuccessStatusCode;
        }


        public async Task<PageResult<ProductViewModel>> GetAll(GetManageProductRequestPaging request)
        {
            return await GetAsync<PageResult<ProductViewModel>>($"/api/products/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&categoryId={request.CategoryId}&brandId={request.BrandId}");
        }

        public async Task<ProductViewModel> GetById(string id)
        {
            return await GetAsync<ProductViewModel>($"/api/products/{id}");
        }
        public async Task<bool> Delete(string productId)
        {
            return await DeleteAsync($"/api/products/{productId}");
        }

        public async Task<bool> CategoryAssign(string id, CategoryAssignRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"/api/products/{id}/categories", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<bool>(result);
            }
            return JsonConvert.DeserializeObject<bool>(result);
        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(int take)
        {
            return await GetListAsync<ProductViewModel>($"/api/products/featured/{take}");
        }

        public async Task<List<ProductViewModel>> GetFeaturedProductsByCategory(string id, int take)
        {
            return await GetListAsync<ProductViewModel>($"/api/products/featured/{id}/{take}");
        }

        public async Task<bool> UpdateCurrentPrice(string id, decimal newCurrentPrice)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var json = JsonConvert.SerializeObject(newCurrentPrice);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.PutAsync($"/api/products/newPriceCurrent/{id}/{newCurrentPrice}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePriceOld(string id, decimal newPriceOld)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var json = JsonConvert.SerializeObject(newPriceOld);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.PutAsync($"/api/products/newPriceOld/{id}/{newPriceOld}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            return await GetListAsync<ProductViewModel>($"/api/products/");
        }

        public async Task<List<ProductImageViewModel>> GetAllImages(string id)
        {
            return await GetListAsync<ProductImageViewModel>($"/api/products/images/{id}");
        }

        public async Task<bool> CreateProductImages(string productId, ProductImageCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();
            //chuyen thumbnail image sang binary array
            if (request.File != null)
            {
                byte[] data;
                foreach (var item in request.File)
                {
                    using (var br = new BinaryReader(item.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)item.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "file", item.FileName);
                }

            }
            requestContent.Add(new StringContent(request.Alt), "alt");
            requestContent.Add(new StringContent(request.IsThumbnail.ToString()), "isThumbnail");

            var response = await client.PostAsync($"/api/products/{productId}/images", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductImages(string id, ProductImageEditRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent();
            if (request.File != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.File.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.File.OpenReadStream().Length);
                }
                ByteArrayContent byteArrayContent = new ByteArrayContent(data);
                requestContent.Add(byteArrayContent, "file", request.File.FileName);

            }
            requestContent.Add(new StringContent(request.Alt), "alt");
            requestContent.Add(new StringContent(request.IsThumbnail.ToString()), "isThumbnail");
            var response = await client.PutAsync($"/api/products/images/{id}", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductImages(string id)
        {
            return await DeleteAsync($"/api/products/images/{id}");
        }

        public async Task<ProductImageViewModel> GetProductImagesById(string id)
        {
            return await GetAsync<ProductImageViewModel>($"/api/products/images/product/{id}");
        }

        public async Task<string> GetBrandNameByProductId(string productId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/products/getbrand/{productId}");
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<bool> SizeAssign(string id, SizeAssignRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"/api/products/sizes/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<bool>(result);
            }
            return JsonConvert.DeserializeObject<bool>(result);
        }

        public async Task<bool> PromotionAssign(string id, PromotionAssignRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"/api/products/{id}/promotions", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<bool>(result);
            }
            return JsonConvert.DeserializeObject<bool>(result);
        }
        public async Task<List<SizeViewModel>> GetSizeNameByProductId(string productId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/products/sizename/{productId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<SizeViewModel> data = (List<SizeViewModel>)JsonConvert.DeserializeObject(result, typeof(List<SizeViewModel>));
                return data;
            }
            throw new Exception(result);
        }

        public async Task<string> GetNameProductById(string id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/products/getnameproduct/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<int> GetQuantityRemain(string productId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/products/getquantity/{productId}");
            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }
            var result = await response.Content.ReadAsStringAsync();
            int quantity = Int32.Parse(result);
            return quantity;
        }

        public async Task<int> GetQuantityRemain(string productId, string sizeId, string colorId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/products/getquantitysizecolor?id={productId}&sizeId={sizeId}&colorId={colorId}");
            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }
            var result = await response.Content.ReadAsStringAsync();
            int quantity = Int32.Parse(result);
            return quantity;
        }

        public async Task<bool> UpdateQuantityRemain(string productId, WareHouseRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.PutAsync($"/api/products/quantityremain/{productId}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }

        public async Task<WareHouseRequest> GetWareHouse(string productId)
        {
            return await GetAsync<WareHouseRequest>($"/api/products/getwarehouse/{productId}");
        }

        public async Task<WareHouseRequest> GetWareHouse(string productId, string sizeId)
        {
            //return await GetAsync<WareHouseRequest>($"/api/products/getwarehouse/index?id={productId}&sizeId={sizeId}");
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/products/getwarehouse/index?id={productId}&sizeId={sizeId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<WareHouseRequest>(result);
            }
            return null;
        }

        public async Task<WareHouseRequest> GetWareHouse(string productId, string sizeId, string colorId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/products/getwarehouse/filter?id={productId}&sizeId={sizeId}&colorId={colorId}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<WareHouseRequest>(result);
            }
            return null;
        }
    }
}
