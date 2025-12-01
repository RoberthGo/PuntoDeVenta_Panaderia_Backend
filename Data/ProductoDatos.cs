using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Panaderia.Data
{
    /// <summary>
    /// Clase para acceso a datos de productos (CRUD con auditoría)
    /// </summary>
    public class ProductoDatos
    {
        /// <summary>
        /// Obtiene todos los productos con su imagen en bytes
        /// </summary>
        /// <returns>Lista de productos</returns>
        public List<Producto> Listar()
        {
            var oLista = new List<Producto>();
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_producto_leer", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var prod = new Producto() 
                            {
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                Nombre = dr["nombre"].ToString(),
                                Descripcion = dr["descripcion"].ToString(),
                                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                                NombreCategoria = dr["categoria"].ToString(),
                                Precio = Convert.ToDecimal(dr["precio"]),
                                Stock = Convert.ToInt32(dr["stock"]),
                                ReorderLevel = Convert.ToInt32(dr["reorderLevel"]),
                                Costo = Convert.ToDecimal(dr["costo"])
                            };

                            if (dr["imagen"] != DBNull.Value)
                            {
                                prod.Imagen = (byte[])dr["imagen"];
                            }
                            else
                            {
                                prod.Imagen = null;
                            }

                            oLista.Add(prod);
                        }
                    }
                }
            }
            return oLista;
        }


        /// <summary>
        /// Inserta un nuevo producto y registra la acción en auditoría
        /// </summary>
        /// <param name="oProducto">Datos del producto</param>
        /// <param name="usuario">Usuario que realiza la acción (para auditoría)</param>
        /// <returns>True si se guardó correctamente</returns>
        public bool Guardar(Producto oProducto, string usuario)
        {
            bool respuesta;
            var cn = new ConexionDB();

            try
            {
                using (var conexion = cn.getConexion())
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("sp_producto_crear", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("_nombre", oProducto.Nombre);
                        cmd.Parameters.AddWithValue("_descripcion", oProducto.Descripcion);
                        cmd.Parameters.AddWithValue("_idCategoria", oProducto.IdCategoria);
                        cmd.Parameters.AddWithValue("_precio", oProducto.Precio);
                        cmd.Parameters.AddWithValue("_costo", oProducto.Costo);
                        cmd.Parameters.AddWithValue("_stock", oProducto.Stock);
                        cmd.Parameters.AddWithValue("_reorderLevel", oProducto.ReorderLevel);
                        cmd.Parameters.AddWithValue("_imagen", oProducto.Imagen ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("_usuario_que_registra", usuario);


                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }

        /// <summary>
        /// Actualiza un producto existente y registra la acción en auditoría
        /// </summary>
        /// <param name="oProducto">Producto con datos actualizados</param>
        /// <param name="usuario">Usuario que realiza la acción (para auditoría)</param>
        /// <returns>True si se actualizó correctamente</returns>
        public bool Editar(Producto oProducto, string usuario)
        {
            bool respuesta;
            var cn = new ConexionDB();

            try
            {
                using (var conexion = cn.getConexion())
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("sp_producto_actualizar", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("_id", oProducto.IdProducto);
                        cmd.Parameters.AddWithValue("_nombre", oProducto.Nombre);
                        cmd.Parameters.AddWithValue("_descripcion", oProducto.Descripcion);
                        cmd.Parameters.AddWithValue("_idCategoria", oProducto.IdCategoria);
                        cmd.Parameters.AddWithValue("_precio", oProducto.Precio);
                        cmd.Parameters.AddWithValue("_costo", oProducto.Costo);
                        cmd.Parameters.AddWithValue("_stock", oProducto.Stock);
                        cmd.Parameters.AddWithValue("_reorderLevel", oProducto.ReorderLevel);
                        cmd.Parameters.AddWithValue("_imagen", oProducto.Imagen ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("_usuario_que_registra", usuario);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                }
            }
            catch (Exception)
            {
                respuesta = false;
            }
            return respuesta;
        }

        /// <summary>
        /// Elimina un producto y registra la acción en auditoría
        /// </summary>
        /// <param name="idProducto">ID del producto a eliminar</param>
        /// <param name="usuario">Usuario que realiza la acción (para auditoría)</param>
        /// <returns>True si se eliminó correctamente</returns>
        public bool Eliminar(int idProducto, string usuario)
        {
            bool respuesta;
            var cn = new ConexionDB();

            try
            {
                using (var conexion = cn.getConexion())
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("sp_producto_eliminar", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_id", idProducto);
                        cmd.Parameters.AddWithValue("_usuario_que_elimina", usuario);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                }
            }
            catch (Exception)
            {
                respuesta = false;
            }
            return respuesta;
        }

    }
}
