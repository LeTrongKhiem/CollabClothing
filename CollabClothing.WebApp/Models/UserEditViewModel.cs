using CollabClothing.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Models
{
    public class UserEditViewModel
    {
        public UserEditRequest EditRequest { get; set; }
        public EditPasswordRequest EditPassword { get; set; }
    }
}
