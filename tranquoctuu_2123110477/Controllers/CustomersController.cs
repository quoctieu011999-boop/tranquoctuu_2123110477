using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers // Đồng nhất namespace với các file trước
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            if (_context.Customers == null) return NotFound();
            
            // Chỉ lấy những khách hàng chưa bị xóa (Soft Delete)
            return await _context.Customers
                                 .Where(c => c.DeletedAt == null)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            if (_context.Customers == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null || customer.DeletedAt != null)
            {
                return NotFound($"Không tìm thấy khách hàng với Id = {id}");
            }

            return customer; // Trả về 1 khách hàng duy nhất, không trả về List ở đây
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            customer.CreatedAt = DateTime.Now;
            customer.CreatedBy = "admin"; // Gán mặc định admin nếu chưa có Auth

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id) return BadRequest("Id không khớp.");

            customer.UpdatedAt = DateTime.Now;
            customer.UpdatedBy = "admin";

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            // Thực hiện xóa mềm để đồng bộ với logic Get
            customer.DeletedAt = DateTime.Now;
            customer.DeletedBy = "admin";
            
            // Nếu muốn xóa hẳn khỏi DB thì dùng: _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id && e.DeletedAt == null);
        }
    }
}