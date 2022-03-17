using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class Permission
    {
        public string RoleId { get; set; }
        public string FunctionId { get; set; }
        public string ActionId { get; set; }

        public virtual Action Action { get; set; }
        public virtual Function Function { get; set; }
        public virtual Role Role { get; set; }
    }
}
