using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;

namespace Panaderia.Data
{
    /// <summary>
    /// Clase para acceso a datos de usuarios (autenticación y registro)
    /// </summary>
    public class UsuarioDatos
    {
        /// <summary>
        /// Valida las credenciales del usuario contra la base de datos
        /// </summary>
        /// <param name="oLogin">Credenciales (usuario y contraseña)</param>
        /// <returns>Objeto Usuario si las credenciales son válidas, null si no</returns>
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

        /// <summary>
        /// Registra un nuevo usuario asociado a un empleado
        /// </summary>
        /// <param name="oRegistro">Datos del usuario (IdEmpleado, usuario, clave)</param>
        /// <returns>True si se registró correctamente, False si el usuario ya existe</returns>
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
