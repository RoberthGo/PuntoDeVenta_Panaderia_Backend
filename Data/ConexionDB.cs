using MySql.Data.MySqlClient;

namespace Panaderia.Data
{
    /// <summary>
    /// Clase para gestionar la conexión a la base de datos MySQL
    /// </summary>
    public class ConexionDB
    {
        private string connectionString = string.Empty;

        /// <summary>
        /// Constructor que lee la cadena de conexión desde appsettings.json
        /// </summary>
        public ConexionDB()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            connectionString = builder.GetSection("ConnectionStrings:CadenaSQL").Value;
        }

        /// <summary>
        /// Crea y retorna una nueva conexión MySQL
        /// </summary>
        /// <returns>Objeto MySqlConnection listo para abrir</returns>
        public MySqlConnection getConexion()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
