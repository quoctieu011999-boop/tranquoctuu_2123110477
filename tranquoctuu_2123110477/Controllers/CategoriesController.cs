using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context) => _context = context;

        // GET: api/Categories hoặc api/Categories/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetCategories(int? id)
        {
            if (id.HasValue)
            {
                var data = await _context.Categories.FindAsync(id.Value);
                if (data == null) return NotFound("Không tìm thấy danh mục");
                return Ok(data);
            }
            return Ok(await _context.Categories.ToListAsync());
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> Post(Category model)
        {
            _context.Categories.Add(model);
            await _context.SaveChangesAsync(); 
            return CreatedAtAction(nameof(GetCategories), new { id = model.Id }, model);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Category model)
        {
            if (id != model.Id) return BadRequest("Id không khớp");

            var existing = await _context.Categories.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = model.Name;
            existing.Description = model.Description;

            await _context.SaveChangesAsync(); 
            return NoContent();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Categories.FindAsync(id);
            if (data == null) return NotFound();

            _context.Categories.Remove(data); 
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}