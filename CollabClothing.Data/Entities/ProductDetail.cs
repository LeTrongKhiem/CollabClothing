﻿using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class ProductDetail
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Details { get; set; }

        public virtual Product Product { get; set; }
    }
}
