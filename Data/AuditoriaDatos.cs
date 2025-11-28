using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;

namespace Panaderia.Data
{
    public class AuditoriaDatos
    {
        public List<AuditoriaProducto> Listar()
        {
            var oLista = new List<AuditoriaProducto>();
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_auditoria_leer", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new AuditoriaProducto()
                            {
                                IdAuditoria = Convert.ToInt32(dr["idAuditoria"]),
                                FechaHora = Convert.ToDateTime(dr["fechaHora"]),
                                Usuario = dr["usuario"].ToString(),
                                Accion = dr["accion"].ToString(),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                ValorAnterior = dr["valorAnterior"] != DBNull.Value ? dr["valorAnterior"].ToString() : "-",
                                ValorNuevo = dr["valorNuevo"] != DBNull.Value ? dr["valorNuevo"].ToString() : "-"
                            });
                        }
                    }
                }
            }
            return oLista;
        }
    }

}
