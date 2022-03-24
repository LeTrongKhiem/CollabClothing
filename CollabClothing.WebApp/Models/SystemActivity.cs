using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("SystemActivity")]
    public partial class SystemActivity
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string ActionName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ActionDate { get; set; }
        [Required]
        [StringLength(255)]
        public string FunctionId { get; set; }
        public int UserId { get; set; }
        [Column("ClientIP")]
        [StringLength(255)]
        public string ClientIp { get; set; }

        [ForeignKey(nameof(FunctionId))]
        [InverseProperty("SystemActivities")]
        public virtual Function Function { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("SystemActivities")]
        public virtual User User { get; set; }
    }
}
