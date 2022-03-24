using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("Banner")]
    public partial class Banner
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string NameBanner { get; set; }
        [Required]
        [StringLength(255)]
        public string Images { get; set; }
        [Required]
        [StringLength(255)]
        public string Alt { get; set; }
        [Required]
        [StringLength(255)]
        public string TypeBannerId { get; set; }
        [Required]
        [StringLength(255)]
        public string Text { get; set; }

        [ForeignKey(nameof(TypeBannerId))]
        [InverseProperty(nameof(BannerType.Banners))]
        public virtual BannerType TypeBanner { get; set; }
    }
}
