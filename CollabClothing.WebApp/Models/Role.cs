using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class Role
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string NameRole { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
    }
}
