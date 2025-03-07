namespace AutoNest.Web.Areas.Admin.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class DetailedUserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public List<IdentityRole> Roles { get; set; }
        public string CreatedDate { get; set; }
    }

}
