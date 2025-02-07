using AutoNest.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AutoNest.Models.Parts
{
    public class PartAddViewModel
    {
        [Required(ErrorMessage = "Brand is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Brand must be between 2 and 100 characters")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Model must be between 2 and 100 characters")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between $0.01 and $999,999.99")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, 10000, ErrorMessage = "Quantity must be between 0 and 10,000")]
        public float Quantity { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public string CategoryId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Categories { get; set; }
        public IFormFile Image { get; set; }
    }
}
