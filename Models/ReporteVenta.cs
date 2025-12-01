namespace Panaderia.Models
{
    /// <summary>
    /// Modelo para los reportes de ventas (por rango de fechas o mensual)
    /// </summary>
    public class ReporteVenta
    {
        /// <summary>IdProducto</summary>
        public int Clave { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        /// <summary>Total de unidades vendidas en el periodo</summary>
        public int UnidadesVendidas { get; set; }
        /// <summary>Suma total de ventas en dinero</summary>
        public decimal MontoTotal { get; set; }
    }
}
