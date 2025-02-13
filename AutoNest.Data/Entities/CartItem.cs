using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoNest.Data.Common.Models;

namespace AutoNest.Data.Entities
{
    public class CartItem:BaseDeletableModel<string>
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
