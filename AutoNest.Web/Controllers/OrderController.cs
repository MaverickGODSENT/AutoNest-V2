using AutoNest.Models.Orders;
using AutoNest.Services.Carts;
using AutoNest.Services.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class OrderController:Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IOrderService orderService,
            ICartService cartService,
            UserManager<IdentityUser> userManager)
        {
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Order()
        {
            OrderInputModel inputModel = new OrderInputModel();
            var cart = await _cartService.RetrieveUserCartAsync(_userManager.GetUserId(User));
            var cartItemms = await _cartService.GetCartItemsForCartAsync(cart.Id);

            inputModel.ShippingCost = 5*cartItemms.Count;
            inputModel.SubTotal = cartItemms.Sum(x => x.Price * x.Quantity);
            inputModel.TotalAmount = inputModel.ShippingCost + inputModel.SubTotal;

            return View(inputModel);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitOrder(OrderInputModel inputModel)
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _cartService.RetrieveUserCartAsync(userId);
            inputModel.CartId = cart.Id;
            inputModel.CartItems = await _cartService.GetCartItemsForCartAsync(cart.Id);

            await _orderService.AddOrderAsync(inputModel,userId); 

            return RedirectToAction("Index", "Payment",inputModel);
        }

    }
}
