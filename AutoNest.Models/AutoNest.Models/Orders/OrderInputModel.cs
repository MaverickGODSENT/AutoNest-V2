using AutoNest.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace AutoNest.Models.Orders
{

    public class OrderInputModel
    {
        public string OrderId { get; set; }
        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? CartId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Subtotal must be a positive value.")]
        public decimal SubTotal { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Shipping cost must be a positive value.")]
        public decimal ShippingCost { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be a positive value.")]
        public decimal TotalAmount { get; set; }

        [Required, StringLength(255, ErrorMessage = "Shipping address must be within 255 characters.")]
        public string? ShippingAddress { get; set; }

        [Required, StringLength(100, ErrorMessage = "City name must be within 100 characters.")]
        public string? ShippingCity { get; set; }

        [Required, StringLength(50, ErrorMessage = "State name must be within 50 characters.")]
        public string? ShippingState { get; set; }

        [Required, RegularExpression(@"^\d{4}$", ErrorMessage = "Enter a valid Bulgarian ZIP code (4 digits).")]
        public string? ShippingZipCode { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentMethod), ErrorMessage = "Invalid payment method.")]
        public PaymentMethod PaymentMethod { get; set; }


        public List<CartItem>? CartItems { get; set; }
    }

    public enum PaymentMethod
    {
        CreditCard,
    }

}
