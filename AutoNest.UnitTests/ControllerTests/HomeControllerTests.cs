using AutoNest.Models.Cars;
using AutoNest.Models.Categories;
using AutoNest.Models.Home;
using AutoNest.Models.Parts;
using AutoNest.Services.Cars;
using AutoNest.Services.Carts;
using AutoNest.Services.Categories;
using AutoNest.Services.Contacts;
using AutoNest.Services.Parts;
using AutoNest.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AutoNest.UnitTests.ControllerTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<ICarService> _mockCarService;
        private Mock<ICategoryService> _mockCategoryService;
        private Mock<ICartService> _mockCartService;
        private Mock<IContactService> _mockContactService;
        private Mock<IPartService> _mockPartService;
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private HomeController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockCarService = new Mock<ICarService>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockCartService = new Mock<ICartService>();
            _mockContactService = new Mock<IContactService>();
            _mockPartService = new Mock<IPartService>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>();

            _controller = new HomeController(
                _mockCarService.Object,
                _mockCategoryService.Object,
                _mockCartService.Object,
                _mockContactService.Object,
                _mockPartService.Object,
                _mockUserManager.Object
            );
        }

        [Test]
        public async Task Index_ShouldReturnViewWithHomeViewModel()
        {
            // Arrange
            _mockCarService.Setup(x => x.GetAll()).Returns(new List<CarViewModel>());
            _mockCategoryService.Setup(x => x.GetAll()).Returns(new List<CategoryViewModel>());

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.InstanceOf<HomeViewModel>());
            var model = viewResult.Model as HomeViewModel;
            Assert.NotNull(model.Cars);
            Assert.NotNull(model.Categories);
        }

        [Test]
        public async Task SearchBarSearch_ShouldReturnSearchResultView_WhenPartsFound()
        {
            // Arrange
            var brandModel = "Toyota Camry";
            _mockPartService.Setup(x => x.GetAll()).Returns(new List<PartViewModel> { new PartViewModel() });

            // Act
            var result = _controller.SearchBarSearch(brandModel) as Task<IActionResult>;

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result.Result as ViewResult;
            Assert.AreEqual("../Part/SearchResult", viewResult.ViewName);
        }

        [Test]
        public async Task SearchBarSearch_ShouldRedirectToIndex_WhenNoPartsFound()
        {
            // Arrange
            var brandModel = "Unknown Model";
            _mockPartService.Setup(x => x.GetAll()).Returns(new List<PartViewModel>());

            // Act
            var result = _controller.SearchBarSearch(brandModel) as Task<IActionResult>;

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result.Result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }
        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }
    }
}
