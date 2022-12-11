using System;
using System.Collections.Generic;

namespace CollabClothing.ViewModels.Catalog.Cart
{
    public class CheckoutRequest
    {
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public string? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusOrder { get; set; }
        public List<OrderDetailsViewModel> OrderDetails { get; set; } = new List<OrderDetailsViewModel>();
    }
}
