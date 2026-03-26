using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyTransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoyaltyTransactionsController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoyaltyTransaction>>> GetLoyaltyTransactions()
        {
            return await _context.LoyaltyTransactions
                                 .Include(lt => lt.Customer)
                                 .OrderByDescending(lt => lt.CreatedAt)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoyaltyTransaction>> GetLoyaltyTransaction(int id)
        {
            var transaction = await _context.LoyaltyTransactions
                                            .Include(lt => lt.Customer)
                                            .FirstOrDefaultAsync(lt => lt.Id == id);

            if (transaction == null)
            {
                return NotFound($"Không tìm thấy giao dịch với Id = {id}");
            }

            return transaction;
        }

       
        [HttpPost]
        public async Task<ActionResult<LoyaltyTransaction>> PostLoyaltyTransaction(LoyaltyTransaction transaction)
        {
          
            var lastTransaction = await _context.LoyaltyTransactions
                                                .OrderByDescending(t => t.Id)
                                                .FirstOrDefaultAsync();

            transaction.PreviousHash = lastTransaction?.Hash ?? "0";
            transaction.CreatedAt = DateTime.Now;

            _context.LoyaltyTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoyaltyTransaction), new { id = transaction.Id }, transaction);
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoyaltyTransaction(int id)
        {
            var transaction = await _context.LoyaltyTransactions.FindAsync(id);
            if (transaction == null) return NotFound();

            _context.LoyaltyTransactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoyaltyTransactionExists(int id)
        {
            return _context.LoyaltyTransactions.Any(e => e.Id == id);
        }
    }
}