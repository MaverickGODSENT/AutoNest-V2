using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNest.Models.Engines
{
    public class EngineAddViewModel
    {
        [Required(ErrorMessage = "Engine code is required.")]
        [StringLength(50, ErrorMessage = "Engine code cannot exceed 50 characters.")]
        public string EngineCode { get; set; }

        [Required(ErrorMessage = "Engine displacement is required.")]
        [Range(0.5, 10.0, ErrorMessage = "Engine displacement must be between 0.5 and 10.0 liters.")]
        public float EngineDisplacement { get; set; }

        [Required(ErrorMessage = "Engine horsepower is required.")]
        [Range(50, 2000, ErrorMessage = "Engine horsepower must be between 50 and 2000.")]
        public float EngineHorsePower { get; set; }

        [Required(ErrorMessage = "Transmission type is required.")]
        [StringLength(50, ErrorMessage = "Transmission type cannot exceed 50 characters.")]
        public string Transmission { get; set; }

        [Required(ErrorMessage = "Drivetrain type is required.")]
        [StringLength(50, ErrorMessage = "Drivetrain type cannot exceed 50 characters.")]
        public string Drivetrain { get; set; }

    }
}
