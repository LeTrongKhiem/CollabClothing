using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class AspNetUserClaim : IdentityUserClaim<Guid>
    {
        //public int Id { get; set; }
        //public string UserId { get; set; }
        //public string ClaimType { get; set; }
        //public string ClaimValue { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}
