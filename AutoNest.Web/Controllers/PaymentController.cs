using AutoNest.Models.Carts;
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

    public PaymentController(IStripeService stripeService, IOrderService orderService, ICartService cartService, UserManager<IdentityUser> userManager)
    {
        _stripeService = stripeService;
        _orderService = orderService;
        _cartService = cartService;
        _userManager = userManager;
    }
    public IActionResult PaymentCancel()
    {
        return View();
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

    public async Task<IActionResult> PaymentSuccess(string session_id)
    {
        if (string.IsNullOrEmpty(session_id))
        {
            return BadRequest("Invalid payment session.");
        }

        var isPaymentSuccessful = await _stripeService.VerifyPaymentAsync(session_id);
        var userId = User.Identity.IsAuthenticated ? _userManager.GetUserId(User) : "Guest";
        var orderCheck = await _orderService.GetOrderForUserAsync(userId);
        var orderId = orderCheck?.Id;

        if (isPaymentSuccessful)
        {
            if (orderCheck == null)
            {
                return NotFound("Order not found.");
            }
        }

        if (string.IsNullOrEmpty(orderId))
        {
            return RedirectToAction("Index", "Home");
        }

        var order = _orderService.GetOrderById(orderId);
        var cartItems = await _cartService.GetCartItemsForCartAsync(order.CartId);



        if (order == null)
        {
            return NotFound();
        }

        var orderViewModel = new OrderViewModel
        {
            OrderId = order.Id,
            UserId = order.UserId,
            CartId = order.CartId,
            SubTotal = order.SubTotal,
            ShippingCost = order.ShippingCost,
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            ShippingCity = order.ShippingCity,
            ShippingState = order.ShippingState,
            ShippingZipCode = order.ShippingZipCode,
            OrderStatus = order.OrderStatus.ToString(),
            CartItems = cartItems.Select(c => new CartItemModel
            {
                PartId = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Price = c.Price,
                Quantity = c.Quantity,
                ImageUrl = c.ImageUrl,
            }).ToList()
        };
        
        await _cartService.ClearCartItems(order.CartId);
        return View(orderViewModel);
    }
}
