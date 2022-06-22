using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.System.Users
{
    public class ForgotPasswordRequest
    {
        [Display(Name = "Email đăng ký")]
        public string Email { get; set; }
    }
}
