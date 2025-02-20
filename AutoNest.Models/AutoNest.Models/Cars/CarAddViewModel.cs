using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AutoNest.Models.Cars
{

    public class AddCarViewModel
    {
        
        [Required]
        [StringLength(50, ErrorMessage = "Brand name must be at most 50 characters.")]
        public string Brand { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Model name must be at most 50 characters.")]
        public string Model { get; set; }

        [Required]
        [Range(1886, 2100, ErrorMessage = "Model year must be valid.")]
        public int ModelYear { get; set; }

        public List<string> SelectedEngineIds { get; set; } = new List<string>();
        public List<string> SelectedPartIds { get; set; } = new List<string>();    

        public IEnumerable<SelectListItem> AvailableEngines { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AvailableParts { get; set; } = new List<SelectListItem>();
    }

}
