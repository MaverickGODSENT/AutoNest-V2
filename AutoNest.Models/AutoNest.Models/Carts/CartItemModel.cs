namespace AutoNest.Models.Carts
{
    public class CartItemModel
    {
        public string PartId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
