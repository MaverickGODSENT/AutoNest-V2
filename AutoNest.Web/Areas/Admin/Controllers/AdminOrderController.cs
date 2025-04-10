using AutoNest.Models.Carts;
using AutoNest.Models.Orders;
using AutoNest.Services.Carts;
using AutoNest.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        public AdminOrderController(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var orders = _orderService.GetAllOrders().Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                CartId = o.CartId,
                SubTotal = o.SubTotal,
                TotalAmount = o.TotalAmount,
                ShippingCost = o.ShippingCost,
                ShippingAddress = o.ShippingAddress,
                ShippingCity = o.ShippingCity,
                ShippingState = o.ShippingState,
                ShippingZipCode = o.ShippingZipCode,
            });

            return View(orders);
        }

        public async Task<IActionResult> Details(string orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            var cartItems = await _cartService.GetCartItemsForCartAsync(order.CartId);

            OrderViewModel orderViewModel = new OrderViewModel
            {
                OrderId = order.Id,
                UserId = order.UserId,
                CartId = order.CartId,
                OrderStatus = order.OrderStatus.ToString(),
                SubTotal = order.SubTotal,
                TotalAmount = order.TotalAmount,
                ShippingCost = order.ShippingCost,
                ShippingAddress = order.ShippingAddress,
                ShippingCity = order.ShippingCity,
                ShippingState = order.ShippingState,
                ShippingZipCode = order.ShippingZipCode,
                CartItems = cartItems.Select(c => new CartItemModel
                {
                    PartId = c.PartId,
                    Quantity = c.Quantity,
                    Brand = c.Brand,
                    Model = c.Model,
                    Price = c.Price,
                    ImageUrl = c.ImageUrl,
                }).ToList(),
            };

            return View(orderViewModel);
        }

        public async Task<IActionResult> Delete(string orderId)
        {
            await _orderService.DeleteOrderAsync(orderId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(string orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            var model = new OrderInputModel
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
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(OrderInputModel orderInputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            await _orderService.UpdateOrderAsync(orderInputModel);
            return RedirectToAction("Index");
        }
    }
}
