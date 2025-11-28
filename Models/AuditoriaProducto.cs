namespace Panaderia.Models
{
    public class AuditoriaProducto
    {
        public int IdAuditoria { get; set; }
        public DateTime FechaHora { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; } // INSERT, UPDATE, DELETE
        public int IdProducto { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
    }
}
