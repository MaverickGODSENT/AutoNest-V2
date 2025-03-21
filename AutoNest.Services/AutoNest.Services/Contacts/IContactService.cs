using AutoNest.Models.Home;

namespace AutoNest.Services.Contacts
{
    public interface IContactService
    {
        public List<ContactFormModel> GetAllContactForms();
        public ContactFormModel GetContactFormById(string id);
        public Task Add(ContactFormModel contact);
        public Task Delete(string id);
    }
}
