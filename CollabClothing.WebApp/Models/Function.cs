using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class Function
    {
        public Function()
        {
            SystemActivities = new HashSet<SystemActivity>();
        }

        public string Id { get; set; }
        public string NameFunction { get; set; }
        public string Url { get; set; }
        public string ParentId { get; set; }
        public bool Status { get; set; }
        public string SortOrder { get; set; }

        public virtual ICollection<SystemActivity> SystemActivities { get; set; }
    }
}
