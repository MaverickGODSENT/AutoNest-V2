using AutoNest.Data.Entities;
using AutoNest.Models.Carts;
using AutoNest.Services.Carts;
using AutoNest.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AutoNest.UnitTests.ControllerTests
{
    public class CartControllerTests : IDisposable
    {
        private Mock<ICartService> _cartServiceMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private CartController _controller;
        private string _userId = "testUserId";

        [SetUp]
        public void Setup()
        {
            _cartServiceMock = new Mock<ICartService>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);

            _controller = new CartController(_cartServiceMock.Object, _userManagerMock.Object);

            // Mock the GetUserId method to return a test user ID
            _userManagerMock.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(_userId);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public async Task AddToCart_ShouldRedirectToHome_WhenItemIsAdded()
        {
            // Arrange
            var partId = "part123";
            var quantity = 2;

            // Act
            var result = await _controller.AddToCart(partId, quantity);

            // Assert
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
            Assert.AreEqual("Home", redirectToActionResult.ControllerName);

            _cartServiceMock.Verify(x => x.AddToCartAsync(_userId, partId, quantity), Times.Once);
        }

        [Test]
        public async Task ViewCart_ShouldReturnViewWithCartModel_WhenCartExists()
        {
            // Arrange
            var cart = new Cart
            {
                UserId = _userId,
                TotalCost = 400,
                Parts = new List<CartItem>
                    {
                        new CartItem { PartId = "part1", Brand = "Brand1", Model = "Model1", Quantity = 2, Price = 100, ImageUrl = "url1" },
                        new CartItem { PartId = "part2", Brand = "Brand2", Model = "Model2", Quantity = 1, Price = 200, ImageUrl = "url2" }
                    }
            };

            _cartServiceMock.Setup(x => x.RetrieveUserCartAsync(_userId)).ReturnsAsync(cart);

            // Act
            var result = await _controller.ViewCart();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as CartModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Items.Count);
            Assert.AreEqual(400, model.TotalPrice);
        }

        [Test]
        public async Task RemoveFromCart_ShouldRedirectToViewCart_WhenItemIsRemoved()
        {
            // Arrange
            var partId = "part123";

            // Act
            var result = await _controller.RemoveFromCart(partId);

            // Assert
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.AreEqual("ViewCart", redirectToActionResult.ActionName);

            _cartServiceMock.Verify(x => x.RemoveFromCartAsync(_userId, partId), Times.Once);
        }

        [Test]
        public async Task UpdateCart_ShouldReturnJson_WhenCartIsUpdated()
        {
            // Arrange
            var request = new CartController.UpdateQuantityRequest
            {
                PartId = "part123",
                Quantity = 3
            };

            var cart = new Cart
            {
                UserId = _userId,
                Parts = new List<CartItem>
                    {
                        new CartItem { PartId = "part123", Brand = "Brand1", Model = "Model1", Quantity = 2, Price = 100, ImageUrl = "url1" }
                    }
            };

            _cartServiceMock.Setup(x => x.RetrieveUserCartAsync(_userId)).ReturnsAsync(cart);
            _cartServiceMock.Setup(x => x.UpdateCartAsync(cart)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCart(request);

            // Assert
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);

            var jsonResponse = jsonResult.Value as dynamic;
            Assert.AreEqual("300.00", jsonResponse.itemTotal);  // 100 * 3
            Assert.AreEqual("300.00", jsonResponse.cartTotal);  // 100 * 3
        }

        [Test]
        public async Task UpdateCart_ShouldReturnBadRequest_WhenInvalidRequest()
        {
            // Arrange
            var request = new CartController.UpdateQuantityRequest
            {
                PartId = "", // Invalid partId
                Quantity = -1 // Invalid quantity
            };

            // Act
            var result = await _controller.UpdateCart(request);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task UpdateCart_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var request = new CartController.UpdateQuantityRequest
            {
                PartId = "part123",
                Quantity = 2
            };

            _userManagerMock.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(string.Empty);

            // Act
            var result = await _controller.UpdateCart(request);

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}
