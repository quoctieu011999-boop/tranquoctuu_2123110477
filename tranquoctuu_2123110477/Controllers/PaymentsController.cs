using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;
using System.Security.Cryptography;
using System.Text;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentsController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ==========================================
        // 1. CÁC HÀM CRUD CƠ BẢN
        // ==========================================

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

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }

        // ==========================================
        // 2. TÍCH HỢP CỔNG THANH TOÁN VNPAY
        // ==========================================

        [HttpPost("create-vnpay-url")]
        public IActionResult CreatePaymentUrl([FromBody] PaymentRequestModel model)
        {
            try
            {
                // Lấy thông tin cấu hình từ appsettings.json
                string vnp_TmnCode = _configuration["Vnpay:TmnCode"];
                string vnp_HashSecret = _configuration["Vnpay:HashSecret"];
                string vnp_Url = _configuration["Vnpay:BaseUrl"];
                string vnp_Returnurl = _configuration["Vnpay:ReturnUrl"];

                // Lấy IP người dùng
                string vnp_IpAddr = "127.0.0.1";

                // Khởi tạo Parameters
                var vnp_Params = new SortedList<string, string>(new VnPayCompare())
                {
                    { "vnp_Version", "2.1.0" },
                    { "vnp_Command", "pay" },
                    { "vnp_TmnCode", vnp_TmnCode },
                    { "vnp_Amount", ((long)(model.Amount * 100)).ToString() },
                    { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                    { "vnp_CurrCode", "VND" },
                    { "vnp_IpAddr", vnp_IpAddr },
                    { "vnp_Locale", "vn" },
                    { "vnp_OrderInfo", "ThanhToanDonHang_" + model.OrderId },
                    { "vnp_OrderType", "other" },
                    { "vnp_ReturnUrl", vnp_Returnurl },
                    { "vnp_TxnRef", model.OrderId.ToString() + "_" + DateTime.Now.Ticks.ToString() }
                };

                // Lắp ráp chuỗi dữ liệu (ĐÃ FIX LỖI SAI CHỮ KÝ Ở ĐÂY)
                StringBuilder query = new StringBuilder();

                foreach (KeyValuePair<string, string> kv in vnp_Params)
                {
                    if (!string.IsNullOrEmpty(kv.Value))
                    {
                        // Dùng Uri.EscapeDataString để chuẩn hóa %20 thay vì dấu +
                        query.Append(Uri.EscapeDataString(kv.Key) + "=" + Uri.EscapeDataString(kv.Value) + "&");
                    }
                }

                string queryString = query.ToString().TrimEnd('&');

                // Trong phiên bản 2.1.0, signData chính là chuỗi queryString
                string signData = queryString;

                // Tạo chữ ký bảo mật
                string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);

                // URL cuối cùng gửi về cho React
                string paymentUrl = vnp_Url + "?" + queryString + "&vnp_SecureHash=" + vnp_SecureHash;

                return Ok(new { paymentUrl = paymentUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo URL VNPAY", error = ex.Message });
            }
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }

    // Model dùng riêng cho API VNPAY
    public class PaymentRequestModel
    {
        public int OrderId { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
    }

    // Lớp so sánh chuỗi theo chuẩn VNPAY
    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = System.Globalization.CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, System.Globalization.CompareOptions.Ordinal);
        }
    }
}