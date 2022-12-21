using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface IUserApiClient
    {
        Task<ResultApi<string>> Authenticate(LoginRequest request);
        Task<ResultApi<PageResult<UserViewModel>>> GetListUser(GetUserRequestPaging request);
        Task<ResultApi<bool>> Register(RegisterRequest request);
        Task<ResultApi<bool>> Edit(Guid id, UserEditRequest request);
        Task<ResultApi<UserViewModel>> GetById(Guid id);
        Task<ResultApi<UserViewModel>> GetByUsername(string username);
        Task<ResultApi<bool>> Delete(Guid id);
        Task<ResultApi<bool>> RolesAssign(Guid id, RoleAssignRequest request);
        Task<ResultApi<bool>> ConfirmEmail(string id, string code);

        Task<ResultApi<bool>> ForgotPassword(ForgotPasswordRequest request);
        Task<ResultApi<bool>> ResetPassword(ResetPasswordRequest request);
        Task<ResultApi<bool>> EditPassword(Guid id, EditPasswordRequest request);
        Task<AuthenticationProperties> GoogleLogin(string url);
        Task<AuthenticationProperties> FacebookLogin(string url);
        Task<UserViewModel> GoogleResponse();
    }
}
