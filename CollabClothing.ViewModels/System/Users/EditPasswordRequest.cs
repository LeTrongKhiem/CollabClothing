using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.System.Users
{
    public class EditPasswordRequest
    {
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
    }
}
