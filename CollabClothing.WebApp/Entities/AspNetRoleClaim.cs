using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class AspNetRoleClaim : IdentityRoleClaim<Guid>
    {
        //public int Id { get; set; }
        //public string RoleId { get; set; }
        //public string ClaimType { get; set; }
        //public string ClaimValue { get; set; }

        public virtual AspNetRole Role { get; set; }
    }
}
