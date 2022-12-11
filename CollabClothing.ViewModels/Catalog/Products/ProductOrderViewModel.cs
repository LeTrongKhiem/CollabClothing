using System;
namespace CollabClothing.ViewModels.Catalog.Products
{
    public class ProductOrderViewModel
    {
        public string OrderDetailId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceTotal { get; set; }
        public decimal SinglePrice { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string BrandName { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhoneNumber { get; set; }
        public Guid UserId { get; set; }
        public bool Status { get; set; }
        public string PathImage { get; set; }
        public int StatusOrder { get; set; }
    }
}