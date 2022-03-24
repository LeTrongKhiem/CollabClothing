using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("PromotionDetail")]
    public partial class PromotionDetail
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string PromotionId { get; set; }
        public bool OnlinePromotion { get; set; }
        [StringLength(255)]
        public string Info { get; set; }
        public bool More { get; set; }

        [ForeignKey(nameof(PromotionId))]
        [InverseProperty("PromotionDetails")]
        public virtual Promotion Promotion { get; set; }
    }
}
