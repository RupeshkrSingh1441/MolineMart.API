using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MolineMart.API.Data;

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

        [Authorize(Roles = "Admin")]
        [HttpGet("all-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            // Implementation for fetching orders goes here
            var orders = await _context.Orders.OrderByDescending(o => o.OrderDate).ToListAsync();

            return Ok(orders);
        }
    }
}
