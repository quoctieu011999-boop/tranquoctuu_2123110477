using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentsController(AppDbContext context)
        {
            _context = context;
        }

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments
                .Where(x => !x.IsDeleted)
                .Include(x => x.Order)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var data = await _context.Payments
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
                return NotFound($"Không tìm thấy Payment Id = {id}");

            return data;
        }

        
        [HttpPost]
        public async Task<ActionResult<Payment>> Create(Payment model)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == model.OrderId && !o.IsDeleted);

            if (order == null)
                return BadRequest("Order không tồn tại");

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.Status = "Pending";

           
            if (model.Amount <= 0)
                return BadRequest("Số tiền không hợp lệ");

            _context.Payments.Add(model);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = model.Id }, model);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Payment model)
        {
            if (id != model.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.Payments
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null || existing.IsDeleted)
                return NotFound();

            existing.Amount = model.Amount;
            existing.Method = model.Method;
            existing.Status = model.Status;

            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

           
            if (model.Status == "Success")
            {
                existing.Order.Status = "Completed";
                existing.Order.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Payments.FindAsync(id);

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
            return _context.Payments.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}