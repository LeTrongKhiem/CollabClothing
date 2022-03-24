using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("BannerType")]
    public partial class BannerType
    {
        public BannerType()
        {
            Banners = new HashSet<Banner>();
        }

        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string NameBannerType { get; set; }

        [InverseProperty(nameof(Banner.TypeBanner))]
        public virtual ICollection<Banner> Banners { get; set; }
    }
}
