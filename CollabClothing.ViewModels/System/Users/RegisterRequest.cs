using System;

namespace CollabClothing.ViewModels.System.Users
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string ComfirmPassword { get; set; }


    }
}