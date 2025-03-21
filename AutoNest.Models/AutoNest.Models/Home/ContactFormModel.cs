using System.ComponentModel.DataAnnotations;

namespace AutoNest.Models.Home
{
    public class ContactFormModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
