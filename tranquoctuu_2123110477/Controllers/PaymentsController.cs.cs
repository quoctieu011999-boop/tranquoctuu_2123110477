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
            // SỬA LỖI: Dùng 'model' đồng nhất với tham số đầu vào
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == model.OrderId);
            if (!orderExists)
            {
                return BadRequest("Mã đơn hàng (OrderId) không tồn tại.");
            }

            model.CreatedAt = DateTime.Now;
            _context.Payments.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Payment model)
        {
            // SỬA LỖI: Kiểm tra Id từ 'model'
            if (id != model.Id) return BadRequest("Id không trùng khớp.");

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();

            // SỬA LỖI: Thực hiện lệnh xóa thực tế khỏi Database
            _context.Payments.Remove(payment);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            // SỬA LỖI: Đổi tên hàm thành PaymentExists cho khớp với hàm Update ở trên
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}