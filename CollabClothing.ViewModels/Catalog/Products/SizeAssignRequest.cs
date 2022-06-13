using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class SizeAssignRequest
    {
        public string Id { get; set; }
        public List<SelectItem> Sizes { get; set; } = new List<SelectItem>();
    }
}
