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

namespace CollabClothing.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly CollabClothingDBContext _context;
        //private readonly ILogger<UserService> _logger;
        //private readonly IEmailSender _emailSender;

        #region Constructor
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration, CollabClothingDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
            _context = context;

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
            //var userRole = new RoleManager()
            //{

            //};
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>("Đăng kí không thành công");
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
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return new ResultApiError<string>("Đăng nhập thất bại");
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
                //query = query.Where(x => x.user.UserName.Contains(request.Keyword) || x.user.PhoneNumber.Contains(request.Keyword) || x.user.LastName.Contains(request.Keyword)
                //|| x.user.Email.Contains(request.Keyword));
            }
            //if (!string.IsNullOrEmpty(request.RoleId.ToString()))
            //{
            //    query = query.Where(x => x.role.Id == request.RoleId);
            //}
            //total row 
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
    }
}