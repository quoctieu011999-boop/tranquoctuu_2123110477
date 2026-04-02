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
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

    
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int id)
        {
            var data = await _context.Notifications
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
                return NotFound($"Không tìm thấy Id = {id}");

            return data;
        }

       
        [HttpPost]
        public async Task<ActionResult<Notification>> Create(Notification model)
        {
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";

            _context.Notifications.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotification), new { id = model.Id }, model);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Notification model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.Notifications.FindAsync(id);
            if (existing == null || existing.IsDeleted)
                return NotFound();

            existing.Title = model.Title;
            existing.Message = model.Message;
            existing.CustomerId = model.CustomerId;

            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

    
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