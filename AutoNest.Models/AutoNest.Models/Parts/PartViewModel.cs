namespace AutoNest.Models.Parts
{
    public class PartViewModel
    {
        public string Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public int SalesCount { get; set; }
        public float Quantity { get; set; }
        public bool InStock { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
        public string CarId { get; set; }
        public string RemoteImageUrl { get; set; }
    }
}
