using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MolineMart.API.Data;
using MolineMart.API.Dto;
using MolineMart.API.Models;

namespace MolineMart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] ProductFilterDto filter)
        {

            var products =  _context.Products.AsQueryable();

            if(!string.IsNullOrEmpty(filter.Search))
                products = products.Where(p => p.Brand.Contains(filter.Search));

            if (!string.IsNullOrEmpty(filter.Brand))
                products = products.Where(p => p.Brand.Contains(filter.Brand));

            if (!string.IsNullOrEmpty(filter.Category))
                products = products.Where(p => p.Model.Contains(filter.Category));

            

            foreach (var product in products)
            {
                if(!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var fileName = Path.GetFileName(product.ImageUrl);
                    product.ImageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";
                }
            }

            var result = await products.ToListAsync();
            return Ok(result);
        }

        [HttpPost("fix-image-urls")]
        public async Task<IActionResult> FixImageUrls()
        {
            var products = await _context.Products.ToListAsync();
            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var fileName = Path.GetFileName(product.ImageUrl);
                    product.ImageUrl = $"/images/{fileName}"; // relative path
                }
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        

    }
}
