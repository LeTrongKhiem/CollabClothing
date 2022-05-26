using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Models
{
    public class HomeViewModel
    {
        public List<BannerViewModel> ListBanner { get; set; }
        public List<ProductViewModel> ListProductFeatured { get; set; }
        public string ParseToVND(decimal price)
        {
            string a = string.Format("{ 0:0,0 vnđ}", price);
            var current = 100;
            var format = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            String.Format(format, "{0:c0}", current);
            return a;
        }
    }
}
