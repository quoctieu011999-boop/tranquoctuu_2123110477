using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var data = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
                return NotFound();

            return data;
        }


        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product model)
        {
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = model.Id }, model);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product model)
        {
            if (id != model.Id) return BadRequest();

            var existing = await _context.Products.FindAsync(id);

            if (existing == null || existing.IsDeleted)
                return NotFound();

            existing.Name = model.Name;
            existing.Price = model.Price;
            existing.Stock = model.Stock;
            existing.Description = model.Description;

            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Products.FindAsync(id);

            if (data == null || data.IsDeleted)
                return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}