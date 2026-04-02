using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GỘP 2 GET THÀNH 1 (Tất cả hoặc Chi tiết)
        // GET: api/OrderItems
        // GET: api/OrderItems/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetOrderItems(int? id)
        {
            if (_context.OrderItems == null)
                return NotFound();

            // Định nghĩa query chung để dùng Select (tránh lặp lại code)
            var query = _context.OrderItems
                .Where(x => !x.IsDeleted)
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Select(x => new
                {
                    x.Id,
                    x.OrderId,
                    x.ProductId,
                    ProductName = x.Product.Name,
                    x.Quantity,
                    x.Price,
                    x.CreatedAt
                });

            // Trường hợp 1: Lấy chi tiết 1 Item
            if (id.HasValue)
            {
                var item = await query.FirstOrDefaultAsync(x => x.Id == id.Value);
                if (item == null)
                    return NotFound($"Không tìm thấy Item Id = {id.Value}");

                return Ok(item);
            }

            // Trường hợp 2: Lấy toàn bộ danh sách
            var list = await query.ToListAsync();
            return Ok(list);
        }

        // GET: api/OrderItems/order/5 (Lấy các item thuộc về 1 đơn hàng cụ thể)
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetByOrder(int orderId)
        {
            var items = await _context.OrderItems
                .Where(x => x.OrderId == orderId && !x.IsDeleted)
                .Include(x => x.Product)
                .Select(x => new
                {
                    x.Id,
                    x.OrderId,
                    ProductName = x.Product.Name,
                    x.Quantity,
                    x.Price
                })
                .ToListAsync();

            if (!items.Any())
                return NotFound("Không có sản phẩm nào trong đơn hàng này");

            return Ok(items);
        }

        // POST: api/OrderItems
        [HttpPost]
        public async Task<ActionResult<OrderItem>> Create(OrderItem model)
        {
            // Kiểm tra Product có tồn tại không trước khi thêm vào đơn
            var productExists = await _context.Products.AnyAsync(p => p.Id == model.ProductId && !p.IsDeleted);
            if (!productExists)
                return BadRequest("Sản phẩm không tồn tại hoặc đã ngừng kinh doanh");

            model.CreatedAt = DateTime.Now;
            model.IsDeleted = false;

            _context.OrderItems.Add(model);
            await _context.SaveChangesAsync();

            // Trỏ về GetOrderItems (hàm đã gộp)
            return CreatedAtAction(nameof(GetOrderItems), new { id = model.Id }, model);
        }

        // PUT: api/OrderItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderItem model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.OrderItems.FindAsync(id);
            if (existing == null || existing.IsDeleted)
                return NotFound();

            // Cập nhật dữ liệu
            existing.ProductId = model.ProductId;
            existing.Quantity = model.Quantity;
            existing.Price = model.Price;
            existing.OrderId = model.OrderId;
            existing.UpdatedAt = DateTime.Now;

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

        // DELETE: api/OrderItems/5 (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.OrderItems.FindAsync(id);

            if (data == null || data.IsDeleted)
                return NotFound("Sản phẩm không tồn tại hoặc đã bị xóa");

            // Xóa mềm
            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool Exists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}