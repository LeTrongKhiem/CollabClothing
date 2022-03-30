using System.Collections.Generic;
using CollabClothing.ViewModels.Common;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class ProductMapCategoryRequest
    {
        public string ProductId { get; set; }
        public List<SelectItem> Categories { get; set; } = new List<SelectItem>();
    }
}