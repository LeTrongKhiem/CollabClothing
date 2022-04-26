using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Service
{
    public interface IRoleApiClient
    {
        Task<List<RoleViewModel>> GetAll();
        Task<ResultApi<bool>> Create(RoleCreateRequest request);
        Task<ResultApi<bool>> Edit(Guid id, RoleEditRequest request);
        Task<ResultApi<bool>> Delete(Guid id);
        Task<ResultApi<RoleViewModel>> GetById(Guid id);
    }
}
