using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace AutoNest.Services.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;

        public StripeService(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<string> CreateCheckoutSessionAsync(decimal amount, string currency, string successUrl, string cancelUrl)
        {
            var successUri = new UriBuilder(successUrl);
            var query = System.Web.HttpUtility.ParseQueryString(successUri.Query);
            query["session_id"] = "{CHECKOUT_SESSION_ID}";
            successUri.Query = query.ToString();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(amount * 100),
                    Currency = currency,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Order Payment",
                    },
                },
                Quantity = 1,
            },
        },
                Mode = "payment",
                SuccessUrl = successUri.ToString(),
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return session.Url;
        }


        public async Task<bool> VerifyPaymentAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return false;
            }

            try
            {
                var service = new SessionService();
                Session session = await service.GetAsync(sessionId);

                return session.PaymentStatus == "paid";
            }
            catch (StripeException ex)
            {
                Console.WriteLine($"Stripe Error: {ex.Message}");
                return false;
            }
        }
    }
}
