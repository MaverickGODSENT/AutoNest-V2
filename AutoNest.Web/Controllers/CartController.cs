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
                    PartId = x.PartId,
                    Brand = x.Brand,
                    Model = x.Model,
                    Price = x.Price,
                    Quantity = x.Quantity
                }).ToList(),
                TotalPrice = cart.Parts.Sum(x => x.Price * x.Quantity)
            };
            return View(cartModel);
        }
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(string Id)
        {
            var userId = _userManager.GetUserId(User);
            await _cartService.RemoveFromCart(userId, Id);
            return RedirectToAction("ViewCart");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromBody] UpdateQuantityRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.PartId) || request.Quantity < 1)
            {
                return BadRequest(new { message = "Invalid request data." });
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            try
            {
                var cart = _cartService.GetCartForUser(userId).Result;
                if (cart == null || cart.Parts == null)
                {
                    return NotFound(new { message = "Cart not found." });
                }

                var updatedItem = cart.Parts.FirstOrDefault(x => x.PartId == request.PartId);
                if (updatedItem == null)
                {
                    return NotFound(new { message = "Item not found in cart." });
                }

                updatedItem.Quantity = request.Quantity;
                await _cartService.UpdateCart(cart);

                return Json(new
                {
                    itemTotal = (updatedItem.Price * updatedItem.Quantity).ToString("C"),
                    cartTotal = cart.TotalCost.ToString("C")
                });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return StatusCode(500, new { message = "An error occurred while updating the cart." });
            }
        }
        public class UpdateQuantityRequest
        {
            public string PartId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
