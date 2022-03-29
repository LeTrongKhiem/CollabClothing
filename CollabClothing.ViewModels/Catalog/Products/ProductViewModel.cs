using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public decimal PriceCurrent { get; set; }
        public decimal PriceOld { get; set; }
        public int? SaleOff { get; set; }
        public string BrandId { get; set; }
        public bool SoldOut { get; set; }
        public int? Installment { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string Details { get; set; }
        // public string CategoryName { get; set; }
        public string ThumbnailImage { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
}
