using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;

namespace Panaderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        CategoriaDatos _categoriaDatos = new CategoriaDatos();

        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _categoriaDatos.Listar();
            return Ok(lista);
        }
    }
}
