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
                                 .Include(p => p.Order)
                                 .OrderByDescending(p => p.CreatedAt)
                                 .ToListAsync();
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments
                                        .Include(p => p.Order)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound($"Không tìm thấy thông tin thanh toán với Id = {id}");
            }

            return payment;
        }

      
        [HttpPost]
        public async Task<ActionResult<Payment>> Create(Payment model)
        {
           
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == payment.OrderId);
            if (!orderExists)
            {
                return BadRequest("Mã đơn hàng (OrderId) không tồn tại.");
            }

            payment.CreatedAt = DateTime.Now;
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Payment model)
        {
            if (id != payment.Id) return BadRequest();

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id)) return NotFound();
                else throw;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(int id)
        {
            return _context.Payments.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}