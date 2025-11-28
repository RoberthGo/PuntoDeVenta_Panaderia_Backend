namespace Panaderia.Models
{
    public class ReporteVenta
    {
        public int Clave { get; set; } // IdProducto 
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int UnidadesVendidas { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
