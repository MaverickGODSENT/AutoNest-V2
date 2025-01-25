using AutoNest.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoNest.Data.Entities
{
    public class Part : BaseDeletableModel<string>
    {
        public Part()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CompatibleCars = new HashSet<Car>();
        }


        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public int SalesCount { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [ForeignKey(nameof(Category))]
        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual ICollection<Car> CompatibleCars { get; set; }
    }
}
