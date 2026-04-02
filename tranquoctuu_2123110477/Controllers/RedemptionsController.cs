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
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer)
                .Include(x => x.Reward)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Redemption>> GetRedemption(int id)
        {
            var data = await _context.Redemptions
                .Include(x => x.Customer)
                .Include(x => x.Reward)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
                return NotFound($"Không tìm thấy Redemption Id = {id}");

            return data;
        }

        
        [HttpPost]
        public async Task<ActionResult<Redemption>> Create(Redemption model)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == model.CustomerId);

            var reward = await _context.Rewards
                .FirstOrDefaultAsync(r => r.Id == model.RewardId);

            if (customer == null || reward == null)
                return BadRequest("Customer hoặc Reward không tồn tại");

            
            if (customer.Points < reward.PointCost)
                return BadRequest("Không đủ điểm để đổi quà");

            model.PointsUsed = reward.PointCost;
            model.Status = "Completed";
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";

          
            customer.Points -= reward.PointCost;
            customer.UpdatedAt = DateTime.Now;

            _context.Redemptions.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRedemption), new { id = model.Id }, model);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Redemption model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.Redemptions.FindAsync(id);

            if (existing == null || existing.IsDeleted)
                return NotFound();

            existing.Status = model.Status;

            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Redemptions.FindAsync(id);

            if (data == null || data.IsDeleted)
                return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(int id)
        {
            return _context.Redemptions.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}