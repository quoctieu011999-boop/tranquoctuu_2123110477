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

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerInteraction>>> GetCustomerInteractions()
        {
            return await _context.CustomerInteractions
                                 .Include(x => x.Customer)
                                 .ToListAsync();
        }

   
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerInteraction>> GetCustomerInteraction(int id)
        {
            var data = await _context.CustomerInteractions
                                     .Include(x => x.Customer)
                                     .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
                return NotFound($"Không tìm thấy Id = {id}");

            return data;
        }

       
        [HttpPost]
        public async Task<ActionResult<CustomerInteraction>> PostCustomerInteraction(CustomerInteraction model)
        {
            if (model == null)
                return BadRequest("Dữ liệu không hợp lệ");

            _context.CustomerInteractions.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerInteraction), new { id = model.Id }, model);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerInteraction(int id, CustomerInteraction model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.CustomerInteractions.FindAsync(id);
            if (existing == null)
                return NotFound();


            existing.CustomerId = model.CustomerId;
            existing.InteractionType = model.InteractionType;
            existing.Content = model.Content;
            existing.CreatedAt = model.CreatedAt;

            await _context.SaveChangesAsync();

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerInteraction(int id)
        {
            var data = await _context.CustomerInteractions.FindAsync(id);

            if (data == null)
                return NotFound();

            _context.CustomerInteractions.Remove(data);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InteractionExists(int id)
        {
            return _context.CustomerInteractions.Any(e => e.Id == id);
        }
    }
}