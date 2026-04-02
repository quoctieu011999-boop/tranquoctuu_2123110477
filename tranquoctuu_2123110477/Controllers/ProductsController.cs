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

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GỘP 2 GET THÀNH 1
        // GET: api/Products
        // GET: api/Products/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetProducts(int? id)
        {
            if (_context.Products == null)
                return NotFound();

            // Trường hợp 1: Lấy chi tiết sản phẩm theo ID
            if (id.HasValue)
            {
                var data = await _context.Products
                    .FirstOrDefaultAsync(x => x.Id == id.Value && !x.IsDeleted);

                if (data == null)
                    return NotFound($"Không tìm thấy sản phẩm với Id = {id.Value}");

                return Ok(data);
            }

            // Trường hợp 2: Lấy toàn bộ danh sách sản phẩm chưa xóa, mới nhất lên đầu
            var list = await _context.Products
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product model)
        {
            if (model == null) return BadRequest("Dữ liệu không hợp lệ");

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.IsDeleted = false;

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            // Trỏ về GetProducts (hàm đã gộp)
            return CreatedAtAction(nameof(GetProducts), new { id = model.Id }, model);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.Products.FindAsync(id);

            if (existing == null || existing.IsDeleted)
                return NotFound();

            // Cập nhật các trường thông tin
            existing.Name = model.Name;
            existing.Price = model.Price;
            existing.Stock = model.Stock;
            existing.Description = model.Description;

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

        // DELETE: api/Products/5 (Xóa mềm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Products.FindAsync(id);

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
            return _context.Products.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}