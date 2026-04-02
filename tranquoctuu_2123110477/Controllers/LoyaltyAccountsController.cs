using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyAccountsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoyaltyAccountsController(AppDbContext context)
        {
            _context = context;
        }

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoyaltyAccount>>> GetLoyaltyAccounts()
        {
            return await _context.LoyaltyAccounts
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer)
                .ToListAsync();
        }

  
        [HttpGet("{id}")]
        public async Task<ActionResult<LoyaltyAccount>> GetLoyaltyAccount(int id)
        {
            var data = await _context.LoyaltyAccounts
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
                return NotFound("Không tìm thấy");

            return data;
        }

    
        [HttpPost]
        public async Task<ActionResult<LoyaltyAccount>> PostLoyaltyAccount(LoyaltyAccount model)
        {
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";

            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = "admin";

            _context.LoyaltyAccounts.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoyaltyAccount), new { id = model.Id }, model);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoyaltyAccount(int id, LoyaltyAccount model)
        {
            if (id != model.Id)
                return BadRequest("Id không trùng");

            var existing = await _context.LoyaltyAccounts.FindAsync(id);
            if (existing == null || existing.IsDeleted)
                return NotFound();

           
            existing.CustomerId = model.CustomerId;
            existing.Points = model.Points;
            existing.Level = model.Level;

        
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoyaltyAccount(int id)
        {
            var data = await _context.LoyaltyAccounts.FindAsync(id);
            if (data == null || data.IsDeleted)
                return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoyaltyAccountExists(int id)
        {
            return _context.LoyaltyAccounts.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}