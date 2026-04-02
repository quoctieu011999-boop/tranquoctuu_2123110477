using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerChannelsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerChannelsController(AppDbContext context)
        {
            _context = context;
        }

        // GỘP 2 GET THÀNH 1
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetCustomerChannels(int? id)
        {
            if (_context.CustomerChannels == null) return NotFound();

            var query = _context.CustomerChannels.Where(x => !x.IsDeleted);

            if (id.HasValue)
            {
                var data = await query.FirstOrDefaultAsync(x => x.Id == id.Value);
                if (data == null) return NotFound($"Không tìm thấy Channel Id = {id.Value}");
                return Ok(data);
            }

            return Ok(await query.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<CustomerChannel>> Create(CustomerChannel model)
        {
            model.CreatedAt = DateTime.Now;
            model.IsDeleted = false;
            _context.CustomerChannels.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomerChannels), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CustomerChannel model)
        {
            if (id != model.Id) return BadRequest();
            var existing = await _context.CustomerChannels.FindAsync(id);
            if (existing == null || existing.IsDeleted) return NotFound();

            existing.ChannelType = model.ChannelType; 
            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.CustomerChannels.FindAsync(id);
            if (data == null || data.IsDeleted) return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}