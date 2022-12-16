using CollabClothing.Application.System.Users;
using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    public class demo111 : IDemo111
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
        public demo111(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration, CollabClothingDBContext context,
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
        public demo111()
        {

        }
        #endregion
        public async Task<string[]> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return null;
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            string[] userInfor = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };

            if (result.Succeeded)
            {
                return userInfor;
            }
            else
            {
                var user = new AppUser()
                {
                    UserName = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    Dob = DateTime.Now,
                    FirstName = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    LastName = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    PhoneNumber = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    EmailConfirmed = true
                };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    createResult = await _userManager.AddLoginAsync(user, info);
                    if (createResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return userInfor;
                    }
                }
                return null;
            }
        }
    }
}
