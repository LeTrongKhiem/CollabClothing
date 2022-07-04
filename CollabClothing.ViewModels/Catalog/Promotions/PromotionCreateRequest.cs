﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Promotions
{
    public class PromotionCreateRequest
    {
        public string Id { get; set; }
        public string NamePromotion { get; set; }
        public string Info { get; set; }
        public bool More { get; set; }
        public bool Online { get; set; }
    }
}
