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

        // GỘP 2 GET THÀNH 1
        // GET: api/LoyaltyAccounts
        // GET: api/LoyaltyAccounts/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetLoyaltyAccounts(int? id)
        {
            if (_context.LoyaltyAccounts == null)
                return NotFound();

            // Thiết lập truy vấn cơ bản kèm dữ liệu Customer
            var query = _context.LoyaltyAccounts
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer);

            // Trường hợp 1: Lấy chi tiết theo ID
            if (id.HasValue)
            {
                var data = await query.FirstOrDefaultAsync(x => x.Id == id.Value);

                if (data == null)
                    return NotFound("Không tìm thấy tài khoản loyalty");

                return Ok(data);
            }

            // Trường hợp 2: Lấy toàn bộ danh sách chưa xóa
            var list = await query.ToListAsync();

            return Ok(list);
        }

        // POST: api/LoyaltyAccounts
        [HttpPost]
        public async Task<ActionResult<LoyaltyAccount>> PostLoyaltyAccount(LoyaltyAccount model)
        {
            if (model == null) return BadRequest();

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = "admin";
            model.IsDeleted = false;

            _context.LoyaltyAccounts.Add(model);
            await _context.SaveChangesAsync();

            // Trỏ về hàm GetLoyaltyAccounts (đã gộp)
            return CreatedAtAction(nameof(GetLoyaltyAccounts), new { id = model.Id }, model);
        }

        // PUT: api/LoyaltyAccounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoyaltyAccount(int id, LoyaltyAccount model)
        {
            if (id != model.Id)
                return BadRequest("Id không trùng");

            var existing = await _context.LoyaltyAccounts.FindAsync(id);
            if (existing == null || existing.IsDeleted)
                return NotFound();

            // Cập nhật thông tin từ model truyền vào
            existing.CustomerId = model.CustomerId;
            existing.Points = model.Points;
            existing.Level = model.Level;

            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

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

        // DELETE: api/LoyaltyAccounts/5 (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoyaltyAccount(int id)
        {
            var data = await _context.LoyaltyAccounts.FindAsync(id);
            if (data == null || data.IsDeleted)
                return NotFound();

            // Thực hiện xóa mềm
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