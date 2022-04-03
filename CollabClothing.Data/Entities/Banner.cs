using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class Banner
    {
        public string Id { get; set; }
        public string NameBanner { get; set; }
        public string Images { get; set; }
        public string Alt { get; set; }
        public string TypeBannerId { get; set; }
        public string Text { get; set; }

        public virtual BannerType TypeBanner { get; set; }
    }
}
