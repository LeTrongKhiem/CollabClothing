using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.System.Mail
{
    public interface IMailService
    {
        Task SendMailAsync(string email, string subject, string htmlMessage);
    }
}
