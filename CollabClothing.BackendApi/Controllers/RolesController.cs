using CollabClothing.Application.System.Roles;
using CollabClothing.ViewModels.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetAll();
            return Ok(roles);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] RoleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _roleService.Create(request);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _roleService.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Role not found");
        }
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(Guid id, [FromBody] RoleEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _roleService.Edit(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest("Role not found");
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _roleService.Delete(id);
            if (!result.IsSuccessed)
            {
                return BadRequest("Role not found");
            }
            return Ok(result);
        }
    }
}
