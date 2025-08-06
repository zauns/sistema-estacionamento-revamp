using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ParkingSystem.Infrastructure.Data;
using ParkingSystem.Infrastructure.Services;
// Adicione os usings para os novos serviços e DTOs se eles estiverem em namespaces diferentes

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Parking System API", 
        Version = "v1",
        Description = "API para gerenciamento de sistema de estacionamento"
    });
});

// Configurar Entity Framework com SQLite
builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// *** NOVO: Registrar a camada de serviço ***
builder.Services.AddScoped<IParkingService, ParkingService>();

// Configurar CORS para desenvolvimento
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Criar/Migrar banco automaticamente no desenvolvimento
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();
    await context.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking System API V1");
        c.RoutePrefix = "swagger";
    });
    
    app.UseCors("Development");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
