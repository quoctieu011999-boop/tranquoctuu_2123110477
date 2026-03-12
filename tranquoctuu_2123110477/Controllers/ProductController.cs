using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tranquoctuu_2123110477.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<string> Products = new List<string> { "Laptop", "Mouse", "Keyboard" };

   
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Products);
        }

      
        [HttpPost]
        public IActionResult Create([FromBody] string name)
        {
            Products.Add(name);
            return Ok(new { message = "Thêm thành công!", data = Products });
        }

        
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] string newName)
        {
            if (id < 0 || id >= Products.Count) return NotFound("Không tìm thấy sản phẩm");
            Products[id] = newName;
            return Ok(new { message = "Cập nhật thành công!", data = Products });
        }

       
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id < 0 || id >= Products.Count) return NotFound("Không tìm thấy sản phẩm");
            Products.RemoveAt(id);
            return Ok(new { message = "Xóa thành công!", data = Products });
        }
    }
}
