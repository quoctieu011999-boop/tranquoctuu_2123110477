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

        // GỘP 2 GET THÀNH 1
        // GET: api/Payments
        // GET: api/Payments/5
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetPayments(int? id)
        {
            if (_context.Payments == null)
                return NotFound();

            // Thiết lập query cơ bản bao gồm thông tin Order
            var query = _context.Payments
                .Where(x => !x.IsDeleted)
                .Include(x => x.Order);

            // Trường hợp 1: Lấy chi tiết một giao dịch thanh toán
            if (id.HasValue)
            {
                var data = await query.FirstOrDefaultAsync(x => x.Id == id.Value);

                if (data == null)
                    return NotFound($"Không tìm thấy Payment với Id = {id.Value}");

                return Ok(data);
            }

            // Trường hợp 2: Lấy toàn bộ danh sách thanh toán mới nhất lên đầu
            var list = await query.OrderByDescending(x => x.CreatedAt).ToListAsync();

            return Ok(list);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<Payment>> Create(Payment model)
        {
            if (model == null) return BadRequest("Dữ liệu không hợp lệ");

            // Kiểm tra sự tồn tại của Đơn hàng
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == model.OrderId && !o.IsDeleted);

            if (order == null)
                return BadRequest("Đơn hàng (Order) không tồn tại");

            // Kiểm tra số tiền thanh toán
            if (model.Amount <= 0)
                return BadRequest("Số tiền thanh toán phải lớn hơn 0");

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";
            model.Status = "Pending";
            model.IsDeleted = false;

            _context.Payments.Add(model);
            await _context.SaveChangesAsync();

            // Trỏ về GetPayments (hàm đã gộp)
            return CreatedAtAction(nameof(GetPayments), new { id = model.Id }, model);
        }

        // PUT: api/Payments/5
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

            // Cập nhật các trường thông tin
            existing.Amount = model.Amount;
            existing.Method = model.Method;
            existing.Status = model.Status;
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = "admin";

            // LOGIC NGHIỆP VỤ: Nếu thanh toán thành công, cập nhật trạng thái đơn hàng
            if (model.Status == "Success" && existing.Order != null)
            {
                existing.Order.Status = "Completed";
                existing.Order.UpdatedAt = DateTime.Now;
                existing.Order.UpdatedBy = "system_payment";
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

        // DELETE: api/Payments/5 (Xóa mềm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Payments.FindAsync(id);

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
            return _context.Payments.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}