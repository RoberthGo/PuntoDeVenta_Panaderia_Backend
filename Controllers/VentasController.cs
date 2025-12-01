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
        // Uso GET api/Ventas/ReporteRango?inicio=2025-11-01&fin=2025-11-15&ids=1,2,5
        // Si ids es NULL, devuelve TODOS los productos
        [HttpGet("ReporteRango")]
        public IActionResult ReporteRango(
            [FromQuery] string inicio,
            [FromQuery] string fin,
            [FromQuery] string? ids) // Recibe string con comas
        {
            if (string.IsNullOrEmpty(inicio) || string.IsNullOrEmpty(fin))
                return BadRequest(new { mensaje = "Debe enviar 'inicio' y 'fin' (YYYY-MM-DD)" });

            try
            {
                List<int> listaIds = new List<int>();
                if (!string.IsNullOrEmpty(ids))
                {
                    listaIds = ids.Split(',')
                                  .Select(s => int.TryParse(s, out int n) ? n : 0)
                                  .Where(n => n > 0)
                                  .ToList();
                }

                var lista = _ventasDatos.ObtenerReporteRango(inicio, fin, listaIds);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error: " + ex.Message });
            }
        }

        // 2. REPORTE MENSUAL
        // Uso GET api/Ventas/ReporteMensual?mes=11&anio=2025&ids=1,2,5
        // Si ids es NULL, devuelve TODOS los productos
        [HttpGet("ReporteMensual")]
        public IActionResult ReporteMensual(
            [FromQuery] int mes,
            [FromQuery] int anio,
            [FromQuery] string? ids)
        {
            if (mes < 1 || mes > 12 || anio < 2000)
                return BadRequest(new { mensaje = "Mes o año inválidos" });

            try
            {
                List<int> listaIds = new List<int>();
                if (!string.IsNullOrEmpty(ids))
                {
                    listaIds = ids.Split(',')
                                  .Select(s => int.TryParse(s, out int n) ? n : 0)
                                  .Where(n => n > 0)
                                  .ToList();
                }

                var lista = _ventasDatos.ObtenerReporteMensual(mes, anio, listaIds);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error: " + ex.Message });
            }
        }


        // GET: api/Ventas/Detalle/5
        // Obtener los panes de un ticket específico
        [HttpGet("Detalle/{idVenta}")]
        public IActionResult ObtenerDetalles(int idVenta)
        {
            try
            {
                var lista = _ventasDatos.ObtenerDetalles(idVenta);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener detalles: " + ex.Message });
            }
        }

    }
}
