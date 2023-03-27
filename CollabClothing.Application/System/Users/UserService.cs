using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly CollabClothingDBContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #region Constructor
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration, CollabClothingDBContext context,
            ILogger<UserService> logger, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion
        #region Register
        public async Task<ResultApi<bool>> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var email = await _userManager.FindByEmailAsync(request.Email);
            var role = await _roleManager.FindByNameAsync("User");
            if (user != null)
            {
                return new ResultApiError<bool>("Username đã tồn tại");
            }
            if (email != null)
            {
                return new ResultApiError<bool>("Email đã tồn tại");
            }
            Guid g = Guid.NewGuid();
            user = new AppUser()
            {
                Id = g,
                UserName = request.UserName,
                Email = request.Email,
                Dob = request.Dob,
                PhoneNumber = request.Phone,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                string urlWebApp = SystemConstans.AppSettings.urlWebApp;
                var url = urlWebApp + $"/Account/ConfirmEmail?userId={user.Id}&code={code}";
                await _emailSender.SendEmailAsync(request.Email, "Xác nhận địa chỉ email",
                        $"Hãy xác nhận địa chỉ email bằng cách <a href='{url}'>Bấm vào đây</a>.");
                if (_userManager.Options.SignIn.RequireConfirmedEmail)
                {
                    return new ResultApiSuccessed<bool>();
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new ResultApiError<bool>("Vui lòng kiểm tra Email để xác nhận tài khoản");
                }
            }
            return new ResultApiError<bool>("Đăng kí không thành công");
        }
        #endregion
        #region Forgot Password
        public async Task<ResultApi<bool>> ForgotPassword(ForgotPasswordRequest request)
        {
            if (request.Email == null)
            {
                return new ResultApiError<bool>("Vui lòng nhập Email của bạn !");
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return new ResultApiError<bool>("Email không tồn tại vui lòng kiểm tra lại.");
            }
            // Phát sinh Token để reset password
            // Token sẽ được kèm vào link trong email,
            // link dẫn đến trang /Account/ResetPassword để kiểm tra và đặt lại mật khẩu

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            string urlWebApp = SystemConstans.AppSettings.urlWebApp;
            var url = urlWebApp + $"/Account/ResetPassword?code={code}";
            await _emailSender.SendEmailAsync(request.Email, "Xác nhận thay đổi mật khẩu",
                $"Bạn đã yêu cầu thay đổi mật khẩu. Vui lòng <a href='{url}'>bấm vào đây</a>. Nếu không bạn có thể bỏ qua email này.");
            return new ResultApiSuccessed<bool>();
        }
        #endregion
        #region ConfirmEmail
        public async Task<ResultApi<bool>> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new ResultApiError<bool>("Confirm Failed");
            }
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            // Xác thực email
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>("Confirm Failed");
        }
        #endregion
        #region Authenticate
        public async Task<ResultApi<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new ResultApiError<string>("Username không tồn tại");
            }
            var email = await _userManager.FindByEmailAsync(user.Email);
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if (!result.Succeeded)
            {
                return new ResultApiError<string>("Đăng nhập thất bại. Vui lòng kiểm tra lại mật khẩu");
            }
            if (!email.EmailConfirmed)
            {
                return new ResultApiError<string>("Đăng nhập thất bại. Vui lòng kiểm tra Email và xác nhận");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[] {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";", roles)),
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Token:Issuer"],
                        _config["Token:Issuer"],
                        claims,
                        expires: DateTime.Now.AddHours(3),
                        signingCredentials: creds);
            return new ResultApiSuccessed<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }
        #endregion
        #region GetListUser
        public async Task<ResultApi<PageResult<UserViewModel>>> GetListUser(GetUserRequestPaging request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword) || x.LastName.Contains(request.Keyword)
                                        || x.Email.Contains(request.Keyword));
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .Select(x => new UserViewModel()
                            {
                                Id = x.Id,
                                Dob = x.Dob,
                                Email = x.Email,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                PhoneNumber = x.PhoneNumber,
                                UserName = x.UserName
                            }).ToListAsync();
            var pageResult = new PageResult<UserViewModel>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return new ResultApiSuccessed<PageResult<UserViewModel>>(pageResult);
        }
        #endregion

        public async Task<ResultApi<bool>> Edit(Guid id, UserEditRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ResultApiError<bool>("Email đã tồn tại");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ResultApiError<bool>("Tài khoản không tồn tại");
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Dob = request.Dob;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>("Cập nhật không thành công");
        }

        public async Task<ResultApi<UserViewModel>> GetById(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ResultApiError<UserViewModel>("Tài khoản không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserViewModel()
            {
                Id = user.Id,
                Dob = user.Dob,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Roles = roles
            };
            return new ResultApiSuccessed<UserViewModel>(userViewModel);
        }

        public async Task<ResultApi<bool>> Delete(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ResultApiError<bool>("Tài khoản không tồn tại");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>("Xóa thất bại");
        }

        public async Task<ResultApi<bool>> RoleAssign(Guid Id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ResultApiError<bool>("Tài khoản không tồn tại");
            }
            //xoa role
            var deleteRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roles in deleteRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roles) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roles);
                }
            }

            var addRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var roles in addRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roles) == false)
                {
                    await _userManager.AddToRoleAsync(user, roles);
                }
            }
            return new ResultApiSuccessed<bool>();

        }

        public async Task<ResultApi<bool>> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ResultApiError<bool>("Email không tồn tại, Vui lòng kiểm tra lại");
            }
            var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);

            if (!result.Succeeded)
            {
                return new ResultApiError<bool>("Reset Password Failed. Please try again!!!");
            }
            return new ResultApiSuccessed<bool>();
        }

        public async Task<ResultApi<bool>> UpdatePassword(Guid id, EditPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ResultApiError<bool>("Không tìm thấy người dùng");
            }
            if (request.NewPassword != request.NewPasswordConfirm)
            {
                return new ResultApiError<bool>("Mật khẩu xác nhận không đúng");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResultApiError<bool>("Password phải chứa kí tự in hoa, chữ số và kí tự đặc biệt");
            }
            return new ResultApiSuccessed<bool>();
        }

        public Task<ResultApi<bool>> UpdateEmail(Guid id, string newEmail)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultApi<UserViewModel>> GetByUsername(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ResultApiError<UserViewModel>("Tài khoản không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserViewModel()
            {
                Id = user.Id,
                Dob = user.Dob,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Roles = roles
            };
            return new ResultApiSuccessed<UserViewModel>(userViewModel);
        }

        public async Task<Guid?> GetUserId()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return user?.Id;
        }

        public async Task<AuthenticationProperties> GoogleLogin(string url)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);
            return properties;
        }

        public async Task<AuthenticationProperties> FacebookLogin(string url)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", url);
            return properties;
        }

        public async Task<AppUser> GoogleResponse()
        {
            //var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);

            //ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            var user = new AppUser()
            {
                FirstName = "Khiem",
                LastName = "Le",
                Email = "lekhiem20011@gmail.com",
                Id = Guid.NewGuid(),
                UserName = "lekhiem20011",
                Dob = new DateTime(2001, 12, 12),
                EmailConfirmed = true
            };
            //if (info == null)
            //{
            //    return null;
            //}
            //var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            //string[] userInfor = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };

            //if (result.Succeeded)
            //{
            //    return userInfor;
            //}
            //else
            //{
            //var user = new AppUser()
            //{
            //    UserName = info.Principal.FindFirst(ClaimTypes.Name).Value,
            //    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
            //    Dob = DateTime.Now,
            //    FirstName = info.Principal.FindFirst(ClaimTypes.Name).Value,
            //    LastName = info.Principal.FindFirst(ClaimTypes.Name).Value,
            //    PhoneNumber = info.Principal.FindFirst(ClaimTypes.Name).Value,
            //    EmailConfirmed = true,
            //    Id = Guid.NewGuid(),
            //};
            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                //createResult = await _userManager.AddLoginAsync(user, info);
                if (createResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return user;
                }
            }
            return user;
        }
    }
}
//}