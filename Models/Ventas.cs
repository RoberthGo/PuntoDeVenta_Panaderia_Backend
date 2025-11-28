namespace Panaderia.Models
{
    public class Ventas
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public int IdEmpleado { get; set; }
        public decimal Total { get; set; }
    }
}
