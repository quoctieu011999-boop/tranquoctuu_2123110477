using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env; // Tiêm vào để lấy đường dẫn thư mục wwwroot
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var list = await _context.Products
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            return Ok(list);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromForm] Product model, IFormFile? imageFile)
        {
            if (model == null) return BadRequest("Dữ liệu không hợp lệ");

            // Xử lý Upload Ảnh
            if (imageFile != null)
            {
                model.Image = await SaveImage(imageFile);
            }

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.IsDeleted = false;

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Product model, IFormFile? imageFile)
        {
            if (id != model.Id) return BadRequest("Id không trùng khớp");

            var existing = await _context.Products.FindAsync(id);
            if (existing == null || existing.IsDeleted) return NotFound();

            // Cập nhật thông tin cơ bản
            existing.Name = model.Name;
            existing.Price = model.Price;
            existing.Stock = model.Stock;
            existing.Description = model.Description;
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

            // Nếu có ảnh mới thì xóa ảnh cũ và lưu ảnh mới
            if (imageFile != null)
            {
                DeleteImage(existing.Image); // Hàm xóa ảnh cũ
                existing.Image = await SaveImage(imageFile);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Products.FindAsync(id);
            if (data == null || data.IsDeleted) return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- Các hàm bổ trợ (Helper Methods) ---

        private async Task<string> SaveImage(IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string path = Path.Combine(_env.WebRootPath, "images/products");

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        private void DeleteImage(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            string filePath = Path.Combine(_env.WebRootPath, "images/products", fileName);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
        }

        private bool Exists(int id)
        {
            return _context.Products.Any(e => e.Id == id && !e.IsDeleted);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return data == null ? NotFound() : Ok(data);
        }
    }
}