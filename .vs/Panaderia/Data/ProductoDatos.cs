using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Panaderia.Data
{
    public class ProductoDatos
    {
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


        public bool Guardar(Producto oProducto)
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

                        // CAMBIAR POR EL USUARIO DEL LOGIN
                        cmd.Parameters.AddWithValue("_usuario_que_registra", "Roberto (Admin)");

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

        public bool Editar(Producto oProducto)
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

                        // CAMBIAR POR EL USUARIO DEL LOGIN
                        cmd.Parameters.AddWithValue("_usuario_que_registra", "Roberto (Admin)");

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

        public bool Eliminar(int idProducto)
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

                        // CAMBIAR POR EL USUARIO DEL LOGIN
                        cmd.Parameters.AddWithValue("_usuario_que_elimina", "Roberto (Admin)");

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
