namespace Panaderia.Models
{
    public class EmpleadoRegistro
    {
        // Datos del Empleado
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Rol { get; set; }
        public decimal Salario { get; set; }

        // Datos del Usuario
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
    }
}
