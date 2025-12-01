using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;

namespace Panaderia.Controllers
{
    /// <summary>
    /// Controlador para obtener las categorías de productos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        CategoriaDatos _categoriaDatos = new CategoriaDatos();

        /// <summary>
        /// Obtiene todas las categorías disponibles
        /// </summary>
        /// <returns>Lista de categorías en formato JSON</returns>
        // GET: api/Categorias
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _categoriaDatos.Listar();
            return Ok(lista);
        }
    }
}
