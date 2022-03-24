using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class Function
    {
        public Function()
        {
            SystemActivities = new HashSet<SystemActivity>();
        }

        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string NameFunction { get; set; }
        [Required]
        [StringLength(255)]
        public string Url { get; set; }
        [Required]
        [StringLength(255)]
        public string ParentId { get; set; }
        public bool Status { get; set; }
        [Required]
        [StringLength(255)]
        public string SortOrder { get; set; }

        [InverseProperty(nameof(SystemActivity.Function))]
        public virtual ICollection<SystemActivity> SystemActivities { get; set; }
    }
}
