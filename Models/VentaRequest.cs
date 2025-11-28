namespace Panaderia.Models
{
    public class VentaRequest
    {
        public int IdEmpleado { get; set; } 
        public List<ProductoVentaItem> Productos { get; set; }
    }

    public class ProductoVentaItem
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
