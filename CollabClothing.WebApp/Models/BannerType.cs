using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class BannerType
    {
        public BannerType()
        {
            Banners = new HashSet<Banner>();
        }

        public string Id { get; set; }
        public string NameBannerType { get; set; }

        public virtual ICollection<Banner> Banners { get; set; }
    }
}
