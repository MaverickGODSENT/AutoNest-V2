using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Home;

namespace AutoNest.Services.Contacts
{
    public class ContactService : IContactService
    {
        private readonly IDeletableEntityRepository<Contact> _contactEntityRepository;
        public ContactService(IDeletableEntityRepository<Contact> contactEntityRepository)
        {
            _contactEntityRepository = contactEntityRepository;
        }

        public List<ContactFormModel> GetAllContactForms()
        {
            return _contactEntityRepository.All().Select(c => new ContactFormModel
            {
                Id = c.Id,
                Email = c.Email,
                Subject = c.Subject,
                Message = c.Message,
                Name = c.Name,
            }).ToList();
        }

        public ContactFormModel GetContactFormById(string id)
        {
            var contact = _contactEntityRepository.All().FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return null;
            }
            return new ContactFormModel
            {
                Id = contact.Id,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
                Name = contact.Name,
            };
        }

        public async Task Add(ContactFormModel contact)
        {
            var newContact = new Contact
            {
                Name = contact.Name,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
            };
            await _contactEntityRepository.AddAsync(newContact);
            await _contactEntityRepository.SaveChangesAsync();
        }
        public async Task Delete(string id)
        {
            var contact = _contactEntityRepository.All().FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return;
            }
            _contactEntityRepository.Delete(contact);
            await _contactEntityRepository.SaveChangesAsync();
        }
    }
}
