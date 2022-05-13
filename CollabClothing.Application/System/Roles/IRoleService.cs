using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAll();
        Task<ResultApi<bool>> Create(RoleCreateRequest request);
        Task<ResultApi<bool>> Edit(Guid id, RoleEditRequest request);
        Task<ResultApi<bool>> Delete(Guid Id);
        Task<ResultApi<RoleViewModel>> GetById(Guid id);
    }
}
