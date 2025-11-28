using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;

namespace Panaderia.Data
{
    public class CategoriaDatos
    {
        public List<Categoria> Listar()
        {
            var oLista = new List<Categoria>();
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_categoria_leer", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                                Nombre = dr["nombre"].ToString(),
                                Descripcion = dr["descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            return oLista;
        }
    }
}
