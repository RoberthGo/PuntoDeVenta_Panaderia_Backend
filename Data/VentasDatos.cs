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

        public bool Guardar(Ventas oVenta)
        {
            bool respuesta;
            var cn = new ConexionDB();

            try
            {
                using (var conexion = cn.getConexion())
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("sp_venta_crear", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("_fecha", oVenta.Fecha);
                        cmd.Parameters.AddWithValue("_idEmpleado", oVenta.IdEmpleado);
                        cmd.Parameters.AddWithValue("_total", oVenta.Total);
                        
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
