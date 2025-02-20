using System.ComponentModel.DataAnnotations;

namespace AutoNest.Web.Areas.Admin.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

}
