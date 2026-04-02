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
            return await _context.LoyaltyRules
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<LoyaltyRule>> GetLoyaltyRule(int id)
        {
            var data = await _context.LoyaltyRules
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
                return NotFound($"Không tìm thấy rule Id = {id}");

            return data;
        }

        [HttpPost]
        public async Task<ActionResult<LoyaltyRule>> PostLoyaltyRule(LoyaltyRule model)
        {
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";

            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = "admin";

            _context.LoyaltyRules.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoyaltyRule), new { id = model.Id }, model);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoyaltyRule(int id, LoyaltyRule model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.LoyaltyRules.FindAsync(id);
            if (existing == null || existing.IsDeleted)
                return NotFound();

          
            existing.RuleName = model.RuleName;
            existing.Description = model.Description;
            existing.Points = model.Points;
            existing.ActionType = model.ActionType;

           
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoyaltyRule(int id)
        {
            var data = await _context.LoyaltyRules.FindAsync(id);
            if (data == null || data.IsDeleted)
                return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoyaltyRuleExists(int id)
        {
            return _context.LoyaltyRules.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}