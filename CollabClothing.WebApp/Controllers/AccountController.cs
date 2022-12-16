using CollabClothing.ApiShared;
using CollabClothing.ViewModels.System.Users;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public AccountController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        //[HttpGet("/dang-nhap")]
        //[HttpGet("")]
        [HttpGet]
        public async Task<IActionResult> Login()
        {

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //await HttpContext.SignOutAsync("1");
            //await HttpContext.SignOutAsync("2");

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //await HttpContext.SignOutAsync("1");
            //await HttpContext.SignOutAsync("2");

            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Home");
        }

        public ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Token:Issuer"];
            validationParameters.ValidIssuer = _configuration["Token:Issuer"];
            validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            var result = await _userApiClient.Authenticate(request);
            if (result.ResultObject == null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var userPrincipal = this.ValidateToken(result.ResultObject);
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", result.ResultObject);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Register(request);

            var resultLogin = await _userApiClient.Authenticate(new LoginRequest()
            {
                UserName = request.UserName,
                Password = request.Password,
                RememberMe = true
            });
            ViewBag.Email = request.Email;
            TempData["Email"] = request.Email;
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            var userPrincipal = this.ValidateToken(resultLogin.ResultObject);
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true
            };

            HttpContext.Session.SetString("Token", resultLogin.ResultObject);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
            return RedirectToAction("Confirm", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.ConfirmEmail(userId, code);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Login", "Account");

            }
            return RedirectToAction("Error");
        }
        [HttpGet]
        public IActionResult Confirm()
        {
            return View();
        }

        #region Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.ForgotPassword(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Confirm", "Account");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        #endregion
        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return RedirectToAction("Confirm", "Account");
            }
            ResetPasswordRequest result = new ResetPasswordRequest()
            {
                // Giải mã lại code từ code trong url (do mã này khi gửi mail
                // đã thực hiện Encode bằng WebEncoders.Base64UrlEncode)
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                //Code = code
            };
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.ResetPassword(request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại sau!!!");
                return View(request);
            }
            return RedirectToAction("Login");
        }
        #endregion
        #region Edit Profile
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.Name).Value;
                var user = await _userApiClient.GetByUsername(userId);
                if (user.IsSuccessed)
                {
                    var userResult = user.ResultObject;
                    var editUser = new UserEditRequest()
                    {
                        Id = userResult.Id,
                        Dob = userResult.Dob,
                        Email = userResult.Email,
                        FirstName = userResult.FirstName,
                        LastName = userResult.LastName,
                        PhoneNumber = userResult.PhoneNumber
                    };

                    var viewModel = new UserEditViewModel()
                    {
                        EditRequest = editUser
                    };
                    if (TempData["result"] != null)
                    {
                        ViewBag.SuccessMsg = TempData["result"];
                    }
                    return View(viewModel);
                }
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            string userId = User.FindFirst(ClaimTypes.Name).Value;
            var user = await _userApiClient.GetByUsername(userId);
            var result = await _userApiClient.Edit(user.ResultObject.Id, request.EditRequest);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("EditProfile", "Account");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        #endregion
        #region Change Password
        [HttpPost]
        public async Task<IActionResult> EditProfilePassword(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            string username = User.FindFirst(ClaimTypes.Name).Value;
            var user = await _userApiClient.GetByUsername(username);
            var result = await _userApiClient.EditPassword(user.ResultObject.Id, request.EditPassword);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("EditProfile", "Account");
            }
            TempData["result"] = "Cập nhật thất bại";
            return RedirectToAction("EditProfile", "Account");
        }
        #endregion

        #region Login With Google
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string url = Url.Action("GoogleResponse", "Account");
            var properties = _userApiClient.GoogleLogin(url);
            return new ChallengeResult("Google", properties.Result);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await _userApiClient.GoogleResponse();
            if (result != null)
                return View(result);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
