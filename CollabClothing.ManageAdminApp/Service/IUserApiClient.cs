using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Service
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PageResult<UserViewModel>> GetListUser(GetUserRequestPaging request);
        Task<bool> Register(RegisterRequest request);
    }
}
