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
        public List<string> ListImages { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public string CategoryName { get; set; }
        public string BrandName { get; set; }

        public bool Consumer { get; set; }
        public string Type { get; set; }
        public string Form { get; set; }
        public bool Cotton { get; set; }
        public string MadeIn { get; set; }
        public List<string> Sizes { get; set; } = new List<string>();
    }
}
