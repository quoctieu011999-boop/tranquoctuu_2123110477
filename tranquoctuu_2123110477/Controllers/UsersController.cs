using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users hoặc api/Users/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetUsers(int? id)
        {
            var query = _context.Users.Where(x => !x.IsDeleted);

            if (id.HasValue)
            {
                var user = await query.FirstOrDefaultAsync(u => u.Id == id.Value);
                if (user == null) return NotFound("Không tìm thấy người dùng");
                return Ok(user);
            }

            return Ok(await query.ToListAsync());
        }

        // POST: api/Users (Tạo tài khoản)
        [HttpPost]
        public async Task<ActionResult<User>> Create(User model)
        {
            if (_context.Users.Any(u => u.Username == model.Username))
                return BadRequest("Tên đăng nhập đã tồn tại");

            model.CreatedAt = DateTime.Now;
            model.IsDeleted = false;

            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = model.Id }, model);
        }

        // DELETE: api/Users/5 (Xóa mềm hoặc Khóa tài khoản)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}