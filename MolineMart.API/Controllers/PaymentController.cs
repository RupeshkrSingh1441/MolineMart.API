using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MolineMart.API.Data;
using MolineMart.API.Dto;
using MolineMart.API.Models;
using MolineMart.API.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MolineMart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class PaymentController : ControllerBase
    {
        private readonly RazorpayService _razorpayService;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context; // Add this field to inject the DbContext
        private readonly IEmailSender _emailSender; // Add this field to inject the EmailSender service

        public PaymentController(RazorpayService razorpayService,IConfiguration config, ApplicationDbContext context,IEmailSender emailSender)
        {
            _razorpayService = razorpayService;
            _config = config;
            _context = context;
            _emailSender = emailSender;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
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
            // save to DB
            var dbOrder = new Order
            {
                RazorpayOrderId = order["id"].ToString(),
                Amount = request.Amount,
                Email = request.Email,
                Status = "Pending"
            };

            _context.Orders.Add(dbOrder);
            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                key = _config["Razorpay:Key"],
                OrderId = order["id"].ToString(), 
                Amount = order["amount"].ToString(),
                Currency = "INR" 
            });
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
            var signature = Request.Headers["X-Razorpay-Signature"];
            var secret = _config["Razorpay:Secret"];
            var webhookSecret = _config["Razorpay:WebhookSecret"];

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(webhookSecret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var expectedSignature = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            //Convert.ToBase64String(hash);
            //

            if (signature != expectedSignature)
            {
                return Unauthorized("Invalid signature.");
            }

            var data = JsonSerializer.Deserialize<JsonElement>(payload);
            var entity = data.GetProperty("payload").GetProperty("payment").GetProperty("entity");

            var paymentId = entity.GetProperty("id").GetString();
            var orderId = entity.GetProperty("order_id").GetString();
            var status = entity.GetProperty("status").GetString();

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.RazorpayOrderId == orderId);
            if (order != null)
            {
                order.RazorpayPaymentId = paymentId;
                order.Status = status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                if (status == "captured")
                {
                    // Handle successful payment logic here, e.g., update order status, notify user, etc.
                    await _emailSender.SendWebhookEmailAsync(order.Email, "Payment Successful", 
                        $"<h3>Thank you for your purchase!</h3><p>Order ID: {order.RazorpayOrderId}</p><p>Amount: ₹{order.Amount}</p>");
                }
            }
            else
            {
                // Log or handle the case where the order is not found
                return NotFound("Order not found.");
            }

            return Ok(new { Message = "Webhook received successfully.", PaymentId = paymentId });
        }
    }
}
