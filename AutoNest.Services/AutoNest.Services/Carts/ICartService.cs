using AutoNest.Data.Entities;

namespace AutoNest.Services.Carts
{
    public interface ICartService
    {
        Task<Cart> GetCartForUser(string userId);
        Task AddToCart(string userId, string partId, int quantity);
        Task RemoveFromCart(string userId, string partId);
        Task UpdateCart(Cart cart);
    }
}
