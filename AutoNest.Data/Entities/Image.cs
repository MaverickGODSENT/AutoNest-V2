using AutoNest.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoNest.Data.Entities
{
    public class Image : BaseModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [ForeignKey(nameof(Part))]
        public string PartId { get; set; }

        public virtual Part Part { get; set; }
        [Required]
        public string Extension { get; set; }
        [Required]
        public string? RemoteImageUrl { get; set; }

    }
}
