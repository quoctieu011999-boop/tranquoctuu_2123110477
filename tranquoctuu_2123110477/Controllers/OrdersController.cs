using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GỘP 2 GET THÀNH 1
        // GET: api/Orders
        // GET: api/Orders/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetOrders(int? id)
        {
            if (_context.Orders == null)
                return NotFound();

            // Thiết lập query cơ bản với các liên kết bảng liên quan
            var query = _context.Orders
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer)
                .Include(x => x.OrderItems.Where(i => !i.IsDeleted))
                    .ThenInclude(i => i.Product); // Load thêm thông tin Product nếu cần

            // Trường hợp 1: Lấy chi tiết một đơn hàng theo ID
            if (id.HasValue)
            {
                var data = await query.FirstOrDefaultAsync(x => x.Id == id.Value);

                if (data == null)
                    return NotFound($"Không tìm thấy đơn hàng với Id = {id.Value}");

                return Ok(data);
            }

            // Trường hợp 2: Lấy toàn bộ danh sách đơn hàng mới nhất lên đầu
            var list = await query.OrderByDescending(x => x.CreatedAt).ToListAsync();

            return Ok(list);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> Create(Order model)
        {
            if (model == null) return BadRequest("Dữ liệu không hợp lệ");

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.Status = "Pending";
            model.IsDeleted = false;

            // Tính toán tổng tiền từ danh sách Item đính kèm
            if (model.OrderItems != null && model.OrderItems.Any())
            {
                model.TotalAmount = model.OrderItems
                    .Sum(i => i.Price * i.Quantity);

                foreach (var item in model.OrderItems)
                {
                    item.CreatedAt = DateTime.Now;
                    item.CreatedBy = "admin";
                    item.IsDeleted = false;
                }
            }
            else
            {
                model.TotalAmount = 0;
            }

            _context.Orders.Add(model);
            await _context.SaveChangesAsync();

            // Trỏ về GetOrders (hàm đã gộp)
            return CreatedAtAction(nameof(GetOrders), new { id = model.Id }, model);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null || existing.IsDeleted)
                return NotFound();

            // Cập nhật thông tin cơ bản
            existing.CustomerId = model.CustomerId;
            existing.Status = model.Status;
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

            // Xử lý cập nhật danh sách sản phẩm trong đơn (nếu có truyền lên)
            if (model.OrderItems != null)
            {
                // Đánh dấu xóa toàn bộ Item cũ
                foreach (var old in existing.OrderItems)
                {
                    old.IsDeleted = true;
                    old.DeletedAt = DateTime.Now;
                    old.DeletedBy = "admin";
                }

                // Gán danh sách mới
                existing.OrderItems = model.OrderItems;

                foreach (var item in existing.OrderItems)
                {
                    item.CreatedAt = DateTime.Now;
                    item.CreatedBy = "admin";
                }

                // Tính lại tổng tiền sau khi cập nhật Item
                existing.TotalAmount = existing.OrderItems
                    .Where(i => !i.IsDeleted)
                    .Sum(i => i.Price * i.Quantity);
            }

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

        // DELETE: api/Orders/5 (Xóa mềm cả Đơn hàng và Chi tiết đơn hàng)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null || data.IsDeleted)
                return NotFound();

            // Xóa mềm Đơn hàng
            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

            // Xóa mềm toàn bộ Item thuộc đơn hàng đó
            foreach (var item in data.OrderItems)
            {
                item.IsDeleted = true;
                item.DeletedAt = DateTime.Now;
                item.DeletedBy = "admin";
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(int id)
        {
            return _context.Orders.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}