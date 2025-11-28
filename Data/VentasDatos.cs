using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;

namespace Panaderia.Data
{
    public class VentasDatos
    {
        public List<Ventas> Listar()
        {
            var oLista = new List<Ventas>();
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_ventas_leer", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Ventas()
                            {
                                IdEmpleado = Convert.ToInt32(dr["idEmpleado"]),
                                Fecha = Convert.ToDateTime(dr["fecha"]),
                                IdVenta = Convert.ToInt32(dr["idVenta"]),
                                Total = Convert.ToDecimal(dr["total"])
                            });
                        }
                    }
                }
            }
            return oLista;
        }

        public bool RegistrarVentaTransaccion(VentaRequest oVenta)
        {
            bool respuesta = false;
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();

                MySqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    int idVentaGenerada = 0;

                    // Venta
                    using (var cmd = new MySqlCommand("sp_venta_crear", conexion, transaccion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_idEmpleado", oVenta.IdEmpleado);

                        cmd.Parameters.Add("_idVentaGenerado", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        idVentaGenerada = Convert.ToInt32(cmd.Parameters["_idVentaGenerado"].Value);
                    }

                    // Insertar Detalles
                    foreach (var item in oVenta.Productos)
                    {
                        using (var cmd = new MySqlCommand("sp_detalle_venta_crear", conexion, transaccion))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            decimal subtotal = item.Cantidad * item.PrecioUnitario;

                            cmd.Parameters.AddWithValue("_idVenta", idVentaGenerada);
                            cmd.Parameters.AddWithValue("_idProducto", item.IdProducto);
                            cmd.Parameters.AddWithValue("_cantidad", item.Cantidad);
                            cmd.Parameters.AddWithValue("_precioUnitario", item.PrecioUnitario);
                            cmd.Parameters.AddWithValue("_subtotal", subtotal);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaccion.Commit();
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    respuesta = false;
                    Console.WriteLine("Error Transacción: " + ex.Message);
                }
            }
            return respuesta;
        }

        public List<ReporteVenta> ObtenerReporteRango(string fechaInicio, string fechaFin, int? idProducto)
        {
            var oLista = new List<ReporteVenta>();
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_reporte_ventas_rango", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_fechaInicio", DateTime.Parse(fechaInicio));
                    cmd.Parameters.AddWithValue("_fechaFin", DateTime.Parse(fechaFin));

                    // Enviamos el ID del producto o NULL si viene vacío/cero
                    cmd.Parameters.AddWithValue("_idProducto", (idProducto > 0) ? idProducto : DBNull.Value);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new ReporteVenta()
                            {
                                Clave = Convert.ToInt32(dr["Clave"]),
                                Nombre = dr["Nombre"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                UnidadesVendidas = Convert.ToInt32(dr["UnidadesVendidas"]),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"])
                            });
                        }
                    }
                }
            }
            return oLista;
        }

        // MÉTODO 2: REPORTE POR MES Y AÑO
        public List<ReporteVenta> ObtenerReporteMensual(int mes, int anio, int? idProducto)
        {
            var oLista = new List<ReporteVenta>();
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_reporte_ventas_mensual", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_mes", mes);
                    cmd.Parameters.AddWithValue("_anio", anio);

                    // Enviamos el ID del producto o NULL si viene vacío/cero
                    cmd.Parameters.AddWithValue("_idProducto", (idProducto > 0) ? idProducto : DBNull.Value);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new ReporteVenta()
                            {
                                Clave = Convert.ToInt32(dr["Clave"]),
                                Nombre = dr["Nombre"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                UnidadesVendidas = Convert.ToInt32(dr["UnidadesVendidas"]),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"])
                            });
                        }
                    }
                }
            }
            return oLista;
        }
    }
}
