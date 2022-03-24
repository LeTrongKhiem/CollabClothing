using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Keyless]
    [Table("Permission")]
    public partial class Permission
    {
        [Required]
        [StringLength(255)]
        public string RoleId { get; set; }
        [Required]
        [StringLength(255)]
        public string FunctionId { get; set; }
        [Required]
        [StringLength(255)]
        public string ActionId { get; set; }

        [ForeignKey(nameof(ActionId))]
        public virtual Action Action { get; set; }
        [ForeignKey(nameof(FunctionId))]
        public virtual Function Function { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }
    }
}
