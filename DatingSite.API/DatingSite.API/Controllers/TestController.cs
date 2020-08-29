using DatingSite.API.Data;
using DatingSite.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingSite.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase {

        private readonly DataContext _context;

        public TestController(DataContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestData() {
            IEnumerable<Value> values = await _context.Values.ToListAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestData(int id) {
            Value value = await _context.Values.FirstOrDefaultAsync(v => v.Id == id);

            if (value == null) {
                return NotFound();
            }

            return Ok(value);
        }
    }
}
