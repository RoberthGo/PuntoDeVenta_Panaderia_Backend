using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Panaderia.Data;
using Panaderia.Models;

namespace Panaderia.Controllers
{
    /// <summary>
    /// Controlador para la gestión de empleados (CRUD)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        EmpleadoDatos _empleadoDatos = new EmpleadoDatos();

        /// <summary>
        /// Obtiene la lista de todos los empleados
        /// </summary>
        /// <returns>Lista de empleados en formato JSON</returns>
        // GET: api/Empleados
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _empleadoDatos.Listar();
            return Ok(lista);
        }

        /// <summary>
        /// Registra un nuevo empleado junto con su usuario
        /// </summary>
        /// <param name="oEmpleado">Objeto JSON con datos del empleado y credenciales</param>
        /// <returns>Mensaje de éxito o error</returns>
        // POST: api/Empleados - [FromBody] recibe JSON
        [HttpPost]
        public IActionResult Guardar([FromBody] EmpleadoRegistro oEmpleado)
        {
            if (string.IsNullOrEmpty(oEmpleado.Nombre) || string.IsNullOrEmpty(oEmpleado.NombreUsuario))
            {
                return BadRequest(new { mensaje = "Faltan datos obligatorios (Nombre, Usuario)" });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool respuesta = _empleadoDatos.RegistrarCompleto(oEmpleado);

            if (respuesta)
                return Ok(new { mensaje = "Empleado y usuario creados exitosamente" });
            else
                return StatusCode(500, new { mensaje = "Error al registrar empleado" });
        }

        /// <summary>
        /// Actualiza los datos de un empleado existente
        /// </summary>
        /// <param name="oEmpleado">Objeto JSON con los datos actualizados</param>
        /// <returns>Mensaje de éxito o error</returns>
        // PUT: api/Empleados - [FromBody] recibe JSON
        [HttpPut]
        public IActionResult Editar([FromBody] Empleado oEmpleado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool respuesta = _empleadoDatos.Editar(oEmpleado);

            if (respuesta)
                return Ok(new { mensaje = "Empleado actualizado exitosamente" });
            else
                return StatusCode(500, new { mensaje = "Error al actualizar empleado" });
        }

        /// <summary>
        /// Elimina un empleado por su ID
        /// </summary>
        /// <param name="id">ID del empleado a eliminar</param>
        /// <returns>Mensaje de éxito o error</returns>
        // DELETE: api/Empleados/5
        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            bool respuesta = _empleadoDatos.Eliminar(id);

            if (respuesta)
                return Ok(new { mensaje = "Empleado eliminado" });
            else
                return StatusCode(500, new { mensaje = "Error al eliminar empleado" });
        }


    }
}
