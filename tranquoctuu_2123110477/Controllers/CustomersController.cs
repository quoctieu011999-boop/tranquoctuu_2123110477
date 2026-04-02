using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

    
        [HttpGet("{id?}")]
        public async Task<ActionResult<object>> GetCustomers(int? id)
        {
            if (_context.Customers == null)
                return NotFound("Entity set 'Customers' is null.");

            // Nếu có ID: Trả về 1 khách hàng cụ thể
            if (id.HasValue)
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == id.Value && c.DeletedAt == null);

                if (customer == null)
                    return NotFound($"Không tìm thấy khách hàng với Id = {id.Value}");

                return Ok(customer);
            }

            // Nếu không có ID: Trả về toàn bộ danh sách chưa xóa
            var list = await _context.Customers
                .Where(c => c.DeletedAt == null)
                .ToListAsync();

            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            var username = User.Identity?.Name ?? "admin";

            customer.CreatedBy = username;
            customer.CreatedAt = DateTime.Now;

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Sửa lại nameof để trỏ đúng về hàm GetCustomers ở trên
            return CreatedAtAction(nameof(GetCustomers), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
                return BadRequest("Id không khớp");

            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
                return NotFound();

            var username = User.Identity?.Name ?? "admin";

            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.UpdatedBy = username;
            existingCustomer.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null || customer.DeletedAt != null)
                return NotFound();

            var username = User.Identity?.Name ?? "admin";

            customer.IsDeleted = true;
            customer.DeletedAt = DateTime.Now;
            customer.DeletedBy = username;

            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}