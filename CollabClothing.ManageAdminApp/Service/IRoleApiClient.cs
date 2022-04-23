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
        Task<ResultApi<List<RoleViewModel>>> GetAll();
    }
}
