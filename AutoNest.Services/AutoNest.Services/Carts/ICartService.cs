using AutoNest.Data.Entities;

namespace AutoNest.Services.Carts
{
    public interface ICartService
    {
        Task InitCartForUser(string userId);
        Task<Cart> RetrieveUserCartAsync(string userId);
        Task<List<CartItem>> GetCartItemsForCartAsync(string cartId);
        Task ClearCartItems(string cartId);
        Task AddToCartAsync(string userId, string partId, int quantity);
        Task RemoveFromCartAsync(string userId, string partId);
        Task UpdateCartAsync(Cart cart);
    }
}
