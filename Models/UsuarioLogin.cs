namespace Panaderia.Models
{
    /// <summary>
    /// DTO para recibir credenciales de inicio de sesión
    /// </summary>
    public class UsuarioLogin
    {
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
    }
}
