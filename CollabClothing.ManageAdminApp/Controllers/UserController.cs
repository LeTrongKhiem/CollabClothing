using AspNetCoreHero.ToastNotification.Abstractions;
using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;
        //private readonly INotyfService _notyfService;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;

        }
        public async Task<IActionResult> Index(string keyword, Guid roleId, int pageIndex = 1, int pageSize = 10)
        {
            //var session = HttpContext.Session.GetString("Token"); //tao base controller
            var request = new GetUserRequestPaging()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                RoleId = roleId
            };
            var data = await _userApiClient.GetListUser(request);
            ViewBag.Keyword = keyword;
            var roles = await _roleApiClient.GetAll();
            ViewBag.Roles = roles.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = roleId == x.Id && roleId.ToString() != null
            });
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObject);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Register(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm người dùng mới thành công";

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            if (user.IsSuccessed)
            {
                var userResult = user.ResultObject;
                var editUser = new UserEditRequest()
                {
                    Id = id,
                    Dob = userResult.Dob,
                    Email = userResult.Email,
                    FirstName = userResult.FirstName,
                    LastName = userResult.LastName,
                    PhoneNumber = userResult.PhoneNumber
                };

                return View(editUser);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _userApiClient.Edit(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);

        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var user = await _userApiClient.GetById(Id);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(user.ResultObject);

        }
        [HttpGet]
        public IActionResult Delete(Guid Id)
        {
            var user = new UserDeleteRequest()
            {
                Id = Id
            };
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);
            var result = await _userApiClient.Delete(request.Id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Xóa người dùng thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "Home");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> RolesAssign(Guid id)
        {
            var rolesAssignRequest = await GetRoleAssignRequest(id);
            return View(rolesAssignRequest);
        }
        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            var roles = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roles)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = user.ResultObject.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }

        [HttpPost]
        public async Task<IActionResult> RolesAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _userApiClient.RolesAssign(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật quyền thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "");
            var rolesAssignRequest = GetRoleAssignRequest(request.Id);
            return View(rolesAssignRequest);

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }


    }
}
