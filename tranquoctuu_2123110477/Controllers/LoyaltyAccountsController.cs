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
                                 .Include(la => la.Customer)
                                 .ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<LoyaltyAccount>> GetLoyaltyAccount(int id)
        {
            var loyaltyAccount = await _context.LoyaltyAccounts
                                               .Include(la => la.Customer)
                                               .FirstOrDefaultAsync(la => la.Id == id);

            if (loyaltyAccount == null)
            {
                return NotFound($"Không tìm thấy tài khoản loyalty với Id = {id}");
            }

            return loyaltyAccount;
        }

        
        [HttpPost]
        public async Task<ActionResult<LoyaltyAccount>> PostLoyaltyAccount(LoyaltyAccount loyaltyAccount)
        {
            
            loyaltyAccount.UpdatedAt = DateTime.Now;

            _context.LoyaltyAccounts.Add(loyaltyAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoyaltyAccount), new { id = loyaltyAccount.Id }, loyaltyAccount);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoyaltyAccount(int id, LoyaltyAccount loyaltyAccount)
        {
            if (id != loyaltyAccount.Id)
            {
                return BadRequest("Id không trùng khớp.");
            }

            
            loyaltyAccount.UpdatedAt = DateTime.Now;
            _context.Entry(loyaltyAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoyaltyAccountExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoyaltyAccount(int id)
        {
            var loyaltyAccount = await _context.LoyaltyAccounts.FindAsync(id);
            if (loyaltyAccount == null) return NotFound();

            _context.LoyaltyAccounts.Remove(loyaltyAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoyaltyAccountExists(int id)
        {
            return _context.LoyaltyAccounts.Any(e => e.Id == id);
        }
    }
}