using CollabClothing.Application.Catalog.Chat;
using CollabClothing.Application.System.Users;
using CollabClothing.ViewModels.Catalog.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        public ChatController(IChatService chatService, IUserService userService)
        {
            _chatService = chatService;
            _userService = userService;
        }
        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> Message(MessageDTO message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _chatService.Message(message);
            if (result != null)
                return Ok(result);
            return BadRequest();
        }
        //[HttpPost]
        //[Route("sendmessagetouser")]
        //public async Task<IActionResult> SendMessagesToUser([FromBody] string message)
        //{
        //    var userId = User.GetUserId();
        //    var user = await _userService.GetById(userId);
        //    var model = new MessageDTO()
        //    {
        //        Message = message,
        //        UserName = user.ResultObject.UserName
        //    };
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var result = await _chatService.SendMessagesToUser(model);
        //    if (result != null)
        //        return Ok(result);
        //    return BadRequest();
        //}
        //[HttpPost]
        //[Route("sendmessagetogroup")]
        //public async Task<IActionResult> SendMessageToGroup([FromBody] string receiver, [FromBody] string message)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var userId = User.GetUserId();
        //    var user = await _userService.GetById(userId);
        //    var sender = user.ResultObject.UserName;
        //    var result = await _chatService.SendMessageToGroup(sender, receiver, message);
        //    if (result != null)
        //        return Ok(result);
        //    return BadRequest();
        //}
    }
}
