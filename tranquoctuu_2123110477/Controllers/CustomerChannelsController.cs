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

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerChannel>>> GetCustomerChannels()
        {
            return await _context.CustomerChannels
                                 .Include(x => x.Customer)
                                 .ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerChannel>> GetCustomerChannel(int id)
        {
            var data = await _context.CustomerChannels
                                     .Include(x => x.Customer)
                                     .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
                return NotFound($"Không tìm thấy Id = {id}");

            return data;
        }

        
        [HttpPost]
        public async Task<ActionResult<CustomerChannel>> PostCustomerChannel(CustomerChannel model)
        {
            if (model == null)
                return BadRequest("Dữ liệu không hợp lệ");

            _context.CustomerChannels.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerChannel), new { id = model.Id }, model);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerChannel(int id, CustomerChannel model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.CustomerChannels.FindAsync(id);
            if (existing == null)
                return NotFound();


            existing.ChannelType = model.ChannelType;
            existing.CustomerId = model.CustomerId;
           

            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerChannel(int id)
        {
            var data = await _context.CustomerChannels.FindAsync(id);

            if (data == null)
                return NotFound();

            _context.CustomerChannels.Remove(data);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerChannelExists(int id)
        {
            return _context.CustomerChannels.Any(e => e.Id == id);
        }
    }
}