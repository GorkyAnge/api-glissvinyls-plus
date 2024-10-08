using glissvinyls_plus.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar el servicio CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Permite solicitudes desde tu frontend
              .AllowAnyHeader() // Permite cualquier encabezado
              .AllowAnyMethod() // Permite cualquier método HTTP (GET, POST, PUT, DELETE, etc.)
              .AllowCredentials(); // Permitir el envío de cookies
    });
});

// Obtener las configuraciones de JWT desde appsettings.json
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];

// Configuración de JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Leer el token desde la cookie "jwtToken"
                context.Token = context.Request.Cookies["jwtToken"];
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                // Manejar la autenticación fallida (opcional)
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Crear variable para la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Configurar Swagger para desarrollo
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS para solicitudes desde el frontend en localhost:3000
app.UseCors("AllowFrontendLocalhost");

app.UseAuthentication(); // Habilitar la autenticación JWT

app.UseAuthorization();

app.MapControllers();

app.Run();
