using EldenRing.Api.Data;
using EldenRing.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EldenRing.Api.Controllers
{
    // Defino o controlador responsável por gerenciar as requisições de categorias
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly EldenRingContext _context;
        
        // Injeto o contexto do banco de dados no construtor
        public CategoriesController(EldenRingContext context) { _context = context; }

        // Retorno todas as categorias cadastradas no banco de dados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCategory>>> GetAll()
        {
            return Ok(await _context.ItemCategories.ToListAsync());
        }

        // Busco uma categoria específica pelo ID fornecido na rota
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemCategory>> GetById(int id)
        {
            var cat = await _context.ItemCategories.FindAsync(id);
            // Verifico se a categoria foi encontrada, retornando 404 caso contrário
            if (cat == null) return NotFound();
            return Ok(cat);
        }
    }
}
