using AutoNest.Data.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoNest.Data.Entities
{
    public class Payment:BaseDeletableModel<string>
    {
        public Payment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [ForeignKey(nameof(IdentityUser))]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        [ForeignKey(nameof(Order))]
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public decimal Amount { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public string Status { get; set; }
    }
    public enum PaymentMethod
    {
        CreditCard,
        PayPal,
        BankTransfer,
        Crypto
    }
}
