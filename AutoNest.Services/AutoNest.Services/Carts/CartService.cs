using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutoNest.Services.Carts
{
    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<Cart> _cartRepository;
        private readonly IDeletableEntityRepository<Part> _partRepository;
        private readonly IDeletableEntityRepository<CartItem> _cartItemRepository;
        private readonly IRepository<Image> _imageRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CartService(IDeletableEntityRepository<Cart> cartRepository,
            IDeletableEntityRepository<Part> partRepository,
            IDeletableEntityRepository<CartItem> cartItemRepository,
            IRepository<Image> imageRepository,
            UserManager<IdentityUser> userManager)
        {
            _cartRepository = cartRepository;
            _partRepository = partRepository;
            _cartItemRepository = cartItemRepository;
            _imageRepository = imageRepository;
            _userManager = userManager;
        }

        public async Task InitCartForUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.");
            }

            var userExists = await _userManager.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                throw new InvalidOperationException($"User with ID {userId} does not exist.");
            }

            var hasCart = await _cartRepository.All()
                                               .AnyAsync(x => x.UserId == userId);

            if (!hasCart)
            {
                var cart = new Cart
                {
                    UserId = userId,
                    TotalCost = 0,
                };

                await _cartRepository.AddAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }
        }
        public async Task ClearCartItems(string cartId)
        {
            var cartItems = _cartItemRepository.All().Where(x => x.CartId == cartId).ToList();
            foreach (var item in cartItems)
            {
                _cartItemRepository.Delete(item);
            }
            await _cartItemRepository.SaveChangesAsync();
        }
        public async Task AddToCartAsync(string userId, string partId, int quantity)
        {
            var currentCart = _cartRepository.All().FirstOrDefault(x => x.UserId == userId);

            if (currentCart == null)
            {
                currentCart = new Cart
                {
                    UserId = userId,
                    TotalCost = 0,
                };
                await _cartRepository.AddAsync(currentCart);
            }

            var cartItem = currentCart.Parts.FirstOrDefault(x => x.PartId == partId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var part = _partRepository.GetById(partId);
                var image = _imageRepository.All().FirstOrDefault(x => x.PartId == partId);
                if (part == null)
                {
                    throw new ArgumentException("Part not found");
                }

                cartItem = new CartItem
                {
                    CartId = currentCart.Id,
                    PartId = partId,
                    Brand = part.Brand,
                    Model = part.Model,
                    Price = part.Price,
                    Quantity = quantity,
                    ImageUrl = image?.RemoteImageUrl ?? string.Empty,
                };
                currentCart.Parts.Add(cartItem);
                _cartRepository.Update(currentCart);
            }
            await _cartRepository.SaveChangesAsync();
        }

        public async Task<Cart> RetrieveUserCartAsync(string userId)
        {
            var cart = _cartRepository.All().FirstOrDefault(x => x.UserId == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    TotalCost = 0,
                };
                await _cartRepository.AddAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }

            cart.Parts = await GetCartItemsForCartAsync(cart.Id) ?? new List<CartItem>();



            return cart;
        }

        public async Task<List<CartItem>> GetCartItemsForCartAsync(string cartId)
        {
            return await Task.Run(() => _cartItemRepository.All().Where(c => c.CartId == cartId).ToList());
        }


        public async Task RemoveFromCartAsync(string userId, string partId)
        {
            var cart = _cartRepository.All().FirstOrDefault(x => x.UserId == userId);
            if (cart == null) return;
            var parts = _cartItemRepository.All().Where(x => x.CartId == cart.Id).ToList();

            var cartItem = parts.FirstOrDefault(p => p.PartId == partId);
            if (cartItem != null)
            {
                _cartItemRepository.Delete(cartItem);
                await _cartItemRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();
        }
    }
}
