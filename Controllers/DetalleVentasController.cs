using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;
using Panaderia.Models;

namespace Panaderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentasController : ControllerBase
    {
        DetalleVentasDatos _detallesDatos = new DetalleVentasDatos();

        // GET: api/Ventas
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _detallesDatos.Listar();
            return Ok(lista);
        }

        // POST: api/Ventas
        // [FromBody] se recibe un JSON
        [HttpPost]
        public IActionResult Guardar([FromBody] DetalleVentas oDetalles)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool respuesta = _detallesDatos.Guardar(oDetalles);

            if (respuesta)
                return Ok(new { mensaje = "Detalle de venta registrado exitosamente" });
            else
                return StatusCode(500, new { mensaje = "Error al registrar detalle de venta" });
        }
    }
}
