using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;

namespace AutoNest.Services.Carts
{
    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<Cart> _cartRepository;
        private readonly IDeletableEntityRepository<Part> _partRepository;
        private readonly IDeletableEntityRepository<CartItem> _cartItemRepository;
        private readonly IRepository<Image> _imageRepository;

        public CartService(IDeletableEntityRepository<Cart> cartRepository,
            IDeletableEntityRepository<Part> partRepository, 
            IDeletableEntityRepository<CartItem> cartItemRepository,
            IRepository<Image> imageRepository)
        {
            _cartRepository = cartRepository;
            _partRepository = partRepository;
            _cartItemRepository = cartItemRepository;
            _imageRepository = imageRepository;
        }

        public async Task InitCartForUser(string userId)
        {
            var check = _cartRepository.All();
            if (check.Any(x => x.UserId == userId)) return;
            var cart = new Cart
            {
                UserId = userId,
                TotalCost = 0,
            };
            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();
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
                    ImageUrl = image?.RemoteImageUrl,
                };
                currentCart.Parts.Add(cartItem);
                _cartRepository.Update(currentCart);
            }
            await _cartRepository.SaveChangesAsync();
        }

        public async Task<Cart> RetrieveUserCartAsync(string userId)
        {
            var cart = _cartRepository.All().FirstOrDefault(x => x.UserId == userId);
            if(cart == null)
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
            var parts = _cartItemRepository.All().Where(x => x.CartId==cart.Id).ToList();

            var cartItem = parts.FirstOrDefault(p=>p.PartId == partId);
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
