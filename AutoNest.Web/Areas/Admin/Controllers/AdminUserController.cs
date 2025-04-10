using AutoNest.Web.Areas.Admin.Models;
using AutoNest.Web.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminUserController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            var users = _adminService.GetAllUsers();

            return View(users);
        }
        public async Task<IActionResult> DetailsAsync(string userId)
        {
            var user = await _adminService.GetUserById(userId);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _adminService.GetUserById(userId);

            EditUserViewModel viewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await _adminService.GetUserById(model.Id);
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            await _adminService.UpdateUser(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delet(string userId)
        {
            await _adminService.DeleteUser(userId);
            return RedirectToAction("Index");
        }
    }
}
