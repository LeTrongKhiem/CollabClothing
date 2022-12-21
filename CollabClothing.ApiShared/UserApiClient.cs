using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResultApi<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/users/authenticate", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ResultApiError<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ResultApi<bool>> Edit(Guid id, UserEditRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.PutAsync($"/api/users/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        public async Task<ResultApi<bool>> EditPassword(Guid id, EditPasswordRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstans.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstans.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"/api/users/newpassword/{id}", httpContent);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        //method register
        public async Task<ResultApi<bool>> Register(RegisterRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync($"/api/users/register", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        public async Task<ResultApi<UserViewModel>> GetById(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/users/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<UserViewModel>>(body);
            }
            return JsonConvert.DeserializeObject<ResultApiError<UserViewModel>>(body);
        }

        public async Task<ResultApi<PageResult<UserViewModel>>> GetListUser(GetUserRequestPaging request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/users/paging?pageIndex={request.PageIndex}&" +
                $"pageSize={request.PageSize}&keyword={request.Keyword}&roleId={request.RoleId}");
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ResultApiSuccessed<PageResult<UserViewModel>>>(body);
            return users;
        }
        public async Task<ResultApi<bool>> Delete(Guid Id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.DeleteAsync($"/api/users/{Id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        //role assign
        public async Task<ResultApi<bool>> RolesAssign(Guid id, RoleAssignRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"/api/users/{id}/roles", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        public async Task<ResultApi<bool>> ConfirmEmail(string id, string code)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/users/confirm?userId={id}&code={code}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>();
        }

        public async Task<ResultApi<bool>> ForgotPassword(ForgotPasswordRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync($"/api/users/forgotpassword", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultApiSuccessed<bool>>(result);
            return JsonConvert.DeserializeObject<ResultApiError<bool>>(result);
        }

        public async Task<ResultApi<bool>> ResetPassword(ResetPasswordRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.PostAsync($"/api/users/resetpassword", httpContent);
            //var response = await client.PostAsync($"/api/users/resetpassword?Email={request.Email}&Password={request.Password}&ConfirmPassword={request.ConfirmPassword}&Code={request.Code}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>(result);
        }

        public async Task<ResultApi<UserViewModel>> GetByUsername(string username)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/users/get/{username}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultApiSuccessed<UserViewModel>>(body);
            }
            return JsonConvert.DeserializeObject<ResultApiError<UserViewModel>>(body);
        }

        public async Task<AuthenticationProperties> GoogleLogin(string url)
        {
            var json = JsonConvert.SerializeObject(url);
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.PostAsync($"/api/users/google-login", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<AuthenticationProperties>(result));
            }
            return null;
        }

        public async Task<AuthenticationProperties> FacebookLogin(string url)
        {
            var json = JsonConvert.SerializeObject(url);
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.PostAsync($"/api/users/facebook-login", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<AuthenticationProperties>(result));
            }
            return null;
        }

        public async Task<UserViewModel> GoogleResponse()
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            //var json = JsonConvert.SerializeObject(url);
            //var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            //var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/users/google-response");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<UserViewModel>(result);
            }
            return null;
        }
    }
}
