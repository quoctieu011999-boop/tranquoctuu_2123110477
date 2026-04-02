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

        // ================= GET ALL =================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOrderItems()
        {
            var data = await _context.OrderItems
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Select(x => new
                {
                    x.Id,
                    x.OrderId,
                    ProductName = x.Product.Name,
                    x.Quantity,
                    x.Price
                })
                .ToListAsync();

            return Ok(data);
        }

        // ================= GET BY ORDER =================
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetByOrder(int orderId)
        {
            var items = await _context.OrderItems
                .Where(x => x.OrderId == orderId)
                .Include(x => x.Product)
                .Select(x => new
                {
                    x.Id,
                    ProductName = x.Product.Name,
                    x.Quantity,
                    x.Price
                })
                .ToListAsync();

            if (!items.Any())
                return NotFound("Không có sản phẩm trong đơn");

            return Ok(items);
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<ActionResult<OrderItem>> Create(OrderItem model)
        {
            // kiểm tra product tồn tại
            var product = await _context.Products.FindAsync(model.ProductId);
            if (product == null)
                return BadRequest("Product không tồn tại");

            _context.OrderItems.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItems), new { id = model.Id }, model);
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderItem model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.OrderItems.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.ProductId = model.ProductId; 
            existing.Quantity = model.Quantity;
            existing.Price = model.Price;
            existing.OrderId = model.OrderId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.OrderItems.FindAsync(id);

            if (data == null || data.IsDeleted) 
                return NotFound("Sản phẩm không tồn tại hoặc đã bị xóa");

            
            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;

            _context.Entry(data).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}