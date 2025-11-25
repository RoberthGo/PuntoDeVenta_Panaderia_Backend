using Microsoft.AspNetCore.Mvc;
using Panaderia.Data;
using Panaderia.Models;
using System.IO;


namespace Panaderia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase 
    {
        ProductoDatos _productoDatos = new ProductoDatos();

        
        // GET: api/Productos
        // Devuelve el JSON con todos los productos
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = _productoDatos.Listar();
            return Ok(lista);
        }

        // POST: api/Productos
        // Recibe datos + imagen para crear uno nuevo
        [HttpPost]
        public IActionResult Guardar([FromForm] Producto oProducto, IFormFile imagenArchivo)
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

            bool respuesta = _productoDatos.Guardar(oProducto);

            if (respuesta)
                return Ok(new { mensaje = "Producto guardado exitosamente" });
            else
                return StatusCode(500, new { mensaje = "Error al guardar en base de datos" });
        }

        // PUT: api/Productos
        // Editar productos existentes
        [HttpPut]
        public IActionResult Editar([FromForm] Producto oProducto, IFormFile imagenArchivo)
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

            bool respuesta = _productoDatos.Editar(oProducto);

            if (respuesta)
                return Ok(new { mensaje = "Producto editado exitosamente" });
            else
                return StatusCode(500, new { mensaje = "Error al editar el producto" });
        }

        // DELETE: api/Productos/5
        // Elimina por ID
        [HttpDelete("{idProducto}")]
        public IActionResult Eliminar(int idProducto)
        {
            bool respuesta = _productoDatos.Eliminar(idProducto);

            if (respuesta)
                return Ok(new { mensaje = "Producto eliminado" });
            else
                return StatusCode(500, new { mensaje = "Error al eliminar" });
        }
    }
}


