using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Parts;
using AutoNest.Services.Parts;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNest.UnitTests.ServiceTests
{
    [TestFixture]
    public class PartServiceTests
    {
        private Mock<IDeletableEntityRepository<Part>> _partRepoMock;
        private Mock<IDeletableEntityRepository<Category>> _categoryRepoMock;
        private Mock<IRepository<Image>> _imageRepoMock;
        private IPartService _partService;

        [SetUp]
        public void Setup()
        {
            _partRepoMock = new Mock<IDeletableEntityRepository<Part>>();
            _categoryRepoMock = new Mock<IDeletableEntityRepository<Category>>();
            _imageRepoMock = new Mock<IRepository<Image>>();

            _partService = new PartService(
                _partRepoMock.Object,
                _categoryRepoMock.Object,
                _imageRepoMock.Object
            );
        }

        [Test]
        public async Task AddPartAsync_ShouldAddPartAndImage_WhenValidInput()
        {
            // Arrange
            var model = new PartAddViewModel
            {
                Brand = "BMW",
                Model = "E46",
                Description = "Test part",
                CategoryId = "1",
                Quantity = 5,
                Price = 100,
                Image = GetMockImage("part.jpg")
            };

            string imagePath = "test_path";

            _partRepoMock.Setup(r => r.AddAsync(It.IsAny<Part>())).Returns(Task.CompletedTask);
            _partRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            _imageRepoMock.Setup(r => r.AddAsync(It.IsAny<Image>())).Returns(Task.CompletedTask);
            _imageRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _partService.AddPartAsync(model, imagePath));
        }

        [Test]
        public async Task DeletePartAsync_ShouldReturnTrue_WhenPartExists()
        {
            var parts = new List<Part> { new Part { Id = "1" } }.AsQueryable();

            _partRepoMock.Setup(r => r.All()).Returns(parts);
            _partRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _partService.DeletePartAsync("1");

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeletePartAsync_ShouldReturnFalse_WhenPartDoesNotExist()
        {
            var parts = new List<Part>().AsQueryable();

            _partRepoMock.Setup(r => r.All()).Returns(parts);

            var result = await _partService.DeletePartAsync("1");

            Assert.IsFalse(result);
        }

        [Test]
        public void GetAll_ShouldReturnAllParts()
        {
            var parts = new List<Part>
            {
                new Part { Id = "1", Brand = "BMW", Model = "X5", Description = "Engine", CategoryId = "1", Price = 100, Quantity = 3 }
            }.AsQueryable();

            var categories = new List<Category>
            {
                new Category { Id = "1", Name = "Engine Parts" }
            }.AsQueryable();

            var images = new List<Image>
            {
                new Image { PartId = "1", RemoteImageUrl = "/img/1.jpg" }
            }.AsQueryable();

            _partRepoMock.Setup(r => r.AllAsNoTracking()).Returns(parts);
            _categoryRepoMock.Setup(r => r.All()).Returns(categories);
            _imageRepoMock.Setup(r => r.All()).Returns(images);

            var result = _partService.GetAll().ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("BMW", result[0].Brand);
        }

        [Test]
        public void GetPartsForCar_ShouldReturnPartsWithMatchingCarId()
        {
            var parts = new List<Part>
            {
                new Part { Id = "1", Brand = "BMW", Model = "X5", Description = "Engine", CategoryId = "1", Price = 100, Quantity = 3 }
            }.AsQueryable();

            var categories = new List<Category>
            {
                new Category { Id = "1", Name = "Engine Parts" }
            }.AsQueryable();

            var images = new List<Image>
            {
                new Image { PartId = "1", RemoteImageUrl = "/img/1.jpg" }
            }.AsQueryable();

            _partRepoMock.Setup(r => r.All()).Returns(parts);
            _categoryRepoMock.Setup(r => r.All()).Returns(categories);
            _imageRepoMock.Setup(r => r.All()).Returns(images);

            var result = _partService.GetPartsForCar("car123").ToList();

            Assert.AreEqual("car123", result[0].CarId);
        }

        [Test]
        public async Task UpdatePartAsync_ShouldUpdateAndSavePart()
        {
            var model = new PartViewModel
            {
                Id = "1",
                Brand = "Audi",
                Model = "A4",
                Description = "Updated",
                Quantity = 10,
                Price = 150
            };

            _partRepoMock.Setup(r => r.Update(It.IsAny<Part>()));
            _partRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            await _partService.UpdatePartAsync(model);

            _partRepoMock.Verify(r => r.Update(It.IsAny<Part>()), Times.Once);
            _partRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        private IFormFile GetMockImage(string fileName)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("dummy image content");
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "Image", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }
    }
}
