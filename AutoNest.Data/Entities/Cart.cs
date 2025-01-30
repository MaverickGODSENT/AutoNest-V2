using AutoNest.Data.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoNest.Data.Entities
{
    public class Cart : BaseDeletableModel<string>
    {
        public Cart()
        {
            this.Parts = new HashSet<CartItem>();
        }
        [Required]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }

        public virtual ICollection<CartItem> Parts { get; set; }
    }
}
