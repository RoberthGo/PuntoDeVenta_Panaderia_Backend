using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;
using Panaderia.Models;

namespace Panaderia.Controllers
{
    /// <summary>
    /// Controlador para gestión de ventas y reportes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        VentasDatos _ventasDatos = new VentasDatos();

        /// <summary>
        /// Obtiene el historial de todas las ventas
        /// </summary>
        /// <returns>Lista de ventas en formato JSON</returns>
        // GET: api/Ventas
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _ventasDatos.Listar();
            return Ok(lista);
        }

        /// <summary>
        /// Registra una nueva venta con sus productos (transacción atómica)
        /// </summary>
        /// <param name="oVenta">JSON con IdEmpleado y lista de Productos con cantidad</param>
        /// <returns>Mensaje de éxito o error</returns>
        // POST: api/Ventas/Registrar - [FromBody] recibe JSON
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

        /// <summary>
        /// Genera reporte de ventas en un rango de fechas
        /// </summary>
        /// <param name="inicio">Fecha inicio (YYYY-MM-DD)</param>
        /// <param name="fin">Fecha fin (YYYY-MM-DD)</param>
        /// <param name="ids">IDs de productos separados por coma (opcional, si es null retorna todos)</param>
        /// <returns>Lista con totales vendidos por producto</returns>
        // GET: api/Ventas/ReporteRango?inicio=2025-11-01&fin=2025-11-15&ids=1,2,5
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

        /// <summary>
        /// Genera reporte de ventas de un mes específico
        /// </summary>
        /// <param name="mes">Número del mes (1-12)</param>
        /// <param name="anio">Año (ej: 2025)</param>
        /// <param name="ids">IDs de productos separados por coma (opcional)</param>
        /// <returns>Lista con totales vendidos por producto en el mes</returns>
        // GET: api/Ventas/ReporteMensual?mes=11&anio=2025&ids=1,2,5
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


        /// <summary>
        /// Obtiene los productos de una venta específica (detalle del ticket)
        /// </summary>
        /// <param name="idVenta">ID de la venta</param>
        /// <returns>Lista de productos con cantidad y precio de esa venta</returns>
        // GET: api/Ventas/Detalle/5
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
