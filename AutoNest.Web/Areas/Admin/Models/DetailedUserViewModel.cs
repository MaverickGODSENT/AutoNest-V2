namespace AutoNest.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    public class DetailedUserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public string CreatedDate { get; set; }
    }

}
