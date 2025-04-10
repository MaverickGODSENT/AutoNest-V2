using AutoNest.Models.Engines;
using AutoNest.Models.Parts;
using System.ComponentModel.DataAnnotations;

namespace AutoNest.Models.Cars
{
    public class CarInputModel
    {
        public string? Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Brand name cannot exceed 50 characters.")]
        public string Brand { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Model name cannot exceed 50 characters.")]
        public string Model { get; set; }

        [Required]
        [Range(1886, 2100, ErrorMessage = "Enter a valid model year.")]
        public int ModelYear { get; set; }

        public List<string>? CompatibleEngineIds { get; set; } = new List<string>();
        public List<string>? CompatiblePartIds { get; set; } = new List<string>();

        public List<EngineViewModel> AllEngines { get; set; } = new List<EngineViewModel>();
        public List<PartViewModel> AllParts { get; set; } = new List<PartViewModel>();
    }
}
