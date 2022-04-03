using System.Threading.Tasks;
using CollabClothing.ViewModels.System.Users;

namespace CollabClothing.Application.System.Users
{
    public interface IUserService
    {
        public Task<string> Authenticate(LoginRequest request);
        public Task<bool> Register(RegisterRequest request);
    }
}