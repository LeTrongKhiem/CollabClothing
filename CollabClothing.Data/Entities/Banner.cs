using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class Banner
    {
        public string Id { get; set; }
        public string NameBanner { get; set; }
        public string Image { get; set; }
        public string Alt { get; set; }
        public BannerType TypeBannerId { get; set; }
        public string Text { get; set; }
    }
}
