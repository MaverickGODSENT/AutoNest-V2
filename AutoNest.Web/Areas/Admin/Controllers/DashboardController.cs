using AutoNest.Models.Orders;
using AutoNest.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IOrderService _orderService;
        public DashboardController(IOrderService orderService)
        {
            _orderService = orderService;
        }



        public IActionResult Index()
        {
            var orders = _orderService.GetAllOrders();
            var convOrders = orders.Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                CartId = o.CartId,
                ShippingAddress = o.ShippingAddress,
                ShippingCity = o.ShippingCity,
                ShippingCost = o.ShippingCost,
                ShippingState = o.ShippingState,
                ShippingZipCode = o.ShippingZipCode,
                SubTotal = o.SubTotal,
                TotalAmount = o.TotalAmount,
                UserId = o.UserId,
                OrderStatus = o.OrderStatus.ToString(),
            }).ToList();
            return View(convOrders);
        }
    }
}
