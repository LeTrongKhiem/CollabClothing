using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.System.Users
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Tên tài khoản")]
        public string UserName { get; set; }
        [Display(Name = "Tên")]
        public string FirstName { get; set; }
        [Display(Name = "Họ")]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        //public string Address { get; set; }
        public IList<string> Roles { get; set; }

    }
}
