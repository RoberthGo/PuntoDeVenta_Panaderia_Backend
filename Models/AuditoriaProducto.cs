namespace Panaderia.Models
{
    /// <summary>
    /// Modelo que representa un registro de auditoría para cambios en productos
    /// </summary>
    public class AuditoriaProducto
    {
        public int IdAuditoria { get; set; }
        public DateTime FechaHora { get; set; }
        /// <summary>Usuario que realizó la acción</summary>
        public string Usuario { get; set; }
        /// <summary>Tipo de operación: INSERT, UPDATE o DELETE</summary>
        public string Accion { get; set; }
        public int IdProducto { get; set; }
        /// <summary>Valor antes del cambio (JSON o texto)</summary>
        public string ValorAnterior { get; set; }
        /// <summary>Valor después del cambio (JSON o texto)</summary>
        public string ValorNuevo { get; set; }
    }
}
