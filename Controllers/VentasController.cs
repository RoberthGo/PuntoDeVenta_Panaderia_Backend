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

        // POST: api/Ventas/Registrar
        // [FromBody] se recibe un JSON
        [HttpPost("Registrar")]
        public IActionResult Registrar([FromBody] VentaRequest oVenta)
        {
            // Validamos que la lista no venga vacía
            if (oVenta.Productos == null || oVenta.Productos.Count == 0)
            {
                return BadRequest(new { mensaje = "La venta debe contener al menos un producto." });
            }

            // Llamamos al método transaccional
            bool respuesta = _ventasDatos.RegistrarVentaTransaccion(oVenta);

            if (respuesta)
                return Ok(new { mensaje = "Venta registrada exitosamente." });
            else
                return StatusCode(500, new { mensaje = "Error en la transacción. No se guardó nada." });
        }
    }
}
