namespace Panaderia.Models
{
    /// <summary>
    /// Modelo que representa el encabezado de una venta (ticket)
    /// </summary>
    public class Ventas
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        /// <summary>Empleado que realizó la venta</summary>
        public int IdEmpleado { get; set; }
        /// <summary>Suma de todos los subtotales del detalle</summary>
        public decimal Total { get; set; }
    }
}
