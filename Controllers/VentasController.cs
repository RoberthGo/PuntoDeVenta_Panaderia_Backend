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

        // 1. REPORTE POR RANGO
        // Uso GET api/Ventas/ReporteRango?inicio=2025-11-01&fin=2025-11-15&idProducto=5
        // Si idProducto es NULL, devuelve TODOS los productos
        [HttpGet("ReporteRango")]
        public IActionResult ReporteRango(
            [FromQuery] string inicio,
            [FromQuery] string fin,
            [FromQuery] int? idProducto)
        {
            if (string.IsNullOrEmpty(inicio) || string.IsNullOrEmpty(fin))
                return BadRequest(new { mensaje = "Debe enviar 'inicio' y 'fin' (YYYY-MM-DD)" });

            try
            {
                var lista = _ventasDatos.ObtenerReporteRango(inicio, fin, idProducto);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error: " + ex.Message });
            }
        }

        // 2. REPORTE MENSUAL
        // Uso GET api/Ventas/ReporteMensual?mes=11&anio=2025&idProducto=5
        // Si idProducto es NULL, devuelve TODOS los productos
        [HttpGet("ReporteMensual")]
        public IActionResult ReporteMensual(
            [FromQuery] int mes,
            [FromQuery] int anio,
            [FromQuery] int? idProducto)
        {
            if (mes < 1 || mes > 12 || anio < 2000)
                return BadRequest(new { mensaje = "Mes o año inválidos" });

            try
            {
                var lista = _ventasDatos.ObtenerReporteMensual(mes, anio, idProducto);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error: " + ex.Message });
            }
        }

    }
}
