using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNest.Models.Carts
{
    public class CartModel
    {
        public CartModel()
        {
            Items = new List<CartItemModel>();
        }
        public List<CartItemModel> Items { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
