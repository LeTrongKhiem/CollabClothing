using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class PromotionDetails
    {
        public string Id { get; set; }
        public Promotion PromotionId { get; set; }
        public bool OnlinePromotion { get; set; }
        public string Info { get; set; }
        public bool More { get; set; }
    }
}
