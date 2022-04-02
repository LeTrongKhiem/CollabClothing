using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhoneNumber { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
