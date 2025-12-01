namespace Panaderia.Models
{
    /// <summary>
    /// Modelo que representa un empleado de la panadería
    /// </summary>
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        /// <summary>Rol del empleado: 'Empleado' o 'Administrador' (ENUM en BD)</summary>
        public string Rol { get; set; }
        public decimal Salario { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
