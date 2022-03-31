using System.Threading.Tasks;
using CollabClothing.ViewModels.System.Users;

namespace CollabClothing.Application.System.Users
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
    }
}