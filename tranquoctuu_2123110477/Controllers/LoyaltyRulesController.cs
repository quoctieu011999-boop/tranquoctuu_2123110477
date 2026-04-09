using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyRulesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoyaltyRulesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoyaltyRule>>> GetLoyaltyRules()
        {
            // Chỉ lấy các quy tắc chưa bị xóa (nếu dùng soft delete)
            return await _context.LoyaltyRules.Where(r => !r.IsDeleted).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoyaltyRule>> GetLoyaltyRule(int id)
        {
            var loyaltyRule = await _context.LoyaltyRules.FindAsync(id);

            if (loyaltyRule == null || loyaltyRule.IsDeleted)
            {
                return NotFound($"Không tìm thấy quy tắc với Id = {id}");
            }

            return loyaltyRule;
        }

        [HttpPost]
        public async Task<ActionResult<LoyaltyRule>> PostLoyaltyRule(LoyaltyRule model)
        {
            // SỬA LỖI: Sử dụng 'model' đồng nhất với tham số đầu vào
            _context.LoyaltyRules.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoyaltyRule), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoyaltyRule(int id, LoyaltyRule loyaltyRule)
        {
            if (id != loyaltyRule.Id)
            {
                return BadRequest("Id không khớp.");
            }

            _context.Entry(loyaltyRule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoyaltyRuleExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoyaltyRule(int id)
        {
            var loyaltyRule = await _context.LoyaltyRules.FindAsync(id);
            if (loyaltyRule == null) return NotFound();

            // SỬA LỖI: Phải thực thi lệnh xóa (ở đây dùng Soft Delete)
            loyaltyRule.IsDeleted = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoyaltyRuleExists(int id)
        {
            return _context.LoyaltyRules.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}