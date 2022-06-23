using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class PromotionDetail
    {
        public PromotionDetail()
        {
            Promotions = new HashSet<Promotion>();
        }
        public string Id { get; set; }
        public string NamePromotion { get; set; }
        public bool OnlinePromotion { get; set; }
        public string Info { get; set; }
        public bool More { get; set; }

        //public virtual Promotion Promotion { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }
    }
}
