using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.Data.Entities
{
    [Table("sysdiagrams")]
    [Index(nameof(PrincipalId), nameof(Name), Name = "UK_principal_name", IsUnique = true)]
    public partial class Sysdiagram
    {
        [Required]
        [Column("name")]
        [StringLength(128)]
        public string Name { get; set; }
        [Column("principal_id")]
        public int PrincipalId { get; set; }
        [Key]
        [Column("diagram_id")]
        public int DiagramId { get; set; }
        [Column("version")]
        public int? Version { get; set; }
        [Column("definition")]
        public byte[] Definition { get; set; }
    }
}
