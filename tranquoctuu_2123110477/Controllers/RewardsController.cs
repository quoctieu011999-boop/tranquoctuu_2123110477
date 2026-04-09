using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RewardsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reward>>> GetRewards()
        {
            // Chỉ lấy quà tặng chưa bị xóa
            return await _context.Rewards.Where(r => !r.IsDeleted).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reward>> GetReward(int id)
        {
            var reward = await _context.Rewards.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (reward == null)
            {
                return NotFound($"Không tìm thấy quà tặng với Id = {id}");
            }

            return reward;
        }

        [HttpPost]
        public async Task<ActionResult<Reward>> PostReward(Reward reward)
        {
            reward.CreatedAt = DateTime.Now;
            reward.IsDeleted = false;

            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReward), new { id = reward.Id }, reward);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Reward model)
        {
            // SỬA LỖI: Kiểm tra id với model.Id
            if (id != model.Id) return BadRequest("Id không khớp");

            // SỬA LỖI: Thay 'existing' bằng 'model' và cập nhật trạng thái
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = "admin";

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RewardExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reward = await _context.Rewards.FindAsync(id);
            if (reward == null) return NotFound();

            // SỬA LỖI: Thực hiện xóa mềm
            reward.IsDeleted = true;
            reward.DeletedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RewardExists(int id)
        {
            // SỬA LỖI: Khớp tên hàm với phần Update và kiểm tra IsDeleted
            return _context.Rewards.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}