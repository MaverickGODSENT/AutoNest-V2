using AutoNest.Data.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AutoNest.Data.Entities
{
    public class Car : BaseDeletableModel<string>
    {
        public Car()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CompatibleEngines = new HashSet<Engine>();
            this.CompatibleParts = new HashSet<Part>();
        }

        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }
        [Required]
        public int ModelYear { get; set; }
        [Required]
        public virtual ICollection<Engine> CompatibleEngines { get; set; }
        [Required]
        public virtual ICollection<Part> CompatibleParts { get; set; }
    }
}