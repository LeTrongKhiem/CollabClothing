using CollabClothing.ManageAdminApp.Service;
using CollabClothing.ViewModels.System.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleApiClient _roleApiClient;
        private readonly IConfiguration _configuration;
        public RoleController(IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _configuration = configuration;
            _roleApiClient = roleApiClient;

        }
        public async Task<IActionResult> Index()
        {
            var result = await _roleApiClient.GetAll();
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _roleApiClient.Create(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm người Role mới thành công";

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var role = new RoleDeleteRequest()
            {
                Id = id
            };
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(RoleDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _roleApiClient.Delete(request.Id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Xóa Role thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var role = await _roleApiClient.GetById(id);
            if (role.IsSuccessed)
            {
                var roleResult = role.ResultObject;
                var editRole = new RoleEditRequest()
                {
                    Id = id,
                    Name = roleResult.Name,
                    Description = roleResult.Description
                };

                return View(editRole);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _roleApiClient.Edit(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật Role thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

    }
}
