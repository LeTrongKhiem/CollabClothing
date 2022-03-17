using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class SystemActivity
    {
        public string Id { get; set; }
        public string ActionName { get; set; }
        public DateTime ActionDate { get; set; }
        public string FunctionId { get; set; }
        public int UserId { get; set; }
        public string ClientIp { get; set; }

        public virtual Function Function { get; set; }
        public virtual User User { get; set; }
    }
}
