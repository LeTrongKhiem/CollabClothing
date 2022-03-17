using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            SystemActivities = new HashSet<SystemActivity>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<SystemActivity> SystemActivities { get; set; }
    }
}
