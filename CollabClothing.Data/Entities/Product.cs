using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public decimal PriceCur { get; set; }
        public decimal PriceOld { get; set; }
        public int SaleOff { get; set; }
        public string Descript { get; set; }
        public string BrandId { get; set; }
        public bool SoldOut { get; set; }
        public int Installment { get; set; }

    }
}
