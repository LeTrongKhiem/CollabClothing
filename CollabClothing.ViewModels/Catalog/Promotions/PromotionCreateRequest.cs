using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Promotions
{
    public class PromotionCreateRequest
    {
        public string Id { get; set; }
        public string NamePromotion { get; set; }
        public bool OnlinePromotion { get; set; }
        public string Info { get; set; }
        public bool More { get; set; }
        public List<string> ListProduct { get; set; }
    }
}
