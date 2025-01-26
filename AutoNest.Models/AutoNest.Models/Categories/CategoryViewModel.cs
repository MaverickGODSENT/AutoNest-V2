using AutoNest.Data.Entities;

namespace AutoNest.Models.Categories
{
    public class CategoryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<Part>? Parts { get; set; }
    }
}
