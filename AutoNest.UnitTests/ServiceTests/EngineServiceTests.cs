using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Engines;
using AutoNest.Services.Engines;
using Moq;

namespace AutoNest.UnitTests.ServiceTests
{
    [TestFixture]
    public class EngineServiceTests
    {
        private Mock<IDeletableEntityRepository<Engine>> _engineRepositoryMock;
        private IEngineService _engineService;

        [SetUp]
        public void SetUp()
        {
            _engineRepositoryMock = new Mock<IDeletableEntityRepository<Engine>>();
            _engineService = new EngineService(_engineRepositoryMock.Object);
        }

        [Test]
        public void GetAll_ShouldReturnAllEngines()
        {
            // Arrange
            var engines = new List<Engine>
            {
                new Engine { Id = "1", EngineCode = "ABC123", EngineDisplacement = 2.0f, EngineHorsePower = 200, Transmission = "Manual", Drivetrain = "FWD" },
                new Engine { Id = "2", EngineCode = "XYZ789", EngineDisplacement = 3.5f, EngineHorsePower = 300, Transmission = "Automatic", Drivetrain = "AWD" }
            };

            _engineRepositoryMock.Setup(repo => repo.AllAsNoTracking()).Returns(engines.AsQueryable());

            // Act
            var result = _engineService.GetAll().ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("ABC123", result[0].EngineCode);
        }

        [Test]
        public async Task AddEngineAsync_ShouldAddEngine()
        {
            // Arrange
            var newEngine = new EngineAddViewModel
            {
                EngineCode = "NEW123",
                EngineDisplacement = 2.5f,
                EngineHorsePower = 250,
                Transmission = "Manual",
                Drivetrain = "RWD"
            };

            // Act
            await _engineService.AddEngineAsync(newEngine);

            // Assert
            _engineRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Engine>(e =>
                e.EngineCode == "NEW123" && e.EngineDisplacement == 2.5f && e.EngineHorsePower == 250 &&
                e.Transmission == "Manual" && e.Drivetrain == "RWD")), Times.Once);
            _engineRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateEngineAsync_ShouldUpdateExistingEngine()
        {
            // Arrange
            var engineViewModel = new EngineViewModel
            {
                Id = "1",
                EngineCode = "UPDATED123",
                EngineDisplacement = 3.0f,
                EngineHorsePower = 320,
                Transmission = "Automatic",
                Drivetrain = "AWD"
            };

            // Act
            await _engineService.UpdateEngineAsync(engineViewModel);

            // Assert
            _engineRepositoryMock.Verify(repo => repo.Update(It.Is<Engine>(e =>
                e.Id == "1" && e.EngineCode == "UPDATED123" && e.EngineDisplacement == 3.0f &&
                e.EngineHorsePower == 320 && e.Transmission == "Automatic" && e.Drivetrain == "AWD")), Times.Once);
            _engineRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteEngineAsync_ShouldReturnTrue_WhenEngineExists()
        {
            // Arrange
            var engine = new Engine { Id = "1", EngineCode = "DEL123", EngineDisplacement = 2.0f, EngineHorsePower = 200 };
            _engineRepositoryMock.Setup(repo => repo.All()).Returns(new List<Engine> { engine }.AsQueryable());

            // Act
            var result = await _engineService.DeleteEngineAsync("1");

            // Assert
            Assert.IsTrue(result);
            _engineRepositoryMock.Verify(repo => repo.Delete(It.Is<Engine>(e => e.Id == "1")), Times.Once);
            _engineRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteEngineAsync_ShouldReturnFalse_WhenEngineDoesNotExist()
        {
            // Arrange
            _engineRepositoryMock.Setup(repo => repo.All()).Returns(new List<Engine>().AsQueryable());

            // Act
            var result = await _engineService.DeleteEngineAsync("99");

            // Assert
            Assert.IsFalse(result);
            _engineRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Engine>()), Times.Never);
            _engineRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}
