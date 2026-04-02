using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedemptionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RedemptionsController(AppDbContext context)
        {
            _context = context;
        }

        // GỘP 2 GET THÀNH 1
        // GET: api/Redemptions
        // GET: api/Redemptions/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetRedemptions(int? id)
        {
            if (_context.Redemptions == null)
                return NotFound();

            // Thiết lập query chung bao gồm thông tin Khách hàng và Phần thưởng
            var query = _context.Redemptions
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer)
                .Include(x => x.Reward);

            // Trường hợp 1: Lấy chi tiết lịch sử đổi quà theo ID
            if (id.HasValue)
            {
                var data = await query.FirstOrDefaultAsync(x => x.Id == id.Value);

                if (data == null)
                    return NotFound($"Không tìm thấy bản ghi đổi quà Id = {id.Value}");

                return Ok(data);
            }

            // Trường hợp 2: Lấy toàn bộ danh sách đổi quà mới nhất lên đầu
            var list = await query.OrderByDescending(x => x.CreatedAt).ToListAsync();

            return Ok(list);
        }

        // POST: api/Redemptions
        [HttpPost]
        public async Task<ActionResult<Redemption>> Create(Redemption model)
        {
            if (model == null) return BadRequest("Dữ liệu không hợp lệ");

            // Kiểm tra sự tồn tại của Customer và Reward
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == model.CustomerId && !c.IsDeleted);

            var reward = await _context.Rewards
                .FirstOrDefaultAsync(r => r.Id == model.RewardId && !r.IsDeleted);

            if (customer == null || reward == null)
                return BadRequest("Khách hàng hoặc Phần thưởng không tồn tại");

            // LOGIC NGHIỆP VỤ: Kiểm tra quỹ điểm của khách hàng
            if (customer.Points < reward.PointCost)
                return BadRequest("Số dư điểm không đủ để thực hiện đổi phần thưởng này");

            // Thiết lập thông tin giao dịch đổi quà
            model.PointsUsed = reward.PointCost;
            model.Status = "Completed";
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.IsDeleted = false;

            // Khấu trừ điểm trực tiếp của khách hàng
            customer.Points -= reward.PointCost;
            customer.UpdatedAt = DateTime.Now;

            _context.Redemptions.Add(model);
            await _context.SaveChangesAsync();

            // Trỏ về GetRedemptions (hàm đã gộp)
            return CreatedAtAction(nameof(GetRedemptions), new { id = model.Id }, model);
        }

        // PUT: api/Redemptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Redemption model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.Redemptions.FindAsync(id);

            if (existing == null || existing.IsDeleted)
                return NotFound();

            // Thường chỉ cập nhật trạng thái (ví dụ: Chờ xử lý -> Đã giao quà)
            existing.Status = model.Status;
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Redemptions/5 (Xóa mềm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Redemptions.FindAsync(id);

            if (data == null || data.IsDeleted)
                return NotFound();

            // Thực hiện xóa mềm
            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(int id)
        {
            return _context.Redemptions.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}