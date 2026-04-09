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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            return await _context.OrderItems
                                 .Include(oi => oi.Order)
                                 .ToListAsync();
        }

        // Thêm hàm Get theo ID để phục vụ hàm Create
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetByOrder(int orderId)
        {
            var items = await _context.OrderItems
                                      .Where(oi => oi.OrderId == orderId)
                                      .ToListAsync();

            if (items == null || !items.Any()) return NotFound("Không tìm thấy món hàng nào cho đơn hàng này.");

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItem>> Create(OrderItem model)
        {
            // SỬA LỖI: Dùng 'model' thay vì 'orderItem'
            _context.OrderItems.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItem), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderItem model)
        {
            // SỬA LỖI: Dùng 'model' đồng nhất
            if (id != model.Id) return BadRequest("Id không khớp");

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null) return NotFound();

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}