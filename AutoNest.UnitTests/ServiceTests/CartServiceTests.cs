using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Services.Carts;
using Moq;

namespace AutoNest.UnitTests.ServiceTests
{
    [TestFixture]
    public class CartServiceTests
    {
        private Mock<IDeletableEntityRepository<Cart>> _cartRepositoryMock;
        private Mock<IDeletableEntityRepository<CartItem>> _cartItemRepositoryMock;
        private Mock<IDeletableEntityRepository<Part>> _partRepositoryMock;
        private Mock<IRepository<Image>> _imageRepositoryMock;
        private ICartService _cartService;

        [SetUp]
        public void SetUp()
        {
            _cartRepositoryMock = new Mock<IDeletableEntityRepository<Cart>>();
            _cartItemRepositoryMock = new Mock<IDeletableEntityRepository<CartItem>>();
            _partRepositoryMock = new Mock<IDeletableEntityRepository<Part>>();
            _imageRepositoryMock = new Mock<IRepository<Image>>();

            _cartService = new CartService(_cartRepositoryMock.Object, _partRepositoryMock.Object, _cartItemRepositoryMock.Object, _imageRepositoryMock.Object);
        }

        [Test]
        public async Task RetrieveUserCartAsync_ShouldReturnNewCart_WhenCartDoesNotExist()
        {
            // Arrange
            _cartRepositoryMock.Setup(repo => repo.All()).Returns(new List<Cart>().AsQueryable());

            // Act
            var cart = await _cartService.RetrieveUserCartAsync("user1");

            // Assert
            Assert.IsNotNull(cart);
            Assert.AreEqual(0, cart.TotalCost);
            Assert.IsEmpty(cart.Parts);
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddNewItem_WhenItemNotInCart()
        {
            // Arrange
            var part = new Part { Id = "part1", Brand = "BMW", Model = "X5", Price = 100 };
            _partRepositoryMock.Setup(repo => repo.GetById("part1")).Returns(part);

            var cart = new Cart { Id = "cart1", UserId = "user1", TotalCost = 0, Parts = new List<CartItem>() };
            _cartRepositoryMock.Setup(repo => repo.All()).Returns(new List<Cart> { cart }.AsQueryable());

            // Act
            await _cartService.AddToCartAsync("user1", "part1", 2);

            // Assert
            _cartRepositoryMock.Verify(repo => repo.Update(It.Is<Cart>(c =>
                c.Parts.Any(i => i.PartId == "part1" && i.Quantity == 2))), Times.Once);
            _cartRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AddToCartAsync_ShouldIncreaseQuantity_WhenItemAlreadyInCart()
        {
            // Arrange
            var part = new Part { Id = "part1", Brand = "BMW", Model = "X5", Price = 100 };
            var cartItem = new CartItem { CartId = "cart1", PartId = "part1", Quantity = 1, Price = 100 };
            var cart = new Cart { Id = "cart1", UserId = "user1", TotalCost = 100, Parts = new List<CartItem> { cartItem } };

            _partRepositoryMock.Setup(repo => repo.GetById("part1")).Returns(part);
            _cartRepositoryMock.Setup(repo => repo.All()).Returns(new List<Cart> { cart }.AsQueryable());

            // Act
            await _cartService.AddToCartAsync("user1", "part1", 2);

            // Assert
            Assert.AreEqual(3, cart.Parts.First().Quantity);
            _cartRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldRemoveItem_WhenItemExists()
        {
            // Arrange
            var cartItem = new CartItem { CartId = "cart1", PartId = "part1", Quantity = 2 };
            var cart = new Cart { Id = "cart1", UserId = "user1", Parts = new List<CartItem> { cartItem } };

            _cartRepositoryMock.Setup(repo => repo.All()).Returns(new List<Cart> { cart }.AsQueryable());
            _cartItemRepositoryMock.Setup(repo => repo.All()).Returns(new List<CartItem> { cartItem }.AsQueryable());

            // Act
            await _cartService.RemoveFromCartAsync("user1", "part1");

            // Assert
            _cartItemRepositoryMock.Verify(repo => repo.Delete(It.Is<CartItem>(i => i.PartId == "part1")), Times.Once);
            _cartItemRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ClearCartItems_ShouldRemoveAllItems()
        {
            // Arrange
            var cartItem1 = new CartItem { CartId = "cart1", PartId = "part1", Quantity = 2 };
            var cartItem2 = new CartItem { CartId = "cart1", PartId = "part2", Quantity = 1 };
            var cart = new Cart { Id = "cart1", UserId = "user1", Parts = new List<CartItem> { cartItem1, cartItem2 } };

            _cartItemRepositoryMock.Setup(repo => repo.All()).Returns(new List<CartItem> { cartItem1, cartItem2 }.AsQueryable());

            // Act
            await _cartService.ClearCartItems("cart1");

            // Assert
            _cartItemRepositoryMock.Verify(repo => repo.Delete(It.Is<CartItem>(i => i.PartId == "part1")), Times.Once);
            _cartItemRepositoryMock.Verify(repo => repo.Delete(It.Is<CartItem>(i => i.PartId == "part2")), Times.Once);
            _cartItemRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}
