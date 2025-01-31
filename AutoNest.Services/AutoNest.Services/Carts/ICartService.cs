using AutoNest.Data.Entities;

namespace AutoNest.Services.Carts
{
    public interface ICartService
    {
        Task<Cart> GetCartForUser(string userId);
        Task AddToCart(string userId, string partId, int quantity);
        void RemoveFromCart(string userId, string partId);
        void UpdateCart(Cart cart);
    }
}
