using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Promotions
{
    public class PromotionDetailsCreateRequest
    {
        public string Id { get; set; }
        public string PromotionId { get; set; }
        public bool OnlinePromotion { get; set; }
        public string Info { get; set; }
        public bool More { get; set; }
    }
}
