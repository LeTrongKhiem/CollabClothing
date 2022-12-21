using CollabClothing.Application.System.Users;
using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserServiceResponse _userServiceResponse;
        private readonly IDemo111 _demo;
        public UsersController(IUserService userService, IDemo111 demo, IUserServiceResponse userServiceResponse)
        {
            _userService = userService;
            _demo = demo;
            _userServiceResponse = userServiceResponse;
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) //dung frombody de post duoc voi dang json
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.ForgotPassword(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.ResetPassword(request);
            if (!result.IsSuccessed)
            {
                return BadRequest("Reset failed");
            }
            return Ok();
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userService.Authenticate(request);
            if (string.IsNullOrEmpty(resultToken.ResultObject))
            {
                return BadRequest(resultToken);
            }
            return Ok(resultToken);
        }
        [HttpGet("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.ConfirmEmail(userId, code);
            if (!result.IsSuccessed)
            {
                return BadRequest("Unconfimred");
            }
            return Ok();
        }

        //https://localhost/api/users/paging?pageIndex=1&pageSize=10&keyword=
        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListUser([FromQuery] GetUserRequestPaging request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var users = await _userService.GetListUser(request);
            if (users == null)
            {
                return BadRequest("No User find");
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var users = await _userService.GetById(id);
            if (users == null)
            {
                return BadRequest("No User find");
            }
            return Ok(users);
        }
        [HttpGet("get/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByUsername(string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var users = await _userService.GetByUsername(username);
            if (users == null)
            {
                return BadRequest("No User find");
            }
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] UserEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Edit(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("newpassword/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] EditPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdatePassword(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RolesAssign(Guid id, RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _userService.RoleAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Delete(id);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] string url)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.GoogleLogin(url);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] string url)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.FacebookLogin(url);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.GoogleResponse();
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }

    public interface IUserServiceResponse
    {
        Task<ChallengeResult> GoogleLogin(string url);

        Task<string[]> GoogleResponse();
    }


    public class UserServiceResponse : IUserServiceResponse
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
        public UserServiceResponse(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration, CollabClothingDBContext context,
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

        public async Task<ChallengeResult> GoogleLogin(string url)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", "api/users/google-response");
            return new ChallengeResult("Google", properties);
        }

        public async Task<string[]> GoogleResponse()
        {
            //var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", "users/google-response");

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
                    EmailConfirmed = true,
                    Id = Guid.NewGuid(),
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