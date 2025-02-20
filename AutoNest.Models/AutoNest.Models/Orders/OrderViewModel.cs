using AutoNest.Models.Carts;

namespace AutoNest.Models.Orders
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string CartId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZipCode { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; }
        public List<CartItemModel> CartItems { get; set; }
    }
}
