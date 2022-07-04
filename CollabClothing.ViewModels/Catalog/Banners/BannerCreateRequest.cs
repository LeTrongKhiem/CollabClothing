using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Banners
{
    public class BannerCreateRequest
    {
        public string NameBanner { get; set; }
        public IFormFile Images { get; set; }
        public string Alt { get; set; }
        public string TypeBannerId { get; set; }
        public string Text { get; set; }
    }
}
