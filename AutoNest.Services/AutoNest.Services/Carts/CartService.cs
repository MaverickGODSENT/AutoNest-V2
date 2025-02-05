using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Carts;

namespace AutoNest.Services.Carts
{
    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<Cart> _cartRepository;
        private readonly IDeletableEntityRepository<Part> _partRepository;
        private readonly IDeletableEntityRepository<CartItem> _cartItemRepository;
        public CartService(IDeletableEntityRepository<Cart> cartRepository,
            IDeletableEntityRepository<Part> partRepository, 
            IDeletableEntityRepository<CartItem> cartItemRepository)
        {
            _cartRepository = cartRepository;
            _partRepository = partRepository;
            _cartItemRepository = cartItemRepository;
        }




        public async Task AddToCart(string userId, string partId, int quantity)
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
                    Quantity = quantity
                };
                currentCart.Parts.Add(cartItem);
                _cartRepository.Update(currentCart);
            }
            await _cartRepository.SaveChangesAsync();
        }

        public async Task<Cart> GetCartForUser(string userId)
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

            cart.Parts = _cartItemRepository.All().Where(x => x.CartId == cart.Id).ToList();



            return cart;
        }

        public async Task RemoveFromCart(string userId, string partId)
        {
            var cart = _cartRepository.All().FirstOrDefault(x => x.UserId == userId);
            var parts = _cartItemRepository.All().Where(x => x.CartId==cart.Id).ToList();

            var cartItem = parts.FirstOrDefault(p=>p.PartId == partId);
            if (cartItem != null)
            {
                _cartItemRepository.Delete(cartItem);
                await _cartItemRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateCart(Cart cart)
        {
            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();
        }
    }
}
