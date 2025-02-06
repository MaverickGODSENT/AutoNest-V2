using AutoNest.Data.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoNest.Data.Entities
{
    public class Order : BaseDeletableModel<string>
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrderItems = new HashSet<Cart>();
            CreatedAt = DateTime.UtcNow;
            OrderStatus = OrderStatus.Pending;
        }

        [Required]
        public string OrderNumber { get; set; } = Guid.NewGuid().ToString("N")[..10].ToUpper();

        [ForeignKey(nameof(IdentityUser))]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        [ForeignKey(nameof(Cart))]
        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }


        [Required]
        [MaxLength(100)]
        public string ShippingAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string ShippingCity { get; set; }

        [Required]
        [MaxLength(50)]
        public string ShippingState { get; set; }

        [Required]
        [MaxLength(10)]
        public string ShippingZipCode { get; set; }


        public ICollection<Cart> OrderItems { get; set; }
    }
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Returned
    }
}
