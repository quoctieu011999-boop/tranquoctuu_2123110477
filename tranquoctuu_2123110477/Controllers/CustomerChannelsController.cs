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
            
            return await _context.CustomerChannels
                                 .Include(cc => cc.Customer)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerChannel>> GetCustomerChannel(int id)
        {
            var customerChannel = await _context.CustomerChannels
                                                .Include(cc => cc.Customer)
                                                .FirstOrDefaultAsync(cc => cc.Id == id);

            if (customerChannel == null)
            {
                return NotFound($"Không tìm thấy kênh với Id = {id}");
            }

            return customerChannel;
        }

       
        [HttpPost]
        public async Task<ActionResult<CustomerChannel>> PostCustomerChannel(CustomerChannel customerChannel)
        {
            _context.CustomerChannels.Add(customerChannel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerChannel), new { id = customerChannel.Id }, customerChannel);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerChannel(int id, CustomerChannel customerChannel)
        {
            if (id != customerChannel.Id)
            {
                return BadRequest("Id không trùng khớp.");
            }

            _context.Entry(customerChannel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerChannelExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

    
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customerChannel = await _context.CustomerChannels.FindAsync(id);
            if (customerChannel == null) return NotFound();

            _context.CustomerChannels.Remove(customerChannel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}