using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EldenRing.Api.Data;
using EldenRing.Api.Models;
using EldenRing.Api.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EldenRing.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly EldenRingContext _context;

        public ItemsController(EldenRingContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EldenRingItem>>> GetAll()
        {
            var items = await _context.EldenRingItems
                                      .Include(i => i.ItemCategory)
                                      .ToListAsync();
            return Ok(items);
        }

        // GET: api/items/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EldenRingItem>> GetById(int id)
        {
            var item = await _context.EldenRingItems
                                     .Include(i => i.ItemCategory)
                                     .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/items
        [HttpPost]
        public async Task<ActionResult<EldenRingItem>> Create([FromBody] EldenRingItemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = await _context.ItemCategories.FindAsync(dto.ItemCategoryId);
            if (category == null) return UnprocessableEntity(new { message = "Categoria não encontrada." });

            var item = new EldenRingItem
            {
                Name = dto.Name,
                Rarity = dto.Rarity,
                Price = dto.Price,
                Description = dto.Description,
                ItemCategoryId = dto.ItemCategoryId
            };

            _context.EldenRingItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // PUT: api/items/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EldenRingItemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _context.EldenRingItems.FindAsync(id);
            if (existing == null) return NotFound();

            var category = await _context.ItemCategories.FindAsync(dto.ItemCategoryId);
            if (category == null) return UnprocessableEntity(new { message = "Categoria não encontrada." });

            existing.Name = dto.Name;
            existing.Rarity = dto.Rarity;
            existing.Price = dto.Price;
            existing.Description = dto.Description;
            existing.ItemCategoryId = dto.ItemCategoryId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/items/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.EldenRingItems.FindAsync(id);
            if (existing == null) return NotFound();

            _context.EldenRingItems.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
