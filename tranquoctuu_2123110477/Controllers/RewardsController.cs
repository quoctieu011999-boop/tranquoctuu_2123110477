using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RewardsController(AppDbContext context)
        {
            _context = context;
        }

        // GỘP 2 GET THÀNH 1
        // GET: api/Rewards
        // GET: api/Rewards/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetRewards(int? id)
        {
            if (_context.Rewards == null)
                return NotFound();

            // Trường hợp 1: Lấy chi tiết phần thưởng theo ID
            if (id.HasValue)
            {
                var data = await _context.Rewards
                    .FirstOrDefaultAsync(x => x.Id == id.Value && !x.IsDeleted);

                if (data == null)
                    return NotFound($"Không tìm thấy phần thưởng Id = {id.Value}");

                return Ok(data);
            }

            // Trường hợp 2: Lấy toàn bộ danh sách phần thưởng chưa xóa, mới nhất lên đầu
            var list = await _context.Rewards
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/Rewards
        [HttpPost]
        public async Task<ActionResult<Reward>> Create(Reward model)
        {
            if (model == null) return BadRequest("Dữ liệu không hợp lệ");

            // Kiểm tra các điều kiện nghiệp vụ cơ bản
            if (model.PointCost <= 0)
                return BadRequest("Giá trị điểm đổi (PointCost) phải lớn hơn 0");

            if (model.Quantity < 0)
                return BadRequest("Số lượng phần thưởng không được âm");

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.IsDeleted = false;

            _context.Rewards.Add(model);
            await _context.SaveChangesAsync();

            // Trỏ về GetRewards (hàm đã gộp)
            return CreatedAtAction(nameof(GetRewards), new { id = model.Id }, model);
        }

        // PUT: api/Rewards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Reward model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.Rewards.FindAsync(id);

            if (existing == null || existing.IsDeleted)
                return NotFound();

            // Kiểm tra lại tính hợp lệ của dữ liệu cập nhật
            if (model.PointCost <= 0)
                return BadRequest("PointCost phải lớn hơn 0");

            // Cập nhật các trường thông tin
            existing.Name = model.Name;
            existing.Description = model.Description;
            existing.PointCost = model.PointCost;
            existing.Quantity = model.Quantity;

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

        // DELETE: api/Rewards/5 (Xóa mềm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Rewards.FindAsync(id);

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
            return _context.Rewards.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}