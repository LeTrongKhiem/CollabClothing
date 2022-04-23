using System;
using System.Threading.Tasks;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Users;

namespace CollabClothing.Application.System.Users
{
    public interface IUserService
    {
        public Task<ResultApi<string>> Authenticate(LoginRequest request);
        public Task<ResultApi<bool>> Register(RegisterRequest request);
        public Task<ResultApi<bool>> Edit(Guid id, UserEditRequest request);
        public Task<ResultApi<PageResult<UserViewModel>>> GetListUser(GetUserRequestPaging request);
        public Task<ResultApi<UserViewModel>> GetById(Guid Id);
        public Task<ResultApi<bool>> Delete(Guid Id);
        public Task<ResultApi<bool>> RoleAssign(Guid Id, RoleAssignRequest request);
    }
}