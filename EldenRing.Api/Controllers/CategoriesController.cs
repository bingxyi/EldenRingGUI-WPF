using EldenRing.Api.Data;
using EldenRing.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EldenRing.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly EldenRingContext _context;
        public CategoriesController(EldenRingContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCategory>>> GetAll()
        {
            return Ok(await _context.ItemCategories.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemCategory>> GetById(int id)
        {
            var cat = await _context.ItemCategories.FindAsync(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }
    }
}
