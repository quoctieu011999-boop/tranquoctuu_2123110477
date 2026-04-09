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

        // Tách biệt Get All và Get By Id để Swagger hiển thị rõ ràng hơn
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            return await _context.Users.Where(x => !x.IsDeleted).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (user == null) return NotFound("Không tìm thấy người dùng");

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<Users>> Create(Users model)
        {
            // Kiểm tra trùng tên đăng nhập
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                return BadRequest("Tên đăng nhập đã tồn tại");

            model.CreatedAt = DateTime.Now;
            model.IsDeleted = false;

            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            // Redirect về hàm GetUserById để lấy thông tin user vừa tạo
            return CreatedAtAction(nameof(GetUserById), new { id = model.Id }, model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.IsDeleted) return NotFound();

            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}