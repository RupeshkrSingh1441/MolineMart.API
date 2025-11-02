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
    [Authorize(Roles ="Admin")]
    //[Authorize(Policy ="AdminOnly")]
    public class ProductController :ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
              _context = context;  
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll() 
        => await _context.Products.ToListAsync();


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? NotFound() : Ok(product);
        }


        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] Product product)
        //{
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(Get), new {id = product.Id}, product);
        //}

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductCreateDto productDto)
        {
            if (productDto.Image == null || productDto.Image.Length == 0)
                return BadRequest("Image is required.");

            var filePath = Path.Combine("wwwroot/images", productDto.Image.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await productDto.Image.CopyToAsync(stream);
            }

            var product = new Product
            {
                Brand = productDto.Brand,
                Model = productDto.Model,
                Description = productDto.Description,
                Price = productDto.Price,
                ImageUrl = $"/images/{productDto.Image.FileName}"
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product added successfully", product });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
