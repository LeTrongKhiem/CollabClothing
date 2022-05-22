using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Models
{
    public class HomeViewModel
    {
        public List<BannerViewModel> ListBanner { get; set; }
        public List<ProductViewModel> ListProductFeatured { get; set; }
    }
}
