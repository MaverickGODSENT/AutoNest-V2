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

        public async Task AddOrderAsync(OrderInputModel inputModel,string userId)
        {
            Order newOrder = new Order
            {
                CartId = inputModel.CartId,
                UserId = userId,
                SubTotal = inputModel.SubTotal,
                ShippingAddress = inputModel.ShippingAddress,
                ShippingCity = inputModel.ShippingCity,
                ShippingCost = inputModel.ShippingCost,
                TotalAmount = inputModel.TotalAmount,
            };

            await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveChangesAsync();

        }
    }
}
