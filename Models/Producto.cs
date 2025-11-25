namespace Panaderia.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public int IdCategoria { get; set; }

        public decimal Precio { get; set; } 

        public int Stock { get; set; }

        public int ReorderLevel { get; set; }

        public decimal Costo { get; set; }

        public byte[]? Imagen { get; set; }

        public string? NombreCategoria { get; set; }
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
