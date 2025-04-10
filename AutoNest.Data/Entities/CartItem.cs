using AutoNest.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoNest.Data.Entities
{
    public class CartItem : BaseDeletableModel<string>
    {
        public CartItem()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [ForeignKey("CartId")]
        public string CartId { get; set; }

        [ForeignKey(nameof(Part))]
        public string PartId { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Part Part { get; set; }
        [Required]
        public string ImageUrl { get; set; }

    }
}
