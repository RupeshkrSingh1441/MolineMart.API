using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MolineMart.API.Data;
using MolineMart.API.Dto;
using System.Security.Claims;

namespace MolineMart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("all-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            // Implementation for fetching orders goes here
            var orders = await _context.Orders.OrderByDescending(o => o.OrderDate).ToListAsync();

            return Ok(orders);
        }

        // New: user-specific orders
        [HttpGet("user-orders")]
        //[Authorize]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                         User.FindFirst("sub")?.Value ??
                         User.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Include items and product details so frontend can show thumbnails/prices
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Optionally map to DTOs to reduce payload
            var result = orders.Select(o => new UserOrderDto
            {
                Id = o.Id,
                Status = o.Status,
                Amount = o.Amount,
                OrderDate = o.OrderDate,

                ProductModel = o.Items.FirstOrDefault()?.Product?.Model ?? "Product",
                ProductImage = o.Items.FirstOrDefault()?.Product?.ImageUrl
            });
            return Ok(result);

        }
    }
}
