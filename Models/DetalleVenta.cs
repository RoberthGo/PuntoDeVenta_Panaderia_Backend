namespace Panaderia.Models
{
    /// <summary>
    /// Modelo que representa un producto dentro de una venta (línea del ticket)
    /// </summary>
    public class DetalleVentas
    {
        public int IdDetalle { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        /// <summary>Nombre del producto (obtenido del JOIN)</summary>
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        /// <summary>Cantidad * PrecioUnitario</summary>
        public decimal Subtotal { get; set; }
    }
}
