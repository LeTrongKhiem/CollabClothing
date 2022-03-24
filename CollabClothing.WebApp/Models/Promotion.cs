using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("Promotion")]
    public partial class Promotion
    {
        public Promotion()
        {
            PromotionDetails = new HashSet<PromotionDetail>();
        }

        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string NamePromotion { get; set; }
        [Required]
        [StringLength(255)]
        public string ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("Promotions")]
        public virtual Product Product { get; set; }
        [InverseProperty(nameof(PromotionDetail.Promotion))]
        public virtual ICollection<PromotionDetail> PromotionDetails { get; set; }
    }
}
