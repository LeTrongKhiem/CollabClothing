using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class Promotion
    {
        public Promotion()
        {
        }

        public string Id { get; set; }

        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual PromotionDetail PromotionDetail { get; set; }
        //public virtual ICollection<PromotionDetail> PromotionDetails { get; set; }
    }
}
