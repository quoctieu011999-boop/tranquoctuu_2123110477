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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            return await _context.Notifications
                                 .Include(n => n.Customer)
                                 .Where(n => !n.IsDeleted) // Chỉ lấy các thông báo chưa xóa
                                 .OrderByDescending(n => n.CreatedAt)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int id)
        {
            var notification = await _context.Notifications
                                             .Include(n => n.Customer)
                                             .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);

            if (notification == null)
            {
                return NotFound($"Không tìm thấy thông báo với Id = {id}");
            }

            return notification;
        }

        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(Notification notification)
        {
            notification.CreatedAt = DateTime.Now;
            notification.IsDeleted = false; // Đảm bảo trạng thái mặc định

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Sửa lỗi: Trả về CreatedAtAction thay vì NoContent để đúng chuẩn RESTful
            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
        }

        [HttpPatch("{id}/mark-as-sent")]
        public async Task<IActionResult> MarkAsSent(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null || notification.IsDeleted) return NotFound();

            // SỬA LỖI: Phải gán giá trị thay đổi trước khi Save
            notification.IsSent = true;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Notifications.FindAsync(id);
            if (data == null || data.IsDeleted)
                return NotFound();

            // Logic Soft Delete
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