using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Home;
using AutoNest.Services.Contacts;
using Moq;

namespace AutoNest.UnitTests.ServiceTests
{
    [TestFixture]
    public class ContactServiceTests
    {
        private Mock<IDeletableEntityRepository<Contact>> _contactRepositoryMock;
        private IContactService _contactService;

        [SetUp]
        public void SetUp()
        {
            _contactRepositoryMock = new Mock<IDeletableEntityRepository<Contact>>();
            _contactService = new ContactService(_contactRepositoryMock.Object);
        }

        [Test]
        public void GetAllContactForms_ShouldReturnAllContacts()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { Id = "1", Name = "John Doe", Email = "john@example.com", Subject = "Subject 1", Message = "Message 1" },
                new Contact { Id = "2", Name = "Jane Doe", Email = "jane@example.com", Subject = "Subject 2", Message = "Message 2" }
            };

            _contactRepositoryMock.Setup(repo => repo.All()).Returns(contacts.AsQueryable());

            // Act
            var result = _contactService.GetAllContactForms();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("John Doe", result[0].Name);
        }

        [Test]
        public void GetContactFormById_ShouldReturnCorrectContact_WhenExists()
        {
            // Arrange
            var contact = new Contact { Id = "1", Name = "John Doe", Email = "john@example.com", Subject = "Subject 1", Message = "Message 1" };

            _contactRepositoryMock.Setup(repo => repo.All()).Returns(new List<Contact> { contact }.AsQueryable());

            // Act
            var result = _contactService.GetContactFormById("1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.Name);
            Assert.AreEqual("john@example.com", result.Email);
        }

        [Test]
        public void GetContactFormById_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            _contactRepositoryMock.Setup(repo => repo.All()).Returns(new List<Contact>().AsQueryable());

            // Act
            var result = _contactService.GetContactFormById("99");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Add_ShouldAddContact()
        {
            // Arrange
            var newContact = new ContactFormModel
            {
                Name = "New User",
                Email = "new@example.com",
                Subject = "New Subject",
                Message = "New Message"
            };

            // Act
            await _contactService.Add(newContact);

            // Assert
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Contact>(c =>
                c.Name == "New User" && c.Email == "new@example.com" &&
                c.Subject == "New Subject" && c.Message == "New Message")), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldRemoveContact_WhenExists()
        {
            // Arrange
            var contact = new Contact { Id = "1", Name = "Delete Me" };

            _contactRepositoryMock.Setup(repo => repo.All()).Returns(new List<Contact> { contact }.AsQueryable());

            // Act
            await _contactService.Delete("1");

            // Assert
            _contactRepositoryMock.Verify(repo => repo.Delete(It.Is<Contact>(c => c.Id == "1")), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldDoNothing_WhenContactDoesNotExist()
        {
            // Arrange
            _contactRepositoryMock.Setup(repo => repo.All()).Returns(new List<Contact>().AsQueryable());

            // Act
            await _contactService.Delete("99");

            // Assert
            _contactRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Contact>()), Times.Never);
            _contactRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}
