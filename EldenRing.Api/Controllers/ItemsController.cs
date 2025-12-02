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
    // Defino o controlador para gerenciar as operações CRUD dos itens do jogo
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly EldenRingContext _context;

        // Recebo o contexto do banco via injeção de dependência
        public ItemsController(EldenRingContext context)
        {
            _context = context;
        }

        // GET: api/items
        // Busco a lista completa de itens, incluindo os dados da categoria associada
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EldenRingItem>>> GetAll()
        {
            var items = await _context.EldenRingItems
                                      .Include(i => i.ItemCategory)
                                      .ToListAsync();
            return Ok(items);
        }

        // GET: api/items/{id}
        // Retorno um único item baseado no ID, trazendo também sua categoria
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EldenRingItem>> GetById(int id)
        {
            var item = await _context.EldenRingItems
                                     .Include(i => i.ItemCategory)
                                     .FirstOrDefaultAsync(i => i.Id == id);
            
            // Retorno NotFound se o item não existir
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/items
        // Crio um novo item no banco de dados a partir do DTO recebido
        [HttpPost]
        public async Task<ActionResult<EldenRingItem>> Create([FromBody] EldenRingItemCreateDto dto)
        {
            // Valido se o modelo enviado obedece às regras de validação
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Verifico se a categoria informada existe no banco
            var category = await _context.ItemCategories.FindAsync(dto.ItemCategoryId);
            if (category == null) return UnprocessableEntity(new { message = "Categoria não encontrada." });

            // Mapeio os dados do DTO para a entidade de domínio
            var item = new EldenRingItem
            {
                Name = dto.Name,
                Rarity = dto.Rarity,
                Price = dto.Price,
                Description = dto.Description,
                ItemCategoryId = dto.ItemCategoryId
            };

            // Adiciono o item ao contexto e salvo as alterações
            _context.EldenRingItems.Add(item);
            await _context.SaveChangesAsync();

            // Retorno o status 201 Created indicando onde o recurso pode ser acessado
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // PUT: api/items/{id}
        // Atualizo os dados de um item existente
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EldenRingItemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Busco o item original para garantir que ele existe
            var existing = await _context.EldenRingItems.FindAsync(id);
            if (existing == null) return NotFound();

            // Verifico novamente a existência da categoria
            var category = await _context.ItemCategories.FindAsync(dto.ItemCategoryId);
            if (category == null) return UnprocessableEntity(new { message = "Categoria não encontrada." });

            // Atualizo as propriedades da entidade existente
            existing.Name = dto.Name;
            existing.Rarity = dto.Rarity;
            existing.Price = dto.Price;
            existing.Description = dto.Description;
            existing.ItemCategoryId = dto.ItemCategoryId;

            // Persisto as alterações no banco
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/items/{id}
        // Excluo um item do banco de dados pelo seu ID
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
