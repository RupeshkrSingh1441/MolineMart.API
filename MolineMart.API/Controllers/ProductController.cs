using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MolineMart.API.Data;
using MolineMart.API.Dto;
using MolineMart.API.Models;

namespace MolineMart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    //[Authorize(Policy ="AdminOnly")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var result = await _context.Products.ToListAsync();
            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return new ProductDto
            {
                Id = product.Id,
                Brand = product.Brand,
                Model = product.Model,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category,
                Color = product.Color,
                Storage = product.Storage,
                Warranty = product.Warranty,
                ImageUrl = product.ImageUrl
            };
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(ProductDto dto)
        {
            var product = new Product
            {
                Brand = dto.Brand,
                Model = dto.Model,
                Price = dto.Price,
                Description = dto.Description,
                Category = dto.Category,
                Color = dto.Color,
                Storage = dto.Storage,
                Warranty = dto.Warranty,
                ImageUrl = dto.ImageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product.Id);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddProduct([FromForm] ProductDto productDto)
        //{
        //    if (productDto.Image == null || productDto.Image.Length == 0)
        //        return BadRequest("Image is required.");

        //    var filePath = Path.Combine("wwwroot/images", productDto.Image.FileName);
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await productDto.Image.CopyToAsync(stream);
        //    }

        //    var product = new Product
        //    {
        //        Brand = productDto.Brand,
        //        Model = productDto.Model,
        //        Description = productDto.Description,
        //        Price = productDto.Price,
        //        ImageUrl = $"/images/{productDto.Image.FileName}"
        //    };

        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { message = "Product added successfully", product });
        //}


        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, ProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Brand = dto.Brand;
            product.Model = dto.Model;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.Category = dto.Category;
            product.Color = dto.Color;
            product.Storage = dto.Storage;
            product.Warranty = dto.Warranty;
            product.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Route("image")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            // Ensure folder exists
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative URL
            var urlPath = $"/images/{fileName}";
            return Ok(new { imageUrl = urlPath });
        }

        // GET: api/product/search-suggestions?q=iphone
        [HttpGet("search-suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Ok(new List<string>());

            q = q.ToLower();

            var suggestions = await _context.Products
                .Where(p => p.Brand.ToLower().Contains(q)
                         || p.Model.ToLower().Contains(q))
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Brand + " " + p.Model,
                    image = p.ImageUrl
                })
                .Take(10)
                .ToListAsync();

            return Ok(suggestions);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Ok(new List<Product>());

            q = q.ToLower();

            var list = await _context.Products
                .Where(p => p.Brand.ToLower().Contains(q)
                         || p.Model.ToLower().Contains(q)
                         || p.Description.ToLower().Contains(q))
                .ToListAsync();

            return Ok(list);
        }

    }
}
