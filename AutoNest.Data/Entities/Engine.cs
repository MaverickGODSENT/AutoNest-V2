using AutoNest.Data.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AutoNest.Data.Entities
{
    public class Engine : BaseDeletableModel<string>
    {
        public Engine()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CompatibleCars = new HashSet<Car>();
        }

        [Required]
        public string EngineCode { get; set; }
        [Required]
        public float EngineDisplacement { get; set; }
        [Required]
        public float EngineHorsePower { get; set; }
        [Required]
        public string Transmission { get; set; }
        [Required]
        public string Drivetrain { get; set; }
        public virtual ICollection<Car> CompatibleCars { get; set; }
    }
}
