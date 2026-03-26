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
                                 .Include(ci => ci.Customer)
                                 .ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerInteraction>> GetCustomerInteraction(int id)
        {
            var interaction = await _context.CustomerInteractions
                                            .Include(ci => ci.Customer)
                                            .FirstOrDefaultAsync(ci => ci.Id == id);

            if (interaction == null)
            {
                return NotFound($"Không tìm thấy tương tác với Id = {id}");
            }

            return interaction;
        }

       
        [HttpPost]
        public async Task<ActionResult<CustomerInteraction>> PostCustomerInteraction(CustomerInteraction interaction)
        {
            _context.CustomerInteractions.Add(interaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerInteraction), new { id = interaction.Id }, interaction);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerInteraction(int id, CustomerInteraction interaction)
        {
            if (id != interaction.Id)
            {
                return BadRequest("Id không khớp.");
            }

            _context.Entry(interaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InteractionExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerInteraction(int id)
        {
            var interaction = await _context.CustomerInteractions.FindAsync(id);
            if (interaction == null) return NotFound();

            _context.CustomerInteractions.Remove(interaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InteractionExists(int id)
        {
            return _context.CustomerInteractions.Any(e => e.Id == id);
        }
    }
}