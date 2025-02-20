using AutoNest.Models.Orders;
using AutoNest.Services.Carts;
using AutoNest.Services.Orders;
using AutoNest.Services.Stripe;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class PaymentController : Controller
{
    private readonly IStripeService _stripeService;
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly UserManager<IdentityUser> _userManager;

    public PaymentController(IStripeService stripeService, IOrderService orderService, ICartService cartService ,UserManager<IdentityUser> userManager)
    {
        _stripeService = stripeService;
        _orderService = orderService;
        _cartService = cartService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(OrderInputModel orderInputModel)
    {
        var cartItems = await _cartService.GetCartItemsForCartAsync(orderInputModel.CartId);
        orderInputModel.CartItems = cartItems;
        return View(orderInputModel);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(decimal amount, string orderId)
    {
        var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(
            amount,
            "usd",
            Url.Action("PaymentSuccess", "Payment", new { orderId }, Request.Scheme),
            Url.Action("PaymentCancel", "Payment", null, Request.Scheme)
        );

        return Redirect(sessionUrl);
    }

    public async Task<IActionResult> PaymentSuccess(string session_id, string orderId)
    {
        if (string.IsNullOrEmpty(session_id))
        {
            return BadRequest("Invalid payment session.");
        }

        var isPaymentSuccessful = await _stripeService.VerifyPaymentAsync(session_id);

        if (isPaymentSuccessful)
        {
            var userId = User.Identity.IsAuthenticated ? _userManager.GetUserId(User) : "Guest";

            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
        }

        return RedirectToAction("Index", "Home");
    }
}
