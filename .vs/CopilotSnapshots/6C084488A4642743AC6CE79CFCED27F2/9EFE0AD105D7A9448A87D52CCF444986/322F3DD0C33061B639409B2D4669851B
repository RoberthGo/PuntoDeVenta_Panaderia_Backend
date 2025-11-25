using MySql.Data.MySqlClient;

namespace Panaderia.Data
{
    public class ConexionDB
    {
        private string connectionString = string.Empty;

        public ConexionDB()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            connectionString = builder.GetSection("ConnectionStrings:CadenaSQL").Value;
        }

        public MySqlConnection getConexion()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
