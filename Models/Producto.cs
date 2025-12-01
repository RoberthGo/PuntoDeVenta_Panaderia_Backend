namespace Panaderia.Models
{
    /// <summary>
    /// Modelo que representa un producto de la panadería
    /// </summary>
    public class Producto
    {
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        /// <summary>Cantidad mínima para alertar reabastecimiento</summary>
        public int ReorderLevel { get; set; }
        public decimal Costo { get; set; }
        /// <summary>Imagen almacenada como arreglo de bytes</summary>
        public byte[]? Imagen { get; set; }
        /// <summary>Nombre de la categoría (obtenido del JOIN en BD)</summary>
        public string? NombreCategoria { get; set; }

        /// <summary>
        /// Propiedad calculada de solo lectura que convierte la imagen (bytes) a formato Base64
        /// para ser usada directamente en el atributo src de una etiqueta img en el frontend
        /// </summary>
        public string? ImagenBase64
        {
            get
            {
                if (Imagen != null && Imagen.Length > 0)
                {
                    string base64 = Convert.ToBase64String(Imagen);
                    return $"data:image/jpeg;base64,{base64}";
                }
                return null;
            }
        }
    }
}
