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

    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer)
                .Include(x => x.OrderItems.Where(i => !i.IsDeleted))
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var data = await _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.OrderItems.Where(i => !i.IsDeleted))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
                return NotFound($"Không tìm thấy đơn hàng Id = {id}");

            return data;
        }

       
        [HttpPost]
        public async Task<ActionResult<Order>> Create(Order model)
        {
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.Status = "Pending";

         
            if (model.OrderItems != null && model.OrderItems.Any())
            {
                model.TotalAmount = model.OrderItems
                    .Sum(i => i.Price * i.Quantity);

                foreach (var item in model.OrderItems)
                {
                    item.CreatedAt = DateTime.Now;
                    item.CreatedBy = "admin";
                }
            }

            _context.Orders.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = model.Id }, model);
        }

     
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

            
            existing.CustomerId = model.CustomerId;
            existing.Status = model.Status;

           
            if (model.OrderItems != null)
            {
              
                foreach (var old in existing.OrderItems)
                {
                    old.IsDeleted = true;
                    old.DeletedAt = DateTime.Now;
                    old.DeletedBy = "admin";
                }

                
                existing.OrderItems = model.OrderItems;

                foreach (var item in existing.OrderItems)
                {
                    item.CreatedAt = DateTime.Now;
                    item.CreatedBy = "admin";
                }

                
                existing.TotalAmount = existing.OrderItems
                    .Sum(i => i.Price * i.Quantity);
            }

            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null || data.IsDeleted)
                return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

           
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