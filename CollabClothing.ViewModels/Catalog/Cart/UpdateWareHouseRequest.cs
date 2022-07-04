using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Cart
{
    public class UpdateWareHouseRequest
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
