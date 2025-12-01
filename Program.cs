// ============================================
// API REST - Punto de Venta Panadería
// Backend desarrollado en ASP.NET Core 8
// Base de datos: MySQL
// ============================================

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Documentación interactiva de la API

var app = builder.Build();

// Habilitar Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers(); // Mapea los endpoints de los controladores

app.Run();
