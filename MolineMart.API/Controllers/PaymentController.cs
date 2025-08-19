using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MolineMart.API.Dto;
using MolineMart.API.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MolineMart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly RazorpayService _razorpayService;
        private readonly IConfiguration _config;

        public PaymentController(RazorpayService razorpayService,IConfiguration config)
        {
            _razorpayService = razorpayService;
            _config = config;
        }

        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null || request.Amount <= 0)
            {
                return BadRequest("Invalid order details.");
            }
            var receiptId = string.IsNullOrEmpty(request.ReceiptId) ? Guid.NewGuid().ToString() : request.ReceiptId;
            var order = _razorpayService.CreateOrder(request.Amount, receiptId);
            if (order == null)
            {
                return StatusCode(500, "Failed to create order.");
            }
            return Ok(new { OrderId = order["id"].ToString(), Amount = order["amount"],Currency = "INR" });
        }

        [HttpPost("verify-signature")]
        public IActionResult VerifySignature([FromBody] VerifySignatureRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.OrderId) || string.IsNullOrEmpty(request.PaymentId) || string.IsNullOrEmpty(request.Signature))
            {
                return BadRequest("Invalid signature verification details.");
            }
            var isValid = _razorpayService.VerifySignature(request.OrderId, request.PaymentId, request.Signature);
            if (!isValid)
            {
                return BadRequest("Invalid signature.");
            }
            return Ok("Signature verified successfully.");
        }

        [HttpPost("razorpay-webhook")]
        public async Task<IActionResult> RazorpayWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var payload = await reader.ReadToEndAsync();
            var signature = Request.Headers["X-Razorpay-Signature"].ToString();

            var secret = _config["Razorpay:Secret"];
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var expectedSignature = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

            if(signature != expectedSignature)
            {
                return Unauthorized("Invalid signature.");
            }

            var data = JsonSerializer.Deserialize<JsonElement>(payload);
            var paymentId = data.GetProperty("payload").GetProperty("payment").GetProperty("entity").GetProperty("id").GetString();

            return Ok(new { Message = "Webhook received successfully.", PaymentId = paymentId });
        }
    }
}
