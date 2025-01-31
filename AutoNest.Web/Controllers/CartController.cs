using AutoNest.Models.Carts;
using AutoNest.Services.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class CartController:Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(ICartService cartService,UserManager<IdentityUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(string partId, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            await _cartService.AddToCart(userId, partId, quantity);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult ViewCart()
        {
            var userId = _userManager.GetUserId(User);
            var cart = _cartService.GetCartForUser(userId).Result;

            var cartModel = new CartModel
            {
                Items = cart.Parts.Select(x => new CartItemModel
                {
                    Brand = x.Brand,
                    Model = x.Model,
                    Price = x.Price,
                    Quantity = x.Quantity
                }).ToList(),
                TotalPrice = cart.TotalCost
            };
            return View(cartModel);
        }
        [Authorize]
        public IActionResult RemoveFromCart(string partId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.RemoveFromCart(userId, partId);
            return RedirectToAction("ViewCart");
        }
        [Authorize]
        [HttpPost]
        public IActionResult UpdateCart([FromBody] UpdateQuantityRequest request)
        {
            var userId = _userManager.GetUserId(User);
            var cart = _cartService.GetCartForUser(userId).Result;
            var updatedItem = cart.Parts.FirstOrDefault(x => x.PartId == request.PartId);
            if (updatedItem == null)
            {
                return BadRequest(new { message = "Item not found in cart." });
            }
            updatedItem.Quantity = request.Quantity;

            _cartService.UpdateCart(cart);



            return Json(new
            {
                itemTotal = (updatedItem.Price * updatedItem.Quantity),
                cartTotal = cart.TotalCost.ToString("C")
            });
        }
        public class UpdateQuantityRequest
        {
            public string PartId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
