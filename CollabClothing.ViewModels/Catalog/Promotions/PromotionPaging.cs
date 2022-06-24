using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Promotions
{
    public class PromotionPaging : PagedResultBase
    {
        public string Keyword { get; set; }
        public bool More { get; set; }
        public bool Online { get; set; }
    }
}
