using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models;
using tranquoctuu_2123110477.Data;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyTransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoyaltyTransactionsController(AppDbContext context)
        {
            _context = context;
        }

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoyaltyTransaction>>> GetAll()
        {
            return await _context.LoyaltyTransactions
                .Where(x => !x.IsDeleted)
                .Include(x => x.Customer)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<LoyaltyTransaction>> GetById(int id)
        {
            var data = await _context.LoyaltyTransactions
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (data == null)
                return NotFound($"Không tìm thấy Id = {id}");

            return data;
        }

        
        [HttpPost]
        public async Task<ActionResult<LoyaltyTransaction>> Create(LoyaltyTransaction model)
        {
          
            var last = await _context.LoyaltyTransactions
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            model.PreviousHash = last?.Hash ?? "0";

          
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "admin";

            
            model.Hash = model.CalculateHash();

            _context.LoyaltyTransactions.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

       
        [HttpPut("{id}")]
        public IActionResult BlockUpdate()
        {
            return BadRequest("Không được phép sửa transaction!");
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var data = await _context.LoyaltyTransactions.FindAsync(id);
            if (data == null || data.IsDeleted)
                return NotFound();

            data.IsDeleted = true;
            data.DeletedAt = DateTime.Now;
            data.DeletedBy = "admin";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyChain()
        {
            var list = await _context.LoyaltyTransactions
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .ToListAsync();

            for (int i = 1; i < list.Count; i++)
            {
                var current = list[i];
                var previous = list[i - 1];

               
                if (current.PreviousHash != previous.Hash)
                {
                    return BadRequest($"Chain lỗi tại Id = {current.Id}");
                }

               
                if (current.Hash != current.CalculateHash())
                {
                    return BadRequest($"Hash bị thay đổi tại Id = {current.Id}");
                }
            }

            return Ok("Chuỗi hợp lệ ✅");
        }

        private bool Exists(int id)
        {
            return _context.LoyaltyTransactions.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}