using AutoNest.Services.Stripe;
using Microsoft.Extensions.Configuration;
using Moq;
using Stripe;
using Stripe.BillingPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNest.UnitTests.ServiceTests
{
    public class StripeServiceTests
    {
        private IStripeService _stripeService;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Adjust as needed
                .AddJsonFile("appsettings.Development.json") // or test-specific config
                .Build();

            _stripeService = new StripeService(configuration);
        }

        [Test]
        public async Task CreateCheckoutSessionAsync_ShouldReturn_ValidSessionUrl()
        {
            // Arrange
            decimal amount = 10.00m;
            string currency = "usd";
            string successUrl = "https://example.com/success";
            string cancelUrl = "https://example.com/cancel";

            // Act
            var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(amount, currency, successUrl, cancelUrl);

            // Assert
            Assert.IsNotNull(sessionUrl);
            Assert.IsTrue(sessionUrl.StartsWith("https://checkout.stripe.com/"), "Session URL should start with Stripe checkout base URL");
        }

        [Test]
        public async Task VerifyPaymentAsync_WithInvalidSessionId_ShouldReturnFalse()
        {
            // Arrange
            string invalidSessionId = "invalid_session_id";

            // Act
            bool result = await _stripeService.VerifyPaymentAsync(invalidSessionId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
