using AutoNest.Data.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AutoNest.Data.Entities
{
    public class Category : BaseDeletableModel<string>
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Parts = new HashSet<Part>();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ICollection<Part> Parts { get; set; }
    }
}
