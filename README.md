>Descripción
>Este proyecto es una API RESTful que simula un sistema bancario donde se pueden crear cuentas bancarias, registrar transacciones (depósitos, retiros), aplicar intereses y consultar el historial de transacciones. Se utiliza SQLite como base de datos para pruebas y almacenamiento persistente.

>El proyecto está basado en ASP.NET Core 8.0, y se implementa usando Inyección de Dependencias (DI) para la gestión de servicios como el registro de transacciones.

# Tecnologías utilizadas
* ASP.NET Core 8.0: Framework para construir la API.

* Entity Framework Core: ORM para interactuar con la base de datos.

* SQLite: Base de datos en memoria y persistente.

* xUnit: Framework de pruebas unitarias.

* Swagger: Herramienta para documentar y probar la API.

# Estructura del Proyecto
Prueba_backend/: Contiene la lógica principal de la aplicación, incluidos los modelos, el contexto de la base de datos y los controladores.

* Models/: Define las entidades del sistema como BankAccount, Client, y Transaction.

* Data/: Define el contexto de la base de datos BankingContext que interactúa con la base de datos SQLite.

* Controllers/: Define los controladores de la API para manejar solicitudes HTTP.

* Services/: Contiene los servicios como TransactionService, que gestionan las transacciones y otras lógicas de negocio.

* Bacnkend.Tests/: Contiene las pruebas unitarias para validar las funcionalidades.

* Se utilizan xUnit y SQLite en memoria para realizar las pruebas sin modificar la base de datos real.

# Requisitos previos
* .NET 8.0 SDK: Asegúrate de tener .NET 8.0 SDK instalado en tu máquina. Puedes descargarlo desde aquí.

* SQLite: El proyecto usa SQLite, pero no necesitas instalar nada adicional para pruebas ya que se utiliza en memoria.

# Configuración del Proyecto
1. Cadena de Conexión a SQLite
La base de datos SQLite está configurada en appsettings.json. La cadena de conexión en memoria se utiliza durante las pruebas, pero también puedes cambiarla para usar una base de datos persistente si es necesario.

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=banking.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}

2. Program.cs
El archivo Program.cs contiene la configuración de los servicios y middleware. Aquí es donde registramos el TransactionService y el BankingContext con SQLite.

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


# Cómo Ejecutar la Aplicación
1. Ejecutar la Aplicación
La aplicación estará disponible en http://localhost:5272 por defecto. Para ejecutar la aplicación, abre una terminal en la carpeta raíz del proyecto y ejecuta el siguiente comando:


dotnet run
Esto iniciará el servidor web y podrás acceder a la API en http://localhost:5272.

2. Acceder a Swagger (Documentación Interactiva)
Si Swagger está habilitado, podrás acceder a la documentación interactiva de la API y probar los diferentes endpoints directamente desde el navegador. Para acceder a Swagger, dirígete a la siguiente URL:

http://localhost:5272/swagger
En Swagger podrás ver todos los endpoints disponibles y probar las rutas de la API de manera sencilla, sin necesidad de herramientas adicionales como Postman.

Pruebas Unitarias
1. Ejecutar las Pruebas
Para ejecutar las pruebas unitarias y asegurarte de que todo funcione correctamente, navega al proyecto de pruebas Bacnkend.Tests y ejecuta el siguiente comando:


dotnet test
Este comando ejecutará todas las pruebas unitarias definidas en el proyecto Bacnkend.Tests y mostrará el resultado en la terminal o en Test Explorer (si usas Visual Studio).

2. Pruebas Implementadas
Se han implementado las siguientes pruebas unitarias para verificar el correcto funcionamiento de la API:

Crear una cuenta bancaria.

Registrar depósitos y retiros.

Aplicar intereses al saldo.

Consultar saldo y historial de transacciones.

Estas pruebas aseguran que el sistema funcione correctamente en diferentes escenarios, cubriendo las funcionalidades clave del sistema bancario.

Endpoints de la API
Si tienes Swagger habilitado, podrás ver y probar los siguientes endpoints:

GET /api/accounts: Obtiene una lista de todas las cuentas bancarias.

POST /api/accounts: Crea una nueva cuenta bancaria.

POST /api/transactions/deposit: Realiza un depósito en una cuenta bancaria especificada.

POST /api/transactions/withdraw: Realiza un retiro de una cuenta bancaria especificada.

POST /api/transactions/interest: Aplica intereses a una cuenta bancaria especificada.

## ¿Cómo Funciona el Servicio de Transacciones?
El TransactionService es responsable de gestionar las operaciones de depósito, retiro y aplicación de intereses. Este servicio se encuentra registrado en el contenedor de dependencias de ASP.NET Core y se inyecta automáticamente en los controladores que lo necesiten.

* Métodos disponibles en TransactionService:
DepositAsync: Registra una transacción de depósito en la cuenta bancaria y actualiza su saldo.

* WithdrawAsync: Registra una transacción de retiro en la cuenta bancaria, verificando que el saldo sea suficiente antes de realizar la operación.

* ApplyInterestAsync: Aplica un porcentaje de interés al saldo de la cuenta bancaria.

Este enfoque permite realizar las operaciones bancarias de manera centralizada y desacoplada de los controladores, lo que facilita la extensión de funcionalidades en el futuro.

## Configuración de Swagger
Se ha configurado Swagger para facilitar la interacción con la API y la prueba de los endpoints desde un navegador. Swagger genera automáticamente la documentación de la API y permite realizar pruebas interactivas sin la necesidad de herramientas externas.

ara acceder a Swagger y probar los endpoints de la API, simplemente abre http://localhost:5272/swagger en tu navegador.

# Resumen
Este proyecto proporciona una API RESTful para gestionar un sistema bancario, permitiendo:

* Crear cuentas bancarias.

* Realizar transacciones de depósito y retiro.

* Aplicar intereses a las cuentas.

* Consultar el saldo y el historial de transacciones.

### Tecnologías utilizadas:
* ASP.NET Core 8.0 para el backend.

* Entity Framework Core para la gestión de la base de datos.

* SQLite para el almacenamiento de datos.

* xUnit para las pruebas unitarias.

* Swagger para la documentación interactiva de la API.

Este sistema está diseñado para ser robusto y fácil de mantener, con pruebas unitarias que garantizan la calidad del código. Además, Swagger facilita la interacción con la API durante el desarrollo y la integración.

