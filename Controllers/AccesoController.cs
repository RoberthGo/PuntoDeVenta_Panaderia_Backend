using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;
using Panaderia.Models;

namespace Panaderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        UsuarioDatos _usuarioDatos = new UsuarioDatos();

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
