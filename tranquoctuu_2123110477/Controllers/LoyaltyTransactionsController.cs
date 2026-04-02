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

        // GỘP 2 GET THÀNH 1
        // GET: api/LoyaltyTransactions
        // GET: api/LoyaltyTransactions/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetLoyaltyTransactions(int? id)
        {
            if (_context.LoyaltyTransactions == null)
                return NotFound();

            var query = _context.LoyaltyTransactions
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer);

            // Trường hợp 1: Lấy chi tiết theo ID
            if (id.HasValue)
            {
                var data = await query.FirstOrDefaultAsync(x => x.Id == id.Value);

                if (data == null)
                    return NotFound($"Không tìm thấy giao dịch Id = {id.Value}");

                return Ok(data);
            }

            // Trường hợp 2: Lấy danh sách giao dịch mới nhất lên đầu
            var list = await query.OrderByDescending(x => x.CreatedAt).ToListAsync();

            return Ok(list);
        }

        // POST: api/LoyaltyTransactions
        [HttpPost]
        public async Task<ActionResult<LoyaltyTransaction>> Create(LoyaltyTransaction model)
        {
            if (model == null) return BadRequest("Dữ liệu không hợp lệ");

            // LOGIC BLOCKCHAIN: Lấy giao dịch cuối cùng để lấy Hash cũ nối chuỗi
            var last = await _context.LoyaltyTransactions
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            model.PreviousHash = last?.Hash ?? "0";
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.IsDeleted = false;

            // Tính toán mã Hash cho giao dịch mới (Yêu cầu hàm CalculateHash đã có trong Model)
            model.Hash = model.CalculateHash();

            _context.LoyaltyTransactions.Add(model);
            await _context.SaveChangesAsync();

            // Trỏ về GetLoyaltyTransactions (hàm đã gộp)
            return CreatedAtAction(nameof(GetLoyaltyTransactions), new { id = model.Id }, model);
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

        private bool Exists(int id)
        {
            return _context.LoyaltyTransactions.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}