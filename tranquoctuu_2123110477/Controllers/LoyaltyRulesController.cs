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
            return await _context.LoyaltyRules.ToListAsync();
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<LoyaltyRule>> GetLoyaltyRule(int id)
        {
            var loyaltyRule = await _context.LoyaltyRules.FindAsync(id);

            if (loyaltyRule == null)
            {
                return NotFound($"Không tìm thấy quy tắc với Id = {id}");
            }

            return loyaltyRule;
        }

        // POST: api/LoyaltyRules
        [HttpPost]
        public async Task<ActionResult<LoyaltyRule>> PostLoyaltyRule(LoyaltyRule model)
        {
            _context.LoyaltyRules.Add(loyaltyRule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoyaltyRule), new { id = loyaltyRule.Id }, loyaltyRule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoyaltyRule(int id, LoyaltyRule loyaltyRule)
        {
            if (id != loyaltyRule.Id)
            {
                return BadRequest("Id không khớp.");
            }

            _context.Entry(loyaltyRule).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoyaltyRule(int id)
        {
            var loyaltyRule = await _context.LoyaltyRules.FindAsync(id);
            if (loyaltyRule == null) return NotFound();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoyaltyRuleExists(int id)
        {
            return _context.LoyaltyRules.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}