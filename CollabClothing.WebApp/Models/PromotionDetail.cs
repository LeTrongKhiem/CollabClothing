using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class PromotionDetail
    {
        public string Id { get; set; }
        public string PromotionId { get; set; }
        public bool OnlinePromotion { get; set; }
        public string Info { get; set; }
        public bool More { get; set; }

        public virtual Promotion Promotion { get; set; }
    }
}
