namespace Panaderia.Models
{
    /// <summary>
    /// Modelo de respuesta al validar credenciales (datos del usuario autenticado)
    /// </summary>
    public class Usuario
    {
        public int IdUsuario { get; set; }
        /// <summary>Nombre del empleado asociado</summary>
        public string NombreCompleto { get; set; } 
        /// <summary>Rol para control de permisos en el frontend</summary>
        public string Rol { get; set; }          
    }
}
