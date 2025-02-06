using AutoNest.Data.Entities;

namespace AutoNest.Services.Carts
{
    public interface ICartService
    {
        Task<Cart> RetrieveUserCartAsync(string userId);
        public Task<List<CartItem>> GetCartItemsForCartAsync(string cartId);
        Task AddToCartAsync(string userId, string partId, int quantity);
        Task RemoveFromCartAsync(string userId, string partId);
        Task UpdateCartAsync(Cart cart);
    }
}
