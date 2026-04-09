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
                                 .Where(r => !r.IsDeleted) // Chỉ lấy bản ghi chưa xóa
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Redemption>> GetRedemption(int id)
        {
            var redemption = await _context.Redemptions
                                           .Include(r => r.Customer)
                                           .Include(r => r.Reward)
                                           .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (redemption == null)
            {
                return NotFound($"Không tìm thấy giao dịch đổi quà với Id = {id}");
            }

            return redemption;
        }

        [HttpPost]
        public async Task<ActionResult<Redemption>> Create(Redemption model)
        {
            // SỬA LỖI: Sử dụng 'model' thay vì 'redemption'
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == model.CustomerId);
            var rewardExists = await _context.Rewards.AnyAsync(rw => rw.Id == model.RewardId);

            if (!customerExists || !rewardExists)
            {
                return BadRequest("CustomerId hoặc RewardId không hợp lệ.");
            }

            model.RedeemedAt = DateTime.Now; // Đảm bảo ghi nhận thời gian đổi quà
            model.IsDeleted = false;

            _context.Redemptions.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRedemption), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Redemption model)
        {
            if (id != model.Id) return BadRequest("Id không khớp");

            _context.Entry(model).State = EntityState.Modified;

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

            // SỬA LỖI: Thực hiện xóa mềm (Soft Delete)
            redemption.IsDeleted = true;
            redemption.DeletedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RedemptionExists(int id)
        {
            // SỬA LỖI: Khớp tên hàm với phần Update
            return _context.Redemptions.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}