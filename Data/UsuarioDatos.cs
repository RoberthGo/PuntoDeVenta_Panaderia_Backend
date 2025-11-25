using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;

namespace Panaderia.Data
{
    public class UsuarioDatos
    {
        public Usuario Validar(UsuarioLogin oLogin)
        {
            Usuario objeto = null;
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_validar_usuario", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_usuario", oLogin.NombreUsuario);
                    cmd.Parameters.AddWithValue("_contrasena", oLogin.Clave);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            objeto = new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                                NombreCompleto = dr["nombreEmpleado"].ToString(),
                                Rol = dr["rol"].ToString()
                            };
                        }
                    }
                }
            }
            return objeto; 
        }

        public bool Registrar(UsuarioRegistrar oRegistro)
        {
            bool respuesta;
            var cn = new ConexionDB();

            try
            {
                using (var conexion = cn.getConexion())
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("sp_registrar_usuario", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("_idEmpleado", oRegistro.IdEmpleado);
                        cmd.Parameters.AddWithValue("_usuario", oRegistro.NombreUsuario);
                        cmd.Parameters.AddWithValue("_contrasena", oRegistro.Clave);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // El usuario ya existe o el ID de empleado es inválido
                respuesta = false;
            }
            return respuesta;
        }
    }
}
