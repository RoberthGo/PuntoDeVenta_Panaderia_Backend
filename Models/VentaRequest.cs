namespace Panaderia.Models
{
    /// <summary>
    /// DTO para recibir una nueva venta desde el frontend
    /// </summary>
    public class VentaRequest
    {
        /// <summary>Empleado que registra la venta</summary>
        public int IdEmpleado { get; set; } 
        /// <summary>Lista de productos con cantidad y precio</summary>
        public List<ProductoVentaItem> Productos { get; set; }
    }

    /// <summary>
    /// Representa un producto dentro de la venta
    /// </summary>
    public class ProductoVentaItem
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
