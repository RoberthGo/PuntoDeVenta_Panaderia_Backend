using MySql.Data.MySqlClient;
using Panaderia.Models;
using System.Data;

namespace Panaderia.Data
{
    public class EmpleadoDatos
    {
        public List<Empleado> Listar()
        {
            var oLista = new List<Empleado>();
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();
                using (var cmd = new MySqlCommand("sp_empleado_leer", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Empleado()
                            {
                                IdEmpleado = Convert.ToInt32(dr["idEmpleado"]),
                                Nombre = dr["nombre"].ToString(),
                                Telefono = dr["telefono"].ToString(),
                                Rol = dr["rol"].ToString(),
                                Salario = Convert.ToDecimal(dr["salario"]),
                                // Validar si fechaIngreso no es nula
                                FechaIngreso = dr["fechaIngreso"] != DBNull.Value
                                               ? Convert.ToDateTime(dr["fechaIngreso"])
                                               : DateTime.MinValue
                            });
                        }
                    }
                }
            }
            return oLista;
        }

        public bool Guardar(Empleado oEmpleado)
        {
            bool respuesta;
            var cn = new ConexionDB();

            try
            {
                using (var conexion = cn.getConexion())
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("sp_empleado_crear", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("_nombre", oEmpleado.Nombre);
                        cmd.Parameters.AddWithValue("_telefono", oEmpleado.Telefono);
                        cmd.Parameters.AddWithValue("_rol", oEmpleado.Rol);
                        cmd.Parameters.AddWithValue("_salario", oEmpleado.Salario);

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

        public bool Editar(Empleado oEmpleado)
        {
            bool respuesta;
            var cn = new ConexionDB();

            try
            {
                using (var conexion = cn.getConexion())
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("sp_empleado_actualizar", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("_id", oEmpleado.IdEmpleado);
                        cmd.Parameters.AddWithValue("_nombre", oEmpleado.Nombre);
                        cmd.Parameters.AddWithValue("_telefono", oEmpleado.Telefono);
                        cmd.Parameters.AddWithValue("_rol", oEmpleado.Rol);
                        cmd.Parameters.AddWithValue("_salario", oEmpleado.Salario);

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

        public bool Eliminar(int idEmpleado)
        {
            bool respuesta;
            var cn = new ConexionDB();

            try
            {
                using (var conexion = cn.getConexion())
                {
                    conexion.Open();
                    using (var cmd = new MySqlCommand("sp_empleado_eliminar", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_id", idEmpleado);
                        
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


        public bool RegistrarCompleto(EmpleadoRegistro oRegistro)
        {
            bool respuesta = false;
            var cn = new ConexionDB();

            using (var conexion = cn.getConexion())
            {
                conexion.Open();

                MySqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    int idEmpleadoGenerado = 0;

                    using (var cmd = new MySqlCommand("sp_empleado_crear", conexion, transaccion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_nombre", oRegistro.Nombre);
                        cmd.Parameters.AddWithValue("_telefono", oRegistro.Telefono);
                        cmd.Parameters.AddWithValue("_rol", oRegistro.Rol);
                        cmd.Parameters.AddWithValue("_salario", oRegistro.Salario);

                        cmd.Parameters.Add("_idEmpleadoGenerado", MySqlDbType.Int32).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        idEmpleadoGenerado = Convert.ToInt32(cmd.Parameters["_idEmpleadoGenerado"].Value);
                    }

                    using (var cmd = new MySqlCommand("sp_registrar_usuario", conexion, transaccion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_idEmpleado", idEmpleadoGenerado);
                        cmd.Parameters.AddWithValue("_usuario", oRegistro.NombreUsuario);
                        cmd.Parameters.AddWithValue("_contrasena", oRegistro.Clave);

                        cmd.ExecuteNonQuery();
                    }

                    transaccion.Commit();
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    respuesta = false;
                    Console.WriteLine("Error registro empleado: " + ex.Message);
                }
            }
            return respuesta;
        }
    }
}
