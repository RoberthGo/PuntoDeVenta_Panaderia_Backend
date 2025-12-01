using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;
using Panaderia.Models;

namespace Panaderia.Controllers
{
    /// <summary>
    /// Controlador para autenticación y registro de usuarios
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        UsuarioDatos _usuarioDatos = new UsuarioDatos();

        /// <summary>
        /// Valida las credenciales del usuario para iniciar sesión
        /// </summary>
        /// <param name="oLogin">JSON con NombreUsuario y Clave</param>
        /// <returns>Datos del usuario si es válido, o error 401 si no</returns>
        // POST: api/Acceso/Login - [FromBody] recibe JSON
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UsuarioLogin oLogin)
        {
            if (string.IsNullOrEmpty(oLogin.NombreUsuario) || string.IsNullOrEmpty(oLogin.Clave))
            {
                return BadRequest(new { mensaje = "Faltan datos de acceso (Usuario o Clave)" });
            }

            var usuarioEncontrado = _usuarioDatos.Validar(oLogin);

            if (usuarioEncontrado != null)
            {
                return Ok(new
                {
                    mensaje = "Bienvenido al sistema",
                    usuario = usuarioEncontrado
                });
            }
            else
            {
                return StatusCode(401, new { mensaje = "Usuario o contraseña incorrectos" });
            }
        }


        /// <summary>
        /// Registra un nuevo usuario asociado a un empleado existente
        /// </summary>
        /// <param name="oRegistro">JSON con IdEmpleado, NombreUsuario y Clave</param>
        /// <returns>Mensaje de éxito o error</returns>
        // POST: api/Acceso/Registrar - [FromBody] recibe JSON
        [HttpPost("Registrar")]
        public IActionResult Registrar([FromBody] UsuarioRegistrar oRegistro)
        {
            // Validaciones
            if (oRegistro.IdEmpleado <= 0)
                return BadRequest(new { mensaje = "El IdEmpleado no es válido" });

            if (string.IsNullOrEmpty(oRegistro.NombreUsuario) || string.IsNullOrEmpty(oRegistro.Clave))
                return BadRequest(new { mensaje = "Faltan datos de usuario o clave" });

            bool respuesta = _usuarioDatos.Registrar(oRegistro);

            if (respuesta)
            {
                return Ok(new { mensaje = "Usuario registrado exitosamente" });
            }
            else
            {
                // El usuario ya existe o el empleado no existe
                return StatusCode(500, new { mensaje = "Error al registrar. Verifique que el usuario no exista y el empleado sea válido." });
            }
        }
    }
}
