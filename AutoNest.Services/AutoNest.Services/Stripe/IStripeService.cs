namespace AutoNest.Services.Stripe
{
    public interface IStripeService
    {
        public Task<string> CreateCheckoutSessionAsync(decimal ammount, string currency, string successUrl, string cancelUrl);
        public Task<bool> VerifyPaymentAsync(string sessionId);
    }
}
