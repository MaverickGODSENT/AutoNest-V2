using Microsoft.AspNetCore.Identity;

namespace AutoNest.Web.Areas.Admin.Services
{
    public class AdminService:IAdminService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task EnsureAdminRoleExists()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }
    }
}
