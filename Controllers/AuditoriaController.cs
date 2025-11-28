using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;

namespace Panaderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        AuditoriaDatos _auditoriaDatos = new AuditoriaDatos();

        // GET: api/Auditoria
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _auditoriaDatos.Listar();
            return Ok(lista);
        }
    }
}
