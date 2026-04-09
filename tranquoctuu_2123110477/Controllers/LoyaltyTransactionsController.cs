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
        public async Task<ActionResult<LoyaltyTransaction>> Create(LoyaltyTransaction model)
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

        // PUT: api/LoyaltyTransactions/5 (Chặn chỉnh sửa để đảm bảo tính minh bạch)
        [HttpPut("{id}")]
        public IActionResult BlockUpdate()
        {
            return BadRequest("Nguyên tắc bất biến: Không được phép sửa đổi lịch sử giao dịch!");
        }

        // DELETE: api/LoyaltyTransactions/5 (Xóa mềm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var data = await _context.LoyaltyTransactions.FindAsync(id);
            if (data == null || data.IsDeleted)
                return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // API KIỂM TRA TÍNH TOÀN VẸN (VERIFY DATA)
        // GET: api/LoyaltyTransactions/verify
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyChain()
        {
            // Lấy toàn bộ chuỗi giao dịch theo thứ tự thời gian
            var list = await _context.LoyaltyTransactions
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .ToListAsync();

            for (int i = 1; i < list.Count; i++)
            {
                var current = list[i];
                var previous = list[i - 1];

                // 1. Kiểm tra liên kết PreviousHash
                if (current.PreviousHash != previous.Hash)
                    return BadRequest($"Chuỗi dữ liệu bị đứt gãy tại Id = {current.Id}");

                // 2. Kiểm tra Hash nội tại (Xác nhận dữ liệu không bị sửa lén trong DB)
                if (current.Hash != current.CalculateHash())
                    return BadRequest($"Dữ liệu tại Id = {current.Id} không khớp với mã Hash (có dấu hiệu bị tác động bên ngoài)");
            }

            return Ok("Hệ thống dữ liệu an toàn và nhất quán ✅");
        }

        private bool LoyaltyTransactionExists(int id)
        {
            return _context.LoyaltyTransactions.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}