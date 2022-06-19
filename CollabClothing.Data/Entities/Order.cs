using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhoneNumber { get; set; }
        public bool Status { get; set; }
        public AppUser AppUser { get; set; }
        public Guid? UserId { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
