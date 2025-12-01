using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;

namespace Panaderia.Data
{
    /// <summary>
    /// Clase para acceso a datos de ventas y generación de reportes
    /// </summary>
    public class VentasDatos
    {
        /// <summary>
        /// Obtiene el historial de todas las ventas
        /// </summary>
        /// <returns>Lista de ventas con fecha, empleado y total</returns>
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

        /// <summary>
        /// Registra una venta con todos sus productos en una transacción atómica.
        /// Si falla algún detalle, se hace rollback completo.
        /// </summary>
        /// <param name="oVenta">Datos de la venta (empleado y lista de productos)</param>
        /// <returns>True si toda la transacción fue exitosa</returns>
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

        /// <summary>
        /// Genera reporte de ventas en un rango de fechas
        /// </summary>
        /// <param name="fechaInicio">Fecha inicio (YYYY-MM-DD)</param>
        /// <param name="fechaFin">Fecha fin (YYYY-MM-DD)</param>
        /// <param name="listaIds">IDs de productos a filtrar (null o vacío = todos)</param>
        /// <returns>Lista con totales vendidos por producto</returns>
        public List<ReporteVenta> ObtenerReporteRango(string fechaInicio, string fechaFin, List<int> listaIds)
        {
            var oListaFinal = new List<ReporteVenta>();
            var cn = new ConexionDB();

            // Si la lista viene vacía o nula, se envian TODOS los productos
            if (listaIds == null || listaIds.Count == 0)
            {
                return ObtenerReporteRangoIndividual(cn, fechaInicio, fechaFin, null);
            }

            // Si tiene IDs específicos, se agregan
            foreach (int id in listaIds)
            {
                var resultados = ObtenerReporteRangoIndividual(cn, fechaInicio, fechaFin, id);
                oListaFinal.AddRange(resultados);
            }

            return oListaFinal;
        }

        /// <summary>
        /// Método auxiliar que ejecuta el SP de reporte por rango para un producto específico o todos
        /// </summary>
        private List<ReporteVenta> ObtenerReporteRangoIndividual(ConexionDB cn, string fechaInicio, string fechaFin, int? idProducto)
        {
            var listaTemp = new List<ReporteVenta>();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_reporte_ventas_rango", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_fechaInicio", DateTime.Parse(fechaInicio));
                    cmd.Parameters.AddWithValue("_fechaFin", DateTime.Parse(fechaFin));

                    cmd.Parameters.AddWithValue("_idProducto", (idProducto > 0) ? idProducto : DBNull.Value);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaTemp.Add(new ReporteVenta()
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
            return listaTemp;
        }

        /// <summary>
        /// Genera reporte de ventas de un mes específico
        /// </summary>
        /// <param name="mes">Número del mes (1-12)</param>
        /// <param name="anio">Año (ej: 2025)</param>
        /// <param name="listaIds">IDs de productos a filtrar (null o vacío = todos)</param>
        /// <returns>Lista con totales vendidos por producto en el mes</returns>
        public List<ReporteVenta> ObtenerReporteMensual(int mes, int anio, List<int> listaIds)
        {
            var oListaFinal = new List<ReporteVenta>();
            var cn = new ConexionDB();

            if (listaIds == null || listaIds.Count == 0)
            {
                return ObtenerReporteMensualIndividual(cn, mes, anio, null);
            }

            foreach (int id in listaIds)
            {
                var resultados = ObtenerReporteMensualIndividual(cn, mes, anio, id);
                oListaFinal.AddRange(resultados);
            }

            return oListaFinal;
        }

        /// <summary>
        /// Método auxiliar que ejecuta el SP de reporte mensual para un producto específico o todos
        /// </summary>
        private List<ReporteVenta> ObtenerReporteMensualIndividual(ConexionDB cn, int mes, int anio, int? idProducto)
        {
            var listaTemp = new List<ReporteVenta>();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_reporte_ventas_mensual", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_mes", mes);
                    cmd.Parameters.AddWithValue("_anio", anio);
                    cmd.Parameters.AddWithValue("_idProducto", (idProducto > 0) ? idProducto : DBNull.Value);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaTemp.Add(new ReporteVenta()
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
            return listaTemp;
        }

        /// <summary>
        /// Obtiene los productos vendidos en una venta específica (detalle del ticket)
        /// </summary>
        /// <param name="idVenta">ID de la venta</param>
        /// <returns>Lista de productos con cantidad, precio y subtotal</returns>
        public List<DetalleVentas> ObtenerDetalles(int idVenta)
        {
            var oLista = new List<DetalleVentas>();
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_detalle_venta_obtener", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_idVenta", idVenta);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new DetalleVentas()
                            {
                                IdDetalle = Convert.ToInt32(dr["idDetalle"]),
                                IdVenta = Convert.ToInt32(dr["idVenta"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                Producto = dr["Producto"].ToString(), 
                                Cantidad = Convert.ToInt32(dr["cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(dr["precioUnitario"]),
                                Subtotal = Convert.ToDecimal(dr["subtotal"])
                            });
                        }
                    }
                }
            }
            return oLista;
        }

    }
}
