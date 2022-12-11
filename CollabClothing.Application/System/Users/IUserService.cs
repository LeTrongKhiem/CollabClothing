using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;

namespace CollabClothing.Application.System.Users
{
    public interface IUserService
    {
        public Task<ResultApi<string>> Authenticate(LoginRequest request);
        public Task<ResultApi<bool>> Register(RegisterRequest request);
        public Task<ResultApi<bool>> Edit(Guid id, UserEditRequest request);
        public Task<ResultApi<PageResult<UserViewModel>>> GetListUser(GetUserRequestPaging request);
        public Task<ResultApi<UserViewModel>> GetById(Guid Id);
        public Task<ResultApi<UserViewModel>> GetByUsername(string username);
        public Task<ResultApi<bool>> Delete(Guid Id);
        public Task<ResultApi<bool>> RoleAssign(Guid Id, RoleAssignRequest request);
        public Task<ResultApi<bool>> ConfirmEmail(string userId, string code);
        public Task<ResultApi<bool>> ForgotPassword(ForgotPasswordRequest request);
        public Task<ResultApi<bool>> ResetPassword(ResetPasswordRequest request);
        public Task<ResultApi<bool>> UpdatePassword(Guid id, EditPasswordRequest request);
        Task<ResultApi<bool>> UpdateEmail(Guid id, string newEmail);
        Task<Guid?> GetUserId();

        Task<AuthenticationProperties> GoogleLogin(string url);

        Task<string[]> GoogleResponse();
    }
}