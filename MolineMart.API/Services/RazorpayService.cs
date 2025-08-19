using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace MolineMart.API.Services
{
    public class RazorpayService
    {
        private readonly string _key;
        private readonly string _secret;

        public RazorpayService(IConfiguration config)
        {
                _key = config["Razorpay:Key"];
                _secret = config["Razorpay:Secret"];
        }

        public Order CreateOrder(decimal amount, string receiptId)
        {
            var client = new RazorpayClient(_key, _secret);
            var options = new Dictionary<string, object>
                {
                    { "amount", (int)(amount * 100) }, // amount in paise
                    { "currency", "INR" },
                    { "receipt", receiptId },
                    { "payment_capture", 1 } // auto capture payment
                };
            return client.Order.Create(options);
        }

        public bool VerifySignature(string orderId, string paymentId, string signature)
        {
            var client = new RazorpayClient(_key, _secret);
            string payLoad = $"{orderId}|{paymentId}";
            string secret = _secret;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payLoad));
            var generatedSignature = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return generatedSignature == signature;
        }   
    }
}
