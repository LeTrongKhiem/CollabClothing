using CollabClothing.Data.Entities;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResultApi<bool>> Create(RoleCreateRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.Name);
            if (role != null)
            {
                return new ResultApiError<bool>("Role đã tồn tại");
            }
            Guid g = Guid.NewGuid();
            role = new AppRole()
            {
                Id = g,
                Name = request.Name,
                Description = request.Description,
                NormalizedName = request.Name,
                ConcurrencyStamp = g.ToString()
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>("Tạo mới Role thất bại");
        }

        public async Task<ResultApi<bool>> Delete(Guid Id)
        {
            var role = await _roleManager.FindByIdAsync(Id.ToString());
            if (role == null)
            {
                return new ResultApiError<bool>("Role không tồn tại");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>("Xóa Role thất bại");
        }

        public async Task<ResultApi<bool>> Edit(Guid id, RoleEditRequest request)
        {
            //if (await _roleManager.Roles.AnyAsync(x => x.Name == request.Name && x.Id == request.Id))
            //{
            //    return new ResultApiError<bool>("Đã tồn tại role");
            //}
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return new ResultApiError<bool>("Role không tồn tại");
            }
            role.Name = request.Name;
            role.Description = request.Description;
            role.NormalizedName = request.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return new ResultApiSuccessed<bool>();
            }
            return new ResultApiError<bool>("Cập nhật Role thất bại");
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
            return roles;
        }

        public async Task<ResultApi<RoleViewModel>> GetById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return new ResultApiError<RoleViewModel>("Role không tồn tại");
            }
            var roleVm = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
            return new ResultApiSuccessed<RoleViewModel>(roleVm);
        }
    }
}
