using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Panaderia.Data;
using Panaderia.Models;

namespace Panaderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        EmpleadoDatos _empleadoDatos = new EmpleadoDatos();

        // GET: api/Empleados
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _empleadoDatos.Listar();
            return Ok(lista);
        }

        // POST: api/Empleados
        // [FromBody] se recibe un JSON
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

        // PUT: api/Empleados
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
