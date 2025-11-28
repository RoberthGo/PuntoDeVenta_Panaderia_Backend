using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;

namespace Panaderia.Data
{
    public class DetalleVentasDatos
    {
        public List<DetalleVentas> Listar()
        {
            var oLista = new List<DetalleVentas>();
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_detalle_venta_leer", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new DetalleVentas()
                            {
                                IdDetalle = Convert.ToInt32(dr["idDetalle"]),
                                IdVenta = Convert.ToInt32(dr["idVenta"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                Cantidad = Convert.ToInt32(dr["cantidad"]),
                                precioUnitario = Convert.ToDecimal(dr["precioUnitario"]),
                                subtotal = Convert.ToDecimal(dr["subtotal"])    
                            });
                        }
                    }
                }
            }
            return oLista;
        }

        public bool Guardar(DetalleVentas oDetalle)
        {
            bool respuesta;
            var cn = new ConexionDB();

            try
            {
                using (var conexion = cn.getConexion())
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("sp_detalle_venta_crear", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("_idVenta", oDetalle.IdVenta);
                        cmd.Parameters.AddWithValue("_idProducto", oDetalle.IdProducto);
                        cmd.Parameters.AddWithValue("_cantidad", oDetalle.Cantidad);
                        cmd.Parameters.AddWithValue("_precioUnitario", oDetalle.precioUnitario);
                        cmd.Parameters.AddWithValue("_subtotal", oDetalle.subtotal);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                respuesta = false;
            }
            return respuesta;
        }
    }
}
