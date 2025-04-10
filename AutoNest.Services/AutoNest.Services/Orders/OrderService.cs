using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Orders;

namespace AutoNest.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> _orderRepository;

        public OrderService(IDeletableEntityRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task AddOrderAsync(OrderInputModel inputModel, string userId)
        {
            Order newOrder = new Order
            {
                CartId = inputModel.CartId,
                UserId = userId,
                SubTotal = inputModel.SubTotal,
                TotalAmount = inputModel.TotalAmount,
                ShippingAddress = inputModel.ShippingAddress,
                ShippingCity = inputModel.ShippingCity,
                ShippingCost = inputModel.ShippingCost,
                ShippingState = inputModel.ShippingState,
                ShippingZipCode = inputModel.ShippingZipCode,
                OrderStatus = OrderStatus.Pending,
            };

            await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveChangesAsync();

        }
        public async Task DeleteOrderAsync(string orderId)
        {
            var order = _orderRepository.GetById(orderId);
            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();
        }

        public List<OrderInputModel> GetAllOrders()
        {
            return _orderRepository.All().Select(o => new OrderInputModel
            {
                OrderId = o.Id,
                CartId = o.CartId,
                ShippingAddress = o.ShippingAddress,
                ShippingCity = o.ShippingCity,
                ShippingCost = o.ShippingCost,
                ShippingState = o.ShippingState,
                ShippingZipCode = o.ShippingZipCode,
                SubTotal = o.SubTotal,
                TotalAmount = o.TotalAmount,
                UserId = o.UserId,
                OrderStatus = o.OrderStatus,
            }).ToList();
        }

        public Order GetOrderById(string id)
        {
            return _orderRepository.GetById(id);
        }
        public async Task<Order> GetOrderForUserAsync(string userId)
        {
            return _orderRepository.All().FirstOrDefault(x => x.UserId == userId);
        }
        public async Task UpdateOrderAsync(OrderInputModel order)
        {
            Order order1 = new Order
            {
                Id = order.OrderId,
                CartId = order.CartId,
                UserId = order.UserId,
                SubTotal = order.SubTotal,
                ShippingCost = order.ShippingCost,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                ShippingCity = order.ShippingCity,
                ShippingState = order.ShippingState,
                ShippingZipCode = order.ShippingZipCode,
                OrderStatus = order.OrderStatus
            };

            _orderRepository.Update(order1);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
