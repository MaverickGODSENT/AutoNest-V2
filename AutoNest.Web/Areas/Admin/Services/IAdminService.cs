using Microsoft.AspNetCore.Identity;

namespace AutoNest.Web.Areas.Admin.Services
{
    public interface IAdminService
    {
        public Task EnsureAdminRoleExists();
        public List<IdentityUser> GetAllUsers();
        public List<IdentityRole> GetAllRoles();
        public Task<IdentityUser> GetUserById(string userId);
        public Task<IdentityRole> GetRoleById(string roleId);
        public Task DeleteUser(string userId);
        public Task UpdateUser(IdentityUser user);
    }
}
