using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificationsController(AppDbContext context)
        {
            _context = context;
        }

        // GỘP 2 GET THÀNH 1
        // GET: api/Notifications
        // GET: api/Notifications/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetNotifications(int? id)
        {
            if (_context.Notifications == null)
                return NotFound();

            // Trường hợp 1: Lấy chi tiết thông báo theo ID
            if (id.HasValue)
            {
                var data = await _context.Notifications
                    .Include(x => x.Customer)
                    .FirstOrDefaultAsync(x => x.Id == id.Value && !x.IsDeleted);

                if (data == null)
                    return NotFound($"Không tìm thấy thông báo Id = {id.Value}");

                return Ok(data);
            }

            // Trường hợp 2: Lấy danh sách thông báo, mới nhất hiện lên trước
            var list = await _context.Notifications
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/Notifications
        [HttpPost]
        public async Task<ActionResult<Notification>> Create(Notification model)
        {
            if (model == null) return BadRequest("Dữ liệu không hợp lệ");

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.IsDeleted = false;
            model.IsSent = false; // Mặc định chưa gửi khi mới tạo

            _context.Notifications.Add(model);
            await _context.SaveChangesAsync();

            // Trỏ về GetNotifications (hàm đã gộp)
            return CreatedAtAction(nameof(GetNotifications), new { id = model.Id }, model);
        }

        // PUT: api/Notifications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Notification model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.Notifications.FindAsync(id);
            if (existing == null || existing.IsDeleted)
                return NotFound();

            // Cập nhật các trường thông tin
            existing.Title = model.Title;
            existing.Message = model.Message;
            existing.CustomerId = model.CustomerId;

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

        // PATCH: api/Notifications/5/mark-as-sent
        [HttpPatch("{id}/mark-as-sent")]
        public async Task<IActionResult> MarkAsSent(int id)
        {
            var data = await _context.Notifications.FindAsync(id);
            if (data == null || data.IsDeleted)
                return NotFound();

            data.IsSent = true;
            data.UpdatedAt = DateTime.Now;
            data.UpdatedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Notifications/5 (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Notifications.FindAsync(id);
            if (data == null || data.IsDeleted)
                return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}