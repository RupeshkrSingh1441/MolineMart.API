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
        public async Task<IActionResult> GetProducts(
        [FromQuery] string q,
        [FromQuery] string brand,
        [FromQuery] string category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string sort,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 12;

            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.Trim().ToLower();
                query = query.Where(p =>
                    p.Model.ToLower().Contains(lower) ||
                    p.Brand.ToLower().Contains(lower) ||
                    p.Description.ToLower().Contains(lower));
            }

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(p => p.Brand == brand);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category == category);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            // sorting
            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "newest" => query.OrderByDescending(p => p.Id), // or CreatedDate
                _ => query.OrderBy(p => p.Brand).ThenBy(p => p.Model)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new {
                    id = p.Id,
                    brand = p.Brand,
                    model = p.Model,
                    price = p.Price,
                    imageUrl = p.ImageUrl,
                    category = p.Category,
                    description = p.Description
                })
                .ToListAsync();

            return Ok(new
            {
                total = totalCount,
                page,
                pageSize,
                items
            });
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

        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _context.Products
                .Where(p => !string.IsNullOrEmpty(p.Brand))
                .Select(p => p.Brand)
                .Distinct()
                .OrderBy(b => b)
                .ToListAsync();

            return Ok(brands);
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
