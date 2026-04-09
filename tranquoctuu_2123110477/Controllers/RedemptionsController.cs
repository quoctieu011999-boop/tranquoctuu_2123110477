using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedemptionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RedemptionsController(AppDbContext context)
        {
            _context = context;
        }

      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Redemption>>> GetRedemptions()
        {
            return await _context.Redemptions
                                 .Include(r => r.Customer)
                                 .Include(r => r.Reward)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Redemption>> GetRedemption(int id)
        {
            var redemption = await _context.Redemptions
                                           .Include(r => r.Customer)
                                           .Include(r => r.Reward)
                                           .FirstOrDefaultAsync(r => r.Id == id);

            if (redemption == null)
            {
                return NotFound($"Không tìm thấy giao dịch đổi quà với Id = {id}");
            }

            return redemption;
        }

      
        [HttpPost]
        public async Task<ActionResult<Redemption>> Create(Redemption model)
        {
            
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == redemption.CustomerId);
            var rewardExists = await _context.Rewards.AnyAsync(rw => rw.Id == redemption.RewardId);

            if (!customerExists || !rewardExists)
            {
                return BadRequest("CustomerId hoặc RewardId không hợp lệ.");
            }

            _context.Redemptions.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRedemption), new { id = redemption.Id }, redemption);
        }

    
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Redemption model)
        {
            if (id != redemption.Id) return BadRequest();

            _context.Entry(redemption).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RedemptionExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var redemption = await _context.Redemptions.FindAsync(id);
            if (redemption == null) return NotFound();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(int id)
        {
            return _context.Redemptions.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}