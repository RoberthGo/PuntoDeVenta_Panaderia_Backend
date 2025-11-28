using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;
using Panaderia.Models;

namespace Panaderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        VentasDatos _ventasDatos = new VentasDatos();

        // GET: api/Ventas
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _ventasDatos.Listar();
            return Ok(lista);
        }

        // POST: api/Ventas
        // [FromBody] se recibe un JSON
        [HttpPost]
        public IActionResult Guardar([FromBody] Ventas oVentas)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool respuesta = _ventasDatos.Guardar(oVentas);

            if (respuesta)
                return Ok(new { mensaje = "Venta registrada exitosamente" });
            else
                return StatusCode(500, new { mensaje = "Error al registrar venta" });
        }
    }
}
