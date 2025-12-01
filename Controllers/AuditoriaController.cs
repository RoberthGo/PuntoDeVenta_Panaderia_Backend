using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;

namespace Panaderia.Controllers
{
    /// <summary>
    /// Controlador para consultar el historial de auditoría de productos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        AuditoriaDatos _auditoriaDatos = new AuditoriaDatos();

        /// <summary>
        /// Obtiene el registro de todas las acciones realizadas sobre productos
        /// </summary>
        /// <returns>Lista de registros de auditoría (usuario, acción, fecha)</returns>
        // GET: api/Auditoria
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _auditoriaDatos.Listar();
            return Ok(lista);
        }
    }
}
