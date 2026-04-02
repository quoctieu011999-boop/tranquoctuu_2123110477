using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInteractionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerInteractionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetInteractions(int? id)
        {
            var query = _context.CustomerInteractions
                .Include(x => x.Customer)
                .Where(x => !x.IsDeleted);

            if (id.HasValue)
            {
                var data = await query.FirstOrDefaultAsync(x => x.Id == id.Value);
                if (data == null) return NotFound();
                return Ok(data);
            }

            return Ok(await query.OrderByDescending(x => x.CreatedAt).ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<CustomerInteraction>> Create(CustomerInteraction model)
        {
            model.CreatedAt = DateTime.Now;
            _context.CustomerInteractions.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInteractions), new { id = model.Id }, model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.CustomerInteractions.FindAsync(id);
            if (data == null || data.IsDeleted) return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}