using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MolineMart.API.Data;
using MolineMart.API.Dto;
using MolineMart.API.Models;
using Razorpay.Api;

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
        public async Task<ActionResult<IEnumerable<Models.Product>>> GetAll([FromQuery] ProductFilterDto filter)
        {

            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
                products = products.Where(p => p.Brand.Contains(filter.Search));

            if (!string.IsNullOrEmpty(filter.Brand))
                products = products.Where(p => p.Brand.Contains(filter.Brand));

            if (!string.IsNullOrEmpty(filter.Category))
                products = products.Where(p => p.Model.Contains(filter.Category));



            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var fileName = Path.GetFileName(product.ImageUrl);
                    product.ImageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";
                }
            }

            var result = await products.ToListAsync();
            return Ok(result);
        }

        [HttpGet("product-full/{id}")]
        public async Task<IActionResult> GetProductFull(int id)
        {
            // ✅ Fetch product
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            // ✅ Normalize image URL and product fields
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var fileName = Path.GetFileName(product.ImageUrl);
                product.ImageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";
            }

            // ✅ Fetch all additional product images
            var images = await _context.ProductImages
                .Where(x => x.ProductId == id)
                .Select(x => $"{Request.Scheme}://{Request.Host}{x.ImageUrl}")
                .ToListAsync();

            // ✅ Fetch reviews
            var reviews = await _context.ProductReviews
                .Where(x => x.ProductId == id)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();

            // ✅ Fetch features
            var features = await _context.ProductFeatures
                .Where(x => x.ProductId == id)
                .ToListAsync();

            // ✅ Fetch related product IDs
            var relatedIds = await _context.RelatedProducts
                .Where(x => x.ProductId == id)
                .Select(x => x.RelatedProductId)
                .ToListAsync();

            // ✅ Fetch full related product objects
            var relatedProducts = await _context.Products
                .Where(p => relatedIds.Contains(p.Id))
                .ToListAsync();

            // ✅ Normalize related product URLs
            foreach (var rel in relatedProducts)
            {
                if (!string.IsNullOrEmpty(rel.ImageUrl))
                {
                    var fileName = Path.GetFileName(rel.ImageUrl);
                    rel.ImageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";
                }
            }

            return Ok(new
            {
                product,
                images,
                reviews,
                features,
                relatedProducts
            });
        }



        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
        .Where(p => p.Id == id)
        .Select(p => new
        {
            p.Id,
            p.Brand,
            p.Model,
            p.Description,
            p.Price,
            p.Category,
            p.Storage,
            p.Color,
            p.Warranty,
            ImageUrl = !string.IsNullOrEmpty(p.ImageUrl)
                ? $"{Request.Scheme}://{Request.Host}/images/{Path.GetFileName(p.ImageUrl)}"
                : null
        })
        .FirstOrDefaultAsync();

            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }

        [HttpGet("products/related/{brand}")]
        public async Task<IActionResult> GetRelated(string brand)
        {
            var related = await _context.Products
       .Where(p => p.Brand == brand)
       .Select(p => new
       {
           p.Id,
           p.Brand,
           p.Model,
           p.Description,
           p.Price,
           ImageUrl = !string.IsNullOrEmpty(p.ImageUrl)
               ? $"{Request.Scheme}://{Request.Host}/images/{Path.GetFileName(p.ImageUrl)}"
               : null
       })
       .ToListAsync();

            return Ok(related);
        }

        [HttpGet("delivery/{pincode}")]
        public async Task<IActionResult> CheckDelivery(string pincode)
        {
            var availability = await _context.DeliveryAvailability
                .FirstOrDefaultAsync(x => x.Pincode == pincode);

            if (availability == null)
                return Ok(new
                {
                    Pincode = pincode,
                    IsAvailable = false,
                    EstimatedDays = (int?)null,
                    Message = "Delivery not available in your area"
                });

            return Ok(new
            {
                availability.Pincode,
                availability.IsAvailable,
                availability.EstimatedDays,
                Message = availability.IsAvailable
                    ? $"Delivery available in {availability.EstimatedDays} days."
                    : "Delivery is not available at your location."
            });
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
