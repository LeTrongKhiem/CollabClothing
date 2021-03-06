using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Threading.Tasks;
using CollabClothing.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using CollabClothing.Data.Entities;
using CollabClothing.Data.EF;
using CollabClothing.ViewModels.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Policy;
using Microsoft.AspNetCore.Http;
using CollabClothing.Utilities.Constants;

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

        #region Constructor
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration, CollabClothingDBContext context,
            ILogger<UserService> logger, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
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
                return new ResultApiError<bool>("Username ???? t???n t???i");
            }
            if (email != null)
            {
                return new ResultApiError<bool>("Email ???? t???n t???i");
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
                await _emailSender.SendEmailAsync(request.Email, "X??c nh???n ?????a ch??? email",
                        $"H??y x??c nh???n ?????a ch??? email b???ng c??ch <a href='{url}'>B???m v??o ????y</a>.");
                if (_userManager.Options.SignIn.RequireConfirmedEmail)
                {
                    return new ResultApiSuccessed<bool>();
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new ResultApiError<bool>("Vui l??ng ki???m tra Email ????? x??c nh???n t??i kho???n");
                }
            }
            return new ResultApiError<bool>("????ng k?? kh??ng th??nh c??ng");
        }
        #endregion
        #region Forgot Password
        public async Task<ResultApi<bool>> ForgotPassword(ForgotPasswordRequest request)
        {
            if (request.Email == null)
            {
                return new ResultApiError<bool>("Vui l??ng nh???p Email c???a b???n !");
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return new ResultApiError<bool>("Email kh??ng t???n t???i vui l??ng ki???m tra l???i.");
            }
            // Ph??t sinh Token ????? reset password
            // Token s??? ???????c k??m v??o link trong email,
            // link d???n ?????n trang /Account/ResetPassword ????? ki???m tra v?? ?????t l???i m???t kh???u

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            string urlWebApp = SystemConstans.AppSettings.urlWebApp;
            var url = urlWebApp + $"/Account/ResetPassword?code={code}";
            await _emailSender.SendEmailAsync(request.Email, "X??c nh???n thay ?????i m???t kh???u",
                $"B???n ???? y??u c???u thay ?????i m???t kh???u. Vui l??ng <a href='{url}'>b???m v??o ????y</a>. N???u kh??ng b???n c?? th??? b??? qua email n??y.");
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
            // X??c th???c email
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
                return new ResultApiError<string>("Username kh??ng t???n t???i");
            }
            var email = await _userManager.FindByEmailAsync(user.Email);
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if (!result.Succeeded)
            {
                return new ResultApiError<string>("????ng nh???p th???t b???i. Vui l??ng ki???m tra l???i m???t kh???u");
            }
            if (!email.EmailConfirmed)
            {
                return new ResultApiError<string>("????ng nh???p th???t b???i. Vui l??ng ki???m tra Email v?? x??c nh???n");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[] {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";", roles)),
                new Claim(ClaimTypes.Name, request.UserName)
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
                return new ResultApiError<bool>("Email ???? t???n t???i");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ResultApiError<bool>("T??i kho???n kh??ng t???n t???i");
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
            return new ResultApiError<bool>("C???p nh???t kh??ng th??nh c??ng");
        }

        public async Task<ResultApi<UserViewModel>> GetById(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ResultApiError<UserViewModel>("T??i kho???n kh??ng t???n t???i");
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
                return new ResultApiError<bool>("T??i kho???n kh??ng t???n t???i");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>("X??a th???t b???i");
        }

        public async Task<ResultApi<bool>> RoleAssign(Guid Id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ResultApiError<bool>("T??i kho???n kh??ng t???n t???i");
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
                return new ResultApiError<bool>("Email kh??ng t???n t???i, Vui l??ng ki???m tra l???i");
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
                return new ResultApiError<bool>("Kh??ng t??m th???y ng?????i d??ng");
            }
            if (request.NewPassword != request.NewPasswordConfirm)
            {
                return new ResultApiError<bool>("M???t kh???u x??c nh???n kh??ng ????ng");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResultApiError<bool>("Password ph???i ch???a k?? t??? in hoa, ch??? s??? v?? k?? t??? ?????c bi???t");
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
                return new ResultApiError<UserViewModel>("T??i kho???n kh??ng t???n t???i");
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
    }
}