using AutoNest.Services.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminContactController : Controller
    {
        private readonly IContactService _contactService;
        public AdminContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            var contacts = _contactService.GetAllContactForms();
            return View(contacts);
        }
        public IActionResult Detailed(string id)
        {
            var contact = _contactService.GetContactFormById(id);
            return View(contact);
        }
        public IActionResult Delete(string id)
        {
            _contactService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
