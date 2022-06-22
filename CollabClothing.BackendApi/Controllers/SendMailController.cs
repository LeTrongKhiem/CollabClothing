using CollabClothing.Application.System.Mail;
using CollabClothing.ViewModels.System.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SendMailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<SendMailController> _logger;
        public SendMailController(IConfiguration configuration, IEmailSender emailSender, ILogger<SendMailController> logger)
        {
            _configuration = configuration;
            _emailSender = emailSender;
            _logger = logger;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SendMailDemo()
        {
            var url = $"https://localhost:5003/Account/ConfirmEmail?userId={1}&code={2}";
            var sendMail = _emailSender.SendEmailAsync("lekhiem2001@gmail.com", "test", $"{url}");
            return Ok();
        }
    }
}
