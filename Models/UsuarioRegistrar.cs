namespace Panaderia.Models
{
    /// <summary>
    /// DTO para registrar un usuario asociado a un empleado existente
    /// </summary>
    public class UsuarioRegistrar
    {
        /// <summary>ID del empleado al que se asociará el usuario</summary>
        public int IdEmpleado { get; set; }
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
    }
}
