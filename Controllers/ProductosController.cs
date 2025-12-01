using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;
using Panaderia.Models;
using System.IO;


namespace Panaderia.Controllers
{
    /// <summary>
    /// Controlador para la gestión de productos (CRUD con auditoría)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase 
    {
        ProductoDatos _productoDatos = new ProductoDatos();

        /// <summary>
        /// Obtiene la lista de todos los productos con su imagen en Base64
        /// </summary>
        /// <returns>Lista de productos en formato JSON</returns>
        // GET: api/Productos
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _productoDatos.Listar();
            return Ok(lista);
        }

        /// <summary>
        /// Crea un nuevo producto con imagen opcional
        /// </summary>
        /// <param name="oProducto">Datos del producto (form-data)</param>
        /// <param name="imagenArchivo">Archivo de imagen del producto</param>
        /// <param name="usuario">Usuario que realiza la acción (header para auditoría)</param>
        /// <returns>Mensaje de éxito o error</returns>
        // POST: api/Productos - [FromForm] recibe form-data con imagen
        [HttpPost]
        public IActionResult Guardar([FromForm] Producto oProducto, IFormFile imagenArchivo, [FromHeader] string usuario)
        {
            // Validamos si subió imagen
            if (imagenArchivo != null && imagenArchivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imagenArchivo.CopyTo(ms);
                    oProducto.Imagen = ms.ToArray();
                }
            }

            // Ignoramos validaciones de imagen vacía
            ModelState.Remove("Imagen");
            ModelState.Remove("ImagenBase64");

            // Si faltan datos, devolvemos HTTP 400 (Bad Request) y los errores
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool respuesta = _productoDatos.Guardar(oProducto, usuario);

            if (respuesta)
                return Ok(new { mensaje = "Producto guardado exitosamente" });
            else
                return StatusCode(500, new { mensaje = "Error al guardar en base de datos" });
        }

        /// <summary>
        /// Actualiza un producto existente. Si no se envía imagen, conserva la anterior
        /// </summary>
        /// <param name="oProducto">Datos actualizados del producto (form-data)</param>
        /// <param name="imagenArchivo">Nueva imagen (opcional)</param>
        /// <param name="usuario">Usuario que realiza la acción (header para auditoría)</param>
        /// <returns>Mensaje de éxito o error</returns>
        // PUT: api/Productos - [FromForm] recibe form-data
        [HttpPut]
        public IActionResult Editar([FromForm] Producto oProducto, IFormFile imagenArchivo, [FromHeader] string usuario)
        {
            // Lógica de recuperar imagen vieja si no suben una nueva
            if (imagenArchivo != null && imagenArchivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imagenArchivo.CopyTo(ms);
                    oProducto.Imagen = ms.ToArray();
                }
            }
            else
            {
                var productoOriginal = _productoDatos.Listar().Find(p => p.IdProducto == oProducto.IdProducto);
                if (productoOriginal != null)
                {
                    oProducto.Imagen = productoOriginal.Imagen;
                }
            }

            ModelState.Remove("Imagen");
            ModelState.Remove("ImagenBase64");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool respuesta = _productoDatos.Editar(oProducto, usuario);

            if (respuesta)
                return Ok(new { mensaje = "Producto editado exitosamente" });
            else
                return StatusCode(500, new { mensaje = "Error al editar el producto" });
        }

        /// <summary>
        /// Elimina un producto por su ID
        /// </summary>
        /// <param name="idProducto">ID del producto a eliminar</param>
        /// <param name="usuario">Usuario que realiza la acción (header para auditoría)</param>
        /// <returns>Mensaje de éxito o error</returns>
        // DELETE: api/Productos/5
        [HttpDelete("{idProducto}")]
        public IActionResult Eliminar(int idProducto, [FromHeader] string usuario)
        {
            bool respuesta = _productoDatos.Eliminar(idProducto, usuario);

            if (respuesta)
                return Ok(new { mensaje = "Producto eliminado" });
            else
                return StatusCode(500, new { mensaje = "Error al eliminar" });
        }
    }
}


