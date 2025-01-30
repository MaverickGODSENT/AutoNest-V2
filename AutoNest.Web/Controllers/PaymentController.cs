using AutoNest.Data.Entities;
using AutoNest.Services.Stripe;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoNest.Web.Controllers
{
    public class PaymentController : Controller
    {
        /*private readonly IStripeService _stripeService;

        public PaymentController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(decimal ammount)
        {
            var sessionId = await _stripeService.CreateCheckoutSessionAsync(
                ammount,
                "usd",
                Url.Action("PaymentSuccess", "Payment", null, Request.Scheme),
                Url.Action("PaymentCancel", "Payment", null, Request.Scheme)
            );

            return Redirect(sessionId);
        }

        public async Task<IActionResult> PaymentSuccess(string sessionId)
        {
            var isPaymentSuccessful = await _stripeService.VerifyPaymentAsync(sessionId);

            if (isPaymentSuccessful)
            {
                var payment = new Payment
                {
                    UserId = User.Identity.Name,
                };
            }
            return View();
        }*/
    }
}
