using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class PromotionAssignRequest
    {
        public string Id { get; set; }
        public List<SelectItem> Promotions { get; set; } = new List<SelectItem>();
    }
}
