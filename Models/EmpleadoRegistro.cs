namespace Panaderia.Models
{
    /// <summary>
    /// DTO para registrar un empleado junto con su usuario en una sola petición
    /// </summary>
    public class EmpleadoRegistro
    {
        // Datos del Empleado
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Rol { get; set; }
        public decimal Salario { get; set; }

        // Credenciales del Usuario
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
    }
}
