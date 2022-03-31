using CollabClothing.Application.System.Users;
using Microsoft.AspNetCore.Mvc;

namespace CollabClothing.BackendApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
    }
}