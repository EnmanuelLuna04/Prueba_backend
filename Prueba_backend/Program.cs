using Bacnkend.Data;
using Bacnkend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar DbContext con SQLite en memoria
builder.Services.AddDbContext<BankingContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el servicio de transacciones
builder.Services.AddScoped<TransactionService>();

// Agregar controladores
builder.Services.AddControllers();

// Configurar Swagger para documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Usar Swagger UI para la documentación de la API
app.UseSwagger();
app.UseSwaggerUI();

// Autorización (si tienes configurado algún middleware de autenticación)
app.UseAuthorization();

// Mapeo de controladores
app.MapControllers();


app.Run();
