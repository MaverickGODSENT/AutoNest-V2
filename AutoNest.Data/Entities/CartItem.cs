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

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; } 

        [ForeignKey(nameof(Part))]
        public int ProductId { get; set; } 

        [Required]
        public virtual Part Part { get; set; } 

        [Required]
        public int Quantity { get; set; } 

        [Required]
        public decimal Price { get; set; } 

    }
}
