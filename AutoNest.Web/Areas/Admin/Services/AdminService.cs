using Microsoft.AspNetCore.Identity;

namespace AutoNest.Web.Areas.Admin.Services
{
    public class AdminService : IAdminService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task EnsureAdminRoleExists()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }

        public List<IdentityRole> GetAllRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<IdentityRole> GetRoleById(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public List<IdentityUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<IdentityUser> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task UpdateUser(IdentityUser user)
        {
            await _userManager.UpdateAsync(user);
        }

    }
}
