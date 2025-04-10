using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Orders;
using AutoNest.Services.Orders;
using Moq;

namespace AutoNest.UnitTests.ServiceTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IDeletableEntityRepository<Order>> _mockOrderRepo;
        private IOrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            _mockOrderRepo = new Mock<IDeletableEntityRepository<Order>>();
            _orderService = new OrderService(_mockOrderRepo.Object);
        }

        [Test]
        public async Task AddOrderAsync_ShouldAddOrder_WhenValidInput()
        {
            // Arrange
            var orderInput = new OrderInputModel
            {
                OrderId = "123",
                CartId = "456",
                SubTotal = 100.00M,
                ShippingCost = 10.00M,
                TotalAmount = 110.00M,
                ShippingAddress = "123 Main St",
                ShippingCity = "City",
                ShippingState = "State",
                ShippingZipCode = "1234",
                PaymentMethod = PaymentMethod.CreditCard,
                OrderStatus = OrderStatus.Pending
            };

            // Act
            await _orderService.AddOrderAsync(orderInput, "userId");

            // Assert
            _mockOrderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            _mockOrderRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteOrderAsync_ShouldDeleteOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = "123" };
            _mockOrderRepo.Setup(r => r.GetById("123")).Returns(order);

            // Act
            await _orderService.DeleteOrderAsync("123");

            // Assert
            _mockOrderRepo.Verify(r => r.Delete(It.IsAny<Order>()), Times.Once);
            _mockOrderRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteOrderAsync_ShouldNotDelete_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderRepo.Setup(r => r.GetById("123")).Returns((Order)null);

            // Act
            await _orderService.DeleteOrderAsync("123");

            // Assert
            _mockOrderRepo.Verify(r => r.Delete(It.IsAny<Order>()), Times.Never);
            _mockOrderRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void GetAllOrders_ShouldReturnAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = "123", UserId = "user1", CartId = "456", SubTotal = 100.00M },
                new Order { Id = "124", UserId = "user2", CartId = "457", SubTotal = 150.00M }
            };
            _mockOrderRepo.Setup(r => r.All()).Returns(orders.AsQueryable());

            // Act
            var result = _orderService.GetAllOrders();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("123", result.First().OrderId);
            Assert.AreEqual("user1", result.First().UserId);
        }

        [Test]
        public async Task GetOrderById_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = "123", UserId = "user1" };
            _mockOrderRepo.Setup(r => r.GetById("123")).Returns(order);

            // Act
            var result = _orderService.GetOrderById("123");

            // Assert
            Assert.AreEqual("123", result.Id);
            Assert.AreEqual("user1", result.UserId);
        }

        [Test]
        public async Task GetOrderById_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderRepo.Setup(r => r.GetById("123")).Returns((Order)null);

            // Act
            var result = _orderService.GetOrderById("123");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateOrderAsync_ShouldUpdateOrder_WhenOrderExists()
        {
            // Arrange
            var existingOrder = new Order
            {
                Id = "123",
                CartId = "456",
                SubTotal = 100.00M,
                ShippingCost = 10.00M,
                TotalAmount = 110.00M
            };
            _mockOrderRepo.Setup(r => r.GetById("123")).Returns(existingOrder);

            var orderInput = new OrderInputModel
            {
                OrderId = "123",
                CartId = "456",
                SubTotal = 120.00M,
                ShippingCost = 15.00M,
                TotalAmount = 135.00M,
                ShippingAddress = "456 Main St",
                ShippingCity = "New City",
                ShippingState = "New State",
                ShippingZipCode = "5678",
                PaymentMethod = PaymentMethod.CreditCard,
                OrderStatus = OrderStatus.Shipped
            };

            // Act
            await _orderService.UpdateOrderAsync(orderInput);

            // Assert
            _mockOrderRepo.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
            _mockOrderRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateOrderAsync_ShouldNotUpdate_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderRepo.Setup(r => r.GetById("123")).Returns((Order)null);

            var orderInput = new OrderInputModel
            {
                OrderId = "123",
                CartId = "456",
                SubTotal = 120.00M,
                ShippingCost = 15.00M,
                TotalAmount = 135.00M,
                ShippingAddress = "456 Main St",
                ShippingCity = "New City",
                ShippingState = "New State",
                ShippingZipCode = "5678",
                PaymentMethod = PaymentMethod.CreditCard,
                OrderStatus = OrderStatus.Shipped
            };

            // Act
            await _orderService.UpdateOrderAsync(orderInput);

            // Assert
            _mockOrderRepo.Verify(r => r.Update(It.IsAny<Order>()), Times.Never);
            _mockOrderRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
