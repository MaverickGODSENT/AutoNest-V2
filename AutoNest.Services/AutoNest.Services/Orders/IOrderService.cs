using AutoNest.Data.Entities;
using AutoNest.Models.Orders;

namespace AutoNest.Services.Orders
{
    public interface IOrderService
    {
        public List<OrderInputModel> GetAllOrders();
        public Task AddOrderAsync(OrderInputModel inputModel, string userId);
        public Task DeleteOrderAsync(string orderId);
        public Order GetOrderById(string id);
        public Task<Order> GetOrderForUserAsync(string userId);
        public Task UpdateOrderAsync(OrderInputModel order);
    }
}
