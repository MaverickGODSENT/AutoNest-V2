using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Cars;
using AutoNest.Services.Cars;
using Moq;

namespace AutoNest.UnitTests.ServiceTests
{
    [TestFixture]
    public class CarServiceTests
    {
        private Mock<IDeletableEntityRepository<Car>> _carRepositoryMock;
        private Mock<IDeletableEntityRepository<Engine>> _engineRepositoryMock;
        private Mock<IDeletableEntityRepository<Part>> _partRepositoryMock;
        private ICarService _carService;

        [SetUp]
        public void SetUp()
        {
            _carRepositoryMock = new Mock<IDeletableEntityRepository<Car>>();
            _engineRepositoryMock = new Mock<IDeletableEntityRepository<Engine>>();
            _partRepositoryMock = new Mock<IDeletableEntityRepository<Part>>();

            _carService = new CarService(_carRepositoryMock.Object, _engineRepositoryMock.Object, _partRepositoryMock.Object);
        }

        [Test]
        public void GetAll_ShouldReturnAllCars()
        {
            // Arrange
            var cars = new List<Car>
            {
                new Car { Id = "1", Brand = "Toyota", Model = "Corolla", ModelYear = 2020 },
                new Car { Id = "2", Brand = "Honda", Model = "Civic", ModelYear = 2021 }
            };
            _carRepositoryMock.Setup(repo => repo.AllAsNoTracking()).Returns(cars.AsQueryable());

            // Act
            var result = _carService.GetAll().ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Toyota", result[0].Brand);
            Assert.AreEqual("Honda", result[1].Brand);
        }

        [Test]
        public void GetCarById_ShouldReturnCar_WhenFound()
        {
            // Arrange
            var car = new Car { Id = "1", Brand = "Toyota", Model = "Corolla", ModelYear = 2020 };
            _carRepositoryMock.Setup(repo => repo.GetById("1")).Returns(car);

            // Act
            var result = _carService.GetCarById("1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Toyota", result.Brand);
        }

        [Test]
        public void GetCarById_ShouldReturnNull_WhenCarNotFound()
        {
            // Arrange
            _carRepositoryMock.Setup(repo => repo.GetById("99")).Returns((Car)null);

            // Act
            var result = _carService.GetCarById("99");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddCarAsync_ShouldAddCarWithCompatibleEnginesAndParts()
        {
            // Arrange
            var carInput = new CarInputModel
            {
                Brand = "Ford",
                Model = "Focus",
                ModelYear = 2022,
                CompatibleEngineIds = new List<string> { "engine1" },
                CompatiblePartIds = new List<string> { "part1" }
            };

            var engine = new Engine { Id = "engine1" };
            var part = new Part { Id = "part1" };

            _engineRepositoryMock.Setup(repo => repo.GetById("engine1")).Returns(engine);
            _partRepositoryMock.Setup(repo => repo.GetById("part1")).Returns(part);

            // Act
            await _carService.AddCarAsync(carInput);

            // Assert
            _carRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Car>(c =>
                c.Brand == "Ford" &&
                c.Model == "Focus" &&
                c.ModelYear == 2022 &&
                c.CompatibleEngines.Contains(engine) &&
                c.CompatibleParts.Contains(part))), Times.Once);

            _carRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCarAsync_ShouldUpdateCar()
        {
            // Arrange
            var carViewModel = new CarInputModel
            {
                Id = "1",
                Brand = "UpdatedBrand",
                Model = "UpdatedModel",
                ModelYear = 2023,
                CompatibleEngineIds = new List<string> { "engine1" }
            };

            var engine = new Engine { Id = "engine1" };
            _engineRepositoryMock.Setup(repo => repo.GetById("engine1")).Returns(engine);

            // Act
            await _carService.UpdateCarAsync(carViewModel);

            // Assert
            _carRepositoryMock.Verify(repo => repo.Update(It.Is<Car>(c =>
                c.Id == "1" &&
                c.Brand == "UpdatedBrand" &&
                c.Model == "UpdatedModel" &&
                c.ModelYear == 2023 &&
                c.CompatibleEngines.Contains(engine))), Times.Once);

            _carRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCar_ShouldReturnTrue_WhenCarExists()
        {
            // Arrange
            var car = new Car { Id = "1", Brand = "Toyota", Model = "Corolla" };
            _carRepositoryMock.Setup(repo => repo.GetById("1")).Returns(car);

            // Act
            var result = await _carService.DeleteCar("1");

            // Assert
            Assert.IsTrue(result);
            _carRepositoryMock.Verify(repo => repo.Delete(It.Is<Car>(c => c.Id == "1")), Times.Once);
            _carRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCar_ShouldReturnFalse_WhenCarDoesNotExist()
        {
            // Arrange
            _carRepositoryMock.Setup(repo => repo.GetById("99")).Returns((Car)null);

            // Act
            var result = await _carService.DeleteCar("99");

            // Assert
            Assert.IsFalse(result);
            _carRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Car>()), Times.Never);
            _carRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}