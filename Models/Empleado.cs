namespace Panaderia.Models
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }

        public string Nombre { get; set; }

        public string Telefono { get; set; }

        // En la BD es un ENUM ('Empleado', 'Administrador')
        public string Rol { get; set; }

        public decimal Salario { get; set; }

        public DateTime FechaIngreso { get; set; }
    }
}
