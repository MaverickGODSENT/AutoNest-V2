using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Categories;
using AutoNest.Services.Categories;
using Moq;

namespace AutoNest.UnitTests.ServiceTests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<IDeletableEntityRepository<Category>> _categoryRepositoryMock;
        private ICategoryService _categoryService;

        [SetUp]
        public void SetUp()
        {
            _categoryRepositoryMock = new Mock<IDeletableEntityRepository<Category>>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object);
        }

        [Test]
        public void GetAll_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = "1", Name = "Engine Parts", Description = "All engine-related parts" },
                new Category { Id = "2", Name = "Suspension", Description = "Suspension components" }
            };

            _categoryRepositoryMock.Setup(repo => repo.AllAsNoTracking()).Returns(categories.AsQueryable());

            // Act
            var result = _categoryService.GetAll().ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Engine Parts", result[0].Name);
        }

        [Test]
        public async Task AddCategoryAsync_ShouldAddCategory()
        {
            // Arrange
            var newCategory = new CategoryAddViewModel { Name = "Brakes", Description = "Brake components" };

            // Act
            await _categoryService.AddCategoryAsync(newCategory);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Category>(c =>
                c.Name == "Brakes" && c.Description == "Brake components")), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategoryAsync_ShouldUpdateExistingCategory()
        {
            // Arrange
            var categoryViewModel = new CategoryViewModel { Id = "1", Name = "Updated Name", Description = "Updated Description" };

            // Act
            await _categoryService.UpdateCategoryAsync(categoryViewModel);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.Update(It.Is<Category>(c =>
                c.Id == "1" && c.Name == "Updated Name" && c.Description == "Updated Description")), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCategoryAsync_ShouldReturnTrue_WhenCategoryExists()
        {
            // Arrange
            var category = new Category { Id = "1", Name = "Tires", Description = "All tire products" };
            _categoryRepositoryMock.Setup(repo => repo.All()).Returns(new List<Category> { category }.AsQueryable());

            // Act
            var result = await _categoryService.DeleteCategoryAsync("1");

            // Assert
            Assert.IsTrue(result);
            _categoryRepositoryMock.Verify(repo => repo.Delete(It.Is<Category>(c => c.Id == "1")), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCategoryAsync_ShouldReturnFalse_WhenCategoryDoesNotExist()
        {
            // Arrange
            _categoryRepositoryMock.Setup(repo => repo.All()).Returns(new List<Category>().AsQueryable());

            // Act
            var result = await _categoryService.DeleteCategoryAsync("99");

            // Assert
            Assert.IsFalse(result);
            _categoryRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Category>()), Times.Never);
            _categoryRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}
